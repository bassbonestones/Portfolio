//Jeremiah Stones 5/15/2017
//ITSE 2338
//Lab09, Address Book Part 2 of 2

//See Program Specification in Specification Folder or See About During Runtime.

using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.Data;
using System.Linq;

namespace StonesJ_Lab09
{
    public partial class frmRecords : Form
    {
        
        // declare global variables

        public string _state; // for polymorphic state of record form
        public int _entryPlace; // place holder index for which entry the user is on currently
        int _printerPageCtr;
        bool _printMore;
        int _printPageNum;
        public frmRecords(string state) 
        {
            // constructor for frmRecords, sets the state, loads address book from previous form
            // sets initial Entry to the 0th index
            InitializeComponent();
            this._state = state;
            this._entryPlace = 0;
            this._printerPageCtr = 0;
            this._printMore = false;
            this._printPageNum = 1;
            AppExit.ResetCloseFlag();
        }
        public frmRecords(string state, int pos)
        {
            // secondary constructor with position of record
            InitializeComponent();
            this._state = state;
            this._entryPlace = 0;
            this._printerPageCtr = 0;
            this._printMore = false;
            this._printPageNum = 1;
            this._entryPlace = pos;
            AppExit.ResetCloseFlag();
        }

        //// constant referring to a parameter for the form
        //// for disabled exit button
        //private const int CP_NOCLOSE_BUTTON = 0x200;
        //protected override CreateParams CreateParams
        //{
        //    // overridden function to disable the exit button
        //    get
        //    {
        //        CreateParams myCp = base.CreateParams;
        //        myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
        //        return myCp;
        //    }
        //}

        private void frmRecords_Load(object sender, EventArgs e)
        {
            // load event, use to set state and fill data on form
            btnAdd.Bounds = btnUpdate.Bounds;
            btnDelete.Bounds = btnUpdate.Bounds;
            AppExit.ResetCloseFlag();
            SetState(_state);
            this.WindowState = State.MyState;
            if (this.WindowState != FormWindowState.Maximized)
                this.SetDesktopLocation(State.CenterScreenLocation.X, State.CenterScreenLocation.Y);
        }

