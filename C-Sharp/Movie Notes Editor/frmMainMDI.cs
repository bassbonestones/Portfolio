// Jeremy Stones
// Advanced C#
// Brad Willingham

// 7/13/2017
// Lab MDI

// Program Specification
// This program allows the user to open multiple text documents, edit them, and save them.  It also allows the user
// to open a single window that holds all of the details of the Movies database we made during the prior lab in class.
// The user can copy, paste, cut, undo and redo the text in the editor windows.  The user can toggle the visibility of
// the toolbar and status strip.  Also, there are settings to affect the way windows are arranged on the form, including
// an option to auto-layout the windows when a window is dragged to the edges of the parent form; sort of like in Windows 10.
// Finally, there is a help menu and about form to guide the user through benefitting as much as possible from 
// this application.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO; // added
using System.Threading;

namespace Stones_LabMDI
{
    public partial class frmMainMDI : Form
    {
        // set up some variables to keep up with child forms
        private int childFormNumber = 0;
        TextBox[] txtNote = new TextBox[1000];
        Form[] childForm = new Form[1000];
        private int activeFormNumber = -1;
        private int numChildForms = 0;
        private frmMovies moviesForm;
        private bool moviesOpen = false;
        private Point mousePoint;
        private bool autoLayout = true;
        Thread t;
        public frmMainMDI()
        {
            // default constructor
            InitializeComponent();
        }

        private void ShowNewForm(object sender, EventArgs e)
        {
            // this method creates a new child form within the MDI parent

            if(childFormNumber == 999)
            {
                // just in case, so array exception isn't raised
                // 999 to account for 1 possible movies form
                MessageBox.Show("You have reached the maximum number of forms.");
                return;
            }
            // create a textbox and set it up for the new form
            txtNote[childFormNumber] = new TextBox();
            txtNote[childFormNumber].Multiline = true;
            txtNote[childFormNumber].WordWrap = true;
            txtNote[childFormNumber].Dock = DockStyle.Fill;
            txtNote[childFormNumber].ScrollBars = ScrollBars.Vertical;
            // create a new child form in the child form array
            childForm[childFormNumber] = new Form();
            childForm[childFormNumber].MdiParent = this;
            childForm[childFormNumber].Tag = childFormNumber.ToString();
            childForm[childFormNumber].Text = "Window " + (childFormNumber+1);
            childForm[childFormNumber].Controls.Add(txtNote[childFormNumber]);
            childForm[childFormNumber].FormClosing += childForm_FormClosing;
            childForm[childFormNumber].Activated += childForm_Activated;
            childForm[childFormNumber].TextChanged += childForm_TextChanged;
            childForm[childFormNumber].ResizeEnd += childForm_ResizeEnd;
            childForm[childFormNumber].Font = new Font("Arial", 12, FontStyle.Regular);
            childForm[childFormNumber].Icon = new Icon("..\\..\\Images\\jsmall.ico");
            childForm[childFormNumber++].Show();
            // update the number of forms counter
            numChildForms++;
            RefreshMenuStripItems();
            // if conditions met, auto-layout
            if (((numChildForms == 1 && moviesOpen) || (numChildForms == 2 && !moviesOpen)) && autoLayout)
                mnuWindowsTileVertical.PerformClick();
        }
        
        private void childForm_ResizeEnd(object sender, EventArgs e)
        {
            // when a form is moved (caught by this event) check for conditions and auto-layout if they are met

            // if there is more than one form open and user has enabled autoLayout, scale becomes true
            bool scale = ((numChildForms > 1 || (numChildForms == 1 && moviesOpen)) && autoLayout);
            // set the current mouse position right after the form finishes moving
            mousePoint = Control.MousePosition;
            // if the mouse if far to the left or right and scale is set to true
            if ((mousePoint.X < 30 || mousePoint.X > this.Width - 30) && scale)
            {
                // tile windows vertically
                mnuWindowsTileVertical.PerformClick();
            }
            // if the mouse is far to the top or bottom and scale is set to true
            else if ((mousePoint.Y < 90 || mousePoint.Y > this.Height - 80) && scale)
            {
                // tile windows horizontally
                mnuWindowsTileHorizontal.PerformClick();
            }
        }
        
