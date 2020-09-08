using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace amk_dyno
{
    public partial class Form1 : Form
    {
        public bool running = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //program startup
            Algorithm algorithm = new Algorithm();
            algorithm.PlaceParameters();

        }

        private void startstop_Click(object sender, EventArgs e)
        {
            //start-stop button
            running = !running;
            if(running)
            {
                startstop.BackColor = Color.FromArgb(0, 192, 0);
            }
            else
            {
                startstop.BackColor = Color.FromArgb(192, 0, 0);
            }

        }
    }
}
