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
    partial class frmTeamLeader : frmProductionWorker
    { // class for all team leader forms, inherits from Form>>frmEmployee>>frmProductionWorker
        private string _type;
        public frmTeamLeader()
        { // default constructor
            InitializeComponent();
        }
        public frmTeamLeader(string m, string t) :base (m,t)
        { // two arg constructor to pass data manipluation mode and workerType
            InitializeComponent();
            _type = t;
        }

        public override void setTitle()
        { // set the title of this form
            switch (mode)
            {
                case "view":
                    this.Text = "View Team Leader";
                    break;
                case "add":
                    this.Text = "Add Team Leader";
                    break;
                case "update":
                    this.Text = "Update Team Leader";
                    break;
                case "delete":
                    this.Text = "Delete Team Leader";
                    break; 
            }
        }
        public override void LoadEmployee()
        { // loads production worker information onto form
            if (mode == "add")
            {
                pnlArrows.Visible = false;
                lblChange.Text = (TeamLeader.TeamLeaders.Count + 1).ToString() + " of " + (TeamLeader.TeamLeaders.Count + 1).ToString();
                foreach (Control c in pnlInput.Controls)
                    if (c.Name.Contains("txt"))
                        c.Text = "";
                txtEmpID.Text = (Employee.IDCtr + 1).ToString();
                txtEmpID.Focus();
            }
            else
            {
                if (TeamLeader.TeamLeaders.Count == 0)
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
                    lblChange.Text = (TeamLeader.LeaderCtr + 1).ToString() + " of " + TeamLeader.TeamLeaders.Count.ToString();
                    txtEmpID.Text = TeamLeader.TeamLeaders[TeamLeader.LeaderCtr].EmpNum;
                    txtName.Text = TeamLeader.TeamLeaders[TeamLeader.LeaderCtr].FullName;
                    txtPay.Text = TeamLeader.TeamLeaders[TeamLeader.LeaderCtr].HourlyRate;
                    txtShift.Text = TeamLeader.TeamLeaders[TeamLeader.LeaderCtr].ShiftNum;
                    txtBonus.Text = TeamLeader.TeamLeaders[TeamLeader.LeaderCtr].MonthlyBonus;
                    txtReqTrainingHrs.Text = TeamLeader.TeamLeaders[TeamLeader.LeaderCtr].ReqTrainHrs;
                    txtAttTrainHrs.Text = TeamLeader.TeamLeaders[TeamLeader.LeaderCtr].AttTrainHrs;
                    if (TeamLeader.LeaderCtr == 0)
                    {
                        pbxSeekPrevious.Image.Dispose(); pbxSeekPrevious.Image = Properties.Resources.seek_previous_disabled;
                        pbxSkipPrevious.Image.Dispose(); pbxSkipPrevious.Image = Properties.Resources.skip_previous_disabled;
                    }
                    else
                    {
                        pbxSeekPrevious.Image.Dispose(); pbxSeekPrevious.Image = Properties.Resources.media_seek_backward_6;
                        pbxSkipPrevious.Image.Dispose(); pbxSkipPrevious.Image = Properties.Resources.media_skip_backward_6;
                    }
                    if (TeamLeader.LeaderCtr == TeamLeader.TeamLeaders.Count - 1)
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
            if (TeamLeader.LeaderCtr < TeamLeader.TeamLeaders.Count - 1)
            {
                TeamLeader.LeaderCtr++;
                LoadEmployee();
            }
        }
        public override void pbxSeekPrevious_Click(object sender, EventArgs e)
        { // overridden function for the seek previous picture
            if (TeamLeader.LeaderCtr > 0)
            {
                TeamLeader.LeaderCtr--;
                LoadEmployee();
            }
        }
        public override void pbxSkipNext_Click(object sender, EventArgs e)
        { // overridden function for the skip next picture
            if (TeamLeader.LeaderCtr < TeamLeader.TeamLeaders.Count - 1)
            {
                TeamLeader.LeaderCtr = TeamLeader.TeamLeaders.Count - 1;
                LoadEmployee();
            }
        }
        public override void pbxSkipPrevious_Click(object sender, EventArgs e)
        { // overridden function for the skip previous picture
            if (TeamLeader.LeaderCtr > 0)
            {
                TeamLeader.LeaderCtr = 0;
                LoadEmployee();
            }
        }

        public override void btnFunction_Click(object sender, EventArgs e)
        { // multi-purpose button - add, update, delete
            double rateTest;
            bool isDelete = false;
            if (TeamLeader.TeamLeaders.Count == 0 && mode != "add")
            {
                MessageBox.Show("You currently have no Team Leaders.");
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
            if (txtBonus.Text.Trim() == "" && !isDelete)
            {
                MessageBox.Show("Please enter an amount for the employee monthly bonus.  If none, enter 0.");
                txtBonus.Focus(); txtBonus.SelectAll();
            }
            if ((Double.Parse(txtBonus.Text) < 0 || Double.Parse(txtBonus.Text) > 5000) && !isDelete)
            {
                MessageBox.Show("The monthly bonus may not be less than $0.00 or greater than $5,000.00.");
                txtBonus.Focus(); txtBonus.SelectAll();
                return;
            }
            if (txtReqTrainingHrs.Text.Trim() == "" && !isDelete)
            {
                MessageBox.Show("Please enter required training hours.  If none, enter 0.");
                txtReqTrainingHrs.Focus(); txtReqTrainingHrs.SelectAll();
            }
            if ((Double.Parse(txtReqTrainingHrs.Text) < 0 || Double.Parse(txtReqTrainingHrs.Text) > 60) && !isDelete)
            {
                MessageBox.Show("The required training hours may not be less than 0 or greater than 60.");
                txtReqTrainingHrs.Focus(); txtReqTrainingHrs.SelectAll();
                return;
            }
            if (txtAttTrainHrs.Text.Trim() == "" && !isDelete)
            {
                MessageBox.Show("Please enter attended training hours.  If none, enter 0.");
                txtAttTrainHrs.Focus(); txtAttTrainHrs.SelectAll();
            }
            if ((Double.Parse(txtAttTrainHrs.Text) < 0 || Double.Parse(txtAttTrainHrs.Text) > 60) && !isDelete)
            {
                MessageBox.Show("The attended training hours may not be less than 0 or greater than 60.");
                txtAttTrainHrs.Focus(); txtAttTrainHrs.SelectAll();
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
                        TeamLeader.TeamLeaders.Add(new TeamLeader(TeamLeader.TeamLeaders.Count, "Team Leader", txtName.Text.Trim(), txtEmpID.Text.Trim(), txtShift.Text.Trim(), txtPay.Text.Trim(),"","", txtBonus.Text.Trim(), txtReqTrainingHrs.Text.Trim(), txtAttTrainHrs.Text.Trim()));
                        TeamLeader.LeaderCtr = TeamLeader.TeamLeaders.Count - 1;
                        Employee.Employees.Add(new Employee(Employee.Employees.Count, "Team Leader", txtName.Text.Trim(), txtEmpID.Text.Trim()));
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
                        TeamLeader.TeamLeaders[TeamLeader.LeaderCtr].EmpNum = txtEmpID.Text.Trim();
                        TeamLeader.TeamLeaders[TeamLeader.LeaderCtr].FullName = txtName.Text.Trim();
                        TeamLeader.TeamLeaders[TeamLeader.LeaderCtr].ShiftNum = txtShift.Text.Trim();
                        TeamLeader.TeamLeaders[TeamLeader.LeaderCtr].HourlyRate = txtPay.Text.Trim();
                        TeamLeader.TeamLeaders[TeamLeader.LeaderCtr].MonthlyBonus = txtBonus.Text.Trim();
                        TeamLeader.TeamLeaders[TeamLeader.LeaderCtr].ReqTrainHrs = txtReqTrainingHrs.Text.Trim();
                        TeamLeader.TeamLeaders[TeamLeader.LeaderCtr].AttTrainHrs = txtAttTrainHrs.Text.Trim();
                        MessageBox.Show(txtName.Text + " has been successfully updated.");
                        LoadEmployee();
                    }
                    break;
                case "delete":
                    
                    dlg = MessageBox.Show("Are you sure you want to delete the information for " +
                        txtName.Text + "?", "Delete Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dlg == DialogResult.Yes)
                    {
                        string strWorkType = "Team Leader";
                        string strFullName = TeamLeader.TeamLeaders[TeamLeader.LeaderCtr].FullName;
                        string strEmpNum = TeamLeader.TeamLeaders[TeamLeader.LeaderCtr].EmpNum;
                        string strShiftNum = TeamLeader.TeamLeaders[TeamLeader.LeaderCtr].ShiftNum;
                        string strHourlyRate = TeamLeader.TeamLeaders[TeamLeader.LeaderCtr].HourlyRate;
                        string strSalary = " ";
                        string strAnnualBonus = " ";
                        string strMonthlyBonus = TeamLeader.TeamLeaders[TeamLeader.LeaderCtr].MonthlyBonus;
                        string strReqTrainHrs = TeamLeader.TeamLeaders[TeamLeader.LeaderCtr].ReqTrainHrs;
                        string strAttTrainHrs = TeamLeader.TeamLeaders[TeamLeader.LeaderCtr].AttTrainHrs;
                        Employee.Archive.Add(new Employee(0, strWorkType, strFullName, strEmpNum, strShiftNum, strHourlyRate, strSalary, strAnnualBonus, strMonthlyBonus, strReqTrainHrs, strAttTrainHrs));
                        TeamLeader.TeamLeaders.RemoveAt(TeamLeader.LeaderCtr);
                        if (TeamLeader.LeaderCtr == TeamLeader.TeamLeaders.Count)
                            TeamLeader.LeaderCtr--;
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

        private void frmTeamLeader_Load(object sender, EventArgs e)
        {
            // load event
            setTitle();
        }
    }
}