        private bool _myValidation()
        {
            // function to handle entry field validation for both adding and updating records
            if (txtFirst.Text == "")
            {
                MessageBox.Show("Please enter the contact's first name.");
                txtFirst.Focus();
                return false;
            }
            if (txtLast.Text == "")
            {
                MessageBox.Show("Please enter the contact's last name.");
                txtLast.Focus();
                return false;
            }
            if (txtStreet.Text == "")
            {
                MessageBox.Show("Please enter the contact's street address.");
                txtStreet.Focus();
                return false;
            }
            if (txtCity.Text == "")
            {
                MessageBox.Show("Please enter the contact's city.");
                txtCity.Focus();
                return false;
            }
            if (cbxState.SelectedIndex == -1)
            {
                MessageBox.Show("Please select the contact's state.");
                cbxState.Focus();
                return false;
            }
            if (txtZip.Text == "")
            {
                MessageBox.Show("Please enter the contact's zip code.");
                txtZip.Focus();
                return false;
            }
            if (txtZip.Text.Length != 5)
            {
                MessageBox.Show("Please enter a full, 5-digit zip code.");
                txtZip.Focus();
                txtZip.SelectAll();
                return false;
            }
            if (txtPhone.Text == "")
            {
                MessageBox.Show("Please enter the contact's phone number.");
                txtPhone.Focus();
                return false;
            }
            int phoneCtr = 0;
            foreach (char c in txtPhone.Text)
            {
                if (Char.IsNumber(c))
                {
                    phoneCtr++;
                }
            }
            if(phoneCtr!=10)
            {
                MessageBox.Show("Please enter all 10 digits of the phone number");
                txtPhone.Focus();
                txtPhone.SelectAll();
                return false;
            }
            if (txtEmail.Text == "")
            {
                MessageBox.Show("Please enter the contact's email address.");
                txtEmail.Focus();
                return false;
            }
            bool hasAt, hasDecAfterAt, validEmail;
            hasAt = false;
            hasDecAfterAt = false;
            validEmail = true;
            string emailToTest = txtEmail.Text;
            foreach (char c in txtEmail.Text)
            {
                if (hasAt && validEmail && c == '@')
                {
                    validEmail = false;
                }
                if (hasAt && !hasDecAfterAt && c == '.')
                {
                    hasDecAfterAt = true;
                }
                if (!hasAt && c == '@')
                {
                    hasAt = true;
                }
            }
            if ((!Char.IsLetter(emailToTest[emailToTest.Length - 1])) ||
                (!Char.IsNumber(emailToTest[0]) && !Char.IsLetter(emailToTest[0]) && emailToTest[0] != '_') ||
                (!hasAt || !hasDecAfterAt))
            {
                validEmail = false;
            }
            if (!validEmail)
            {
                MessageBox.Show("Please enter a valid email address (example: username@domain.ext).");
                txtEmail.Focus();
                txtEmail.SelectAll();
                return false;
            }
            return true;
        }
        private void SetState(string state)
        {
            // function to enabled/disabled and show/hide controls based on the state of the form
            // ("view", "add", "update", or "delete".

            switch (state)
            {
                case "add":
                    this.Text = "Add Entry";
                    foreach (Control c in this.Controls)
                    {
                        if (c.Name.ToString().ToLower().Contains("txt") || c.Name.ToString().ToLower().Contains("cbx"))
                        {
                            c.Enabled = true;
                            c.Text = String.Empty;
                        }
                    }
                    cbxState.SelectedIndex = -1;
                    btnDelete.Visible = false;
                    btnNext.Visible = false;
                    btnPrevious.Visible = false;
                    btnUpdate.Visible = false;
                    btnAdd.Visible = true;
                    pbxAlter.Image = Image.FromFile(@"C:\Users\Administrator\AppData\Roaming\Jeremy Stones\Graphics\add.png");
                    pbxAlter.Visible = true;
                    pbxAlter2.Image = Image.FromFile(@"C:\Users\Administrator\AppData\Roaming\Jeremy Stones\Graphics\add.png");
                    pbxAlter2.Visible = true;
                    pbxView.Visible = false;
                    mnuEditAdd.Enabled = false;
                    mnuEditUpdate.Enabled = true;
                    mnuEditDelete.Enabled = true;
                    mnuView.Enabled = true;
                    txtFirst.Focus();
                    lblRecordPosition.Text = (Entry.AddressBook.Count + 1).ToString() + " of " + (Entry.AddressBook.Count + 1).ToString();
                    break;
                case "view":
                    this.Text = "View Entry";
                    foreach (Control c in this.Controls)
                    {
                        if (c.Name.ToString().ToLower().Contains("txt") || c.Name.ToString().ToLower().Contains("cbx"))
                        {
                            c.Enabled = false;
                        }
                    }
                    btnDelete.Visible = false;
                    btnNext.Visible = true;
                    btnPrevious.Visible = true;
                    btnUpdate.Visible = false;
                    btnAdd.Visible = false;
                    pbxAlter.Visible = false;
                    pbxAlter2.Visible = false;
                    pbxView.Visible = true;
                    mnuEditAdd.Enabled = true;
                    mnuEditUpdate.Enabled = true;
                    mnuEditDelete.Enabled = true;
                    mnuView.Enabled = false;
                    ShowInfo(_entryPlace);
                    break;
                case "update":
                    this.Text = "Update Entry";
                    foreach (Control c in this.Controls)
                    {
                        if (c.Name.ToString().ToLower().Contains("txt") || c.Name.ToString().ToLower().Contains("cbx"))
                        {
                            c.Enabled = true;
                        }
                    }
                    btnDelete.Visible = false;
                    btnNext.Visible = true;
                    btnPrevious.Visible = true;
                    btnUpdate.Visible = true;
                    btnAdd.Visible = false;
                    pbxAlter.Image = Image.FromFile(@"C:\Users\Administrator\AppData\Roaming\Jeremy Stones\Graphics\edit.png");
                    pbxAlter.Visible = true;
                    pbxAlter2.Image = Image.FromFile(@"C:\Users\Administrator\AppData\Roaming\Jeremy Stones\Graphics\edit.png");
                    pbxAlter2.Visible = true;
                    pbxView.Visible = false;
                    mnuEditAdd.Enabled = true;
                    mnuEditUpdate.Enabled = false;
                    mnuEditDelete.Enabled = true;
                    mnuView.Enabled = true;
                    ShowInfo(_entryPlace);
                    txtFirst.Focus();
                    break;
                case "delete":
                    this.Text = "Delete Entry";
                    foreach (Control c in this.Controls)
                    {
                        if (c.Name.ToString().ToLower().Contains("txt") || c.Name.ToString().ToLower().Contains("cbx"))
                        {
                            c.Enabled = false;
                        }
                    }
                    btnDelete.Visible = true;
                    btnNext.Visible = true;
                    btnPrevious.Visible = true;
                    btnUpdate.Visible = false;
                    btnAdd.Visible = false;
                    pbxAlter.Image = Image.FromFile(@"C:\Users\Administrator\AppData\Roaming\Jeremy Stones\Graphics\delete.png");
                    pbxAlter.Visible = true;
                    pbxAlter2.Image = Image.FromFile(@"C:\Users\Administrator\AppData\Roaming\Jeremy Stones\Graphics\delete.png");
                    pbxAlter2.Visible = true;
                    pbxView.Visible = false;
                    mnuEditAdd.Enabled = true;
                    mnuEditUpdate.Enabled = true;
                    mnuEditDelete.Enabled = false;
                    mnuView.Enabled = true;
                    ShowInfo(_entryPlace);
                    break;
                default:
                    MessageBox.Show("An error has occurred.");
                    break;
            }
        }

