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
    public partial class frmMain : Form
    {
        // This form is the starting screen for the application and acts as a navigation screen.

        public frmMain()
        {
            // Default constructor.
            InitializeComponent();
        }

        private void mnuFileExit_Click(object sender, EventArgs e)
        {
            // Exits application.
            Application.Exit();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            // Exits application
            Application.Exit();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Exits application on form closing.
            Application.Exit();
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            // When the user presses the report button, a new report form is created and shown. This form is hidden.
            frmReport report = new frmReport();
            report.Show();
            this.Hide();
        }
        private void mnuNavigationIItemsTotal_Click(object sender, EventArgs e)
        {
            // When the user presses the Items Total report menu option, a new report form is created and shown. This form is hidden.
            frmTotalItemCost report = new frmTotalItemCost();
            report.Show();
            this.Hide();
        }

        private void mnuNavigateItemDistribution_Click(object sender, EventArgs e)
        {
            // When the user presses the Items Distribution report menu option, a new report form is 
            // created and shown. This form is hidden.
            frmItemDistributionByUser report = new frmItemDistributionByUser();
            report.Show();
            this.Hide();
        }

        private void mnuNavigateUserNetWorth_Click(object sender, EventArgs e)
        {
            // When the user presses the User Net Worth report menu option, a new report form is created 
            // and shown. This form is hidden.
            frmUserNetWorth report = new frmUserNetWorth();
            report.Show();
            this.Hide();
        }

        private void mnuNavigateReport_Click(object sender, EventArgs e)
        {
            // This activates button click event for opening the report form.
            btnReport.PerformClick();
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            // When the user presses the add user button, the user is taken to the 
            // UsersNItems form in 'AddUser' mode.

            // Set form mode.
            frmUsersNItems.formMode = "AddUser";
            // Create a new UsersNItems form.
            frmUsersNItems formUI = new frmUsersNItems();
            // Show the new form.
            formUI.Show();
            // Hide the current form.
            this.Hide();
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            // When the user presses the add user button, the user is taken to the 
            // UsersNItems form in 'AddItem' mode.
            frmUsersNItems.formMode = "AddItem";
            // Create a new UsersNItems form.
            frmUsersNItems formUI = new frmUsersNItems();
            // Show the new form.
            formUI.Show();
            // Hide the current form.
            this.Hide();
        }

        private void btnUpdateItem_Click(object sender, EventArgs e)
        {
            // When the user presses the add user button, the user is taken to the 
            // UsersNItems form in 'UpdateItem' mode.
            frmUsersNItems.formMode = "UpdateItem";
            // Create a new UsersNItems form.
            frmUsersNItems formUI = new frmUsersNItems();
            // Show the new form.
            formUI.Show();
            // Hide the current form.
            this.Hide();
        }

        private void mnuAboutHelp_Click(object sender, EventArgs e)
        {
            // If user selects About/Help from the About menu, create a new about form a show it.
            frmHelp help = new frmHelp();
            help.ShowDialog();
        }

        private void mnuNavigateAddUser_Click(object sender, EventArgs e)
        {
            // If the user selects to Add A User from the Navigation menu, perform a click on the
            // corresponding button.
            btnAddUser.PerformClick();
        }

        private void mnuNavigateAddItem_Click(object sender, EventArgs e)
        {
            // If the user selects to Add An Item from the Navigation menu, perform a click on the
            // corresponding button.
            btnAddItem.PerformClick();
        }

        private void mnuNavigateUpdateItem_Click(object sender, EventArgs e)
        {
            // If the user selects to Add A User from the Navigation menu, perform a click on the
            // corresponding button.
            btnUpdateItem.PerformClick();
        }

        
    }
}
