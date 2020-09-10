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

        public System.Windows.Forms.Label test2;

        private void Form1_Load(object sender, EventArgs e)
        {
            //program startup
            Algorithm algorithm = new Algorithm();
            algorithm.PlaceParameters();


            //trying to get a label created here to show up on the form
            this.SuspendLayout();
            this.Controls.Add(this.test2);
            this.test2 = new System.Windows.Forms.Label
            {
                AutoSize = true,
                Location = new System.Drawing.Point(100, 100),
                Name = "test2",
                Size = new System.Drawing.Size(75, 30),
                Text = "testtest"
            };
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void StartStop_Click(object sender, EventArgs e)
        {
            //start-stop button
            running = !running;
            if(running)
            {
                //turn on
                StartStop.BackColor = Color.FromArgb(0, 192, 0);
            }
            else
            {
                //turn off
                StartStop.BackColor = Color.FromArgb(192, 0, 0);
            }

        }
    }
}
