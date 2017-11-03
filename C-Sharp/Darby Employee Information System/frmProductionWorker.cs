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
    public partial class frmProductionWorker : StonesJ_Lab10.frmEmployee
    { // class for production worker forms, inherits from frmEmployee which inherits from Form
        private string _type;
        public frmProductionWorker()
        { // defaut constructor
            InitializeComponent();
        }
        public frmProductionWorker(string m, string t):base(m,t)
        { // two arg constructor to pass data manipluation mode and workerType
            InitializeComponent();
            _type = t;
        }

        public override void setTitle()
        { // set the title of this form
            switch(mode)
            {
                case "view":
                    this.Text = "View Production Worker";
                    break;
                case "add":
                    this.Text = "Add Production Worker";
                    break;
                case "update":
                    this.Text = "Update Production Worker";
                    break;
                case "delete":
                    this.Text = "Delete Production Worker";
                    break;
            }
        }
        public override void LoadEmployee()
        { // loads production worker information onto form
            if (mode == "add")
            {
                pnlArrows.Visible = false;
                setLblChange((ProductionWorker.ProductionWorkers.Count + 1).ToString() + " of " + (ProductionWorker.ProductionWorkers.Count + 1).ToString());
                foreach (Control c in pnlInput.Controls)
                    if (c.Name.Contains("txt"))
                        ((TextBox)c).Text = "";
                txtEmpID.Text = (Employee.IDCtr + 1).ToString();
                txtEmpID.Focus();
            }
            else
            {
                if (ProductionWorker.ProductionWorkers.Count == 0)
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
                    setLblChange((ProductionWorker.WorkerCtr + 1).ToString() + " of " + ProductionWorker.ProductionWorkers.Count.ToString());
                    txtEmpID.Text = ProductionWorker.ProductionWorkers[ProductionWorker.WorkerCtr].EmpNum;
                    txtName.Text = ProductionWorker.ProductionWorkers[ProductionWorker.WorkerCtr].FullName;
                    txtPay.Text = ProductionWorker.ProductionWorkers[ProductionWorker.WorkerCtr].HourlyRate;
                    txtShift.Text = ProductionWorker.ProductionWorkers[ProductionWorker.WorkerCtr].ShiftNum;
                    if (ProductionWorker.WorkerCtr == 0)
                    {
                        pbxSeekPrevious.Image.Dispose(); pbxSeekPrevious.Image = Properties.Resources.seek_previous_disabled;
                        pbxSkipPrevious.Image.Dispose(); pbxSkipPrevious.Image = Properties.Resources.skip_previous_disabled;
                    }
                    else
                    {
                        pbxSeekPrevious.Image.Dispose(); pbxSeekPrevious.Image = Properties.Resources.media_seek_backward_6;
                        pbxSkipPrevious.Image.Dispose(); pbxSkipPrevious.Image = Properties.Resources.media_skip_backward_6;
                    }
                    if (ProductionWorker.WorkerCtr == ProductionWorker.ProductionWorkers.Count - 1)
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
        { // overridden function for the seek next picture
            if (ProductionWorker.WorkerCtr < ProductionWorker.ProductionWorkers.Count - 1)
            {
                ProductionWorker.WorkerCtr++;
                LoadEmployee();
            }
        }
        public override void pbxSeekPrevious_Click(object sender, EventArgs e)
        { // overridden function for the seek previous picture
            if (ProductionWorker.WorkerCtr > 0)
            {
                ProductionWorker.WorkerCtr--;
                LoadEmployee();
            }
        }
        public override void pbxSkipNext_Click(object sender, EventArgs e)
        { // overridden function for the skip next picture
            if (ProductionWorker.WorkerCtr < ProductionWorker.ProductionWorkers.Count - 1)
            {
                ProductionWorker.WorkerCtr = ProductionWorker.ProductionWorkers.Count - 1;
                LoadEmployee();
            }
        }
        public override void pbxSkipPrevious_Click(object sender, EventArgs e)
        { // overridden function for the skip previous picture
            if (ProductionWorker.WorkerCtr > 0)
            {
                ProductionWorker.WorkerCtr = 0;
                LoadEmployee();
            }
        }

        public override void btnFunction_Click(object sender, EventArgs e)
        { // multi-purpose button - add, update, delete
            double rateTest;
            bool isDelete = false;
            if (ProductionWorker.ProductionWorkers.Count == 0 && mode != "add")
            {
                MessageBox.Show("You currently have no Production Workers.");
                    return;              
            }
            if (mode == "delete")
                isDelete = true;
            // data validation
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
                MessageBox.Show("Please enter the employee hourly rate of pay.");
                txtPay.Focus(); txtPay.SelectAll();
                return;
            }
            if (!Double.TryParse(txtPay.Text, out rateTest) && !isDelete)
            {
                MessageBox.Show("Please enter a valid number for the rate of pay.");
                txtPay.Focus(); txtPay.SelectAll();
                return;
            }
            if ((rateTest < 7.25 || rateTest > 30) && !isDelete)
            {
                MessageBox.Show("The rate of pay may not be less than 7.25 or greater than 30.00.");
                txtPay.Focus(); txtPay.SelectAll();
                return;
            }
            if ((txtShift.Text.Trim() != "1" && txtShift.Text.Trim() != "2") && !isDelete)
            {
                MessageBox.Show("Please enter the employee shift number (1 or 2).");
                txtShift.Focus(); txtShift.SelectAll();
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
                        ProductionWorker.ProductionWorkers.Add(new ProductionWorker(ProductionWorker.ProductionWorkers.Count, "Production Worker", txtName.Text.Trim(), txtEmpID.Text.Trim(), txtShift.Text.Trim(), txtPay.Text.Trim(),"","","","",""));
                        ProductionWorker.WorkerCtr = ProductionWorker.ProductionWorkers.Count - 1;
                        Employee.Employees.Add(new Employee(Employee.Employees.Count, "Production Worker", txtName.Text.Trim(), txtEmpID.Text.Trim()));
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
                        ProductionWorker.ProductionWorkers[ProductionWorker.WorkerCtr].EmpNum = txtEmpID.Text.Trim();
                        ProductionWorker.ProductionWorkers[ProductionWorker.WorkerCtr].FullName = txtName.Text.Trim();
                        ProductionWorker.ProductionWorkers[ProductionWorker.WorkerCtr].ShiftNum = txtShift.Text.Trim();
                        ProductionWorker.ProductionWorkers[ProductionWorker.WorkerCtr].HourlyRate = txtPay.Text.Trim();
                        MessageBox.Show(txtName.Text + " has been successfully updated.");
                        LoadEmployee();
                    }
                    break;
                case "delete":
                    dlg = MessageBox.Show("Are you sure you want to delete the information for " +
                        txtName.Text + "?", "Delete Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dlg == DialogResult.Yes)
                    {
                        string strWorkType = "Production Worker";
                        string strFullName = ProductionWorker.ProductionWorkers[ProductionWorker.WorkerCtr].FullName;
                        string strEmpNum = ProductionWorker.ProductionWorkers[ProductionWorker.WorkerCtr].EmpNum;
                        string strShiftNum = ProductionWorker.ProductionWorkers[ProductionWorker.WorkerCtr].ShiftNum;
                        string strHourlyRate = ProductionWorker.ProductionWorkers[ProductionWorker.WorkerCtr].HourlyRate;
                        string strSalary = " ";
                        string strAnnualBonus = " ";
                        string strMonthlyBonus = " ";
                        string strReqTrainHrs = " ";
                        string strAttTrainHrs = " ";
                        Employee.Archive.Add(new Employee(0,strWorkType,strFullName,strEmpNum,strShiftNum,strHourlyRate,strSalary,strAnnualBonus,strMonthlyBonus,strReqTrainHrs,strAttTrainHrs));
                        ProductionWorker.ProductionWorkers.RemoveAt(ProductionWorker.WorkerCtr);
                        if (ProductionWorker.WorkerCtr == ProductionWorker.ProductionWorkers.Count)
                            ProductionWorker.WorkerCtr--;
                        for (int i = 0; i < Employee.Employees.Count; i++)
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

        private void frmProductionWorker_Load(object sender, EventArgs e)
        {
            // load event
            
            setTitle();
        }
    }
}
