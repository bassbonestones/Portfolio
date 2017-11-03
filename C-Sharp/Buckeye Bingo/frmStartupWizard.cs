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
    public partial class frmStartupWizard : Form
    {
        // form that allows user to get a quick start with the bingo session by entering in all the starting info

        // bool to test for a closing event that is a user submission
        bool isSubmitClose = false;
        // important counters
        int tabIndexCtr = 0;
        int qtyPullTabs = 3;
        int qtyFloorWorkers = 3;
        // lists for the dynamic controls of floor cards section
        List<Label> lbls = new List<Label>();
        List<TextBox> ser1txts = new List<TextBox>();
        List<TextBox> ser2txts = new List<TextBox>();
        List<PictureBox> addPbxs = new List<PictureBox>();
        List<PictureBox> subPbxs = new List<PictureBox>();
        List<Panel> ser1pnls = new List<Panel>();
        List<Panel> ser2pnls = new List<Panel>();

        public frmStartupWizard()
        {
            // constructor for form
            InitializeComponent();
            DbOps.FillDefaultsTable();
            // set location of main panel to center of form
            pnlWiz.Location = new Point(this.Width / 2 - pnlWiz.Width / 2, pnlWiz.Location.Y);
            // fill the tables needed to start the wizard functionality
            DbOps.FillCharitiesTable();
            DbOps.FillSessionTemplatesTable();
            DbOps.FillWorkersTable();
            DbOps.FillPullTabsTable();
            foreach (Charity c in DbOps.Charities)
            {
                // add charities to combo box
                cbxCharities.Items.Add(c.Name);
            }
            foreach (SessionTemplate st in DbOps.SessionTemplates)
            {
                // add session template (programs) to combo box
                cbxPrograms.Items.Add(st.TemplateName);
            }
            foreach (Worker w in DbOps.Workers)
            {
                // add worker names to combo boxes
                cbxFloor1.Items.Add(w.Name);
                cbxFloor2.Items.Add(w.Name);
                cbxFloor3.Items.Add(w.Name);
                cbxFloor4.Items.Add(w.Name);
                cbxDeskWorker.Items.Add(w.Name);
                cbxPullTabDeskWorker.Items.Add(w.Name);
            }
            foreach (PullTab pt in DbOps.PullTabs)
            {
                // add pull tabs to combo boxes
                cbxPT1.Items.Add(pt.Description);
                cbxPT2.Items.Add(pt.Description);
                cbxPT3.Items.Add(pt.Description);
                cbxPT4.Items.Add(pt.Description);
                cbxPT5.Items.Add(pt.Description);
                cbxPT6.Items.Add(pt.Description);
            }
            // update based on the number of floor workers
            switch(DbOps.NumFloorWorkers)
            {
                case 1:
                    pbxRemoveFloor_Click(new object(), new EventArgs());
                    pbxRemoveFloor_Click(new object(), new EventArgs());
                    break;
                case 2:
                    pbxRemoveFloor_Click(new object(), new EventArgs());
                    break;
                case 3:
                    // do nothing.
                    break;
                case 4:
                    pbxAddFloor_Click(new object(), new EventArgs());
                    break;
                default: // n/a
                    break;
            }
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

        public void AddPictureBoxAdd(string name, Control.ControlCollection cc, int w, int h, Point p)
        {
            // method used to add new plus sign picturebox at runtime for floor cards
            PictureBox pbx = new PictureBox();
            pbx.Size = new Size(w, h);
            pbx.BackgroundImageLayout = ImageLayout.Zoom;
            pbx.BackgroundImage = Image.FromFile(@"C:\Users\Administrator\AppData\Roaming\Jeremy Stones\Images\plus.png");
            pbx.Tag = name;
            cc.Add(pbx);
            pbx.Location = p;
            pbx.Name = addPbxs.Count.ToString();
            addPbxs.Add(pbx);
            // lambda function to pass extra info to event handler
            pbx.Click += new EventHandler((s, e) => EnableSecondFCSerial(s, e, pbx));
        }
        public void AddPictureBoxSub(string name, Control.ControlCollection cc, int w, int h, Point p)
        {
            // method used to add new minus sign picturebox at runtime for floor cards
            PictureBox pbx = new PictureBox();
            pbx.Size = new Size(w, h);
            pbx.BackgroundImageLayout = ImageLayout.Zoom;
            pbx.BackgroundImage = Image.FromFile(@"C:\Users\Administrator\AppData\Roaming\Jeremy Stones\Images\null.png");
            pbx.Tag = name;
            cc.Add(pbx);
            pbx.Location = p;
            pbx.Name = subPbxs.Count.ToString();
            subPbxs.Add(pbx);
            // lambda function to pass extra info to event handler
            pbx.Click += new EventHandler((s, e) => DisableSecondFCSerial(s, e, pbx));
        }
        public void AddColorLabel(string name, Control.ControlCollection cc, Color col, int w, int h, Point p)
        {
            // method to create labels for each of the floor cards created dynamically at runtime
            Panel pnl = new Panel();
            pnl.Width = w;
            pnl.Height = h;
            pnl.BackColor = col;
            Label lbl = new Label();
            lbl.Text = name;
            lbl.Tag = name;
            lbl.Width = w - 8;
            lbl.Height = h - 8;
            lbl.BackColor = Color.FromArgb(220, 220, 220);
            lbl.TextAlign = ContentAlignment.MiddleCenter;
            pnl.Tag = ".";
            pnl.Controls.Add(lbl);
            lbl.Location = new Point(4, 4);
            cc.Add(pnl);
            pnl.Location = p;
            lbls.Add(lbl);
        }
        public void AddColorTextBox(string name, Control.ControlCollection cc, Color col, int w, int h, Point p, bool isFirstTxt)
        {
            // method to add textbox controls at runtime to floor cards section

            // get index from previous picturebox - used to get background color of panel
            int index = int.Parse((subPbxs.Count - 1).ToString());
            // create panel behind textbox
            Panel pnl = new Panel();
            pnl.Width = w;
            pnl.Height = h;
            pnl.BackColor = col;
            pnl.Tag = name;
            // create textbox
            TextBox txt = new TextBox();
            txt.Width = w - 8;
            txt.Height = h - 8;
            txt.MaxLength = 8;
            txt.TabIndex = tabIndexCtr++;
            txt.Tag = name;
            // add event handlers with lambda
            txt.PreviewKeyDown += new PreviewKeyDownEventHandler((s, e) => MoveBoxes(s, e, txt, pnl));
            txt.Enter += new EventHandler((s, e) => txt.SelectAll());
            txt.KeyPress += new KeyPressEventHandler((s, e) => StoreTextInPanelTag(s, e, txt, pnl, name));
            pnl.Enter += new EventHandler((s, e) => SendFocusToTxt(s, e, txt));

            // add textbox to panel
            pnl.Controls.Add(txt);
            txt.Location = new Point(4, 4);
            // add panel to floor cards panel
            cc.Add(pnl);
            // set this panel's location in floor cards panel
            pnl.Location = p;
            if (isFirstTxt)
            {
                // if a serial1 textbox add to appropriate list
                ser1txts.Add(txt);
                ser1pnls.Add(pnl);
            }
            else
            {
                // if a serial2 textbox, add to appropriate list
                ser2txts.Add(txt);
                ser2pnls.Add(pnl);
                pnl.Enabled = false;
                txt.BackColor = FloorCard.CurrentFloorCards[index].BackColor;
            }

        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            // properly exit the application
            Application.Exit();
        }
        public void SendFocusToTxt(object sender, EventArgs e, TextBox t)
        {
            // method to trick focus from parent panel into textbox inside it
            t.Focus();
        }
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            // When users submits, test every field and set flags and messages to reflect the current state of field values
            // There is a lot of repetitive code, so I didn't bother documenting it all

            // message string
            string missingMessage = "";

            // bools for every possible form field's existance, initialized to appropriate values
            bool hasProgram = true;
            bool hasCharity = true;
            bool hasFloor1 = true;
            bool hasFloor2 = false;
            if (qtyFloorWorkers > 1)
                hasFloor2 = true;
            bool hasFloor3 = false;
            if (qtyFloorWorkers > 2)
                hasFloor3 = true;
            bool hasFloor4 = false;
            if (qtyFloorWorkers > 3)
                hasFloor4 = true;
            bool hasPullTabDeskWorker = true;
            bool hasDeskWorker = true;
            List<bool> hasFloorCardSerial1 = new List<bool>();
            List<bool> hasFloorCardSerial2 = new List<bool>();
            bool hasPaperSet1Serial1 = false;
            bool hasPaperSet1Qty1 = false;
            bool hasPaperSet1Start1 = false;
            bool hasPaperSet1Serial2 = false;
            bool hasPaperSet1Qty2 = false;
            bool hasPaperSet1Start2 = false;
            if (PaperSet.QtyPaperSets > 0)
            {
                hasPaperSet1Serial1 = true;
                hasPaperSet1Qty1 = true;
                hasPaperSet1Start1 = true;
                if (PaperSet.CurrentPaperSets[0].QtySerials == 2)
                {
                    hasPaperSet1Serial2 = true;
                    hasPaperSet1Qty2 = true;
                    hasPaperSet1Start2 = true;
                }
            }
            bool hasPaperSet2Serial1 = false;
            bool hasPaperSet2Qty1 = false;
            bool hasPaperSet2Start1 = false;
            bool hasPaperSet2Serial2 = false;
            bool hasPaperSet2Qty2 = false;
            bool hasPaperSet2Start2 = false;
            if (PaperSet.QtyPaperSets > 1)
            {
                hasPaperSet2Serial1 = true;
                hasPaperSet2Qty1 = true;
                hasPaperSet2Start1 = true;
                if (PaperSet.CurrentPaperSets[1].QtySerials == 2)
                {
                    hasPaperSet2Serial2 = true;
                    hasPaperSet2Qty2 = true;
                    hasPaperSet2Start2 = true;
                }
            }
            bool hasPaperSet3Serial1 = false;
            bool hasPaperSet3Qty1 = false;
            bool hasPaperSet3Start1 = false;
            bool hasPaperSet3Serial2 = false;
            bool hasPaperSet3Qty2 = false;
            bool hasPaperSet3Start2 = false;
            bool hasPT1Name = true;
            bool hasPT1Serial = true;
            bool hasPT2Name = false;
            bool hasPT2Serial = false;
            if (qtyPullTabs > 1)
            {
                hasPT2Name = true;
                hasPT2Serial = true;
            }
            bool hasPT3Name = false;
            bool hasPT3Serial = false;
            if (qtyPullTabs > 2)
            {
                hasPT3Name = true;
                hasPT3Serial = true;
            }
            bool hasPT4Name = false;
            bool hasPT4Serial = false;
            if (qtyPullTabs > 3)
            {
                hasPT4Name = true;
                hasPT4Serial = true;
            }
            bool hasPT5Name = false;
            bool hasPT5Serial = false;
            if (qtyPullTabs > 4)
            {
                hasPT5Name = true;
                hasPT5Serial = true;
            }
            bool hasPT6Name = false;
            bool hasPT6Serial = false;
            if (qtyPullTabs > 5)
            {
                hasPT6Name = true;
                hasPT6Serial = true;
            }
            if (PaperSet.QtyPaperSets > 2)
            {
                hasPaperSet3Serial1 = true;
                hasPaperSet3Qty1 = true;
                hasPaperSet3Start1 = true;
                if (PaperSet.CurrentPaperSets[2].QtySerials == 2)
                {
                    hasPaperSet3Serial2 = true;
                    hasPaperSet3Qty2 = true;
                    hasPaperSet3Start2 = true;
                }
            }

            // now that bools are set, test each variable quantity and add missing fields to message
            // as well as toggle associated flag
            if (cbxPrograms.SelectedIndex == -1)
            {
                // if no program selected end event and give error message
                MessageBox.Show("A session program must be selected before submitting.");
                return;
            }
            if (cbxCharities.SelectedIndex == -1)
            {
                missingMessage += "No charity has been selected.\n";
                hasCharity = false;
            }
            if (cbxFloor1.Text.Trim().Length < 1)
            {
                missingMessage += "Floor worker 1 has not been selected.\n";
                hasFloor1 = false;
            }
            if (hasFloor2 && cbxFloor2.Text.Trim().Length < 1)
            {
                missingMessage += "Floor worker 2 has not been selected.\n";
                hasFloor2 = false;
            }
            if (hasFloor3 && cbxFloor3.Text.Trim().Length < 1)
            {
                missingMessage += "Floor worker 3 has not been selected.\n";
                hasFloor3 = false;
            }
            if (hasFloor4 && cbxFloor4.Text.Trim().Length < 1)
            {
                missingMessage += "Floor worker 4 has not been selected.\n";
                hasFloor4 = false;
            }
            if (cbxPullTabDeskWorker.Text.Trim().Length < 1)
            {
                missingMessage += "No pull tab desk worker has been selected.\n";
                hasPullTabDeskWorker = false;
            }
            if (cbxDeskWorker.Text.Trim().Length < 1)
            {
                missingMessage += "No desk worker has been selected.\n";
                hasDeskWorker = false;
            }
            int txtCtr = 0;
            foreach (TextBox tb in ser1txts)
            {
                bool tempBool = true;
                if (tb.Text.Trim() == "")
                {
                    tempBool = false;
                    missingMessage += "No first serial entered for " + lbls[txtCtr].Text + ".\n";
                }
                hasFloorCardSerial1.Add(tempBool);
                txtCtr++;
            }
            txtCtr = 0;
            foreach (TextBox tb in ser2txts)
            {
                bool tempBool = false;
                if (FloorCard.CurrentFloorCards[txtCtr].QtySerials == 2)
                    tempBool = true;
                if (tb.Text.Trim() == "" && FloorCard.CurrentFloorCards[txtCtr].QtySerials == 2)
                {
                    tempBool = false;
                    missingMessage += "No second serial entered for " + lbls[txtCtr].Text + ".\n";
                }
                hasFloorCardSerial2.Add(tempBool);
                txtCtr++;
            }
            if (PaperSet.QtyPaperSets > 0)
            {
                if (txtPaper1Serial1.Text.Trim() == "")
                {
                    missingMessage += "No first serial entered for " + gbxPaper1.Text + ".\n";
                    hasPaperSet1Serial1 = false;
                }
                if (nudPaper1Start1.Value == 0)
                {
                    missingMessage += "No first start number entered for " + gbxPaper1.Text + ".\n";
                    hasPaperSet1Start1 = false;
                }
                if (nudPaper1Qty1.Value == 0)
                {
                    missingMessage += "No first quantity entered for " + gbxPaper1.Text + ".\n";
                    hasPaperSet1Qty1 = false;
                }
                if (PaperSet.CurrentPaperSets[0].QtySerials == 2)
                {
                    if (txtPaper1Serial2.Text.Trim() == "")
                    {
                        missingMessage += "No second serial entered for " + gbxPaper1.Text + ".\n";
                        hasPaperSet1Serial2 = false;
                    }
                    if (nudPaper1Start2.Value == 0)
                    {
                        missingMessage += "No second start number entered for " + gbxPaper1.Text + ".\n";
                        hasPaperSet1Start2 = false;
                    }
                    if (nudPaper1Qty2.Value == 0)
                    {
                        missingMessage += "No second quantity entered for " + gbxPaper1.Text + ".\n";
                        hasPaperSet1Qty2 = false;
                    }
                }
            }
            if (PaperSet.QtyPaperSets > 1)
            {
                if (txtPaper2Serial1.Text.Trim() == "")
                {
                    missingMessage += "No first serial entered for " + gbxPaper2.Text + ".\n";
                    hasPaperSet2Serial1 = false;
                }
                if (nudPaper2Start1.Value == 0)
                {
                    missingMessage += "No first start number entered for " + gbxPaper2.Text + ".\n";
                    hasPaperSet2Start1 = false;
                }
                if (nudPaper2Qty1.Value == 0)
                {
                    missingMessage += "No first quantity entered for " + gbxPaper2.Text + ".\n";
                    hasPaperSet2Qty1 = false;
                }
                if (PaperSet.CurrentPaperSets[1].QtySerials == 2)
                {
                    if (txtPaper2Serial2.Text.Trim() == "")
                    {
                        missingMessage += "No second serial entered for " + gbxPaper2.Text + ".\n";
                        hasPaperSet2Serial2 = false;
                    }
                    if (nudPaper2Start2.Value == 0)
                    {
                        missingMessage += "No second start number entered for " + gbxPaper2.Text + ".\n";
                        hasPaperSet2Start2 = false;
                    }
                    if (nudPaper2Qty2.Value == 0)
                    {
                        missingMessage += "No second quantity entered for " + gbxPaper2.Text + ".\n";
                        hasPaperSet2Qty2 = false;
                    }
                }
            }
            if (PaperSet.QtyPaperSets > 2)
            {
                if (txtPaper3Serial1.Text.Trim() == "")
                {
                    missingMessage += "No first serial entered for " + gbxPaper3.Text + ".\n";
                    hasPaperSet3Serial1 = false;
                }
                if (nudPaper3Start1.Value == 0)
                {
                    missingMessage += "No first start number entered for " + gbxPaper3.Text + ".\n";
                    hasPaperSet3Start1 = false;
                }
                if (nudPaper3Qty1.Value == 0)
                {
                    missingMessage += "No first quantity entered for " + gbxPaper3.Text + ".\n";
                    hasPaperSet3Qty1 = false;
                }
                if (PaperSet.CurrentPaperSets[2].QtySerials == 2)
                {
                    if (txtPaper3Serial2.Text.Trim() == "")
                    {
                        missingMessage += "No second serial entered for " + gbxPaper3.Text + ".\n";
                        hasPaperSet3Serial2 = false;
                    }
                    if (nudPaper3Start2.Value == 0)
                    {
                        missingMessage += "No second start number entered for " + gbxPaper3.Text + ".\n";
                        hasPaperSet3Start2 = false;
                    }
                    if (nudPaper3Qty2.Value == 0)
                    {
                        missingMessage += "No second quantity entered for " + gbxPaper3.Text + ".\n";
                        hasPaperSet3Qty2 = false;
                    }
                }
            }
            if (cbxPT1.SelectedIndex == -1)
            {
                missingMessage += "First pull tab not selected.\n";
                hasPT1Name = false;
            }
            if (txtPTSerial1.Text.Trim() == "")
            {
                missingMessage += "No serial for first pull tab entered.\n";
                hasPT1Serial = false;
            }
            if (qtyPullTabs > 1)
            {
                if (cbxPT2.SelectedIndex == -1)
                {
                    missingMessage += "Second pull tab not selected.\n";
                    hasPT2Name = false;
                }
                if (txtPTSerial2.Text.Trim() == "")
                {
                    missingMessage += "No serial for second pull tab entered.\n";
                    hasPT2Serial = false;
                }
            }
            if (qtyPullTabs > 2)
            {
                if (cbxPT3.SelectedIndex == -1)
                {
                    missingMessage += "Third pull tab not selected.\n";
                    hasPT3Name = false;
                }
                if (txtPTSerial3.Text.Trim() == "")
                {
                    missingMessage += "No serial for third pull tab entered.\n";
                    hasPT3Serial = false;
                }
            }
            if (qtyPullTabs > 3)
            {
                if (cbxPT4.SelectedIndex == -1)
                {
                    missingMessage += "Fourth pull tab not selected.\n";
                    hasPT4Name = false;
                }
                if (txtPTSerial4.Text.Trim() == "")
                {
                    missingMessage += "No serial for fourth pull tab entered.\n";
                    hasPT4Serial = false;
                }
            }
            if (qtyPullTabs > 4)
            {
                if (cbxPT5.SelectedIndex == -1)
                {
                    missingMessage += "Fifth pull tab not selected.\n";
                    hasPT5Name = false;
                }
                if (txtPTSerial5.Text.Trim() == "")
                {
                    missingMessage += "No serial for fifth pull tab entered.\n";
                    hasPT5Serial = false;
                }
            }
            if (qtyPullTabs > 5)
            {
                if (cbxPT6.SelectedIndex == -1)
                {
                    missingMessage += "Sixth pull tab not selected.\n";
                    hasPT6Name = false;
                }
                if (txtPTSerial6.Text.Trim() == "")
                {
                    missingMessage += "No serial for sixth pull tab entered.\n";
                    hasPT6Serial = false;
                }
            }


            // if there is anything in the message string, inform the user and see if they want to continue or not
            if (missingMessage != "")
            {
                DialogResult dlg = MessageBox.Show("The following items are missing:\n\n" + missingMessage + "\nPress OK to continue anyways.\n" +
                    "Press CANCEL to return and make changes.", "Missing Information", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dlg == DialogResult.Cancel)
                {
                    return;
                }
            }
            //

            /////////////////
            // do some validation here, otherwise users could enter incorrect values for workers/pulltabs
            ////////////

            // use bools from above to insert data into appropriate database classes, which can be accessed from any
            // part of the application

            if (hasProgram)
                SessionTemplate.CurrentTemplate = DbOps.SessionTemplates[cbxPrograms.SelectedIndex];
            if (hasCharity)
                Charity.CurrentCharity = DbOps.Charities[cbxCharities.SelectedIndex];
            if (hasFloor1)
                Worker.CurrentWorkersList.Add(new Worker(cbxFloor1.Text));
            if (hasFloor2)
                Worker.CurrentWorkersList.Add(new Worker(cbxFloor2.Text));
            if (hasFloor3)
                Worker.CurrentWorkersList.Add(new Worker(cbxFloor3.Text));
            if (hasFloor4)
                Worker.CurrentWorkersList.Add(new Worker(cbxFloor4.Text));
            if (hasPullTabDeskWorker)
                Worker.CurrentPTDeskWorker = new Worker(cbxPullTabDeskWorker.Text);
            else
                Worker.CurrentPTDeskWorker = new Worker("PT Desk");
            if (hasDeskWorker)
                Worker.CurrentDeskWorker = new Worker(cbxDeskWorker.Text);
            else
                Worker.CurrentDeskWorker = new Worker("Desk Worker");


            for (int i = 0; i < hasFloorCardSerial1.Count; i++)
            {
                if (hasFloorCardSerial1[i])
                    FloorCard.CurrentFloorCards[i].Serial1 = ser1txts[i].Text;
                if (hasFloorCardSerial2[i])
                    FloorCard.CurrentFloorCards[i].Serial2 = ser2txts[i].Text;
            }
            if (hasPaperSet1Serial1)
                PaperSet.CurrentPaperSets[0].Serial1 = txtPaper1Serial1.Text;
            if (hasPaperSet1Serial2)
                PaperSet.CurrentPaperSets[0].Serial2 = txtPaper1Serial2.Text;
            if (hasPaperSet1Start1)
                PaperSet.CurrentPaperSets[0].StartNum1 = (int)nudPaper1Start1.Value;
            if (hasPaperSet1Start2)
                PaperSet.CurrentPaperSets[0].StartNum2 = (int)nudPaper1Start2.Value;
            if (hasPaperSet1Qty1)
                PaperSet.CurrentPaperSets[0].QtyIssued1 = (int)nudPaper1Qty1.Value;
            if (hasPaperSet1Qty2)
                PaperSet.CurrentPaperSets[0].QtyIssued2 = (int)nudPaper1Qty2.Value;

            if (hasPaperSet2Serial1)
                PaperSet.CurrentPaperSets[1].Serial1 = txtPaper2Serial1.Text;
            if (hasPaperSet2Serial2)
                PaperSet.CurrentPaperSets[1].Serial2 = txtPaper2Serial2.Text;
            if (hasPaperSet2Start1)
                PaperSet.CurrentPaperSets[1].StartNum1 = (int)nudPaper2Start1.Value;
            if (hasPaperSet2Start2)
                PaperSet.CurrentPaperSets[1].StartNum2 = (int)nudPaper2Start2.Value;
            if (hasPaperSet2Qty1)
                PaperSet.CurrentPaperSets[1].QtyIssued1 = (int)nudPaper2Qty1.Value;
            if (hasPaperSet2Qty2)
                PaperSet.CurrentPaperSets[1].QtyIssued2 = (int)nudPaper2Qty2.Value;

            if (hasPaperSet3Serial1)
                PaperSet.CurrentPaperSets[2].Serial1 = txtPaper3Serial1.Text;
            if (hasPaperSet3Serial2)
                PaperSet.CurrentPaperSets[2].Serial2 = txtPaper3Serial2.Text;
            if (hasPaperSet3Start1)
                PaperSet.CurrentPaperSets[2].StartNum1 = (int)nudPaper3Start1.Value;
            if (hasPaperSet3Start2)
                PaperSet.CurrentPaperSets[2].StartNum2 = (int)nudPaper3Start2.Value;
            if (hasPaperSet3Qty1)
                PaperSet.CurrentPaperSets[2].QtyIssued1 = (int)nudPaper3Qty1.Value;
            if (hasPaperSet3Qty2)
                PaperSet.CurrentPaperSets[2].QtyIssued2 = (int)nudPaper3Qty2.Value;

            if (hasPT1Name)
                PullTab.CurrentPullTabs.Add(DbOps.PullTabs[cbxPT1.SelectedIndex]);
            if (hasPT1Serial && hasPT1Name)
                PullTab.CurrentPullTabs[PullTab.CurrentPullTabs.Count - 1].Serial = txtPTSerial1.Text;

            if (hasPT2Name)
                PullTab.CurrentPullTabs.Add(DbOps.PullTabs[cbxPT2.SelectedIndex]);
            if (hasPT2Serial && hasPT2Name)
                PullTab.CurrentPullTabs[PullTab.CurrentPullTabs.Count - 1].Serial = txtPTSerial2.Text;

            if (hasPT3Name)
                PullTab.CurrentPullTabs.Add(DbOps.PullTabs[cbxPT3.SelectedIndex]);
            if (hasPT3Serial && hasPT3Name)
                PullTab.CurrentPullTabs[PullTab.CurrentPullTabs.Count - 1].Serial = txtPTSerial3.Text;

            if (hasPT4Name)
                PullTab.CurrentPullTabs.Add(DbOps.PullTabs[cbxPT4.SelectedIndex]);
            if (hasPT4Serial && hasPT4Name)
                PullTab.CurrentPullTabs[PullTab.CurrentPullTabs.Count - 1].Serial = txtPTSerial4.Text;

            if (hasPT5Name)
                PullTab.CurrentPullTabs.Add(DbOps.PullTabs[cbxPT5.SelectedIndex]);
            if (hasPT5Serial && hasPT5Name)
                PullTab.CurrentPullTabs[PullTab.CurrentPullTabs.Count - 1].Serial = txtPTSerial5.Text;

            if (hasPT6Name)
                PullTab.CurrentPullTabs.Add(DbOps.PullTabs[cbxPT6.SelectedIndex]);
            if (hasPT6Serial && hasPT6Name)
                PullTab.CurrentPullTabs[PullTab.CurrentPullTabs.Count - 1].Serial = txtPTSerial6.Text;
            isSubmitClose = true;
            frmMain main = new frmMain();
            main.Show();
            this.Close();
        }
        private void cbxPrograms_SelectedIndexChanged(object sender, EventArgs e)
        {
            // method to handle change in selecton of program

            // clean up old stuff
            ClearFloorStuff();
            ClearPaperSetStuff();

            // get index
            int index = cbxPrograms.SelectedIndex;
            int templateID = DbOps.SessionTemplates[index].TemplateID;

            // fill appropriate datasets
            DbOps.FillFloorCardsTable(templateID);
            DbOps.FillPaperSetsTable(templateID);
            DbOps.FillElectronicsTable(templateID);
            DbOps.FillSessionBingoGamesTable(templateID);
            // position for control addition to panel
            int XX = 0;
            int YY = 10;
            // to set number of serials to 1 for every floor card (changes if serial added manually)
            int ctr = 0;
            // hide panel so dynamic control addition won't look glitchy
            pnlFloorCards.Visible = false;

            foreach (FloorCard f in FloorCard.CurrentFloorCards)
            {
                // loop through every floor card and dynamically create a label, pictures, and textboxes for each
                // set QtySerials to 1
                FloorCard.CurrentFloorCards[ctr++].QtySerials = 1;
                // add controls for floor card
                AddColorLabel(f.Description, pnlFloorCards.Controls, f.BackColor, 190, 34, new Point(XX + 5, YY));
                AddPictureBoxAdd(f.Description, pnlFloorCards.Controls, 27, 24, new Point(XX + 200, YY + 5));
                AddPictureBoxSub(f.Description, pnlFloorCards.Controls, 27, 24, new Point(XX + 233, YY + 5));
                AddColorTextBox("1," + f.Description, pnlFloorCards.Controls, f.BackColor, 120, 34, new Point(XX + 265, YY), true);
                AddColorTextBox("2," + f.Description, pnlFloorCards.Controls, f.BackColor, 120, 34, new Point(XX + 390, YY), false);
                // increment y position
                YY += 40;
            }
            // show panel for floor cards again after dynamic creation of controls
            pnlFloorCards.Visible = true;
            // hide panel for paper sets, while conrols are being turned on
            pnlPaperSets.Visible = false;
            if (PaperSet.QtyPaperSets >= 1)
            {
                // if one paper set exists, show groupbox for it
                pbxPaper1Sub_Click(new object(), new EventArgs());
                pbxPaper1Add.Visible = true;
                pbxPaper1Sub.Visible = true;
                gbxPaper1.Visible = true;
                gbxPaper1.Text = PaperSet.CurrentPaperSets[0].Description;
                PaperSet.CurrentPaperSets[0].QtySerials = 1;
            }
            if (PaperSet.QtyPaperSets >= 2)
            {
                // if second paper set exists, show groupbox for it
                pbxPaper2Sub_Click(new object(), new EventArgs());
                pbxPaper2Add.Visible = true;
                pbxPaper2Sub.Visible = true;
                gbxPaper2.Visible = true;
                gbxPaper2.Text = PaperSet.CurrentPaperSets[1].Description;
                PaperSet.CurrentPaperSets[1].QtySerials = 1;
            }
            if (PaperSet.QtyPaperSets == 3)
            {
                // if third paper set exists, show groupbox for it
                pbxPaper3Sub_Click(new object(), new EventArgs());
                pbxPaper3Add.Visible = true;
                pbxPaper3Sub.Visible = true;
                gbxPaper3.Visible = true;
                gbxPaper3.Text = PaperSet.CurrentPaperSets[2].Description;
                PaperSet.CurrentPaperSets[2].QtySerials = 1;
            }
            // reshow panel for paper sets after groupbox(es) added
            pnlPaperSets.Visible = true;
        }
        private void ClearFloorStuff()
        {
            // clear all controls and floor card info to reset program info

            tabIndexCtr = 0;
            pnlFloorCards.Controls.Clear();
            lbls.Clear();
            ser1txts.Clear();
            ser2txts.Clear();
            addPbxs.Clear();
            subPbxs.Clear();
            ser1pnls.Clear();
            ser2pnls.Clear();
        }
        private void ClearPaperSetStuff()
        {
            // hide items in Paper Sets to reset program info

            // make pbxs invisible
            pbxPaper1Add.Visible = false;
            pbxPaper1Sub.Visible = false;
            pbxPaper2Add.Visible = false;
            pbxPaper2Sub.Visible = false;
            pbxPaper3Add.Visible = false;
            pbxPaper3Sub.Visible = false;
            // make gbxs invisible
            gbxPaper1.Visible = false;
            gbxPaper2.Visible = false;
            gbxPaper3.Visible = false;
        }
        public void DisableSecondFCSerial(object sender, EventArgs e, PictureBox pbx)
        {
            // event handler when minus sign in floor cards is clicked
            // disables textbox for second floor card serial to be entered
            int index = int.Parse(pbx.Name);
            pbx.BackgroundImage = Image.FromFile(@"C:\Users\Administrator\AppData\Roaming\Jeremy Stones\Images\null.png");
            addPbxs[index].BackgroundImage = Image.FromFile(@"C:\Users\Administrator\AppData\Roaming\Jeremy Stones\Images\plus.png");
            ser2pnls[index].Enabled = false;
            ser2txts[index].BackColor = FloorCard.CurrentFloorCards[int.Parse(pbx.Name)].BackColor;
            FloorCard.CurrentFloorCards[index].QtySerials--;
        }
        public void EnableSecondFCSerial(object sender, EventArgs e, PictureBox pbx)
        {
            // event handler when plus sign in floor cards is clicked
            // enables textbox for second floor card serial to be entered
            int index = int.Parse(pbx.Name);
            pbx.BackgroundImage = Image.FromFile(@"C:\Users\Administrator\AppData\Roaming\Jeremy Stones\Images\null.png");
            subPbxs[index].BackgroundImage = Image.FromFile(@"C:\Users\Administrator\AppData\Roaming\Jeremy Stones\Images\minus.png");
            ser2pnls[index].Enabled = true;
            ser2txts[index].BackColor = Color.White;
            FloorCard.CurrentFloorCards[index].QtySerials++;
        }
        private void frmStartupWizard_FormClosing(object sender, FormClosingEventArgs e)
        {
            // if the user has not clicked submit and the form is closing, make sure the entire application properly exits
            if (!isSubmitClose)
                Application.Exit();
        }
        public void MoveBoxes(object sender, PreviewKeyDownEventArgs e, TextBox t, Panel p)
        {
            // allows user to navigate the textboxes in floorcards more easily
            if (e.KeyCode == Keys.Down)
            {
                // if key is DOWN key, test to see if there's a panel below and move focus to that panel
                foreach (Control c in pnlFloorCards.Controls)
                {
                    if (c.Location.Y == p.Location.Y + 40 && c.Location.X == p.Location.X)
                        c.Focus();
                }
            }
            else if (e.KeyCode == Keys.Up)
            {
                // if key is UP key, test to see if there's a panel above and move focus to that panel
                foreach (Control c in pnlFloorCards.Controls)
                {
                    if (c.Location.Y == p.Location.Y - 40 && c.Location.X == p.Location.X)
                        c.Focus();
                }
            }
            else if (e.KeyCode == Keys.Right)
            {
                // if user presses RIGHT key, send TAB
                SendKeys.Send("{TAB}");
            }
            else if (e.KeyCode == Keys.Left)
            {
                // if user presses LEFT key, send SHIFT + TAB
                SendKeys.Send("+{TAB}");
            }
        }
        private void mnuFileExit_Click(object sender, EventArgs e)
        {
            // this menu option activates the exit button which exits the application
            btnExit.PerformClick();
        }
        private void mnuFileSubmit_Click(object sender, EventArgs e)
        {
            // redirects users action to submit button click - send info to next form
            btnSubmit.PerformClick();
        }
        private void mnuInfoAbout_Click(object sender, EventArgs e)
        {
            // creates and opens an about form
            frmAbout about = new frmAbout();
            about.ShowDialog();
        }
        private void mnuInfoHelp_Click(object sender, EventArgs e)
        {
            // creates and opens a help form
            frmHelp help = new frmHelp();
            help.ShowDialog();
        }
        private void pbxPaper1Add_Click(object sender, EventArgs e)
        {
            // allows user to add another serial, StartNum, and qty for PaperSet1
            pbxPaper1Add.BackgroundImage = Image.FromFile(@"C:\Users\Administrator\AppData\Roaming\Jeremy Stones\Images\null.png");
            pbxPaper1Sub.BackgroundImage = Image.FromFile(@"C:\Users\Administrator\AppData\Roaming\Jeremy Stones\Images\minus.png");
            txtPaper1Serial2.Enabled = true;
            nudPaper1Qty2.Enabled = true;
            nudPaper1Start2.Enabled = true;
            PaperSet.CurrentPaperSets[0].QtySerials++;
        }
        private void pbxPaper1Sub_Click(object sender, EventArgs e)
        {
            // allows user to subtract a serial, StartNum, and qty from PaperSet1
            pbxPaper1Add.BackgroundImage = Image.FromFile(@"C:\Users\Administrator\AppData\Roaming\Jeremy Stones\Images\plus.png");
            pbxPaper1Sub.BackgroundImage = Image.FromFile(@"C:\Users\Administrator\AppData\Roaming\Jeremy Stones\Images\null.png");
            txtPaper1Serial2.Enabled = false;
            nudPaper1Qty2.Enabled = false;
            nudPaper1Start2.Enabled = false;
            PaperSet.CurrentPaperSets[0].QtySerials--;
        }
        private void pbxPaper2Add_Click(object sender, EventArgs e)
        {
            // allows user to add another serial, StartNum, and qty for PaperSet2
            pbxPaper2Add.BackgroundImage = Image.FromFile(@"C:\Users\Administrator\AppData\Roaming\Jeremy Stones\Images\null.png");
            pbxPaper2Sub.BackgroundImage = Image.FromFile(@"C:\Users\Administrator\AppData\Roaming\Jeremy Stones\Images\minus.png");
            txtPaper2Serial2.Enabled = true;
            nudPaper2Qty2.Enabled = true;
            nudPaper2Start2.Enabled = true;
            PaperSet.CurrentPaperSets[1].QtySerials++;
        }
        private void pbxPaper2Sub_Click(object sender, EventArgs e)
        {
            // allows user to subtract a serial, StartNum, and qty from PaperSet2
            pbxPaper2Add.BackgroundImage = Image.FromFile(@"C:\Users\Administrator\AppData\Roaming\Jeremy Stones\Images\plus.png");
            pbxPaper2Sub.BackgroundImage = Image.FromFile(@"C:\Users\Administrator\AppData\Roaming\Jeremy Stones\Images\null.png");
            txtPaper2Serial2.Enabled = false;
            nudPaper2Qty2.Enabled = false;
            nudPaper2Start2.Enabled = false;
            PaperSet.CurrentPaperSets[1].QtySerials--;
        }
        private void pbxPaper3Add_Click(object sender, EventArgs e)
        {
            // allows user to add another serial, StartNum, and qty for PaperSet3
            pbxPaper3Add.BackgroundImage = Image.FromFile(@"C:\Users\Administrator\AppData\Roaming\Jeremy Stones\Images\null.png");
            pbxPaper3Sub.BackgroundImage = Image.FromFile(@"C:\Users\Administrator\AppData\Roaming\Jeremy Stones\Images\minus.png");
            txtPaper3Serial2.Enabled = true;
            nudPaper3Qty2.Enabled = true;
            nudPaper3Start2.Enabled = true;
            PaperSet.CurrentPaperSets[2].QtySerials++;
        }
        private void pbxPaper3Sub_Click(object sender, EventArgs e)
        {
            // allows user to subtract a serial, StartNum, and qty from PaperSet3
            pbxPaper3Add.BackgroundImage = Image.FromFile(@"C:\Users\Administrator\AppData\Roaming\Jeremy Stones\Images\plus.png");
            pbxPaper3Sub.BackgroundImage = Image.FromFile(@"C:\Users\Administrator\AppData\Roaming\Jeremy Stones\Images\null.png");
            txtPaper3Serial2.Enabled = false;
            nudPaper3Qty2.Enabled = false;
            nudPaper3Start2.Enabled = false;
            PaperSet.CurrentPaperSets[2].QtySerials--;
        }
        private void pbxAddPT_Click(object sender, EventArgs e)
        {
            // when user clicks plus, add one pt control set to form
            switch (qtyPullTabs)
            {
                case 1:
                    cbxPT2.Visible = true;
                    txtPTSerial2.Visible = true;
                    pbxRemovePT.Visible = true;
                    qtyPullTabs++;
                    break;
                case 2:
                    cbxPT3.Visible = true;
                    txtPTSerial3.Visible = true;
                    qtyPullTabs++;
                    break;
                case 3:
                    cbxPT4.Visible = true;
                    txtPTSerial4.Visible = true;
                    qtyPullTabs++;
                    break;
                case 4:
                    cbxPT5.Visible = true;
                    txtPTSerial5.Visible = true;
                    qtyPullTabs++;
                    break;
                case 5:
                    cbxPT6.Visible = true;
                    txtPTSerial6.Visible = true;
                    qtyPullTabs++;
                    pbxAddPT.Visible = false;
                    break;
                default:
                    break;
            }
        }
        private void pbxRemovePT_Click(object sender, EventArgs e)
        {
            // when user clicks minus, remove one pt control set from form
            switch (qtyPullTabs)
            {
                case 2:
                    cbxPT2.Visible = false;
                    cbxPT2.SelectedIndex = -1;
                    txtPTSerial2.Visible = false;
                    txtPTSerial2.Clear();
                    pbxRemovePT.Visible = false;
                    qtyPullTabs--;
                    break;
                case 3:
                    cbxPT3.Visible = false;
                    cbxPT3.SelectedIndex = -1;
                    txtPTSerial3.Visible = false;
                    txtPTSerial3.Clear();
                    qtyPullTabs--;
                    break;
                case 4:
                    cbxPT4.Visible = false;
                    cbxPT4.SelectedIndex = -1;
                    txtPTSerial4.Visible = false;
                    txtPTSerial4.Clear();
                    qtyPullTabs--;
                    break;
                case 5:
                    cbxPT5.Visible = false;
                    cbxPT5.SelectedIndex = -1;
                    txtPTSerial5.Visible = false;
                    txtPTSerial5.Clear();
                    qtyPullTabs--;
                    break;
                case 6:
                    cbxPT6.Visible = false;
                    cbxPT6.SelectedIndex = -1;
                    txtPTSerial6.Visible = false;
                    txtPTSerial6.Clear();
                    pbxAddPT.Visible = true;
                    qtyPullTabs--;
                    break;
                default:
                    break;
            }
        }
        private void pbxAddFloor_Click(object sender, EventArgs e)
        {
            // when user clicks plus, add one floor worker control set to form
            switch (qtyFloorWorkers)
            {
                case 1:
                    cbxFloor2.Visible = true;
                    pbxRemoveFloor.Visible = true;
                    qtyFloorWorkers++;
                    break;
                case 2:
                    cbxFloor3.Visible = true;
                    qtyFloorWorkers++;
                    break;
                case 3:
                    cbxFloor4.Visible = true;
                    pbxAddFloor.Visible = false;
                    qtyFloorWorkers++;
                    break;
                default:
                    break;
            }
        }
        private void pbxRemoveFloor_Click(object sender, EventArgs e)
        {
            // when user clicks minus, remove one floor worker control set from form
            switch (qtyFloorWorkers)
            {
                case 2:
                    cbxFloor2.Visible = false;
                    cbxFloor2.SelectedIndex = -1;
                    pbxRemoveFloor.Visible = false;
                    qtyFloorWorkers--;
                    break;
                case 3:
                    cbxFloor3.Visible = false;
                    cbxFloor3.SelectedIndex = -1;
                    qtyFloorWorkers--;
                    break;
                case 4:
                    cbxFloor4.Visible = false;
                    cbxFloor4.SelectedIndex = -1;
                    pbxAddFloor.Visible = true;
                    qtyFloorWorkers--;
                    break;
                default:
                    break;
            }
        }
        public void StoreTextInPanelTag(object sender, KeyPressEventArgs e, TextBox t, Panel p, string name)
        {
            // method used to store additional info in the panel
            p.Tag = name + "," + t.Text;
        }

        private bool SecurityPassed()
        {
            using (frmSecurity security = new frmSecurity())
            {
                DialogResult dlg = security.ShowDialog();
                if (dlg == DialogResult.OK)
                    return true;
                else
                    return false;
            }
        }
        private void mnuEditSessionTemplates_Click(object sender, EventArgs e)
        {
            if (SecurityPassed())
            {
                // close this form and open edit from with templates page showing
                this.Hide();
                frmEdit edit = new frmEdit(0);
                edit.Show();
                isSubmitClose = true;
                this.Close();
            }
        }

        private void mnuEditCharities_Click(object sender, EventArgs e)
        {
            if (SecurityPassed())
            {
                // close this form and open edit from with charities page showing
                this.Hide();
                frmEdit edit = new frmEdit(1);
                edit.Show();
                isSubmitClose = true;
                this.Close();
            }
        }

        private void mnuEditWorkers_Click(object sender, EventArgs e)
        {
            if (SecurityPassed())
            {
                // close this form and open edit from with workers page showing
                this.Hide();
                frmEdit edit = new frmEdit(2);
                edit.Show();
                isSubmitClose = true;
                this.Close();
            }
        }

        private void mnuEditPullTabs_Click(object sender, EventArgs e)
        {
            if (SecurityPassed())
            {
                // close this form and open edit from with templates page showing
                this.Hide();
                frmEdit edit = new frmEdit(3);
                edit.Show();
                isSubmitClose = true;
                this.Close();
            }
        }

        private void mnuEditDefaults_Click(object sender, EventArgs e)
        {
            if (SecurityPassed())
            {
                // close this form and open edit from with defaults page showing
                this.Hide();
                frmEdit edit = new frmEdit(4);
                edit.Show();
                isSubmitClose = true;
                this.Close();
            }
        }
    }
}
