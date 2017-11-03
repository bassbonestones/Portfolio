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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StonesJ_Lab09
{
    public partial class frmMain : Form
    {
        // declare variables
        const string PATH = @"C:\Users\Administrator\AppData\Roaming\Jeremy Stones\TextFiles\AddressBook.csv";
        int printerPageCtr;
        bool printMore;
        int printPageNum;
        public frmMain()
        {
            // constructor, initializes vars for printing
            InitializeComponent();
            printerPageCtr = 0;
            printMore = false;
            printPageNum = 1;
            AppExit.ResetCloseFlag();
        }

        // The following commented-out code disables the ControlBox Close Button.

        //// create const variable for form param of disabled X button

        //private const int CP_NOCLOSE_BUTTON = 0x200;
        //protected override CreateParams CreateParams
        //{
        //    // overrides CreateParams for the the form to disable the X button
        //    get
        //    {
        //        CreateParams myCp = base.CreateParams;
        //        myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
        //        return myCp;
        //    }
        //}

        private void mnuFileExit_Click(object sender, EventArgs e)
        {
            // This click event is for the exit menu item.  If user clicks exit, verify and then exit.
            
            this.Close();
        }

        private void LoadAddressBook()
        {
            // loads the address book from the file into the list
            if (File.Exists(PATH))
            {
                StreamReader sr;
                string[] _strEntry = new string[8];

                sr = File.OpenText(PATH);
                Entry.AddressBook.Clear();
                int ctr = 1;
                while (!sr.EndOfStream)
                {
                    _strEntry = sr.ReadLine().Split(',');
                    if (_strEntry[0].Trim() != "")
                    {
                        Entry.AddressBook.Add(new Entry(_strEntry[0], _strEntry[1], _strEntry[2], _strEntry[3], _strEntry[4], _strEntry[5], _strEntry[6], _strEntry[7], ctr));
                        ctr++;
                    }
                }
                sr.Close();
                MessageBox.Show("Your address book was successfully loaded.");
            }
            else
            {
                try
                {
                    StreamWriter sw;
                    sw = File.CreateText(PATH);
                    sw.Close();
                    MessageBox.Show("Your address book file was not found, so a new one was created.");
                }
                catch(Exception ex)
                {
                    MessageBox.Show("There was an error loading your address book. Make sure the proper directory exists.");
                }
            }
        }

        private void mnuEditAdd_Click(object sender, EventArgs e)
        {
            // when add is selected from the menu strip, moves to the records form
            // and sets the state of the form to "add"
            frmRecords _frmAddRecords = new frmRecords("add");
            _frmAddRecords.Show();
            this.Hide();
        }

        private void mnuViewView_Click(object sender, EventArgs e)
        {
            // when view is selected from the menu strip, moves to the records form
            // and sets the state of the form to "view"
            if (Entry.AddressBook.Count == 0)
            {
                MessageBox.Show("You don't have any contacts to view");
                return;
            }
            frmRecords _frmViewRecords = new frmRecords("view");
            _frmViewRecords.Show();
            this.Hide();
        }

        private void mnuEditUpdate_Click(object sender, EventArgs e)
        {
            // when update is selected from the menu strip, moves to the records form
            // and sets the state of the form to "update"
            if (Entry.AddressBook.Count == 0)
            {
                MessageBox.Show("You don't have any contacts to update");
                return;
            }
            frmRecords _frmUpdateRecords = new frmRecords("update");
            _frmUpdateRecords.Show();
            this.Hide();
        }

        private void mnuEditDelete_Click(object sender, EventArgs e)
        {
            // when delete is selected from the menu strip, moves to the records form
            // and sets the state of the form to "delete"
            if (Entry.AddressBook.Count == 0)
            {
                MessageBox.Show("You don't have any contacts to delete");
                return;
            }
            frmRecords _frmDeleteRecords = new frmRecords("delete");
            _frmDeleteRecords.Show();
            this.Hide();
        }

        private void mnuFilePreview_Click(object sender, EventArgs e)
        {
            // creates a print preview
            if (Entry.AddressBook.Count == 0)
            {
                MessageBox.Show("You don't have any contacts to print");
                return;
            }
            ((Form)dlgPpvMain).WindowState = FormWindowState.Maximized;
            ((Form)dlgPpvMain).BackColor = Color.Pink;
            ((Form)dlgPpvMain).Font = new Font("Times New Roman", 25, FontStyle.Bold);

            if (Entry.AddressBook.Count < 11)
            {
                dlgPpvMain.PrintPreviewControl.Columns = 1;
            }
            else if (Entry.AddressBook.Count < 23)
            {
                dlgPpvMain.PrintPreviewControl.Columns = 2;
            }
            else if (Entry.AddressBook.Count < 34)
            {
                dlgPpvMain.PrintPreviewControl.Columns = 3;
            }
            else if (Entry.AddressBook.Count < 45)
            {
                dlgPpvMain.PrintPreviewControl.Columns = 2;
                dlgPpvMain.PrintPreviewControl.Rows = 2;
            }
            else
            {
                dlgPpvMain.PrintPreviewControl.Columns = 3;
                dlgPpvMain.PrintPreviewControl.Rows = 2;
            }

            dlgPpvMain.ShowDialog();
        }

        private void mnuFilePrint_Click(object sender, EventArgs e)
        {
            // prints the page directly
            if (Entry.AddressBook.Count == 0)
            {
                MessageBox.Show("You don't have any contacts to print");
                return;
            }
            docPrintMain.Print();
        }

        private void docPrintMain_PrintPage(object sender, PrintPageEventArgs e)
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

        private void frmMain_Load(object sender, EventArgs e)
        {
            // called when form loads, this even handler calls the load address book function
            LoadAddressBook();
            State.CenterScreenLocation = this.Location;
        }

        private void mnuHelpAbout_Click(object sender, EventArgs e)
        {
            // shows the about form
            frmAbout frm = new frmAbout();
            frm.ShowDialog();
        }

        private void frmMain_Resize(object sender, EventArgs e)
        {
            // when the form is resized, the size is stored in the State class
            Point _badLoc = new Point(0, 0);
            lblClass.SetBounds(this.Size.Width / 2 - lblClass.Size.Width / 2, this.Size.Height - lblClass.Size.Height * 4, lblClass.Size.Width, lblClass.Size.Height);
            State.MyState = this.WindowState;
            if (State.MyState != FormWindowState.Maximized)
                this.Location = State.CenterScreenLocation;
            else
                State.FormPosition = State.CenterScreenLocation;
            if (this.Location == _badLoc)
                this.SetDesktopLocation(State.CenterScreenLocation.X, State.CenterScreenLocation.Y);
        }

        private void mnuSearchSearchLast_Click(object sender, EventArgs e)
        {
            // sets the state of the search form to "last" and creates/shows
            // a new search form, hiding frmMain
            if (Entry.AddressBook.Count == 0)
            {
                MessageBox.Show("You don't have any contacts to search");
                return;
            }
            string _state = "last";
            frmSearch form = new frmSearch(_state);
            form.Show();
            this.Hide();
        }

        private void mnuSearchSearchFirst_Click(object sender, EventArgs e)
        {
            // sets the state of the search form to "last" and creates/shows
            // a new search form, hiding frmMain
            if (Entry.AddressBook.Count == 0)
            {
                MessageBox.Show("You don't have any contacts to search");
                return;
            }
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
                MessageBox.Show("You don't have any contacts to search");
                return;
            }
            string _state = "full";
            frmSearch form = new frmSearch(_state);
            form.Show();
            this.Hide();
        }

        private void frmMain_Activated(object sender, EventArgs e)
        {
            // when the form is re-activated (moved to form another form), 
            this.WindowState = FormWindowState.Normal;
            this.Location = State.CenterScreenLocation;
        }

        private void frmMain_LocationChanged(object sender, EventArgs e)
        {
            // when the form changes location on the screen, the location is
            // recorded into the State class
            Point _badLoc = new Point(0, 0);
            if (this.WindowState != FormWindowState.Maximized)
            {
                State.FormPosition = this.Location;
                if (this.Location == _badLoc)
                    this.SetDesktopLocation(State.CenterScreenLocation.X, State.CenterScreenLocation.Y);
            }
                
        }

        private void mnuEditAlphaLast_Click(object sender, EventArgs e)
        {
            // alphabetizes the address book by last name
            if (Entry.AddressBook.Count == 0)
            {
                MessageBox.Show("You don't have any contacts to alphabetize");
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
                MessageBox.Show("You don't have any contacts to alphabetize");
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

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            // tests to see how the fomr was closed and if the user closed it on purpose,
            // since this is the main form, go ahead and ask for exit verification and write to file
            if (e.CloseReason == CloseReason.UserClosing)
            {
                DialogResult dlg;
                dlg = MessageBox.Show("Are you sure you want to exit?", "Exit Verification", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dlg == DialogResult.Yes)
                {
                    AppExit.WriteToFile();
                }
                else
                    e.Cancel = true;
            }
            else
            {
                AppExit.WriteToFile();
            }
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            // once form is called, even though it should exit the application,
            // call it explicitly to make sure
            Application.Exit();
        }
    }
}
