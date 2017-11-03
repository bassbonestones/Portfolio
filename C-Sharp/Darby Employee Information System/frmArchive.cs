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
    public partial class frmArchive : Form
    {
        public frmArchive()
        {
            InitializeComponent();
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

        private void frmArchive_Load(object sender, EventArgs e)
        {
            
            lvwArchive.View = View.Details;
            lvwArchive.GridLines = true;
            lvwArchive.FullRowSelect = true;
            lvwArchive.Columns[0].Width = 110;
            lvwArchive.Columns[1].Width = lvwArchive.Width - 114;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if(Employee.Archive.Count == 0)
            {
                MessageBox.Show("There are currently no archived employees to show.");
                return;
            }
            lvwArchive.Items.Clear();
            foreach(Employee emp in Employee.Archive)
            {
                if(emp.EmpNum.Contains(txtSearch.Text.Trim()) || emp.FullName.Contains(txtSearch.Text.Trim()))
                {
                    string[] strEmp = { emp.EmpNum, emp.FullName };
                    lvwArchive.Items.Add(new ListViewItem(strEmp));
                    lvwArchive.Refresh();
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < Employee.Archive.Count;i++)
            {
                if (lvwArchive.FocusedItem != null)
                {
                    if (lvwArchive.FocusedItem.ToString().Contains(Employee.Archive[i].EmpNum))
                    {
                        Employee.Employees.Add(Employee.Archive[i]);
                        if (Employee.Archive[i].WorkType == "Production Worker")
                            ProductionWorker.ProductionWorkers.Add(Employee.EmpToProd(Employee.Archive[i]));
                        if (Employee.Archive[i].WorkType == "Shift Supervisor")
                            ShiftSupervisor.ShiftSupervisors.Add(Employee.EmpToSuper(Employee.Archive[i]));
                        if (Employee.Archive[i].WorkType == "Team Leader")
                            TeamLeader.TeamLeaders.Add(Employee.EmpToLead(Employee.Archive[i]));
                        MessageBox.Show("Employee, " + Employee.Archive[i].FullName + ", has been added back as a current employee. In order to update the employee's old information, please go back to \"Update " + Employee.Archive[i].WorkType + "\" from the Edit Menu.");
                        Employee.Archive.RemoveAt(i);
                        this.Close();
                        break;
                    }
                }
            }
        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            if (Employee.Archive.Count == 0)
            {
                MessageBox.Show("There are currently no archived employees to show.");
                return;
            }
            lvwArchive.Items.Clear();
            foreach (Employee emp in Employee.Archive)
            {
                    string[] strEmp = { emp.EmpNum, emp.FullName };
                    lvwArchive.Items.Add(new ListViewItem(strEmp));
                    lvwArchive.Refresh();
            }
        }

        private void lvwArchive_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < Employee.Archive.Count; i++)
            {
                if (lvwArchive.FocusedItem != null)
                {
                    if (lvwArchive.FocusedItem.ToString().Contains(Employee.Archive[i].EmpNum))
                    {
                        Employee.Employees.Add(Employee.Archive[i]);
                        if (Employee.Archive[i].WorkType == "Production Worker")
                            ProductionWorker.ProductionWorkers.Add(Employee.EmpToProd(Employee.Archive[i]));
                        if (Employee.Archive[i].WorkType == "Shift Supervisor")
                            ShiftSupervisor.ShiftSupervisors.Add(Employee.EmpToSuper(Employee.Archive[i]));
                        if (Employee.Archive[i].WorkType == "Team Leader")
                            TeamLeader.TeamLeaders.Add(Employee.EmpToLead(Employee.Archive[i]));
                        MessageBox.Show("Employee, " + Employee.Archive[i].FullName + ", has been added back as a current employee. In order to update the employee's old information, please go back to \"Update " + Employee.Archive[i].WorkType + "\" from the Edit Menu.");
                        Employee.Archive.RemoveAt(i);
                        this.Close();
                        break;
                    }
                }
            }
        }
    }
}
