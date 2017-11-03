//Jeremiah Stones
//Advanced C#
//Kathy Wilganowski

// Program Specification can be found on the about page during runtime.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace StonesJ_Lab10
{
    public partial class frmShiftSupervisor : StonesJ_Lab10.frmEmployee
    { // class to use for all shift supervisor forms, inherits from frmEmployee, which inherits from Form
        private string _type;
        public frmShiftSupervisor()
        { // default constructor
            InitializeComponent();
        }
        public frmShiftSupervisor(string m, string t): base(m,t)
        { // constructor to pass data manipulation mode and worker type
            InitializeComponent();
            _type = t;
        }
        public override void setTitle()
        { // sets the title of this form
            switch (mode)
            {
                case "view":
                    this.Text = "View Shift Supervisor";
                    break;
                case "add":
                    this.Text = "Add Shift Supervisor";
                    break;
                case "update":
                    this.Text = "Update Shift Supervisor";
                    break;
                case "delete":
                    this.Text = "Delete Shift Supervisor";
                    break;
                default:
                    MessageBox.Show("Error: set mode function switch default reached. no such type found");
                    break;
            }
        }
        public override void LoadEmployee()
        { // loads shift supervisor information to form text boxes
            if (mode == "add")
            {
                pnlArrows.Visible = false;
                lblChange.Text = (ShiftSupervisor.ShiftSupervisors.Count + 1).ToString() + " of " + (ShiftSupervisor.ShiftSupervisors.Count + 1).ToString();
                foreach (Control c in pnlInput.Controls)
                    if (c.Name.Contains("txt"))
                        ((TextBox)c).Text = "";
                txtEmpID.Text = (Employee.IDCtr + 1).ToString();
                txtEmpID.Focus();
            }
            else
            {
                if (ShiftSupervisor.ShiftSupervisors.Count == 0)
                {
                    foreach (Control c in pnlInput.Controls)
                        if (c.Name.Contains("txt"))
                            ((TextBox)c).Text = "";
                    setLblChange("no employees");
                    txtEmpID.Focus();
                }
                else
                {
                    pnlArrows.Visible = true;
                    lblChange.Text = (ShiftSupervisor.SuperCtr + 1).ToString() + " of " + ShiftSupervisor.ShiftSupervisors.Count.ToString();
                    txtEmpID.Text = ShiftSupervisor.ShiftSupervisors[ShiftSupervisor.SuperCtr].EmpNum;
                    txtName.Text = ShiftSupervisor.ShiftSupervisors[ShiftSupervisor.SuperCtr].FullName;
                    txtPay.Text = ShiftSupervisor.ShiftSupervisors[ShiftSupervisor.SuperCtr].Salary;
                    txtBonus.Text = ShiftSupervisor.ShiftSupervisors[ShiftSupervisor.SuperCtr].AnnualBonus;
                    if (ShiftSupervisor.SuperCtr == 0)
                    {
                        pbxSeekPrevious.Image.Dispose(); pbxSeekPrevious.Image = Properties.Resources.seek_previous_disabled;
                        pbxSkipPrevious.Image.Dispose(); pbxSkipPrevious.Image = Properties.Resources.skip_previous_disabled;
                    }
                    else
                    {
                        pbxSeekPrevious.Image.Dispose(); pbxSeekPrevious.Image = Properties.Resources.media_seek_backward_6;
                        pbxSkipPrevious.Image.Dispose(); pbxSkipPrevious.Image = Properties.Resources.media_skip_backward_6;
                    }
                    if (ShiftSupervisor.SuperCtr == ShiftSupervisor.ShiftSupervisors.Count - 1)
                    {
                        pbxSeekNext.Image.Dispose(); pbxSeekNext.Image = Properties.Resources.seek_next_disabled;
                        pbxSkipNext.Image.Dispose(); pbxSkipNext.Image = Properties.Resources.skip_next_disabled;
                    }
                    else
                    {
                        pbxSeekNext.Image.Dispose(); pbxSeekNext.Image = Properties.Resources.media_seek_forward_6;
                        pbxSkipNext.Image.Dispose(); pbxSkipNext.Image = Properties.Resources.media_skip_forward_6;
                    }
                }
            }
        }
        public override void pbxSeekNext_Click(object sender, EventArgs e)
        {  // overridden function for the seek next picture
            if (ShiftSupervisor.SuperCtr < ShiftSupervisor.ShiftSupervisors.Count - 1)
            {
                ShiftSupervisor.SuperCtr++;
                LoadEmployee();
            }
        }
        public override void pbxSeekPrevious_Click(object sender, EventArgs e)
        {// overridden function for the seek previous picture
            if (ShiftSupervisor.SuperCtr > 0)
            {
                ShiftSupervisor.SuperCtr--;
                LoadEmployee();
            }
        }
        public override void pbxSkipNext_Click(object sender, EventArgs e)
        { // overridden function for the skip next picture
            if (ShiftSupervisor.SuperCtr < ShiftSupervisor.ShiftSupervisors.Count - 1)
            {
                ShiftSupervisor.SuperCtr = ShiftSupervisor.ShiftSupervisors.Count - 1;
                LoadEmployee();
            }
        }
        public override void pbxSkipPrevious_Click(object sender, EventArgs e)
        { // overridden function for the skip previous picture
            if (ShiftSupervisor.SuperCtr > 0)
            {
                ShiftSupervisor.SuperCtr = 0;
                LoadEmployee();
            }
        }


        public override void btnFunction_Click(object sender, EventArgs e)
        { // multi-purpose button - add, update, delete
            double rateTest;
            bool isDelete = false;
            if (mode == "delete")
                if (ShiftSupervisor.ShiftSupervisors.Count == 0 && mode != "add")
                {
                    MessageBox.Show("You currently have no Shift Supervisors.");
                    return;
                }
            if (mode == "delete")
                isDelete = true;
            if (txtEmpID.Text.Trim() == "" && !isDelete)
            {
                MessageBox.Show("Please enter the employee ID.");
                txtEmpID.Focus(); txtEmpID.SelectAll();
                return;
            }
            if (txtEmpID.Text.Length < 6 && !isDelete)
            {
                MessageBox.Show("Employee IDs must be 6 or 7 digits long.");
                txtEmpID.Focus(); txtEmpID.SelectAll();
                return;
            }
            if (txtName.Text.Trim() == "" && !isDelete)
            {
                MessageBox.Show("Please enter the employee name.");
                txtName.Focus(); txtName.SelectAll();
                return;
            }
            if (txtPay.Text.Trim() == "" && !isDelete)
            {
                MessageBox.Show("Please enter the employee salary.");
                txtPay.Focus(); txtPay.SelectAll();
                return;
            }
            if (!Double.TryParse(txtPay.Text, out rateTest) && !isDelete)
            {
                MessageBox.Show("Please enter a valid number for salary.");
                txtPay.Focus(); txtPay.SelectAll();
                return;
            }
            if ((rateTest < 30000 || rateTest > 90000) && !isDelete)
            {
                MessageBox.Show("The salary may not be less than $30,000.00 or greater than $90,000.00.");
                txtPay.Focus(); txtPay.SelectAll();
                return;
            }
            if (txtBonus.Text.Trim() == "" && !isDelete)
            {
                MessageBox.Show("Please enter the employee's annual bonus.");
                txtBonus.Focus(); txtBonus.SelectAll();
                return;
            }
            if (!Double.TryParse(txtBonus.Text, out rateTest) && !isDelete)
            {
                MessageBox.Show("Please enter a valid number for bonus.");
                txtBonus.Focus(); txtBonus.SelectAll();
                return;
            }
            if ((rateTest < 0 || rateTest > 8000) && !isDelete)
            {
                MessageBox.Show("The annual bonus may not be less than $0.00 or greater than $8,000.00.");
                txtBonus.Focus(); txtBonus.SelectAll();
                return;
            }
            
            switch (mode)
            { // handle update of list information based on form state
                case "add":
                    
                    DialogResult dlg = MessageBox.Show("Are you sure you want to add employee information for " +
                        txtName.Text + "?", "Add Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dlg == DialogResult.Yes)
                    {
                        if (txtEmpID.Text.Trim().Length == 6)
                            txtEmpID.Text = "0" + txtEmpID.Text;
                        ShiftSupervisor.ShiftSupervisors.Add(new ShiftSupervisor(ShiftSupervisor.ShiftSupervisors.Count, "Shift Supervisor", txtName.Text.Trim(), txtEmpID.Text.Trim(), "","",txtPay.Text.Trim(), txtBonus.Text.Trim(),"","",""));
                        ShiftSupervisor.SuperCtr = ShiftSupervisor.ShiftSupervisors.Count - 1;
                        Employee.Employees.Add(new Employee(Employee.Employees.Count, "Shift Supervisor", txtName.Text.Trim(), txtEmpID.Text.Trim()));
                        Employee.IDCtr++;
                        MessageBox.Show(txtName.Text + " has been successfully added.");
                        LoadEmployee();
                    }
                    break;
                case "update":
                    dlg = MessageBox.Show("Are you sure you want to update the information for " +
                        txtName.Text + "?", "Update Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dlg == DialogResult.Yes)
                    {
                        if (txtEmpID.Text.Trim().Length == 6)
                            txtEmpID.Text = "0" + txtEmpID.Text;
                        ShiftSupervisor.ShiftSupervisors[ShiftSupervisor.SuperCtr].EmpNum = txtEmpID.Text.Trim();
                        ShiftSupervisor.ShiftSupervisors[ShiftSupervisor.SuperCtr].FullName = txtName.Text.Trim();
                        ShiftSupervisor.ShiftSupervisors[ShiftSupervisor.SuperCtr].AnnualBonus = txtBonus.Text.Trim();
                        ShiftSupervisor.ShiftSupervisors[ShiftSupervisor.SuperCtr].Salary = txtPay.Text.Trim();
                        MessageBox.Show(txtName.Text + " has been successfully updated.");
                        LoadEmployee();
                    }
                    break;
                case "delete":
                    dlg = MessageBox.Show("Are you sure you want to delete the information for " +
                        txtName.Text + "?", "Delete Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dlg == DialogResult.Yes)
                    {
                        string strWorkType = "Shift Supervisor";
                        string strFullName = ShiftSupervisor.ShiftSupervisors[ShiftSupervisor.SuperCtr].FullName;
                        string strEmpNum = ShiftSupervisor.ShiftSupervisors[ShiftSupervisor.SuperCtr].EmpNum;
                        string strShiftNum = " ";
                        string strHourlyRate = " ";
                        string strSalary = ShiftSupervisor.ShiftSupervisors[ShiftSupervisor.SuperCtr].Salary; 
                        string strAnnualBonus = ShiftSupervisor.ShiftSupervisors[ShiftSupervisor.SuperCtr].AnnualBonus; 
                        string strMonthlyBonus = " ";
                        string strReqTrainHrs = " ";
                        string strAttTrainHrs = " ";
                        Employee.Archive.Add(new Employee(0, strWorkType, strFullName, strEmpNum, strShiftNum, strHourlyRate, strSalary, strAnnualBonus, strMonthlyBonus, strReqTrainHrs, strAttTrainHrs));
                        ShiftSupervisor.ShiftSupervisors.RemoveAt(ShiftSupervisor.SuperCtr);
                        if (ShiftSupervisor.SuperCtr == ShiftSupervisor.ShiftSupervisors.Count)
                            ShiftSupervisor.SuperCtr--;
                        for(int i = 0; i < Employee.Employees.Count; i++)
                        {
                            if (Employee.Employees[i].EmpNum == txtEmpID.Text.Trim())
                                Employee.Employees.RemoveAt(i);
                        }
                        LoadEmployee();
                    }
                    break;
                default:
                    MessageBox.Show("Error: this btnFunction switch default shouldn't have been reached.");
                    break;
            }
        }





        private void frmShiftSupervisor_Load(object sender, EventArgs e)
        { // load event
            // sets title and loads employees to textboxes
            setTitle();
            LoadEmployee();
        }
    }
}
