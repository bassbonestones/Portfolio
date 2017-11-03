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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stones_Final_Project
{
    public partial class frmMain : Form
    {
        // this form is the main functionality of the application.  After the user has finished the 
        // the setup wizard, they will be directed here
        
        // data sets to store information for customized datagridviews

        // create datasets to hold current session information and use in datagridviews
        DataSet DsCurrentPaperSets = new DataSet();
        DataSet DsCurrentElectronics = new DataSet();
        DataSet DsCurrentSessionBingoGames = new DataSet();
        // create array of max 19 pulltabs
        PullTab[] tablePullTabs = new PullTab[19];
        // rectangle to get screen dimensions
        Rectangle screenRect;
        // counters
        int tabIndexCtr = 0;
        int numWorkers = 0;
        int numPullTabs = 0;
        // create lists to hold dynamic information
        List<Label> lstLblTitle = new List<Label>();
        List<TextBox> lstTxtSerial1 = new List<TextBox>();
        List<TextBox> lstTxtSerial2 = new List<TextBox>();
        List<PictureBox> lstPbxAdds = new List<PictureBox>();
        List<PictureBox> lstPbxSubs = new List<PictureBox>();
        List<TextBox> lstTxtW1Out = new List<TextBox>();
        List<TextBox> lstTxtW1In = new List<TextBox>();
        List<TextBox> lstTxtW1Owes = new List<TextBox>();
        List<TextBox> lstTxtW2Out = new List<TextBox>();
        List<TextBox> lstTxtW2In = new List<TextBox>();
        List<TextBox> lstTxtW2Owes = new List<TextBox>();
        List<TextBox> lstTxtW3Out = new List<TextBox>();
        List<TextBox> lstTxtW3In = new List<TextBox>();
        List<TextBox> lstTxtW3Owes = new List<TextBox>();
        List<TextBox> lstTxtW4Out = new List<TextBox>();
        List<TextBox> lstTxtW4In = new List<TextBox>();
        List<TextBox> lstTxtW4Owes = new List<TextBox>();
        List<TextBox> lstTxtW5Out = new List<TextBox>();
        List<TextBox> lstTxtW5In = new List<TextBox>();
        List<TextBox> lstTxtW5Owes = new List<TextBox>();
        List<TextBox> lstTxtSales = new List<TextBox>();
        List<TextBox> lstTxtCards = new List<TextBox>();
        List<Panel> lstPnlFloorCards = new List<Panel>();
        // create arrays for easy data manip
        ComboBox[] arrCbxWorkers = new ComboBox[5];
        TextBox[] arrTxtTotalsRow = new TextBox[7];
        // flag to prevent infinite loop
        bool refreshingWorkers = false;
        // session report variables
        double bingoTotalPrizes = 0;
        double bingoTotalTax = 0;
        double bingoShortTax = 0;
        double ptTotalPrizes = 0;
        double ptTotalGross = 0;
        double ptTotalTax = 0;
        double ptShortTax = 0;
        double TotalSales = 0;
        double TotalPrizes = 0;
        double TotalTax = 0;
        double TotalShortTax = 0;
        double AmountRemoved = 0;
        double PTDeskOwes = 0;
        double DeskOwes = 0;
        double NetDeposit = 0;
        double electronicsTotal = 0;
        double allPaperSalesTotal = 0;
        // constructor
        public frmMain()
        {
            // on creation of frmMain
            Worker.CurrentPTDeskWorker = new Worker("PT Desk");
            numWorkers = Worker.CurrentWorkersList.Count;
            screenRect = Screen.FromControl(this).Bounds;
            this.Size = new Size(screenRect.Width, screenRect.Height);
            
            InitializeComponent();

            pnlPullTabs.Height -= (36 * 13);
            pnlPTTotalsRow.Location = new Point(pnlPTTotalsRow.Location.X, pnlPTTotalsRow.Location.Y - (36 * 13));
            foreach (Charity c in DbOps.Charities)
            {
                cbxCharities.Items.Add(c.Name);
            }
            try
            {
                cbxCharities.SelectedItem = Charity.CurrentCharity.Name;
            }
            catch (Exception) { }
            FormatPaperSetDGV();
            FormatElectronicsDGV();
            FormatSessionBingoGamesDGV();
            //dgvPullTabNumbers.RowHeadersWidth = 280
            dgvElectronics.Location = new Point(dgvElectronics.Location.X, dgvPaperSets.Location.Y + dgvPaperSets.Height + 20);
            pnlControlContainer.Height = dgvSessionBingoGames.Height + 50;
            pnlControlContainer.Location = new Point(this.Width / 2 - pnlControlContainer.Width / 2 - 50, 75);
            pnlFloorCards.Location = new Point(this.Width / 2 - pnlFloorCards.Width / 2 - 25, 75);
            pnlPullTabs.Location = new Point(this.Width / 2 - pnlPullTabs.Width / 2 - 50, pnlPullTabs.Location.Y);
            pnlReport.Location = new Point(this.Width / 2 - pnlReport.Width / 2 - 25, 75);
           
            //AddColorFloorCardPanel("test", pnlFloorCards.Controls, Color.SandyBrown, new Point(50, 50));
        }
        // wndProc redefinitions
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

        // non-event-handler methods
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
            pbx.Name = lstPbxAdds.Count.ToString();
            lstPbxAdds.Add(pbx);
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
            pbx.Name = lstPbxSubs.Count.ToString();
            lstPbxSubs.Add(pbx);
            // lambda function to pass extra info to event handler
            pbx.Click += new EventHandler((s, e) => DisableSecondFCSerial(s, e, pbx));
        }
        private void AddPT()
        {
            //method which adds a row of controls to the pull tabs tabpage 
            switch (numPullTabs)
            {
                case 0:
                    pbxRemovePT.Visible = true;
                    txtPT01Serial.Visible = true;
                    cbxPT01Name.Visible = true;
                    txtPT01Floor1.Visible = true;
                    txtPT01Floor2.Visible = true;
                    txtPT01Floor3.Visible = true;
                    txtPT01Floor4.Visible = true;
                    txtPT01Floor5.Visible = true;
                    lblPT01Total.Visible = true;
                    pnlPullTabs.Height += 36;
                    pnlPTTotalsRow.Location = new Point(pnlPTTotalsRow.Location.X, pnlPTTotalsRow.Location.Y + 36);
                    break;
                case 1:
                    txtPT02Serial.Visible = true;
                    cbxPT02Name.Visible = true;
                    txtPT02Floor1.Visible = true;
                    txtPT02Floor2.Visible = true;
                    txtPT02Floor3.Visible = true;
                    txtPT02Floor4.Visible = true;
                    txtPT02Floor5.Visible = true;
                    lblPT02Total.Visible = true;
                    pnlPullTabs.Height += 36;
                    pnlPTTotalsRow.Location = new Point(pnlPTTotalsRow.Location.X, pnlPTTotalsRow.Location.Y + 36);
                    break;
                case 2:
                    txtPT03Serial.Visible = true;
                    cbxPT03Name.Visible = true;
                    txtPT03Floor1.Visible = true;
                    txtPT03Floor2.Visible = true;
                    txtPT03Floor3.Visible = true;
                    txtPT03Floor4.Visible = true;
                    txtPT03Floor5.Visible = true;
                    lblPT03Total.Visible = true;
                    pnlPullTabs.Height += 36;
                    pnlPTTotalsRow.Location = new Point(pnlPTTotalsRow.Location.X, pnlPTTotalsRow.Location.Y + 36);
                    break;
                case 3:
                    txtPT04Serial.Visible = true;
                    cbxPT04Name.Visible = true;
                    txtPT04Floor1.Visible = true;
                    txtPT04Floor2.Visible = true;
                    txtPT04Floor3.Visible = true;
                    txtPT04Floor4.Visible = true;
                    txtPT04Floor5.Visible = true;
                    lblPT04Total.Visible = true;
                    pnlPullTabs.Height += 36;
                    pnlPTTotalsRow.Location = new Point(pnlPTTotalsRow.Location.X, pnlPTTotalsRow.Location.Y + 36);
                    break;
                case 4:
                    txtPT05Serial.Visible = true;
                    cbxPT05Name.Visible = true;
                    txtPT05Floor1.Visible = true;
                    txtPT05Floor2.Visible = true;
                    txtPT05Floor3.Visible = true;
                    txtPT05Floor4.Visible = true;
                    txtPT05Floor5.Visible = true;
                    lblPT05Total.Visible = true;
                    pnlPullTabs.Height += 36;
                    pnlPTTotalsRow.Location = new Point(pnlPTTotalsRow.Location.X, pnlPTTotalsRow.Location.Y + 36);
                    break;
                case 5:
                    txtPT06Serial.Visible = true;
                    cbxPT06Name.Visible = true;
                    txtPT06Floor1.Visible = true;
                    txtPT06Floor2.Visible = true;
                    txtPT06Floor3.Visible = true;
                    txtPT06Floor4.Visible = true;
                    txtPT06Floor5.Visible = true;
                    lblPT06Total.Visible = true;
                    pnlPullTabs.Height += 36;
                    pnlPTTotalsRow.Location = new Point(pnlPTTotalsRow.Location.X, pnlPTTotalsRow.Location.Y + 36);
                    break;
                case 6:
                    txtPT07Serial.Visible = true;
                    cbxPT07Name.Visible = true;
                    txtPT07Floor1.Visible = true;
                    txtPT07Floor2.Visible = true;
                    txtPT07Floor3.Visible = true;
                    txtPT07Floor4.Visible = true;
                    txtPT07Floor5.Visible = true;
                    lblPT07Total.Visible = true;
                    pnlPullTabs.Height += 36;
                    pnlPTTotalsRow.Location = new Point(pnlPTTotalsRow.Location.X, pnlPTTotalsRow.Location.Y + 36);
                    break;
                case 7:
                    txtPT08Serial.Visible = true;
                    cbxPT08Name.Visible = true;
                    txtPT08Floor1.Visible = true;
                    txtPT08Floor2.Visible = true;
                    txtPT08Floor3.Visible = true;
                    txtPT08Floor4.Visible = true;
                    txtPT08Floor5.Visible = true;
                    lblPT08Total.Visible = true;
                    pnlPullTabs.Height += 36;
                    pnlPTTotalsRow.Location = new Point(pnlPTTotalsRow.Location.X, pnlPTTotalsRow.Location.Y + 36);
                    break;
                case 8:
                    txtPT09Serial.Visible = true;
                    cbxPT09Name.Visible = true;
                    txtPT09Floor1.Visible = true;
                    txtPT09Floor2.Visible = true;
                    txtPT09Floor3.Visible = true;
                    txtPT09Floor4.Visible = true;
                    txtPT09Floor5.Visible = true;
                    lblPT09Total.Visible = true;
                    pnlPullTabs.Height += 36;
                    pnlPTTotalsRow.Location = new Point(pnlPTTotalsRow.Location.X, pnlPTTotalsRow.Location.Y + 36);
                    break;
                case 9:
                    txtPT10Serial.Visible = true;
                    cbxPT10Name.Visible = true;
                    txtPT10Floor1.Visible = true;
                    txtPT10Floor2.Visible = true;
                    txtPT10Floor3.Visible = true;
                    txtPT10Floor4.Visible = true;
                    txtPT10Floor5.Visible = true;
                    lblPT10Total.Visible = true;
                    pnlPullTabs.Height += 36;
                    pnlPTTotalsRow.Location = new Point(pnlPTTotalsRow.Location.X, pnlPTTotalsRow.Location.Y + 36);
                    break;
                case 10:
                    txtPT11Serial.Visible = true;
                    cbxPT11Name.Visible = true;
                    txtPT11Floor1.Visible = true;
                    txtPT11Floor2.Visible = true;
                    txtPT11Floor3.Visible = true;
                    txtPT11Floor4.Visible = true;
                    txtPT11Floor5.Visible = true;
                    lblPT11Total.Visible = true;
                    pnlPullTabs.Height += 36;
                    pnlPTTotalsRow.Location = new Point(pnlPTTotalsRow.Location.X, pnlPTTotalsRow.Location.Y + 36);
                    break;
                case 11:
                    txtPT12Serial.Visible = true;
                    cbxPT12Name.Visible = true;
                    txtPT12Floor1.Visible = true;
                    txtPT12Floor2.Visible = true;
                    txtPT12Floor3.Visible = true;
                    txtPT12Floor4.Visible = true;
                    txtPT12Floor5.Visible = true;
                    lblPT12Total.Visible = true;
                    pnlPullTabs.Height += 36;
                    pnlPTTotalsRow.Location = new Point(pnlPTTotalsRow.Location.X, pnlPTTotalsRow.Location.Y + 36);
                    break;
                case 12:
                    txtPT13Serial.Visible = true;
                    cbxPT13Name.Visible = true;
                    txtPT13Floor1.Visible = true;
                    txtPT13Floor2.Visible = true;
                    txtPT13Floor3.Visible = true;
                    txtPT13Floor4.Visible = true;
                    txtPT13Floor5.Visible = true;
                    lblPT13Total.Visible = true;
                    pnlPullTabs.Height += 36;
                    pnlPTTotalsRow.Location = new Point(pnlPTTotalsRow.Location.X, pnlPTTotalsRow.Location.Y + 36);
                    break;
                case 13:
                    txtPT14Serial.Visible = true;
                    cbxPT14Name.Visible = true;
                    txtPT14Floor1.Visible = true;
                    txtPT14Floor2.Visible = true;
                    txtPT14Floor3.Visible = true;
                    txtPT14Floor4.Visible = true;
                    txtPT14Floor5.Visible = true;
                    lblPT14Total.Visible = true;
                    pnlPullTabs.Height += 36;
                    pnlPTTotalsRow.Location = new Point(pnlPTTotalsRow.Location.X, pnlPTTotalsRow.Location.Y + 36);
                    break;
                case 14:
                    txtPT15Serial.Visible = true;
                    cbxPT15Name.Visible = true;
                    txtPT15Floor1.Visible = true;
                    txtPT15Floor2.Visible = true;
                    txtPT15Floor3.Visible = true;
                    txtPT15Floor4.Visible = true;
                    txtPT15Floor5.Visible = true;
                    lblPT15Total.Visible = true;
                    pnlPullTabs.Height += 36;
                    pnlPTTotalsRow.Location = new Point(pnlPTTotalsRow.Location.X, pnlPTTotalsRow.Location.Y + 36);
                    break;
                case 15:
                    txtPT16Serial.Visible = true;
                    cbxPT16Name.Visible = true;
                    txtPT16Floor1.Visible = true;
                    txtPT16Floor2.Visible = true;
                    txtPT16Floor3.Visible = true;
                    txtPT16Floor4.Visible = true;
                    txtPT16Floor5.Visible = true;
                    lblPT16Total.Visible = true;
                    pnlPullTabs.Width += 17;
                    pnlPTTotalsRow.Location = new Point(pnlPTTotalsRow.Location.X, pnlPTTotalsRow.Location.Y + 36);
                    break;
                case 16:
                    txtPT17Serial.Visible = true;
                    cbxPT17Name.Visible = true;
                    txtPT17Floor1.Visible = true;
                    txtPT17Floor2.Visible = true;
                    txtPT17Floor3.Visible = true;
                    txtPT17Floor4.Visible = true;
                    txtPT17Floor5.Visible = true;
                    lblPT17Total.Visible = true;
                    pnlPTTotalsRow.Location = new Point(pnlPTTotalsRow.Location.X, pnlPTTotalsRow.Location.Y + 36);
                    break;
                case 17:
                    txtPT18Serial.Visible = true;
                    cbxPT18Name.Visible = true;
                    txtPT18Floor1.Visible = true;
                    txtPT18Floor2.Visible = true;
                    txtPT18Floor3.Visible = true;
                    txtPT18Floor4.Visible = true;
                    txtPT18Floor5.Visible = true;
                    lblPT18Total.Visible = true;
                    pnlPTTotalsRow.Location = new Point(pnlPTTotalsRow.Location.X, pnlPTTotalsRow.Location.Y + 36);
                    break;
                case 18:
                    txtPT19Serial.Visible = true;
                    cbxPT19Name.Visible = true;
                    txtPT19Floor1.Visible = true;
                    txtPT19Floor2.Visible = true;
                    txtPT19Floor3.Visible = true;
                    txtPT19Floor4.Visible = true;
                    txtPT19Floor5.Visible = true;
                    lblPT19Total.Visible = true;
                    pnlPTTotalsRow.Location = new Point(pnlPTTotalsRow.Location.X, pnlPTTotalsRow.Location.Y + 36);
                    pbxAddPT.Visible = false;
                    break;
                default:
                    break;

            }
            numPullTabs++;
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
            lstLblTitle.Add(lbl);
            // test if row is totals row
            bool totalsRow = (name == "TOTALS");
            // add context menu for serial numbers
            int index = lstLblTitle.Count - 1;
            // create a context menu for it
            ContextMenuStrip cm = new ContextMenuStrip();
            if (!totalsRow)
            {
                // allows user to edit serial numbers, new button and textbox save new info
                ToolStripMenuItem mi1 = new ToolStripMenuItem("Serial 1: " + FloorCard.CurrentFloorCards[index].Serial1);
                ToolStripMenuItem mi2 = new ToolStripMenuItem("Serial 2: " + FloorCard.CurrentFloorCards[index].Serial2);
                cm.Items.Add(mi1);
                cm.Items.Add(mi2);
                ToolStripTextBox mi1edit = new ToolStripTextBox();
                mi1edit.BorderStyle = BorderStyle.Fixed3D;
                mi1edit.MaxLength = 8;
                mi1edit.BackColor = SystemColors.Control;
                ToolStripMenuItem mi1AddNewPrompt = new ToolStripMenuItem("Enter New Serial");
                ToolStripButton mi1btn = new ToolStripButton("Click To Update");
                mi1btn.BackColor = Color.Lavender;
                mi1.DropDownItems.Add(mi1AddNewPrompt);
                mi1.DropDownItems.Add(mi1edit);
                mi1.DropDownItems.Add(mi1btn);
                mi1btn.Click += new EventHandler((s, e) => UpdateSerial(s, e, 1, mi1edit.Text, index, mi1, mi1edit));
                mi1edit.KeyPress += new KeyPressEventHandler((s, e) => AutoClick(s, e, mi1btn));
                ToolStripTextBox mi2edit = new ToolStripTextBox();
                mi2edit.BorderStyle = BorderStyle.Fixed3D;
                mi2edit.BackColor = SystemColors.Control;
                mi2edit.MaxLength = 8;
                ToolStripMenuItem mi2AddNewPrompt = new ToolStripMenuItem("Enter New Serial");
                ToolStripButton mi2btn = new ToolStripButton("Click To Update");
                mi2btn.BackColor = Color.Lavender;
                mi2.DropDownItems.Add(mi2AddNewPrompt);
                mi2.DropDownItems.Add(mi2edit);
                mi2.DropDownItems.Add(mi2btn);
                mi2btn.Click += new EventHandler((s, e) => UpdateSerial(s, e, 2, mi2edit.Text, index, mi2, mi2edit));
                mi2edit.KeyPress += new KeyPressEventHandler((s, e) => AutoClick(s, e, mi2btn));
                lbl.ContextMenuStrip = cm;
                lbl.MouseEnter += new EventHandler((s, e) => lbl.ContextMenuStrip.Show(lbl, new Point(lbl.Width, 0)));
            }
        }
        public void AddColorTextBox(string name, Control.ControlCollection cc, Color col, int w, int h, Point p, bool isFirstTxt)
        {
            // method to add textbox controls at runtime to floor cards section

            // get index from previous picturebox - used to get background color of panel
            int index = int.Parse((lstPbxSubs.Count - 1).ToString());
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
            txt.MaxLength = 10;
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
                lstTxtSerial1.Add(txt);
                //ser1pnls.Add(pnl);
            }
            else
            {
                // if a serial2 textbox, add to appropriate list
                lstTxtSerial2.Add(txt);
                //ser2pnls.Add(pnl);
                pnl.Enabled = false;
                txt.BackColor = FloorCard.CurrentFloorCards[index].BackColor; // edit for this form
            }

        }
        public void AddColorFloorCardPanel(string name, Control.ControlCollection cc, Color col, Point p)
        {
            // method to add textbox controls at runtime to floor cards section

            // get index from previous
            int index = int.Parse((lstLblTitle.Count - 1).ToString());
            // see if totals row
            bool totalsRow = (name == "totals");
            // create panel behind textbox
            Panel pnl = new Panel();
            pnl.Width = 1069;
            pnl.Height = 34;
            pnl.BackColor = col;
            pnl.Tag = name;

            // create textbox
            TextBox txtW1Out = new TextBox();
            txtW1Out.Width = 50;
            txtW1Out.Height = 26;
            txtW1Out.MaxLength = 4;
            txtW1Out.TabStop = false;
            txtW1Out.ReadOnly = true;
            txtW1Out.Tag = name;
            int intW1Out = 0;
            if (!totalsRow) 
                intW1Out = FloorCard.CurrentFloorCards[index].Quantity1;
            txtW1Out.Text = intW1Out.ToString();
            txtW1Out.TextAlign = HorizontalAlignment.Center;
            if(!totalsRow)
                lstTxtW1Out.Add(txtW1Out);
            // add event handlers with lambda
            txtW1Out.DoubleClick += new EventHandler((s, e) => {
                txtW1Out.ReadOnly = false;
                txtW1Out.Focus();
                txtW1Out.SelectAll();
            });
            txtW1Out.Leave += new EventHandler((s, e) => {
                txtW1Out.ReadOnly = true;
            });
            txtW1Out.TextChanged += new EventHandler((s, e) => UpdateFloorCardTotals(s,e,index));

            // create textbox
            TextBox txtW1In = new TextBox();
            txtW1In.Width = 50;
            txtW1In.Height = 26;
            txtW1In.MaxLength = 4;
            if (!totalsRow)
                txtW1In.TabIndex = tabIndexCtr++;
            txtW1In.Tag = name;
            txtW1In.TextAlign = HorizontalAlignment.Center;
            if (!totalsRow)
                lstTxtW1In.Add(txtW1In);
            // add event handlers with lambda
            txtW1In.PreviewKeyDown += new PreviewKeyDownEventHandler((s, e) => MoveBoxesFloor(s,e)); 
            txtW1In.Enter += new EventHandler((s, e) => txtW1In.SelectAll());
            txtW1In.Click += new EventHandler((s, e) => txtW1In.SelectAll());
            txtW1In.TextChanged += new EventHandler((s, e) => UpdateFloorCardTotals(s, e, index));


            // create textbox
            TextBox txtW1Owes = new TextBox();
            txtW1Owes.Width = 75;
            txtW1Owes.Height = 26;
            txtW1Owes.MaxLength = 8;
            txtW1Owes.Tag = name;
            txtW1Owes.TabStop = false;
            txtW1Owes.ReadOnly = true;
            txtW1Owes.BackColor = SystemColors.GradientInactiveCaption;
            double dblW1Owes = 0.0;
            if (!totalsRow)
                dblW1Owes = (double)FloorCard.CurrentFloorCards[index].Quantity1 * FloorCard.CurrentFloorCards[index].Price;
            txtW1Owes.Text = (dblW1Owes).ToString("n2");
            txtW1Owes.TextAlign = HorizontalAlignment.Right;
            if (!totalsRow)
                lstTxtW1Owes.Add(txtW1Owes);

            // create textbox
            TextBox txtW2Out = new TextBox();
            txtW2Out.Width = 50;
            txtW2Out.Height = 26;
            txtW2Out.MaxLength = 4;
            txtW2Out.TabStop = false;
            txtW2Out.ReadOnly = true;
            txtW2Out.Tag = name;
            int intW2Out = 0;
            if (!totalsRow)
                intW2Out = FloorCard.CurrentFloorCards[index].Quantity2;
            txtW2Out.Text = intW2Out.ToString();
            txtW2Out.TextAlign = HorizontalAlignment.Center;
            if (!totalsRow)
                lstTxtW2Out.Add(txtW2Out);
            // add event handlers with lambda
            txtW2Out.DoubleClick += new EventHandler((s, e) => {
                txtW2Out.ReadOnly = false;
                txtW2Out.Focus();
                txtW2Out.SelectAll();
            });
            txtW2Out.Leave += new EventHandler((s, e) => {
                txtW2Out.ReadOnly = true;
            });
            txtW2Out.TextChanged += new EventHandler((s, e) => UpdateFloorCardTotals(s, e, index));

            // create textbox
            TextBox txtW2In = new TextBox();
            txtW2In.Width = 50;
            txtW2In.Height = 26;
            txtW2In.MaxLength = 4;
            if (!totalsRow)
                txtW2In.TabIndex = tabIndexCtr++;
            txtW2In.Tag = name;
            txtW2In.TextAlign = HorizontalAlignment.Center;
            if (!totalsRow)
                lstTxtW2In.Add(txtW2In);
            // add event handlers with lambda
            txtW2In.PreviewKeyDown += new PreviewKeyDownEventHandler((s, e) => MoveBoxesFloor(s,e));
            txtW2In.Enter += new EventHandler((s, e) => txtW2In.SelectAll());
            txtW2In.Click += new EventHandler((s, e) => txtW2In.SelectAll());
            txtW2In.TextChanged += new EventHandler((s, e) => UpdateFloorCardTotals(s, e, index));

            // create textbox
            TextBox txtW2Owes = new TextBox();
            txtW2Owes.Width = 75;
            txtW2Owes.Height = 26;
            txtW2Owes.MaxLength = 8;
            txtW2Owes.ReadOnly = true;
            txtW2Owes.Tag = name;
            txtW2Owes.TabStop = false;
            txtW2Owes.BackColor = SystemColors.GradientInactiveCaption;
            double dblW2Owes = 0;
            if (!totalsRow)
                dblW2Owes = (double)FloorCard.CurrentFloorCards[index].Quantity2 * FloorCard.CurrentFloorCards[index].Price;
            txtW2Owes.Text = (dblW2Owes).ToString("n2");
            txtW2Owes.TextAlign = HorizontalAlignment.Right;
            if (!totalsRow)
                lstTxtW2Owes.Add(txtW2Owes);

            // create textbox
            TextBox txtW3Out = new TextBox();
            txtW3Out.Width = 50;
            txtW3Out.Height = 26;
            txtW3Out.MaxLength = 4;
            txtW3Out.TabStop = false;
            txtW3Out.ReadOnly = true;
            txtW3Out.Tag = name;
            int intW3Out = 0;
            if (!totalsRow)
                intW3Out = FloorCard.CurrentFloorCards[index].Quantity3;
            txtW3Out.Text = intW3Out.ToString();
            txtW3Out.TextAlign = HorizontalAlignment.Center;
            if (!totalsRow)
                lstTxtW3Out.Add(txtW3Out);
            // add event handlers with lambda
            txtW3Out.DoubleClick += new EventHandler((s, e) => {
                txtW3Out.ReadOnly = false;
                txtW3Out.Focus();
                txtW3Out.SelectAll();
            });
            txtW3Out.Leave += new EventHandler((s, e) => {
                txtW3Out.ReadOnly = true;
            });
            txtW3Out.TextChanged += new EventHandler((s, e) => UpdateFloorCardTotals(s, e, index));

            // create textbox
            TextBox txtW3In = new TextBox();
            txtW3In.Width = 50;
            txtW3In.Height = 26;
            txtW3In.MaxLength = 4;
            if (!totalsRow)
                txtW3In.TabIndex = tabIndexCtr++;
            txtW3In.Tag = name;
            txtW3In.TextAlign = HorizontalAlignment.Center;
            if (!totalsRow)
                lstTxtW3In.Add(txtW3In);
            // add event handlers with lambda
            txtW3In.PreviewKeyDown += new PreviewKeyDownEventHandler((s, e) => MoveBoxesFloor(s,e));
            txtW3In.Enter += new EventHandler((s, e) => txtW3In.SelectAll());
            txtW3In.Click += new EventHandler((s, e) => txtW3In.SelectAll());
            txtW3In.TextChanged += new EventHandler((s, e) => UpdateFloorCardTotals(s, e, index));

            // create textbox
            TextBox txtW3Owes = new TextBox();
            txtW3Owes.Width = 75;
            txtW3Owes.Height = 26;
            txtW3Owes.MaxLength = 8;
            txtW3Owes.ReadOnly = true;
            txtW3Owes.Tag = name;
            txtW3Owes.TabStop = false;
            txtW3Owes.BackColor = SystemColors.GradientInactiveCaption;
            double dblW3Owes = 0;
            if (!totalsRow)
                dblW3Owes = (double)FloorCard.CurrentFloorCards[index].Quantity3 * FloorCard.CurrentFloorCards[index].Price;
            txtW3Owes.Text = (dblW3Owes).ToString("n2");
            txtW3Owes.TextAlign = HorizontalAlignment.Right;
            if (!totalsRow)
                lstTxtW3Owes.Add(txtW3Owes);

            // create textbox
            TextBox txtW4Out = new TextBox();
            txtW4Out.Width = 50;
            txtW4Out.Height = 26;
            txtW4Out.MaxLength = 4;
            txtW4Out.TabStop = false;
            txtW4Out.ReadOnly = true;
            txtW4Out.Tag = name;
            int intW4Out = 0;
            if (!totalsRow)
                intW4Out = FloorCard.CurrentFloorCards[index].Quantity4;
            txtW4Out.Text = intW4Out.ToString();
            txtW4Out.TextAlign = HorizontalAlignment.Center;
            if (!totalsRow)
                lstTxtW4Out.Add(txtW4Out);
            // add event handlers with lambda
            txtW4Out.DoubleClick += new EventHandler((s, e) => {
                txtW4Out.ReadOnly = false;
                txtW4Out.Focus();
                txtW4Out.SelectAll();
            });
            txtW4Out.Leave += new EventHandler((s, e) => {
                txtW4Out.ReadOnly = true;
            });
            txtW4Out.TextChanged += new EventHandler((s, e) => UpdateFloorCardTotals(s, e, index));

            // create textbox
            TextBox txtW4In = new TextBox();
            txtW4In.Width = 50;
            txtW4In.Height = 26;
            txtW4In.MaxLength = 4;
            if (!totalsRow)
                txtW4In.TabIndex = tabIndexCtr++;
            txtW4In.Tag = name;
            txtW4In.TextAlign = HorizontalAlignment.Center;
            if (!totalsRow)
                lstTxtW4In.Add(txtW4In);
            // add event handlers with lambda
            txtW4In.PreviewKeyDown += new PreviewKeyDownEventHandler((s, e) => MoveBoxesFloor(s,e));
            txtW4In.Enter += new EventHandler((s, e) => txtW4In.SelectAll());
            txtW4In.Click += new EventHandler((s, e) => txtW4In.SelectAll());
            txtW4In.TextChanged += new EventHandler((s, e) => UpdateFloorCardTotals(s, e, index));

            // create textbox
            TextBox txtW4Owes = new TextBox();
            txtW4Owes.Width = 75;
            txtW4Owes.Height = 26;
            txtW4Owes.MaxLength = 8;
            txtW4Owes.ReadOnly = true;
            txtW4Owes.Tag = name;
            txtW4Owes.TabStop = false;
            txtW4Owes.BackColor = SystemColors.GradientInactiveCaption;
            double dblW4Owes = 0;
            if (!totalsRow)
                dblW4Owes = (double)FloorCard.CurrentFloorCards[index].Quantity4 * FloorCard.CurrentFloorCards[index].Price;
            txtW4Owes.Text = (dblW4Owes).ToString("n2");
            txtW4Owes.TextAlign = HorizontalAlignment.Right;
            if (!totalsRow)
                lstTxtW4Owes.Add(txtW4Owes);
            
            // create textbox
            TextBox txtW5Out = new TextBox();
            txtW5Out.Width = 50;
            txtW5Out.Height = 26;
            txtW5Out.MaxLength = 4;
            txtW5Out.TabStop = false;
            txtW5Out.ReadOnly = true;
            txtW5Out.Tag = name;
            int intW5Out = 0;
            if (!totalsRow)
                intW5Out = FloorCard.CurrentFloorCards[index].Quantity5;
            txtW5Out.Text = intW5Out.ToString();
            txtW5Out.TextAlign = HorizontalAlignment.Center;
            if (!totalsRow)
                lstTxtW5Out.Add(txtW5Out);
            // add event handlers with lambda
            txtW5Out.DoubleClick += new EventHandler((s, e) => {
                txtW5Out.ReadOnly = false;
                txtW5Out.Focus();
                txtW5Out.SelectAll();
            });
            txtW5Out.Leave += new EventHandler((s, e) => {
                txtW5Out.ReadOnly = true;
            });
            txtW5Out.TextChanged += new EventHandler((s, e) => UpdateFloorCardTotals(s, e, index));

            // create textbox
            TextBox txtW5In = new TextBox();
            txtW5In.Width = 50;
            txtW5In.Height = 26;
            txtW5In.MaxLength = 4;
            if (!totalsRow)
                txtW5In.TabIndex = tabIndexCtr++;
            txtW5In.Tag = name;
            txtW5In.TextAlign = HorizontalAlignment.Center;
            if (!totalsRow)
                lstTxtW5In.Add(txtW5In);
            // add event handlers with lambda
            txtW5In.PreviewKeyDown += new PreviewKeyDownEventHandler((s, e) => MoveBoxesFloor(s,e));
            txtW5In.Enter += new EventHandler((s, e) => txtW5In.SelectAll());
            txtW5In.Click += new EventHandler((s, e) => txtW5In.SelectAll());
            txtW5In.TextChanged += new EventHandler((s, e) => UpdateFloorCardTotals(s, e, index));

            // create textbox
            TextBox txtW5Owes = new TextBox();
            txtW5Owes.Width = 75;
            txtW5Owes.Height = 26;
            txtW5Owes.MaxLength = 8;
            txtW5Owes.ReadOnly = true;
            txtW5Owes.Tag = name;
            txtW5Owes.TabStop = false;
            txtW5Owes.BackColor = SystemColors.GradientInactiveCaption;
            double dblW5Owes = 0;
            if (!totalsRow)
                dblW5Owes = (double)FloorCard.CurrentFloorCards[index].Quantity5 * FloorCard.CurrentFloorCards[index].Price;
            txtW5Owes.Text = (dblW5Owes).ToString("n2");
            txtW5Owes.TextAlign = HorizontalAlignment.Right;
            if (!totalsRow)
                lstTxtW5Owes.Add(txtW5Owes);

            

            // create textbox
            TextBox txtCards = new TextBox();
            txtCards.Width = 75;
            txtCards.Height = 26;
            txtCards.MaxLength = 4;
            txtCards.Tag = name;
            txtCards.ReadOnly = true;
            txtCards.TabStop = false;
            int totalCards = intW1Out + intW2Out + intW3Out + intW4Out + intW5Out;
            txtCards.Text = totalCards.ToString();
            txtCards.TextAlign = HorizontalAlignment.Center;
            if (!totalsRow)
                lstTxtCards.Add(txtCards);

            // create textbox
            TextBox txtSales = new TextBox();
            txtSales.Width = 75;
            txtSales.Height = 26;
            txtSales.MaxLength = 8;
            txtSales.Tag = name;
            txtSales.ReadOnly = true;
            txtSales.TabStop = false;
            txtSales.BackColor = Color.LightYellow;
            double totalSales = dblW1Owes + dblW2Owes + dblW3Owes + dblW4Owes + dblW5Owes;
            txtSales.Text = totalSales.ToString("n2");
            txtSales.TextAlign = HorizontalAlignment.Right;
            if (!totalsRow)
                lstTxtSales.Add(txtSales);
            if (totalsRow)
                txtSales.TextChanged += new EventHandler((s, e) => {
                    txtFloor.Text = double.Parse(txtSales.Text).ToString("c");
                    txtFloor.Tag = txtSales.Text;
                    txtAllPaperSales.Text = (double.Parse(txtSales.Text) + allPaperSalesTotal).ToString("c");
                    txtAllPaperSales.Tag = (double.Parse(txtSales.Text) + allPaperSalesTotal).ToString().ToString();
                });


            TextBox[] range = { txtW1Owes, txtW2Owes, txtW3Owes, txtW4Owes, txtW5Owes, txtCards, txtSales};
            TextBox[] range2 = { txtW1Out, txtW1In, txtW2Out, txtW2In, txtW3Out, txtW3In, txtW4Out, txtW4In,
                txtW5Out, txtW5In };
            // add textbox to panel
            pnl.Controls.AddRange(range);
            if(!totalsRow)
                pnl.Controls.AddRange(range2);
            int XX = 4;
            txtW1Out.Location = new Point(XX, 4);  XX += 50;
            txtW1In.Location = new Point(XX, 4);   XX += 50;
            txtW1Owes.Location = new Point(XX, 4); XX += 80;
            txtW2Out.Location = new Point(XX, 4);  XX += 50;
            txtW2In.Location = new Point(XX, 4);   XX += 50;
            txtW2Owes.Location = new Point(XX, 4); XX += 80;
            txtW3Out.Location = new Point(XX, 4);  XX += 50;
            txtW3In.Location = new Point(XX, 4);   XX += 50;
            txtW3Owes.Location = new Point(XX, 4); XX += 80;
            txtW4Out.Location = new Point(XX, 4);  XX += 50;
            txtW4In.Location = new Point(XX, 4);   XX += 50;
            txtW4Owes.Location = new Point(XX, 4); XX += 80;
            txtW5Out.Location = new Point(XX, 4);  XX += 50;
            txtW5In.Location = new Point(XX, 4);   XX += 50;
            txtW5Owes.Location = new Point(XX, 4); XX += 90;
            txtCards.Location = new Point(XX, 4);  XX += 75;
            txtSales.Location = new Point(XX, 4);
            
            // make some extra changes if totals row
            if (totalsRow)
            {
                arrTxtTotalsRow[0] = txtW1Owes;
                arrTxtTotalsRow[1] = txtW2Owes;
                arrTxtTotalsRow[2] = txtW3Owes;
                arrTxtTotalsRow[3] = txtW4Owes;
                arrTxtTotalsRow[4] = txtW5Owes;
                arrTxtTotalsRow[5] = txtCards;
                arrTxtTotalsRow[6] = txtSales;

                RefreshTotalsRow();
                
            }
            // add panel to floor cards panel
            cc.Add(pnl);
            // set this panel's location in floor cards panel
            pnl.Location = p;
            lstPnlFloorCards.Add(pnl);
        }
        private void loadPullTabComboBoxes()
        {
            // method that loads all the pulltab names into the comboboxes for pulltabs
            foreach (PullTab pt in DbOps.PullTabs)
            {
                cbxPT01Name.Items.Add(pt.Description);
                cbxPT02Name.Items.Add(pt.Description);
                cbxPT03Name.Items.Add(pt.Description);
                cbxPT04Name.Items.Add(pt.Description);
                cbxPT05Name.Items.Add(pt.Description);
                cbxPT06Name.Items.Add(pt.Description);
                cbxPT07Name.Items.Add(pt.Description);
                cbxPT08Name.Items.Add(pt.Description);
                cbxPT09Name.Items.Add(pt.Description);
                cbxPT10Name.Items.Add(pt.Description);
                cbxPT11Name.Items.Add(pt.Description);
                cbxPT12Name.Items.Add(pt.Description);
                cbxPT13Name.Items.Add(pt.Description);
                cbxPT14Name.Items.Add(pt.Description);
                cbxPT15Name.Items.Add(pt.Description);
                cbxPT16Name.Items.Add(pt.Description);
                cbxPT17Name.Items.Add(pt.Description);
                cbxPT18Name.Items.Add(pt.Description);
                cbxPT19Name.Items.Add(pt.Description);
            }
            for (int i = 0; i < PullTab.CurrentPullTabs.Count; i++)
            {
                tablePullTabs[i] = PullTab.CurrentPullTabs[i];
                numPullTabs++;
            }

            for (int i = PullTab.CurrentPullTabs.Count; i < 19; i++)
            {
                tablePullTabs[i] = new PullTab();
            }

            // store descriptions while updating serials txts (textChangedEvent will erase)
            string d1 = tablePullTabs[0].Description;
            string d2 = tablePullTabs[1].Description;
            string d3 = tablePullTabs[2].Description;
            string d4 = tablePullTabs[3].Description;
            string d5 = tablePullTabs[4].Description;
            string d6 = tablePullTabs[5].Description;
            // also store gross
            double g1 = tablePullTabs[0].Gross;
            double g2 = tablePullTabs[1].Gross;
            double g3 = tablePullTabs[2].Gross;
            double g4 = tablePullTabs[3].Gross;
            double g5 = tablePullTabs[4].Gross;
            double g6 = tablePullTabs[5].Gross;
            // update serial txtx
            txtPT01Serial.Text = tablePullTabs[0].Serial;
            txtPT02Serial.Text = tablePullTabs[1].Serial;
            txtPT03Serial.Text = tablePullTabs[2].Serial;
            txtPT04Serial.Text = tablePullTabs[3].Serial;
            txtPT05Serial.Text = tablePullTabs[4].Serial;
            txtPT06Serial.Text = tablePullTabs[5].Serial;
            // restore gross values
            tablePullTabs[0].Gross = g1;
            tablePullTabs[1].Gross = g2;
            tablePullTabs[2].Gross = g3;
            tablePullTabs[3].Gross = g4;
            tablePullTabs[4].Gross = g5;
            tablePullTabs[5].Gross = g6;
            // update combos with stored descriptions
            cbxPT01Name.SelectedItem = d1;
            cbxPT02Name.SelectedItem = d2;
            cbxPT03Name.SelectedItem = d3;
            cbxPT04Name.SelectedItem = d4;
            cbxPT05Name.SelectedItem = d5;
            cbxPT06Name.SelectedItem = d6;

        }
        private void MoveBoxesFloor(object sender, PreviewKeyDownEventArgs e)
        {
            // use arrows to navigate floor tabs page
            if (e.KeyCode == Keys.Left)
            {
                SendKeys.Send("+{TAB}");
            }
            else if (e.KeyCode == Keys.Right)
            {
                SendKeys.Send("{TAB}");
            }
            else if (e.KeyCode == Keys.Up)
            {

                SendKeys.Send("+{TAB}");
                SendKeys.Send("+{TAB}");
                SendKeys.Send("+{TAB}");
                SendKeys.Send("+{TAB}");
                SendKeys.Send("+{TAB}");
            }
            else if (e.KeyCode == Keys.Down)
            {
                SendKeys.Send("{TAB}");
                SendKeys.Send("{TAB}");
                SendKeys.Send("{TAB}");
                SendKeys.Send("{TAB}");
                SendKeys.Send("{TAB}");
            }
        }
        public void RefreshPullTabTotals()
        {
            // create variables to calculate pulltab worker totals and gross
            double gross = 0;
            int w1total = 0;
            int w2total = 0;
            int w3total = 0;
            int w4total = 0;
            int w5total = 0;

            // loop to calculate totals
            foreach (PullTab pt in tablePullTabs)
            {
                gross += pt.Gross;
                w1total += pt.QtyWorker1;
                w2total += pt.QtyWorker2;
                w3total += pt.QtyWorker3;
                w4total += pt.QtyWorker4;
                w5total += pt.QtyWorker5;
            }
            // set text to new values
            txtPTTotalW1.Text = w1total.ToString();
            txtPTTotalW2.Text = w2total.ToString();
            txtPTTotalW3.Text = w3total.ToString();
            txtPTTotalW4.Text = w4total.ToString();
            txtPTTotalW5.Text = w5total.ToString();
            lblPTGrandTotal.Text = gross.ToString("n");
        }
        private void RefreshTotalsRow()
        {
            // for floor cards store new values and update the appropriate textboxes

            // create doubles to hold amounts sold in dollars
            double totalW1 = 0.0;
            double totalW2 = 0.0;
            double totalW3 = 0.0;
            double totalW4 = 0.0;
            double totalW5 = 0.0;
            int totalC = 0;
            double totalS = 0.0;

            // get totals for each worker
            foreach (TextBox t in lstTxtW1Owes)
            {
                totalW1 += double.Parse(t.Text);
            }
            foreach (TextBox t in lstTxtW2Owes)
            {
                totalW2 += double.Parse(t.Text);
            }
            foreach (TextBox t in lstTxtW3Owes)
            {
                totalW3 += double.Parse(t.Text);
            }
            foreach (TextBox t in lstTxtW4Owes)
            {
                totalW4 += double.Parse(t.Text);
            }
            foreach (TextBox t in lstTxtW5Owes)
            {
                totalW5 += double.Parse(t.Text);
            }
            foreach (TextBox t in lstTxtCards)
            {
                totalC += int.Parse(t.Text);
            }
            foreach (TextBox t in lstTxtSales)
            {
                totalS += double.Parse(t.Text);
            }
            // store new totals in textboxes 
            arrTxtTotalsRow[0].Text = totalW1.ToString("n2");
            arrTxtTotalsRow[1].Text = totalW2.ToString("n2");
            arrTxtTotalsRow[2].Text = totalW3.ToString("n2");
            arrTxtTotalsRow[3].Text = totalW4.ToString("n2");
            arrTxtTotalsRow[4].Text = totalW5.ToString("n2");
            arrTxtTotalsRow[5].Text = totalC.ToString();
            arrTxtTotalsRow[6].Text = totalS.ToString("n2");
        }
        public void RefreshWorkers()
        {
            // when workers combos change, get data from all of them and reset the appropriate labels and comboboxes

            if (!refreshingWorkers)
            {
                refreshingWorkers = true;
                numWorkers = 0;
                // get values form each combobox
                string w1 = arrCbxWorkers[0].Text.Trim();
                string w2 = arrCbxWorkers[1].Text.Trim();
                string w3 = arrCbxWorkers[2].Text.Trim();
                string w4 = arrCbxWorkers[3].Text.Trim();
                string w5 = arrCbxWorkers[4].Text.Trim();
                // test for length and increment numWorkers counter
                if (w1.Length > 0)
                {
                    Worker.CurrentPTDeskWorker = new Worker(w1);
                    numWorkers++;
                }
                if (w2.Length > 0)
                {
                    Worker.CurrentPTDeskWorker = new Worker(w2);
                    numWorkers++;
                }
                if (w3.Length > 0)
                {
                    Worker.CurrentPTDeskWorker = new Worker(w3);
                    numWorkers++;
                }
                if (w4.Length > 0)
                {
                    Worker.CurrentPTDeskWorker = new Worker(w4);
                    numWorkers++;
                }
                if (w5.Length > 0)
                {
                    Worker.CurrentPTDeskWorker = new Worker(w5);
                    numWorkers++;
                }
                // set label text based on new values
                lblFloor1.Text = w1;
                lblFloor2.Text = w2;
                lblFloor3.Text = w3;
                lblFloor4.Text = w4;
                lblFloor5.Text = w5;
                // set comboboxes based on new values
                arrCbxWorkers[0].Text = w1;
                arrCbxWorkers[1].Text = w2;
                arrCbxWorkers[2].Text = w3;
                arrCbxWorkers[3].Text = w4;
                arrCbxWorkers[4].Text = w5;


                // if no workers, set generic PT Desk name
                if (numWorkers == 0)
                {
                    Worker.CurrentPTDeskWorker = new Worker("PT Desk");
                    lblFloor1.Text = "PT Desk";
                    arrCbxWorkers[0].Text = "PT Desk";
                }
                refreshingWorkers = false;
            }
        }
        private void RemovePT()
        {
            // event to remove an entire row of controls for a pull tab on pulltab tabpage
            switch (numPullTabs)
            {
                case 1:
                    pbxRemovePT.Visible = false;
                    txtPT01Serial.Visible = false;
                    cbxPT01Name.Visible = false;
                    txtPT01Floor1.Visible = false;
                    txtPT01Floor2.Visible = false;
                    txtPT01Floor3.Visible = false;
                    txtPT01Floor4.Visible = false;
                    txtPT01Floor5.Visible = false;
                    lblPT01Total.Visible = false;
                    txtPT01Serial.Clear();
                    cbxPT01Name.SelectedIndex = -1;
                    txtPT01Floor1.Clear();
                    txtPT01Floor2.Clear();
                    txtPT01Floor3.Clear();
                    txtPT01Floor4.Clear();
                    txtPT01Floor5.Clear();
                    lblPT01Total.Text = "0/0";
                    pnlPTTotalsRow.Location = new Point(pnlPTTotalsRow.Location.X, pnlPTTotalsRow.Location.Y - 36);
                    pnlPullTabs.Height -= 36;
                    break;
                case 2:
                    txtPT02Serial.Visible = false;
                    cbxPT02Name.Visible = false;
                    txtPT02Floor1.Visible = false;
                    txtPT02Floor2.Visible = false;
                    txtPT02Floor3.Visible = false;
                    txtPT02Floor4.Visible = false;
                    txtPT02Floor5.Visible = false;
                    lblPT02Total.Visible = false;
                    txtPT02Serial.Clear();
                    cbxPT02Name.SelectedIndex = -1;
                    txtPT02Floor1.Clear();
                    txtPT02Floor2.Clear();
                    txtPT02Floor3.Clear();
                    txtPT02Floor4.Clear();
                    txtPT02Floor5.Clear();
                    lblPT02Total.Text = "0/0";
                    pnlPTTotalsRow.Location = new Point(pnlPTTotalsRow.Location.X, pnlPTTotalsRow.Location.Y - 36);
                    pnlPullTabs.Height -= 36;
                    break;
                case 3:
                    txtPT03Serial.Visible = false;
                    cbxPT03Name.Visible = false;
                    txtPT03Floor1.Visible = false;
                    txtPT03Floor2.Visible = false;
                    txtPT03Floor3.Visible = false;
                    txtPT03Floor4.Visible = false;
                    txtPT03Floor5.Visible = false;
                    lblPT03Total.Visible = false;
                    txtPT03Serial.Clear();
                    cbxPT03Name.SelectedIndex = -1;
                    txtPT03Floor1.Clear();
                    txtPT03Floor2.Clear();
                    txtPT03Floor3.Clear();
                    txtPT03Floor4.Clear();
                    txtPT03Floor5.Clear();
                    lblPT03Total.Text = "0/0";
                    pnlPTTotalsRow.Location = new Point(pnlPTTotalsRow.Location.X, pnlPTTotalsRow.Location.Y - 36);
                    pnlPullTabs.Height -= 36;
                    break;
                case 4:
                    txtPT04Serial.Visible = false;
                    cbxPT04Name.Visible = false;
                    txtPT04Floor1.Visible = false;
                    txtPT04Floor2.Visible = false;
                    txtPT04Floor3.Visible = false;
                    txtPT04Floor4.Visible = false;
                    txtPT04Floor5.Visible = false;
                    lblPT04Total.Visible = false;
                    txtPT04Serial.Clear();
                    cbxPT04Name.SelectedIndex = -1;
                    txtPT04Floor1.Clear();
                    txtPT04Floor2.Clear();
                    txtPT04Floor3.Clear();
                    txtPT04Floor4.Clear();
                    txtPT04Floor5.Clear();
                    lblPT04Total.Text = "0/0";
                    pnlPTTotalsRow.Location = new Point(pnlPTTotalsRow.Location.X, pnlPTTotalsRow.Location.Y - 36);
                    pnlPullTabs.Height -= 36;
                    break;
                case 5:
                    txtPT05Serial.Visible = false;
                    cbxPT05Name.Visible = false;
                    txtPT05Floor1.Visible = false;
                    txtPT05Floor2.Visible = false;
                    txtPT05Floor3.Visible = false;
                    txtPT05Floor4.Visible = false;
                    txtPT05Floor5.Visible = false;
                    lblPT05Total.Visible = false;
                    txtPT05Serial.Clear();
                    cbxPT05Name.SelectedIndex = -1;
                    txtPT05Floor1.Clear();
                    txtPT05Floor2.Clear();
                    txtPT05Floor3.Clear();
                    txtPT05Floor4.Clear();
                    txtPT05Floor5.Clear();
                    lblPT05Total.Text = "0/0";
                    pnlPTTotalsRow.Location = new Point(pnlPTTotalsRow.Location.X, pnlPTTotalsRow.Location.Y - 36);
                    pnlPullTabs.Height -= 36;
                    break;
                case 6:
                    txtPT06Serial.Visible = false;
                    cbxPT06Name.Visible = false;
                    txtPT06Floor1.Visible = false;
                    txtPT06Floor2.Visible = false;
                    txtPT06Floor3.Visible = false;
                    txtPT06Floor4.Visible = false;
                    txtPT06Floor5.Visible = false;
                    lblPT06Total.Visible = false;
                    txtPT06Serial.Clear();
                    cbxPT06Name.SelectedIndex = -1;
                    txtPT06Floor1.Clear();
                    txtPT06Floor2.Clear();
                    txtPT06Floor3.Clear();
                    txtPT06Floor4.Clear();
                    txtPT06Floor5.Clear();
                    lblPT06Total.Text = "0/0";
                    pnlPTTotalsRow.Location = new Point(pnlPTTotalsRow.Location.X, pnlPTTotalsRow.Location.Y - 36);
                    pnlPullTabs.Height -= 36;
                    break;
                case 7:
                    txtPT07Serial.Visible = false;
                    cbxPT07Name.Visible = false;
                    txtPT07Floor1.Visible = false;
                    txtPT07Floor2.Visible = false;
                    txtPT07Floor3.Visible = false;
                    txtPT07Floor4.Visible = false;
                    txtPT07Floor5.Visible = false;
                    lblPT07Total.Visible = false;
                    txtPT07Serial.Clear();
                    cbxPT07Name.SelectedIndex = -1;
                    txtPT07Floor1.Clear();
                    txtPT07Floor2.Clear();
                    txtPT07Floor3.Clear();
                    txtPT07Floor4.Clear();
                    txtPT07Floor5.Clear();
                    lblPT07Total.Text = "0/0";
                    pnlPTTotalsRow.Location = new Point(pnlPTTotalsRow.Location.X, pnlPTTotalsRow.Location.Y - 36);
                    pnlPullTabs.Height -= 36;
                    break;
                case 8:
                    txtPT08Serial.Visible = false;
                    cbxPT08Name.Visible = false;
                    txtPT08Floor1.Visible = false;
                    txtPT08Floor2.Visible = false;
                    txtPT08Floor3.Visible = false;
                    txtPT08Floor4.Visible = false;
                    txtPT08Floor5.Visible = false;
                    lblPT08Total.Visible = false;
                    txtPT08Serial.Clear();
                    cbxPT08Name.SelectedIndex = -1;
                    txtPT08Floor1.Clear();
                    txtPT08Floor2.Clear();
                    txtPT08Floor3.Clear();
                    txtPT08Floor4.Clear();
                    txtPT08Floor5.Clear();
                    lblPT08Total.Text = "0/0";
                    pnlPTTotalsRow.Location = new Point(pnlPTTotalsRow.Location.X, pnlPTTotalsRow.Location.Y - 36);
                    pnlPullTabs.Height -= 36;
                    break;
                case 9:
                    txtPT09Serial.Visible = false;
                    cbxPT09Name.Visible = false;
                    txtPT09Floor1.Visible = false;
                    txtPT09Floor2.Visible = false;
                    txtPT09Floor3.Visible = false;
                    txtPT09Floor4.Visible = false;
                    txtPT09Floor5.Visible = false;
                    lblPT09Total.Visible = false;
                    txtPT09Serial.Clear();
                    cbxPT09Name.SelectedIndex = -1;
                    txtPT09Floor1.Clear();
                    txtPT09Floor2.Clear();
                    txtPT09Floor3.Clear();
                    txtPT09Floor4.Clear();
                    txtPT09Floor5.Clear();
                    lblPT09Total.Text = "0/0";
                    pnlPTTotalsRow.Location = new Point(pnlPTTotalsRow.Location.X, pnlPTTotalsRow.Location.Y - 36);
                    pnlPullTabs.Height -= 36;
                    break;
                case 10:
                    txtPT10Serial.Visible = false;
                    cbxPT10Name.Visible = false;
                    txtPT10Floor1.Visible = false;
                    txtPT10Floor2.Visible = false;
                    txtPT10Floor3.Visible = false;
                    txtPT10Floor4.Visible = false;
                    txtPT10Floor5.Visible = false;
                    lblPT10Total.Visible = false;
                    txtPT10Serial.Clear();
                    cbxPT10Name.SelectedIndex = -1;
                    txtPT10Floor1.Clear();
                    txtPT10Floor2.Clear();
                    txtPT10Floor3.Clear();
                    txtPT10Floor4.Clear();
                    txtPT10Floor5.Clear();
                    lblPT10Total.Text = "0/0";
                    pnlPTTotalsRow.Location = new Point(pnlPTTotalsRow.Location.X, pnlPTTotalsRow.Location.Y - 36);
                    pnlPullTabs.Height -= 36;
                    break;
                case 11:
                    txtPT11Serial.Visible = false;
                    cbxPT11Name.Visible = false;
                    txtPT11Floor1.Visible = false;
                    txtPT11Floor2.Visible = false;
                    txtPT11Floor3.Visible = false;
                    txtPT11Floor4.Visible = false;
                    txtPT11Floor5.Visible = false;
                    lblPT11Total.Visible = false;
                    txtPT11Serial.Clear();
                    cbxPT11Name.SelectedIndex = -1;
                    txtPT11Floor1.Clear();
                    txtPT11Floor2.Clear();
                    txtPT11Floor3.Clear();
                    txtPT11Floor4.Clear();
                    txtPT11Floor5.Clear();
                    lblPT11Total.Text = "0/0";
                    pnlPTTotalsRow.Location = new Point(pnlPTTotalsRow.Location.X, pnlPTTotalsRow.Location.Y - 36);
                    pnlPullTabs.Height -= 36;
                    break;
                case 12:
                    txtPT12Serial.Visible = false;
                    cbxPT12Name.Visible = false;
                    txtPT12Floor1.Visible = false;
                    txtPT12Floor2.Visible = false;
                    txtPT12Floor3.Visible = false;
                    txtPT12Floor4.Visible = false;
                    txtPT12Floor5.Visible = false;
                    lblPT12Total.Visible = false;
                    txtPT12Serial.Clear();
                    cbxPT12Name.SelectedIndex = -1;
                    txtPT12Floor1.Clear();
                    txtPT12Floor2.Clear();
                    txtPT12Floor3.Clear();
                    txtPT12Floor4.Clear();
                    txtPT12Floor5.Clear();
                    lblPT12Total.Text = "0/0";
                    pnlPTTotalsRow.Location = new Point(pnlPTTotalsRow.Location.X, pnlPTTotalsRow.Location.Y - 36);
                    pnlPullTabs.Height -= 36;
                    break;
                case 13:
                    txtPT13Serial.Visible = false;
                    cbxPT13Name.Visible = false;
                    txtPT13Floor1.Visible = false;
                    txtPT13Floor2.Visible = false;
                    txtPT13Floor3.Visible = false;
                    txtPT13Floor4.Visible = false;
                    txtPT13Floor5.Visible = false;
                    lblPT13Total.Visible = false;
                    txtPT13Serial.Clear();
                    cbxPT13Name.SelectedIndex = -1;
                    txtPT13Floor1.Clear();
                    txtPT13Floor2.Clear();
                    txtPT13Floor3.Clear();
                    txtPT13Floor4.Clear();
                    txtPT13Floor5.Clear();
                    lblPT13Total.Text = "0/0";
                    pnlPTTotalsRow.Location = new Point(pnlPTTotalsRow.Location.X, pnlPTTotalsRow.Location.Y - 36);
                    pnlPullTabs.Height -= 36;
                    break;
                case 14:
                    txtPT14Serial.Visible = false;
                    cbxPT14Name.Visible = false;
                    txtPT14Floor1.Visible = false;
                    txtPT14Floor2.Visible = false;
                    txtPT14Floor3.Visible = false;
                    txtPT14Floor4.Visible = false;
                    txtPT14Floor5.Visible = false;
                    lblPT14Total.Visible = false;
                    txtPT14Serial.Clear();
                    cbxPT14Name.SelectedIndex = -1;
                    txtPT14Floor1.Clear();
                    txtPT14Floor2.Clear();
                    txtPT14Floor3.Clear();
                    txtPT14Floor4.Clear();
                    txtPT14Floor5.Clear();
                    lblPT14Total.Text = "0/0";
                    pnlPTTotalsRow.Location = new Point(pnlPTTotalsRow.Location.X, pnlPTTotalsRow.Location.Y - 36);
                    pnlPullTabs.Height -= 36;
                    break;
                case 15:
                    txtPT15Serial.Visible = false;
                    cbxPT15Name.Visible = false;
                    txtPT15Floor1.Visible = false;
                    txtPT15Floor2.Visible = false;
                    txtPT15Floor3.Visible = false;
                    txtPT15Floor4.Visible = false;
                    txtPT15Floor5.Visible = false;
                    lblPT15Total.Visible = false;
                    txtPT15Serial.Clear();
                    cbxPT15Name.SelectedIndex = -1;
                    txtPT15Floor1.Clear();
                    txtPT15Floor2.Clear();
                    txtPT15Floor3.Clear();
                    txtPT15Floor4.Clear();
                    txtPT15Floor5.Clear();
                    lblPT15Total.Text = "0/0";
                    pnlPTTotalsRow.Location = new Point(pnlPTTotalsRow.Location.X, pnlPTTotalsRow.Location.Y - 36);
                    pnlPullTabs.Height -= 36;
                    break;
                case 16:
                    txtPT16Serial.Visible = false;
                    cbxPT16Name.Visible = false;
                    txtPT16Floor1.Visible = false;
                    txtPT16Floor2.Visible = false;
                    txtPT16Floor3.Visible = false;
                    txtPT16Floor4.Visible = false;
                    txtPT16Floor5.Visible = false;
                    lblPT16Total.Visible = false;
                    txtPT16Serial.Clear();
                    cbxPT16Name.SelectedIndex = -1;
                    txtPT16Floor1.Clear();
                    txtPT16Floor2.Clear();
                    txtPT16Floor3.Clear();
                    txtPT16Floor4.Clear();
                    txtPT16Floor5.Clear();
                    lblPT16Total.Text = "0/0";
                    pnlPTTotalsRow.Location = new Point(pnlPTTotalsRow.Location.X, pnlPTTotalsRow.Location.Y - 36);
                    pnlPullTabs.Width -= 17;
                    break;
                case 17:
                    txtPT17Serial.Visible = false;
                    cbxPT17Name.Visible = false;
                    txtPT17Floor1.Visible = false;
                    txtPT17Floor2.Visible = false;
                    txtPT17Floor3.Visible = false;
                    txtPT17Floor4.Visible = false;
                    txtPT17Floor5.Visible = false;
                    lblPT17Total.Visible = false;
                    txtPT17Serial.Clear();
                    cbxPT17Name.SelectedIndex = -1;
                    txtPT17Floor1.Clear();
                    txtPT17Floor2.Clear();
                    txtPT17Floor3.Clear();
                    txtPT17Floor4.Clear();
                    txtPT17Floor5.Clear();
                    lblPT17Total.Text = "0/0";
                    pnlPTTotalsRow.Location = new Point(pnlPTTotalsRow.Location.X, pnlPTTotalsRow.Location.Y - 36);
                    break;
                case 18:
                    txtPT18Serial.Visible = false;
                    cbxPT18Name.Visible = false;
                    txtPT18Floor1.Visible = false;
                    txtPT18Floor2.Visible = false;
                    txtPT18Floor3.Visible = false;
                    txtPT18Floor4.Visible = false;
                    txtPT18Floor5.Visible = false;
                    lblPT18Total.Visible = false;
                    txtPT18Serial.Clear();
                    cbxPT18Name.SelectedIndex = -1;
                    txtPT18Floor1.Clear();
                    txtPT18Floor2.Clear();
                    txtPT18Floor3.Clear();
                    txtPT18Floor4.Clear();
                    txtPT18Floor5.Clear();
                    lblPT18Total.Text = "0/0";
                    pnlPTTotalsRow.Location = new Point(pnlPTTotalsRow.Location.X, pnlPTTotalsRow.Location.Y - 36);
                    break;
                case 19:
                    txtPT19Serial.Visible = false;
                    cbxPT19Name.Visible = false;
                    txtPT19Floor1.Visible = false;
                    txtPT19Floor2.Visible = false;
                    txtPT19Floor3.Visible = false;
                    txtPT19Floor4.Visible = false;
                    txtPT19Floor5.Visible = false;
                    lblPT19Total.Visible = false;
                    txtPT19Serial.Clear();
                    cbxPT19Name.SelectedIndex = -1;
                    txtPT19Floor1.Clear();
                    txtPT19Floor2.Clear();
                    txtPT19Floor3.Clear();
                    txtPT19Floor4.Clear();
                    txtPT19Floor5.Clear();
                    lblPT19Total.Text = "0/0";
                    pnlPTTotalsRow.Location = new Point(pnlPTTotalsRow.Location.X, pnlPTTotalsRow.Location.Y - 36);
                    pbxAddPT.Visible = true;
                    break;
                default:
                    break;

            }
            numPullTabs--;
        }

        // formatting functions for form (in alphabetical order)
        #region FormattingFunctions
        private void FormatElectronicsDGV()
        {
            // sets datasource for electronics 
            DataTable DtCurrentElectronics = new DataTable("Current Machines");
            // create columns
            DataColumn colName = DtCurrentElectronics.Columns.Add("Electronic Device", Type.GetType("System.String"));
            DataColumn colSerial = DtCurrentElectronics.Columns.Add("Sold", Type.GetType("System.String"));
            DataColumn colStartNum = DtCurrentElectronics.Columns.Add("Revenue", Type.GetType("System.String"));
            // add table to dataset
            DsCurrentElectronics.Tables.Add(DtCurrentElectronics);
            // set readonly columns
            DsCurrentElectronics.Tables[0].Columns[0].ReadOnly = true;
            // add machines as rows
            foreach (Machine m in Machine.CurrentMachines)
            {
                DataRow dr = DsCurrentElectronics.Tables["Current Machines"].NewRow();
                dr["Electronic Device"] = m.Description;
                dr["Sold"] = "";
                dr["Revenue"] = "";
                DsCurrentElectronics.Tables["Current Machines"].Rows.Add(dr);
            }
            //set font and datasource of dgv
            dgvElectronics.Font = new Font("Arial", 14, FontStyle.Regular);
            dgvElectronics.DataSource = DsCurrentElectronics.Tables["Current Machines"];
            // set width of columns
            int x = 100;
            dgvElectronics.Columns[1].Width = x;
            dgvElectronics.Columns[2].Width = x;
            dgvElectronics.Columns["Electronic Device"].DefaultCellStyle.BackColor = Color.LightGray;
            ((DataGridViewTextBoxColumn)dgvElectronics.Columns[1]).MaxInputLength = 4;
            ((DataGridViewTextBoxColumn)dgvElectronics.Columns[2]).MaxInputLength = 8;
            dgvElectronics.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvElectronics.AllowUserToAddRows = false;
            dgvElectronics.AllowUserToDeleteRows = false;
            dgvElectronics.AllowUserToOrderColumns = false;
            dgvElectronics.AllowUserToResizeColumns = false;
            dgvElectronics.AllowUserToResizeRows = false;
            dgvElectronics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvElectronics.RowHeadersVisible = false;
            dgvElectronics.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 14, FontStyle.Bold);
            // disallow sorting
            foreach (DataGridViewColumn c in dgvElectronics.Columns)
            {
                c.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            // set height based on number of rows
            int totalHeight = 32;
            foreach (DataGridViewRow dgvrow in dgvElectronics.Rows)
            {
                totalHeight += 26;
            }
            dgvElectronics.Height = totalHeight;
        }
        private void FormatPaperSetDGV()
        {
            // define dataset for paper dgv
            // create table
            DataTable DtCurrentPaperSets = new DataTable("Current Paper Sets");
            // create columns
            DataColumn colName = DtCurrentPaperSets.Columns.Add("Paper Set", Type.GetType("System.String"));
            DataColumn colSerial = DtCurrentPaperSets.Columns.Add("Serial", Type.GetType("System.String"));
            DataColumn colStartNum = DtCurrentPaperSets.Columns.Add("Start#", Type.GetType("System.String"));
            DataColumn colEndNum = DtCurrentPaperSets.Columns.Add("End#", Type.GetType("System.String"));
            DataColumn colIssued = DtCurrentPaperSets.Columns.Add("Issued", Type.GetType("System.String"));
            DataColumn colReturned = DtCurrentPaperSets.Columns.Add("Returned", Type.GetType("System.String"));
            DataColumn colSold = DtCurrentPaperSets.Columns.Add("Sold", Type.GetType("System.String"));
            DataColumn colGross = DtCurrentPaperSets.Columns.Add("Gross", Type.GetType("System.String"));
            // add table to dataset
            DsCurrentPaperSets.Tables.Add(DtCurrentPaperSets);
            // set readonly columns
            DsCurrentPaperSets.Tables[0].Columns[0].ReadOnly = true;
            DsCurrentPaperSets.Tables[0].Columns[6].ReadOnly = true;
            DsCurrentPaperSets.Tables[0].Columns[7].ReadOnly = true;
            // add paper sets as rows to table
            foreach (PaperSet ps in PaperSet.CurrentPaperSets)
            {
                DataRow dr = DsCurrentPaperSets.Tables["Current Paper Sets"].NewRow();
                dr["Paper Set"] = ps.Description + " (a)";
                dr["Serial"] = ps.Serial1;
                if (ps.StartNum1 > 0)
                    dr["Start#"] = ps.StartNum1.ToString();
                else
                    dr["Start#"] = "";
                dr["End#"] = "";
                if (ps.QtyIssued1 > 0)
                    dr["Issued"] = ps.QtyIssued1.ToString();
                else
                    dr["Issued"] = "";
                dr["Returned"] = "";
                dr["Sold"] = "";
                dr["Gross"] = (0).ToString("c");
                DsCurrentPaperSets.Tables["Current Paper Sets"].Rows.Add(dr);

                DataRow dr2 = DsCurrentPaperSets.Tables["Current Paper Sets"].NewRow();
                dr2["Paper Set"] = ps.Description + " (b)";
                dr2["Serial"] = ps.Serial2;
                if (ps.StartNum2 > 0)
                    dr2["Start#"] = ps.StartNum2.ToString();
                else
                    dr2["Start#"] = "";
                dr2["End#"] = "";
                if (ps.QtyIssued2 > 0)
                    dr2["Issued"] = ps.QtyIssued2.ToString();
                else
                    dr2["Issued"] = "";
                dr2["Returned"] = "";
                dr2["Sold"] = "";
                dr2["Gross"] = (0).ToString("c");
                DsCurrentPaperSets.Tables["Current Paper Sets"].Rows.Add(dr2);
            }
            // set fonts, datasource, column width, colors, and max inputs of datagridview
            dgvPaperSets.Font = new Font("Arial", 14, FontStyle.Regular);
            dgvPaperSets.DataSource = DsCurrentPaperSets.Tables["Current Paper Sets"];
            int x = 100;
            dgvPaperSets.Columns[1].Width = x;
            dgvPaperSets.Columns[2].Width = x;
            dgvPaperSets.Columns[3].Width = x;
            dgvPaperSets.Columns[4].Width = x;
            dgvPaperSets.Columns[5].Width = x;
            dgvPaperSets.Columns[6].Width = x;
            dgvPaperSets.Columns[7].Width = x;
            dgvPaperSets.Columns[0].DefaultCellStyle.BackColor = Color.LightGray;
            dgvPaperSets.Columns[6].DefaultCellStyle.BackColor = Color.LightGray;
            dgvPaperSets.Columns[7].DefaultCellStyle.BackColor = Color.LightGray;
            ((DataGridViewTextBoxColumn)dgvPaperSets.Columns[1]).MaxInputLength = 8;
            ((DataGridViewTextBoxColumn)dgvPaperSets.Columns[2]).MaxInputLength = 8;
            ((DataGridViewTextBoxColumn)dgvPaperSets.Columns[3]).MaxInputLength = 8;
            ((DataGridViewTextBoxColumn)dgvPaperSets.Columns[4]).MaxInputLength = 8;
            ((DataGridViewTextBoxColumn)dgvPaperSets.Columns[5]).MaxInputLength = 8;

            // set edit permission on dgv
            dgvPaperSets.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvPaperSets.AllowUserToAddRows = false;
            dgvPaperSets.AllowUserToDeleteRows = false;
            dgvPaperSets.AllowUserToOrderColumns = false;
            dgvPaperSets.AllowUserToResizeColumns = false;
            dgvPaperSets.AllowUserToResizeRows = false;
            dgvPaperSets.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvPaperSets.RowHeadersVisible = false;
            dgvPaperSets.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 14, FontStyle.Bold);
            // disallow sorting
            foreach (DataGridViewColumn c in dgvPaperSets.Columns)
            {
                c.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            // set height based on number of rows
            int totalHeight = 32;
            foreach (DataGridViewRow dgvrow in dgvPaperSets.Rows)
            {
                totalHeight += 26;
            }
            dgvPaperSets.Height = totalHeight;
        }
        private void FormatSessionBingoGamesDGV()
        {
            // define dataset for sessionbingogames
            // create table
            DataTable DtCurrentSessionBingoGames = new DataTable("Current Bingo Games");

            // create columns
            DataColumn colName = DtCurrentSessionBingoGames.Columns.Add("Game", Type.GetType("System.String"));
            DataColumn colSerial = DtCurrentSessionBingoGames.Columns.Add("#Win", Type.GetType("System.String"));
            DataColumn colStartNum = DtCurrentSessionBingoGames.Columns.Add("Prize", Type.GetType("System.String"));
            // add table to dataset
            DsCurrentSessionBingoGames.Tables.Add(DtCurrentSessionBingoGames);
            // set readonly columns
            DsCurrentSessionBingoGames.Tables[0].Columns[0].ReadOnly = true;
            DsCurrentSessionBingoGames.Tables[0].Columns[2].ReadOnly = true;
            // set game numbers based on game type and order
            char gameNum = 'A';
            int intGameNum = 1;
            bool switchToReg = false;
            // add games as rows in table
            foreach (SessionBingoGame sbg in SessionBingoGame.CurrentSessionBingoGames)
            {
                DataRow dr = DsCurrentSessionBingoGames.Tables["Current Bingo Games"].NewRow();
                if (sbg.EarlyBird && !switchToReg)
                {
                    dr["Game"] = (gameNum++).ToString();
                }
                else
                {
                    if (!switchToReg)
                    {
                        dr["Game"] = (intGameNum++).ToString();
                        switchToReg = true;
                    }
                    else
                    {
                        dr["Game"] = (intGameNum++).ToString();
                    }
                }
                dr["#Win"] = "";
                dr["Prize"] = sbg.MaxOrSetPrice.ToString("c");
                DsCurrentSessionBingoGames.Tables["Current Bingo Games"].Rows.Add(dr);
            }
            // set font, widths, colors, and input lengths
            dgvSessionBingoGames.Font = new Font("Arial", 14, FontStyle.Regular);
            dgvSessionBingoGames.DataSource = DsCurrentSessionBingoGames.Tables["Current Bingo Games"];
            dgvSessionBingoGames.Columns[1].Width = 80;
            dgvSessionBingoGames.Columns[2].Width = 90;
            dgvSessionBingoGames.Columns[0].DefaultCellStyle.BackColor = Color.LightGray;
            dgvSessionBingoGames.Columns[2].DefaultCellStyle.BackColor = Color.LightGray;

            ((DataGridViewTextBoxColumn)dgvSessionBingoGames.Columns[1]).MaxInputLength = 3;
            // set edit permissions on dgv
            dgvSessionBingoGames.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvSessionBingoGames.AllowUserToAddRows = false;
            dgvSessionBingoGames.AllowUserToDeleteRows = false;
            dgvSessionBingoGames.AllowUserToOrderColumns = false;
            dgvSessionBingoGames.AllowUserToResizeColumns = false;
            dgvSessionBingoGames.AllowUserToResizeRows = false;
            dgvSessionBingoGames.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvSessionBingoGames.RowHeadersVisible = false;
            dgvSessionBingoGames.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 14, FontStyle.Bold);
            // disallow sorting of columns
            foreach (DataGridViewColumn c in dgvSessionBingoGames.Columns)
            {
                c.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            // set height of dgv based on number of rows
            int totalHeight = 32;
            foreach (DataGridViewRow dgvrow in dgvSessionBingoGames.Rows)
            {
                totalHeight += 26;
            }
            dgvSessionBingoGames.Height = totalHeight;
        }
        #endregion FormattingFunctions

        // event handlers (in alphabetical order)
        #region EventHandlers
        private void AnyTotal_TextChanged(object sender, EventArgs e)
        {
            // when a total changes on the reports page, update all report information

            try
            {
                // reset all report values (so they don't just keep growing and growing!)
                bingoTotalPrizes = 0;
                bingoTotalTax = 0;
                bingoShortTax = 0;
                ptTotalPrizes = 0;
                ptTotalGross = 0;
                ptTotalTax = 0;
                ptShortTax = 0;
                TotalSales = 0;
                TotalPrizes = 0;
                TotalTax = 0;
                TotalShortTax = 0;
                AmountRemoved = 0;
                PTDeskOwes = 0;
                DeskOwes = 0;
                NetDeposit = 0;
                electronicsTotal = 0;
                allPaperSalesTotal = 0;

                // parse strings for valid double characters
                string electronics = "";
                string e2Parse = txtElectronics.Text;
                foreach (char ch in e2Parse)
                    if (Char.IsNumber(ch) || ch == '.')
                        electronics += ch;
                electronicsTotal = double.Parse(electronics);
                string pts = "";
                string pt2Parse = txtPullTabs.Text;
                foreach (char ch in pt2Parse)
                    if (Char.IsNumber(ch) || ch == '.')
                        pts += ch;
                ptTotalGross = double.Parse(pts);
                string papers = "";
                string papers2Parse = txtAllPaperSales.Text;
                foreach (char ch in papers2Parse)
                    if (Char.IsNumber(ch) || ch == '.')
                        papers += ch;
                allPaperSalesTotal = double.Parse(papers);

                // calcuate total sales
                TotalSales = electronicsTotal + ptTotalGross + allPaperSalesTotal;
                txtTotalSales.Text = (TotalSales).ToString("c");


                // calulate total bingo prizes
                foreach (SessionBingoGame sbg in SessionBingoGame.CurrentSessionBingoGames)
                {
                    bingoTotalPrizes += sbg.MaxOrSetPrice;
                }
                // calulate totals for pull tab info
                for (int i = 0; i < numPullTabs; i++)
                {
                    ptTotalPrizes += tablePullTabs[i].Prizes;
                    ptTotalGross += tablePullTabs[i].Gross;
                    ptShortTax += tablePullTabs[i].Tax;
                }
                // calculate total of all prizes, taxes and amounts withheld
                TotalPrizes = bingoTotalPrizes + ptTotalPrizes;
                bingoTotalTax = bingoTotalPrizes * .05;
                ptTotalTax = ptTotalPrizes * .05;
                TotalTax = bingoTotalTax + ptTotalTax;
                TotalShortTax = bingoShortTax + ptShortTax;
                // test for amount removed and set it accordingly
                if (txtAmountRemoved.Text.Length > 0)
                {
                    if (!double.TryParse(txtAmountRemoved.Text, out AmountRemoved))
                    {
                        AmountRemoved = 0;
                    }
                }
                else
                    AmountRemoved = 0;
                // final calculations
                NetDeposit = TotalSales - TotalPrizes + TotalTax - TotalShortTax - AmountRemoved;
                double netChange = NetDeposit - Math.Truncate(NetDeposit);
                PTDeskOwes = Math.Truncate(ptTotalGross - ptTotalPrizes + ptTotalTax - ptShortTax) + netChange;
                DeskOwes = NetDeposit - PTDeskOwes;
                // update texboxes finally
                txtNetDeposit.Text = NetDeposit.ToString("c");
                txtPTDeskOwes.Text = PTDeskOwes.ToString("c");
                txtDeskOwes.Text = DeskOwes.ToString("c");
            }
            catch
            {
                // if error parsing, show user, so they can fix it
                txtTotalSales.Text = "ERR";
                txtNetDeposit.Text = "ERR";
                txtPTDeskOwes.Text = "ERR";
                txtDeskOwes.Text = "ERR";
            }
        }
        private void AutoClick(object sender, KeyPressEventArgs e, ToolStripButton btn)
        {
            // when user presses enter, click button for them (changing serial numbers)
            if (e.KeyChar == (char)Keys.Enter)
            {
                btn.PerformClick();
                e.Handled = true;
            }
        }
        private void btnGenerate_Click(object sender, EventArgs e)
        {
            // generates a report after all data has been entered
            MessageBox.Show("Report Generated");
        }
        private void cbxCharities_SelectedIndexChanged(object sender, EventArgs e)
        {
            // when a new charity has been selected, update the current charity, no need to test for -1 index, because dropdownitem mode
            Charity.CurrentCharity = DbOps.Charities[cbxCharities.SelectedIndex];
        }
        private void chkPettyCash_CheckedChanged(object sender, EventArgs e)
        {
            // sets visibility of amount removed controls to state of check
            lblAmountRemoved.Visible = (chkPettyCash.Checked);
            txtAmountRemoved.Visible = (chkPettyCash.Checked);
        }
        public void DisableSecondFCSerial(object sender, EventArgs e, PictureBox pbx)
        {
            // event handler when minus sign in floor cards is clicked
            // disables textbox for second floor card serial to be entered
            int index = int.Parse(pbx.Name);
            pbx.BackgroundImage = Image.FromFile(@"C:\Users\Administrator\AppData\Roaming\Jeremy Stones\Images\null.png");
            lstPbxAdds[index].BackgroundImage = Image.FromFile(@"C:\Users\Administrator\AppData\Roaming\Jeremy Stones\Images\plus.png");
            lstTxtSerial2[index].Enabled = false;
            lstTxtSerial2[index].BackColor = FloorCard.CurrentFloorCards[int.Parse(pbx.Name)].BackColor;
            FloorCard.CurrentFloorCards[index].QtySerials--;
        }
        private void dgv_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            // when datagridviews are done loading, deselect all cells
            try
            {
                DataGridView dgv = sender as DataGridView;
                dgv.Rows[0].Selected = false;
            }
            catch (Exception) { }
        }
        private void dgv_Leave(object sender, EventArgs e)
        {
            // when a datagridview is left, deselect all cells
            try
            {
                DataGridView dgv = sender as DataGridView;
                for (int i = 0; i < dgv.Rows.Count; i++)
                    dgv.Rows[i].Selected = false;
            }
            catch (Exception) { }
        }
        private void dgvElectronics_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            // disables selection of read only columns
            if (dgvElectronics.CurrentCell.ColumnIndex == 0 || dgvElectronics.CurrentCell.ColumnIndex == 3)
                dgvElectronics.CurrentCell.Selected = false;
        }
        private void dgvElectronics_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            // once a cell has changed in the electronics table, set the new row values and update the total
            if (e.RowIndex > -1)
            {
                DataGridViewRow row = dgvElectronics.Rows[e.RowIndex];
                string qty = row.Cells[1].Value.ToString();
                string revenue = row.Cells[2].Value.ToString();
                int intQty;
                double dblRevenue;
                if (int.TryParse(qty, out intQty))
                {
                    Machine.CurrentMachines[e.RowIndex].QtySold = intQty;
                }
                else
                {
                    Machine.CurrentMachines[e.RowIndex].QtySold = -1;
                }
                if (double.TryParse(revenue, out dblRevenue))
                {
                    Machine.CurrentMachines[e.RowIndex].IndividualRevenue = dblRevenue;
                    row.Cells[2].Value = dblRevenue.ToString("n2");
                    txtElectronics.Text = dblRevenue.ToString("c");
                    txtElectronics.Tag = dblRevenue.ToString();
                }
                else
                {
                    Machine.CurrentMachines[e.RowIndex].IndividualRevenue = -1;
                    txtElectronics.Text = "$0.00";
                }
            }
        }
        private void dgvElectronics_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            // allows user to end edit on arrow key press
            DataGridViewTextBoxEditingControl tb = (DataGridViewTextBoxEditingControl)e.Control;
            tb.PreviewKeyDown += new PreviewKeyDownEventHandler(dgvElectronics_PreviewKeyDown);
            e.Control.PreviewKeyDown += new PreviewKeyDownEventHandler(dgvElectronics_PreviewKeyDown);
        }
        private void dgvElectronics_KeyDown(object sender, KeyEventArgs e)
        {
            // allows user to delete contents of an entire cell 
            if(e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete)
            {
                if (!dgvElectronics.CurrentCell.ReadOnly && dgvElectronics.CurrentCell.RowIndex > -1
                    && dgvElectronics.CurrentCell.ColumnIndex > -1)
                    dgvElectronics.CurrentCell.Value = "";
            }
        }
        private void dgvElectronics_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            // helps user validate cell on arrow key press, rather than pressing enter or clicking out of it
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right || e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
            {
                SendKeys.Send("{ENTER}");
            }
        }
        private void dgvPaperSets_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            // disables select of read only columns
            if (dgvPaperSets.CurrentCell.ColumnIndex == 0 || dgvPaperSets.CurrentCell.ColumnIndex == 6 || dgvPaperSets.CurrentCell.ColumnIndex == 7)
                dgvPaperSets.CurrentCell.Selected = false;
        }
        private void dgvPaperSets_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            // when a paperset value is changed, update the current rows values, including total
            if (e.RowIndex > -1)
            {
                // test to see if row A or B of a paper set type
                int paperSetIndex = (e.RowIndex % 2 == 0) ? e.RowIndex / 2 : (e.RowIndex - 1) / 2;
                // put row in record for easy data manip
                DataGridViewRow row = dgvPaperSets.Rows[e.RowIndex];
                // get values from row cells
                string startNum = row.Cells[2].Value.ToString();
                string endNum = row.Cells[3].Value.ToString();
                string issued = row.Cells[4].Value.ToString();
                string returned = row.Cells[5].Value.ToString();
                // create ints to store string values
                int intStartNum;
                int intEndNum;
                int intIssued;
                int intReturned;
                // try to parse data of each cell and set to 0 if invalid number
                if (int.TryParse(startNum, out intStartNum))
                {
                    if (e.RowIndex % 2 == 0)
                        PaperSet.CurrentPaperSets[paperSetIndex].StartNum1 = intStartNum;
                    else
                        PaperSet.CurrentPaperSets[paperSetIndex].StartNum2 = intStartNum;
                }
                else
                {
                    if (e.RowIndex % 2 == 0)
                        PaperSet.CurrentPaperSets[paperSetIndex].StartNum1 = 0;
                    else
                        PaperSet.CurrentPaperSets[paperSetIndex].StartNum2 = 0;
                }
                if (int.TryParse(endNum, out intEndNum))
                {
                    if (e.RowIndex % 2 == 0)
                        PaperSet.CurrentPaperSets[paperSetIndex].EndNum1 = intEndNum;
                    else
                        PaperSet.CurrentPaperSets[paperSetIndex].EndNum2 = intEndNum;
                }
                else
                {
                    if (e.RowIndex % 2 == 0)
                        PaperSet.CurrentPaperSets[paperSetIndex].EndNum1 = 0;
                    else
                        PaperSet.CurrentPaperSets[paperSetIndex].EndNum2 = 0;
                }
                // if isseud and returned are valid integers, perform calc for sold, otherwise set values to empty/0
                if (int.TryParse(issued, out intIssued) && int.TryParse(returned, out intReturned))
                {
                    DsCurrentPaperSets.Tables[0].Columns[6].ReadOnly = false;
                    DsCurrentPaperSets.Tables[0].Columns[7].ReadOnly = false;
                    dgvPaperSets[6, e.RowIndex].Value = (intIssued - intReturned).ToString();
                    dgvPaperSets[7, e.RowIndex].Value = ((double)(intIssued - intReturned) * PaperSet.CurrentPaperSets[paperSetIndex].Price).ToString("c");
                    DsCurrentPaperSets.Tables[0].Columns[6].ReadOnly = true;
                    DsCurrentPaperSets.Tables[0].Columns[7].ReadOnly = false;
                    if (e.RowIndex % 2 == 0)
                    {
                        PaperSet.CurrentPaperSets[paperSetIndex].QtyIssued1 = intIssued;
                        PaperSet.CurrentPaperSets[paperSetIndex].QtyReturned1 = intReturned;
                        PaperSet.CurrentPaperSets[paperSetIndex].QtySold1 = (intIssued - intReturned);
                        PaperSet.CurrentPaperSets[paperSetIndex].Gross1 = (double)(intIssued - intReturned) * PaperSet.CurrentPaperSets[paperSetIndex].Price;
                    }
                    else
                    {
                        PaperSet.CurrentPaperSets[paperSetIndex].QtyIssued2 = intIssued;
                        PaperSet.CurrentPaperSets[paperSetIndex].QtyReturned2 = intReturned;
                        PaperSet.CurrentPaperSets[paperSetIndex].QtySold2 = (intIssued - intReturned);
                        PaperSet.CurrentPaperSets[paperSetIndex].Gross2 = (double)(intIssued - intReturned) * PaperSet.CurrentPaperSets[paperSetIndex].Price;
                    }
                }
                else
                {
                    DsCurrentPaperSets.Tables[0].Columns[6].ReadOnly = false;
                    DsCurrentPaperSets.Tables[0].Columns[7].ReadOnly = false;
                    dgvPaperSets[6, e.RowIndex].Value = "";
                    dgvPaperSets[7, e.RowIndex].Value = (0).ToString("c");
                    DsCurrentPaperSets.Tables[0].Columns[6].ReadOnly = true;
                    DsCurrentPaperSets.Tables[0].Columns[7].ReadOnly = false;

                    if (e.RowIndex % 2 == 0)
                    {
                        PaperSet.CurrentPaperSets[paperSetIndex].QtySold1 = 0;
                        PaperSet.CurrentPaperSets[paperSetIndex].Gross1 = 0.0;
                    }
                    else
                    {
                        PaperSet.CurrentPaperSets[paperSetIndex].QtySold2 = 0;
                        PaperSet.CurrentPaperSets[paperSetIndex].Gross2 = 0.0;
                    }
                }
                // loop through paper sets and get total gross of both serial numbers
                double totalGrossPaperSets = 0.0;
                foreach(PaperSet ps in PaperSet.CurrentPaperSets)
                {
                    totalGrossPaperSets += ps.Gross1;
                    totalGrossPaperSets += ps.Gross2;
                }
                // store paperset total in tag of label, why not...
                allPaperSalesTotal = totalGrossPaperSets;
                double totalFloorPaper;
                try
                {
                    if (!double.TryParse(txtFloor.Tag.ToString(), out totalFloorPaper))
                    {
                        totalFloorPaper = 0;
                    }
                }
                catch (Exception) { totalFloorPaper = 0; }
                double totalAllPaper = totalGrossPaperSets + totalFloorPaper;
                // update report tabpage
                txtAllPaperSales.Text = totalAllPaper.ToString("c");
                txtAllPaperSales.Tag = totalAllPaper.ToString();

                
            }
        }
        private void dgvPaperSets_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            // while user editing cell, creates a temp object which allows user to end edit on left arrow press
            DataGridViewTextBoxEditingControl tb = (DataGridViewTextBoxEditingControl)e.Control;
            tb.PreviewKeyDown += new PreviewKeyDownEventHandler(dgvPaperSets_PreviewKeyDown);
            e.Control.PreviewKeyDown += new PreviewKeyDownEventHandler(dgvPaperSets_PreviewKeyDown);
        }
        private void dgvPaperSets_KeyDown(object sender, KeyEventArgs e)
        {
            // allows user to delete entire contents of a cell while not in EditingControlShowing
            if(e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete)
            {
                if (!dgvPaperSets.CurrentCell.ReadOnly && dgvPaperSets.CurrentCell.RowIndex > -1
                    && dgvPaperSets.CurrentCell.ColumnIndex > -1)
                    dgvPaperSets.CurrentCell.Value = "";
            }
        }
        private void dgvPaperSets_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            // allows user to end editing on left arrow press
            if(e.KeyCode == Keys.Left)
            {
                dgvPaperSets.EndEdit();
            }
        }
        private void dgvSessionBingoGames_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            // removes selection of read only cells
            if (dgvSessionBingoGames.CurrentCell.ColumnIndex == 0 || dgvSessionBingoGames.CurrentCell.ColumnIndex == 2)
                dgvSessionBingoGames.CurrentCell.Selected = false;
        }
        private void dgvSessionBingoGames_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            // if current row valid, update quantity of winners
            if (e.RowIndex > -1)
            {
                DataGridViewRow row = dgvSessionBingoGames.Rows[e.RowIndex];
                string qty = row.Cells[1].Value.ToString();
                int intQty;
                if (int.TryParse(qty, out intQty))
                    SessionBingoGame.CurrentSessionBingoGames[e.RowIndex].QtyWinners = intQty;
                else
                    SessionBingoGame.CurrentSessionBingoGames[e.RowIndex].QtyWinners = 0;
            }
        }
        private void dgvSessionBingoGames_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            // if control being edited currently, create temp object that has a listener which allows user to end edit on left arrow press
            DataGridViewTextBoxEditingControl tb = (DataGridViewTextBoxEditingControl)e.Control;
            tb.PreviewKeyDown += new PreviewKeyDownEventHandler(dgvSessionBingoGames_PreviewKeyDown);
            e.Control.PreviewKeyDown += new PreviewKeyDownEventHandler(dgvSessionBingoGames_PreviewKeyDown);
        }
        private void dgvSessionBingoGames_KeyDown(object sender, KeyEventArgs e)
        {
            // allows users to delete the contents of an entire cell when not EditingControlShowing
            if (e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete)
            {
                if (!dgvSessionBingoGames.CurrentCell.ReadOnly && dgvSessionBingoGames.CurrentCell.RowIndex > -1
                   && dgvSessionBingoGames.CurrentCell.ColumnIndex > -1)
                    dgvSessionBingoGames.CurrentCell.Value = "";
            }
        }
        private void dgvSessionBingoGames_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            // allows user to end editing of dgv when they press the left arrow
            if(e.KeyCode == Keys.Left)
            {
                dgvSessionBingoGames.EndEdit();
            }
        }
        public void EnableSecondFCSerial(object sender, EventArgs e, PictureBox pbx)
        {
            // event handler when plus sign in floor cards is clicked
            // enables textbox for second floor card serial to be entered
            int index = int.Parse(pbx.Name);
            pbx.BackgroundImage = Image.FromFile(@"C:\Users\Administrator\AppData\Roaming\Jeremy Stones\Images\null.png");
            lstPbxSubs[index].BackgroundImage = Image.FromFile(@"C:\Users\Administrator\AppData\Roaming\Jeremy Stones\Images\minus.png");
            lstTxtSerial2[index].Enabled = true;
            lstTxtSerial2[index].BackColor = Color.White;
            FloorCard.CurrentFloorCards[index].QtySerials++;
        }
        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            // if frmMain closes, close entire application
            Application.Exit();
        }
        private void frmMain_Load(object sender, EventArgs e)
        {
            // on form load perform several tasks:

            // load all pull tab comboboxes
            loadPullTabComboBoxes();
            // reset panel location for pulltabs
            pnlPullTabs.Location = new Point(this.Width / 2 - pnlPullTabs.Width / 2 - 50, pnlPullTabs.Location.Y);
            // get number of pulltabs - a little manipulation to keep correct number while removing controls
            int tempNumPullTabs = numPullTabs;
            numPullTabs = 6;
            for (int i = 0; i < (6 - tempNumPullTabs); i++)
            {
                RemovePT();
            }
            numPullTabs = tempNumPullTabs;

            // set counter to increment through individual floorcards
            int ctr = 0;

            // user variables for location within panel
            int XX = 80;
            int YY = 15;

            // create new label for show Users header
            Label lblFloor = new Label();
            lblFloor.Width = 150;
            lblFloor.TextAlign = ContentAlignment.MiddleRight;
            lblFloor.Text = "Ushers \u2192";
            lblFloor.Font = new Font("Arial", 18, FontStyle.Bold);
            pnlFloorCards.Controls.Add(lblFloor);
            lblFloor.Location = new Point(XX, YY-3); XX += 160;

            // create comboboxes for each floorworker, autocomplete on (selectedindex event at end to manipulate data
            // needed from this function
            ComboBox cbxW1 = new ComboBox();
            cbxW1.Width = 175;
            cbxW1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cbxW1.AutoCompleteSource = AutoCompleteSource.ListItems;
            cbxW1.BackColor = Color.White;
            cbxW1.TabStop = false;
            arrCbxWorkers[0] = cbxW1;
            ComboBox cbxW2 = new ComboBox();
            cbxW2.Width = 175;
            cbxW2.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cbxW2.AutoCompleteSource = AutoCompleteSource.ListItems;
            cbxW2.BackColor = Color.White;
            cbxW2.TabStop = false;
            arrCbxWorkers[1] = cbxW2;
            ComboBox cbxW3 = new ComboBox();
            cbxW3.Width = 175;
            cbxW3.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cbxW3.AutoCompleteSource = AutoCompleteSource.ListItems;
            cbxW3.BackColor = Color.White;
            cbxW3.TabStop = false;
            arrCbxWorkers[2] = cbxW3;
            ComboBox cbxW4 = new ComboBox();
            cbxW4.Width = 175;
            cbxW4.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cbxW4.AutoCompleteSource = AutoCompleteSource.ListItems;
            cbxW4.BackColor = Color.White;
            cbxW4.TabStop = false;
            arrCbxWorkers[3] = cbxW4;
            ComboBox cbxW5 = new ComboBox();
            cbxW5.Width = 175;
            cbxW5.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cbxW5.AutoCompleteSource = AutoCompleteSource.ListItems;
            cbxW5.BackColor = Color.White;
            cbxW5.TabStop = false;
            arrCbxWorkers[4] = cbxW5;

            //add data to the comboboxes (worker names)
            foreach(Worker w in DbOps.Workers)
            {
                cbxW1.Items.Add(w.Name);
                cbxW2.Items.Add(w.Name);
                cbxW3.Items.Add(w.Name);
                cbxW4.Items.Add(w.Name);
                cbxW5.Items.Add(w.Name);
            }
            // add comboboxes to the panel
            pnlFloorCards.Controls.Add(cbxW1);
            pnlFloorCards.Controls.Add(cbxW2);
            pnlFloorCards.Controls.Add(cbxW3);
            pnlFloorCards.Controls.Add(cbxW4);
            pnlFloorCards.Controls.Add(cbxW5);
            // set combobox locations
            cbxW1.Location = new Point(XX, YY); XX += 180;
            cbxW2.Location = new Point(XX, YY); XX += 180;
            cbxW3.Location = new Point(XX, YY); XX += 180;
            cbxW4.Location = new Point(XX, YY); XX += 180;
            cbxW5.Location = new Point(XX, YY); XX += 195;


            // create header for totals
            Label lblTotals = new Label();
            lblTotals.Text = "TOTALS";
            lblTotals.TextAlign = ContentAlignment.BottomCenter;
            lblTotals.Width = 150;
            lblTotals.Height = 30;
            lblTotals.BackColor = Color.Black;
            lblTotals.ForeColor = Color.White;
            lblTotals.Font = new Font("Arial", 18, FontStyle.Bold);
            pnlFloorCards.Controls.Add(lblTotals);
            lblTotals.Location = new Point(XX, YY-3);

            // reset location
            YY += 30;
            XX = 240;

            // create headers to give user useful info about the grid of textboxes
            Label lblOut1 = new Label();
            lblOut1.Text = "OUT";
            lblOut1.Width = 50;
            lblOut1.TextAlign = ContentAlignment.BottomCenter;
            lblOut1.Font = new Font("Arial",12,FontStyle.Bold);
            pnlFloorCards.Controls.Add(lblOut1);
            lblOut1.Location = new Point(XX, YY); XX += 50;

            Label lblIn1 = new Label();
            lblIn1.Text = "IN";
            lblIn1.Width = 50;
            lblIn1.TextAlign = ContentAlignment.BottomCenter;
            lblIn1.Font = new Font("Arial", 12, FontStyle.Bold);
            pnlFloorCards.Controls.Add(lblIn1);
            lblIn1.Location = new Point(XX, YY); XX += 50;

            Label lblOwe1 = new Label();
            lblOwe1.Text = "$";
            lblOwe1.Width = 75;
            lblOwe1.TextAlign = ContentAlignment.BottomCenter;
            lblOwe1.Font = new Font("Arial", 12, FontStyle.Bold);
            pnlFloorCards.Controls.Add(lblOwe1);
            lblOwe1.Location = new Point(XX, YY); XX += 80;

            Label lblOut2 = new Label();
            lblOut2.Text = "OUT";
            lblOut2.Width = 50;
            lblOut2.TextAlign = ContentAlignment.BottomCenter;
            lblOut2.Font = new Font("Arial", 12, FontStyle.Bold);
            pnlFloorCards.Controls.Add(lblOut2);
            lblOut2.Location = new Point(XX, YY); XX += 50;

            Label lblIn2 = new Label();
            lblIn2.Text = "IN";
            lblIn2.Width = 50;
            lblIn2.TextAlign = ContentAlignment.BottomCenter;
            lblIn2.Font = new Font("Arial", 12, FontStyle.Bold);
            pnlFloorCards.Controls.Add(lblIn2);
            lblIn2.Location = new Point(XX, YY); XX += 50;

            Label lblOwe2 = new Label();
            lblOwe2.Text = "$";
            lblOwe2.Width = 75;
            lblOwe2.TextAlign = ContentAlignment.BottomCenter;
            lblOwe2.Font = new Font("Arial", 12, FontStyle.Bold);
            pnlFloorCards.Controls.Add(lblOwe2);
            lblOwe2.Location = new Point(XX, YY); XX += 80;

            Label lblOut3 = new Label();
            lblOut3.Text = "OUT";
            lblOut3.Width = 50;
            lblOut3.TextAlign = ContentAlignment.BottomCenter;
            lblOut3.Font = new Font("Arial", 12, FontStyle.Bold);
            pnlFloorCards.Controls.Add(lblOut3);
            lblOut3.Location = new Point(XX, YY); XX += 50;

            Label lblIn3 = new Label();
            lblIn3.Text = "IN";
            lblIn3.Width = 50;
            lblIn3.TextAlign = ContentAlignment.BottomCenter;
            lblIn3.Font = new Font("Arial", 12, FontStyle.Bold);
            pnlFloorCards.Controls.Add(lblIn3);
            lblIn3.Location = new Point(XX, YY); XX += 50;

            Label lblOwe3 = new Label();
            lblOwe3.Text = "$";
            lblOwe3.Width = 75;
            lblOwe3.TextAlign = ContentAlignment.BottomCenter;
            lblOwe3.Font = new Font("Arial", 12, FontStyle.Bold);
            pnlFloorCards.Controls.Add(lblOwe3);
            lblOwe3.Location = new Point(XX, YY); XX += 80;

            Label lblOut4 = new Label();
            lblOut4.Text = "OUT";
            lblOut4.Width = 50;
            lblOut4.TextAlign = ContentAlignment.BottomCenter;
            lblOut4.Font = new Font("Arial", 12, FontStyle.Bold);
            pnlFloorCards.Controls.Add(lblOut4);
            lblOut4.Location = new Point(XX, YY); XX += 50;

            Label lblIn4 = new Label();
            lblIn4.Text = "IN";
            lblIn4.Width = 50;
            lblIn4.TextAlign = ContentAlignment.BottomCenter;
            lblIn4.Font = new Font("Arial", 12, FontStyle.Bold);
            pnlFloorCards.Controls.Add(lblIn4);
            lblIn4.Location = new Point(XX, YY); XX += 50;

            Label lblOwe4 = new Label();
            lblOwe4.Text = "$";
            lblOwe4.Width = 75;
            lblOwe4.TextAlign = ContentAlignment.BottomCenter;
            lblOwe4.Font = new Font("Arial", 12, FontStyle.Bold);
            pnlFloorCards.Controls.Add(lblOwe4);
            lblOwe4.Location = new Point(XX, YY); XX += 80;

            Label lblOut5 = new Label();
            lblOut5.Text = "OUT";
            lblOut5.Width = 50;
            lblOut5.TextAlign = ContentAlignment.BottomCenter;
            lblOut5.Font = new Font("Arial", 12, FontStyle.Bold);
            pnlFloorCards.Controls.Add(lblOut5);
            lblOut5.Location = new Point(XX, YY); XX += 50;

            Label lblIn5 = new Label();
            lblIn5.Text = "IN";
            lblIn5.Width = 50;
            lblIn5.TextAlign = ContentAlignment.BottomCenter;
            lblIn5.Font = new Font("Arial", 12, FontStyle.Bold);
            pnlFloorCards.Controls.Add(lblIn5);
            lblIn5.Location = new Point(XX, YY); XX += 50;

            Label lblOwe5 = new Label();
            lblOwe5.Text = "$";
            lblOwe5.Width = 75;
            lblOwe5.TextAlign = ContentAlignment.BottomCenter;
            lblOwe5.Font = new Font("Arial", 12, FontStyle.Bold);
            pnlFloorCards.Controls.Add(lblOwe5);
            lblOwe5.Location = new Point(XX, YY); XX += 90;

            Label lblCards = new Label();
            lblCards.Text = "CARDS";
            lblCards.Width = 75;
            lblCards.TextAlign = ContentAlignment.BottomCenter;
            lblCards.Font = new Font("Arial", 12, FontStyle.Bold);
            pnlFloorCards.Controls.Add(lblCards);
            lblCards.Location = new Point(XX, YY); XX += 75;

            Label lblTotal = new Label();
            lblTotal.Text = "SALES";
            lblTotal.Width = 75;
            lblTotal.TextAlign = ContentAlignment.BottomCenter;
            lblTotal.Font = new Font("Arial", 12, FontStyle.Bold);
            pnlFloorCards.Controls.Add(lblTotal);
            lblTotal.Location = new Point(XX, YY);

            // jump to next line to begin creating lines for floor cards
            YY += 23;
            foreach (FloorCard f in FloorCard.CurrentFloorCards)
            {
                // loop through every floor card and dynamically create a label, pictures, and textboxes for each
                // set QtySerials to 1
                FloorCard.CurrentFloorCards[ctr++].QtySerials = 1;
                // add controls for floor card
                XX = 10;
                AddColorLabel(f.Description + " (" + f.Price.ToString("C") + ")", pnlFloorCards.Controls, f.BackColor, 220, 34, new Point(XX, YY)); XX += 230;
                AddColorFloorCardPanel(f.Description, pnlFloorCards.Controls, f.BackColor, new Point(XX, YY));
                // increment y position
                YY += 40;
            }
            // reset x coord
            XX = 10;
            // add a totals for for floorcards
            AddColorLabel("TOTALS", pnlFloorCards.Controls, Color.Black, 220, 34, new Point(XX, YY)); XX += 230;
            AddColorFloorCardPanel("totals", pnlFloorCards.Controls, Color.Black, new Point(XX, YY));
            // get number of workers to user in switch statment - number from wizard
            numWorkers = Worker.CurrentWorkersList.Count;
            // SET WORKERS 
            switch(numWorkers)
            {
                // set the data in the form according to the number of workers and the pulltabdesk worker
                case 0:
                    lblFloor1.Text = Worker.CurrentPTDeskWorker.Name;
                    arrCbxWorkers[0].Text = Worker.CurrentPTDeskWorker.Name;
                    break;
                case 1:
                    lblFloor1.Text = Worker.CurrentWorkersList[0].Name;
                    lblFloor2.Text = Worker.CurrentPTDeskWorker.Name;
                    arrCbxWorkers[0].Text = Worker.CurrentWorkersList[0].Name;
                    arrCbxWorkers[1].Text = Worker.CurrentPTDeskWorker.Name;
                    break;
                case 2:
                    lblFloor1.Text = Worker.CurrentWorkersList[0].Name;
                    lblFloor2.Text = Worker.CurrentWorkersList[1].Name;
                    lblFloor3.Text = Worker.CurrentPTDeskWorker.Name;
                    arrCbxWorkers[0].Text = Worker.CurrentWorkersList[0].Name;
                    arrCbxWorkers[1].Text = Worker.CurrentWorkersList[1].Name;
                    arrCbxWorkers[2].Text = Worker.CurrentPTDeskWorker.Name;
                    break;
                case 3:
                    lblFloor1.Text = Worker.CurrentWorkersList[0].Name;
                    lblFloor2.Text = Worker.CurrentWorkersList[1].Name;
                    lblFloor3.Text = Worker.CurrentWorkersList[2].Name;
                    lblFloor4.Text = Worker.CurrentPTDeskWorker.Name;
                    arrCbxWorkers[0].Text = Worker.CurrentWorkersList[0].Name;
                    arrCbxWorkers[1].Text = Worker.CurrentWorkersList[1].Name;
                    arrCbxWorkers[2].Text = Worker.CurrentWorkersList[2].Name;
                    arrCbxWorkers[3].Text = Worker.CurrentPTDeskWorker.Name;
                    break;
                case 4:
                    lblFloor1.Text = Worker.CurrentWorkersList[0].Name;
                    lblFloor2.Text = Worker.CurrentWorkersList[1].Name;
                    lblFloor3.Text = Worker.CurrentWorkersList[2].Name;
                    lblFloor4.Text = Worker.CurrentWorkersList[3].Name;
                    lblFloor5.Text = Worker.CurrentPTDeskWorker.Name;
                    arrCbxWorkers[0].Text = Worker.CurrentWorkersList[0].Name;
                    arrCbxWorkers[1].Text = Worker.CurrentWorkersList[1].Name;
                    arrCbxWorkers[2].Text = Worker.CurrentWorkersList[2].Name;
                    arrCbxWorkers[3].Text = Worker.CurrentWorkersList[3].Name;
                    arrCbxWorkers[4].Text = Worker.CurrentPTDeskWorker.Name;
                    break;
                default:
                    break;
            }
            // after initialization of workers add listening events for changes

            cbxW1.Validated += new EventHandler((s, ez) => RefreshWorkers());
            cbxW2.Validated += new EventHandler((s, ez) => RefreshWorkers());
            cbxW3.Validated += new EventHandler((s, ez) => RefreshWorkers());
            cbxW4.Validated += new EventHandler((s, ez) => RefreshWorkers());
            cbxW5.Validated += new EventHandler((s, ez) => RefreshWorkers());

        }
        private void lblPTGrandTotal_TextChanged(object sender, EventArgs e)
        {
            // update reports page with total pull tab sales when the quantity changes on the pull tabs tab page
            txtPullTabs.Text = double.Parse(lblPTGrandTotal.Text).ToString("c");
            txtPullTabs.Tag = double.Parse(lblPTGrandTotal.Text).ToString();
        }
        private void lblPTTotal_TextChanged(object sender, EventArgs e)
        {
            // when the total of pull tabs changes, test for the correct number for tabs, and change the color to indicate correctness
            Label tb = sender as Label;

            string text = tb.Text;
            int length = text.Length;
            int slashIndex = text.IndexOf('/');
            string current = text.Substring(0, slashIndex);
            string gross = text.Substring(slashIndex + 1);
            if (current != gross)
                tb.BackColor = Color.LightPink;
            else
                tb.BackColor = Color.LightGreen;

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
            // close entire application from this form's close
            Application.Exit();
        }
        private void mnuFileReport_Click(object sender, EventArgs e)
        {
            // redirects user to a btnGenerate Click, creates report for session at end of data entry
            btnGenerate.PerformClick();
        }
        private void mnuInfoAbout_Click(object sender, EventArgs e)
        {
            // creates and generates a new about form
            frmAbout about = new frmAbout();
            about.ShowDialog();
        }
        private void mnuInfoHelp_Click(object sender, EventArgs e)
        {
            // creates and show a new help form
            frmHelp help = new frmHelp();
            help.ShowDialog();
        }
        private void mnuViewSecurity_Click(object sender, EventArgs e)
        {
            // method is used to text password form stuff
            frmSecurity securityForm = new frmSecurity();
            securityForm.Show();
        }
        private void nudAttendance_Enter(object sender, EventArgs e)
        {
            // method that selects all text of numberupdown when control gains focus by tabbing
            nudAttendance.Select(0, nudAttendance.Text.Length);
        }
        private void nudAttendance_MouseClick(object sender, MouseEventArgs e)
        {
            // method that selects all text of numberupdown when control is clicked
            nudAttendance.Select(0, nudAttendance.Text.Length);
        }
        private void nudAttendance_ValueChanged(object sender, EventArgs e)
        {
            // event that stores the newest entered value for attendance, numberupdown control requires no validation
            DbOps.Attendance = (int)nudAttendance.Value;
        }
        private void pbxAddPT_Click(object sender, EventArgs e)
        {
            // call another method to add one row of pulltab controls 
            AddPT();
        }
        private void pbxRemovePT_Click(object sender, EventArgs e)
        {
            // event that redirects to another method (which is also used in form load)
            // removes one row of controls for a pull tab
            RemovePT();
        }
        public void SendFocusToTxt(object sender, EventArgs e, TextBox t)
        {
            // method to trick focus from parent panel into textbox inside it
            t.Focus();
        }
        public void StoreTextInPanelTag(object sender, KeyPressEventArgs e, TextBox t, Panel p, string name)
        {
            // method used to store additional info in the panel
            p.Tag = name + "," + t.Text;
        }
        private void tabMain_KeyDown(object sender, KeyEventArgs e)
        {
            // keeps the tab page from moving around between pages with arrow keys
            if (!cbxCharities.Focused && !dgvPaperSets.Focused && !dgvElectronics.Focused && !dgvSessionBingoGames.Focused)
                e.Handled = true;
        }
        private void txtPT_Enter(object sender, EventArgs e)
        {
            // focus event that selects all the text in the current field
            TextBox tb = sender as TextBox;
            tb.SelectAll();
        }
        private void txtPT_MouseUp(object sender, MouseEventArgs e)
        {
            // mouse even that selects all the text in the current box
            TextBox tb = sender as TextBox;
            tb.SelectAll();
        }
        private void txtPT_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            // to help user navigate the pull tabs tabpage - uses arrow keys to move between fields
            int indexTarget;
            if (e.KeyCode == Keys.Right)
            {
                // on right, move to next tab index
                if (((Control)(sender)).TabIndex == 132)
                    indexTarget = 0;
                else
                    indexTarget = ((Control)(sender)).TabIndex + 1;
                foreach (Control c in pnlPullTabs.Controls)
                {
                    if (c.TabIndex == indexTarget && !c.Name.Contains("pbx"))
                        c.Focus();
                }
            }
            if (e.KeyCode == Keys.Left)
            {
                // on left, look for tab index down one
                if (((Control)(sender)).TabIndex == 0)
                    indexTarget = 132;
                else
                    indexTarget = ((Control)(sender)).TabIndex - 1;
                foreach (Control c in pnlPullTabs.Controls)
                {
                    if (c.TabIndex == indexTarget && !c.Name.Contains("pbx"))
                        c.Focus();
                }
            }
            if (e.KeyCode == Keys.Up)
            {
                // on up, look for tab index 7 less
                if (((Control)(sender)).TabIndex < 7)
                    indexTarget = ((Control)(sender)).TabIndex + 126;
                else
                    indexTarget = ((Control)(sender)).TabIndex - 7;
                foreach (Control c in pnlPullTabs.Controls)
                {
                    if (c.TabIndex == indexTarget && !c.Name.Contains("pbx"))
                        c.Focus();
                }
            }
            if (e.KeyCode == Keys.Down)
            {
                // on down, look for tab index 7 more
                if (((Control)(sender)).TabIndex > 126)
                    indexTarget = ((Control)(sender)).TabIndex - 126;
                else
                    indexTarget = ((Control)(sender)).TabIndex + 7;
                foreach (Control c in pnlPullTabs.Controls)
                {
                    if (c.TabIndex == indexTarget && !c.Name.Contains("pbx"))
                        c.Focus();
                }
            }

        }
        private void txtPT01_TextChanged(object sender, EventArgs e)
        {
            // when text changes for pull tab one, store all of the current info 
            // from the textboxes and update the current total

            tablePullTabs[0].Serial = txtPT01Serial.Text;
            if (cbxPT01Name.SelectedIndex != -1)
            {
                tablePullTabs[0].Description = cbxPT01Name.SelectedItem.ToString();
                tablePullTabs[0].Gross = DbOps.PullTabs[cbxPT01Name.SelectedIndex].Gross;
                tablePullTabs[0].Prizes = DbOps.PullTabs[cbxPT01Name.SelectedIndex].Prizes;
                tablePullTabs[0].Tax = DbOps.PullTabs[cbxPT01Name.SelectedIndex].Tax;
                tablePullTabs[0].FormID = DbOps.PullTabs[cbxPT01Name.SelectedIndex].FormID;
            }
            else
            {
                tablePullTabs[0].Description = "";
                tablePullTabs[0].Gross = 0;
                tablePullTabs[0].Prizes = 0;
                tablePullTabs[0].Tax = 0;
                tablePullTabs[0].FormID = "";
            }
            // get worker quantities
            int f1 = 0;
            int f2 = 0;
            int f3 = 0;
            int f4 = 0;
            int f5 = 0;
            // set default of 0 if necessary
            if (txtPT01Floor1.Text.Trim() == "")
                txtPT01Floor1.Text = "0";
            if (txtPT01Floor2.Text.Trim() == "")
                txtPT01Floor2.Text = "0";
            if (txtPT01Floor3.Text.Trim() == "")
                txtPT01Floor3.Text = "0";
            if (txtPT01Floor4.Text.Trim() == "")
                txtPT01Floor4.Text = "0";
            if (txtPT01Floor5.Text.Trim() == "")
                txtPT01Floor5.Text = "0";
            if (!int.TryParse(txtPT01Floor1.Text, out f1) || !int.TryParse(txtPT01Floor2.Text, out f2) ||
                !int.TryParse(txtPT01Floor3.Text, out f3) || !int.TryParse(txtPT01Floor4.Text, out f4) ||
                !int.TryParse(txtPT01Floor5.Text, out f5))
            {
                // if parsing any of the numbers fails don't set numbers into pull tab, display error ('n/a')
                // the slash works with the logic in the "text changed of label" event
                lblPT01Total.Text = "n/a";
                return;
            }
            // set quantities in pull tab array
            tablePullTabs[0].QtyWorker1 = f1;
            tablePullTabs[0].QtyWorker2 = f2;
            tablePullTabs[0].QtyWorker3 = f3;
            tablePullTabs[0].QtyWorker4 = f4;
            tablePullTabs[0].QtyWorker5 = f5;
            // get total current
            int current = f1 + f2 + f3 + f4 + f5;
            // set current 
            tablePullTabs[0].QtyAllWorkers = current;
            // update label for total
            lblPT01Total.Text = current.ToString() + "/" + tablePullTabs[0].Gross.ToString();

            RefreshPullTabTotals();
        }
        private void txtPT02_TextChanged(object sender, EventArgs e)
        {
            // when text changes for pull tab two, store all of the current info 
            // from the textboxes and update the current total
            tablePullTabs[1].Serial = txtPT02Serial.Text;
            if (cbxPT02Name.SelectedIndex != -1)
            {
                tablePullTabs[1].Description = cbxPT02Name.SelectedItem.ToString();
                tablePullTabs[1].Gross = DbOps.PullTabs[cbxPT02Name.SelectedIndex].Gross;
                tablePullTabs[1].Prizes = DbOps.PullTabs[cbxPT02Name.SelectedIndex].Prizes;
                tablePullTabs[1].Tax = DbOps.PullTabs[cbxPT02Name.SelectedIndex].Tax;
                tablePullTabs[1].FormID = DbOps.PullTabs[cbxPT02Name.SelectedIndex].FormID;
            }
            else
            {
                tablePullTabs[1].Description = "";
                tablePullTabs[1].Gross = 0;
                tablePullTabs[1].Prizes = 0;
                tablePullTabs[1].Tax = 0;
                tablePullTabs[1].FormID = "";
            }
            // get worker quantities
            int f1 = 0;
            int f2 = 0;
            int f3 = 0;
            int f4 = 0;
            int f5 = 0;
            // set default of 0 if necessary
            if (txtPT02Floor1.Text.Trim() == "")
                txtPT02Floor1.Text = "0";
            if (txtPT02Floor2.Text.Trim() == "")
                txtPT02Floor2.Text = "0";
            if (txtPT02Floor3.Text.Trim() == "")
                txtPT02Floor3.Text = "0";
            if (txtPT02Floor4.Text.Trim() == "")
                txtPT02Floor4.Text = "0";
            if (txtPT02Floor5.Text.Trim() == "")
                txtPT02Floor5.Text = "0";
            if (!int.TryParse(txtPT02Floor1.Text, out f1) || !int.TryParse(txtPT02Floor2.Text, out f2) ||
                !int.TryParse(txtPT02Floor3.Text, out f3) || !int.TryParse(txtPT02Floor4.Text, out f4) ||
                !int.TryParse(txtPT02Floor5.Text, out f5))
            {
                // if parsing any of the numbers fails don't set numbers into pull tab, display error ('n/a')
                // the slash works with the logic in the "text changed of label" event
                lblPT02Total.Text = "n/a";
                return;
            }
            // set quantities in pull tab array
            tablePullTabs[1].QtyWorker1 = f1;
            tablePullTabs[1].QtyWorker2 = f2;
            tablePullTabs[1].QtyWorker3 = f3;
            tablePullTabs[1].QtyWorker4 = f4;
            tablePullTabs[1].QtyWorker5 = f5;
            // get total current
            int current = f1 + f2 + f3 + f4 + f5;
            // set current 
            tablePullTabs[1].QtyAllWorkers = current;
            // update label for total
            lblPT02Total.Text = current.ToString() + "/" + tablePullTabs[1].Gross.ToString();// when text changes for pull tab one, store all of the current info 


            RefreshPullTabTotals();
        }
        private void txtPT03_TextChanged(object sender, EventArgs e)
        {
            // when text changes for pull tab three, store all of the current info 
            // from the textboxes and update the current total
            tablePullTabs[2].Serial = txtPT03Serial.Text;
            if (cbxPT03Name.SelectedIndex != -1)
            {
                tablePullTabs[2].Description = cbxPT03Name.SelectedItem.ToString();
                tablePullTabs[2].Gross = DbOps.PullTabs[cbxPT03Name.SelectedIndex].Gross;
                tablePullTabs[2].Prizes = DbOps.PullTabs[cbxPT03Name.SelectedIndex].Prizes;
                tablePullTabs[2].Tax = DbOps.PullTabs[cbxPT03Name.SelectedIndex].Tax;
                tablePullTabs[2].FormID = DbOps.PullTabs[cbxPT03Name.SelectedIndex].FormID;
            }                 
            else              
            {                 
                tablePullTabs[2].Description = "";
                tablePullTabs[2].Gross = 0;
                tablePullTabs[2].Prizes = 0;
                tablePullTabs[2].Tax = 0;
                tablePullTabs[2].FormID = "";
            }
            // get worker quantities
            int f1 = 0;
            int f2 = 0;
            int f3 = 0;
            int f4 = 0;
            int f5 = 0;
            // set default of 0 if necessary
            if (txtPT03Floor1.Text.Trim() == "")
                txtPT03Floor1.Text = "0";
            if (txtPT03Floor2.Text.Trim() == "")
                txtPT03Floor2.Text = "0";
            if (txtPT03Floor3.Text.Trim() == "")
                txtPT03Floor3.Text = "0";
            if (txtPT03Floor4.Text.Trim() == "")
                txtPT03Floor4.Text = "0";
            if (txtPT03Floor5.Text.Trim() == "")
                txtPT03Floor5.Text = "0";
            if (!int.TryParse(txtPT03Floor1.Text, out f1) || !int.TryParse(txtPT03Floor2.Text, out f2) ||
                !int.TryParse(txtPT03Floor3.Text, out f3) || !int.TryParse(txtPT03Floor4.Text, out f4) ||
                !int.TryParse(txtPT03Floor5.Text, out f5))
            {
                // if parsing any of the numbers fails don't set numbers into pull tab, display error ('n/a')
                // the slash works with the logic in the "text changed of label" event
                lblPT03Total.Text = "n/a";
                return;
            }

            // set quantities in pull tab array
            tablePullTabs[2].QtyWorker1 = f1;
            tablePullTabs[2].QtyWorker2 = f2;
            tablePullTabs[2].QtyWorker3 = f3;
            tablePullTabs[2].QtyWorker4 = f4;
            tablePullTabs[2].QtyWorker5 = f5;
            // get total current
            int current = f1 + f2 + f3 + f4 + f5;
            // set current 
            tablePullTabs[2].QtyAllWorkers = current;
            // update label for total
            lblPT03Total.Text = current.ToString() + "/" + tablePullTabs[2].Gross.ToString();
            RefreshPullTabTotals();
        }
        private void txtPT04_TextChanged(object sender, EventArgs e)
        {
            // when text changes for pull tab three, store all of the current info 
            // from the textboxes and update the current total
            tablePullTabs[3].Serial = txtPT04Serial.Text;
            if (cbxPT04Name.SelectedIndex != -1)
            {
                tablePullTabs[3].Description = cbxPT04Name.SelectedItem.ToString();
                tablePullTabs[3].Gross = DbOps.PullTabs[cbxPT04Name.SelectedIndex].Gross;
                tablePullTabs[3].Prizes = DbOps.PullTabs[cbxPT04Name.SelectedIndex].Prizes;
                tablePullTabs[3].Tax = DbOps.PullTabs[cbxPT04Name.SelectedIndex].Tax;
                tablePullTabs[3].FormID = DbOps.PullTabs[cbxPT04Name.SelectedIndex].FormID;
            }
            else
            {
                tablePullTabs[3].Description = "";
                tablePullTabs[3].Gross = 0;
                tablePullTabs[3].Prizes = 0;
                tablePullTabs[3].Tax = 0;
                tablePullTabs[3].FormID = "";
            }
            // get worker quantities
            int f1 = 0;
            int f2 = 0;
            int f3 = 0;
            int f4 = 0;
            int f5 = 0;
            // set default of 0 if necessary
            if (txtPT04Floor1.Text.Trim() == "")
                txtPT04Floor1.Text = "0";
            if (txtPT04Floor2.Text.Trim() == "")
                txtPT04Floor2.Text = "0";
            if (txtPT04Floor3.Text.Trim() == "")
                txtPT04Floor3.Text = "0";
            if (txtPT04Floor4.Text.Trim() == "")
                txtPT04Floor4.Text = "0";
            if (txtPT04Floor5.Text.Trim() == "")
                txtPT04Floor5.Text = "0";
            if (!int.TryParse(txtPT04Floor1.Text, out f1) || !int.TryParse(txtPT04Floor2.Text, out f2) ||
                !int.TryParse(txtPT04Floor3.Text, out f3) || !int.TryParse(txtPT04Floor4.Text, out f4) ||
                !int.TryParse(txtPT04Floor5.Text, out f5))
            {
                // if parsing any of the numbers fails don't set numbers into pull tab, display error ('n/a')
                // the slash works with the logic in the "text changed of label" event
                lblPT04Total.Text = "n/a";
                return;
            }

            // set quantities in pull tab array
            tablePullTabs[3].QtyWorker1 = f1;
            tablePullTabs[3].QtyWorker2 = f2;
            tablePullTabs[3].QtyWorker3 = f3;
            tablePullTabs[3].QtyWorker4 = f4;
            tablePullTabs[3].QtyWorker5 = f5;
            // get total current
            int current = f1 + f2 + f3 + f4 + f5;
            // set current 
            tablePullTabs[3].QtyAllWorkers = current;
            // update label for total
            lblPT04Total.Text = current.ToString() + "/" + tablePullTabs[3].Gross.ToString();
            RefreshPullTabTotals();
        }
        private void txtPT05_TextChanged(object sender, EventArgs e)
        {
            // when text changes for pull tab three, store all of the current info 
            // from the textboxes and update the current total
            tablePullTabs[4].Serial = txtPT05Serial.Text;
            if (cbxPT05Name.SelectedIndex != -1)
            {
                tablePullTabs[4].Description = cbxPT05Name.SelectedItem.ToString();
                tablePullTabs[4].Gross = DbOps.PullTabs[cbxPT05Name.SelectedIndex].Gross;
                tablePullTabs[4].Prizes = DbOps.PullTabs[cbxPT05Name.SelectedIndex].Prizes;
                tablePullTabs[4].Tax = DbOps.PullTabs[cbxPT05Name.SelectedIndex].Tax;
                tablePullTabs[4].FormID = DbOps.PullTabs[cbxPT05Name.SelectedIndex].FormID;
            }
            else
            {
                tablePullTabs[4].Description = "";
                tablePullTabs[4].Gross = 0;
                tablePullTabs[4].Prizes = 0;
                tablePullTabs[4].Tax = 0;
                tablePullTabs[4].FormID = "";
            }
            // get worker quantities
            int f1 = 0;
            int f2 = 0;
            int f3 = 0;
            int f4 = 0;
            int f5 = 0;
            // set default of 0 if necessary
            if (txtPT05Floor1.Text.Trim() == "")
                txtPT05Floor1.Text = "0";
            if (txtPT05Floor2.Text.Trim() == "")
                txtPT05Floor2.Text = "0";
            if (txtPT05Floor3.Text.Trim() == "")
                txtPT05Floor3.Text = "0";
            if (txtPT05Floor4.Text.Trim() == "")
                txtPT05Floor4.Text = "0";
            if (txtPT05Floor5.Text.Trim() == "")
                txtPT05Floor5.Text = "0";
            if (!int.TryParse(txtPT05Floor1.Text, out f1) || !int.TryParse(txtPT05Floor2.Text, out f2) ||
                !int.TryParse(txtPT05Floor3.Text, out f3) || !int.TryParse(txtPT05Floor4.Text, out f4) ||
                !int.TryParse(txtPT05Floor5.Text, out f5))
            {
                // if parsing any of the numbers fails don't set numbers into pull tab, display error ('n/a')
                // the slash works with the logic in the "text changed of label" event
                lblPT05Total.Text = "n/a";
                return;
            }

            // set quantities in pull tab array
            tablePullTabs[4].QtyWorker1 = f1;
            tablePullTabs[4].QtyWorker2 = f2;
            tablePullTabs[4].QtyWorker3 = f3;
            tablePullTabs[4].QtyWorker4 = f4;
            tablePullTabs[4].QtyWorker5 = f5;
            // get total current
            int current = f1 + f2 + f3 + f4 + f5;
            // set current 
            tablePullTabs[4].QtyAllWorkers = current;
            // update label for total
            lblPT05Total.Text = current.ToString() + "/" + tablePullTabs[4].Gross.ToString();
            RefreshPullTabTotals();
        }
        private void txtPT06_TextChanged(object sender, EventArgs e)
        {
            // when text changes for pull tab three, store all of the current info 
            // from the textboxes and update the current total
            tablePullTabs[5].Serial = txtPT06Serial.Text;
            if (cbxPT06Name.SelectedIndex != -1)
            {
                tablePullTabs[5].Description = cbxPT06Name.SelectedItem.ToString();
                tablePullTabs[5].Gross = DbOps.PullTabs[cbxPT06Name.SelectedIndex].Gross;
                tablePullTabs[5].Prizes = DbOps.PullTabs[cbxPT06Name.SelectedIndex].Prizes;
                tablePullTabs[5].Tax = DbOps.PullTabs[cbxPT06Name.SelectedIndex].Tax;
                tablePullTabs[5].FormID = DbOps.PullTabs[cbxPT06Name.SelectedIndex].FormID;
            }
            else
            {
                tablePullTabs[5].Description = "";
                tablePullTabs[5].Gross = 0;
                tablePullTabs[5].Prizes = 0;
                tablePullTabs[5].Tax = 0;
                tablePullTabs[5].FormID = "";
            }
            // get worker quantities
            int f1 = 0;
            int f2 = 0;
            int f3 = 0;
            int f4 = 0;
            int f5 = 0;
            // set default of 0 if necessary
            if (txtPT06Floor1.Text.Trim() == "")
                txtPT06Floor1.Text = "0";
            if (txtPT06Floor2.Text.Trim() == "")
                txtPT06Floor2.Text = "0";
            if (txtPT06Floor3.Text.Trim() == "")
                txtPT06Floor3.Text = "0";
            if (txtPT06Floor4.Text.Trim() == "")
                txtPT06Floor4.Text = "0";
            if (txtPT06Floor5.Text.Trim() == "")
                txtPT06Floor5.Text = "0";
            if (!int.TryParse(txtPT06Floor1.Text, out f1) || !int.TryParse(txtPT06Floor2.Text, out f2) ||
                !int.TryParse(txtPT06Floor3.Text, out f3) || !int.TryParse(txtPT06Floor4.Text, out f4) ||
                !int.TryParse(txtPT06Floor5.Text, out f5))
            {
                // if parsing any of the numbers fails don't set numbers into pull tab, display error ('n/a')
                // the slash works with the logic in the "text changed of label" event
                lblPT06Total.Text = "n/a";
                return;
            }

            // set quantities in pull tab array
            tablePullTabs[5].QtyWorker1 = f1;
            tablePullTabs[5].QtyWorker2 = f2;
            tablePullTabs[5].QtyWorker3 = f3;
            tablePullTabs[5].QtyWorker4 = f4;
            tablePullTabs[5].QtyWorker5 = f5;
            // get total current
            int current = f1 + f2 + f3 + f4 + f5;
            // set current 
            tablePullTabs[5].QtyAllWorkers = current;
            // update label for total
            lblPT06Total.Text = current.ToString() + "/" + tablePullTabs[5].Gross.ToString();
            RefreshPullTabTotals();
        }
        private void txtPT07_TextChanged(object sender, EventArgs e)
        {
            // when text changes for pull tab three, store all of the current info 
            // from the textboxes and update the current total
            tablePullTabs[6].Serial = txtPT07Serial.Text;
            if (cbxPT07Name.SelectedIndex != -1)
            {
                tablePullTabs[6].Description = cbxPT07Name.SelectedItem.ToString();
                tablePullTabs[6].Gross = DbOps.PullTabs[cbxPT07Name.SelectedIndex].Gross;
                tablePullTabs[6].Prizes = DbOps.PullTabs[cbxPT07Name.SelectedIndex].Prizes;
                tablePullTabs[6].Tax = DbOps.PullTabs[cbxPT07Name.SelectedIndex].Tax;
                tablePullTabs[6].FormID = DbOps.PullTabs[cbxPT07Name.SelectedIndex].FormID;
            }
            else
            {
                tablePullTabs[6].Description = "";
                tablePullTabs[6].Gross = 0;
                tablePullTabs[6].Prizes = 0;
                tablePullTabs[6].Tax = 0;
                tablePullTabs[6].FormID = "";
            }
            // get worker quantities
            int f1 = 0;
            int f2 = 0;
            int f3 = 0;
            int f4 = 0;
            int f5 = 0;
            // set default of 0 if necessary
            if (txtPT07Floor1.Text.Trim() == "")
                txtPT07Floor1.Text = "0";
            if (txtPT07Floor2.Text.Trim() == "")
                txtPT07Floor2.Text = "0";
            if (txtPT07Floor3.Text.Trim() == "")
                txtPT07Floor3.Text = "0";
            if (txtPT07Floor4.Text.Trim() == "")
                txtPT07Floor4.Text = "0";
            if (txtPT07Floor5.Text.Trim() == "")
                txtPT07Floor5.Text = "0";
            if (!int.TryParse(txtPT07Floor1.Text, out f1) || !int.TryParse(txtPT07Floor2.Text, out f2) ||
                !int.TryParse(txtPT07Floor3.Text, out f3) || !int.TryParse(txtPT07Floor4.Text, out f4) ||
                !int.TryParse(txtPT07Floor5.Text, out f5))
            {
                // if parsing any of the numbers fails don't set numbers into pull tab, display error ('n/a')
                // the slash works with the logic in the "text changed of label" event
                lblPT07Total.Text = "n/a";
                return;
            }

            // set quantities in pull tab array
            tablePullTabs[6].QtyWorker1 = f1;
            tablePullTabs[6].QtyWorker2 = f2;
            tablePullTabs[6].QtyWorker3 = f3;
            tablePullTabs[6].QtyWorker4 = f4;
            tablePullTabs[6].QtyWorker5 = f5;
            // get total current
            int current = f1 + f2 + f3 + f4 + f5;
            // set current 
            tablePullTabs[6].QtyAllWorkers = current;
            // update label for total
            lblPT07Total.Text = current.ToString() + "/" + tablePullTabs[6].Gross.ToString();
            RefreshPullTabTotals();
        }
        private void txtPT08_TextChanged(object sender, EventArgs e)
        {
            // when text changes for pull tab three, store all of the current info 
            // from the textboxes and update the current total
            tablePullTabs[7].Serial = txtPT08Serial.Text;
            if (cbxPT08Name.SelectedIndex != -1)
            {
                tablePullTabs[7].Description = cbxPT08Name.SelectedItem.ToString();
                tablePullTabs[7].Gross = DbOps.PullTabs[cbxPT08Name.SelectedIndex].Gross;
                tablePullTabs[7].Prizes = DbOps.PullTabs[cbxPT08Name.SelectedIndex].Prizes;
                tablePullTabs[7].Tax = DbOps.PullTabs[cbxPT08Name.SelectedIndex].Tax;
                tablePullTabs[7].FormID = DbOps.PullTabs[cbxPT08Name.SelectedIndex].FormID;
            }
            else
            {
                tablePullTabs[7].Description = "";
                tablePullTabs[7].Gross = 0;
                tablePullTabs[7].Prizes = 0;
                tablePullTabs[7].Tax = 0;
                tablePullTabs[7].FormID = "";
            }
            // get worker quantities
            int f1 = 0;
            int f2 = 0;
            int f3 = 0;
            int f4 = 0;
            int f5 = 0;
            // set default of 0 if necessary
            if (txtPT08Floor1.Text.Trim() == "")
                txtPT08Floor1.Text = "0";
            if (txtPT08Floor2.Text.Trim() == "")
                txtPT08Floor2.Text = "0";
            if (txtPT08Floor3.Text.Trim() == "")
                txtPT08Floor3.Text = "0";
            if (txtPT08Floor4.Text.Trim() == "")
                txtPT08Floor4.Text = "0";
            if (txtPT08Floor5.Text.Trim() == "")
                txtPT08Floor5.Text = "0";
            if (!int.TryParse(txtPT08Floor1.Text, out f1) || !int.TryParse(txtPT08Floor2.Text, out f2) ||
                !int.TryParse(txtPT08Floor3.Text, out f3) || !int.TryParse(txtPT08Floor4.Text, out f4) ||
                !int.TryParse(txtPT08Floor5.Text, out f5))
            {
                // if parsing any of the numbers fails don't set numbers into pull tab, display error ('n/a')
                // the slash works with the logic in the "text changed of label" event
                lblPT08Total.Text = "n/a";
                return;
            }

            // set quantities in pull tab array
            tablePullTabs[7].QtyWorker1 = f1;
            tablePullTabs[7].QtyWorker2 = f2;
            tablePullTabs[7].QtyWorker3 = f3;
            tablePullTabs[7].QtyWorker4 = f4;
            tablePullTabs[7].QtyWorker5 = f5;
            // get total current
            int current = f1 + f2 + f3 + f4 + f5;
            // set current 
            tablePullTabs[7].QtyAllWorkers = current;
            // update label for total
            lblPT08Total.Text = current.ToString() + "/" + tablePullTabs[7].Gross.ToString();
            RefreshPullTabTotals();
        }
        private void txtPT09_TextChanged(object sender, EventArgs e)
        {
            // when text changes for pull tab three, store all of the current info 
            // from the textboxes and update the current total
            tablePullTabs[8].Serial = txtPT09Serial.Text;
            if (cbxPT09Name.SelectedIndex != -1)
            {
                tablePullTabs[8].Description = cbxPT09Name.SelectedItem.ToString();
                tablePullTabs[8].Gross = DbOps.PullTabs[cbxPT09Name.SelectedIndex].Gross;
                tablePullTabs[8].Prizes = DbOps.PullTabs[cbxPT09Name.SelectedIndex].Prizes;
                tablePullTabs[8].Tax = DbOps.PullTabs[cbxPT09Name.SelectedIndex].Tax;
                tablePullTabs[8].FormID = DbOps.PullTabs[cbxPT09Name.SelectedIndex].FormID;
            }
            else
            {
                tablePullTabs[8].Description = "";
                tablePullTabs[8].Gross = 0;
                tablePullTabs[8].Prizes = 0;
                tablePullTabs[8].Tax = 0;
                tablePullTabs[8].FormID = "";
            }
            // get worker quantities
            int f1 = 0;
            int f2 = 0;
            int f3 = 0;
            int f4 = 0;
            int f5 = 0;
            // set default of 0 if necessary
            if (txtPT09Floor1.Text.Trim() == "")
                txtPT09Floor1.Text = "0";
            if (txtPT09Floor2.Text.Trim() == "")
                txtPT09Floor2.Text = "0";
            if (txtPT09Floor3.Text.Trim() == "")
                txtPT09Floor3.Text = "0";
            if (txtPT09Floor4.Text.Trim() == "")
                txtPT09Floor4.Text = "0";
            if (txtPT09Floor5.Text.Trim() == "")
                txtPT09Floor5.Text = "0";
            if (!int.TryParse(txtPT09Floor1.Text, out f1) || !int.TryParse(txtPT09Floor2.Text, out f2) ||
                !int.TryParse(txtPT09Floor3.Text, out f3) || !int.TryParse(txtPT09Floor4.Text, out f4) ||
                !int.TryParse(txtPT09Floor5.Text, out f5))
            {
                // if parsing any of the numbers fails don't set numbers into pull tab, display error ('n/a')
                // the slash works with the logic in the "text changed of label" event
                lblPT09Total.Text = "n/a";
                return;
            }

            // set quantities in pull tab array
            tablePullTabs[8].QtyWorker1 = f1;
            tablePullTabs[8].QtyWorker2 = f2;
            tablePullTabs[8].QtyWorker3 = f3;
            tablePullTabs[8].QtyWorker4 = f4;
            tablePullTabs[8].QtyWorker5 = f5;
            // get total current
            int current = f1 + f2 + f3 + f4 + f5;
            // set current 
            tablePullTabs[8].QtyAllWorkers = current;
            // update label for total
            lblPT09Total.Text = current.ToString() + "/" + tablePullTabs[8].Gross.ToString();
            RefreshPullTabTotals();
        }
        private void txtPT10_TextChanged(object sender, EventArgs e)
        {
            // when text changes for pull tab three, store all of the current info 
            // from the textboxes and update the current total
            tablePullTabs[9].Serial = txtPT10Serial.Text;
            if (cbxPT10Name.SelectedIndex != -1)
            {
                tablePullTabs[9].Description = cbxPT10Name.SelectedItem.ToString();
                tablePullTabs[9].Gross = DbOps.PullTabs[cbxPT10Name.SelectedIndex].Gross;
                tablePullTabs[9].Prizes = DbOps.PullTabs[cbxPT10Name.SelectedIndex].Prizes;
                tablePullTabs[9].Tax = DbOps.PullTabs[cbxPT10Name.SelectedIndex].Tax;
                tablePullTabs[9].FormID = DbOps.PullTabs[cbxPT10Name.SelectedIndex].FormID;
            }
            else
            {
                tablePullTabs[9].Description = "";
                tablePullTabs[9].Gross = 0;
                tablePullTabs[9].Prizes = 0;
                tablePullTabs[9].Tax = 0;
                tablePullTabs[9].FormID = "";
            }
            // get worker quantities
            int f1 = 0;
            int f2 = 0;
            int f3 = 0;
            int f4 = 0;
            int f5 = 0;
            // set default of 0 if necessary
            if (txtPT10Floor1.Text.Trim() == "")
                txtPT10Floor1.Text = "0";
            if (txtPT10Floor2.Text.Trim() == "")
                txtPT10Floor2.Text = "0";
            if (txtPT10Floor3.Text.Trim() == "")
                txtPT10Floor3.Text = "0";
            if (txtPT10Floor4.Text.Trim() == "")
                txtPT10Floor4.Text = "0";
            if (txtPT10Floor5.Text.Trim() == "")
                txtPT10Floor5.Text = "0";
            if (!int.TryParse(txtPT10Floor1.Text, out f1) || !int.TryParse(txtPT10Floor2.Text, out f2) ||
                !int.TryParse(txtPT10Floor3.Text, out f3) || !int.TryParse(txtPT10Floor4.Text, out f4) ||
                !int.TryParse(txtPT10Floor5.Text, out f5))
            {
                // if parsing any of the numbers fails don't set numbers into pull tab, display error ('n/a')
                // the slash works with the logic in the "text changed of label" event
                lblPT10Total.Text = "n/a";
                return;
            }

            // set quantities in pull tab array
            tablePullTabs[9].QtyWorker1 = f1;
            tablePullTabs[9].QtyWorker2 = f2;
            tablePullTabs[9].QtyWorker3 = f3;
            tablePullTabs[9].QtyWorker4 = f4;
            tablePullTabs[9].QtyWorker5 = f5;
            // get total current
            int current = f1 + f2 + f3 + f4 + f5;
            // set current 
            tablePullTabs[9].QtyAllWorkers = current;
            // update label for total
            lblPT10Total.Text = current.ToString() + "/" + tablePullTabs[9].Gross.ToString();
            RefreshPullTabTotals();
        }
        private void txtPT11_TextChanged(object sender, EventArgs e)
        {
            // when text changes for pull tab three, store all of the current info 
            // from the textboxes and update the current total
            tablePullTabs[10].Serial = txtPT11Serial.Text;
            if (cbxPT11Name.SelectedIndex != -1)
            {
                tablePullTabs[10].Description = cbxPT11Name.SelectedItem.ToString();
                tablePullTabs[10].Gross = DbOps.PullTabs[cbxPT11Name.SelectedIndex].Gross;
                tablePullTabs[10].Prizes = DbOps.PullTabs[cbxPT11Name.SelectedIndex].Prizes;
                tablePullTabs[10].Tax = DbOps.PullTabs[cbxPT11Name.SelectedIndex].Tax;
                tablePullTabs[10].FormID = DbOps.PullTabs[cbxPT11Name.SelectedIndex].FormID;
            }
            else
            {
                tablePullTabs[10].Description = "";
                tablePullTabs[10].Gross = 0;
                tablePullTabs[10].Prizes = 0;
                tablePullTabs[10].Tax = 0;
                tablePullTabs[10].FormID = "";
            }
            // get worker quantities
            int f1 = 0;
            int f2 = 0;
            int f3 = 0;
            int f4 = 0;
            int f5 = 0;
            // set default of 0 if necessary
            if (txtPT11Floor1.Text.Trim() == "")
                txtPT11Floor1.Text = "0";
            if (txtPT11Floor2.Text.Trim() == "")
                txtPT11Floor2.Text = "0";
            if (txtPT11Floor3.Text.Trim() == "")
                txtPT11Floor3.Text = "0";
            if (txtPT11Floor4.Text.Trim() == "")
                txtPT11Floor4.Text = "0";
            if (txtPT11Floor5.Text.Trim() == "")
                txtPT11Floor5.Text = "0";
            if (!int.TryParse(txtPT11Floor1.Text, out f1) || !int.TryParse(txtPT11Floor2.Text, out f2) ||
                !int.TryParse(txtPT11Floor3.Text, out f3) || !int.TryParse(txtPT11Floor4.Text, out f4) ||
                !int.TryParse(txtPT11Floor5.Text, out f5))
            {
                // if parsing any of the numbers fails don't set numbers into pull tab, display error ('n/a')
                // the slash works with the logic in the "text changed of label" event
                lblPT11Total.Text = "n/a";
                return;
            }

            // set quantities in pull tab array
            tablePullTabs[10].QtyWorker1 = f1;
            tablePullTabs[10].QtyWorker2 = f2;
            tablePullTabs[10].QtyWorker3 = f3;
            tablePullTabs[10].QtyWorker4 = f4;
            tablePullTabs[10].QtyWorker5 = f5;
            // get total current
            int current = f1 + f2 + f3 + f4 + f5;
            // set current 
            tablePullTabs[10].QtyAllWorkers = current;
            // update label for total
            lblPT11Total.Text = current.ToString() + "/" + tablePullTabs[10].Gross.ToString();
            RefreshPullTabTotals();
        }
        private void txtPT12_TextChanged(object sender, EventArgs e)
        {
            // when text changes for pull tab three, store all of the current info 
            // from the textboxes and update the current total
            tablePullTabs[11].Serial = txtPT12Serial.Text;
            if (cbxPT12Name.SelectedIndex != -1)
            {
                tablePullTabs[11].Description = cbxPT12Name.SelectedItem.ToString();
                tablePullTabs[11].Gross = DbOps.PullTabs[cbxPT12Name.SelectedIndex].Gross;
                tablePullTabs[11].Prizes = DbOps.PullTabs[cbxPT12Name.SelectedIndex].Prizes;
                tablePullTabs[11].Tax = DbOps.PullTabs[cbxPT12Name.SelectedIndex].Tax;
                tablePullTabs[11].FormID = DbOps.PullTabs[cbxPT12Name.SelectedIndex].FormID;
            }
            else
            {
                tablePullTabs[11].Description = "";
                tablePullTabs[11].Gross = 0;
                tablePullTabs[11].Prizes = 0;
                tablePullTabs[11].Tax = 0;
                tablePullTabs[11].FormID = "";
            }
            // get worker quantities
            int f1 = 0;
            int f2 = 0;
            int f3 = 0;
            int f4 = 0;
            int f5 = 0;
            // set default of 0 if necessary
            if (txtPT12Floor1.Text.Trim() == "")
                txtPT12Floor1.Text = "0";
            if (txtPT12Floor2.Text.Trim() == "")
                txtPT12Floor2.Text = "0";
            if (txtPT12Floor3.Text.Trim() == "")
                txtPT12Floor3.Text = "0";
            if (txtPT12Floor4.Text.Trim() == "")
                txtPT12Floor4.Text = "0";
            if (txtPT12Floor5.Text.Trim() == "")
                txtPT12Floor5.Text = "0";
            if (!int.TryParse(txtPT12Floor1.Text, out f1) || !int.TryParse(txtPT12Floor2.Text, out f2) ||
                !int.TryParse(txtPT12Floor3.Text, out f3) || !int.TryParse(txtPT12Floor4.Text, out f4) ||
                !int.TryParse(txtPT12Floor5.Text, out f5))
            {
                // if parsing any of the numbers fails don't set numbers into pull tab, display error ('n/a')
                // the slash works with the logic in the "text changed of label" event
                lblPT12Total.Text = "n/a";
                return;
            }

            // set quantities in pull tab array
            tablePullTabs[11].QtyWorker1 = f1;
            tablePullTabs[11].QtyWorker2 = f2;
            tablePullTabs[11].QtyWorker3 = f3;
            tablePullTabs[11].QtyWorker4 = f4;
            tablePullTabs[11].QtyWorker5 = f5;
            // get total current
            int current = f1 + f2 + f3 + f4 + f5;
            // set current 
            tablePullTabs[11].QtyAllWorkers = current;
            // update label for total
            lblPT12Total.Text = current.ToString() + "/" + tablePullTabs[11].Gross.ToString();
            RefreshPullTabTotals();
        }
        private void txtPT13_TextChanged(object sender, EventArgs e)
        {
            // when text changes for pull tab three, store all of the current info 
            // from the textboxes and update the current total
            tablePullTabs[12].Serial = txtPT13Serial.Text;
            if (cbxPT13Name.SelectedIndex != -1)
            {
                tablePullTabs[12].Description = cbxPT13Name.SelectedItem.ToString();
                tablePullTabs[12].Gross = DbOps.PullTabs[cbxPT13Name.SelectedIndex].Gross;
                tablePullTabs[12].Prizes = DbOps.PullTabs[cbxPT13Name.SelectedIndex].Prizes;
                tablePullTabs[12].Tax = DbOps.PullTabs[cbxPT13Name.SelectedIndex].Tax;
                tablePullTabs[12].FormID = DbOps.PullTabs[cbxPT13Name.SelectedIndex].FormID;
            }
            else
            {
                tablePullTabs[12].Description = "";
                tablePullTabs[12].Gross = 0;
                tablePullTabs[12].Prizes = 0;
                tablePullTabs[12].Tax = 0;
                tablePullTabs[12].FormID = "";
            }
            // get worker quantities
            int f1 = 0;
            int f2 = 0;
            int f3 = 0;
            int f4 = 0;
            int f5 = 0;
            // set default of 0 if necessary
            if (txtPT13Floor1.Text.Trim() == "")
                txtPT13Floor1.Text = "0";
            if (txtPT13Floor2.Text.Trim() == "")
                txtPT13Floor2.Text = "0";
            if (txtPT13Floor3.Text.Trim() == "")
                txtPT13Floor3.Text = "0";
            if (txtPT13Floor4.Text.Trim() == "")
                txtPT13Floor4.Text = "0";
            if (txtPT13Floor5.Text.Trim() == "")
                txtPT13Floor5.Text = "0";
            if (!int.TryParse(txtPT13Floor1.Text, out f1) || !int.TryParse(txtPT13Floor2.Text, out f2) ||
                !int.TryParse(txtPT13Floor3.Text, out f3) || !int.TryParse(txtPT13Floor4.Text, out f4) ||
                !int.TryParse(txtPT13Floor5.Text, out f5))
            {
                // if parsing any of the numbers fails don't set numbers into pull tab, display error ('n/a')
                // the slash works with the logic in the "text changed of label" event
                lblPT13Total.Text = "n/a";
                return;
            }

            // set quantities in pull tab array
            tablePullTabs[12].QtyWorker1 = f1;
            tablePullTabs[12].QtyWorker2 = f2;
            tablePullTabs[12].QtyWorker3 = f3;
            tablePullTabs[12].QtyWorker4 = f4;
            tablePullTabs[12].QtyWorker5 = f5;
            // get total current
            int current = f1 + f2 + f3 + f4 + f5;
            // set current 
            tablePullTabs[12].QtyAllWorkers = current;
            // update label for total
            lblPT13Total.Text = current.ToString() + "/" + tablePullTabs[12].Gross.ToString();
            RefreshPullTabTotals();
        }
        private void txtPT14_TextChanged(object sender, EventArgs e)
        {
            // when text changes for pull tab three, store all of the current info 
            // from the textboxes and update the current total
            tablePullTabs[13].Serial = txtPT14Serial.Text;
            if (cbxPT14Name.SelectedIndex != -1)
            {
                tablePullTabs[13].Description = cbxPT14Name.SelectedItem.ToString();
                tablePullTabs[13].Gross = DbOps.PullTabs[cbxPT14Name.SelectedIndex].Gross;
                tablePullTabs[13].Prizes = DbOps.PullTabs[cbxPT14Name.SelectedIndex].Prizes;
                tablePullTabs[13].Tax = DbOps.PullTabs[cbxPT14Name.SelectedIndex].Tax;
                tablePullTabs[13].FormID = DbOps.PullTabs[cbxPT14Name.SelectedIndex].FormID;
            }
            else
            {
                tablePullTabs[13].Description = "";
                tablePullTabs[13].Gross = 0;
                tablePullTabs[13].Prizes = 0;
                tablePullTabs[13].Tax = 0;
                tablePullTabs[13].FormID = "";
            }
            // get worker quantities
            int f1 = 0;
            int f2 = 0;
            int f3 = 0;
            int f4 = 0;
            int f5 = 0;
            // set default of 0 if necessary
            if (txtPT14Floor1.Text.Trim() == "")
                txtPT14Floor1.Text = "0";
            if (txtPT14Floor2.Text.Trim() == "")
                txtPT14Floor2.Text = "0";
            if (txtPT14Floor3.Text.Trim() == "")
                txtPT14Floor3.Text = "0";
            if (txtPT14Floor4.Text.Trim() == "")
                txtPT14Floor4.Text = "0";
            if (txtPT14Floor5.Text.Trim() == "")
                txtPT14Floor5.Text = "0";
            if (!int.TryParse(txtPT14Floor1.Text, out f1) || !int.TryParse(txtPT14Floor2.Text, out f2) ||
                !int.TryParse(txtPT14Floor3.Text, out f3) || !int.TryParse(txtPT14Floor4.Text, out f4) ||
                !int.TryParse(txtPT14Floor5.Text, out f5))
            {
                // if parsing any of the numbers fails don't set numbers into pull tab, display error ('n/a')
                // the slash works with the logic in the "text changed of label" event
                lblPT14Total.Text = "n/a";
                return;
            }

            // set quantities in pull tab array
            tablePullTabs[13].QtyWorker1 = f1;
            tablePullTabs[13].QtyWorker2 = f2;
            tablePullTabs[13].QtyWorker3 = f3;
            tablePullTabs[13].QtyWorker4 = f4;
            tablePullTabs[13].QtyWorker5 = f5;
            // get total current
            int current = f1 + f2 + f3 + f4 + f5;
            // set current 
            tablePullTabs[13].QtyAllWorkers = current;
            // update label for total
            lblPT14Total.Text = current.ToString() + "/" + tablePullTabs[13].Gross.ToString();
            RefreshPullTabTotals();
        }
        private void txtPT15_TextChanged(object sender, EventArgs e)
        {
            // when text changes for pull tab three, store all of the current info 
            // from the textboxes and update the current total
            tablePullTabs[14].Serial = txtPT15Serial.Text;
            if (cbxPT15Name.SelectedIndex != -1)
            {
                tablePullTabs[14].Description = cbxPT15Name.SelectedItem.ToString();
                tablePullTabs[14].Gross = DbOps.PullTabs[cbxPT15Name.SelectedIndex].Gross;
                tablePullTabs[14].Prizes = DbOps.PullTabs[cbxPT15Name.SelectedIndex].Prizes;
                tablePullTabs[14].Tax = DbOps.PullTabs[cbxPT15Name.SelectedIndex].Tax;
                tablePullTabs[14].FormID = DbOps.PullTabs[cbxPT15Name.SelectedIndex].FormID;
            }
            else
            {
                tablePullTabs[14].Description = "";
                tablePullTabs[14].Gross = 0;
                tablePullTabs[14].Prizes = 0;
                tablePullTabs[14].Tax = 0;
                tablePullTabs[14].FormID = "";
            }
            // get worker quantities
            int f1 = 0;
            int f2 = 0;
            int f3 = 0;
            int f4 = 0;
            int f5 = 0;
            // set default of 0 if necessary
            if (txtPT15Floor1.Text.Trim() == "")
                txtPT15Floor1.Text = "0";
            if (txtPT15Floor2.Text.Trim() == "")
                txtPT15Floor2.Text = "0";
            if (txtPT15Floor3.Text.Trim() == "")
                txtPT15Floor3.Text = "0";
            if (txtPT15Floor4.Text.Trim() == "")
                txtPT15Floor4.Text = "0";
            if (txtPT15Floor5.Text.Trim() == "")
                txtPT15Floor5.Text = "0";
            if (!int.TryParse(txtPT15Floor1.Text, out f1) || !int.TryParse(txtPT15Floor2.Text, out f2) ||
                !int.TryParse(txtPT15Floor3.Text, out f3) || !int.TryParse(txtPT15Floor4.Text, out f4) ||
                !int.TryParse(txtPT15Floor5.Text, out f5))
            {
                // if parsing any of the numbers fails don't set numbers into pull tab, display error ('n/a')
                // the slash works with the logic in the "text changed of label" event
                lblPT15Total.Text = "n/a";
                return;
            }

            // set quantities in pull tab array
            tablePullTabs[14].QtyWorker1 = f1;
            tablePullTabs[14].QtyWorker2 = f2;
            tablePullTabs[14].QtyWorker3 = f3;
            tablePullTabs[14].QtyWorker4 = f4;
            tablePullTabs[14].QtyWorker5 = f5;
            // get total current
            int current = f1 + f2 + f3 + f4 + f5;
            // set current 
            tablePullTabs[14].QtyAllWorkers = current;
            // update label for total
            lblPT15Total.Text = current.ToString() + "/" + tablePullTabs[14].Gross.ToString();
            RefreshPullTabTotals();
        }
        private void txtPT16_TextChanged(object sender, EventArgs e)
        {
            // when text changes for pull tab three, store all of the current info 
            // from the textboxes and update the current total
            tablePullTabs[15].Serial = txtPT16Serial.Text;
            if (cbxPT16Name.SelectedIndex != -1)
            {
                tablePullTabs[15].Description = cbxPT16Name.SelectedItem.ToString();
                tablePullTabs[15].Gross = DbOps.PullTabs[cbxPT16Name.SelectedIndex].Gross;
                tablePullTabs[15].Prizes = DbOps.PullTabs[cbxPT16Name.SelectedIndex].Prizes;
                tablePullTabs[15].Tax = DbOps.PullTabs[cbxPT16Name.SelectedIndex].Tax;
                tablePullTabs[15].FormID = DbOps.PullTabs[cbxPT16Name.SelectedIndex].FormID;
            }
            else
            {
                tablePullTabs[15].Description = "";
                tablePullTabs[15].Gross = 0;
                tablePullTabs[15].Prizes = 0;
                tablePullTabs[15].Tax = 0;
                tablePullTabs[15].FormID = "";
            }
            // get worker quantities
            int f1 = 0;
            int f2 = 0;
            int f3 = 0;
            int f4 = 0;
            int f5 = 0;
            // set default of 0 if necessary
            if (txtPT16Floor1.Text.Trim() == "")
                txtPT16Floor1.Text = "0";
            if (txtPT16Floor2.Text.Trim() == "")
                txtPT16Floor2.Text = "0";
            if (txtPT16Floor3.Text.Trim() == "")
                txtPT16Floor3.Text = "0";
            if (txtPT16Floor4.Text.Trim() == "")
                txtPT16Floor4.Text = "0";
            if (txtPT16Floor5.Text.Trim() == "")
                txtPT16Floor5.Text = "0";
            if (!int.TryParse(txtPT16Floor1.Text, out f1) || !int.TryParse(txtPT16Floor2.Text, out f2) ||
                !int.TryParse(txtPT16Floor3.Text, out f3) || !int.TryParse(txtPT16Floor4.Text, out f4) ||
                !int.TryParse(txtPT16Floor5.Text, out f5))
            {
                // if parsing any of the numbers fails don't set numbers into pull tab, display error ('n/a')
                // the slash works with the logic in the "text changed of label" event
                lblPT16Total.Text = "n/a";
                return;
            }

            // set quantities in pull tab array
            tablePullTabs[15].QtyWorker1 = f1;
            tablePullTabs[15].QtyWorker2 = f2;
            tablePullTabs[15].QtyWorker3 = f3;
            tablePullTabs[15].QtyWorker4 = f4;
            tablePullTabs[15].QtyWorker5 = f5;
            // get total current
            int current = f1 + f2 + f3 + f4 + f5;
            // set current 
            tablePullTabs[15].QtyAllWorkers = current;
            // update label for total
            lblPT16Total.Text = current.ToString() + "/" + tablePullTabs[15].Gross.ToString();
            RefreshPullTabTotals();
        }
        private void txtPT17_TextChanged(object sender, EventArgs e)
        {
            // when text changes for pull tab three, store all of the current info 
            // from the textboxes and update the current total
            tablePullTabs[16].Serial = txtPT17Serial.Text;
            if (cbxPT17Name.SelectedIndex != -1)
            {
                tablePullTabs[16].Description = cbxPT17Name.SelectedItem.ToString();
                tablePullTabs[16].Gross = DbOps.PullTabs[cbxPT17Name.SelectedIndex].Gross;
                tablePullTabs[16].Prizes = DbOps.PullTabs[cbxPT17Name.SelectedIndex].Prizes;
                tablePullTabs[16].Tax = DbOps.PullTabs[cbxPT17Name.SelectedIndex].Tax;
                tablePullTabs[16].FormID = DbOps.PullTabs[cbxPT17Name.SelectedIndex].FormID;
            }
            else
            {
                tablePullTabs[16].Description = "";
                tablePullTabs[16].Gross = 0;
                tablePullTabs[16].Prizes = 0;
                tablePullTabs[16].Tax = 0;
                tablePullTabs[16].FormID = "";
            }
            // get worker quantities
            int f1 = 0;
            int f2 = 0;
            int f3 = 0;
            int f4 = 0;
            int f5 = 0;
            // set default of 0 if necessary
            if (txtPT17Floor1.Text.Trim() == "")
                txtPT17Floor1.Text = "0";
            if (txtPT17Floor2.Text.Trim() == "")
                txtPT17Floor2.Text = "0";
            if (txtPT17Floor3.Text.Trim() == "")
                txtPT17Floor3.Text = "0";
            if (txtPT17Floor4.Text.Trim() == "")
                txtPT17Floor4.Text = "0";
            if (txtPT17Floor5.Text.Trim() == "")
                txtPT17Floor5.Text = "0";
            if (!int.TryParse(txtPT17Floor1.Text, out f1) || !int.TryParse(txtPT17Floor2.Text, out f2) ||
                !int.TryParse(txtPT17Floor3.Text, out f3) || !int.TryParse(txtPT17Floor4.Text, out f4) ||
                !int.TryParse(txtPT17Floor5.Text, out f5))
            {
                // if parsing any of the numbers fails don't set numbers into pull tab, display error ('n/a')
                // the slash works with the logic in the "text changed of label" event
                lblPT17Total.Text = "n/a";
                return;
            }

            // set quantities in pull tab array
            tablePullTabs[16].QtyWorker1 = f1;
            tablePullTabs[16].QtyWorker2 = f2;
            tablePullTabs[16].QtyWorker3 = f3;
            tablePullTabs[16].QtyWorker4 = f4;
            tablePullTabs[16].QtyWorker5 = f5;
            // get total current
            int current = f1 + f2 + f3 + f4 + f5;
            // set current 
            tablePullTabs[16].QtyAllWorkers = current;
            // update label for total
            lblPT17Total.Text = current.ToString() + "/" + tablePullTabs[16].Gross.ToString();
            RefreshPullTabTotals();
        }
        private void txtPT18_TextChanged(object sender, EventArgs e)
        {
            // when text changes for pull tab three, store all of the current info 
            // from the textboxes and update the current total
            tablePullTabs[17].Serial = txtPT18Serial.Text;
            if (cbxPT18Name.SelectedIndex != -1)
            {
                tablePullTabs[17].Description = cbxPT18Name.SelectedItem.ToString();
                tablePullTabs[17].Gross = DbOps.PullTabs[cbxPT18Name.SelectedIndex].Gross;
                tablePullTabs[17].Prizes = DbOps.PullTabs[cbxPT18Name.SelectedIndex].Prizes;
                tablePullTabs[17].Tax = DbOps.PullTabs[cbxPT18Name.SelectedIndex].Tax;
                tablePullTabs[17].FormID = DbOps.PullTabs[cbxPT18Name.SelectedIndex].FormID;
            }
            else
            {
                tablePullTabs[17].Description = "";
                tablePullTabs[17].Gross = 0;
                tablePullTabs[17].Prizes = 0;
                tablePullTabs[17].Tax = 0;
                tablePullTabs[17].FormID = "";
            }
            // get worker quantities
            int f1 = 0;
            int f2 = 0;
            int f3 = 0;
            int f4 = 0;
            int f5 = 0;
            // set default of 0 if necessary
            if (txtPT18Floor1.Text.Trim() == "")
                txtPT18Floor1.Text = "0";
            if (txtPT18Floor2.Text.Trim() == "")
                txtPT18Floor2.Text = "0";
            if (txtPT18Floor3.Text.Trim() == "")
                txtPT18Floor3.Text = "0";
            if (txtPT18Floor4.Text.Trim() == "")
                txtPT18Floor4.Text = "0";
            if (txtPT18Floor5.Text.Trim() == "")
                txtPT18Floor5.Text = "0";
            if (!int.TryParse(txtPT18Floor1.Text, out f1) || !int.TryParse(txtPT18Floor2.Text, out f2) ||
                !int.TryParse(txtPT18Floor3.Text, out f3) || !int.TryParse(txtPT18Floor4.Text, out f4) ||
                !int.TryParse(txtPT18Floor5.Text, out f5))
            {
                // if parsing any of the numbers fails don't set numbers into pull tab, display error ('n/a')
                // the slash works with the logic in the "text changed of label" event
                lblPT18Total.Text = "n/a";
                return;
            }

            // set quantities in pull tab array
            tablePullTabs[17].QtyWorker1 = f1;
            tablePullTabs[17].QtyWorker2 = f2;
            tablePullTabs[17].QtyWorker3 = f3;
            tablePullTabs[17].QtyWorker4 = f4;
            tablePullTabs[17].QtyWorker5 = f5;
            // get total current
            int current = f1 + f2 + f3 + f4 + f5;
            // set current 
            tablePullTabs[17].QtyAllWorkers = current;
            // update label for total
            lblPT18Total.Text = current.ToString() + "/" + tablePullTabs[17].Gross.ToString();
            RefreshPullTabTotals();
        }
        private void txtPT19_TextChanged(object sender, EventArgs e)
        {
            // when text changes for pull tab three, store all of the current info 
            // from the textboxes and update the current total
            tablePullTabs[18].Serial = txtPT19Serial.Text;
            if (cbxPT19Name.SelectedIndex != -1)
            {
                tablePullTabs[18].Description = cbxPT19Name.SelectedItem.ToString();
                tablePullTabs[18].Gross = DbOps.PullTabs[cbxPT19Name.SelectedIndex].Gross;
                tablePullTabs[18].Prizes = DbOps.PullTabs[cbxPT19Name.SelectedIndex].Prizes;
                tablePullTabs[18].Tax = DbOps.PullTabs[cbxPT19Name.SelectedIndex].Tax;
                tablePullTabs[18].FormID = DbOps.PullTabs[cbxPT19Name.SelectedIndex].FormID;
            }
            else
            {
                tablePullTabs[18].Description = "";
                tablePullTabs[18].Gross = 0;
                tablePullTabs[18].Prizes = 0;
                tablePullTabs[18].Tax = 0;
                tablePullTabs[18].FormID = "";
            }
            // get worker quantities
            int f1 = 0;
            int f2 = 0;
            int f3 = 0;
            int f4 = 0;
            int f5 = 0;
            // set default of 0 if necessary
            if (txtPT19Floor1.Text.Trim() == "")
                txtPT19Floor1.Text = "0";
            if (txtPT19Floor2.Text.Trim() == "")
                txtPT19Floor2.Text = "0";
            if (txtPT19Floor3.Text.Trim() == "")
                txtPT19Floor3.Text = "0";
            if (txtPT19Floor4.Text.Trim() == "")
                txtPT19Floor4.Text = "0";
            if (txtPT19Floor5.Text.Trim() == "")
                txtPT19Floor5.Text = "0";
            if (!int.TryParse(txtPT19Floor1.Text, out f1) || !int.TryParse(txtPT19Floor2.Text, out f2) ||
                !int.TryParse(txtPT19Floor3.Text, out f3) || !int.TryParse(txtPT19Floor4.Text, out f4) ||
                !int.TryParse(txtPT19Floor5.Text, out f5))
            {
                // if parsing any of the numbers fails don't set numbers into pull tab, display error ('n/a')
                // the slash works with the logic in the "text changed of label" event
                lblPT19Total.Text = "n/a";
                return;
            }

            // set quantities in pull tab array
            tablePullTabs[18].QtyWorker1 = f1;
            tablePullTabs[18].QtyWorker2 = f2;
            tablePullTabs[18].QtyWorker3 = f3;
            tablePullTabs[18].QtyWorker4 = f4;
            tablePullTabs[18].QtyWorker5 = f5;
            // get total current
            int current = f1 + f2 + f3 + f4 + f5;
            // set current 
            tablePullTabs[18].QtyAllWorkers = current;
            // update label for total
            lblPT19Total.Text = current.ToString() + "/" + tablePullTabs[18].Gross.ToString();
            RefreshPullTabTotals();
        }
        private void UpdateFloorCardTotals(object sender, EventArgs e, int index)
        {
            // get's information from floor card worker total arrays (of textboxes) and updates the new total

            // put textbox info in strings
            string strW1Out = lstTxtW1Out[index].Text;
            string strW2Out = lstTxtW2Out[index].Text;
            string strW3Out = lstTxtW3Out[index].Text;
            string strW4Out = lstTxtW4Out[index].Text;
            string strW5Out = lstTxtW5Out[index].Text;
            string strW1In = lstTxtW1In[index].Text;
            string strW2In = lstTxtW2In[index].Text;
            string strW3In = lstTxtW3In[index].Text;
            string strW4In = lstTxtW4In[index].Text;
            string strW5In = lstTxtW5In[index].Text;
            // create integers to hold parsed info
            int intW1Out;
            int intW2Out;
            int intW3Out;
            int intW4Out;
            int intW5Out;
            int intW1In;
            int intW2In;
            int intW3In;
            int intW4In;
            int intW5In;
            // variables to hold calculations
            int intW1Sold = 0;
            int intW2Sold = 0;
            int intW3Sold = 0;
            int intW4Sold = 0;
            int intW5Sold = 0;
            double dblW1Owes = 0.0;
            double dblW2Owes = 0.0;
            double dblW3Owes = 0.0;
            double dblW4Owes = 0.0;
            double dblW5Owes = 0.0;
            int intCards;
            double dblSales;
            //hold error status flag
            bool isError = false;
            // set any value to zero if empty field
            if (strW1In.Trim() == "")
                strW1In = "0";
            if (strW2In.Trim() == "")
                strW2In = "0";
            if (strW3In.Trim() == "")
                strW3In = "0";
            if (strW4In.Trim() == "")
                strW4In = "0";
            if (strW5In.Trim() == "")
                strW5In = "0";
            
            // try to parse each string as a number and yeild error if exists

            if (int.TryParse(strW1Out, out intW1Out) && int.TryParse(strW1In, out intW1In))
            {
                intW1Sold = intW1Out - intW1In;
                dblW1Owes = (double)intW1Sold * FloorCard.CurrentFloorCards[index].Price;
                lstTxtW1Owes[index].Text = dblW1Owes.ToString("n2");
            }
            else
            {
                isError = true;
            }
            if (int.TryParse(strW2Out, out intW2Out) && int.TryParse(strW2In, out intW2In))
            {
                intW2Sold = intW2Out - intW2In;
                dblW2Owes = (double)intW2Sold * FloorCard.CurrentFloorCards[index].Price;
                lstTxtW2Owes[index].Text = dblW2Owes.ToString("n2");
            }
            else
            {
                isError = true;
            }
            if (int.TryParse(strW3Out, out intW3Out) && int.TryParse(strW3In, out intW3In))
            {
                intW3Sold = intW3Out - intW3In;
                dblW3Owes = (double)intW3Sold * FloorCard.CurrentFloorCards[index].Price;
                lstTxtW3Owes[index].Text = dblW3Owes.ToString("n2");
            }
            else
            {
                isError = true;
            }
            if (int.TryParse(strW4Out, out intW4Out) && int.TryParse(strW4In, out intW4In))
            {
                intW4Sold = intW4Out - intW4In;
                dblW4Owes = (double)intW4Sold * FloorCard.CurrentFloorCards[index].Price;
                lstTxtW4Owes[index].Text = dblW4Owes.ToString("n2");
            }
            else
            {
                isError = true;
            }
            if (int.TryParse(strW5Out, out intW5Out) && int.TryParse(strW5In, out intW5In))
            {
                intW5Sold = intW5Out - intW5In;
                dblW5Owes = (double)intW5Sold * FloorCard.CurrentFloorCards[index].Price;
                lstTxtW5Owes[index].Text = dblW5Owes.ToString("n2");
            }
            else
            {
                isError = true;
            }
            if(isError)
            {
                // after all parsing if error flag set, show user with ERR message in totals fields
                lstTxtCards[index].Text = "ERR";
                lstTxtSales[index].Text = "ERR";
            }
            else
            {
                // if all values parse, display new totals
                intCards = intW1Sold + intW2Sold + intW3Sold + intW4Sold + intW5Sold;
                dblSales = dblW1Owes + dblW2Owes + dblW3Owes + dblW4Owes + dblW5Owes;
                lstTxtCards[index].Text = intCards.ToString();
                lstTxtSales[index].Text = dblSales.ToString("n2");
                RefreshTotalsRow();
            }
        }
        private void UpdateSerial(object sender, EventArgs e, int which, string newSerial, int index, ToolStripMenuItem item2update, ToolStripTextBox txt)
        {
            // gets information from contextMenuStrip and updates the current object's serial number
            string serial = "";
            if (which == 1)
            {
                // if it's the first serial number
                FloorCard.CurrentFloorCards[index].Serial1 = newSerial;
                serial = "Serial 1: ";
            }
            else
            {
                // if it's the second serial number
                FloorCard.CurrentFloorCards[index].Serial2 = newSerial;
                serial = "Serial 2: ";
            }

            item2update.Text = serial + newSerial;
            txt.Clear();
            // success message to user
            MessageBox.Show("Update Successful for " + FloorCard.CurrentFloorCards[index].Description + "!\n" + serial + newSerial);
        }


        #endregion EventHandlers

    }
}
