// Jeremy Stones
// Special Topics
// Brad Willingham

// Lab 02
// 9/8/2017

// Program Specification
// This program connects to a database, verifies user account by login screen, and then
// allows the user to select both a user and an item to view information about any item
// owned by a user.

using System;
using System.Windows.Forms;

namespace Stones_Lab02
{
    public partial class frmInfo : Form
    {
        // variables to hold login info
        private string currentUser;
        private string currentUserID;
        public frmInfo()
        {
            // default constuctor
            InitializeComponent();
            // pull users from database before login
            if(!Stones_Lab02_DLL.DBOps.GetUsers())
            {
                MessageBox.Show(Stones_Lab02_DLL.DBOps.ErrorMessage);
            }
            // get user items
            if(!Stones_Lab02_DLL.DBOps.GetItems())
            {
                MessageBox.Show(Stones_Lab02_DLL.DBOps.ErrorMessage);
            }
        }

        private void mnuExit_Click(object sender, EventArgs e)
        {
            // exit application properly
            Application.Exit();
        }

        private void frmInfo_Load(object sender, EventArgs e)
        {
            
            using (frmLogin login = new frmLogin())
            {
                // show login form
                login.ShowDialog();
                // set current user
                currentUser = frmLogin.currentUser;
            }
            // fill combo box with users
            FillUsersCombo();
            // set combobox to current user
            cbxUser.SelectedItem = currentUser;
           
        }

        private void FillUsersCombo()
        {
            // method to fill users combobox
            foreach (Stones_Lab02_DLL.User user in Stones_Lab02_DLL.User.CurrentUsers)
            {
                // get list from DLL and fill user combo box
                cbxUser.Items.Add(user.FullName);
            }
        }

        private void cbxUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            // when user picks a different user from the combo box, reset the combobox for items

            // Clear the form.
            txtCost.Clear();
            txtDescription.Clear();
            txtItemID.Clear();
            txtItemName.Clear();
            txtOwner.Clear();
            txtQuantity.Clear();
            // first clear the combo box items
            cbxItem.Items.Clear();
            // set current user based on new selection
            currentUser = cbxUser.SelectedItem.ToString();
            foreach(Stones_Lab02_DLL.User user in Stones_Lab02_DLL.User.CurrentUsers)
            {
                // loop through all users and set current user
                if(user.FullName == currentUser)
                {
                    // set when full name matches combo value
                    currentUserID = user.UserID;
                }
            }
            foreach(Stones_Lab02_DLL.Item item in Stones_Lab02_DLL.Item.CurrentItems)
            {
                // loop through all items and add items belonging to current user
                if(item.UserID == currentUserID)
                {
                    // if userID found in item, add to combobox
                    cbxItem.Items.Add(item.ItemName);
                }
            }
            // set item to first item for current user
            try
            {
                // try, because there might not be an item for a particular user in the future
                // even though they all have one right now
                cbxItem.SelectedIndex = 0;
            }
            catch (Exception) { }
        }

        private void cbxItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            // when items combobox selection is changed, update all the textboxes with new item's info
            foreach (Stones_Lab02_DLL.Item item in Stones_Lab02_DLL.Item.CurrentItems)
            {
                // loop through items to find current item selected
                if(item.ItemName == cbxItem.SelectedItem.ToString())
                {
                    // when item found, set textboxes with item's info
                    txtCost.Text = double.Parse(item.ItemCost).ToString("c");
                    txtDescription.Text = item.ItemDescription;
                    txtItemID.Text = item.ItemID;
                    txtItemName.Text = item.ItemName;
                    txtOwner.Text = cbxUser.SelectedItem.ToString();
                    txtQuantity.Text = item.Quantity;
                }
            }
        }
    }
}
