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
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stones_Final_Project
{
    public partial class frmEdit : Form
    {
        // This form is for adding, updating, and deleting bingo session template information.
        // Changes are saved to the database, and therefore, admin access is required with a password.

        // exit flag
        bool backToWizard = false;
        // template variales
        string templateName = "";
        int templateID = 0;
        // electronics variables
        string electronicsName = "";
        int electronicsID = 0;
        // floor card variables
        string fcName = "";
        int fcID = 0;
        double fcPrice = 0;
        int fcQty1 = 0;
        int fcQty2 = 0;
        int fcQty3 = 0;
        int fcQty4 = 0;
        int fcQty5 = 0;
        int fcGame1 = 0;
        string fcGame1Name = "";
        int fcGame2 = 0;
        string fcGame2Name = "";
        int fcGame3 = 0;
        string fcGame3Name = "";
        int fcRed = 0;
        int fcGreen = 0;
        int fcBlue = 0;
        // session bingo game variables
        int gameID = 0;
        int gameNum = 0;
        string gameName = "";
        double gamePrize = 0;
        int gameCard1 = 0;
        string gameCard1Name = "";
        int gameCard2 = 0;
        string gameCard2Name = "";
        int gameCard3 = 0;
        string gameCard3Name = "";
        bool gameEB = false;
        bool game5050 = false;
        bool gameChangePaid = true;
        int gameRed = 0;
        int gameGreen = 0;
        int gameBlue = 0;
        // paper set variables
        int paperID = 0;
        string paperName = "";
        double paperPrice = 0;
        bool paperRedemptAvail = false;
        double paperRedemptPrice = 0;
        // charity vars
        string charityTaxID = "";
        string charityName = "";
        // worker vars
        string workerName = "";
        // pull tab vars
        string pullTabName = "";
        string pullTabFormID = "";
        double pullTabGross = 0;
        double pullTabPrizes = 0;
        double pullTabTax = 0;
        bool pullTabArchive = false;

        public frmEdit(int index)
        {

            
            InitializeComponent();
            
            // set tab index
            tabEdit.SelectedIndex = index;
            // fill listboxes
            UpdateTemplateListbox();
            UpdateCharitiesListBox();
            UpdateWorkersListBox();
            UpdatePullTabsListBox();
            FillDefaults();

            // update panel locations to be in correct places
            pnlAddTemplate.Location = new Point(77, 289);
            pnlTemplates.Location = new Point(this.Width / 2 - pnlTemplates.Width / 2, pnlTemplates.Location.Y);
            pnlCharities.Location = new Point(this.Width / 2 - pnlCharities.Width / 2, pnlCharities.Location.Y);
            pnlWorkers.Location = new Point(this.Width / 2 - pnlWorkers.Width / 2, pnlWorkers.Location.Y);
            pnlPullTabs.Location = new Point(this.Width / 2 - pnlPullTabs.Width / 2, pnlPullTabs.Location.Y);
            pnlUpdateDefaults.Location = new Point(this.Width / 2 - pnlUpdateDefaults.Width / 2, pnlUpdateDefaults.Location.Y);
            pnlUpdateTemplateElectronics.Location = new Point(14, 97);
            pnlUpdateTemplateFloorCards.Location = new Point(14, 97);
            pnlUpdateTemplateGames.Location = new Point(14, 97);
            pnlUpdateTemplatePaperSets.Location = new Point(14, 97);
            pnlUpdateTemplateName.Location = new Point(14, 97);
            pnlUpdateMachine.Location = new Point(485, 8);
            pnlUpdateFloorCard.Location = new Point(485, 8);
            pnlUpdateGame.Location = new Point(485, 8);
            pnlUpdatePaperSet.Location = new Point(485, 8);
            pnlUpdateCharity.Location = new Point(77, 289);
            pnlAddCharity.Location = new Point(77, 289);
            pnlAddWorker.Location = new Point(77, 289);
            pnlUpdateWorker.Location = new Point(77, 289);
            pnlAddPullTab.Location = new Point(77, 289);
            pnlUpdatePullTab.Location = new Point(77, 289);

            // update panel sizes to be correct size on display

            // size of update name
            int nW;
            int nH;
            string strNLoc = pnlUpdateTemplateName.Tag.ToString();
            GetSize(strNLoc, out nW, out nH);
            pnlUpdateTemplateName.Size = new Size(nW, nH);

            // size of update electronics
            int electronicsW;
            int electronicsH;
            string strElecLoc = pnlUpdateTemplateElectronics.Tag.ToString();
            GetSize(strElecLoc, out electronicsW, out electronicsH);
            pnlUpdateTemplateElectronics.Size = new Size(electronicsW,electronicsH);

            // size of update floor cards
            int fcW;
            int fcH;
            string strFCLoc = pnlUpdateTemplateFloorCards.Tag.ToString();
            GetSize(strFCLoc, out fcW, out fcH);
            pnlUpdateTemplateFloorCards.Size = new Size(fcW, fcH);

            // size of update games
            int gW;
            int gH;
            string strGLoc = pnlUpdateTemplateGames.Tag.ToString();
            GetSize(strGLoc, out gW, out gH);
            pnlUpdateTemplateGames.Size = new Size(gW, gH);

            // size of update paper sets
            int psW;
            int psH;
            string strPSLoc = pnlUpdateTemplatePaperSets.Tag.ToString();
            GetSize(strFCLoc, out psW, out psH);
            pnlUpdateTemplatePaperSets.Size = new Size(psW, psH);

        }


        protected override void WndProc(ref Message message)
        {
            // override windproc to keep user from being able to move form at all (minimize allowed)
            const int WM_SYSCOMMAND = 0x0112;
            const int SC_MOVE = 0xF010;

            switch (message.Msg)
            {
                case WM_SYSCOMMAND:
                    int command = message.WParam.ToInt32() & 0xfff0;
                    if (command == SC_MOVE)
                        return;
                    break;
            }

            base.WndProc(ref message);
        }
        private void FillDefaults()
        {
            DbOps.FillDefaultsTable();
            // fill defaults
            txtEmail.Text = DbOps.Email;
            nudDefaultNumWorkers.Value = DbOps.NumFloorWorkers;
            nudDefaultTaxRate.Value = (decimal)DbOps.TaxRate;
        }
        private void GetSize(string size, out int w, out int h)
        {
            w = int.Parse(size.Substring(0, size.IndexOf(',')));
            h = int.Parse(size.Substring(size.IndexOf(',') + 1));
        }
        private void UpdateTemplateListbox()
        {
            DbOps.FillSessionTemplatesTable();
            // add templates to template listbox
            lstTemplates.Items.Clear();
            foreach (SessionTemplate st in DbOps.SessionTemplates)
            {
                lstTemplates.Items.Add(st.TemplateName);
            }
        }
        private void mnuInfoAbout_Click(object sender, EventArgs e)
        {
            // create and show an about form
            frmAbout about = new frmAbout();
            about.ShowDialog();
        }
        private void mnuInfoHelp_Click(object sender, EventArgs e)
        {
            // create and show new help form
            frmHelp help = new frmHelp();
            help.ShowDialog();
        }
        private void mnuFileExit_Click(object sender, EventArgs e)
        {
            // properly exit program
            Application.Exit();
        }
        private void mnuFileBackToWizard_Click(object sender, EventArgs e)
        {
            // close this form and open a new wizard form
            backToWizard = true;
            this.Hide();
            frmStartupWizard startup = new frmStartupWizard();
            startup.Show();
            this.Close();
        }
        private void mnuEditSessionTemplates_Click(object sender, EventArgs e)
        {
            // move to Templates tab
            tabEdit.SelectedIndex = 0;
        }
        private void mnuEditCharities_Click(object sender, EventArgs e)
        {
            // move to charities tab
            tabEdit.SelectedIndex = 1;
        }
        private void mnuEditWorkers_Click(object sender, EventArgs e)
        {
            // move to workers tab
            tabEdit.SelectedIndex = 2;
        }
        private void mnuEditPullTabs_Click(object sender, EventArgs e)
        {
            // move to pull tabs tab
            tabEdit.SelectedIndex = 3;
        }
        private void mnuEditDefaults_Click(object sender, EventArgs e)
        {
            // move to defaults tab
            tabEdit.SelectedIndex = 4;
        }
        private void lstTemplates_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(lstTemplates.SelectedIndex == -1)
            {
                btnRemove.Visible = false;
                pnlAddTemplate.Visible = false;
                pnlUpdateTemplate.Visible = false;
                lblTemplateObjects.Text = "Select a template...";
                cbxTemplateObjects.Visible = false;
                ClearAllFields();
            }
            else
            {
                lblTemplateObjects.Text = "Template Attribute to Update:";
                cbxTemplateObjects.Visible = true;
                templateName = DbOps.SessionTemplates[lstTemplates.SelectedIndex].TemplateName;
                templateID = DbOps.SessionTemplates[lstTemplates.SelectedIndex].TemplateID;
                lblTemplateName.Text = templateName;
                cbxTemplateObjects.SelectedIndex = -1;
                // fill datasets from database
                DbOps.FillPaperSetsTable(templateID);
                DbOps.FillSessionBingoGamesTable(templateID);
                UpdateElectronicsListBox();
                UpdateFloorCardsListBox();
                UpdateBingoGamesListBox();
                ClearUpdateTemplateFields();
            }
        }
        private void UpdateFloorCardsListBox()
        {
            DbOps.FillFloorCardsTable(templateID);
            // add floorcards to fc listbox
            lstFloorCards.Items.Clear();
            foreach (FloorCard fc in FloorCard.CurrentFloorCards)
            {
                if (fc.TemplateID == templateID)
                    lstFloorCards.Items.Add(fc.Description);
            }
        }
        private void rdoTemplate_CheckChanged(object sender, EventArgs e)
        {
            if(rdoAddTemplate.Checked)
            {
                pnlAddTemplate.Visible = true;
                pnlUpdateTemplate.Visible = false;
                btnRemove.Visible = false;
                lstTemplates.ClearSelected();
            }
            else if(rdoUpdateTemplate.Checked)
            {
                pnlAddTemplate.Visible = false;
                pnlUpdateTemplate.Visible = true;
                btnRemove.Visible = false;
            }
            else if(rdoRemoveTemplate.Checked)
            {
                pnlAddTemplate.Visible = false;
                pnlUpdateTemplate.Visible = false;
                btnRemove.Visible = true;
            }
            ClearAllFields();
        }
        private void ClearUpdateTemplateFields()
        {
            txtAddBingoName.Clear();
            txtAddFloorCardNewName.Clear();
            txtAddMachineName.Clear();
            txtAddPaperSetName.Clear();
            txtBingoGameName.Clear();
            txtFloorCardName.Clear();
            txtFloorCardPrice.Clear();
            txtMaxPayout.Clear();
            txtNewMachineName.Clear();
            txtNewTemplateName.Clear();
            txtPaperSetName.Clear();
            txtPaperSetPrice.Clear();
            txtPaperSetRedemptionPrice.Clear();
            txtQtyW1.Clear();
            txtQtyW2.Clear();
            txtQtyW3.Clear();
            txtQtyW4.Clear();
            txtQtyW5.Clear();
            rdoAddBingoGame.Checked = false;
            rdoUpdateBingoGame.Checked = false;
            rdoRemoveBingoGame.Checked = false;
            rdoAddElectronics.Checked = false;
            rdoUpdateElectronics.Checked = false;
            rdoRemoveElectronics.Checked = false;
            rdoAddFloorCard.Checked = false;
            rdoUpdateFloorCard.Checked = false;
            rdoAddFloorCard.Checked = false;
            rdoAddPaperSet.Checked = false;
            rdoUpdatePaperSet.Checked = false;
            rdoRemovePaperSet.Checked = false;
            lstBingoGames.ClearSelected();
            lstElectronics.ClearSelected();
            lstFloorCards.ClearSelected();
            lstPaperSets.ClearSelected();
            chk5050.Checked = false;
            chkChangePaid.Checked = true;
            chkEarlyBird.Checked = false;
            pnlFloorCardColor.BackColor = Color.Black;
            pnlGameColor.BackColor = Color.Black;
            cbxFCAssociation1.SelectedIndex = -1;
            cbxFCAssociation2.SelectedIndex = -1;
            cbxFCAssociation3.SelectedIndex = -1;
            cbxGameAssociation1.SelectedIndex = -1;
            cbxGameAssociation2.SelectedIndex = -1;
            cbxGameAssociation3.SelectedIndex = -1;
            cbxTemplateObjects.SelectedIndex = -1;
            lblCurrentMachineNameValue.Text = "*";
            lblGameNumberValue.Text = "*";
        }
        private void ClearAllFields()
        {
            
            txtAddTemplate.Clear();
            ClearUpdateTemplateFields();

           
        }
        private void cbxTemplateObjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(cbxTemplateObjects.SelectedIndex)
            {
                case 0: // name
                    pnlUpdateTemplateElectronics.Visible = false;
                    pnlUpdateTemplateFloorCards.Visible = false;
                    pnlUpdateTemplateGames.Visible = false;
                    pnlUpdateTemplatePaperSets.Visible = false;
                    pnlUpdateTemplateName.Visible = true;
                    break;
                case 1: // electronics
                    pnlUpdateTemplateElectronics.Visible = true;
                    pnlUpdateTemplateFloorCards.Visible = false;
                    pnlUpdateTemplateGames.Visible = false;
                    pnlUpdateTemplatePaperSets.Visible = false;
                    pnlUpdateTemplateName.Visible = false;
                    break;
                case 2: // paper sets
                    pnlUpdateTemplateElectronics.Visible = false;
                    pnlUpdateTemplateFloorCards.Visible = true;
                    pnlUpdateTemplateGames.Visible = false;
                    pnlUpdateTemplatePaperSets.Visible = false;
                    pnlUpdateTemplateName.Visible = false;
                    break;
                case 3: // floor cards
                    pnlUpdateTemplateElectronics.Visible = false;
                    pnlUpdateTemplateFloorCards.Visible = false;
                    pnlUpdateTemplateGames.Visible = false;
                    pnlUpdateTemplatePaperSets.Visible = true;
                    pnlUpdateTemplateName.Visible = false;
                    break;
                case 4: // session bingo games
                    pnlUpdateTemplateElectronics.Visible = false;
                    pnlUpdateTemplateFloorCards.Visible = false;
                    pnlUpdateTemplateGames.Visible = true;
                    pnlUpdateTemplatePaperSets.Visible = false;
                    pnlUpdateTemplateName.Visible = false;
                    break;
                default: // -1
                    pnlUpdateTemplateElectronics.Visible = false;
                    pnlUpdateTemplateFloorCards.Visible = false;
                    pnlUpdateTemplateGames.Visible = false;
                    pnlUpdateTemplatePaperSets.Visible = false;
                    pnlUpdateTemplateName.Visible = false;
                    break;
            }
        }
        private void UpdateFloorCardGameCombos()
        {
            foreach(SessionBingoGame s in SessionBingoGame.CurrentSessionBingoGames)
            {
                cbxGameAssociation1.Items.Add(s.Description);
                cbxGameAssociation2.Items.Add(s.Description);
                cbxGameAssociation3.Items.Add(s.Description);
            }
        }

        private void pnlFloorCardColor_Click(object sender, EventArgs e)
        {
            if (dlgColor.ShowDialog() == DialogResult.OK)
            {
                pnlFloorCardColor.BackColor = dlgColor.Color;
                fcRed = (int)dlgColor.Color.R;
                fcGreen = (int)dlgColor.Color.G;
                fcBlue = (int)dlgColor.Color.B;
            }
        }
        private void frmEdit_FormClosed(object sender, FormClosedEventArgs e)
        {
            // exit application
            if(!backToWizard)
                Application.Exit();
        }
        private void btnRemove_Click(object sender, EventArgs e)
        {
            // if user clicks remove template button
            if(lstTemplates.SelectedIndex == -1)
            {
                // check for template selection and return error if none selected
                MessageBox.Show("No template selected.");
                return;
            }
            DialogResult dlg = MessageBox.Show("Are you sure you want to remove this template? Removal cannot be reversed.  All games, floor cards, and other template information.", "WARNING - CONFIRMATION", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dlg == DialogResult.Yes)
            {
                // see if user is sure they want to delete and if so, remove form database
                int id = DbOps.SessionTemplates[lstTemplates.SelectedIndex].TemplateID;
                string name = DbOps.SessionTemplates[lstTemplates.SelectedIndex].TemplateName;
                DbOps.DeleteSessionTemplate(id, name);
                UpdateTemplateListbox();
                MessageBox.Show("Session template, " + name + ", has been removed permanently from the database.");
            }
        }
        private void btnAddTemplate_Click(object sender, EventArgs e)
        {
            // if user clicks add template button
            if(txtAddTemplate.Text.Trim().Length < 1)
            {
                // check to make sure a name has been added, error if not
                MessageBox.Show("A template name is required to add a new template.");
                return;
            }
            // add template with new name
            string name = txtAddTemplate.Text;
            DbOps.AddSessionTemplate(name);
            UpdateTemplateListbox();
            MessageBox.Show("Session template, " + name + ", has been added.");
            txtAddTemplate.Clear();
        }
        private void btnUpdateTemplateName_Click(object sender, EventArgs e)
        {
            if(txtNewTemplateName.Text.Trim().Length < 1)
            {
                MessageBox.Show("Name not entered.");
                return;
            }
            string oldName = lblTemplateName.Text;
            string newName = txtNewTemplateName.Text;
            DbOps.UpdateSessionTemplate(templateID, newName);
            UpdateTemplateListbox();
            txtNewTemplateName.Clear();
            MessageBox.Show("Template name successfully updated from " + oldName + " to " + newName + ".");
            lstTemplates.ClearSelected();
        }
        private void rdoElectronics_CheckChanged(object sender, EventArgs e)
        {
            
            if (rdoAddElectronics.Checked)
            {
                pnlAddElectronics.Visible = true;
                pnlUpdateMachine.Visible = false;
                btnRemoveElectronics.Visible = false;
                lstElectronics.ClearSelected();
            }
            else if (lstElectronics.SelectedIndex == -1)
            {
                pnlAddElectronics.Visible = false;
                pnlUpdateMachine.Visible = false;
                btnRemoveElectronics.Visible = false;
            }
            else if (rdoUpdateElectronics.Checked)
            {

                pnlAddElectronics.Visible = false;
                pnlUpdateMachine.Visible = true;
                btnRemoveElectronics.Visible = false;
            }
            else if (rdoRemoveElectronics.Checked)
            {
                pnlAddElectronics.Visible = false;
                pnlUpdateMachine.Visible = false;
                btnRemoveElectronics.Visible = true;
            }
        }
        private void btnRemoveElectronics_Click(object sender, EventArgs e)
        {
            // if user clicks remove template button
            if (lstElectronics.SelectedIndex == -1)
            {
                // check for template selection and return error if none selected
                MessageBox.Show("No machine selected.");
                return;
            }
            DialogResult dlg = MessageBox.Show("Are you sure you want to remove this machine? Removal cannot be reversed.", "WARNING - CONFIRMATION", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dlg == DialogResult.Yes)
            {
                // see if user is sure they want to delete and if so, remove from database
                int id = Machine.CurrentMachines[lstElectronics.SelectedIndex].ElectronicsID;
                string name = Machine.CurrentMachines[lstElectronics.SelectedIndex].Description;
                DbOps.DeleteMachine(id, name);
                UpdateElectronicsListBox();
                MessageBox.Show("Machine, " + name + ", has been removed permanently from the database.");
            }
        }
        private void UpdateElectronicsListBox()
        {
            DbOps.FillElectronicsTable(templateID);
            // add templates to template listbox
            lstElectronics.Items.Clear();
            foreach (Machine m in Machine.CurrentMachines)
            {
                if(m.TemplateID == templateID)
                    lstElectronics.Items.Add(m.Description);
            }
        }
        private void lstElectronics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstElectronics.SelectedIndex == -1)
            {
                btnRemoveElectronics.Visible = false;
                pnlAddElectronics.Visible = false;
                pnlUpdateMachine.Visible = false;
            }
            else
            {
                electronicsName = Machine.CurrentMachines[lstElectronics.SelectedIndex].Description;
                electronicsID = Machine.CurrentMachines[lstElectronics.SelectedIndex].ElectronicsID;
                lblCurrentMachineNameValue.Text = electronicsName;
                if (rdoUpdateElectronics.Checked)
                    pnlUpdateMachine.Visible = true;
                if (rdoRemoveElectronics.Checked)
                    btnRemoveElectronics.Visible = true;
            }
        }
        private void btnAddMachine_Click(object sender, EventArgs e)
        {
            // if user clicks add template button
            if (txtAddMachineName.Text.Trim().Length < 1)
            {
                // check to make sure a name has been added, error if not
                MessageBox.Show("A machine name is required to add a new machine.");
                return;
            }
            // add machine with new name
            string name = txtAddMachineName.Text;
            DbOps.AddMachine(templateID, name);
            UpdateElectronicsListBox();
            MessageBox.Show("Machine, " + name + ", has been added to the template, " + templateName + ".");
            txtAddMachineName.Clear();
        }
        private void btnUpdateMachineName_Click(object sender, EventArgs e)
        {
            
            string oldName = electronicsName;
            string newName = txtNewMachineName.Text;
            if(newName.Trim().Length < 1)
            {
                MessageBox.Show("No name entered.");
                return;
            }
            DbOps.UpdateMachine(electronicsID, newName);
            UpdateElectronicsListBox();
            MessageBox.Show("Machine name changed from " + oldName +" to " + newName +".");
            lblCurrentMachineNameValue.Text = newName;
            txtNewMachineName.Clear();
        }
        private void txtAddTemplate_KeyDown(object sender, KeyEventArgs e)
        {
            // if user in textbox and hits enter, press button
            if(e.KeyCode == Keys.Enter)
            {
                btnAddTemplate.PerformClick();
                e.Handled = true;
            }
        }
        private void txtAddMachineName_KeyDown(object sender, KeyEventArgs e)
        {
            // if user in textbox and hits enter, press button
            if (e.KeyCode == Keys.Enter)
            {
                btnAddMachine.PerformClick();
                e.Handled = true;
            }
        }
        private void txtNewMachineName_KeyDown(object sender, KeyEventArgs e)
        {
            // if user in textbox and hits enter, press button
            if (e.KeyCode == Keys.Enter)
            {
                btnUpdateMachineName.PerformClick();
                e.Handled = true;
            }
        }
        private void txtNewTemplateName_KeyDown(object sender, KeyEventArgs e)
        {
            // if user in textbox and hits enter, press button
            if (e.KeyCode == Keys.Enter)
            {
                btnUpdateTemplateName.PerformClick();
                e.Handled = true;
            }
        }
        private void rdoFloorCards_CheckChanged(object sender, EventArgs e)
        {
            if (rdoAddFloorCard.Checked)
            {
                pnlAddFloorCard.Visible = true;
                pnlUpdateFloorCard.Visible = false;
                btnRemoveFloorCard.Visible = false;
                lstFloorCards.ClearSelected();
            }
            else if (lstFloorCards.SelectedIndex == -1)
            {
                pnlAddFloorCard.Visible = false;
                pnlUpdateFloorCard.Visible = false;
                btnRemoveFloorCard.Visible = false;
            }
            else if (rdoUpdateFloorCard.Checked)
            {

                pnlAddFloorCard.Visible = false;
                pnlUpdateFloorCard.Visible = true;
                btnRemoveFloorCard.Visible = false;
            }
            else if (rdoRemoveFloorCard.Checked)
            {
                pnlAddFloorCard.Visible = false;
                pnlUpdateFloorCard.Visible = false;
                btnRemoveFloorCard.Visible = true;
            }
        }
        private void lstFloorCards_SelectedIndexChanged(object sender, EventArgs e)
        {
            // if selection is changed on floor cards listbox
            if (lstFloorCards.SelectedIndex == -1)
            {
                //
                btnRemoveFloorCard.Visible = false;
                pnlAddFloorCard.Visible = false;
                pnlUpdateFloorCard.Visible = false;
            }
            else
            {
                UpdateFloorCardGameCombos();

                fcName = FloorCard.CurrentFloorCards[lstFloorCards.SelectedIndex].Description;
                fcID = FloorCard.CurrentFloorCards[lstFloorCards.SelectedIndex].CardID;
                fcPrice = FloorCard.CurrentFloorCards[lstFloorCards.SelectedIndex].Price;
                fcGame1 = FloorCard.CurrentFloorCards[lstFloorCards.SelectedIndex].GameID1;
                fcGame1Name = "";
                foreach(SessionBingoGame s in SessionBingoGame.CurrentSessionBingoGames)
                {
                    if (fcGame1 == s.GameID)
                        fcGame1Name = s.Description;
                }
                fcGame2 = FloorCard.CurrentFloorCards[lstFloorCards.SelectedIndex].GameID2;
                fcGame2Name = "";
                foreach (SessionBingoGame s in SessionBingoGame.CurrentSessionBingoGames)
                {
                    if (fcGame2 == s.GameID)
                        fcGame2Name = s.Description;
                }
                fcGame3 = FloorCard.CurrentFloorCards[lstFloorCards.SelectedIndex].GameID3;
                fcGame3Name = "";
                foreach (SessionBingoGame s in SessionBingoGame.CurrentSessionBingoGames)
                {
                    if (fcGame3 == s.GameID)
                        fcGame3Name = s.Description;
                }
                fcQty1 = FloorCard.CurrentFloorCards[lstFloorCards.SelectedIndex].Quantity1;
                fcQty2 = FloorCard.CurrentFloorCards[lstFloorCards.SelectedIndex].Quantity2;
                fcQty3 = FloorCard.CurrentFloorCards[lstFloorCards.SelectedIndex].Quantity3;
                fcQty4 = FloorCard.CurrentFloorCards[lstFloorCards.SelectedIndex].Quantity4;
                fcQty5 = FloorCard.CurrentFloorCards[lstFloorCards.SelectedIndex].Quantity5;
                fcRed = FloorCard.CurrentFloorCards[lstFloorCards.SelectedIndex].ColorRed;
                fcGreen = FloorCard.CurrentFloorCards[lstFloorCards.SelectedIndex].ColorGreen;
                fcBlue = FloorCard.CurrentFloorCards[lstFloorCards.SelectedIndex].ColorBlue;

                txtFloorCardName.Text = fcName;
                txtFloorCardPrice.Text = fcPrice.ToString("n2");
                txtQtyW1.Text = fcQty1.ToString();
                txtQtyW2.Text = fcQty2.ToString();
                txtQtyW3.Text = fcQty3.ToString();
                txtQtyW4.Text = fcQty4.ToString();
                txtQtyW5.Text = fcQty5.ToString();
                // add back after games added
                if (fcGame1Name.Length > 0)
                    cbxGameAssociation1.SelectedItem = fcGame1Name;
                else
                    cbxGameAssociation1.SelectedIndex = -1;
                if (fcGame2Name.Length > 0)
                    cbxGameAssociation2.SelectedItem = fcGame2Name;
                else
                    cbxGameAssociation2.SelectedIndex = -1;
                if (fcGame3Name.Length > 0)
                    cbxGameAssociation3.SelectedItem = fcGame3Name;
                else
                    cbxGameAssociation3.SelectedIndex = -1;
                pnlFloorCardColor.BackColor = Color.FromArgb(fcRed, fcGreen, fcBlue);

                if (rdoUpdateFloorCard.Checked)
                    pnlUpdateFloorCard.Visible = true;
                if (rdoRemoveFloorCard.Checked)
                    btnRemoveFloorCard.Visible = true;
            }
        }
        private void txtAddFloorCardNewName_KeyDown(object sender, KeyEventArgs e)
        {
            // if user in textbox and hits enter, press button
            if (e.KeyCode == Keys.Enter)
            {
                btnAddFloorCard.PerformClick();
                e.Handled = true;
            }
        }
        private void btnAddFloorCard_Click(object sender, EventArgs e)
        {
            // if user clicks add fc button
            if (txtAddFloorCardNewName.Text.Trim().Length < 1)
            {
                // check to make sure a name has been added, error if not
                MessageBox.Show("A floor card name is required to add a new floor card.");
                return;
            }
            // add floor card with new name
            string name = txtAddFloorCardNewName.Text;
            DbOps.AddFloorCard(templateID, name);
            UpdateFloorCardsListBox();
            MessageBox.Show("Floor card, " + name + ", has been added to the template, " + templateName + ".\n" + 
                "Make sure to update floor card attributes.");
            txtAddFloorCardNewName.Clear();
        }
        private void btnRemoveFloorCard_Click(object sender, EventArgs e)
        {
            // if user clicks remove template button
            if (lstFloorCards.SelectedIndex == -1)
            {
                // check for fc selection and return error if none selected
                MessageBox.Show("No floor card selected.");
                return;
            }
            DialogResult dlg = MessageBox.Show("Are you sure you want to remove this floor card? Removal cannot be reversed.", "WARNING - CONFIRMATION", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dlg == DialogResult.Yes)
            {
                // see if user is sure they want to delete and if so, remove from database
                int id = FloorCard.CurrentFloorCards[lstFloorCards.SelectedIndex].CardID;
                string name = FloorCard.CurrentFloorCards[lstFloorCards.SelectedIndex].Description;
                DbOps.DeleteFloorCard(id, name);
                UpdateFloorCardsListBox();
                MessageBox.Show("FloorCard, " + name + ", has been removed permanently from the database.");
            }
        }
        private void btnUpdateFloorCard_Click(object sender, EventArgs e)
        {
            // first validate all fields for being filled
            string name = txtFloorCardName.Text.Trim();
            string price = GetNumberOnly(txtFloorCardPrice.Text.Trim());
            string qty1 = txtQtyW1.Text.Trim();
            string qty2 = txtQtyW2.Text.Trim();
            string qty3 = txtQtyW3.Text.Trim();
            string qty4 = txtQtyW4.Text.Trim();
            string qty5 = txtQtyW5.Text.Trim();
            if(name.Length < 1 || price.Length < 1 || qty1.Length < 1 || qty2.Length < 1 || qty3.Length < 1 || qty4.Length < 1)
            {
                MessageBox.Show("All attributes must be filled in to update floor card.");
                return;
            }
            // make sure all quantities are numbers
            double dblPrice;
            int intQty1;
            int intQty2;
            int intQty3;
            int intQty4;
            int intQty5;
            bool allInts = true;
            bool isDbl = double.TryParse(price, out dblPrice);
            if (!isDbl)
                allInts = false;
            bool isInt = int.TryParse(qty1, out intQty1);
            if (!isInt)
                allInts = false;
            isInt = int.TryParse(qty2, out intQty2);
            if (!isInt)
                allInts = false;
            isInt = int.TryParse(qty3, out intQty3);
            if (!isInt)
                allInts = false;
            isInt = int.TryParse(qty4, out intQty4);
            if (!isInt)
                allInts = false;
            isInt = int.TryParse(qty5, out intQty5);
            if (!isInt)
                allInts = false;
            if(!allInts)
            {
                MessageBox.Show("Price and quantities must be numbers. Quantities may only be integers");
                return;
            }
            // continue getting attributes

            // gameId associations
            int game1 = -1;
            int game2 = -1;
            int game3 = -1;
            string game1Name = "";
            string game2Name = "";
            string game3Name = "";
            try
            {
                game1Name = cbxGameAssociation1.SelectedItem.ToString().Trim();
                game2Name = cbxGameAssociation2.SelectedItem.ToString().Trim();
                game3Name = cbxGameAssociation3.SelectedItem.ToString().Trim();
            } catch(Exception) { }

            if (game1Name.Length < 1)
                game1 = -1;
            else
            {
                foreach(SessionBingoGame s in SessionBingoGame.CurrentSessionBingoGames)
                {
                    if (s.Description == game1Name)
                        game1 = s.GameID;
                }
            }
            if (game2Name.Length < 1)
                game2 = -1;
            else
            {
                foreach (SessionBingoGame s in SessionBingoGame.CurrentSessionBingoGames)
                {
                    if (s.Description == game2Name)
                        game2 = s.GameID;
                }
            }
            if (game3Name.Length < 1)
                game3 = -1;
            else
            {
                foreach (SessionBingoGame s in SessionBingoGame.CurrentSessionBingoGames)
                {
                    if (s.Description == game3Name)
                        game3 = s.GameID;
                }
            }

            // color
            Color color = pnlFloorCardColor.BackColor;
            int red = (int)color.R;
            int green = (int)color.G;
            int blue = (int)color.B;

            DbOps.UpdateFloorCard(fcID, name, dblPrice, intQty1, intQty2, intQty3, intQty4, intQty5, game1, game2, game3, red, green, blue);
            MessageBox.Show("Floor card, " + name + ", successfully updated.");
            UpdateFloorCardsListBox();
        }
        private void txtFloorCardName_KeyDown(object sender, KeyEventArgs e)
        {
            // if user in textbox and hits enter, press button
            if (e.KeyCode == Keys.Enter)
            {
                btnUpdateFloorCard.PerformClick();
                e.Handled = true;
            }
        }
        private void rdoGames_CheckedChanged(object sender, EventArgs e)
        {
            // update the session games area if radio updated
            if (rdoAddBingoGame.Checked)
            {
                // add area shown
                pnlAddGame.Visible = true;
                pnlUpdateGame.Visible = false;
                btnRemoveBingoGame.Visible = false;
                lstBingoGames.ClearSelected();
            }
            else if (lstBingoGames.SelectedIndex == -1)
            {
                // no card selected
                pnlAddGame.Visible = false;
                pnlUpdateGame.Visible = false;
                btnRemoveBingoGame.Visible = false;
            }
            else if (rdoUpdateBingoGame.Checked)
            {
                // update area shown
                pnlAddGame.Visible = false;
                pnlUpdateGame.Visible = true;
                btnRemoveBingoGame.Visible = false;
            }
            else if (rdoRemoveBingoGame.Checked)
            {
                // remove area shown
                pnlAddGame.Visible = false;
                pnlUpdateGame.Visible = false;
                btnRemoveBingoGame.Visible = true;
            }
        }
        private void UpdateGameFloorCardCombos()
        {
            foreach (FloorCard fc in FloorCard.CurrentFloorCards)
            {
                if (fc.TemplateID == templateID)
                {
                    cbxFCAssociation1.Items.Add(fc.Description);
                    cbxFCAssociation2.Items.Add(fc.Description);
                    cbxFCAssociation3.Items.Add(fc.Description);
                }
            }
        }
        private void lstBingoGames_SelectedIndexChanged(object sender, EventArgs e)
        {
            // if selection is changed on floor cards listbox
            if (lstBingoGames.SelectedIndex == -1)
            {
                // no card selected
                btnRemoveBingoGame.Visible = false;
                pnlAddGame.Visible = false;
                pnlUpdateGame.Visible = false;
            }
            else
            {
                UpdateGameFloorCardCombos();

                gameName = SessionBingoGame.CurrentSessionBingoGames[lstBingoGames.SelectedIndex].Description;
                gameID = SessionBingoGame.CurrentSessionBingoGames[lstBingoGames.SelectedIndex].GameID;
                gamePrize = SessionBingoGame.CurrentSessionBingoGames[lstBingoGames.SelectedIndex].MaxOrSetPrice;//[sic] (prize)
                gameNum = SessionBingoGame.CurrentSessionBingoGames[lstBingoGames.SelectedIndex].GameNumber;
                gameCard1 = SessionBingoGame.CurrentSessionBingoGames[lstBingoGames.SelectedIndex].CardID1;
                gameCard1Name = "";
                foreach (FloorCard fc in FloorCard.CurrentFloorCards)
                {
                    if (gameCard1 == fc.CardID)
                        gameCard1Name = fc.Description;
                }
                gameCard2 = SessionBingoGame.CurrentSessionBingoGames[lstBingoGames.SelectedIndex].CardID2;
                gameCard2Name = "";
                foreach (FloorCard fc in FloorCard.CurrentFloorCards)
                {
                    if (gameCard2 == fc.CardID)
                        gameCard2Name = fc.Description;
                }
                gameCard3 = SessionBingoGame.CurrentSessionBingoGames[lstBingoGames.SelectedIndex].CardID3;
                gameCard3Name = "";
                foreach (FloorCard fc in FloorCard.CurrentFloorCards)
                {
                    if (gameCard3 == fc.CardID)
                        gameCard3Name = fc.Description;
                }

                gameEB = SessionBingoGame.CurrentSessionBingoGames[lstBingoGames.SelectedIndex].EarlyBird;
                game5050 = SessionBingoGame.CurrentSessionBingoGames[lstBingoGames.SelectedIndex].FiftyFifty;
                gameChangePaid = SessionBingoGame.CurrentSessionBingoGames[lstBingoGames.SelectedIndex].ChangePaidOut;

                gameRed = SessionBingoGame.CurrentSessionBingoGames[lstBingoGames.SelectedIndex].ColorRed;
                gameGreen = SessionBingoGame.CurrentSessionBingoGames[lstBingoGames.SelectedIndex].ColorGreen;
                gameBlue = SessionBingoGame.CurrentSessionBingoGames[lstBingoGames.SelectedIndex].ColorBlue;

                txtBingoGameName.Text = gameName;
                txtMaxPayout.Text = gamePrize.ToString("n2");

                chkEarlyBird.Checked = gameEB;
                chk5050.Checked = game5050;
                chkChangePaid.Checked = gameChangePaid;
                // add back after games added
                if (gameCard1Name.Length > 0)
                    cbxFCAssociation1.SelectedItem = gameCard1Name;
                else
                    cbxFCAssociation1.SelectedIndex = -1;
                if (gameCard2Name.Length > 0)
                    cbxFCAssociation2.SelectedItem = gameCard2Name;
                else
                    cbxFCAssociation2.SelectedIndex = -1;
                if (gameCard3Name.Length > 0)
                    cbxFCAssociation3.SelectedItem = gameCard3Name;
                else
                    cbxFCAssociation3.SelectedIndex = -1;
                lblGameNumberValue.Text = gameNum.ToString();
                pnlGameColor.BackColor = Color.FromArgb(gameRed, gameGreen, gameBlue);

                if (rdoUpdateBingoGame.Checked)
                    pnlUpdateGame.Visible = true;
                if (rdoRemoveBingoGame.Checked)
                    btnRemoveBingoGame.Visible = true;
            }
        }
        private void txtAddBingoName_KeyDown(object sender, KeyEventArgs e)
        {
            // if user in textbox and hits enter, press button
            if (e.KeyCode == Keys.Enter)
            {
                btnAddBingoGame.PerformClick();
                e.Handled = true;
            }
        }
        private void txtUpdateGame_KeyDown(object sender, KeyEventArgs e)
        {
            // if user in textbox and hits enter, press button
            if (e.KeyCode == Keys.Enter)
            {
                btnUpdateBingoGame.PerformClick();
                e.Handled = true;
            }
        }
        private void pnlGameColor_Click(object sender, EventArgs e)
        {
            if (dlgColor.ShowDialog() == DialogResult.OK)
            {
                pnlGameColor.BackColor = dlgColor.Color;
                gameRed = (int)dlgColor.Color.R;
                gameGreen = (int)dlgColor.Color.G;
                gameBlue = (int)dlgColor.Color.B;
            }
        }
        private void UpdateBingoGamesListBox()
        {
            DbOps.FillSessionBingoGamesTable(templateID);
            // add games to games listbox
            lstBingoGames.Items.Clear();
            foreach (SessionBingoGame s in SessionBingoGame.CurrentSessionBingoGames)
            {
                if (s.TemplateID == templateID)
                    lstBingoGames.Items.Add(s.Description);
            }
        }
        private void btnAddBingoGame_Click(object sender, EventArgs e)
        {
            // if user clicks add game button
            if (txtAddBingoName.Text.Trim().Length < 1)
            {
                // check to make sure a name has been added, error if not
                MessageBox.Show("A bingo game name is required to add a new bingo game.");
                return;
            }
            // add game with new name
            string name = txtAddBingoName.Text;
            DbOps.AddSessionBingoGame(templateID, name);
            UpdateBingoGamesListBox();
            MessageBox.Show("Bingo game, " + name + ", has been added to the template, " + templateName + ".\n" +
                "Make sure to update session bingo game attributes.");
            txtAddBingoName.Clear();
        }
        private void btnRemoveBingoGame_Click(object sender, EventArgs e)
        {
            // if user clicks remove bingo game button
            if (lstBingoGames.SelectedIndex == -1)
            {
                // check for game selection and return error if none selected
                MessageBox.Show("No game selected.");
                return;
            }
            DialogResult dlg = MessageBox.Show("Are you sure you want to remove this session bingo game? Removal cannot be reversed.", "WARNING - CONFIRMATION", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dlg == DialogResult.Yes)
            {
                // see if user is sure they want to delete and if so, remove from database
                int id = SessionBingoGame.CurrentSessionBingoGames[lstBingoGames.SelectedIndex].GameID;
                string name = SessionBingoGame.CurrentSessionBingoGames[lstBingoGames.SelectedIndex].Description;
                DbOps.DeleteSessionBingoGame(id, name);
                UpdateBingoGamesListBox();
                MessageBox.Show("Bingo game, " + name + ", has been removed permanently from the database.");
            }
        }
        private void btnUpdateBingoGame_Click(object sender, EventArgs e)
        {
            // first validate all fields for being filled
            string name = txtBingoGameName.Text.Trim();
            string payout = GetNumberOnly(txtMaxPayout.Text.Trim());
            bool eb = chkEarlyBird.Checked;
            bool ff = chk5050.Checked;
            bool change = chkChangePaid.Checked;

            if (name.Length < 1 || payout.Length < 1)
            {
                MessageBox.Show("All attributes must be filled in to update floor card.");
                return;
            }
            // make sure all quantities are numbers
            double dblPrize;
            
            bool allInts = true;
            bool isDbl = double.TryParse(payout, out dblPrize);
            if (!isDbl)
                allInts = false;
           
            if (!allInts)
            {
                MessageBox.Show("Max payout must be a number.");
                return;
            }
            // continue getting attributes

            // cardID associations
            int card1 = -1;
            int card2 = -1;
            int card3 = -1;
            string card1Name = "";
            string card2Name = "";
            string card3Name = "";
            try
            {
                card1Name = cbxFCAssociation1.SelectedItem.ToString().Trim();
                card2Name = cbxFCAssociation2.SelectedItem.ToString().Trim();
                card3Name = cbxFCAssociation2.SelectedItem.ToString().Trim();
            }
            catch (Exception) { }

            if (card1Name.Length < 1)
                card1 = -1;
            else
            {
                foreach (FloorCard f in FloorCard.CurrentFloorCards)
                {
                    if (f.Description == card1Name)
                        card1 = f.CardID;
                }
            }
            if (card2Name.Length < 1)
                card2 = -1;
            else
            {
                foreach (FloorCard f in FloorCard.CurrentFloorCards)
                {
                    if (f.Description == card2Name)
                        card2 = f.CardID;
                }
            }
            if (card3Name.Length < 1)
                card3 = -1;
            else
            {
                foreach (FloorCard f in FloorCard.CurrentFloorCards)
                {
                    if (f.Description == card3Name)
                        card3 = f.CardID;
                }
            }

            // color
            Color color = pnlGameColor.BackColor;
            int red = (int)color.R;
            int green = (int)color.G;
            int blue = (int)color.B;

            DbOps.UpdateSessionBingoGame(gameID, gameNum, name, dblPrize, card1, card2, card3, eb, change, ff, red, green, blue);
            MessageBox.Show("Session bingo game, " + name + ", successfully updated.");
            UpdateBingoGamesListBox();
        }
        private void txtPaperSetName_KeyDown(object sender, KeyEventArgs e)
        {
            // if user in textbox and hits enter, press button
            if (e.KeyCode == Keys.Enter)
            {
                btnUpdatePaperSet.PerformClick();
                e.Handled = true;
            }
        }
        private void txtAddPaperSetName_KeyDown(object sender, KeyEventArgs e)
        {
            // if user in textbox and hits enter, press button
            if (e.KeyCode == Keys.Enter)
            {
                btnAddPaperSet.PerformClick();
                e.Handled = true;
            }
        }
        private void rdoPaperSet_CheckChanged(object sender, EventArgs e)
        {
            if (rdoAddPaperSet.Checked)
            {
                pnlAddPaperSet.Visible = true;
                pnlUpdatePaperSet.Visible = false;
                btnRemovePaperSet.Visible = false;
                lstPaperSets.ClearSelected();
            }
            else if (lstPaperSets.SelectedIndex == -1)
            {
                pnlAddPaperSet.Visible = false;
                pnlUpdatePaperSet.Visible = false;
                btnRemovePaperSet.Visible = false;
            }
            else if (rdoUpdatePaperSet.Checked)
            {

                pnlAddPaperSet.Visible = false;
                pnlUpdatePaperSet.Visible = true;
                btnRemovePaperSet.Visible = false;
            }
            else if (rdoRemovePaperSet.Checked)
            {
                pnlAddPaperSet.Visible = false;
                pnlUpdatePaperSet.Visible = false;
                btnRemovePaperSet.Visible = true;
            }
        }
        private void lstPaperSets_SelectedIndexChanged(object sender, EventArgs e)
        {
            // if selection is changed on paper sets listbox
            if (lstPaperSets.SelectedIndex == -1)
            {
                // no paper set selected
                btnRemovePaperSet.Visible = false;
                pnlAddPaperSet.Visible = false;
                pnlUpdatePaperSet.Visible = false;
            }
            else
            {
                paperName = PaperSet.CurrentPaperSets[lstPaperSets.SelectedIndex].Description;
                paperID = PaperSet.CurrentPaperSets[lstPaperSets.SelectedIndex].PaperSetID;
                paperPrice = PaperSet.CurrentPaperSets[lstPaperSets.SelectedIndex].Price;
                paperRedemptAvail = PaperSet.CurrentPaperSets[lstPaperSets.SelectedIndex].RedemptionAvailable;
                paperRedemptPrice = PaperSet.CurrentPaperSets[lstPaperSets.SelectedIndex].RedemptionPrice;

                txtPaperSetName.Text = paperName;
                txtPaperSetPrice.Text = paperPrice.ToString("n2");
                chkRedemptionAvailable.Checked = paperRedemptAvail;
                txtPaperSetRedemptionPrice.Text = paperRedemptPrice.ToString("n2");

               

                if (rdoUpdatePaperSet.Checked)
                    pnlUpdatePaperSet.Visible = true;
                if (rdoRemovePaperSet.Checked)
                    btnRemovePaperSet.Visible = true;
            }
        }
        private void UpdatePaperSetListBox()
        {
            DbOps.FillPaperSetsTable(templateID);
            // add games to paper set listbox
            lstPaperSets.Items.Clear();
            foreach (PaperSet s in PaperSet.CurrentPaperSets)
            {
                if (s.TemplateID == templateID)
                    lstPaperSets.Items.Add(s.Description);
            }
        }
        private void btnAddPaperSet_Click(object sender, EventArgs e)
        {
            // if user clicks add paper set button
            if (txtAddPaperSetName.Text.Trim().Length < 1)
            {
                // check to make sure a name has been added, error if not
                MessageBox.Show("A paper set name is required to add a new paper set.");
                return;
            }
            // add game with new name
            string name = txtAddPaperSetName.Text;
            DbOps.AddPaperSet(templateID, name);
            UpdatePaperSetListBox();
            MessageBox.Show("Paper set, " + name + ", has been added to the template, " + templateName + ".\n" +
                "Make sure to update paper set attributes.");
            txtAddPaperSetName.Clear();
        }
        private void btnRemovePaperSet_Click(object sender, EventArgs e)
        {
            // if user clicks remove paper set button
            if (lstPaperSets.SelectedIndex == -1)
            {
                // check for paper selection and return error if none selected
                MessageBox.Show("No paper set selected.");
                return;
            }
            DialogResult dlg = MessageBox.Show("Are you sure you want to remove this paper set? Removal cannot be reversed.", "WARNING - CONFIRMATION", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dlg == DialogResult.Yes)
            {
                // see if user is sure they want to delete and if so, remove from database
                
                int id = PaperSet.CurrentPaperSets[lstPaperSets.SelectedIndex].PaperSetID;
                string name = PaperSet.CurrentPaperSets[lstPaperSets.SelectedIndex].Description;
                DbOps.DeletePaperSet(id, name);
                UpdatePaperSetListBox();
                MessageBox.Show("Paper set, " + name + ", has been removed permanently from the database.");
            }
        }
        private void btnUpdatePaperSet_Click(object sender, EventArgs e)
        {
            // first validate all fields for being filled
            string name = txtPaperSetName.Text.Trim();
            string price = GetNumberOnly(txtPaperSetPrice.Text.Trim());
            bool redemptAvail = chkRedemptionAvailable.Checked;
            string redemptPrice = GetNumberOnly(txtPaperSetRedemptionPrice.Text.Trim());
            
            if (name.Length < 1 || price.Length < 1 || redemptPrice.Length < 1)
            {
                MessageBox.Show("All attributes must be filled in to update paper set.");
                return;
            }
            // make sure all quantities are numbers
            double dblPrice;
            double dblRedemptPrice;

            bool allInts = true;
            bool isDbl = double.TryParse(price, out dblPrice);
            if (!isDbl)
                allInts = false;
            bool idDbl = double.TryParse(redemptPrice, out dblRedemptPrice);
            if (!isDbl)
                allInts = false;

            if (!allInts)
            {
                MessageBox.Show("Prices must be a numbers.");
                return;
            }

            // update
            DbOps.UpdatePaperSet(paperID, name, dblPrice, redemptAvail, dblRedemptPrice);
            MessageBox.Show("Paper set, " + name + ", successfully updated.");
            UpdatePaperSetListBox();
        }
        private void txtNewCharityName_KeyDown(object sender, KeyEventArgs e)
        {
            // if user in textbox and hits enter, press button
            if (e.KeyCode == Keys.Enter)
            {
                btnAddCharity.PerformClick();
                e.Handled = true;
            }
        }
        private void txtCharityName_KeyDown(object sender, KeyEventArgs e)
        {
            // if user in textbox and hits enter, press button
            if (e.KeyCode == Keys.Enter)
            {
                btnUpdateCharity.PerformClick();
                e.Handled = true;
            }
        }
        private void rdoCharities_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoAddCharity.Checked)
            {
                pnlAddCharity.Visible = true;
                pnlUpdateCharity.Visible = false;
                btnRemoveCharity.Visible = false;
                lstCharities.ClearSelected();
            }
            else if (lstCharities.SelectedIndex == -1)
            {
                pnlAddCharity.Visible = false;
                pnlUpdateCharity.Visible = false;
                btnRemoveCharity.Visible = false;
            }
            else if (rdoUpdateCharity.Checked)
            {

                pnlAddCharity.Visible = false;
                pnlUpdateCharity.Visible = true;
                btnRemoveCharity.Visible = false;
            }
            else if (rdoRemoveCharity.Checked)
            {
                pnlAddCharity.Visible = false;
                pnlUpdateCharity.Visible = false;
                btnRemoveCharity.Visible = true;
            }
        }
        private void lstCharities_SelectedIndexChanged(object sender, EventArgs e)
        {
            // if selection is changed on paper sets listbox
            if (lstCharities.SelectedIndex == -1)
            {
                // no paper set selected
                btnRemoveCharity.Visible = false;
                pnlAddCharity.Visible = false;
                pnlUpdateCharity.Visible = false;
            }
            else
            {
                charityTaxID = DbOps.Charities[lstCharities.SelectedIndex].TaxID;
                charityName = DbOps.Charities[lstCharities.SelectedIndex].Name;

                txtCharityName.Text = charityName;
                txtCharityTaxID.Text = charityTaxID;
                
                if (rdoUpdateCharity.Checked)
                    pnlUpdateCharity.Visible = true;
                if (rdoRemoveCharity.Checked)
                    btnRemoveCharity.Visible = true;
            }
        }
        private void UpdateCharitiesListBox()
        {
            DbOps.FillCharitiesTable();
            // add games to paper set listbox
            lstCharities.Items.Clear();
            foreach (Charity c in DbOps.Charities)
            {
                lstCharities.Items.Add(c.Name);
            }
        }
        private void btnAddCharity_Click(object sender, EventArgs e)
        {
            // if user clicks add charity button
            if (txtNewCharityName.Text.Trim().Length < 1)
            {
                // check to make sure a name has been added, error if not
                MessageBox.Show("A charity name is required to add a worker.");
                return;
            }
            if (txtNewCharityTaxID.Text.Trim().Length < 1)
            {
                // check to make sure a name has been added, error if not
                MessageBox.Show("A charity tax ID is required to add a new worker.");
                return;
            }
            // add game with new name
            string name = txtNewCharityName.Text.Trim();
            string taxID = txtNewCharityTaxID.Text.Trim();
            foreach(char ch in taxID)
            {
                if(!Char.IsDigit(ch))
                {
                    MessageBox.Show("The tax ID must be entirely numeric. Normal tax IDs are 11 digits.");
                    return;
                }
            }
            DbOps.AddCharity(taxID, name);
            UpdateCharitiesListBox();
            MessageBox.Show("Charity, " + name + ", has been added.");
            txtNewCharityName.Clear();
            txtNewCharityTaxID.Clear();
        }
        private void btnRemoveCharity_Click(object sender, EventArgs e)
        {
            // if user clicks remove charity button
            if (lstCharities.SelectedIndex == -1)
            {
                // check for charity selection and return error if none selected
                MessageBox.Show("No charity selected.");
                return;
            }
            DialogResult dlg = MessageBox.Show("Are you sure you want to remove this charity? Removal cannot be reversed.", "WARNING - CONFIRMATION", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dlg == DialogResult.Yes)
            {
                // see if user is sure they want to delete and if so, remove from database

                string id = DbOps.Charities[lstCharities.SelectedIndex].TaxID;
                string name = DbOps.Charities[lstCharities.SelectedIndex].Name;
                DbOps.DeleteCharity(id, name);
                UpdateCharitiesListBox();
                MessageBox.Show("Charity, " + name + ", has been removed permanently from the database.");
            }
        }
        private void btnUpdateCharity_Click(object sender, EventArgs e)
        {
            // first validate all fields for being filled
            string name = txtCharityName.Text.Trim();
            string id = txtCharityTaxID.Text.Trim();
           

            if (name.Length < 1 || id.Length < 1)
            {
                MessageBox.Show("All attributes must be filled in to update charity.");
                return;
            }
            // make sure all quantities are numbers

            bool allInts = true;
            foreach(char ch in id)
            {
                if (!Char.IsDigit(ch))
                    allInts = false;
            }

            if (!allInts)
            {
                MessageBox.Show("TaxID must be a numbers.");
                return;
            }

            // update
            DbOps.UpdateCharity(charityTaxID, id, name);
            MessageBox.Show("Charity, " + name + ", successfully updated.");
            UpdateCharitiesListBox();
        }
        private void txtAddWorkerName_KeyDown(object sender, KeyEventArgs e)
        {
            // if user in textbox and hits enter, press button
            if (e.KeyCode == Keys.Enter)
            {
                btnAddWorker.PerformClick();
                e.Handled = true;
            }
        }
        private void txtWorkerName_KeyDown(object sender, KeyEventArgs e)
        {
            // if user in textbox and hits enter, press button
            if (e.KeyCode == Keys.Enter)
            {
                btnUpdateWorker.PerformClick();
                e.Handled = true;
            }
        }
        private void rdoWorker_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoAddWorker.Checked)
            {
                pnlAddWorker.Visible = true;
                pnlUpdateWorker.Visible = false;
                btnRemoveWorker.Visible = false;
                lstWorkers.ClearSelected();
            }
            else if (lstWorkers.SelectedIndex == -1)
            {
                pnlAddWorker.Visible = false;
                pnlUpdateWorker.Visible = false;
                btnRemoveWorker.Visible = false;
            }
            else if (rdoUpdateWorker.Checked)
            {

                pnlAddWorker.Visible = false;
                pnlUpdateWorker.Visible = true;
                btnRemoveWorker.Visible = false;
            }
            else if (rdoRemoveWorker.Checked)
            {
                pnlAddWorker.Visible = false;
                pnlUpdateWorker.Visible = false;
                btnRemoveWorker.Visible = true;
            }
        }
        private void lstWorkers_SelectedIndexChanged(object sender, EventArgs e)
        {
            // if selection is changed on workers listbox
            if (lstWorkers.SelectedIndex == -1)
            {
                // no paper set selected
                btnRemoveWorker.Visible = false;
                pnlAddWorker.Visible = false;
                pnlUpdateWorker.Visible = false;
            }
            else
            {
                workerName = DbOps.Workers[lstWorkers.SelectedIndex].Name;
                txtWorkerName.Text = workerName;
                
                if (rdoUpdateWorker.Checked)
                    pnlUpdateWorker.Visible = true;
                if (rdoRemoveWorker.Checked)
                    btnRemoveWorker.Visible = true;
            }
        }
        private void UpdateWorkersListBox()
        {
            DbOps.FillWorkersTable();
            // add games to paper set listbox
            lstWorkers.Items.Clear();
            foreach (Worker w in DbOps.Workers)
            {
                lstWorkers.Items.Add(w.Name);
            }
        }
        private void btnAddWorker_Click(object sender, EventArgs e)
        {
            // if user clicks add worker button
            if (txtAddWorkerName.Text.Trim().Length < 1)
            {
                // check to make sure a name has been added, error if not
                MessageBox.Show("A worker name is required to add a new worker.");
                return;
            }
            // add worker with new name
            string name = txtAddWorkerName.Text.Trim();
            try
            {
                DbOps.AddWorker(name);
                UpdateWorkersListBox();
                MessageBox.Show("Worker, " + name + ", has been added.");
            }
            catch (Exception) { }
            txtAddWorkerName.Clear();
        }
        private void btnRemoveWorker_Click(object sender, EventArgs e)
        {
            // if user clicks remove worker button
            if (lstWorkers.SelectedIndex == -1)
            {
                // check for worker selection and return error if none selected
                MessageBox.Show("No worker selected.");
                return;
            }
            string name = DbOps.Workers[lstWorkers.SelectedIndex].Name;
            DbOps.DeleteWorker(name);
            UpdateWorkersListBox();
            MessageBox.Show("Worker, " + name + ", has been removed from the database.");
        }
        private void btnUpdateWorker_Click(object sender, EventArgs e)
        {
            // first validate all fields for being filled
            string name = txtWorkerName.Text.Trim();


            if (name.Length < 1)
            {
                MessageBox.Show("All attributes must be filled in add to worker.");
                return;
            }
            // update
            DbOps.UpdateWorker(workerName, name);
            MessageBox.Show("Worker, " + name + ", successfully updated.");
            UpdateWorkersListBox();
        }
        private void txtNewPullTabName_KeyDown(object sender, KeyEventArgs e)
        {
            // if user in textbox and hits enter, press button
            if (e.KeyCode == Keys.Enter)
            {
                btnAddPullTab.PerformClick();
                e.Handled = true;
            }
        }
        private void txtPullTabTax_KeyDown(object sender, KeyEventArgs e)
        {
            // if user in textbox and hits enter, press button
            if (e.KeyCode == Keys.Enter)
            {
                btnUpdatePullTab.PerformClick();
                e.Handled = true;
            }
        }
        private void rdoPullTabs_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoAddPullTab.Checked)
            {
                pnlAddPullTab.Visible = true;
                pnlUpdatePullTab.Visible = false;
                btnRemovePullTab.Visible = false;
                lstPullTabs.ClearSelected();
            }
            else if (lstPullTabs.SelectedIndex == -1)
            {
                pnlAddPullTab.Visible = false;
                pnlUpdatePullTab.Visible = false;
                btnRemovePullTab.Visible = false;
            }
            else if (rdoUpdatePullTab.Checked)
            {

                pnlAddPullTab.Visible = false;
                pnlUpdatePullTab.Visible = true;
                btnRemovePullTab.Visible = false;
            }
            else if (rdoRemovePullTab.Checked)
            {
                pnlAddPullTab.Visible = false;
                pnlUpdatePullTab.Visible = false;
                btnRemovePullTab.Visible = true;
            }
        }
        private void lstPullTabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            // if selection is changed on pull tabs listbox
            if (lstPullTabs.SelectedIndex == -1)
            {
                // no pull tab selected
                btnRemovePullTab.Visible = false;
                pnlAddPullTab.Visible = false;
                pnlUpdatePullTab.Visible = false;
            }
            else
            {
                pullTabName = DbOps.PullTabs[lstPullTabs.SelectedIndex].Description;
                pullTabFormID = DbOps.PullTabs[lstPullTabs.SelectedIndex].FormID;
                pullTabGross = DbOps.PullTabs[lstPullTabs.SelectedIndex].Gross;
                pullTabPrizes = DbOps.PullTabs[lstPullTabs.SelectedIndex].Prizes;
                pullTabTax = DbOps.PullTabs[lstPullTabs.SelectedIndex].Tax;
                pullTabArchive = !DbOps.PullTabs[lstPullTabs.SelectedIndex].Current;

                txtPullTabName.Text = pullTabName;
                txtPullTabFormID.Text = pullTabFormID;
                txtPullTabGross.Text = pullTabGross.ToString("n2");
                txtPullTabPrizes.Text = pullTabPrizes.ToString("n2");
                txtPullTabTax.Text = pullTabTax.ToString("n2");
                chkArchive.Checked = pullTabArchive;

                if (rdoUpdatePullTab.Checked)
                    pnlUpdatePullTab.Visible = true;
                if (rdoRemovePullTab.Checked)
                    btnRemovePullTab.Visible = true;
            }
        }
        private void UpdatePullTabsListBox()
        {
            DbOps.FillPullTabsTable();
            // add pull tab to paper set listbox
            lstPullTabs.Items.Clear();
            foreach (PullTab pt in DbOps.PullTabs)
            {
                lstPullTabs.Items.Add(pt.Description);
            }
        }
        private void btnAddPullTab_Click(object sender, EventArgs e)
        {
            // if user clicks add pulltab button
            if (txtNewPullTabName.Text.Trim().Length < 1)
            {
                // check to make sure a name has been added, error if not
                MessageBox.Show("A pull tab name is required to add a new pull tab.");
                return;
            }
            if (txtNewPullTabFormID.Text.Trim().Length < 1)
            {
                // check to make sure a name has been added, error if not
                MessageBox.Show("A pull tab form ID is required to add a new pull tab.");
                return;
            }
            // add pull ab with new name
            string name = txtNewPullTabName.Text.Trim();
            string formid = txtNewPullTabFormID.Text.Trim();

            try
            {
                DbOps.AddPullTab(formid,name);
                UpdatePullTabsListBox();
                MessageBox.Show("Pull tab, " + name + ", has been added.\n" + 
                    "Make sure to update pull tab information.");
            }
            catch (Exception) { }
            txtNewPullTabName.Clear();
            txtNewPullTabFormID.Clear();
        }
        private void btnRemovePullTab_Click(object sender, EventArgs e)
        {
            // if user clicks remove pulltab button
            if (lstPullTabs.SelectedIndex == -1)
            {
                // check for pulltab selection and return error if none selected
                MessageBox.Show("No pull tab selected.");
                return;
            }
            DialogResult dlg = MessageBox.Show("Are you sure you want to remove this pull tab? Removal cannot be reversed.", "WARNING - CONFIRMATION", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dlg == DialogResult.Yes)
            {
                // see if user is sure they want to delete and if so, remove from database

                string name = DbOps.PullTabs[lstPullTabs.SelectedIndex].Description;
                string formid = DbOps.PullTabs[lstPullTabs.SelectedIndex].FormID;
                DbOps.DeletePullTab(formid, name);
                UpdatePullTabsListBox();
                MessageBox.Show("Pull tab, " + name + ", has been removed from the database.");
            }
        }
        private void btnUpdatePullTab_Click(object sender, EventArgs e)
        {
            // first validate all fields for being filled
            string name = txtPullTabName.Text.Trim();
            string formid = txtPullTabFormID.Text.Trim();
            string gross = GetNumberOnly(txtPullTabGross.Text.Trim());
            string prizes = GetNumberOnly(txtPullTabPrizes.Text.Trim());
            string tax = GetNumberOnly(txtPullTabTax.Text.Trim());
            bool archive = chkArchive.Checked;

            if (name.Length < 1 || formid.Length < 1 || gross.Length < 1 || prizes.Length < 1 || tax.Length < 1)
            {
                MessageBox.Show("All attributes must be filled in add to pull tab.");
                return;
            }
            double dblGross = 0;
            double dblPrizes = 0;
            double dblTax = 0;

            // check for numeric values
            bool allNumeric = true;
            bool isNumeric = double.TryParse(gross, out dblGross);
            if (!isNumeric)
                allNumeric = false;
            isNumeric = double.TryParse(prizes, out dblPrizes);
            if (!isNumeric)
                allNumeric = false;
            isNumeric = double.TryParse(tax, out dblTax);
            if (!isNumeric)
                allNumeric = false;
            if(!allNumeric)
            {
                MessageBox.Show("Gross, prizes, and tax must all be numeric values.");
                return;
            }
            // update
            DbOps.UpdatePullTab(pullTabFormID, formid, name, dblGross, dblPrizes, dblTax, !archive);
            txtPullTabName.Text = name;
            txtPullTabFormID.Text = formid;
            txtPullTabGross.Text = dblGross.ToString("n2");
            txtPullTabPrizes.Text = dblPrizes.ToString("n2");
            txtPullTabTax.Text = dblTax.ToString("n2");
            chkArchive.Checked = archive;

            MessageBox.Show("Pull tab, " + name + ", successfully updated.");
            UpdatePullTabsListBox();
        }
        private string GetNumberOnly(string number)
        {
            string newNum = "";
            foreach(char ch in number)
            {
                if (Char.IsNumber(ch) || ch == '.')
                    newNum += ch;
            }
            return newNum;
        }
        private void txtEmail_KeyDown(object sender, KeyEventArgs e)
        {
            // if user in textbox and hits enter, press button
            if (e.KeyCode == Keys.Enter)
            {
                btnUpdateDefaults.PerformClick();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }
        private void btnUpdateDefaults_Click(object sender, EventArgs e)
        {
            // first validate all fields for being filled
            string email = txtEmail.Text.Trim();
            string numWorkers = nudDefaultNumWorkers.Value.ToString();
            string taxRate = nudDefaultTaxRate.Value.ToString("n2");

            bool validEmail = true;
            if (email.Length < 1)
            {
                // email not entered so error message
                MessageBox.Show("Email must be filled in to update defaults correctly.");
                FillDefaults();
                txtEmail.Focus();
                txtEmail.SelectAll();
                return;
            }
            try
            {
                // easiest way to validate email :)
                MailAddress m = new MailAddress(email);
            }
            catch (FormatException)
            {
                validEmail = false;
            }

            if (!validEmail)
            {
                MessageBox.Show("Invalid email. Try again.");
                txtEmail.Focus();
                txtEmail.SelectAll();
                return;
            }

            // update
            DbOps.UpdateDefault(email, "Email");
            DbOps.UpdateDefault(numWorkers, "NumFloorWorkers");
            DbOps.UpdateDefault(taxRate, "TaxRate");
            // update defaults table and fields
            FillDefaults();

            MessageBox.Show("Defaults updated.\n\n" +
                "Email: " + DbOps.Email + "\n" +
                "Number of Floor Workers: " + DbOps.NumFloorWorkers.ToString() + "\n" +
                "Tax Rate: " + DbOps.TaxRate.ToString("n2"));
        }

        private void chkRedemptionAvailable_CheckedChanged(object sender, EventArgs e)
        {
            // if user unchecks this box remove the contents of the rememption price textbox
            // and replace it with the value - 0
            if (!chkRedemptionAvailable.Checked)
            {
                txtPaperSetRedemptionPrice.Text = "0.00";
            }
        }

        private void mnuWizard_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
}
