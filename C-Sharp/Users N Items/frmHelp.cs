// Jeremy Stones
// Special Topics
// Brad Willingham

// Lab 4
// 10/1/2017

// Program Specification:
// This program is an application to alter a database by allowing users to add a new database user
// or add/update new database user items.  The application user start at the main navigation page 
// where they can choose from various database operations, exiting the program, or viewing the report
// of user items. Users can see more information about how to use this program in the About/Help menu
// option, which displays a help form.


using System;
using System.Windows.Forms;

namespace Stones_Lab04
{
    public partial class frmHelp : Form
    {
        // This form is for displaying a help screen where users can get information about
        // how to use the application.
        public frmHelp()
        {
            // default constructor
            InitializeComponent();
        }

        private void lstHelpTopics_SelectedIndexChanged(object sender, EventArgs e)
        {
            // When the listbox selection changes, this event is fired.  It will update the
            // help label depending on what is selected.
            if(lstHelpTopics.SelectedIndex != -1)
            {
                // If something is actually selected, set the label with the
                // appropriate help info.
                switch(lstHelpTopics.SelectedIndex)
                {
                    case 0: // Home Screen
                        lblHelpInfo.Text = "From the Home Screen you can choose where you want to go " +
                            "in the application. \n\n" +
                            "Choose any of the options from the Navigation menu or the buttons in the " +
                            "middle of the form in order to:\n" + 
                            "        * add a user.\n        * add an item.\n        * update an item.\n" +
                            "        * view the items report.\n        * exit the program.";
                        break;
                    case 1: // Add A User
                        lblHelpInfo.Text = "In 'Add User' mode on the form for updating the database, " + 
                            "you may enter new user's information and press 'Add' to insert this new record " +
                            "into the database.\n\n" + 
                            "Duplicate names are accepted, but the program will warn you first.  Duplicate " + 
                            "usernames will be rejected. There is no specific password policy, but a password is " + 
                            "required. The password confirmation field must match the first password field.";
                        break;
                    case 2: // Add An Item
                        lblHelpInfo.Text = "In 'Add Item' mode on the form for updating the database, " +
                            "you may enter new item's information and press 'Add' to insert this new record " +
                            "into the database.\n\n" +
                            "All item attributes except a picture must be entered before an item can be added.\n\n" + 
                            "Item costs must be valid numbers with no special symbols other than the decimal. Quantities " +
                            "must be whole numbers.";
                        break;
                    case 3: // Update An Item
                        lblHelpInfo.Text = "In 'Update Item' mode on the form for updating the database, " +
                            "you may update an item's information and press 'Update' to change this new record " +
                            "in the database.\n\n" +
                            "All item attributes except a picture must be entered before an item can be added.\n\n" +
                            "Item costs must be valid numbers with no special symbols other than the decimal. Quantities " +
                            "must be whole numbers.";
                        break;
                    case 4: // View Items Report
                        lblHelpInfo.Text = "This form generates a current report organized by users that own items and listing " +
                            "all of their items.";
                        break;
                    case 5: // More Info
                        lblHelpInfo.Text = "This program was written by Jeremiah Stones.\n\n" +
                            "Copyright 2017\n\n" + "TSTC Special Topics Course\n\n" +
                            "Professor: Brad Willingham\n\nThis program is an application to " +
                            "alter a database by allowing users to add a new database user " +
                            "or add/update new database user items.  The application user starts " +
                            "at the main navigation page where they can choose from various database " +
                            "operations, exiting the program, or viewing the report of user items. ";
                        break;
                    default:
                        break;
                }
            }
            else
            {
                // If nothing was selected, prompt the user to select a topic.
                lblHelpInfo.Text = "Select a help topic from the list on the left.";
            }
        }
    }
}
