// Jeremy Stones
// Special Topics
// Brad Willingham

// Lab 02
// 9/8/2017

// Program Specification
// This program connects to a database, verifies user account by login screen, and then
// allows the user to select both a user and an item to view information about any item
// owned by a user.

using System;
using System.Windows.Forms;

namespace Stones_Lab02
{
    public partial class frmLogin : Form
    {
        // This form class sets up a form for users to enter the page where they can acces
        // item information.

        // variable for current user
        public static string currentUser = "";
        // variable to exit if not submitting for log-in
        bool submitting = false;

        public frmLogin()
        {
            // constructor
            InitializeComponent();
            txtUsername.Text = "admin";
            txtPassword.Text = "admin";
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            // exit the program
            Application.Exit();
        }

        private void frmLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            // if form closing and not a log-in submission, close the application
            if (!submitting)
                Application.Exit();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            // when user clicks login button test for a valid match and log them in if correct

            if(txtUsername.Text.Trim().Length < 1)
            {
                // if nothing is entered for username, message user and return from button event
                MessageBox.Show("Username was not entered");
                txtUsername.Focus();
                return;
            }
            if(txtPassword.Text.Trim().Length < 1)
            {
                // if nothing is entered for password, message user and return from button event
                MessageBox.Show("Password was not entered.");
                txtPassword.Focus();
                return;
            }
            // boolean variables to test for correct username/password
            bool userFound = false;
            bool match = false;
            foreach (Stones_Lab02_DLL.User user in Stones_Lab02_DLL.User.CurrentUsers)
            {
                // loop through all users and find user data
                if (user.UserName == txtUsername.Text)
                {
                    // if username match is found, set userFound flag to true and test password
                    userFound = true;
                    if (user.Password == txtPassword.Text)
                    {
                        // if test this user's password again password textbox
                        // if a match, set flag to true and set current user
                        match = true;
                        currentUser = user.FullName;
                    }
                }
            }
            if(!userFound)
            {
                // if no matching username, message user and return
                MessageBox.Show("User not found.");
                return;
            }
            if(!match)
            {
                // if no matching password, message user and return
                MessageBox.Show("Invalid password.");
                return;
            }
            // if all checks passed, set submitting flag to continue to next form and close this form
            submitting = true;
            this.Close();
        }

        private void txtUsername_KeyDown(object sender, KeyEventArgs e)
        {
            // keypress event while user in either textbox
            if(e.KeyCode == Keys.Enter)
            {
                // perform click on login button when user hits enter
                btnLogin.PerformClick();
            }
        }
    }
}
