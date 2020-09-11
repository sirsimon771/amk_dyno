using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Ixxat.Vci4;
using Ixxat.Vci4.Bal;
using Ixxat.Vci4.Bal.Can;

namespace amk_dyno
{
    /// <summary>
    /// contains the read and write functions to communicate with the CAN adapter
    /// using the provided C# API of the driver
    /// just copy from example, put some textboxes down for config and a connect button
    /// </summary>
    class CAN
    {

        /// <summary>
        ///   Reference to the used VCI device.
        /// </summary>
        static IVciDevice mDevice;

        /// <summary>
        ///   Reference to the CAN controller.
        /// </summary>
        static ICanControl mCanCtl;

        /// <summary>
        ///   Reference to the CAN message communication channel.
        /// </summary>
        static ICanChannel mCanChn;

        /// <summary>
        ///   Reference to the CAN message scheduler.
        /// </summary>
        static ICanScheduler mCanSched;

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



        public void CanSetup()
        {

        }

        public bool CanSend()
        {
            return true;
        }

        public double CanReceive()
        {
            return 1;
        }

    }
}
