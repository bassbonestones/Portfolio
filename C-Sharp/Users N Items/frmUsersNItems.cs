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
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace Stones_Lab04
{
    public partial class frmUsersNItems : Form
    {
        // This form is the interface from which the user can alter the database by adding a user, adding an
        // item, or updating an item.  

        // Create a public property to hold the form's current mode.
        public static string formMode;
        bool reporting = false;
        Image noimage;
        public frmUsersNItems()
        {
            // Before initializing this form, run the powershell command to start a new LoadForm application process.
            DbOps.ShowLoad();
            // Begin intializing this form.
            InitializeComponent();
            // After this form has fully loaded and is shown, call the powershell command to close any
            // process of the LoadForm type.
            this.Activated += new EventHandler((s, e) =>
            {
                // Close any process of the LoadForm type.
                DbOps.CloseLoad();
            });
            var request = WebRequest.Create("https://s3.amazonaws.com/bzucket/noimage.png");

            using (var response = request.GetResponse())
            using (var stream = response.GetResponseStream())
            {
                noimage = Bitmap.FromStream(stream);
            }
        }

        private void frmUsersNItems_Load(object sender, EventArgs e)
        {
            // On the load event, get the data from the database, and format the default datagridview
            // state.  Select the appropriate radio button, conditionally format the DGV based on mode,
            // and update the controls conditionally as well.


            // Call methods to get data.
            DbOps.GetItems();
            DbOps.GetUsers();
            // Set up the DGV baseline defaults.
            dgvInfo.AutoGenerateColumns = true;
            dgvInfo.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvInfo.AllowUserToAddRows = false;
            dgvInfo.AllowUserToDeleteRows = false;
            dgvInfo.AllowUserToOrderColumns = false;
            dgvInfo.AllowUserToResizeColumns = false;
            dgvInfo.AllowUserToResizeRows = false;
            dgvInfo.RowHeadersVisible = false;
            dgvInfo.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
            dgvInfo.ColumnHeadersDefaultCellStyle.BackColor = Color.Linen;
            dgvInfo.EnableHeadersVisualStyles = false;
            dgvInfo.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            switch (formMode)
            {
                // Check the correct radio button.
                case "AddUser":
                    rdoAddUser.Checked = true;
                    break;
                case "AddItem":
                    rdoAddItem.Checked = true;
                    break;
                case "UpdateItem":
                    rdoUpdateItem.Checked = true;
                    break;
                default:
                    this.Close();
                    break;
            }
            // Use the mode to format the columns of the DGV.
            FormatDGV();
            // Fill the users combo box with the current users.
            FillUsersCombo();
            // Set control properties based on edit mode.
            UpdateControls();
        }

        private void FillUsersCombo()
        {
            // This method is used to clear the users combo box and refill it with the current users list.
            cbxUsers.Items.Clear();
            foreach (User user in User.AllUsers)
            {
                // Loop through all users and add their First/Last names to the drop down list.
                cbxUsers.Items.Add(user.FName + " " + user.LName);
            }
        }

        public void FormatDGV()
        {
            // Before updating the DGV with any new info, open a new LoadForm process with PowerShell.
            DbOps.ShowLoad();
            // This method changes the DGV based on the mode.
            if (formMode == "AddItem" || formMode == "UpdateItem")
            {
                // If editing Items, clear the data in the DGV and set it back to the Items.

                // Create a new table object and set it to a cast of the current DGV source.
                DataTable dt = (DataTable)dgvInfo.DataSource;
                // Test to see if it's null and clear it if it has data.
                if (dt != null)
                    dt.Clear();
                // Call GetItems to make sure the program has a freshly populated items dataset.
                DbOps.GetItems();
                // Set the DGV datasource to this new dataset.
                dgvInfo.DataSource = DbOps.DsItems.Tables[DbOps.DtItems];
                // Hide the ItemID.
                dgvInfo.Columns[0].Visible = false;
                // Set the ItemName column to 250 width.
                dgvInfo.Columns[1].Width = 250;
                // Set the Item Name header text.
                dgvInfo.Columns[1].HeaderText = "Item Name";
                // Set the Item Description Header text.
                dgvInfo.Columns[2].HeaderText = "Item Description.";
                // Set the format, alignment, header text, and width for 'cost'
                dgvInfo.Columns[3].DefaultCellStyle.Format = "c";
                dgvInfo.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvInfo.Columns[3].HeaderText = "Item Cost";
                dgvInfo.Columns[3].Width = 190;
                dgvInfo.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                // Set the alignment and width for Quantity.
                dgvInfo.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvInfo.Columns[4].Width = 100;
                dgvInfo.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                // Make the rest of the columns invisible
                dgvInfo.Columns[5].Visible = false;
                dgvInfo.Columns[6].Visible = false;
                dgvInfo.Columns[7].Visible = false;
                // Set column 2 to fill (at end, because more appropriately placed after other widths defined).
                dgvInfo.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                // Refresh the data grid view.
                dgvInfo.Refresh();
            }
            else if (formMode == "AddUser")
            {
                DataTable DT = (DataTable)dgvInfo.DataSource;
                if (DT != null)
                    DT.Clear();
                //dgvInfo.Rows.Clear();
                //dgvInfo.Columns.Clear();
                DbOps.GetUsers();
                dgvInfo.DataSource = DbOps.DsUsers.Tables[DbOps.DtUsers];
                dgvInfo.Columns[0].Visible = false;
                dgvInfo.Columns[1].Width = 200;
                dgvInfo.Columns[3].Visible = false;
                dgvInfo.Columns[4].Visible = false;
                dgvInfo.Columns[5].Visible = true;
                dgvInfo.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvInfo.Refresh();
            }
            // After the DGV has fully loaded, use PowerShell to kill any process of the LoadForm type.
            DbOps.CloseLoad();
        }

        private void frmUsersNItems_FormClosed(object sender, FormClosedEventArgs e)
        {
            // If this form closes, show the home navigation form.
            if (reporting == false)
                Application.OpenForms[0].Show();
            else
            {
                try
                {
                    // For some reason, the form was going to the back of any open
                    // applications when going from the Update form to Report form, so I added this
                    // code to prevent it.
                    Application.OpenForms[1].Focus();
                }
                catch (Exception) { }
            }
        }

        private void UpdateControls()
        {
            // This method updates control properties of the form based on the current formMode variable value.

            // Before updating fields, clear them all.
            ClearForm();

            switch (formMode)
            {
                case "AddUser":
                    // If adding a user set the appropriate control's attributes to display the form properly
                    // for adding a user.
                    lblDescription.Visible = false;
                    lblFirstNameItemName.Text = "First Name";
                    lblLastName.Visible = true;
                    lblPasswordQuantity.Text = "Password";
                    lblSelectUser.Visible = false;
                    lblUsernameCost.Text = "Username";
                    lblPasswordConfirm.Visible = true;
                    txtDescription.Visible = false;
                    txtLastName.Visible = true;
                    txtPasswordConfirm.Visible = true;
                    btnFunction.Text = "Add";
                    chkAdmin.Visible = true;
                    pbxItem.Visible = false;
                    cbxUsers.Visible = false;
                    lblTitle.Text = "Add User";
                    pbxUsers.Visible = true;
                    btnEditPic.Visible = false;
                    mnuNavigateAddUser.Enabled = false;
                    mnuNavigateAddItem.Enabled = true;
                    mnuNavigateUpdateItem.Enabled = true;
                    txtPasswordQuantity.UseSystemPasswordChar = true;
                    break;
                case "AddItem":
                    // If adding an item set the appropriate control's attributes to display the form properly
                    // for adding an item.
                    lblDescription.Visible = true;
                    lblFirstNameItemName.Text = "Item Name";
                    lblLastName.Visible = false;
                    lblPasswordQuantity.Text = "Quantity";
                    lblSelectUser.Visible = true;
                    lblUsernameCost.Text = "Cost";
                    lblPasswordConfirm.Visible = false;
                    txtDescription.Visible = true;
                    txtLastName.Visible = false;
                    txtPasswordConfirm.Visible = false;
                    btnFunction.Text = "Add";
                    chkAdmin.Visible = false;
                    pbxItem.Visible = true;
                    cbxUsers.Visible = true;
                    lblTitle.Text = "Add Item";
                    pbxUsers.Visible = false;
                    btnEditPic.Visible = true;
                    mnuNavigateAddUser.Enabled = true;
                    mnuNavigateAddItem.Enabled = false;
                    mnuNavigateUpdateItem.Enabled = true;
                    txtPasswordQuantity.UseSystemPasswordChar = false;
                    break;
                case "UpdateItem":
                    // If updating an item set the appropriate control's attributes to display the form properly
                    // for updating an item.
                    lblDescription.Visible = true;
                    lblFirstNameItemName.Text = "Item Name";
                    lblLastName.Visible = false;
                    lblPasswordQuantity.Text = "Quantity";
                    lblSelectUser.Visible = true;
                    lblUsernameCost.Text = "Cost";
                    lblPasswordConfirm.Visible = false;
                    txtDescription.Visible = true;
                    txtLastName.Visible = false;
                    txtPasswordConfirm.Visible = false;
                    btnFunction.Text = "Update";
                    chkAdmin.Visible = false;
                    pbxItem.Visible = true;
                    cbxUsers.Visible = true;
                    lblTitle.Text = "Update Item";
                    pbxUsers.Visible = false;
                    btnEditPic.Visible = true;
                    mnuNavigateAddUser.Enabled = true;
                    mnuNavigateAddItem.Enabled = true;
                    mnuNavigateUpdateItem.Enabled = false;
                    txtPasswordQuantity.UseSystemPasswordChar = false;
                    break;
                default:
                    break;
            }
        }

        private void rdoAddUser_CheckedChanged(object sender, EventArgs e)
        {
            // If the user selects the Add User radio button, set the mode to 'AddUser', and update the form.
            if (formMode != "AddUser")
            {
                formMode = "AddUser";
                FormatDGV();
                UpdateControls();
            }
        }

        private void rdoAddItem_CheckedChanged(object sender, EventArgs e)
        {
            // If the user selects the Add Item radio button, set the mode to 'AddItem', and update the form.
            if (formMode != "AddItem")
            {
                formMode = "AddItem";
                FormatDGV();
                UpdateControls();
            }
        }

        private void rdoUpdateItem_CheckedChanged(object sender, EventArgs e)
        {
            // If the user selects the Update Item radio button, set the mode to 'UpdateItem', and update the form.
            if (formMode != "UpdateItem")
            {
                formMode = "UpdateItem";
                FormatDGV();
                UpdateControls();
            }
        }

        private void btnFunction_Click(object sender, EventArgs e)
        {
            // This button handles the final update commands for each possible mode form.

            // First check for row selection, and if none, return.
            if (formMode == "UpdateItem" && dgvInfo.SelectedRows.Count != 1)
            {
                MessageBox.Show("Please select an item to update.");
                return;
            }
            switch (formMode)
            {
                case "AddUser":
                    // If the user is adding a user...
                    {
                        // Validate all form fields.
                        if (txtFirstNameItemName.Text.Trim() == "")
                        {
                            // If nothing is entered for the first name, show an error.
                            MessageBox.Show("Please make sure to enter the user's first name.");
                            txtFirstNameItemName.Focus();
                            return;
                        }
                        if (txtLastName.Text.Trim() == "")
                        {
                            // If nothing is entered for the last name, show an error.
                            MessageBox.Show("Please make sure to enter the user's last name.");
                            txtLastName.Focus();
                            return;
                        }
                        if (txtUserNameCost.Text.Trim() == "")
                        {
                            // If nothing is entered for the username, show an error.
                            MessageBox.Show("Please make sure to enter the username.");
                            txtUserNameCost.Focus();
                            return;
                        }
                        if (txtPasswordQuantity.Text.Trim() == "")
                        {
                            // If nothing is entered for the password, show an error.
                            MessageBox.Show("Please make sure to enter the password.");
                            txtPasswordQuantity.Focus();
                            return;
                        }
                        if (txtPasswordConfirm.Text.Trim() == "")
                        {
                            // If nothing is entered for the password confirmation, show an error.
                            MessageBox.Show("Please make sure to enter the password twice to confirm it.");
                            txtPasswordConfirm.Focus();
                            return;
                        }
                        if (txtPasswordQuantity.Text != txtPasswordConfirm.Text)
                        {
                            // If the passwords do not match, show an error.
                            MessageBox.Show("The passwords do not match.");
                            txtPasswordQuantity.Clear();
                            txtPasswordConfirm.Clear();
                            txtPasswordQuantity.Focus();
                            return;
                        }
                        foreach (User u in User.AllUsers)
                        {
                            // Loop through all the users.

                            if (txtFirstNameItemName.Text == u.FName && txtLastName.Text == u.LName)
                            {
                                // If a matching first and last name occur, ask the user if this was intentional.
                                DialogResult dlg = MessageBox.Show("A user with the name, " + u.FName + " " + u.LName +
                                    ", already exists. If this was intentional and you would like to proceed, press 'Yes'. " +
                                    "Otherwise, press 'No'.", "Duplicate Name Found", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                if (dlg == DialogResult.No)
                                {
                                    // If it was unintentional, clear the form and send the focus back to the First Name text box.
                                    ClearForm();
                                    txtFirstNameItemName.Focus();
                                    return;
                                }
                            }
                            if (txtUserNameCost.Text == u.Username)
                            {
                                // If a username already exists, show an error.
                                MessageBox.Show("Sorry, the username, " + u.Username + ", is already taken.");
                                txtUserNameCost.Focus();
                                txtUserNameCost.SelectAll();
                                return;
                            }
                        }
                        // If validation passes, create a new User object, store its attributes, and add it to 
                        // the users list.
                        User user = new User();
                        user.FName = txtFirstNameItemName.Text;
                        user.LName = txtLastName.Text;
                        user.Username = txtUserNameCost.Text;
                        user.Password = txtPasswordConfirm.Text;
                        user.Admin = chkAdmin.Checked;
                        // Add the user to the users list.
                        DbOps.AddUser(user);
                        if (DbOps.ErrorExists)
                        {
                            // If the user was not added, show the message associated with that error.
                            MessageBox.Show(DbOps.ErrorMessage);
                            return;
                        }
                        // Success message.
                        MessageBox.Show("User successfully added.");
                        // Update the form.
                        FormatDGV();
                        FillUsersCombo();
                        ClearForm();
                    }
                    break;
                case "AddItem":
                    {
                        // If user is adding an item, validate the input and add item to database.

                        if (cbxUsers.SelectedIndex == -1)
                        {
                            // If no input, return an error, set the focus, and end the event.
                            MessageBox.Show("Please make sure to selected the item's owner from the users list.");
                            cbxUsers.Focus();
                            return;
                        }
                        if (txtFirstNameItemName.Text.Trim() == "")
                        {
                            // If no input, return an error, set the focus, and end the event.
                            MessageBox.Show("Please make sure to enter the item name.");
                            txtFirstNameItemName.Focus();
                            return;
                        }
                        if (txtUserNameCost.Text.Trim() == "")
                        {
                            // If no input, return an error, set the focus, and end the event.
                            MessageBox.Show("Please make sure to enter the item's cost.");
                            txtUserNameCost.Focus();
                            return;
                        }
                        if (txtPasswordQuantity.Text.Trim() == "")
                        {
                            // If no input, return an error, set the focus, and end the event.
                            MessageBox.Show("Please make sure to enter the item's quantity.");
                            txtPasswordQuantity.Focus();
                            return;
                        }
                        if (txtDescription.Text.Trim() == "")
                        {
                            // If no input, return an error, set the focus, and end the event.
                            MessageBox.Show("Please make sure to enter the item description.");
                            txtDescription.Focus();
                            return;
                        }
                        // Test for a valid cost - must be a float and no more than 2 decimal places.
                        double costTest;
                        bool costValid = double.TryParse(txtUserNameCost.Text, out costTest);
                        if (costValid)
                        {
                            int testInt;
                            // If it's a floating point number, test for max 2 decimal precision.
                            if (!int.TryParse((double.Parse(txtUserNameCost.Text) * 100.0).ToString(), out testInt))
                            {
                                costValid = false;
                            }
                        }
                        if (!costValid)
                        {
                            // If not valid, return an error, set the focus, and end the event.
                            MessageBox.Show("Please enter a valid cost.");
                            txtUserNameCost.Focus();
                            txtUserNameCost.SelectAll();
                            return;
                        }
                        // Text quantity for a valid positive integer.
                        int qtyTest;
                        bool qtyValid = int.TryParse(txtPasswordQuantity.Text, out qtyTest);
                        if (qtyValid)
                        {
                            // If the value is an integer, test for positive.
                            if (qtyTest < 0)
                            {
                                // Return false for negative numbers
                                qtyValid = false;
                            }
                        }
                        if (!qtyValid)
                        {
                            // If not valid, return an error, set the focus, and end the event.
                            MessageBox.Show("Please enter a valid quantity. Only whole numbers are allowed.");
                            txtPasswordQuantity.Focus();
                            txtPasswordQuantity.SelectAll();
                            return;
                        }

                        // Create a new, temp item.
                        Item item = new Item();
                        // Set its attributes.
                        item.Active = 1;
                        item.ItemCost = float.Parse(txtUserNameCost.Text);
                        item.ItemDescription = txtDescription.Text;
                        item.ItemName = txtFirstNameItemName.Text;
                        item.Quantity = int.Parse(txtPasswordQuantity.Text);
                        item.UserID = DbOps.GetIDFromUser(cbxUsers.SelectedItem.ToString());
                        // Set Image to image of picturebox.
                        item.ItemImage = pbxItem.Image;
                        // Also set byte[] of image.
                        MemoryStream ms = new MemoryStream();
                        pbxItem.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        item.ItemImageBytes = ms.ToArray();
                        // Add the item to the database.
                        DbOps.AddItem(item);
                        if (DbOps.ErrorExists)
                        {
                            // Check for any database errors.
                            MessageBox.Show(DbOps.ErrorMessage);
                            return;
                        }
                        // Success message.
                        MessageBox.Show("Item successfully added.");
                        // Refresh view.
                        FormatDGV();
                        ClearForm();
                    }
                    break;
                case "UpdateItem":
                    {
                        // If user is updating an item, validate the input and update item in database.

                        if (cbxUsers.SelectedIndex == -1)
                        {
                            // If no input, return an error, set the focus, and end the event.
                            MessageBox.Show("Please make sure to selected the item's owner from the users list.");
                            cbxUsers.Focus();
                            return;
                        }
                        if (txtFirstNameItemName.Text.Trim() == "")
                        {
                            // If no input, return an error, set the focus, and end the event.
                            MessageBox.Show("Please make sure to enter the item name.");
                            txtFirstNameItemName.Focus();
                            return;
                        }
                        if (txtUserNameCost.Text.Trim() == "")
                        {
                            // If no input, return an error, set the focus, and end the event.
                            MessageBox.Show("Please make sure to enter the item's cost.");
                            txtUserNameCost.Focus();
                            return;
                        }
                        if (txtPasswordQuantity.Text.Trim() == "")
                        {
                            // If no input, return an error, set the focus, and end the event.
                            MessageBox.Show("Please make sure to enter the item's quantity.");
                            txtPasswordQuantity.Focus();
                            return;
                        }
                        if (txtDescription.Text.Trim() == "")
                        {
                            // If no input, return an error, set the focus, and end the event.
                            MessageBox.Show("Please make sure to enter the item description.");
                            txtDescription.Focus();
                            return;
                        }
                        // Test for a valid cost - must be a float and no more than 2 decimal places.
                        double costTest;
                        bool costValid = double.TryParse(txtUserNameCost.Text, out costTest);
                        if (costValid)
                        {
                            int testInt;
                            // If it's a floating point number, test for max 2 decimal precision.
                            if (!int.TryParse((double.Parse(txtUserNameCost.Text) * 100.0).ToString(), out testInt))
                            {
                                costValid = false;
                            }
                        }
                        if (!costValid)
                        {
                            // If not valid, return an error, set the focus, and end the event.
                            MessageBox.Show("Please enter a valid cost.");
                            txtUserNameCost.Focus();
                            txtUserNameCost.SelectAll();
                            return;
                        }
                        // Text quantity for a valid positive integer.
                        int qtyTest;
                        bool qtyValid = int.TryParse(txtPasswordQuantity.Text, out qtyTest);
                        if (qtyValid)
                        {
                            // If the value is an integer, test for positive.
                            if (qtyTest < 0)
                            {
                                // Return false for negative numbers
                                qtyValid = false;
                            }
                        }
                        if (!qtyValid)
                        {
                            // If not valid, return an error, set the focus, and end the event.
                            MessageBox.Show("Please enter a valid quantity. Only whole numbers are allowed.");
                            txtPasswordQuantity.Focus();
                            txtPasswordQuantity.SelectAll();
                            return;
                        }

                        // Create a new, temp item.
                        Item item = new Item();
                        // Set its attributes.
                        item.ItemID = int.Parse(dgvInfo.SelectedRows[0].Cells[0].Value.ToString());
                        item.Active = 1;
                        item.ItemCost = float.Parse(txtUserNameCost.Text);
                        item.ItemDescription = txtDescription.Text;
                        item.ItemName = txtFirstNameItemName.Text;
                        item.Quantity = int.Parse(txtPasswordQuantity.Text);
                        item.UserID = DbOps.GetIDFromUser(cbxUsers.SelectedItem.ToString());
                        // Set Image to image of picturebox.
                        item.ItemImage = pbxItem.Image;
                        // Also set byte[] of image.
                        MemoryStream ms = new MemoryStream();
                        pbxItem.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        item.ItemImageBytes = ms.ToArray();
                        // Add the item to the database.
                        DbOps.UpdateItem(item);
                        if (DbOps.ErrorExists)
                        {
                            // Check for any database errors.
                            MessageBox.Show(DbOps.ErrorMessage);
                            return;
                        }
                        // Success message.
                        MessageBox.Show("Item successfully updated.");
                        // Refresh view.
                        FormatDGV();
                        ClearForm();
                    }
                    break;
                default:
                    this.Close();
                    break;
            }
        }

        private void ClearForm()
        {
            // This method clears all form fields to their default state.
            txtDescription.Clear();
            txtFirstNameItemName.Clear();
            txtLastName.Clear();
            txtPasswordConfirm.Clear();
            txtPasswordQuantity.Clear();
            txtUserNameCost.Clear();
            chkAdmin.Checked = false;
            cbxUsers.SelectedIndex = -1;
            pbxItem.Image = noimage;
            dgvInfo.ClearSelection();
        }

        private void txt_Keydown(object sender, KeyEventArgs e)
        {
            // If a user presses enter in any of the text boxes, peform a click on the
            // main functionality button.
            if (e.KeyCode == Keys.Enter)
            {
                btnFunction.PerformClick();
            }
        }

        private void dgvInfo_SelectionChanged(object sender, EventArgs e)
        {
            // If the DGV selection changes, test for the mode, and handle the selection appropriately.

            if (formMode == "AddUser" || formMode == "AddItem")
            {
                // If the form is in AddUser or AddItem mode, clear the selection.
                dgvInfo.ClearSelection();
            }
            else if (formMode == "UpdateItem")
            {
                // If the form is in UpdateItem mode, attempt to retrieve the item info into the 
                // form fields.
                if (dgvInfo.SelectedRows.Count == 1)
                {
                    // Create temp item to only pull image of current item.
                    Item tempItem = new Item();
                    // If a row is currently selected...
                    foreach (Item item in Item.AllItems)
                    {
                        // Loop through all items, and if the itemID matches, retrieve its information
                        if (item.ItemID.ToString() == dgvInfo.SelectedRows[0].Cells[0].Value.ToString())
                        {
                            // Set the item's name
                            txtFirstNameItemName.Text = item.ItemName;
                            // Set the item's description.
                            txtDescription.Text = item.ItemDescription;
                            // Set the item's cost and display with a float precision of 2.
                            txtUserNameCost.Text = item.ItemCost.ToString("n2");
                            // Set the item's quantity.
                            txtPasswordQuantity.Text = item.Quantity.ToString();
                            // Set the item's owner.
                            cbxUsers.SelectedItem = DbOps.GetUserFromID(item.UserID);
                            tempItem = item;

                        }
                    }
                    // Set Image from ID.
                    if (tempItem.ItemImage == null)
                    {
                        tempItem.ItemImage = DbOps.GetPic(tempItem.ItemID);
                    }
                    // Also set byte[] of image.
                    if (tempItem.ItemImage != null)
                    {
                        MemoryStream ms = new MemoryStream();
                        tempItem.ItemImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        tempItem.ItemImageBytes = ms.ToArray();
                    }

                    if (tempItem.ItemImage != null)
                    {
                        // If the item has an image, set its image.
                        pbxItem.Image = tempItem.ItemImage;
                    }
                    else
                    {
                        // If the item has no image, display the noimage.png image.
                        pbxItem.Image = noimage;
                    }
                }
                else
                {
                    // If nothing is selected, clear the form.
                    dgvInfo.ClearSelection();
                    ClearForm();
                }
            }
        }

        private void btnEditPic_Click(object sender, EventArgs e)
        {
            // If a users clicks the edit button for the image, show an open file
            // dialog and let them select a new picture.

            // If the user isn't adding a new item and there isn't a row selected, end event.
            if (dgvInfo.SelectedRows.Count != 1 && !rdoAddItem.Checked)
                return;
            DialogResult dlg = ofdMain.ShowDialog();
            if (dlg == DialogResult.OK)
            {
                // Set the picture box image, if they user selects an image.
                pbxItem.Image = Image.FromFile(ofdMain.FileName);
                // Remind the user that changes are not saved until they hit the function button.
                string mode = formMode == "AddItem" ? "'Add'" : "'Update'";
                MessageBox.Show("In order to save these changes, you still need to press the " + mode + " button.");
            }

        }

        private void mnuFileClose_Click(object sender, EventArgs e)
        {
            // If the user selects the Return To Navigation option from the File menu
            // close the current form.
            this.Close();
        }

        private void mnuEditClear_Click(object sender, EventArgs e)
        {
            // If the user selects the Clear option from the Edit menu, call the ClearForm method.
            ClearForm();
        }

        private void mnuNavigateAddUser_Click(object sender, EventArgs e)
        {
            // If the user selects the Add User option from the Navigate menu, select the Add User radio.
            rdoAddUser.Select();
        }

        private void mnuNavigateAddItem_Click(object sender, EventArgs e)
        {
            // If the user selects the Add Item option from the Navigate menu, select the Add Item radio.
            rdoAddItem.Select();
        }

        private void mnuNavigateUpdateItem_Click(object sender, EventArgs e)
        {
            // If the user selects the Update Item option from the Navigate menu, select the Update Item radio.
            rdoUpdateItem.Select();
        }

        private void mnuNavigateReport_Click(object sender, EventArgs e)
        {
            // If the user selects the View Report option from the Navigate menu, 
            // create a new report form and show it (close this one).
            frmReport report = new frmReport();
            report.Show();
            // Set reporting flag to true so navigation will not open also.
            reporting = true;
            this.Close();
        }
        private void mnuNavigationIItemsTotal_Click(object sender, EventArgs e)
        {
            // When the user presses the Items Total report menu option, a new report form is created and shown. This form is hidden.
            frmTotalItemCost report = new frmTotalItemCost();
            report.Show();
            // Set reporting flag to true so navigation will not open also.
            reporting = true;
            this.Close();
        }

        private void mnuNavigateItemDistribution_Click(object sender, EventArgs e)
        {
            // When the user presses the Items Distribution report menu option, a new report form is 
            // created and shown. This form is hidden.
            frmItemDistributionByUser report = new frmItemDistributionByUser();
            report.Show();
            // Set reporting flag to true so navigation will not open also.
            reporting = true;
            this.Close();
        }

        private void mnuNavigateUserNetWorth_Click(object sender, EventArgs e)
        {
            // When the user presses the User Net Worth report menu option, a new report form is created 
            // and shown. This form is hidden.
            frmUserNetWorth report = new frmUserNetWorth();
            report.Show();
            // Set reporting flag to true so navigation will not open also.
            reporting = true;
            this.Close();
        }
        private void mnuNavigateHome_Click(object sender, EventArgs e)
        {
            // If the user selects the Return to Home Screen option from the Navigation menu, close this one.
            this.Close();
        }

        private void mnuAboutHelp_Click(object sender, EventArgs e)
        {
            // If the user selects the Help/About option from the About menu, 
            // create a new Help form and show it modally on top of the current form.
            frmHelp help = new frmHelp();
            help.ShowDialog();
        }
    }
}
