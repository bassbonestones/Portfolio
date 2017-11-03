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
using System.Windows.Forms;

namespace Stones_Final_Project
{
    public partial class frmSplash : Form
    {
        // this form is the splash screen for the program
        // it appears for one second and then the main form takes over
        Timer t;
        public frmSplash()
        {
            // initialize the form
            InitializeComponent();
            // set the transparency key to the forms background color so it appears completely
            // invisible except its controls 
            this.TransparencyKey = BackColor;
            // using a timer and tick even to end the splash screen and start frmMain
            t = new Timer();
            t.Interval = 3000;
            t.Start();
            t.Tick += new EventHandler((s,e)=> CloseMe(s,e,t));
        }
        private void CloseMe(object sender, EventArgs e, Timer t)
        {
            t.Stop();
            // Tick Event Handler, hides this form and opens a new instance
            // of frmMain
            this.Hide();
            frmStartupWizard form = new frmStartupWizard();
            form.Show();
            
        }
    }
}
