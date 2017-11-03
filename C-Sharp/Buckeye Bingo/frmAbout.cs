// Jeremy Stones
// Advanced C#
// Brad Willinghame

// 8/13/2017
// Final Project

// This program is a bingo session program. It is meant to be used by the desk workers to 
// handle all of the data and calculations that are used over the course of the session.
// for more information about the form and how to use it, see the about and help pages
// at runtime.


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stones_Final_Project
{
    public partial class frmAbout : Form
    {
        // this form is just the about page, a simple view 
        public frmAbout()
        {
            // constructor
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            // close form when user clicks close button
            this.Close();
        }
    }
}
