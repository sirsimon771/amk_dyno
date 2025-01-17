//////////////////////////////////////////////////////////////////////////
// HMS Technology Center Ravensburg GmbH
//////////////////////////////////////////////////////////////////////////
/**
  Demo application for the IXXAT VCI .NET-API.

  @file "CanFdConNet.cs"

  @note 
    This demo demonstrates the following VCI features
    - adapter selection
    - controller initialization 
    - creation of a message channel
    - transmission / reception of CAN messages
*/
//////////////////////////////////////////////////////////////////////////
// Copyright (C) 2016-2017
// HMS Technology Center Ravensburg GmbH, all rights reserved
//////////////////////////////////////////////////////////////////////////

/*****************************************************************************
 * used namespaces
 ****************************************************************************/
using System;
using System.Text;
using System.Collections;
using System.Threading;
using Ixxat.Vci4;
using Ixxat.Vci4.Bal;
using Ixxat.Vci4.Bal.Can;


/*****************************************************************************
 * namespace CanConNet
 ****************************************************************************/
namespace CanFdConNet
{
  //##########################################################################
  /// <summary>
  ///   This class provides the entry point for the IXXAT VCI .NET 2.0 API
  ///   demo application. 
  /// </summary>
  //##########################################################################
  class CanConNet
  {
    #region Member variables

    /// <summary>
    ///   Reference to the used VCI device.
    /// </summary>
    static IVciDevice mDevice;

    /// <summary>
    ///   Reference to the CAN controller.
    /// </summary>
    static ICanControl2 mCanCtl;

    /// <summary>
    ///   Reference to the CAN message communication channel.
    /// </summary>
    static ICanChannel2 mCanChn;

    /// <summary>
    ///   Reference to the CAN message scheduler.
    /// </summary>
    static ICanScheduler2 mCanSched;

    /// <summary>
    ///   Reference to the message writer of the CAN message channel.
    /// </summary>
    static ICanMessageWriter mWriter;

    /// <summary>
    ///   Reference to the message reader of the CAN message channel.
    /// </summary>
    static ICanMessageReader mReader;

    /// <summary>
    ///   Thread that handles the message reception.
    /// </summary>
    static Thread rxThread;

    /// <summary>
    ///   Quit flag for the receive thread.
    /// </summary>
    static long mMustQuit = 0;

    /// <summary>
    ///   Event that's set if at least one message was received.
    /// </summary>
    static AutoResetEvent mRxEvent;

    #endregion

    #region Application entry point

    //************************************************************************
    /// <summary>
    ///   The entry point of this console application.
    /// </summary>
    //************************************************************************
    static void Main(string[] args)
    {
      Console.WriteLine(" >>>> VCI - .NET 2.0 - API Example V1.1 <<<<");
      Console.WriteLine(" initializes the CAN-FD with 500 kBaud in arbitration");
      Console.WriteLine(" and 2000 kBaud in data phase");
      Console.WriteLine(" key 'c' starts/stops a cyclic message object with id 200H");
      Console.WriteLine(" key 't' sends a message with id 100H");
      Console.WriteLine(" shows all received messages");
      Console.WriteLine(" Quit the application with ESC\n");

      Console.Write(" Select Adapter...\n");
      SelectDevice();
      Console.WriteLine(" Select Adapter.......... OK !\n");

      Console.Write(" Initialize CAN FD...\n");

      if (!InitSocket(0))
      {
        Console.WriteLine(" Initialize CAN FD............ FAILED !\n");
      }
      else
      {
        Console.WriteLine(" Initialize CAN FD............ OK !\n");

        //
        // start the receive thread
        //
        rxThread = new Thread(new ThreadStart(ReceiveThreadFunc));
        rxThread.Start();

        //
        // start a cyclic object
        //
        ICanCyclicTXMsg2 cyclicMsg = mCanSched.AddMessage();

        cyclicMsg.Identifier = 200;
        cyclicMsg.CycleTicks = 100;
        cyclicMsg.DataLength = 32;
        cyclicMsg.SelfReceptionRequest = true;
        cyclicMsg.ExtendedDataLength = true;
        cyclicMsg.FastDataRate = true;

        for (Byte i = 0; i < cyclicMsg.DataLength; i++)
        {
          cyclicMsg[i] = i;
        }

        //
        // wait for keyboard hit transmit  CAN-Messages cyclically
        //
        ConsoleKeyInfo cki = new ConsoleKeyInfo();

        Console.WriteLine(" Press T to transmit single message.");
        Console.WriteLine(" Press C to start/stop cyclic message.");
        Console.WriteLine(" Press ESC to exit.");
        do
        {
          while (!Console.KeyAvailable)
          {
            Thread.Sleep(10);
          }
          cki = Console.ReadKey(true);
          if (cki.Key == ConsoleKey.T)
          {
            TransmitData();
          }
          else if (cki.Key == ConsoleKey.C)
          {
            if (cyclicMsg.Status != CanCyclicTXStatus.Busy)
            {
              cyclicMsg.Start(0);
            }
            else
            {
              cyclicMsg.Stop();
            }
          }
        } while (cki.Key != ConsoleKey.Escape);

        //
        // stop cyclic message
        //
        cyclicMsg.Stop();

        //
        // tell receive thread to quit
        //
        Interlocked.Exchange(ref mMustQuit, 1);

        //
        // Wait for termination of receive thread
        //
        rxThread.Join();
      }

      Console.Write("\n Free VCI - Resources...\n");
      FinalizeApp();
      Console.WriteLine(" Free VCI - Resources........ OK !\n");

      Console.Write(" Done");
      Console.ReadLine();
    }

