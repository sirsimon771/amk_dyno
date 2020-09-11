'//////////////////////////////////////////////////////////////////////////
'// HMS Technology Center Ravensburg GmbH
'//////////////////////////////////////////////////////////////////////////
'
' Demonstration source code how to use the VCI with Visual Baisc .NET
'
' Features:
'
' - Includes a listcontrol to show the available interfaces
'   A own thread wait for a event that some interface has removed or added
'   (this happens typically with USB interfaces)
' - Shows how to read the unique hardware ID of the device
'   and show it as string on a label after selecting a device. A bug in
'   the function "SelectDevice" removed, MoveNext was called 2 times.
' - Shows how to convert the uniques hardware ID to a 
'   human readable serial number, see function "GetSerialNumber"
' - Use ASCII for encoding the serial number.
' - Shows error frames. 
' - Show the version number of the assembly.
' - Shoes how to set an acceptance filter to bit 0.
' - Implements a baud rate list box.
' 
'//////////////////////////////////////////////////////////////////////////
'// Copyright (C) 2016-2018
'// HMS Technology Center Ravensburg GmbH, all rights reserved
'//////////////////////////////////////////////////////////////////////////
'
Imports System
Imports System.Threading

Public Class FormVCIV4Demo

  Private mDevice As Ixxat.Vci4.IVciDevice = Nothing
  Private mCanChn As Ixxat.Vci4.Bal.Can.ICanChannel = Nothing
  Private mReader As Ixxat.Vci4.Bal.Can.ICanMessageReader = Nothing
  Private mWriter As Ixxat.Vci4.Bal.Can.ICanMessageWriter = Nothing
  Private mCanCtl As Ixxat.Vci4.Bal.Can.ICanControl = Nothing

  Private mRxEvent As System.Threading.AutoResetEvent = Nothing
  Private rxThread As System.Threading.Thread = Nothing

  Private deviceManager As Ixxat.Vci4.IVciDeviceManager = Nothing
  Private deviceList As Ixxat.Vci4.IVciDeviceList = Nothing
  Private deviceEnum As IEnumerator = Nothing

  Private changeEvent As New AutoResetEvent(True)
  Private interfaceChangeThread As System.Threading.Thread = Nothing


  ' This delegate enables asynchronous calls for setting
  ' the text property on a TextBox control.
  Delegate Sub SetTextCallback(ByVal [text] As String)

  ' Here is the tread safe call for the interface list box
  Delegate Sub FillListBoxWithInterFacesCallBack()


  Private Sub CloseAll()
    If mCanCtl IsNot Nothing Then
      ' stop the CAN controller
      Try
        mCanCtl.StopLine()
      Catch ex As Exception
        ' the can control has leave the scope,
        ' e.g. a plugable interface which was removed
      End Try
    End If

    TimerGetStatus.Enabled = False

    If rxThread IsNot Nothing Then
      '
      ' tell receive thread to quit
      '
      rxThread.Abort()

      '
      ' Wait for termination of receive thread
      '
      rxThread.Join()

      rxThread = Nothing
    End If

    If interfaceChangeThread IsNot Nothing Then
      '
      ' tell interface change thread to quit
      '
      interfaceChangeThread.Abort()

      '
      ' Wait for termination of interface change thread
      '
      interfaceChangeThread.Join()

      interfaceChangeThread = Nothing
    End If

    ' dispose all open objects including the vci object itself
    CloseVciObjects(True)
  End Sub


  Private Sub ButtonClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonClose.Click

    CloseAll()

    Close()
  End Sub

  Private Sub ButtonInit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonInit.Click

    Dim CurListViewItem As ListViewItem
    CurListViewItem = ListViewAvailInterfaces.SelectedItems(0)

    TimerGetStatus.Enabled = False

    CloseCurrentExistingController()

    If CurListViewItem IsNot Nothing Then


      SelectDevice(CurListViewItem.Tag)
      InitSocket(0)

      '
      ' start the receive thread
      '
      rxThread = New System.Threading.Thread(New System.Threading.ThreadStart(AddressOf ReceiveThreadFunc))
      rxThread.Start()


      ButtonInit.Enabled = False
      ButtonTransmitData.Enabled = True
      TimerGetStatus.Enabled = True

      '      TimerSendData.Enabled = True
    End If  'If CurListViewItem
  End Sub

  Private Sub SelectDevice(ByVal DeviceEnumNumber As Long)
    Dim mTempDevice As Ixxat.Vci4.IVciDevice = Nothing
    Dim deviceHardwareID As Object
    Try
      deviceEnum = deviceList.GetEnumerator()
      deviceEnum.Reset()

      Do While deviceEnum.MoveNext() = True

        mTempDevice = deviceEnum.Current

        If mTempDevice.VciObjectId = DeviceEnumNumber Then
          mDevice = mTempDevice

          ' a sample how to read the unique hardware ID from the device
          deviceHardwareID = mTempDevice.UniqueHardwareId

          ' if the UniqueHardwareId returns a GUID the try to convert it
          ' to a IXXAT like serial number HWxxxxxx
          If TypeOf deviceHardwareID Is System.Guid Then
            LblHWID.Text = GetSerialNumber(deviceHardwareID)
          Else
            LblHWID.Text = deviceHardwareID.ToString()
          End If

        End If

      Loop

    Catch ex As Exception
      ' todo show the exception
    End Try

  End Sub


  Private Sub InitSocket(ByVal canNo As Byte)
    Dim bal As Ixxat.Vci4.Bal.IBalObject

    Dim balType As Type
    balType = GetType(Ixxat.Vci4.Bal.Can.ICanChannel)


    Try
      bal = mDevice.OpenBusAccessLayer()

      mCanChn = bal.OpenSocket(canNo, balType)

      ' Initialize the message channel
      mCanChn.Initialize(1024, 128, False)

      ' Get a message reader object
      mReader = mCanChn.GetMessageReader()

      ' Initialize message reader
      mReader.Threshold = 1

      ' Create and assign the event that's set if at least one message
      ' was received.
      mRxEvent = New System.Threading.AutoResetEvent(False)
      mReader.AssignEvent(mRxEvent)

      ' Get a message writer object
      mWriter = mCanChn.GetMessageWriter()

      ' Initialize message writer
      mWriter.Threshold = 1

      ' Activate the message channel
      mCanChn.Activate()


      ' Open the CAN controller
      Dim canCtrlType As Type
      canCtrlType = GetType(Ixxat.Vci4.Bal.Can.ICanControl)
      mCanCtl = bal.OpenSocket(canNo, canCtrlType)

      ' Initialize the CAN controller
      Dim operatingMode As Byte
      operatingMode = Ixxat.Vci4.Bal.Can.CanOperatingModes.Standard Or Ixxat.Vci4.Bal.Can.CanOperatingModes.Extended Or Ixxat.Vci4.Bal.Can.CanOperatingModes.ErrFrame


      Dim bitRate As Ixxat.Vci4.Bal.Can.CanBitrate
      bitRate = GetSelectedBaudRate()
      mCanCtl.InitLine(operatingMode, bitRate)

      '  Set the acceptance filter
      Dim accCode As UInteger
      Dim accMask As UInteger

      accCode = Ixxat.Vci4.Bal.Can.CanAccCode.All
      accMask = Ixxat.Vci4.Bal.Can.CanAccMask.All

      mCanCtl.SetAccFilter(Ixxat.Vci4.Bal.Can.CanFilter.Std, accCode, accMask)

      ' Start the CAN controller
      mCanCtl.StartLine()
    Catch ex As Exception
      MessageBox.Show(ex.ToString())
      Return
    End Try


  End Sub

  ' if another controller is selected then the
  ' existing controller and channel objects must be closed
  Private Sub CloseCurrentExistingController()
    ' if necessary close a existing connection
    If mCanCtl IsNot Nothing Then
      ' stop the CAN controller
      Try
        mCanCtl.StopLine()
      Catch ex As Exception
        ' the can control has leave the scope,
        ' e.g. a plugable interface which was removed
      End Try
    End If

    ' close the receive thread we will reopen it
    ' with another event
    If rxThread IsNot Nothing Then
      '
      ' tell receive thread to quit
      '
      rxThread.Abort()

      '
      ' Wait for termination of receive thread
      '
      rxThread.Join()

      rxThread = Nothing
    End If
    CloseVciObjects(False)
  End Sub

  Private Sub CloseVciObjects(closeVciObject as Boolean)
    ' Dispose all hold VCI objects.

    ' Dispose message reader
    If (mReader IsNot Nothing) Then
      DisposeVciObject(mReader)
      mReader = Nothing
    End If

    ' Dispose message writer 
    If (mWriter IsNot Nothing) Then
      DisposeVciObject(mWriter)
      mWriter = Nothing
    End If

    ' Dispose CAN channel
    If (mCanChn IsNot Nothing) Then
      DisposeVciObject(mCanChn)
      mCanChn = Nothing
    End If

    ' Dispose CAN controller
    If (mCanCtl IsNot Nothing) Then
      DisposeVciObject(mCanCtl)
      mCanCtl = Nothing
    End If

    ' Dispose VCI device
    DisposeVciObject(mDevice)
  End Sub

  Private Sub DisposeVciObject(ByVal obj As Object)
    If obj IsNot Nothing Then
      Dim dispose As System.IDisposable
      dispose = obj
      If dispose IsNot Nothing Then
        dispose.Dispose()
        obj = Nothing
      End If
    End If
  End Sub

  Private Sub SetText(ByVal [text] As String)

    ' InvokeRequired required compares the thread ID of the
    ' calling thread to the thread ID of the creating thread.
    ' If these threads are different, it returns true.
    If labelLastRxMsg.InvokeRequired Then
      Dim d As New SetTextCallback(AddressOf SetText)
      Me.Invoke(d, New Object() {[text]})
    Else
      labelLastRxMsg.Text = [text]
    End If
  End Sub

  ' a thread callback function to check if the 
  ' interfaces have changed (e.g. removed or added a USB interface)
  Private Sub InterfaceChangeThreadFunc()
    Do
      If changeEvent.WaitOne(-1, False) Then
        FillListBoxWithInterFaces()
      End If
    Loop
  End Sub


  Private Sub ReceiveThreadFunc()

    Dim canMessage As Ixxat.Vci4.Bal.Can.ICanMessage = Nothing

    Do
      ' Wait 100 msec for a message reception
      If mRxEvent.WaitOne(100, False) Then

        '      // read a CAN message from the receive FIFO
        If mReader.ReadMessage(canMessage) Then
          ShowReceivedMessage(canMessage)
        End If 'If mReader.ReadMessage(canMessage) Then
      End If  'If mRxEvent.WaitOne(100, False) Then
    Loop
  End Sub

  Private Sub ShowReceivedMessage(ByVal canMessage As Ixxat.Vci4.Bal.Can.ICanMessage)
    Select Case canMessage.FrameType
      Case Ixxat.Vci4.Bal.Can.CanMsgFrameType.Data
        ShowDataMessage(canMessage)
      Case Ixxat.Vci4.Bal.Can.CanMsgFrameType.Error
        ShowErrorMessage(canMessage)
      Case Ixxat.Vci4.Bal.Can.CanMsgFrameType.Info
        ShowInfoMessage(canMessage)
      Case Ixxat.Vci4.Bal.Can.CanMsgFrameType.Status
        ShowStatusMessage(canMessage)
      Case Ixxat.Vci4.Bal.Can.CanMsgFrameType.TimeOverrun
        ShowTimerOverrunMessage(canMessage)
      Case Ixxat.Vci4.Bal.Can.CanMsgFrameType.TimeReset
        ShowTimerResetMessage(canMessage)
      Case Ixxat.Vci4.Bal.Can.CanMsgFrameType.Wakeup
        ShowWakeUpMessage(canMessage)
    End Select
  End Sub

  Private Sub ShowDataMessage(ByVal canMessage As Ixxat.Vci4.Bal.Can.ICanMessage)
    Dim textLine As String
    textLine = "Time: " + canMessage.TimeStamp.ToString + " ID: " + canMessage.Identifier.ToString("X3") + "h"

    If canMessage.RemoteTransmissionRequest Then
      textLine = textLine + " Remote Request Data Length: " + canMessage.DataLength.ToString()
    Else
      Dim i As Byte
      For i = 1 To canMessage.DataLength
        ' we start the data bytes from 0
        textLine = textLine + " " + canMessage(i - 1).ToString("X2")
      Next
    End If

    If canMessage.SelfReceptionRequest Then
      textLine = textLine + " Self Reception"
    End If

    ' set the text thread safe to the label
    SetText(textLine)
  End Sub

  Private Sub ShowErrorMessage(ByVal canMessage As Ixxat.Vci4.Bal.Can.ICanMessage)
    ' todo
    Dim msgError As Ixxat.Vci4.Bal.Can.CanMsgError
    msgError = canMessage(0)
    Select Case msgError
      Case Ixxat.Vci4.Bal.Can.CanMsgError.Acknowledge
        SetText("Error: Acknowledge")
      Case Ixxat.Vci4.Bal.Can.CanMsgError.Bit
        SetText("Error: Bit")
      Case Ixxat.Vci4.Bal.Can.CanMsgError.Crc
        SetText("Error: Crc")
      Case Ixxat.Vci4.Bal.Can.CanMsgError.Form
        SetText("Error: Form")
      Case Ixxat.Vci4.Bal.Can.CanMsgError.Other
        SetText("Error: Other")
      Case Ixxat.Vci4.Bal.Can.CanMsgError.Stuff
        SetText("Error: Stuff")
    End Select
  End Sub
  Private Sub ShowInfoMessage(ByVal canMessage As Ixxat.Vci4.Bal.Can.ICanMessage)

  End Sub
  Private Sub ShowStatusMessage(ByVal canMessage As Ixxat.Vci4.Bal.Can.ICanMessage)
  End Sub
  Private Sub ShowTimerOverrunMessage(ByVal canMessage As Ixxat.Vci4.Bal.Can.ICanMessage)
    ' todo
  End Sub
  Private Sub ShowTimerResetMessage(ByVal canMessage As Ixxat.Vci4.Bal.Can.ICanMessage)
    ' todo
  End Sub
  Private Sub ShowWakeUpMessage(ByVal canMessage As Ixxat.Vci4.Bal.Can.ICanMessage)
    ' todo
  End Sub



  Private Sub ButtonTransmitData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonTransmitData.Click

    If mWriter IsNot Nothing Then
      'IMessageFactory factory = VciServer.Instance().MsgFactory;
      Dim canMessage As Ixxat.Vci4.Bal.Can.ICanMessage
      Dim factory As Ixxat.Vci4.IMessageFactory
      factory = Ixxat.Vci4.VciServer.Instance.MsgFactory

      canMessage = factory.CreateMsg(GetType(Ixxat.Vci4.Bal.Can.ICanMessage))

      ' canMessage = factory.CreateMsg(Ixxat.Vci4.Bal.Can.ICanMessage)

      'ICanMessage canMsg = (ICanMessage)factory.CreateMsg(typeof(Ixxat.Vci4.Bal.Can.ICanMessage));


      canMessage.TimeStamp = 0
      canMessage.Identifier = &H100
      canMessage.FrameType = Ixxat.Vci4.Bal.Can.CanMsgFrameType.Data
      canMessage.DataLength = 8
      canMessage.SelfReceptionRequest = True

      Dim i As Byte
      For i = 0 To 7
        canMessage(i) = i
      Next

      ' Write the CAN message into the transmit FIFO
      If mWriter.Capacity > 0 Then
        mWriter.SendMessage(canMessage)
      End If



    End If

  End Sub

  Private Sub TimerGetStatus_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimerGetStatus.Tick

    If mCanCtl IsNot Nothing Then
      Dim lineStatus As Ixxat.Vci4.Bal.Can.CanLineStatus

      ' use a try, when we want read the status of 
      ' a interface which does not longer exists it will
      ' crash
      Try
        lineStatus = mCanCtl.LineStatus

        If lineStatus.IsInInitMode Then
          PictureBoxInitMode.BackColor = Color.Red
        Else
          PictureBoxInitMode.BackColor = Color.Green
        End If

        If lineStatus.IsTransmitPending Then
          PictureBoxTxPending.BackColor = Color.Red
        Else
          PictureBoxTxPending.BackColor = Color.Green
        End If

        If lineStatus.HasDataOverrun Then
          PictureBoxOverrun.BackColor = Color.Red
        Else
          PictureBoxOverrun.BackColor = Color.Green
        End If

        If lineStatus.HasErrorOverrun Then
          PictureBoxWarningLevel.BackColor = Color.Red
        Else
          PictureBoxWarningLevel.BackColor = Color.Green
        End If

        If lineStatus.IsBusOff Then
          PictureBoxBusOff.BackColor = Color.Red
        Else
          PictureBoxBusOff.BackColor = Color.Green
        End If
      Catch ex As Exception
        PictureBoxInitMode.BackColor = Color.Gray
        PictureBoxTxPending.BackColor = Color.Gray
        PictureBoxOverrun.BackColor = Color.Gray
        PictureBoxOverrun.BackColor = Color.Gray
        PictureBoxWarningLevel.BackColor = Color.Gray
        PictureBoxBusOff.BackColor = Color.Gray
      End Try



    End If

  End Sub

  Private Sub FillListBoxWithInterFaces()

    If ListViewAvailInterfaces.InvokeRequired Then
      Dim d As New FillListBoxWithInterFacesCallBack(AddressOf FillListBoxWithInterFaces)
      Me.Invoke(d, New Object() {})
    Else
      ' first remove all items of the listbox
      ' ListBoxAvailInterfaces.Items.Clear()
      ListViewAvailInterfaces.Items.Clear()

      '    Dim CurItem As Item
      'Dim CurItemIndex As Integer

      ' now walk through the device list
      Try
        deviceManager = Ixxat.Vci4.VciServer.Instance().DeviceManager
        deviceList = deviceManager.GetDeviceList()
        deviceEnum = deviceList.GetEnumerator()
        deviceEnum.Reset()
        'WuP: todo
        ' deviceList.AssignEvent(changeEvent)
        Do While deviceEnum.MoveNext() = True
          'while deviceEnum.MoveNext() = True Then
          mDevice = deviceEnum.Current


          ' set the new list view item
          ' the Tag should be the unique object ID
          ' the Text should be the device description
          Dim CurListViewItem = New ListViewItem
          CurListViewItem.Tag = mDevice.VciObjectId
          CurListViewItem.Text = mDevice.Description

          ListViewAvailInterfaces.Items.Add(CurListViewItem)
        Loop


      Catch ex As Exception
      End Try
    End If



  End Sub

  Private Sub FormVCIV4Demo_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    Text = Text + " V" + Application.ProductVersion
    deviceManager = Ixxat.Vci4.VciServer.Instance().DeviceManager
    deviceList = deviceManager.GetDeviceList()

    ' set a event to the devicelist to send this event if the interface list changes
    changeEvent = New System.Threading.AutoResetEvent(False)
    deviceList.AssignEvent(changeEvent)

    FillListBoxWithInterFaces()

    ' set 500 kBit/s as default baud rate
    ListBoxBaudrate.SelectedIndex = 2

    ' start a own thread which wait for a interface change message e.g. a USB device was plugged in or out
    interfaceChangeThread = New System.Threading.Thread(New System.Threading.ThreadStart(AddressOf InterfaceChangeThreadFunc))
    interfaceChangeThread.Start()

  End Sub


  Private Sub ListViewAvailInterfaces_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListViewAvailInterfaces.SelectedIndexChanged
    ButtonInit.Enabled = True
    ShowDeviceHardwareID()
  End Sub

  Private Sub TimerSendData_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimerSendData.Tick
    If mWriter IsNot Nothing Then
      Dim canMessage As Ixxat.Vci4.Bal.Can.ICanMessage
      Dim factory As Ixxat.Vci4.IMessageFactory
      factory = Ixxat.Vci4.VciServer.Instance.MsgFactory

      canMessage = factory.CreateMsg(GetType(Ixxat.Vci4.Bal.Can.ICanMessage))

      canMessage.TimeStamp = 0
      canMessage.Identifier = &H100
      canMessage.FrameType = Ixxat.Vci4.Bal.Can.CanMsgFrameType.Data
      canMessage.DataLength = 8
      canMessage.SelfReceptionRequest = True

      Dim i As Byte
      For i = 0 To 7
        canMessage(i) = i
      Next

      ' Write the CAN message into the transmit FIFO
      If (Not mWriter.SendMessage(canMessage)) Then
        TimerSendData.Enabled = False
      End If

    End If
  End Sub

  Private Sub ShowDeviceHardwareID()
    Dim CurListViewItem As ListViewItem

    Dim selIndex As Windows.Forms.ListView.SelectedIndexCollection

    selIndex = ListViewAvailInterfaces.SelectedIndices()

    If selIndex.Count > 0 Then
      CurListViewItem = ListViewAvailInterfaces.SelectedItems(0)
      If CurListViewItem IsNot Nothing Then
        SelectDevice(CurListViewItem.Tag)
      End If
    End If
  End Sub


  ' Change the UniqueHardwareID to a string.
  ' Because of a bug in the .NET API unteil VCI 3.1.4.1784 the property
  ' UniqueHardwareId returns always a GUID. In newer version it returns
  ' a string with the HWxxxxx serial number.
  Private Function GetSerialNumber(ByVal inputGuid As System.Guid) As String
    Dim resultString As String

    ' Convert the GUID to a byte array
    Dim byteArray() As Byte = inputGuid.ToByteArray()

    ' The first 2 bytes must have HW as data, then it is really a serial number
    If (Chr(byteArray(0)) = "H") And (Chr(byteArray(1)) = "W") Then
      resultString = System.Text.Encoding.ASCII.GetString(byteArray)
    Else
      resultString = inputGuid.ToString()
    End If


    Return resultString
  End Function

  Private Function GetSelectedBaudRate() As Ixxat.Vci4.Bal.Can.CanBitrate

    Dim resultBaud As Ixxat.Vci4.Bal.Can.CanBitrate

    Select Case ListBoxBaudrate.SelectedIndex
      Case 0
        resultBaud = Ixxat.Vci4.Bal.Can.CanBitrate.Cia125KBit
      Case 1
        resultBaud = Ixxat.Vci4.Bal.Can.CanBitrate.Cia250KBit
      Case 2
        resultBaud = Ixxat.Vci4.Bal.Can.CanBitrate.Cia500KBit
      Case 3
        resultBaud = Ixxat.Vci4.Bal.Can.CanBitrate.Cia800KBit
      Case Else
        resultBaud = Ixxat.Vci4.Bal.Can.CanBitrate.Cia1000KBit
    End Select

    Return resultBaud
  End Function

  Private Sub FormVCIV4Demo_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
    CloseAll()
  End Sub

  ' Set the acceptance mask that all identifiers can pass
  Private Sub btnAcceptanceAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAcceptanceAll.Click

    mCanCtl.StopLine()

    '  Set the acceptance filter
    Dim accCode As UInteger
    Dim accMask As UInteger

    accCode = Ixxat.Vci4.Bal.Can.CanAccCode.All
    accMask = Ixxat.Vci4.Bal.Can.CanAccMask.All

    mCanCtl.SetAccFilter(Ixxat.Vci4.Bal.Can.CanFilter.Std, accCode, accMask)

    mCanCtl.StartLine()

  End Sub

  ' Set the acceptance mask that all identifiers with bit 0 have value 1 can pass
  Private Sub btnAcceptanceID1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAcceptanceID1.Click

    mCanCtl.StopLine()

    '  Set the acceptance filter
    Dim accCode As UInteger
    Dim accMask As UInteger

    ' we want to see all identifier which have bit 0 set as 1
    ' binary mask/value filter:
    ' 000 0000 0001
    ' 000 0000 0001
    ' -------------
    ' xxx xxxx xxx1
    accCode = &H1
    accMask = &H1

    ' shift the identifier values one bit left, this means that the last
    ' bit (bit 0) is 0 and the RTR bit in the filter doesn't matter
    ' binary mask/value filter:
    ' 0000 0000 0010
    ' 0000 0000 0010
    ' --------------
    ' xxxx xxxx xx1x
    accCode = accCode << 1
    accMask = accMask << 1

    accCode = accCode + 1
    accMask = accMask + 1



    mCanCtl.SetAccFilter(Ixxat.Vci4.Bal.Can.CanFilter.Std, accCode, accMask)

    mCanCtl.StartLine()

  End Sub
End Class
