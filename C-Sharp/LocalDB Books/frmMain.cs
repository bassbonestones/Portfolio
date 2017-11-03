//Programmed by: Jeremiah Stones
//Date: 6/13/2017
//Teacher: Kathy Wilganowski
//Class: Advanced C#

//Specification:  This program connects to a Microsoft Access database file and allows the user to interact 
//indirectly with the database.The user may view, add, update or delete books.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StonesJ_Lab11
{
    public partial class frmMain : Form
    {
        // defines main form class

        // variables
        Data data = new Data();
        string mode = "view";
        bool deleting = false;
        bool updating = false;

        public frmMain()
        { // default constructor
            InitializeComponent();
        }

        private void LoadGridView()
        { // sets datagrid view with books datasource
            dgvBooks.DataSource = data.DsBooks.Tables[data.DtBooks];
            dgvBooks.ClearSelection();
        }

        private void FormatGridView()
        { // formats view of DGV
            dgvBooks.AllowUserToResizeColumns = false;
            dgvBooks.AllowUserToResizeRows = false;
            dgvBooks.Cursor = Cursors.Hand;
            dgvBooks.EditMode = DataGridViewEditMode.EditProgrammatically;
            dgvBooks.MultiSelect = false;
            dgvBooks.AllowUserToOrderColumns = false;
            dgvBooks.ReadOnly = true;
            dgvBooks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBooks.AllowUserToAddRows = false;
            dgvBooks.Columns["ISBN"].Visible = false;
            dgvBooks.Columns["CopyDate"].Visible = false;
            dgvBooks.Columns["Publisher"].Visible = false;
            dgvBooks.Columns["Pages"].Visible = false;
            dgvBooks.Columns["SubCode"].Visible = false;
            dgvBooks.Columns["TItle"].Visible = true;
            dgvBooks.Columns["TItle"].Width = 400;
            dgvBooks.Columns["TItle"].HeaderText = "Title";
            dgvBooks.Columns["TItle"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvBooks.Columns["Author"].Visible = true;
            dgvBooks.Columns["Author"].Width = 200;
            if (dgvBooks.Rows.Count < 7)
                dgvBooks.Columns["Author"].Width = 217;
            dgvBooks.Columns["Author"].HeaderText = "Author";
            dgvBooks.Columns["Author"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvBooks.BackgroundColor = Color.White;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            // get the form dimensions and set control positions
            int formWidth = this.Bounds.Width;
            lblAvailSubCode.Location = new Point(formWidth/2 - 460,lblAvailSubCode.Location.Y);
            cbxAvailable.Location = new Point(formWidth / 2 - 260, cbxAvailable.Location.Y);
            gbxInfo.Location = new Point(formWidth / 2 - gbxInfo.Width / 2 - 8, gbxInfo.Location.Y);
            gbxGridView.Location = new Point(formWidth / 2 - gbxGridView.Width / 2 - 8, gbxGridView.Location.Y);
            data.CommandConnection(); // connect command with dataAdapter
            data.LoadDataBooks(); // load dataset/datatable for books
            data.LoadDataSubjects(); // load dataset/datatable for subjects
            LoadGridView(); 
            FormatGridView();
            cbxAvailable.SelectedIndex = 0;
            foreach(DataRow r in data.DsSubjects.Tables["SubjectsTable"].Rows)
            { // fill combo boxes with subject info
                cbxAvailable.Items.Add(r["SubjectCode"].ToString());
                cbxSubjects.Items.Add(r["Subject"].ToString());
            }
        }
        

        private void dgvBooks_SelectionChanged(object sender, EventArgs e)
        {
            // if add mode clear selection each time, because we aren't viewing current books
            if (mode == "add")
                dgvBooks.ClearSelection();
            else
            {
                // otherwise if row is selected and we are programmatically in a place
                // where we desire a selecion change to affect the controls of the form
                // set the controls to the DGV values of the selected row
                if(dgvBooks.SelectedRows.Count > 0 && !deleting && !updating)
                {
                    string s = dgvBooks.CurrentRow.Cells[6].Value.ToString();
                    //MessageBox.Show(s);
                    try
                    {
                        txtISBN.Text = dgvBooks.CurrentRow.Cells[0].Value.ToString();
                        txtTitle.Text = (dgvBooks.CurrentRow.Cells[1].Value.ToString());
                        txtCopyDate.Text = dgvBooks.CurrentRow.Cells[2].Value.ToString();
                        txtAuthor.Text = dgvBooks.CurrentRow.Cells[3].Value.ToString();
                        txtPublisher.Text = dgvBooks.CurrentRow.Cells[4].Value.ToString();
                        txtPages.Text = dgvBooks.CurrentRow.Cells[5].Value.ToString();
                        cbxSubjects.SelectedItem = data.getSubject(s);
                    }
                    catch (Exception ex) { } // if error do nothing
                }
            }
        }

        private void mnuFileClear_Click(object sender, EventArgs e)
        {
            // clear all control values
            updating = true;
            data.LoadDataBooks();
            updating = false;
            LoadGridView();
            FormatGridView();
            dgvBooks.ClearSelection();
            cbxSubjects.SelectedIndex = -1;
            foreach (Control c in gbxInfo.Controls)
            {
                if (c.Name.ToString().Contains("txt"))
                    ((TextBox)c).Text = "";
            }
            cbxAvailable.SelectedIndex = 0;
        }

        private void mnuFileExit_Click(object sender, EventArgs e)
        {
            // proper exit from program
            Application.Exit();
        }

        private void mnuEditView_Click(object sender, EventArgs e)
        {
            // this menu item sets the form to VIEW mode
            // controls are affected for viewing only functionality
            if (mode == "add") // if coming from add to this mode
                mnuFileClear.PerformClick();
            mode = "view";
            cbxSubjects.Enabled = false;
            btnButton.Visible = false;
            mnuEditView.Enabled = false;
            mnuEditAdd.Enabled = true;
            mnuEditUpdate.Enabled = true;
            mnuEditDelete.Enabled = true;
            lblAvailSubCode.Visible = true;
            cbxAvailable.Visible = true;
            foreach (Control c in gbxInfo.Controls)
            {
                if (c.Name.ToString().Contains("txt"))
                {
                    ((TextBox)c).ReadOnly = true;
                    ((TextBox)c).TabStop = false;
                }
            }
        }

        private void mnuEditAdd_Click(object sender, EventArgs e)
        {
            // this menu item sets the form to ADD mode
            // controls are affected for adding only functionality
            mode = "add";
            cbxSubjects.Enabled = true;
            btnButton.Text = "Add";
            btnButton.Visible = true;
            mnuEditView.Enabled = true;
            mnuEditAdd.Enabled = false;
            mnuEditUpdate.Enabled = true;
            mnuEditDelete.Enabled = true;
            lblAvailSubCode.Visible = false;
            cbxAvailable.Visible = false;
            mnuFileClear.PerformClick();
            foreach (Control c in gbxInfo.Controls)
            {
                if (c.Name.ToString().Contains("txt"))
                {
                    ((TextBox)c).ReadOnly = false;
                    ((TextBox)c).TabStop = true;
                }
            }
        }

        private void mnuEditUpdate_Click(object sender, EventArgs e)
        {
            // this menu item sets the form to UPDATE mode
            // controls are affected for updating only functionality
            if (mode == "add")// if coming from add mode 
                mnuFileClear.PerformClick();
            mode = "update";
            cbxSubjects.Enabled = false;
            btnButton.Text = "Update";
            btnButton.Visible = true;
            mnuEditView.Enabled = true;
            mnuEditAdd.Enabled = true;
            mnuEditUpdate.Enabled = false;
            mnuEditDelete.Enabled = true;
            lblAvailSubCode.Visible = true;
            cbxAvailable.Visible = true;
            foreach (Control c in gbxInfo.Controls) 
            {
                if (c.Name.ToString().Contains("txt"))
                {
                    ((TextBox)c).ReadOnly = false;
                    ((TextBox)c).TabStop = true;
                }
            }
        }

        private void mnuEditDelete_Click(object sender, EventArgs e)
        {
            // this menu item sets the form to DELETE mode
            // controls are affected for deleting only functionality
            if (mode == "add") // if coming from add
                mnuFileClear.PerformClick();
            mode = "delete";
            cbxSubjects.Enabled = false;
            btnButton.Text = "Delete";
            btnButton.Visible = true;
            mnuEditView.Enabled = true;
            mnuEditAdd.Enabled = true;
            mnuEditUpdate.Enabled = true;
            mnuEditDelete.Enabled = false;
            lblAvailSubCode.Visible = true;
            cbxAvailable.Visible = true;
            foreach (Control c in gbxInfo.Controls)
            {
                if (c.Name.ToString().Contains("txt"))
                {
                    ((TextBox)c).ReadOnly = true;
                    ((TextBox)c).TabStop = false;
                }
            }
        }

        private void btnButton_Click(object sender, EventArgs e)
        {
            // multi-function button depending on mode of form
            if (mode != "add")
            {
                // if the mode is anything other than add
                if (dgvBooks.SelectedRows.Count < 1)
                { 
                    // if nothing selected in DGV, throw error message
                    MessageBox.Show("Please make sure to select a book from the table first.");
                    return;
                }
            }
            if (mode != "delete")
            {
                // for modes other than delete tests for data validation are performed
                // unnecessary in delete mode, because user has no control
                // also not needed for view, but this button is invisible in view mode
                if(cbxSubjects.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select a subject.");
                    cbxSubjects.Focus();
                    return;
                }
                if(txtISBN.Text == "")
                {
                    MessageBox.Show("Please enter the book's ISBN.");
                    txtISBN.Focus();
                    return;
                }
                if(txtISBN.Text.Length < 8)
                {
                    MessageBox.Show("The ISBN is too short.");
                    txtISBN.Focus();
                    txtISBN.SelectAll();
                    return;
                }
                if(txtISBN.Text.Length > 13)
                {
                    MessageBox.Show("The ISBN is too long.");
                    txtISBN.Focus();
                    txtISBN.SelectAll();
                    return;
                }
                foreach(char c in txtISBN.Text)
                {
                    if(!Char.IsDigit(c) && c != 'X' && c != '-')
                    {
                        MessageBox.Show("The ISBN contains invalid characters. Allowed: 0-9, 'X', and '-'. Invalid character: " + c);
                        txtISBN.Focus();
                        txtISBN.SelectAll();
                        return;
                    }
                }
                if (!Char.IsDigit(txtISBN.Text[0]) || (!Char.IsDigit(txtISBN.Text[txtISBN.Text.Length-1]) && txtISBN.Text[txtISBN.Text.Length - 1] != 'X'))
                {
                    MessageBox.Show("The ISBN must begin with a number and end with either a number or the character, 'X'.");
                    txtISBN.Focus();
                    txtISBN.SelectAll();
                    return;
                }
                if(txtTitle.Text == "")
                {
                    MessageBox.Show("Please enter the book's Title.");
                    txtTitle.Focus();
                    return;
                }
                if(txtCopyDate.Text == "")
                {
                    MessageBox.Show("Please enter the book's copy date.");
                    txtCopyDate.Focus();
                    return;
                }
                if(txtCopyDate.Text.Length != 4)
                {
                    MessageBox.Show("Please enter only the 4-digit year for the copy date.");
                    txtCopyDate.Focus();
                    txtCopyDate.SelectAll();
                    return;
                }
                foreach(char c in txtCopyDate.Text)
                {
                    if(!Char.IsDigit(c))
                    {
                        MessageBox.Show("The copy year must only contain numbers, 0-9. Invalid character: " + c);
                        txtCopyDate.Focus();
                        txtCopyDate.SelectAll();
                        return;
                    }
                }
                DateTime d = DateTime.Now;
                string y = d.Year.ToString();
                if(string.Compare(txtCopyDate.Text,y) > 0)
                {
                    MessageBox.Show("Invalid year entered for copy date. Must be current year [" + y + "] or prior.");
                    txtCopyDate.Focus();
                    txtCopyDate.SelectAll();
                    return;
                }
                if(txtAuthor.Text == "")
                {
                    MessageBox.Show("Please enter the book's author.");
                    txtAuthor.Focus();
                    return;
                }
                foreach(char c in txtAuthor.Text)
                {
                    if(!Char.IsLetter(c) && c != ' ' && c != '-' &&
                        c != '\'' && c != '.' && c != ',' && c != '\"')
                    {
                        MessageBox.Show("Invalid character entered for author: " + c);
                        txtAuthor.Focus();
                        txtAuthor.SelectAll();
                        return;
                    }
                }
                if(txtPublisher.Text == "")
                {
                    MessageBox.Show("Please enter the book's publisher.");
                    txtPublisher.Focus();
                    return;
                }
                foreach (char c in txtPublisher.Text)
                {
                    if (!Char.IsLetter(c) && c != ' ' && c != '-' &&
                        c != '\'' && c != '.' && c != ',' && c != '\"')
                    {
                        MessageBox.Show("Invalid character entered for publisher: " + c);
                        txtPublisher.Focus();
                        txtPublisher.SelectAll();
                        return;
                    }
                }
                if (txtPages.Text == "")
                {
                    MessageBox.Show("Please enter the number of pages for the book.");
                    txtPages.Focus();
                    return;
                }
                foreach(char c in txtPages.Text)
                {
                    if(!Char.IsDigit(c))
                    {
                        MessageBox.Show("Please only enter a number for the pages. Invalid character: " + c);
                        txtPages.Focus();
                        txtPages.SelectAll();
                        return;
                    }
                }
            }
            // after validation set string for subject code
            string s = data.getSubCode(cbxSubjects.SelectedItem.ToString());
            switch (mode)
            {
                // based on mode of form perform specific functionality with the database
                case "add":
                    data.setBook(txtISBN.Text, txtTitle.Text, txtCopyDate.Text, txtAuthor.Text, txtPublisher.Text, txtPages.Text, s);
                    data.addBook();
                    data.LoadDataBooks();
                    LoadGridView();
                    FormatGridView();
                    mnuFileClear.PerformClick();
                    break;
                case "update":
                    updating = true;
                    data.setBook(txtISBN.Text, txtTitle.Text, txtCopyDate.Text, txtAuthor.Text, txtPublisher.Text, txtPages.Text, s);
                    data.updateBook(dgvBooks.CurrentRow.Cells[0].Value.ToString());
                    data.LoadDataBooks();
                    updating = false;
                    LoadGridView();
                    FormatGridView();
                    mnuFileClear.PerformClick();
                    break;
                case "delete":
                    deleting = true;
                    data.setBook(txtISBN.Text, txtTitle.Text, txtCopyDate.Text, txtAuthor.Text, txtPublisher.Text, txtPages.Text, s);
                    data.deleteBook();
                    data.LoadDataBooks();
                    deleting = false;
                    LoadGridView();
                    FormatGridView();
                    mnuFileClear.PerformClick();
                    break;
                default:
                    break;
            }
        }

        private void cbxAvailable_SelectedIndexChanged(object sender, EventArgs e)
        {
            // when available subjects combo changed, if there is a subject selected,
            // update the dataGridView with new info
            if (cbxAvailable.SelectedIndex > 0)
            {
                string code = cbxAvailable.SelectedItem.ToString();
                updating = true;
                data.LoadDataBooks(code);
                updating = false;
                LoadGridView();
                FormatGridView();
            }
            if(cbxAvailable.SelectedIndex == 0)
            {
                // otherwise fill the DGV with all books
                updating = true;
                data.LoadDataBooks();
                updating = false;
                LoadGridView();
                FormatGridView();
            }
        }

        private void cbxSubCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            // if in add mode and the user picks a subject for their book, the DGV
            // is update with books from only that category, allowing the user to see if 
            // that particular book is already in the database
            if(mode == "add" && cbxSubjects.SelectedIndex >=0 )
            {
                string subject = cbxSubjects.SelectedItem.ToString();
                string code = data.getSubCode(subject);
                updating = true;
                data.LoadDataBooks(code);
                updating = false;
                LoadGridView();
                FormatGridView();
            }
        }

        private void mnuHelpAbout_Click(object sender, EventArgs e)
        {
            // display about form modally
            frmAbout about = new frmAbout();
            about.ShowDialog();
        }

        private void frmMain_Resize(object sender, EventArgs e)
        {
            // code to handle resizing the form so that the controls are always in a nice-looking,
            // logical location on the form
            if (this.Width > 1034)
            {
                lblAvailSubCode.Location = new Point(this.Width / 2 - 460, lblAvailSubCode.Location.Y);
                cbxAvailable.Location = new Point(this.Width / 2 - 260, cbxAvailable.Location.Y);
                gbxInfo.Location = new Point(this.Width / 2 - gbxInfo.Width / 2 - 8, gbxInfo.Location.Y);
                gbxGridView.Location = new Point(this.Width / 2 - gbxGridView.Width / 2 - 8, gbxGridView.Location.Y);
            }
            else
            {
                lblAvailSubCode.Location = new Point(50, lblAvailSubCode.Location.Y);
                cbxAvailable.Location = new Point(250, cbxAvailable.Location.Y);
                gbxInfo.Location = new Point(20, gbxInfo.Location.Y);
                gbxGridView.Location = new Point(20, gbxGridView.Location.Y);
            }
        }
        protected override void WndProc(ref Message m)
        {
            // extension of functionality for resizing the form, if the form is maximized, no resize message is 
            // thrown by the operating system, for you program to handle it.  Only be overriding the Windows Procedure,
            // can we account for a maximize or restore event
            base.WndProc(ref m);

            // WM_SYSCOMMAND
            if (m.Msg == 0x0112)
            {
                if (m.WParam == new IntPtr(0xF030) // Maximize event - SC_MAXIMIZE from Winuser.h
                    || m.WParam == new IntPtr(0xF120)) // Restore event - SC_RESTORE from Winuser.h
                {
                    if (this.Width > 1034) // same code as function above resize event
                    {
                        lblAvailSubCode.Location = new Point(this.Width / 2 - 460, lblAvailSubCode.Location.Y);
                        cbxAvailable.Location = new Point(this.Width / 2 - 260, cbxAvailable.Location.Y);
                        gbxInfo.Location = new Point(this.Width / 2 - gbxInfo.Width / 2 - 8, gbxInfo.Location.Y);
                        gbxGridView.Location = new Point(this.Width / 2 - gbxGridView.Width / 2 - 8, gbxGridView.Location.Y);
                    }
                    else
                    {
                        lblAvailSubCode.Location = new Point(50, lblAvailSubCode.Location.Y);
                        cbxAvailable.Location = new Point(250, cbxAvailable.Location.Y);
                        gbxInfo.Location = new Point(20, gbxInfo.Location.Y);
                        gbxGridView.Location = new Point(20, gbxGridView.Location.Y);
                        this.Width = 1034;
                        this.Height = 736;
                        this.CenterToScreen();
                    }
                }
            }
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            // if form closed by any other means, like X, make sure application exits fully
            Application.Exit();
        }
    }
}