        private void childForm_TextChanged(object sender, EventArgs e)
        {
            // this handler activates when I change the text property of one of the childForms
            // it essentially refreshes the child form properties so that the Windows menu
            // will display the correct name of the file being edited by the text form 
            this.ActivateMdiChild(null);
            this.ActivateMdiChild(sender as Form);
        }
        private void childForm_Activated(object sender, EventArgs e)
        {
            // every time a form is re-activated (or activated for the first time)
            // the activeFormNumber is updated with the active form instance ID from the active forms tag property

            Form activeForm = this.ActiveMdiChild;
            if (activeForm.Tag.ToString() == "movies")
            {
                // however, if it's the movies form that's been activated, simply refresh the menu trip items
                // so the user can't access the edit options and cause an exception to be thrown, then end event
                RefreshMenuStripItems();
                return;
            }
            else
            {
                // if normal text childForm, set active form number
                activeFormNumber = int.Parse(activeForm.Tag.ToString());
            }
            RefreshMenuStripItems();
        }

        private void childForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // test before to see if file has been saved beforehand and is current
            if (IsConnectedToFile())
            {
                // if already current and saved, return
                if (IsCurrentVersionOfFile())
                {
                    numChildForms--;
                    // test for reset of childFormNumber and activeFormNumber, as well as menuItems
                    RefreshCounters();
                    RefreshMenuStripItems();
                    return;
                }
            }
            else
            {
                // if not associated with file and blank, return
                if (txtNote[activeFormNumber].Text.Trim() == "")
                {
                    numChildForms--;
                    // test for reset of childFormNumber and activeFormNumber, as well as menuItems
                    RefreshCounters();
                    RefreshMenuStripItems();
                    return;
                }
            }
            // ask the user if they want to save
            DialogResult dlg = MessageBox.Show("Would you like to save the form before closing?", "Unsaved Work", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            if(dlg == DialogResult.Yes)
            {
                // open a save dialog and save text to file
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                // set initial directory of save dialog
                saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                // set up file filter for dialog
                saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    // user has clicked ok, save file with StreamWriter
                    string FileName = saveFileDialog.FileName;
                    string localFileName = FileName.Substring(FileName.LastIndexOf('\\') + 1);
                    string extention = localFileName.Substring(localFileName.Length - 3);
                    StreamWriter file;
                    string activeText = txtNote[activeFormNumber].Text;
                    file = File.CreateText(FileName);
                    file.WriteLine(activeText);
                    file.Close();
                    // show success message in status
                    try
                    {
                        // first abort previous thread, so it doesn't delete message of new notification thread
                        t.Abort();
                    }
                    catch (Exception) { }
                    t = new Thread(new ParameterizedThreadStart(NotificationMessage));
                    t.Start("File Successfully Saved");
                    // update the number of child forms open
                    numChildForms--;
                    return;
                }
                else
                {
                    // if user hits cancel while in save file dialog, cancel the close event
                    e.Cancel = true;
                }
            }
            else if(dlg == DialogResult.Cancel)
            {
                // if user hits cancel, they do not want to actually close the form at all
                // and the action is cancelled
                e.Cancel = true;
            }
            else
            {
                // user selected no, closes without saving
                numChildForms--;
            }
            // test for reset of childFormNumber and activeFormNumber
            RefreshCounters();
            RefreshMenuStripItems();
            // show not saved
            try
            {
                // first abort previous thread, so it doesn't delete message of new notification thread
                t.Abort();
            }
            catch (Exception) { }
            t = new Thread(new ParameterizedThreadStart(NotificationMessage));
            t.Start("Text Not Saved");
        }

