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
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stones_Final_Project
{
    public partial class frmSecurity : Form
    {
        // this form is for verifying admin user to be able to update any bingo database information
        // once verified, they have full access to database alteration

        // variables for form setup
        private Point ChangePoint_Active;
        private int ChangeModeHeight = 270;
        private int PasswordModeHeight = 195;
        private int lastTxtFocused = 0;
        public static bool passed = false;

        public frmSecurity()
        {
            // constructor
            InitializeComponent();
            // define change location
            ChangePoint_Active = new Point(pnlChange.Location.X, 55);
            // set system password use at beginning (user can alter)
            txtPassword.UseSystemPasswordChar = true;
            txtNew1Change.UseSystemPasswordChar = true;
            txtNew2Change.UseSystemPasswordChar = true;
            txtOldChange.UseSystemPasswordChar = true;
            txtNew1Temp.UseSystemPasswordChar = true;
            txtNew2Temp.UseSystemPasswordChar = true;
            txtOldTemp.UseSystemPasswordChar = true;
            // set panel locations
            pnlChange.Location = ChangePoint_Active;
            pnlTemp.Location = ChangePoint_Active;
            // set height
            this.Height = PasswordModeHeight;
            txtPassword.Focus();
            txtPassword.Text = "password";
        }
        private void chkShowPass_CheckedChanged(object sender, EventArgs e)
        {
            // show password clicked to toggle state
            if (chkShowPass.Checked)
            {
                // show password
                txtPassword.UseSystemPasswordChar = false;
            }
            else
            {
                // hide password
                txtPassword.UseSystemPasswordChar = true;
            }
            // toggle other checkboxes to do the same
            chkShowChange.CheckState = chkShowPass.CheckState;
            chkShowTemp.CheckState = chkShowPass.CheckState;
            txtPassword.Focus();
        }
        private void lklChange_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // lable link clicked to change password so display change password boxes
            txtPassword.Clear();
            pnlPassword.Visible = false;
            this.Height = ChangeModeHeight;
            pnlChange.Visible = true;
            lblTitle.Text = "Change Password";
            txtOldChange.Focus();
        }
        private void btnCancelChange_Click(object sender, EventArgs e)
        {
            // user clicks cancel change password, so return to regular entry
            txtNew1Change.Clear();
            txtNew2Change.Clear();
            txtOldChange.Clear();
            pnlChange.Visible = false;
            pnlPassword.Visible = true;
            this.Height = PasswordModeHeight;
            lblTitle.Text = "Administrator Access Password";
            txtPassword.Focus();
        }
        private void chkShowChange_CheckedChanged(object sender, EventArgs e)
        {
            // is user click show password check box alter state
            if (chkShowChange.Checked)
            {
                // if checked, show all passwords
                txtNew1Change.UseSystemPasswordChar = false;
                txtNew2Change.UseSystemPasswordChar = false;
                txtOldChange.UseSystemPasswordChar = false;
            }
            else
            {
                // hide passwords
                txtNew1Change.UseSystemPasswordChar = true;
                txtNew2Change.UseSystemPasswordChar = true;
                txtOldChange.UseSystemPasswordChar = true;
            }
            // toggle other show password checkboxes
            chkShowPass.CheckState = chkShowChange.CheckState;
            chkShowTemp.CheckState = chkShowChange.CheckState;
            // return user to last focused textbox
            switch(lastTxtFocused)
            {
                case 1:
                    txtOldChange.Focus();
                break;
                case 2:
                    txtNew1Change.Focus();
                break;
                case 3:
                    txtNew2Change.Focus();
                break;
                default:
                    break;
            }
        }
        private void lklForgot_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string tempPass = "";
            Random random; 
            
            for(int i = 0; i < 8; i++)
            {
                // loop 8 times and append a new character to the password each time

                // seed new random
                random = new Random(Environment.TickCount);
                // get a random character type (3 options)
                int type = random.Next(0,3);
                // create variable to hold next character int value
                int charInt = 50;
                switch(type)
                {
                    case 0:
                        // type number
                        charInt = random.Next(48, 58);
                        break;
                    case 1:
                        // type uppercase letter
                        charInt = random.Next(65, 91);
                        break;
                    case 2:
                        // type lowercase letter
                        charInt = random.Next(97, 123);
                        break;
                    default:
                        break;
                }
                tempPass += (char)charInt;
                // sleep 20 ms to help randomization seed
                Thread.Sleep(20);
            }
            // set up mail message
            MailMessage mail = new MailMessage();
            // add default email to send temp password
            mail.To.Add(DbOps.Email);
            // register host account for mail send
            mail.From = new MailAddress("passhelp.buckeye@gmail.com");
            mail.Subject = "Password Recovery: Buckeye Bingo Charity Sessions";
            string Body = "Your new temporary password is: " + tempPass +
                "  -- Enter this temporary password to gain administrative access to the Buckeye Bingo Charity Sessions " +
                "application. You will be required to set up a new password. Do not respond to this email address.";
            mail.Body = Body;
            mail.IsBodyHtml = true;
            // update temp password in program to allow user to verify it
            DbOps.TempPassword = tempPass;
            // commented code left for future reference
            // Attachment at = new Attachment(Server.MapPath("~/ExcelFile/TestCSV.csv"));
            // mail.Attachments.Add(at);
            // for faster and more reliable receiving
            mail.Priority = MailPriority.High;
            // set up smtp client
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com"; 
            // credentials for fake email (setup just for this)
            smtp.Credentials = new System.Net.NetworkCredential("passhelp.buckeye@gmail.com", "Bingo123!");
            // enable ssl for secure email sending
            smtp.EnableSsl = true;
            // gmail send port
            smtp.Port = 587;
            smtp.Send(mail);
            // toggle form to temp password entry
            pnlPassword.Visible = false;
            pnlTemp.Visible = true;
            txtPassword.Clear();
            lblTitle.Text = "Reset Security Password";
            this.Height = ChangeModeHeight;
            txtOldTemp.Focus();
        }
        private void btnCancelTemp_Click(object sender, EventArgs e)
        {
            // if user cancels temp password entry, return to regular password entry
            pnlTemp.Visible = false;
            pnlPassword.Visible = true;
            this.Height = PasswordModeHeight;
            txtNew1Temp.Clear();
            txtNew2Temp.Clear();
            txtOldTemp.Clear();
            lblTitle.Text = "Administrator Access Password";
            txtPassword.Focus();
        }
        private void chkShowTemp_CheckedChanged(object sender, EventArgs e)
        {
            // method to toggle password display
            if (chkShowTemp.Checked)
            {
                // show all passwords
                txtNew1Temp.UseSystemPasswordChar = false;
                txtNew2Temp.UseSystemPasswordChar = false;
                txtOldTemp.UseSystemPasswordChar = false;
            }
            else
            {
                // hide all passwords
                txtNew1Temp.UseSystemPasswordChar = true;
                txtNew2Temp.UseSystemPasswordChar = true;
                txtOldTemp.UseSystemPasswordChar = true;
            }
            // toggle other password show checkboxes to do the same with them
            chkShowPass.CheckState = chkShowTemp.CheckState;
            chkShowChange.CheckState = chkShowTemp.CheckState;
            switch(lastTxtFocused)
            {
                // return user to last textbox focused
                case 1:
                    txtOldTemp.Focus();
                    break;
                case 2:
                    txtNew1Temp.Focus();
                    break;
                case 3:
                    txtNew2Temp.Focus();
                    break;
                default:
                    break;
            }
        }
        private void lklSendAgain_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // secondary link to send email if first didn't work or email lost
            lklForgot_LinkClicked(sender, e);
        }
        private void btnCancelPass_Click(object sender, EventArgs e)
        {
            // close this form
            this.Close();
        }
        private void txtOldChange_Enter(object sender, EventArgs e)
        {
            // allows program to keep track of last textbox user was in to return to it
            lastTxtFocused = 1;
        }
        private void txtNew1Change_Enter(object sender, EventArgs e)
        {
            // allows program to keep track of last textbox user was in to return to it
            lastTxtFocused = 2;
        }
        private void txtNew2Change_Enter(object sender, EventArgs e)
        {
            // allows program to keep track of last textbox user was in to return to it
            lastTxtFocused = 3;
        }
        private void txtOldTemp_Enter(object sender, EventArgs e)
        {
            // allows program to keep track of last textbox user was in to return to it
            lastTxtFocused = 1;
        }
        private void txtNew1Temp_Enter(object sender, EventArgs e)
        {
            // allows program to keep track of last textbox user was in to return to it
            lastTxtFocused = 2;
        }
        private void txtNew2Temp_Enter(object sender, EventArgs e)
        {
            // allows program to keep track of last textbox user was in to return to it
            lastTxtFocused = 3;
        }
        private void btnSubmitPass_Click(object sender, EventArgs e)
        {
            // submission of password for security

            if(txtPassword.Text == DbOps.CurrentPassword || txtPassword.Text == DbOps.MasterPassword)
            {
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                // if error return CANCEL
                MessageBox.Show("incorrect password");
            }
        }
        private void btnSubmitChange_Click(object sender, EventArgs e)
        {
            // password change verify
            if(txtOldChange.Text == DbOps.CurrentPassword)
            {
                if (txtNew1Change.Text.Trim().Length > 0)
                {
                    if (txtNew1Change.Text == txtNew2Change.Text)
                    {
                        DbOps.UpdateDefault(txtNew1Change.Text, "CurrentPassword");
                        DbOps.FillDefaultsTable();
                        MessageBox.Show("Password updated.");
                        btnCancelChange.PerformClick();
                    }
                }
            }
            else
            {
                MessageBox.Show("invalid password");
                return;
            }
        }
        private void btnSubmitTemp_Click(object sender, EventArgs e)
        {
            // temp password verify
            if (txtOldTemp.Text == DbOps.TempPassword)
            {
                // if old password is correct
                if (txtNew1Temp.Text.Trim().Length > 0 && txtNew2Temp.Text.Trim().Length > 0)
                {
                    // test to see if new enter exists
                    if (txtNew1Temp.Text == txtNew2Temp.Text)
                    {
                        // test that both textboxes are the same, if so, change password
                        DbOps.UpdateDefault(txtNew1Temp.Text, "CurrentPassword");
                        DbOps.FillDefaultsTable();
                        MessageBox.Show("Password updated.");
                        // click cancel to return to password entry 
                        btnCancelTemp.PerformClick();
                    }
                    else
                    {
                        // error if new password don't match
                        MessageBox.Show("New passwords do not match.");
                    }
                }
                else
                {
                    // error if not both new password textboxes filled
                    MessageBox.Show("No password entered for one or more fields.");
                }
            }
            else
            {
                // if invalid old password, message user
                MessageBox.Show("invalid password");
                return;
            }
        }
        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            // if user hits enter while in textbox, click button
            if(e.KeyCode == Keys.Enter)
            {
                btnSubmitPass.PerformClick();
                e.Handled = true;
            }
        }
    }
}
