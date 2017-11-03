//Jeremiah Stones
//Advanced C#
//Kathy Wilganowski

// Program Specification can be found on the about page during runtime.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StonesJ_Lab10
{
    public partial class frmEmployee : Form
    {
        // This class only inherits the basic Form class.  It's used to outline all of the controls that 
        // any type of employee will need on their form.  It handles all of the button text changes and
        // key press events. It also sets up virtual functions so that derived form classes can overload them.

        // set up public properties to determine form states.
        public static string mode { get; set; }
        public static string type { get; set; }
        public frmEmployee()
        { // default constructor
            InitializeComponent();
        }
        public frmEmployee(string m, string t)
        { // two arg constructor for seting up the form state
            InitializeComponent();
            mode = m;
            type = t;
        }

        // create const variable for form param of disabled X button

        private const int CP_NOCLOSE_BUTTON = 0x200;
        protected override CreateParams CreateParams
        {
            // overrides CreateParams for the the form to disable the X button
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        }

        public void turnOffShift()
        { // turns off the Shift TextBox and Label
            txtShift.Visible = false;
            lblShift.Visible = false;
        }
        public void turnOffTraining()
        { // turns off both training TextBoxes and Labels
            txtReqTrainingHrs.Visible = false;
            lblReqTrainHrs.Visible = false;
            txtAttTrainHrs.Visible = false;
            lblAttTrainHrs.Visible = false;
        }
        public void turnOffBonus()
        { // turns off the bonus textbox and label
            txtBonus.Visible = false;
            lblBonus.Visible = false;
        }
        public void setLblChange(string message)
        { // changes the text of the record number label
            lblChange.Text = message;
        }
        public void setlblPay(string s)
        { // changes the text of the employee pay label
            lblPay.Text = s;
        }
        public void setlblBonus(string s)
        { // changes the text of the bonus label
            lblBonus.Text = s;
        }
        private void txtEmpID_KeyPress(object sender, KeyPressEventArgs e)
        { // event handles keypress for employee ID and Shift
            // it only allows numbers to be entered
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
            if(e.KeyChar == (char)Keys.Enter)
                btnFunction.PerformClick();
        }

        public virtual void LoadEmployee()
        { // pure virtual function for loading employee data into textboxes
        }
        private void SetMode()
        { // function for enabling/disabling textboxes depending on form state
            switch (mode)
            {
                case "view":
                    foreach (Control c in pnlInput.Controls)
                    {
                        if (c.Name.ToLower().Contains("txt"))
                        {
                            ((TextBox)c).Enabled = false;
                        }
                    }
                    btnFunction.Visible = false;
                    break;
                case "add":
                    txtEmpID.Enabled = false;
                    btnFunction.Text = "Add";
                    break;
                case "update":
                    txtEmpID.Enabled = false;
                    btnFunction.Text = "Update";
                    break;
                case "delete":
                    foreach (Control c in pnlInput.Controls)
                    {
                        if (c.Name.ToLower().Contains("txt"))
                        {
                            ((TextBox)c).Enabled = false;
                        }
                    }
                    btnFunction.Text = "Delete";
                    break;
            }

        }
        public virtual void setTitle()
        { // pure virtual function for setting the form title

        }
        private void btnHome_Click(object sender, EventArgs e)
        {
            Application.OpenForms[0].Show();
            this.Close();
        }

        public virtual void btnFunction_Click(object sender, EventArgs e)
        { // pure virutal function for changing the main button's functionality

        }

        public virtual void pbxSkipPrevious_Click(object sender, EventArgs e)
        { // pure virtual function for the skip previous picture

        }

        public virtual void pbxSeekPrevious_Click(object sender, EventArgs e)
        { // pure virtual function for the seek previous picture

        }

        public virtual void pbxSeekNext_Click(object sender, EventArgs e)
        { // pure virtual function for the seek next picture

        }

        public virtual void pbxSkipNext_Click(object sender, EventArgs e)
        { // pure virtual function for the skip next picture

        }

        private void txtName_KeyPress(object sender, KeyPressEventArgs e)
        { // key press event definition for employee name
            // only allows letters and specific characters
            if (!Char.IsLetter(e.KeyChar) && !Char.IsControl(e.KeyChar) && e.KeyChar != '.' &&
                e.KeyChar != ' ' && e.KeyChar != '-' && e.KeyChar != '\'')
                e.Handled = true;
            if (e.KeyChar == (char)Keys.Enter)
                btnFunction.PerformClick();
        }

        private void txtPay_KeyPress(object sender, KeyPressEventArgs e)
        { //key press event definition for pay, bonus, and both training textboxes
            // only allows numbers and a decimal.  Coded to disable the ability
            // to enter in more than two number past the decimal.
            foreach (Control c in pnlInput.Controls)
                if (c.Name.Contains("txt"))
                    if (((TextBox)c).Focused)
                    {
                        switch (c.Name)
                        {
                            case "txtPay":
                                if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar) && e.KeyChar != '.')
                                {
                                    e.Handled = true;
                                }
                                if (e.KeyChar == '.')
                                {
                                    for (int i = 0; i < txtPay.Text.Length; i++)
                                    {
                                        if (txtPay.Text[i] == '.')
                                            e.Handled = true;
                                    }
                                }
                                else if (Char.IsDigit(e.KeyChar))
                                {
                                    int i; bool hasDec = false;
                                    for (i = 0; i < txtPay.Text.Length; i++)
                                    {
                                        if (txtPay.Text[i] == '.')
                                        {
                                            hasDec = true;
                                            break;
                                        }
                                    } // this if statement is where the magic happens :)
                                    if (hasDec && (txtPay.SelectionStart > (i + 2) || (txtPay.SelectionStart > i && txtPay.Text.IndexOf('.') == txtPay.Text.Trim().Length - 3)))
                                        e.Handled = true;
                                }
                                break;
                            case "txtBonus":
                                if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar) && e.KeyChar != '.')
                                {
                                    e.Handled = true;
                                }
                                if (e.KeyChar == '.')
                                {
                                    for (int i = 0; i < txtBonus.Text.Length; i++)
                                    {
                                        if (txtBonus.Text[i] == '.')
                                            e.Handled = true;
                                    }
                                }
                                else if (Char.IsDigit(e.KeyChar))
                                {
                                    int i; bool hasDec = false;
                                    for (i = 0; i < txtBonus.Text.Length; i++)
                                    {
                                        if (txtBonus.Text[i] == '.')
                                        {
                                            hasDec = true;
                                            break;
                                        }
                                    } // this if statement is where the magic happens :)
                                    if (hasDec && (txtBonus.SelectionStart > (i + 2) || (txtBonus.SelectionStart > i && txtBonus.Text.IndexOf('.') == txtBonus.Text.Trim().Length - 3)))
                                        e.Handled = true;
                                }
                                break;
                            case "txtAttTrainHrs":
                                if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar) && e.KeyChar != '.')
                                {
                                    e.Handled = true;
                                }
                                if (e.KeyChar == '.')
                                {
                                    for (int i = 0; i < txtAttTrainHrs.Text.Length; i++)
                                    {
                                        if (txtAttTrainHrs.Text[i] == '.')
                                            e.Handled = true;
                                    }
                                }
                                else if (Char.IsDigit(e.KeyChar))
                                {
                                    int i; bool hasDec = false;
                                    for (i = 0; i < txtAttTrainHrs.Text.Length; i++)
                                    {
                                        if (txtAttTrainHrs.Text[i] == '.')
                                        {
                                            hasDec = true;
                                            break;
                                        }
                                    } // this if statement is where the magic happens :)
                                    if (hasDec && (txtAttTrainHrs.SelectionStart > (i + 2) || (txtAttTrainHrs.SelectionStart > i && txtAttTrainHrs.Text.IndexOf('.') == txtAttTrainHrs.Text.Trim().Length - 3)))
                                        e.Handled = true;
                                }
                                break;
                            case "txtReqTrainingHrs":
                                if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar) && e.KeyChar != '.')
                                {
                                    e.Handled = true;
                                }
                                if (e.KeyChar == '.')
                                {
                                    for (int i = 0; i < txtReqTrainingHrs.Text.Length; i++)
                                    {
                                        if (txtReqTrainingHrs.Text[i] == '.')
                                            e.Handled = true;
                                    }
                                }
                                else if (Char.IsDigit(e.KeyChar))
                                {
                                    int i; bool hasDec = false;
                                    for (i = 0; i < txtReqTrainingHrs.Text.Length; i++)
                                    {
                                        if (txtReqTrainingHrs.Text[i] == '.')
                                        {
                                            hasDec = true;
                                            break;
                                        }
                                    } // this if statement is where the magic happens :)
                                    if (hasDec && (txtReqTrainingHrs.SelectionStart > (i + 2) || (txtReqTrainingHrs.SelectionStart > i && txtReqTrainingHrs.Text.IndexOf('.') == txtReqTrainingHrs.Text.Trim().Length - 3)))
                                        e.Handled = true;
                                }
                                break;
                        }
                        
                    }
            
            if (e.KeyChar == (char)Keys.Enter)
                btnFunction.PerformClick();
        }

        public void frmEmployee_Load(object sender, EventArgs e)
        { // load event for any form with this class as a base class
            // first it handles any unnecessary loads during designMode
            // sets the form state using LoadEmployee and SetMode
            // along with workerType.
            if (DesignMode) return;
            LoadEmployee();
            SetMode();
            if (type == "worker")
            {
                turnOffBonus();
                turnOffTraining();
                setlblPay("Hourly Pay Rate:");
            }
            else if(type == "supervisor")
            {
                setlblBonus("Annual Bonus:");
                setlblPay("Salary:");
                turnOffTraining();
                turnOffShift();
            }
            else if(type == "leader")
            {
                setlblPay("Hourly Pay Rate:");
                setlblBonus("Monthly Bonus:");
            }

        }
    }
}