    #endregion

    #region Device selection

    //************************************************************************
    /// <summary>
    ///   Selects the first CAN adapter.
    /// </summary>
    //************************************************************************
    static void SelectDevice()
    {
      IVciDeviceManager deviceManager = null;
      IVciDeviceList    deviceList    = null;
      IEnumerator       deviceEnum    = null;

      try
      {
        //
        // Get device manager from VCI server
        //
        deviceManager = VciServer.Instance().DeviceManager;

        //
        // Get the list of installed VCI devices
        //
        deviceList = deviceManager.GetDeviceList();

        //
        // Get enumerator for the list of devices
        //
        deviceEnum = deviceList.GetEnumerator();

        //
        // Get first device
        //
        deviceEnum.MoveNext();
        mDevice = deviceEnum.Current as IVciDevice;

        //
        // print bus type and controller type of first controller
        //
        IVciCtrlInfo info = mDevice.Equipment[0];
        Console.Write(" BusType    : {0}\n", info.BusType);
        Console.Write(" CtrlType   : {0}\n", info.ControllerType);

        // show the device name and serial number
        object serialNumberGuid = mDevice.UniqueHardwareId;
        string serialNumberText = GetSerialNumberText(ref serialNumberGuid);
        Console.Write(" Interface    : " + mDevice.Description + "\n");
        Console.Write(" Serial number: " + serialNumberText + "\n");
      }
      catch (Exception exc)
      {
        Console.WriteLine("Error: " + exc.Message);
      }
      finally
      {
        //
        // Dispose device manager ; it's no longer needed.
        //
        DisposeVciObject(deviceManager);

        //
        // Dispose device list ; it's no longer needed.
        //
        DisposeVciObject(deviceList);

        //
        // Dispose device list ; it's no longer needed.
        //
        DisposeVciObject(deviceEnum);
      }
    }

    #endregion

    #region Opening socket

    //************************************************************************
    /// <summary>
    ///   Opens the specified socket, creates a message channel, initializes
    ///   and starts the CAN controller.
    /// </summary>
    /// <param name="canNo">
    ///   Number of the CAN controller to open.
    /// </param>
    /// <returns>
    ///   A value indicating if the socket initialization succeeded or failed.
    /// </returns>
    //************************************************************************
    static bool InitSocket(Byte canNo)
    {
      IBalObject bal = null;
      bool succeeded = false;

      try
      {
        //
        // Open bus access layer
        //
        bal = mDevice.OpenBusAccessLayer();

        //
        // Open a message channel for the CAN controller
        //
        mCanChn = bal.OpenSocket(canNo, typeof(ICanChannel2)) as ICanChannel2;

        //
        // Open the scheduler of the CAN controller
        //
        mCanSched = bal.OpenSocket(canNo, typeof(ICanScheduler2)) as ICanScheduler2;

        // Initialize the message channel
        mCanChn.Initialize(1024, 128, 100, CanFilterModes.Pass, false);

        // Get a message reader object
        mReader = mCanChn.GetMessageReader();

        // Initialize message reader
        mReader.Threshold = 1;

        // Create and assign the event that's set if at least one message
        // was received.
        mRxEvent = new AutoResetEvent(false);
        mReader.AssignEvent(mRxEvent);

        // Get a message wrtier object
        mWriter = mCanChn.GetMessageWriter();

        // Initialize message writer
        mWriter.Threshold = 1;

        // Activate the message channel
        mCanChn.Activate();


        //
        // Open the CAN controller
        //
        mCanCtl = bal.OpenSocket(canNo, typeof(ICanControl2)) as ICanControl2;

        // Initialize the CAN controller
        // set the arbitration bitrate to 500kBit/s
        //  (NonRaw) bitrate  500000, TSeg1: 6400, TSeg2: 1600, SJW:  1600, SSPoffset/TDO  not used
        // set the fast bitrate to 2000kBit/s
        //  (NonRaw) bitrate 2000000, TSeg1: 6400, TSeg2:  400, SJW:   400, SSPoffset/TDO  1600 ( == 80% )
        mCanCtl.InitLine(CanOperatingModes.Standard | 
          CanOperatingModes.Extended | 
          CanOperatingModes.ErrFrame, 
          CanExtendedOperatingModes.ExtendedDataLength | 
          CanExtendedOperatingModes.FastDataRate,
          CanFilterModes.Pass,
          2048,
          CanFilterModes.Pass,
          2048,
          CanBitrate2.CANFD500KBit, 
          CanBitrate2.CANFD2000KBit);

        //
        // print line status
        //
        Console.WriteLine(" LineStatus: {0}", mCanCtl.LineStatus);

        // Start the CAN controller
        mCanCtl.StartLine();

        succeeded = true;
      }
      catch (Exception exc)
      {
        Console.WriteLine("Error: Initializing socket failed : " + exc.Message);
        succeeded = false;
      }
      finally
      {
        //
        // Dispose bus access layer
        //
        DisposeVciObject(bal);
      }

      return succeeded;
    }

