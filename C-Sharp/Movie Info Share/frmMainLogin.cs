// Jeremiah Stones
// Advanced C#
// Brad Willingham
// 7/4/2017

// Program Specification
// This program allows user to view, update, add, and delete information from a SQL Server database.
// The user logs in (can create new user), and then can access the database.  The user may view other
// people's movie information as well.  The about form gives more detailed information about how to use
// the form.

using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace StonesJ_AdvancedDB
{
    public partial class frmMainLogin : Form
    {
        public frmMainLogin()
        {
            // default constructor for login form
            InitializeComponent();
            // DBOps.RemoveUser("Jeremy.Stones"); // used for testing log-in
            // still need method to remove all associated movies when a log-in is deleted
        }
        private void frmMainLogin_Load(object sender, EventArgs e)
        {
            // load users into dataSet for users
            try
            {
                DBOps.getUsers();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            // set initial password character - filled circle
            txtPassword.PasswordChar = (char)0x25CF;
            txtNewPassword.PasswordChar = (char)0x25CF;
            txtPasswordConfirm.PasswordChar = (char)0x25CF;
            // Load credentials for portfolio.
            txtUsername.Text = "user";
            txtPassword.Text = "password";
            // center panels for nice look!
            pnlNewUser.Location = new Point(this.Width / 2 - pnlNewUser.Width / 2 + pnlLogin.Width / 2, this.Height / 2 - pnlNewUser.Height / 2);
            pnlOldUser.Location = new Point(this.Width / 2 - pnlOldUser.Width / 2 + pnlLogin.Width / 2, this.Height / 2 - pnlOldUser.Height / 2);
        }

        private void chkShow_CheckedChanged(object sender, EventArgs e)
        {
            // if check state is changes, test it and set password character accordingly
            if (chkShow.Checked == true)
                txtPassword.PasswordChar = (char)0;
            else
                txtPassword.PasswordChar = (char)0x25CF;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            // properly exit program
            Application.Exit();
        }

        private void btnSignUpStart_Click(object sender, EventArgs e)
        {
            // alters form so that user can sign up for a new account
            pnlNewUser.Visible = true;
            pnlOldUser.Visible = false;
            clearControls();
            // refocus on first field of this view
            txtNewUsername.Focus();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // alters form back to original state of login of currently existing user
            pnlNewUser.Visible = false;
            pnlOldUser.Visible = true;
            clearControls();
            // refocus on first field of this view
            txtUsername.Focus();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            // if login button clicked, test username and password
            // store username and password textbox data into variables
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            // see if user found, so if not error message can be properly displayed
            bool userFound = false;
            foreach(DataRow r in DBOps.dsUsers.Tables["Users"].Rows)
            {
                // test every username and see if there's a match
                if(r[1].ToString() == username)
                {
                    // if a match is found set flag
                    userFound = true;
                    if (r[2].ToString() != password)
                    {
                        // test to see if the password entered matches the username in the database
                        // and if not, return an error message
                        MessageBox.Show("Incorrect password entered for user, " + username + ".");
                        // send user to re-enter password
                        txtPassword.Clear();
                        txtPassword.Focus();
                        return;
                    }
                }
            }
            if(!userFound)
            {
                // if the username was not in the database, display proper error message
                MessageBox.Show("This username does not exist: " + username);
                // send user to re-enter username and password
                txtUsername.Clear();
                txtPassword.Clear();
                txtUsername.Focus();
                return;
            }
            // if username/password match, set db current user to login username
            DBOps.currentUser = username;
            // clear form controls in case of return to form
            clearControls();
            // go to next form using current user
            frmMovies form = new frmMovies();
            form.Show();
            this.Hide();            
        }

        private void btnSignUpConfirm_Click(object sender, EventArgs e)
        {
            // if sign up button clicked, attempt sign up username and password
            // store username and password textbox data into variables
            string username = txtNewUsername.Text;
            string password = txtNewPassword.Text;
            string passwordConfirm = txtPasswordConfirm.Text;
            foreach (DataRow r in DBOps.dsUsers.Tables["Users"].Rows)
            {
                // test every username and see if there's a match
                if (r[1].ToString() == username)
                {
                    // if a match is found show error
                    MessageBox.Show("This username already exists.");
                    // send user back to enter new username and password
                    txtNewUsername.Clear();
                    txtNewPassword.Clear();
                    txtPasswordConfirm.Clear();
                    txtNewUsername.Focus();
                    return;
                }
            }
            // test to see if username meets required standards of username characters - using POSIX standards
            // POSIX = Portable Operating System Interface for Unix
            bool validUsername = DBOps.ValidUsername(username);
            if (!validUsername)
            {
                // if not a valid username, send user to enter new username and password
                txtNewUsername.Clear();
                txtNewPassword.Clear();
                txtPasswordConfirm.Clear();
                txtNewUsername.Focus();
                return;
            }
            // test to make sure user has matching passwords entered
            if(password != passwordConfirm)
            {
                // if passwords do not match, send user to re-enter passwords
                txtNewPassword.Clear();
                txtPasswordConfirm.Clear();
                txtNewPassword.Focus();
                MessageBox.Show("Passwords do not match.");
                return;
            }
            // test to see if password meets required standards for password characters
            bool validPass = DBOps.ValidPassword(password);
            if (!validPass)
            {
                // if invalid password, send user to re-enter passwords
                txtNewPassword.Clear();
                txtPasswordConfirm.Clear();
                txtNewPassword.Focus();
                return;
            }
            // sign add username to users table and sign in to next form using new user
            DBOps.AddUser(username, password);
            // show success message, and return to normal login state of form
            MessageBox.Show("User, " + username + ", has been added. Please login now.");
            btnCancel.PerformClick();
        }
        private void clearControls()
        {
            // clears form controls
            txtNewPassword.Clear();
            txtNewUsername.Clear();
            txtPassword.Clear();
            txtPasswordConfirm.Clear();
            txtUsername.Clear();
            chkShow.Checked = false;
        }

        private void txtUsernameANDtxtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            // tests to see if user hits the ENTER key while in username or password 
            // if so automatically presses the Login button for them
            if (e.KeyChar == (char)Keys.Enter)
                btnLogin.PerformClick();
        }
        private void SignUpTextBoxes_KeyPress(object sender, KeyPressEventArgs e)
        {
            // tests to see if user hits the ENTER key while in username or password 
            // if so automatically presses the Login button for them
            if (e.KeyChar == (char)Keys.Enter)
                btnSignUpConfirm.PerformClick();
        }
    }
}
