using System;
using System.Collections.Generic;
using System.Text;

namespace amk_dyno
{
    /// <summary>
    /// class that defines what an algorithm is, contains algorithm object
    /// </summary>
    class Algorithm
    {

        //dictionary that includes all parameters of the algorithm
        public Dictionary<string, double> parameters = new Dictionary<string, double>();


        //constructor, declare keys and default values for parameters dictionary
        public Algorithm()
        {
            this.parameters.Add("test1", 12);
            this.parameters.Add("test2", 13);
            this.parameters.Add("test3", 14);
            this.parameters.Add("test4", 15);
            this.parameters.Add("test5", 16);
        }


        //--RENAME--this places input boxes for all parameters
        //          and puts their names in labels in front, called at startup
        public void PlaceParameters()
        {
            int numOfParams = this.parameters.Count;

            //Label[] labels = new Label[numOfParams];
            //TextBox[] textBoxes = new TextBox[numOfParams];

            int i = 0;
            foreach (KeyValuePair<string, double> pars in parameters)
            {
                /*
                //place labels and textboxes in the right spots
                //put the created objects in arrays for reading later
                //  pars.Key and pars.Value to acces in this loop
                labels[i] = new Label
                {
                    Location = new Point(10, 10 + (i * 15)),
                    Text = pars.Key
                };

                textBoxes[i] = new TextBox
                {
                    Location = new Point(50, 10 + (i * 15))
                };
                i++; */
            }
        }

        //called when start button is pressed, fills parameters dictionary with values from gui
        public void ReadParameters()
        {
            int numOfParams = this.parameters.Count;

            //read all parameters from the text boxes
            //for-each on the array that contains the textboxes from PlaceParameters()
        }

        //function that returns a torque value to send to the inverter
        public float Torque()
        {

            //calculate torque from inputs and parameters
            return 0;
        }


    }
}