    #endregion

    #region Message transmission

    /// <summary>
    ///   Transmits a CAN-FD message with ID 0x100.
    /// </summary>
    static void TransmitData()
    {
      IMessageFactory factory = VciServer.Instance().MsgFactory;
      ICanMessage2 canMsg = (ICanMessage2)factory.CreateMsg(typeof(ICanMessage2));

      canMsg.TimeStamp  = 0;
      canMsg.Identifier = 0x100;
      canMsg.FrameType  = CanMsgFrameType.Data;
      canMsg.DataLength = 64;
      canMsg.SelfReceptionRequest = true;  // show this message in the console window
      canMsg.FastDataRate = true;
      canMsg.ExtendedDataLength = true;

      for (Byte i = 0; i < canMsg.DataLength; i++)
      {
        canMsg[i] = i;
      }

      // Write the CAN message into the transmit FIFO
      mWriter.SendMessage(canMsg);
    }

    #endregion

    #region Message reception

    //************************************************************************
    /// <summary>
    /// Print a CAN message
    /// </summary>
    /// <param name="canMessage"></param>
    //************************************************************************
    static void PrintMessage(ICanMessage2 canMessage)
    {
      switch (canMessage.FrameType)
      {
        //
        // show data frames
        //
        case CanMsgFrameType.Data:
          {
            if (!canMessage.RemoteTransmissionRequest)
            {
              Console.Write("\nTime: {0,10}  ID: {1,3:X}  DLC: {2,1}  Data:",
                            canMessage.TimeStamp,
                            canMessage.Identifier,
                            canMessage.DataLength);

              for (int index = 0; index < canMessage.DataLength; index++)
              {
                Console.Write(" {0,2:X}", canMessage[index]);
              }
            }
            else
            {
              Console.Write("\nTime: {0,10}  ID: {1,3:X}  DLC: {2,1}  Remote Frame",
                            canMessage.TimeStamp,
                            canMessage.Identifier,
                            canMessage.DataLength);
            }
            break;
          }

        //
        // show informational frames
        //
        case CanMsgFrameType.Info:
          {
            switch ((CanMsgInfoValue)canMessage[0])
            {
              case CanMsgInfoValue.Start:
                Console.Write("\nCAN started...");
                break;
              case CanMsgInfoValue.Stop:
                Console.Write("\nCAN stopped...");
                break;
              case CanMsgInfoValue.Reset:
                Console.Write("\nCAN reseted...");
                break;
            }
            break;
          }

        //
        // show error frames
        //
        case CanMsgFrameType.Error:
          {
            switch ((CanMsgError)canMessage[0])
            {
              case CanMsgError.Stuff: 
                Console.Write("\nstuff error...");
                break;
              case CanMsgError.Form:
                Console.Write("\nform error...");
                break;
              case CanMsgError.Acknowledge:
                Console.Write("\nacknowledgment error...");
                break;
              case CanMsgError.Bit:
                Console.Write("\nbit error...");
                break;
              case CanMsgError.Fdb:
                Console.Write("\nfast data bit error...");
                break;
              case CanMsgError.Crc:
                Console.Write("\nCRC error...");
                break;
              case CanMsgError.Dlc:
                Console.Write("\nData length error...");
                break;
              case CanMsgError.Other:
                Console.Write("\nother error...");
                break;
            }
            break;
          }
      }
    }

