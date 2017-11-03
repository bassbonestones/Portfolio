//Jeremiah Stones 5/15/2017
//ITSE 2338
//Lab09, Address Book Part 2 of 2

//See Program Specification in Specification Folder or See About During Runtime.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StonesJ_Lab09
{
    class State
    {
        // class created to store the form location on the screen and the form state
        // so that the info can be passed to each form as a new form is activated or loaded

        private static FormWindowState _myState;
        private static Point _formLocation;
        private static Point _centerScreenLocation;
        public static FormWindowState MyState
        {
            get { return _myState; }
            set { _myState = value; }
        }
        public static Point FormPosition
        {
            get { return _formLocation; }
            set { _formLocation = value; }
        }
        public static Point CenterScreenLocation
        {
            get { return _centerScreenLocation; }
            set { _centerScreenLocation = value; }
        }
    }
}
