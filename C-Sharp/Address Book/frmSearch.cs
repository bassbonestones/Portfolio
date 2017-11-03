//Jeremiah Stones 5/15/2017
//ITSE 2338
//Lab09, Address Book Part 2 of 2

//See Program Specification in Specification Folder or See About During Runtime.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StonesJ_Lab09
{
    public partial class frmSearch : Form
    {
        // declare variables
        int _pos;
        const string PATH = @"C:\Users\Administrator\AppData\Roaming\Jeremy Stones\TextFiles\AddressBook.csv";
        int printerPageCtr;
        bool printMore;
        int printPageNum;
        string _state;

        public frmSearch(string searchBy)
        {
            // constructor, initializes _pos to first record and sets state of search
            InitializeComponent();
            _state = searchBy;
            _pos = 0;
            AppExit.ResetCloseFlag();
        }

        // constant referring to a parameter for the form
        // for disabled exit button
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

        private void mnuHelpAbout_Click(object sender, EventArgs e)
        {
            // displays the about form to the user
            AppExit.CloseToNew = true;
            frmAbout frm = new frmAbout();
            frm.ShowDialog();
        }

        private void mnuViewView_Click(object sender, EventArgs e)
        {
            // when view is selected from the menu strip, moves to the records form
            // and sets the state of the form to "view"
            AppExit.CloseToNew = true;
            frmRecords _frmAddRecords = new frmRecords("view", _pos);
            _frmAddRecords.Show();
            this.Hide();
        }

        private void mnuEditAdd_Click(object sender, EventArgs e)
        {
            // when add is selected from the menu strip, moves to the records form
            // and sets the state of the form to "add"
            AppExit.CloseToNew = true;
            frmRecords _frmAddRecords = new frmRecords("add");
            _frmAddRecords.Show();
            this.Hide();
        }

        private void mnuEditUpdate_Click(object sender, EventArgs e)
        {
            // when update is selected from the menu strip, moves to the records form
            // and sets the state of the form to "update"

            AppExit.CloseToNew = true;
            frmRecords _frmAddRecords = new frmRecords("update", _pos);
            _frmAddRecords.Show();
            this.Hide();
        }

        private void mnuEditDelete_Click(object sender, EventArgs e)
        {
            // when delete is selected from the menu strip, moves to the records form
            // and sets the state of the form to "delete"

            AppExit.CloseToNew = true;
            frmRecords _frmAddRecords = new frmRecords("delete", _pos);
            _frmAddRecords.Show();
            this.Hide();
        }

        private void mnuFilePrint_Click(object sender, EventArgs e)
        {
            // prints the page directly

            docPrintSearch.Print();
        }

        private void mnuFilePreview_Click(object sender, EventArgs e)
        {
            // creates a print preview
            ((Form)dlgPpvSearch).WindowState = FormWindowState.Maximized;
            ((Form)dlgPpvSearch).BackColor = Color.Pink;
            ((Form)dlgPpvSearch).Font = new Font("Times New Roman", 25, FontStyle.Bold);

            if (Entry.AddressBook.Count < 11)
            {
                dlgPpvSearch.PrintPreviewControl.Columns = 1;
            }
            else if (Entry.AddressBook.Count < 23)
            {
                dlgPpvSearch.PrintPreviewControl.Columns = 2;
            }
            else if (Entry.AddressBook.Count < 34)
            {
                dlgPpvSearch.PrintPreviewControl.Columns = 3;
            }
            else if (Entry.AddressBook.Count < 45)
            {
                dlgPpvSearch.PrintPreviewControl.Columns = 2;
                dlgPpvSearch.PrintPreviewControl.Rows = 2;
            }
            else
            {
                dlgPpvSearch.PrintPreviewControl.Columns = 3;
                dlgPpvSearch.PrintPreviewControl.Rows = 2;
            }

            dlgPpvSearch.ShowDialog();
        }
        
        private void docPrintSearch_PrintPage(object sender, PrintPageEventArgs e)
        {
            // event handler for print page
            // draws title and horizontal rule to each page and draws
            // all of the contacts onto the document
            if (!printMore)
            {
                printerPageCtr = 0;
                printPageNum = 1;
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
            lineStr = "Page " + printPageNum;
            e.Graphics.DrawString(lineStr, printFontNorm, Brushes.Black, 400, e.MarginBounds.Bottom + 50);

            // Call Draw Title
            yPrnLocation = DrawTitle(e, xPrnLocation, yPrnLocation);
            // Draw a Horizontal Division Line across page
            e.Graphics.DrawLine(bluePen, 60, yPrnLocation, 800, yPrnLocation);

            // Call Address Book Print Procedure



            // Adjust Horizontal AND Vertical to prepare for table print
            xPrnLocation = e.MarginBounds.Left;
            yPrnLocation += lineHeight;
            while (printerPageCtr < Entry.AddressBook.Count)
            {
                if (yPrnLocation > 950)
                {
                    e.HasMorePages = true;
                    printMore = true;
                    printPageNum++;
                    return;
                }
                // Print name (First Last)
                lineStr = "Name: " + Entry.AddressBook[printerPageCtr].FName + " " + Entry.AddressBook[printerPageCtr].LName;
                e.Graphics.DrawString(lineStr, printFontBold, Brushes.Black, xPrnLocation, yPrnLocation);
                yPrnLocation += lineHeight;
                xPrnLocation += 25;
                // Print Other Info (Address/Phone/Email)
                lineStr = ("Address: " + Entry.AddressBook[printerPageCtr].Street).PadRight(70).Substring(0, 70);
                e.Graphics.DrawString(lineStr, printFontNorm, Brushes.Black, xPrnLocation, yPrnLocation);
                lineStr = "Phone: " + Entry.AddressBook[printerPageCtr].Phone;
                xPrnLocation += 375;
                e.Graphics.DrawString(lineStr, printFontNorm, Brushes.Black, xPrnLocation, yPrnLocation);
                yPrnLocation += lineHeight;
                xPrnLocation -= 375;
                lineStr = ("               " + Entry.AddressBook[printerPageCtr].City + ", " + Entry.AddressBook[printerPageCtr].State + " " + Entry.AddressBook[printerPageCtr].Zip).PadRight(73).Substring(0, 73);
                e.Graphics.DrawString(lineStr, printFontNorm, Brushes.Black, xPrnLocation, yPrnLocation);
                lineStr = "Email: " + Entry.AddressBook[printerPageCtr].Email;
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
                printerPageCtr++;
            }
            e.HasMorePages = false;
            printMore = false;

        }

        private Single DrawTitle(PrintPageEventArgs e, Single xPrnLocation, Single yPrnLocation)
        {
            // draws title to the document

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

        private void frmSearch_Load(object sender, EventArgs e)
        {
            _setState(_state);
            this.WindowState = State.MyState;
            AppExit.ResetCloseFlag();
            if (this.WindowState != FormWindowState.Maximized)
                this.SetDesktopLocation(State.FormPosition.X, State.FormPosition.Y);
        }

        private void _setState(string state)
        {
            // changes formatting on the form based on how the user wants
            // to search (first/last/full name)
            switch(state)
            {
                case "last":
                    lblLast.ForeColor = Color.BlueViolet;
                    lblLast.Font = new Font(lblLast.Font, FontStyle.Bold);
                    lblFirst.ForeColor = SystemColors.HotTrack;
                    lblFirst.Font = new Font(lblLast.Font, FontStyle.Regular);
                    lblFull.ForeColor = SystemColors.HotTrack;
                    lblFull.Font = new Font(lblLast.Font, FontStyle.Regular);
                    break;
                case "first":
                    lblFirst.ForeColor = Color.BlueViolet;
                    lblFirst.Font = new Font(lblLast.Font, FontStyle.Bold);
                    lblLast.ForeColor = SystemColors.HotTrack;
                    lblLast.Font = new Font(lblLast.Font, FontStyle.Regular);
                    lblFull.ForeColor = SystemColors.HotTrack;
                    lblFull.Font = new Font(lblLast.Font, FontStyle.Regular);
                    break;
                case "full":
                    lblFull.ForeColor = Color.BlueViolet;
                    lblFull.Font = new Font(lblLast.Font, FontStyle.Bold);
                    lblFirst.ForeColor = SystemColors.HotTrack;
                    lblFirst.Font = new Font(lblLast.Font, FontStyle.Regular);
                    lblLast.ForeColor = SystemColors.HotTrack;
                    lblLast.Font = new Font(lblLast.Font, FontStyle.Regular);
                    break;
                default:
                    MessageBox.Show("An Error Has Occurred.");
                    break;
            }
        }

        private void lblLast_Click(object sender, EventArgs e)
        {
            // changes the state to search by last name
            _state = "last";
            _setState(_state);
        }

        private void lblFirst_Click(object sender, EventArgs e)
        {
            // changes the state to search by first name
            _state = "first";
            _setState(_state);
        }

        private void lblFull_Click(object sender, EventArgs e)
        {
            // changes the state to search by full name
            _state = "full";
            _setState(_state);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            // when the user clicks the search button, it searches through
            // the address book for entries that contain the search string
            if(txtSearch.Text.Trim() == "")
            {
                MessageBox.Show("Please enter your search first.");
                return;
            }
            lstSearch.Items.Clear();
            string _listItem;
            if(_state == "full")
            {
                foreach(Entry ent in Entry.AddressBook)
                {
                    if((ent.FName.Trim().ToUpper() + " " + ent.LName.Trim().ToUpper()).Contains(txtSearch.Text.Trim().ToUpper()) || 
                        (ent.LName.Trim().ToUpper() + " " + ent.FName.Trim().ToUpper()).Contains(txtSearch.Text.Trim().ToUpper()))
                    {
                        _listItem = _makeListItem(ent);
                        lstSearch.Items.Add(_listItem);
                    }
                }
            }
            else if(_state == "last")
            {
                foreach(Entry ent in Entry.AddressBook)
                {
                    if(ent.LName.Trim().ToUpper().Contains(txtSearch.Text.Trim().ToUpper()))
                    {
                        _listItem = _makeListItem(ent);
                        lstSearch.Items.Add(_listItem);
                    }
                }
            }
            else // if _state == "first" -- only other condition possible
            {
                foreach(Entry ent in Entry.AddressBook)
                {
                    if(ent.FName.Trim().ToUpper().Contains(txtSearch.Text.Trim().ToUpper()))
                    {
                        _listItem = _makeListItem(ent);
                        lstSearch.Items.Add(_listItem);
                    }
                }
            }
            if(lstSearch.Items.Count == 0)
            {
                lstSearch.Items.Add("No matching items found...");
            }
        }

        private string _makeListItem(Entry ent)
        {
            // function to create a string that can be passed into the search results list
            string s;
            if(ent.Num < 10)
            {
                s = "0" + ent.Num.ToString();
            }
            else
            {
                s = ent.Num.ToString();
            }
            return (" " + s.PadRight(6) + "| " + ent.LName.PadRight(23).Substring(0,23) + "| " + ent.FName);
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            // moves user to the records form with the form state set
            // to "view"
            if (lstSearch.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a record first.");
                return;
            }
            else
            {
                frmRecords form = new frmRecords("view", int.Parse(lstSearch.SelectedItem.ToString().Substring(1, 2)) - 1);
                form.Show();
                AppExit.CloseToNew = true;
                this.Close();
            } 
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // moves user to the records form with the form state set
            // to "update"
            if (lstSearch.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a record first.");
                return;
            }
            else
            {
                frmRecords form = new frmRecords("update", int.Parse(lstSearch.SelectedItem.ToString().Substring(1, 2)) - 1);
                form.Show();
                AppExit.CloseToNew = true;
                this.Close();
            }
        }

        private void mnuFileHome_Click(object sender, EventArgs e)
        {
            // returns user to frmMain
            AppExit.CloseToNew = true;
            Application.OpenForms[0].Show();
            this.Close();
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            // if user presses ENTER while the search textbox has the focus
            // the search button will be clicked
            if(e.KeyChar== (char)Keys.Enter)
            {
                btnSearch.PerformClick();
            }
        }

        private void frmSearch_Resize(object sender, EventArgs e)
        {
            // if the form WindowState is changed it 
            // will be stored in the State class
            Point _badLoc = new Point(0, 0);
            State.MyState = this.WindowState;
            if (State.MyState != FormWindowState.Maximized)
                this.Location = State.CenterScreenLocation;
            else
                State.FormPosition = State.CenterScreenLocation;
            if (this.Location == _badLoc)
                this.SetDesktopLocation(State.CenterScreenLocation.X, State.CenterScreenLocation.Y);
        }

        private void frmSearch_LocationChanged(object sender, EventArgs e)
        {
            // if the form's position on the screen is 
            // changed it will be stored in the State class
            Point _badLoc = new Point(0, 0);
            if (this.WindowState != FormWindowState.Maximized)
                State.FormPosition = this.Location;
            if (this.Location == _badLoc)
                this.SetDesktopLocation(State.CenterScreenLocation.X, State.CenterScreenLocation.Y);
        }

        private void mnuEditAlphaLast_Click(object sender, EventArgs e)
        {
            // alphabetizes the address book by last name
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

        private void frmSearch_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!AppExit.CloseToNew)
            {
                if(e.CloseReason == CloseReason.UserClosing)
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

        private void frmSearch_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!AppExit.CloseToNew)
            {
                Application.Exit();
            }
        }

        private void mnuFileExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