        private void mnuEditAdd_Click(object sender, EventArgs e)
        {
            // user selects Add, change state and set it

            _state = "add";
            SetState(_state);
        }

        private void mnuEditUpdate_Click(object sender, EventArgs e)
        {
            // user selects Update, change state and set it
            if (Entry.AddressBook.Count == 0)
            {
                MessageBox.Show("There are no contacts to update.");
                return;
            }
            _state = "update";
            SetState(_state);
        }

        private void mnuEditDelete_Click(object sender, EventArgs e)
        {
            // user selects Delete, change state and set it
            if (Entry.AddressBook.Count == 0)
            {
                MessageBox.Show("There are no contacts to delete.");
                return;
            }
            _state = "delete";
            SetState(_state);
        }

        private void mnuViewView_Click(object sender, EventArgs e)
        {
            // user selects View, change state and set it
            if (Entry.AddressBook.Count == 0)
            {
                MessageBox.Show("There are no contacts to view.");
                return;
            }
            _state = "view";
            SetState(_state);
        }

        private void ShowInfo(int entryNum)
        {
            // sets the form info based on the Entry position of the user
            // also enables/disables the next/previous buttons depending on which entry

            txtFirst.Text = Entry.AddressBook[entryNum].FName;
            txtLast.Text = Entry.AddressBook[entryNum].LName;
            txtStreet.Text = Entry.AddressBook[entryNum].Street;
            txtCity.Text = Entry.AddressBook[entryNum].City;
            cbxState.SelectedItem = Entry.AddressBook[entryNum].State;
            txtZip.Text = Entry.AddressBook[entryNum].Zip;
            txtPhone.Text = Entry.AddressBook[entryNum].Phone;
            txtEmail.Text = Entry.AddressBook[entryNum].Email;
            lblRecordPosition.Text = (_entryPlace + 1).ToString() + " of " + Entry.AddressBook.Count;
            if (_entryPlace == 0)
            {
                btnPrevious.Enabled = false;
            }
            else
            {
                btnPrevious.Enabled = true;
            }
            if (_entryPlace == Entry.AddressBook.Count - 1)
            {
                btnNext.Enabled = false;
            }
            else
            {
                btnNext.Enabled = true;
            }
        }
        private void btnPrevious_Click(object sender, EventArgs e)
        {
            // When user clicks previous, move the entry index forwards and show info

            _entryPlace--;
            ShowInfo(_entryPlace);            
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            // when user clicks Next, move the entry index backwards and show info
            _entryPlace++;
            ShowInfo(_entryPlace);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // ask the user if they want to delete, and if so, delete the current entry
            if(Entry.AddressBook.Count == 0)
            {
                MessageBox.Show("You don't have any contacts to Delete");
                return;
            }

            DialogResult dlg = MessageBox.Show("Are you sure you want to delete the record for " + Entry.AddressBook[_entryPlace].FName
                 + " " + Entry.AddressBook[_entryPlace].LName + "?", "Confirm Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Hand);
            if(dlg == DialogResult.Yes)
            {
                Entry.AddressBook.RemoveAt(_entryPlace);
                if(Entry.AddressBook.Count == 0)
                {
                    mnuEditAdd.PerformClick();
                    return;
                }
                if (_entryPlace == Entry.AddressBook.Count)
                {
                    _entryPlace--;
                }

                ShowInfo(_entryPlace);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // ask the user if they want to add, and if so, add the current entry

            // performs all validation
            if (!_myValidation())
                return;
            DialogResult dlg = MessageBox.Show("Are you sure you want to add the record for " + txtFirst.Text
                 + " " + txtLast.Text + "?", "Confirm Add Record", MessageBoxButtons.YesNo, MessageBoxIcon.Hand);
            if (dlg == DialogResult.Yes)
            {
                string _strFirst = txtFirst.Text;
                string _strLast = txtLast.Text;
                string _strStreet = txtStreet.Text;
                string _strCity = txtCity.Text;
                string _strState = cbxState.SelectedItem.ToString();
                string _strZip = txtZip.Text;
                string _strPhone = txtPhone.Text;
                string _strEmail = txtEmail.Text;
                Entry temp = new Entry(_strFirst, _strLast, _strStreet, _strCity, _strState, _strZip, _strPhone, _strEmail,(_entryPlace +1));
                Entry.AddressBook.Add(temp);
                MessageBox.Show(temp.FName + " " + temp.LName + " was successfully added.");
                _entryPlace = Entry.AddressBook.Count - 1;
                SetState(_state);
            }
        }

        private void mnuFilePrint_Click(object sender, EventArgs e)
        {
            // prints the page directly
            if (Entry.AddressBook.Count == 0)
            {
                MessageBox.Show("You don't have any contacts to print");
                return;
            }
            docPrintRecords.Print();
        }

        private void mnuFilePreview_Click(object sender, EventArgs e)
        {
            // creates a print preview
            if (Entry.AddressBook.Count == 0)
            {
                MessageBox.Show("You don't have any contacts to print");
                return;
            }
            ((Form)dlgPpvRecords).WindowState = FormWindowState.Maximized;
            ((Form)dlgPpvRecords).BackColor = Color.Pink;
            ((Form)dlgPpvRecords).Font = new Font("Times New Roman", 25, FontStyle.Bold);

            if (Entry.AddressBook.Count < 11)
            {
                dlgPpvRecords.PrintPreviewControl.Columns = 1;
            }
            else if (Entry.AddressBook.Count < 23)
            {
                dlgPpvRecords.PrintPreviewControl.Columns = 2;
            }
            else if (Entry.AddressBook.Count < 34)
            {
                dlgPpvRecords.PrintPreviewControl.Columns = 3;
            }
            else if (Entry.AddressBook.Count < 45)
            {
                dlgPpvRecords.PrintPreviewControl.Columns = 2;
                dlgPpvRecords.PrintPreviewControl.Rows = 2;
            }
            else
            {
                dlgPpvRecords.PrintPreviewControl.Columns = 3;
                dlgPpvRecords.PrintPreviewControl.Rows = 2;
            }

            dlgPpvRecords.ShowDialog();
        }

        private void docPrintRecords_PrintPage(object sender, PrintPageEventArgs e)
        {
            // event handler for printPage on the document
            // prints a title and horizontal rule at the top of each page
            // as well as a page number at the bottom
            // then it loads up to 11 entries from the phone book on each page.
            // It customizes the number of pages viewed depending on the number of
            // entires in the phone book.

            if (!_printMore)
            {
                _printerPageCtr = 0;
                _printPageNum = 1;
            }
            // String to print out
            string lineStr;
            // Used to Draw Text Division Horizontal Line
            Pen bluePen = new Pen(Color.DarkBlue, 4);
            // Font definitions
            Font printFontNorm = new Font("Times New Roman", 12);
            Font printFontBold = new Font("Times New Roman", 12, FontStyle.Bold);
            // Used to Draw Table Division Lines
            Pen blackPen = new Pen(Color.Black, 2);
            // Line height definition
            float lineHeight = printFontNorm.GetHeight();
            // Page margin definition
            float xPrnLocation = e.MarginBounds.Left;
            float yPrnLocation = e.MarginBounds.Top;
            // Draw Page Number
            lineStr = "Page " + _printPageNum;
            e.Graphics.DrawString(lineStr, printFontNorm, Brushes.Black, 400, e.MarginBounds.Bottom + 50);

            // Call Draw Title
            yPrnLocation = DrawTitle(e, xPrnLocation, yPrnLocation);
            // Draw a Horizontal Division Line across page
            e.Graphics.DrawLine(bluePen, 60, yPrnLocation, 800, yPrnLocation);

            // Call Address Book Print Procedure



            // Adjust Horizontal AND Vertical to prepare for table print
            xPrnLocation = e.MarginBounds.Left;
            yPrnLocation += lineHeight;
            while (_printerPageCtr < Entry.AddressBook.Count)
            {
                // tests at the beginning of each loop to see if it needs to go
                // to a new page, before printing each addressbook entry to the 
                // document.
                if (yPrnLocation > 950)
                {
                    e.HasMorePages = true;
                    _printMore = true;
                    _printPageNum++;
                    return;
                }
                // Print name (First Last)
                lineStr = "Name: " + Entry.AddressBook[_printerPageCtr].FName + " " + Entry.AddressBook[_printerPageCtr].LName;
                e.Graphics.DrawString(lineStr, printFontBold, Brushes.Black, xPrnLocation, yPrnLocation);
                yPrnLocation += lineHeight;
                xPrnLocation += 25;
                // Print Other Info (Address/Phone/Email)
                lineStr = ("Address: " + Entry.AddressBook[_printerPageCtr].Street).PadRight(70).Substring(0, 70);
                e.Graphics.DrawString(lineStr, printFontNorm, Brushes.Black, xPrnLocation, yPrnLocation);
                lineStr = "Phone: " + Entry.AddressBook[_printerPageCtr].Phone;
                xPrnLocation += 375;
                e.Graphics.DrawString(lineStr, printFontNorm, Brushes.Black, xPrnLocation, yPrnLocation);
                yPrnLocation += lineHeight;
                xPrnLocation -= 375;
                lineStr = ("               " + Entry.AddressBook[_printerPageCtr].City + ", " + Entry.AddressBook[_printerPageCtr].State + " " + Entry.AddressBook[_printerPageCtr].Zip).PadRight(73).Substring(0, 73);
                e.Graphics.DrawString(lineStr, printFontNorm, Brushes.Black, xPrnLocation, yPrnLocation);
                lineStr = "Email: " + Entry.AddressBook[_printerPageCtr].Email;
                xPrnLocation += 375;
                e.Graphics.DrawString(lineStr, printFontNorm, Brushes.Black, xPrnLocation, yPrnLocation);
                yPrnLocation += lineHeight;

                // draw a horizontal divider
                xPrnLocation = e.MarginBounds.Left;
                lineStr = "";
                for (int j = 0; j < 65; j++)
                    lineStr += "- ";
                e.Graphics.DrawString(lineStr, printFontNorm, Brushes.ForestGreen, 80, yPrnLocation);

                // Adjust Vertical Position
                yPrnLocation += lineHeight;
                _printerPageCtr++;
            }
            e.HasMorePages = false;
            _printMore = false;

        }

        private Single DrawTitle(PrintPageEventArgs e, Single xPrnLocation, Single yPrnLocation)
        {
            // unnecessary function to print a title at the top of each document page
            // This could have been directly in the event handler.

            // Font definitions
            Font printFontBig = new Font("Times New Roman", 32, FontStyle.Italic);
            Font printFontNorm = new Font("Times New Roman", 12);

            // Line height variable
            float lineHeight;

            // Print the demo title
            string lineStr = "Address Book Entries";
            xPrnLocation += 120; // center text
            e.Graphics.DrawString(lineStr, printFontBig, Brushes.DarkBlue, xPrnLocation, yPrnLocation);
            lineHeight = printFontNorm.GetHeight();
            yPrnLocation += lineHeight * 3;

            return yPrnLocation;
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            // goes back to frmMain
            AppExit.CloseToNew = true;
            Application.OpenForms[0].Show();
            this.Close();
        }

        private void mnuHelpAbout_Click(object sender, EventArgs e)
        {
            // goes to the about page
            frmAbout frm = new frmAbout();
            frm.ShowDialog();
        }


        private void txtFirstLastCity_KeyPress(object sender, KeyPressEventArgs e)
        {
            // prevents users from entering in unwanted characters while in these textboxes
            // (firstname, lastname, city)
            if (!Char.IsLetter(e.KeyChar) && !Char.IsControl(e.KeyChar) && e.KeyChar != ' '
                && e.KeyChar != '.' && e.KeyChar != '-' && e.KeyChar != '\'')
            {
                e.Handled = true;
            }
        }

        private void txtStreet_KeyPress(object sender, KeyPressEventArgs e)
        {
            // prevents users from entering in unwanted characters while in the street address textbox
            if (!Char.IsLetter(e.KeyChar) && !Char.IsControl(e.KeyChar) && e.KeyChar != ' '
                && e.KeyChar != '.' && e.KeyChar != '-' && e.KeyChar != '\''
                && e.KeyChar != '#' && !Char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtZip_KeyPress(object sender, KeyPressEventArgs e)
        {
            // only allows numbers for the zip
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            // only allows numbers for the phone (now not needed with masked textbox)
            // left in for future changes
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtEmail_KeyPress(object sender, KeyPressEventArgs e)
        {
            // disallows unwanted characters from the email textbox
            if (!Char.IsLetter(e.KeyChar) && !Char.IsControl(e.KeyChar)
                && e.KeyChar != '.' && e.KeyChar != '@' && e.KeyChar != '-'
                && e.KeyChar != '_' && !Char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // ask the user if they want to update, and if so, update the current entry

            // performs all validation
            if (!_myValidation())
                return;

            DialogResult dlg = MessageBox.Show("Are you sure you want to update the record for " + txtFirst.Text
                 + " " + txtLast.Text + "?", "Confirm Add Record", MessageBoxButtons.YesNo, MessageBoxIcon.Hand);
            if (dlg == DialogResult.Yes)
            {
                string _strFirst = txtFirst.Text;
                string _strLast = txtLast.Text;
                string _strStreet = txtStreet.Text;
                string _strCity = txtCity.Text;
                string _strState = cbxState.SelectedItem.ToString();
                string _strZip = txtZip.Text;
                string _strPhone = txtPhone.Text;
                string _strEmail = txtEmail.Text;
                Entry temp = new Entry(_strFirst, _strLast, _strStreet, _strCity, _strState, _strZip, _strPhone, _strEmail,(_entryPlace +1));
                Entry.AddressBook.RemoveAt(_entryPlace);
                Entry.AddressBook.Insert(_entryPlace, temp);
                MessageBox.Show(temp.FName + " " + temp.LName + " was successfully updated.");
                SetState(_state);
            }
        }

        private void mnuSearchSearchLast_Click(object sender, EventArgs e)
        {
            // sets the state of the search form to "last" and creates/shows
            // a new search form, hiding frmMain
            if (Entry.AddressBook.Count == 0)
            {
                MessageBox.Show("There are no contacts to search.");
                return;
            }

            AppExit.CloseToNew = true;
            string _state = "last";
            frmSearch form = new frmSearch(_state);
            form.Show();
            this.Hide();
        }

        private void mnuSearchSearchFirst_Click(object sender, EventArgs e)
        {
            // sets the state of the search form to "first" and creates/shows
            // a new search form, hiding frmMain
            if (Entry.AddressBook.Count == 0)
            {
                MessageBox.Show("There are no contacts to search.");
                return;
            }
            AppExit.CloseToNew = true;
            string _state = "first";
            frmSearch form = new frmSearch(_state);
            form.Show();
            this.Hide();
        }

        private void mnuSearchSearchFull_Click(object sender, EventArgs e)
        {
            // sets the state of the search form to "full" and creates/shows
            // a new search form, hiding frmMain
            if (Entry.AddressBook.Count == 0)
            {
                MessageBox.Show("There are no contacts to search.");
                return;
            }
            AppExit.CloseToNew = true;
            string _state = "full";
            frmSearch form = new frmSearch(_state);
            form.Show();
            this.Hide();
        }

        private void mnuFileHome_Click(object sender, EventArgs e)
        {
            // returns user to frmMain
            AppExit.CloseToNew = true;
            Application.OpenForms[0].Show();
            this.Close();
        }

        private void frmRecords_LocationChanged(object sender, EventArgs e)
        {
            // when the location of the form on the screen changes,
            // this stores the new location in the State class
            Point _badLoc = new Point(0, 0);
            if (this.WindowState != FormWindowState.Maximized)
                State.FormPosition = this.Location;
            if (this.Location == _badLoc)
                this.SetDesktopLocation(State.CenterScreenLocation.X, State.CenterScreenLocation.Y);
        }

        private void frmRecords_Resize(object sender, EventArgs e)
        {
            // when the form is resized, the state is saved in the 
            // State class
            Point _badLoc = new Point(0, 0);
            State.MyState = this.WindowState;
            if (State.MyState != FormWindowState.Maximized)
                this.Location = State.CenterScreenLocation;
            else
                State.FormPosition = State.CenterScreenLocation;
            if (this.Location == _badLoc)
                this.SetDesktopLocation(State.CenterScreenLocation.X, State.CenterScreenLocation.Y);
        }

        private void txtStreet_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            // if user clicks enter, depending on state, click specific button
            if(e.KeyChar == (char)Keys.Enter)
            {
                if(_state == "add")
                {
                    btnAdd.PerformClick();
                }
                else
                {
                    btnUpdate.PerformClick();
                }
            }
        }

        private void txtFirst_Enter(object sender, EventArgs e)
        {
            // if the user tabs into a textbox, this selects all the text in that textbox
            foreach(Control c in this.Controls)
            {
                if (c.Name.Contains("txt"))
                {
                    if (c.Focused)
                    {
                        if (c.Name.Contains("txtPhone"))
                            ((MaskedTextBox)c).SelectAll();
                        else
                            ((TextBox)c).SelectAll();
                    } 
                        
                }
            }
        }

        private void txtFirst_Click(object sender, EventArgs e)
        {
            // if a user clicks on a textbox, this selects all the text in that textbox
            foreach (Control c in this.Controls)
            {
                if (c.Name.Contains("txt"))
                {
                    if (c.Focused)
                    {
                        if (c.Name.Contains("txtPhone"))
                            ((MaskedTextBox)c).SelectAll();
                        else
                            ((TextBox)c).SelectAll();
                    }

                }
            }
        }

        private void mnuEditAlphaLast_Click(object sender, EventArgs e)
        {
            // alphabetizes the address book by last name
            if (Entry.AddressBook.Count == 0)
            {
                MessageBox.Show("There are no contacts to alphabetize.");
                return;
            }
            DialogResult dlg = MessageBox.Show("Are you sure you want to sort the address book alphabetically by LAST NAME?", "Sort", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlg == DialogResult.Yes)
            {
                Entry.AddressBook = Entry.AddressBook.OrderBy(Entry => Entry.LName).ToList();
                for (int i = 0; i < Entry.AddressBook.Count; i++)
                {
                    Entry.AddressBook[i].Num = i + 1;
                }
                MessageBox.Show("Your address book as been reordered.");
            }

        }

        private void mnuEditAlphaFirst_Click(object sender, EventArgs e)
        {
            // alphabetizes the address book by first name
            if (Entry.AddressBook.Count == 0)
            {
                MessageBox.Show("There are no contacts to alphabetize.");
                return;
            }
            DialogResult dlg = MessageBox.Show("Are you sure you want to sort the address book alphabetically by FIRST NAME?", "Sort", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlg == DialogResult.Yes)
            {
                Entry.AddressBook = Entry.AddressBook.OrderBy(Entry => Entry.FName).ToList();
                for (int i = 0; i < Entry.AddressBook.Count; i++)
                {
                    Entry.AddressBook[i].Num = i + 1;
                }
                MessageBox.Show("Your address book as been reordered.");
            }
        }

        private void frmRecords_FormClosing(object sender, FormClosingEventArgs e)
        {
            // find out when closing whether the form is being directed to another page, and
            // if not, then ask the user if they really want to exit entire application.
            // if so, write the contacts to file to save changes and close form
            if (!AppExit.CloseToNew)
            {
                if (e.CloseReason == CloseReason.UserClosing)
                {
                    DialogResult dlg = MessageBox.Show("Are you sure you want to exit?", "Exit Verification", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dlg == DialogResult.No)
                    {
                        e.Cancel = true;
                        return;
                    }
                }
                AppExit.WriteToFile();
            }
        }

        private void frmRecords_FormClosed(object sender, FormClosedEventArgs e)
        {
            // if form closed and not directed to new form, exit application
            if (!AppExit.CloseToNew)
            {
                Application.Exit();
            }
        }

        private void mnuFileExit_Click(object sender, EventArgs e)
        {
            // if exit is selected from the file menu,
            // set var CloseToNew to false and close the form
            // (redirects to formClosing for verification and form
            // closed for application exit)
            AppExit.CloseToNew = false;
            this.Close();
        }
    }
}
