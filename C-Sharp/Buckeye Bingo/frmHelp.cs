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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stones_Final_Project
{
    public partial class frmHelp : Form
    {
        // This form allows users to select from a list and get more information on how to 
        // use this application.

        public frmHelp()
        {
            InitializeComponent();
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            // close current form
            this.Close();
        }

        private void lstTopics_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(lstTopics.SelectedIndex)
            {
                case 0: // getting started
                    lblHelp.Text = "The program starts out in the setup wizard screen.  This is where " +
                        " starting session information can be entered.\n\n* Select a program\n" +
                        "* Select a charity\n* Select floor worker names\n* Select the pull tab desk worker\n" +
                        "* Select the desk worker\n* Enter information about each paper set\n* Enter serial " +
                        "number for each floor card\n* Select pull tabs and enter their serial numbers\n\n" +
                        "Workers, pull tabs, and serials numbers can be added by clicking the plus signs. Likewise " +
                        "they can be removed by clicking the minus signs.\nWhen finished, click 'Submit'.\n\n" +
                        "The only required field is the session program.";
                    break;
                case 1: // in session
                    lblHelp.Text = "After you enter the main part of the program, which is after submitting the " +
                        "startup wizard, there are tabbed pages that can be navigated on the right side of the screen." +
                        "\n\nDesk Page:\n\t    * Enter paper set data\n\t    * Enter electronics data\n\t    * Enter number of winners " +
                        "for each game\nPull Tabs Page:\n\t    * Add or remove pull tabs using the plus and minus\n\t" +
                        "    * Enter serial number, name, and worker quantities for each tab\nFloor Page:\n\t    * Enter the " +
                        "quantity returned for each worker per card type\n\t     (The amount owed will automatically be generated)\n\t" +
                        "    * Double-click any 'OUT' field to edit the number of cards a worker has\n     received\n\t" +
                        "     (The field with be auto-locked upon leaving it)\n\t    * To update the serials for each floor " +
                        " card, hover over its name and use the\n     context menu that appears\nReport Page:\n\t    * Get totals to enter " +
                        "into the POS terminal\n\t    * View amounts owed for Desk and Pull Tab Desk workers\n\t" +
                        "    * Enter cash removed to update the net deposit.";
                    break;
                case 2: // changing session info
                    lblHelp.Text = "Click any of the items in the 'Edit' menu to enter a password and update session information.\n\n" +
                        "Tabs for navigation are located on the right side of the screen.\n\nClick any 'Update' radio button to edit " +
                        "the corresponding item. Make sure all fields are entered fully and correctly when updating.\n\n" +
                        "For additional help hover the mouse over items on the screen.\n\nWhen adding an item, not all item information " + 
                        "may be available on the add screen.  Make sure to enter edit mode for that item and update the information, so " + 
                        "that item can be used correctly in the session.";
                    break;
                case 3: // forgotten password
                    lblHelp.Text = "Forgotten password? That's okay.\n\nClick 'forgot?' when being prompted for the password. An email will " +
                        "be sent to your default email account with a temporary verification code.  Enter the code, and procede to set a new " +
                        "password.\n\nThe password may be updated at any time on the same pop-up screen that prompts you for your password (when " +
                        "attempting to access the 'Edit Sessions' page). Simply enter your old password and then the new one.\n\n" +
                        "The default email address can be updated in the 'Defaults' tab page of the 'Edit Sessions' page.\n\n If you cannot recover " +
                        "your password, contact Jeremy Stones at jwstones@mymail.tstc.edu.";
                    break;
                default:
                    break;
            }
        }
    }
}