    //************************************************************************
    /// <summary>
    /// Demonstrate reading messages via MsgReader::ReadMessages() function
    /// </summary>
    //************************************************************************
    static void ReadMultipleMsgsViaReadMessages()
    {
      ICanMessage2[] msgArray;

      do
      {
        // Wait 100 msec for a message reception
        if (mRxEvent.WaitOne(100, false))
        {
          if (mReader.ReadMessages(out msgArray) > 0)
          {
            foreach (ICanMessage2 entry in msgArray)
            {
              PrintMessage(entry);
            }
          }
        }
      } while (0 == mMustQuit);
    }

    //************************************************************************
    /// <summary>
    /// Demonstrate reading messages via MsgReader::ReadMessage() function
    /// </summary>
    //************************************************************************
    static void ReadMsgsViaReadMessage()
    {
      ICanMessage2 canMessage;

      do
      {
        // Wait 100 msec for a message reception
        if (mRxEvent.WaitOne(100, false))
        {
          // read a CAN message from the receive FIFO
          while (mReader.ReadMessage(out canMessage))
          {
            PrintMessage(canMessage);
          }
        }
      } while (0 == mMustQuit);
    }

    //************************************************************************
    /// <summary>
    ///   This method is the works as receive thread.
    /// </summary>
    //************************************************************************
    static void ReceiveThreadFunc()
    {
      ReadMsgsViaReadMessage();
      //
      // alternative: use ReadMultipleMsgsViaReadMessages();
      //
    }

    #endregion

    #region Utility methods

    /// <summary>
    /// Returns the UniqueHardwareID GUID number as string which
    /// shows the serial number.
    /// Note: This function will be obsolete in later version of the VCI.
    /// Until VCI Version 3.1.4.1784 there is a bug in the .NET API which
    /// returns always the GUID of the interface. In later versions there
    /// the serial number itself will be returned by the UniqueHardwareID property.
    /// </summary>
    /// <param name="serialNumberGuid">Data read from the VCI.</param>
    /// <returns>The GUID as string or if possible the  serial number as string.</returns>
    static string GetSerialNumberText(ref object serialNumberGuid)
    {
      string resultText;

      // check if the object is really a GUID type
      if (serialNumberGuid.GetType() == typeof(System.Guid))
      {
        // convert the object type to a GUID
        System.Guid tempGuid = (System.Guid)serialNumberGuid;

        // copy the data into a byte array
        byte[] byteArray = tempGuid.ToByteArray();

        // serial numbers starts always with "HW"
        if (((char)byteArray[0] == 'H') && ((char)byteArray[1] == 'W'))
        {
          // run a loop and add the byte data as char to the result string
          resultText = "";
          int i = 0;
          while (true)
          {
            // the string stops with a zero
            if (byteArray[i] != 0)
              resultText += (char)byteArray[i];
            else
              break;
            i++;

            // stop also when all bytes are converted to the string
            // but this should never happen
            if (i == byteArray.Length)
              break;
          }
        }
        else
        {
          // if the data did not start with "HW" convert only the GUID to a string
          resultText = serialNumberGuid.ToString();
        }
      }
      else
      {
        // if the data is not a GUID convert it to a string
        string tempString = (string) (string) serialNumberGuid;
        resultText = "";
        for (int i=0; i < tempString.Length; i++)
        {
          if (tempString[i] != 0)
            resultText += tempString[i];
          else
            break;
        }
      }

      return resultText;
    }


    //************************************************************************
    /// <summary>
    ///   Finalizes the application 
    /// </summary>
    //************************************************************************
    static void FinalizeApp()
    {
      //
      // Dispose all hold VCI objects.
      //

      // Dispose message reader
      DisposeVciObject(mReader);

      // Dispose message writer 
      DisposeVciObject(mWriter);

      // Dispose CAN channel
      DisposeVciObject(mCanChn);

      // Dispose CAN controller
      DisposeVciObject(mCanCtl);

      // Dispose VCI device
      DisposeVciObject(mDevice);
    }


    //************************************************************************
    /// <summary>
    ///   This method tries to dispose the specified object.
    /// </summary>
    /// <param name="obj">
    ///   Reference to the object to be disposed.
    /// </param>
    /// <remarks>
    ///   The VCI interfaces provide access to native driver resources. 
    ///   Because the .NET garbage collector is only designed to manage memory, 
    ///   but not native OS and driver resources the application itself is 
    ///   responsible to release these resources via calling 
    ///   IDisposable.Dispose() for the obects obtained from the VCI API 
    ///   when these are no longer needed. 
    ///   Otherwise native memory and resource leaks may occure.  
    /// </remarks>
    //************************************************************************
    static void DisposeVciObject(object obj)
    {
      if (null != obj)
      {
        IDisposable dispose = obj as IDisposable;
        if (null != dispose)
        {
          dispose.Dispose();
          obj = null;
        }
      }
    }

    #endregion
  }
}
