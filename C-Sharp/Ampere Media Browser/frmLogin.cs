using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stones_MediaLab
{
    public partial class frmLogin : Form
    {
        public static string Username;
        public static string Password;

        public frmLogin(frmMain form)
        {
           
            InitializeComponent();

            form.Hide();
            form.Show();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            // load users into dataSet for users
            try
            {
                DBOps.GetUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            // set initial password character - filled circle
            txtPassword.PasswordChar = (char)0x25CF;
            txtNewPassword.PasswordChar = (char)0x25CF;
            txtPasswordConfirm.PasswordChar = (char)0x25CF;
            txtUsername.Text = "username";
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
            // set dialog result
            this.DialogResult = DialogResult.Cancel;
            // close form
            this.Close();
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

            // set properties and dialog result
            Username = txtUsername.Text;
            Password = txtPassword.Text;
            // see if user found, so if not error message can be properly displayed
            bool userFound = false;
            foreach (DataRow r in DBOps.DsUsers.Tables[DBOps.DtUsers].Rows)
            {
                // test every username and see if there's a match
                if (r[0].ToString() == Username)
                {
                    // if a match is found set flag
                    userFound = true;
                    if (r[1].ToString() != Password)
                    {
                        // test to see if the password entered matches the username in the database
                        // and if not, return an error message
                        MessageBox.Show("Incorrect password entered for user, " + Username + ".");
                        // send user to re-enter password
                        txtPassword.Clear();
                        txtPassword.Focus();
                        return;
                    }
                }
            }
            if (!userFound)
            {
                // if the username was not in the database, display proper error message
                MessageBox.Show("This username does not exist: " + Username);
                // send user to re-enter username and password
                txtUsername.Clear();
                txtPassword.Clear();
                txtUsername.Focus();
                return;
            }
            // clear form controls in case of return to form
            clearControls();

            this.DialogResult = DialogResult.OK;
            // go to next form using current user
            this.Close();
        }

        private void btnSignUpConfirm_Click(object sender, EventArgs e)
        {
            // if sign up button clicked, attempt sign up username and password
            // store username and password textbox data into variables
            Username = txtNewUsername.Text;
            Password = txtNewPassword.Text;
            string passwordConfirm = txtPasswordConfirm.Text;
            foreach (DataRow r in DBOps.DsUsers.Tables["Users"].Rows)
            {
                // test every username and see if there's a match
                if (r[0].ToString() == Username)
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
            bool validUsername = DBOps.ValidUsername(Username);
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
            if (Password != passwordConfirm)
            {
                // if passwords do not match, send user to re-enter passwords
                txtNewPassword.Clear();
                txtPasswordConfirm.Clear();
                txtNewPassword.Focus();
                MessageBox.Show("Passwords do not match.");
                return;
            }
            // test to see if password meets required standards for password characters
            bool validPass = DBOps.ValidPassword(Password);
            if (!validPass)
            {
                // if invalid password, send user to re-enter passwords
                txtNewPassword.Clear();
                txtPasswordConfirm.Clear();
                txtNewPassword.Focus();
                return;
            }
            // sign add username to users table and sign in to next form using new user
            DBOps.AddUser(Username, Password);
            // show success message, and return to normal login state of form
            MessageBox.Show("User, " + Username + ", has been added. Please login now.");
            try
            {
                DBOps.GetUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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

        private void txtUsername_KeyPress(object sender, KeyPressEventArgs e)
        {
            // tests to see if user hits the ENTER key while in username or password 
            // if so automatically presses the Login button for them
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnLogin.PerformClick();
                // removes annoying sound
                e.Handled = true;
            }
        }

        private void txtNewUsername_KeyPress(object sender, KeyPressEventArgs e)
        {
            // tests to see if user hits the ENTER key while in username or password 
            // if so automatically presses the Sign Up button for them
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnSignUpConfirm.PerformClick();
                // removes annoying sound
                e.Handled = true;
            } 
        }
    }
}
