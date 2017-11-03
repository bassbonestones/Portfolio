using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stones_Lab04
{
    public partial class frmItemDistributionByUser : Form
    {
        // This class is to create a form that displays item distribution by user information in a Crystal Report.

        // Create flag to prevent navigation screen from appearing when the user is switching to the
        // update form.
        bool updating = false;

        public frmItemDistributionByUser()
        {
            // Before initializing the report form, begin the Loading Screen application.
            DbOps.ShowLoad();
            // Being initialing this form.
            InitializeComponent();
            // Create an activated event that closes any process of the LoadForm type.
            this.Activated += new EventHandler((s, e) =>
            {
                // Close any LoadForm type.
                DbOps.CloseLoad();
            });
        }

        private void frmItemDistributionByUser_Load(object sender, EventArgs e)
        {
            // Upon the load of this form, clear the current viewer, and set it to show the
            // user items Crystal Report.

            // Refresh the viewer.
            crvItemDistribution.RefreshReport();
            // Create a new report document item.
            crItemQtyDistributionByUser report = new crItemQtyDistributionByUser();
            // Store the database login information here so the user is not prompted every 
            // time they try to access the report.
            report.SetDatabaseLogon("bassbonestones", "Rachel0315!");
            // Set the viewer's report object to the 'report' Crystal Report object.
            crvItemDistribution.ReportSource = report;
        }

        private void mnuNavigateHome_Click(object sender, EventArgs e)
        {
            // If the users clicks the 'home' button, close this form.
            this.Close();
        }

        private void frmReport_FormClosed(object sender, FormClosedEventArgs e)
        {
            // If this form is closed, show the home navigation screen.
            try
            {
                if (!updating)
                    Application.OpenForms[0].Show();
            }
            catch (Exception) { Application.Exit(); }
        }

        private void mnuAboutHelp_Click(object sender, EventArgs e)
        {
            // If user selects About/Help from the About menu, create a new about form a show it.
            frmHelp help = new frmHelp();
            help.ShowDialog();
        }

        private void mnuFileClose_Click(object sender, EventArgs e)
        {
            // If user clicks the Return to Home Screen option from the menu, close this form.
            this.Close();
        }

        private void mnuNavigateAddUser_Click(object sender, EventArgs e)
        {
            // When the user presses the add user button, the user is taken to the 
            // UsersNItems form in 'AddUser' mode.

            // Set form mode.
            frmUsersNItems.formMode = "AddUser";
            // Create a new UsersNItems form.
            frmUsersNItems formUI = new frmUsersNItems();
            // Show the new form.
            formUI.Show();
            // Set updating flag to true, so navigation screen doesn't also appear.
            updating = true;
            // Hide the current form.
            this.Close();
        }

        private void mnuNavigateAddItem_Click(object sender, EventArgs e)
        {
            // When the user presses the add item button, the user is taken to the 
            // UsersNItems form in 'AddItem' mode.

            // Set form mode.
            frmUsersNItems.formMode = "AddItem";
            // Create a new UsersNItems form.
            frmUsersNItems formUI = new frmUsersNItems();
            // Show the new form.
            formUI.Show();
            // Set updating flag to true, so navigation screen doesn't also appear.
            updating = true;
            // Hide the current form.
            this.Close();
        }

        private void mnuNavigateUpdateItem_Click(object sender, EventArgs e)
        {
            // When the user presses the update item button, the user is taken to the 
            // UsersNItems form in 'UpdateItem' mode.

            // Set form mode.
            frmUsersNItems.formMode = "UpdateItem";
            // Create a new UsersNItems form.
            frmUsersNItems formUI = new frmUsersNItems();
            // Show the new form.
            formUI.Show();
            // Set updating flag to true, so navigation screen doesn't also appear.
            updating = true;
            // Hide the current form.
            this.Close();
        }

        private void mnuNavigationReport_Click(object sender, EventArgs e)
        {
            // When the user presses the report button, a new report form is created and shown. This form is hidden.
            frmReport report = new frmReport();
            report.Show();
            // Set updating flag to true so navigation will not open also.
            updating = true;
            this.Close();
        }
        private void mnuNavigationIItemsTotal_Click(object sender, EventArgs e)
        {
            // When the user presses the Items Total report menu option, a new report form is created and shown. This form is hidden.
            frmTotalItemCost report = new frmTotalItemCost();
            report.Show();
            // Set updating flag to true so navigation will not open also.
            updating = true;
            this.Close();
        }
        

        private void mnuNavigateUserNetWorth_Click(object sender, EventArgs e)
        {
            // When the user presses the User Net Worth report menu option, a new report form is created 
            // and shown. This form is hidden.
            frmUserNetWorth report = new frmUserNetWorth();
            report.Show();
            // Set updating flag to true so navigation will not open also.
            updating = true;
            this.Close();
        }
        
    }
}