        private void RefreshCounters()
        {
            // method to reset the counters when there are no more forms open, that way when a new form is created,
            // it will display 'Window 1' again.  This method is called each time a child form is closed and the numChildForms
            // counter is decreased.
            if (numChildForms == 0)
            {
                childFormNumber = 0;
                activeFormNumber = -1; // arbitrary invalid number to show invalid state
            }
        }
        private void OpenFile(object sender, EventArgs e)
        {
            // method for the user to pick a text file to open in this application for editing
            
            // create an openFileDialog and let the user pick a file
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|Comma-Separated Values Files (*.csv)|*.csv|Data File (*.dat)|*.dat|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                // if a file was successfully picked perform some string operations to extract useful substrings
                string FileName = openFileDialog.FileName;
                string localFileName = FileName.Substring(FileName.LastIndexOf('\\') + 1);
                string extention = localFileName.Substring(localFileName.Length - 3);
                // only accept valid extensions
                if(extention != "txt" && extention != "csv" && extention != "dat")
                {
                    MessageBox.Show("Unrecognized File Type");
                    return;
                }
                mnuFileNew.PerformClick();
                // set form's text property to the local filename and the tag to the full path
                childForm[activeFormNumber].Text = localFileName;
                txtNote[activeFormNumber].Tag = FileName;
                // open a file reader and set the text of the window's text box to the file contents
                StreamReader fileReader;
                try
                {
                    // set file contents to form textbox
                    fileReader = File.OpenText(FileName);
                    txtNote[activeFormNumber].Text = fileReader.ReadToEnd();
                    fileReader.Close();
                }
                catch (Exception)
                {
                    MessageBox.Show("File Read Error");
                }
            }
        }

        private void mnuFileSaveAs_Click(object sender, EventArgs e)
        {
            // lets the user pick a filename to save their text editing handywork as

            // return if there are no open child forms
            if (numChildForms == 0)
                return;
            // otherwise open a savefiledialog and let the use pick a file location and name
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|Comma-Separated Values Files (*.csv)|*.csv|Data File (*.dat)|*.dat|All Files (*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                // if a file name was picked and OK was pressed perform useful string operations to get filename parts
                string FileName = saveFileDialog.FileName;
                string localFileName = FileName.Substring(FileName.LastIndexOf('\\') + 1);
                string extention = localFileName.Substring(localFileName.Length - 3);
                StreamWriter file;
                // use streamWriter to write to or write over contents of chosen filename
                string activeText = txtNote[activeFormNumber].Text;
                file = File.CreateText(FileName);
                file.WriteLine(activeText);
                file.Close();
                // set form's text property to the local filename and the tag to the full path
                childForm[activeFormNumber].Text = localFileName;
                txtNote[activeFormNumber].Tag = FileName;
                // show saved message as status
                try
                {
                    // first abort previous thread, so it doesn't delete message of new notification thread
                    t.Abort();
                }
                catch (Exception) { }
                t = new Thread(new ParameterizedThreadStart(NotificationMessage));
                t.Start("File Successfully Saved");
            }
        }

        private void mnuFileExit_Click(object sender, EventArgs e)
        {
            // when user clicks exit, properly exit program
            // note: the form closing event will be triggered for any unsaved windows
            Application.Exit();
        }

        private void mnuEditCut_Click(object sender, EventArgs e)
        {
            // when the user clicks cut, call cut on the currently active form's text box
            txtNote[activeFormNumber].Cut();
            // show clipboard message
            try
            {
                // first abort previous thread, so it doesn't delete message of new notification thread
                t.Abort();
            }
            catch (Exception) { }
            t = new Thread(new ParameterizedThreadStart(NotificationMessage));
            t.Start("Cut Successful: Text Saved To Clipboard");
        }

        private void mnuEditCopy_Click(object sender, EventArgs e)
        {
            // when the user clicks copy, call copy on the currently active form's text box
            txtNote[activeFormNumber].Copy();
            // show clipboard message
            try
            {
                // first abort previous thread, so it doesn't delete message of new notification thread
                t.Abort();
            }
            catch (Exception) { }
            t = new Thread(new ParameterizedThreadStart(NotificationMessage));
            t.Start("Copy Successful: Text Saved To Clipboard");
        }

        private void mnuEditPaste_Click(object sender, EventArgs e)
        {
            // when the user clicks paste, call paste on the currently active form's text box 
            txtNote[activeFormNumber].Paste();
        }
        private void mnuEditUndo_Click(object sender, EventArgs e)
        {
            // when the user clicks undo, call undo on the currently active form's text box
            txtNote[activeFormNumber].Undo();
        }

        private void mnuEditRedo_Click(object sender, EventArgs e)
        {
            // when the user clicks redo, call undo (this sucks) on the currently active form's text box
            txtNote[activeFormNumber].Undo();
        }
        private void mnuEditSelectAll_Click(object sender, EventArgs e)
        {
            // when user clicks select all, call select all on the currently active form's text box
            txtNote[activeFormNumber].SelectAll();
        }

        private void mnuViewToolbar_Click(object sender, EventArgs e)
        {
            // change visibility of the tool strip when user clicks this menu option
            toolMain.Visible = mnuViewToolbar.Checked;
        }

        private void mnuViewStatusBar_Click(object sender, EventArgs e)
        {
            // change visibility of the status strip when user clicks this menu option
            stsMain.Visible = mnuViewStatusBar.Checked;
        }

        private void mnuWindowsCascade_Click(object sender, EventArgs e)
        {
            // set the MDI layout to cascade if user selects this layout
            LayoutMdi(MdiLayout.Cascade);
        }

        private void mnuWindowsTileVertical_Click(object sender, EventArgs e)
        {
            // set the MDI layout to tile vertical if user selects this layout
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void mnuWindowsTileHorizontal_Click(object sender, EventArgs e)
        {
            // set the MDI layout to tile horizontal if user selects this layout
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void mnuWindowsArrangeIcons_Click(object sender, EventArgs e)
        {
            // set the MDI layout to arrange icons if user selects this layout
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void mnuWindowsCloseAll_Click(object sender, EventArgs e)
        {
            // when a user has clicked the option to close all child forms
            foreach (Form childForm in MdiChildren)
            {
                // loop through all child forms and close them
                childForm.Close();
            }
        }

       

        private void frmMainMDI_Load(object sender, EventArgs e)
        {
            // when the form loads, set the status label on the status strip to the current dateTime
            toolStsLblNow.Text = DateTime.Now.ToString();
            // go ahead and load dataset from database
            DBOps.getMovies();
            // send status Welcome -- so it will disappear instead of sit there forever
            try
            {
                // first abort previous thread, so it doesn't delete message of new notification thread
                t.Abort();
            }
            catch (Exception) { }
            t = new Thread(new ParameterizedThreadStart(NotificationMessage));
            t.Start("Welcome!");
        }

        private void mnuFileSave_Click(object sender, EventArgs e)
        {
            // even to save file if users clicks save menu item or button

            // return if there are no open child forms
            if (numChildForms == 0)
                return;
            // if there has already been a save
            if(IsConnectedToFile())
            {
                // get text box tag, where a file name would be stored
                string windowTag = txtNote[activeFormNumber].Tag.ToString();
                // open a new streamWriter and save over that particular file with 
                // the current text of the active form's text box
                StreamWriter fileWriter;
                string text = txtNote[activeFormNumber].Text;
                fileWriter = File.CreateText(windowTag);
                fileWriter.WriteLine(text);
                fileWriter.Close();
                // show status of updated success
                try
                {
                    // first abort previous thread, so it doesn't delete message of new notification thread
                    t.Abort();
                }
                catch (Exception) { }
                t = new Thread(new ParameterizedThreadStart(NotificationMessage));
                t.Start("File Successfully Updated");
            }
            else
            {
                // if there is not file association already, send the user to the save as event
                mnuFileSaveAs.PerformClick();
            }
        }
        private bool IsConnectedToFile()
        {
            // to test if there is already a save for current child window
           
            // get window text, where a file name would be stored
            string windowText = childForm[activeFormNumber].Text;
            // if the window text has an associated file type in it return true, otherwise false
            if (windowText.Contains(".txt") || windowText.Contains(".csv") || windowText.Contains(".dat"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool IsCurrentVersionOfFile()
        {
            // method to test if the current child form's text box reflects the saved
            // text in its associated file

            // Note: this method does not test for IsConnectedToFile, because that method will
            // and must be used first to check for a file association before testing for current version
            
            // this method is ONLY for the form closing event, so that the user will not be bothered with a 
            // file save question if the current text is already saved

            // create variables to hold text of the textbox and the file
            string activeText = txtNote[activeFormNumber].Text.Trim();
            string fileText;
            //get filename from form's tag property
            string fileName = txtNote[activeFormNumber].Tag.ToString();
            // create and open stream reader with current file
            StreamReader fileReader;
            fileReader = File.OpenText(fileName);
            fileText = fileReader.ReadToEnd().Trim();
            // the match test and return value
            if (activeText == fileText)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void RefreshMenuStripItems()
        {
            // event to reset the menu items, dependent upon the number of forms

            // if no child forms open OR movies form is active
            if(numChildForms == 0  || this.ActiveMdiChild.Tag.ToString() == "movies")
            {
                // go through all menu strip items, test for the "ChildNeeded" tag,
                // and deactivate any with that tag
                mnuEdit.Enabled = false;
                mnuFileSave.Enabled = false;
                mnuFileSaveAs.Enabled = false;
                tlbSave.Enabled = false;
                // doesn't execute if movies form is active, so we can still access windows menu options
                if (this.ActiveMdiChild.Tag.ToString() != "movies")
                {
                    mnuWindowsArrangeIcons.Enabled = false;
                    mnuWindowsCascade.Enabled = false;
                    mnuWindowsCloseAll.Enabled = false;
                    mnuWindowsTileHorizontal.Enabled = false;
                    mnuWindowsTileVertical.Enabled = false;
                }
                tlbSave.Enabled = false;
            }
            // if child forms exist and movies form not active
            else
            {
                // go through all menu strip items and activate them
                mnuEdit.Enabled = true;
                mnuWindowsArrangeIcons.Enabled = true;
                mnuWindowsCascade.Enabled = true;
                mnuWindowsCloseAll.Enabled = true;
                mnuWindowsTileHorizontal.Enabled = true;
                mnuWindowsTileVertical.Enabled = true;
                mnuFileSave.Enabled = true;
                mnuFileSaveAs.Enabled = true;
                tlbSave.Enabled = true;
            }
            // toggle menu icons and buttons affected by the movies form being open
            if(moviesOpen)
            {
                mnuMoviesView.Enabled = false;
                tlbMovies.Enabled = false;
            }
            else
            {
                mnuMoviesView.Enabled = true;
                tlbMovies.Enabled = true;
            }
        }

        private void tlbHelp_Click(object sender, EventArgs e)
        {
            // direct user to help "contents" menu item if they hit the help button
            mnuHelpContents.PerformClick();
        }

        private void mnuHelpContents_Click(object sender, EventArgs e)
        {
            // opens up a modal help form
            using (frmHelp helpForm = new frmHelp())
            {
                helpForm.ShowDialog();
            }
        }

        private void mnuHelpAbout_Click(object sender, EventArgs e)
        {
            // opens up a modal about form
            using (frmAbout aboutForm = new frmAbout())
            {
                aboutForm.ShowDialog();
            }
        }

        private void mnuMoviesView_Click(object sender, EventArgs e)
        {
            // create a new movies form and show it - same basic operations as childForm above
            moviesForm = new frmMovies();
            moviesForm.MdiParent = this;
            moviesForm.Tag = "movies";
            moviesForm.Text = "Movies Database Details";
            moviesForm.Activated += childForm_Activated;
            moviesForm.TextChanged += childForm_TextChanged;
            moviesForm.FormClosing += moviesForm_Closing;
            moviesForm.ResizeEnd += childForm_ResizeEnd;
            moviesForm.Font = new Font("Arial", 12, FontStyle.Regular);
            moviesForm.Icon = new Icon("..\\..\\Images\\jsmall.ico");
            moviesForm.Show();
            mnuMoviesView.Enabled = false;
            tlbMovies.Enabled = false;
            moviesOpen = true;
            // If the movie form is the only form open now, make it fill the screen.
            if (numChildForms == 0 && autoLayout)
                moviesForm.WindowState = FormWindowState.Maximized;
            // if only the movie form and one other child text form are open, auto-align vertical
            if (numChildForms == 1 && autoLayout)
                mnuWindowsTileVertical.PerformClick();
        }
        private void moviesForm_Closing(object sender, FormClosingEventArgs e)
        {
            // when the movies form is closing, enable button and menu option
            // and activate first form in set of child forms
            mnuMoviesView.Enabled = true;
            tlbMovies.Enabled = true;
            moviesOpen = false;
            // ensure that one of the child forms has focus so subquent actions will be directed toward something
            if (numChildForms > 0)
                this.MdiChildren[0].Focus();
        }

        private void tlbMovies_Click(object sender, EventArgs e)
        {
            // if user clicks movies icon, send them to mnuMoviesView option
            mnuMoviesView.PerformClick();
        }

        private void mnuWindowsAutoLayout_Click(object sender, EventArgs e)
        {
            // toggle auto-layout option that snaps the forms to alignment depending on drag position
            autoLayout = mnuWindowsAutoLayout.Checked;
            // status notification of autoLayout toggle
            try
            {
                // first abort previous thread, so it doesn't delete message of new notification thread
                t.Abort();
            }
            catch (Exception) { }
            t = new Thread(new ParameterizedThreadStart(NotificationMessage));
            t.Start("Auto-Layout toggled " + ((autoLayout) ? "ON" : "OFF"));
        }

        private void NotificationMessage(object message)
        {
            // thread method for displaying message in status strip

            toolStsLblNotification.Text = message.ToString();
            Thread.Sleep(4000);
            toolStsLblNotification.Text = "";
        }
    }
}
