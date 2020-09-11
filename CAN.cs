using System;
using System.Collections.Generic;
using System.Text;

namespace amk_dyno
{
    /// <summary>
    /// contains the read and write functions to communicate with the CAN adapter
    /// using the provided C# API of the driver
    /// just copy from example and open a cmd for setup
    /// </summary>
    class CAN
    {
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
