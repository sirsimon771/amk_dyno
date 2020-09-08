using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace amk_dyno
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

        }
    

        /// <summary>
        /// class that defines what an algorithm is,
        /// contains algorithm objects which consist of:
        ///   - parameters
        ///   - function that outputs torque to send to the inverter
        ///     based on parameters and inputs
        /// </summary>
        static class Algorithm
        {





        }



    }
}
