//Jeremiah Stones 5/15/2017
//ITSE 2338
//Lab09, Address Book Part 2 of 2

//See Program Specification in Specification Folder or See About During Runtime.

using System;
using System.Windows.Forms;

namespace StonesJ_Lab09
{
    public partial class frmAbout : Form
    {
        // this for simply shows information about the program
        public frmAbout()
        {
            // constructor, sets the position of elements on the form
            InitializeComponent();
            lblTitle.SetBounds(this.Size.Width / 2 - lblTitle.Size.Width / 2, lblTitle.Bounds.Y, lblTitle.Width, lblTitle.Height);
            pbxHR.SetBounds(this.Size.Width / 2 - pbxHR.Size.Width / 2, pbxHR.Bounds.Y, pbxHR.Width, pbxHR.Height);
            btnHome.SetBounds(this.Size.Width / 2 - btnHome.Size.Width / 2, this.Size.Height - btnHome.Size.Height * 3 + 20, btnHome.Size.Width, btnHome.Size.Height);
            pbxContacts.SetBounds(this.Size.Width / 2 - pbxContacts.Size.Width / 2 + 170, pbxContacts.Bounds.Y, pbxContacts.Width, pbxContacts.Height);
            label1.SetBounds(this.Size.Width / 2 - label1.Size.Width / 2, label1.Bounds.Y, label1.Width, label1.Height);
            AppExit.ResetCloseFlag();
        }

        // const variable for param of disabled X button
        private const int CP_NOCLOSE_BUTTON = 0x200;
        protected override CreateParams CreateParams
        {
            // overrides the CreateParams for the form to disable the X button
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        }


        private void btnHome_Click(object sender, EventArgs e)
        {
            // returns the user to frmMain
            AppExit.CloseToNew = true;
            Application.OpenForms[0].Show();
            this.Close();

        }

        private void frmAbout_Resize(object sender, EventArgs e)
        {
            // resets form control position each time the form size changes
            lblTitle.SetBounds(this.Size.Width / 2 - lblTitle.Size.Width / 2, lblTitle.Bounds.Y, lblTitle.Width, lblTitle.Height);
            pbxHR.SetBounds(this.Size.Width / 2 - pbxHR.Size.Width / 2, pbxHR.Bounds.Y, pbxHR.Width, pbxHR.Height);
            btnHome.SetBounds(this.Size.Width / 2 - btnHome.Size.Width / 2, this.Size.Height - btnHome.Size.Height * 3 + 20, btnHome.Size.Width, btnHome.Size.Height);
            pbxContacts.SetBounds(this.Size.Width / 2 - pbxContacts.Size.Width / 2 + 170, pbxContacts.Bounds.Y, pbxContacts.Width, pbxContacts.Height);
            label1.SetBounds(this.Size.Width / 2 - label1.Size.Width / 2, label1.Bounds.Y, label1.Width, label1.Height);
        }

        private void frmAbout_FormClosing(object sender, FormClosingEventArgs e)
        {
            // if form closes somehow other than by close button,
            // write to file to save changes
            if(!AppExit.CloseToNew)
            {
                AppExit.WriteToFile();
            }
        }

        private void frmAbout_FormClosed(object sender, FormClosedEventArgs e)
        {
            // if this form closes somehow other than by close button
            // after the form has closed (and written to file)
            // also exit the entire application.

            if (!AppExit.CloseToNew)
            {
                Application.Exit();
            }
        }
    }
}
