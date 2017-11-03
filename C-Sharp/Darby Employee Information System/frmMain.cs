//Jeremiah Stones
//Advanced C#
//Kathy Wilganowski

// Program Specification can be found on the about page during runtime.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StonesJ_Lab10
{
    public partial class frmMain : Form
    {
        // this class only inherits the standard Form class
        // main menu form, user navigates printing, viewing, adding, updating, deleting and about

        // variables for file path and printing
        const string PATH = @"C:\Users\Administrator\AppData\Roaming\Jeremy Stones\Data\Employee.csv";
        const string ARCHIVE = @"C:\Users\Administrator\AppData\Roaming\Jeremy Stones\Data\Archive.csv";
        bool printMore = false;
        int printerPageCtr;
        int printPageNum;
        public frmMain()
        { // default constructor
            InitializeComponent();
        }

        public void frmMain_Load(object sender, EventArgs e)
        { // load event centers the form title and loads the records from a file into lists
            lblTitle.Location = new Point((this.Width - lblTitle.Width) / 2 - 5, (this.Height - lblTitle.Height) / 2-20);
            if (File.Exists(PATH))
            {
                string[] strEmp = new string[10];
                Employee emp;
                TeamLeader leader;
                ShiftSupervisor supervisor;
                ProductionWorker worker;
                StreamReader sr;
                sr = File.OpenText(PATH);
                while (!sr.EndOfStream)
                {
                    strEmp = sr.ReadLine().ToString().Split(',');
                    //string s = sr.ReadLine().ToString();
                    
                    if (strEmp[0].Trim() != "")
                    {
                        if (strEmp[0] != "typeWorker")
                        {
                            emp = new Employee(Employee.EmpCtr++, strEmp[0], strEmp[1], strEmp[2], strEmp[3], strEmp[4], strEmp[5], strEmp[6], strEmp[7], strEmp[8], strEmp[9]);
                            Employee.Employees.Add(emp);
                        }
                        if (strEmp[0] == "Production Worker")
                        {
                            worker = new ProductionWorker(ProductionWorker.WorkerCtr++, strEmp[0], strEmp[1], strEmp[2], strEmp[3], strEmp[4], strEmp[5], strEmp[6], strEmp[7], strEmp[8], strEmp[9]);
                            ProductionWorker.ProductionWorkers.Add(worker);
                        }
                        else if (strEmp[0] == "Shift Supervisor")
                        {
                            supervisor = new ShiftSupervisor(ShiftSupervisor.SuperCtr++, strEmp[0], strEmp[1], strEmp[2], strEmp[3], strEmp[4], strEmp[5], strEmp[6], strEmp[7], strEmp[8], strEmp[9]);
                            ShiftSupervisor.ShiftSupervisors.Add(supervisor);
                        }
                        else if (strEmp[0] == "Team Leader")
                        {
                            leader = new TeamLeader(TeamLeader.LeaderCtr++, strEmp[0], strEmp[1], strEmp[2], strEmp[3], strEmp[4], strEmp[5], strEmp[6], strEmp[7], strEmp[8], strEmp[9]);
                            TeamLeader.TeamLeaders.Add(leader);
                        }
                        else
                        {
                            // ignore (title line)
                        }
                    }
                }
                Employee.EmpCtr = 0;
                    ProductionWorker.WorkerCtr = 0;
                    ShiftSupervisor.SuperCtr = 0;
                    TeamLeader.LeaderCtr = 0;
                    sr.Close();
                
            }
            else
            { // if the file or directory for the file doesn't exists, create them
                Directory.CreateDirectory(@"C:\Users\Administrator\AppData\Roaming\Jeremy Stones\Data");
                StreamWriter sw = File.CreateText(PATH);
                sw.Close();
            }
            if (File.Exists(ARCHIVE))
            {
                string[] strEmp = new string[10];
                Employee emp;
                StreamReader sr;
                sr = File.OpenText(ARCHIVE);
                while (!sr.EndOfStream)
                {
                    strEmp = sr.ReadLine().ToString().Split(',');
                    if (strEmp[0].Trim() != "" && strEmp[0] != "typeWorker")
                    {
                        emp = new Employee(0, strEmp[0], strEmp[1], strEmp[2], strEmp[3], strEmp[4], strEmp[5], strEmp[6], strEmp[7], strEmp[8], strEmp[9]);
                        Employee.Archive.Add(emp);
                    }
                }
                sr.Close();
            }
            else
            { // if the file or directory for the file doesn't exists, create them
                Directory.CreateDirectory(@"C:\Users\Administrator\AppData\Roaming\Jeremy Stones\Data");
                StreamWriter sw = File.CreateText(ARCHIVE);
                sw.Close();
            }
            foreach (Employee em in Employee.Employees)
            {
                if (int.Parse(em.EmpNum) > Employee.IDCtr)
                    Employee.IDCtr = int.Parse(em.EmpNum);
            }
            foreach (Employee em in Employee.Archive)
            {
                int empNum;
                if (int.TryParse(em.EmpNum, out empNum))
                    if (empNum > Employee.IDCtr)
                        Employee.IDCtr = int.Parse(em.EmpNum);
            }
            
        }
        private void mnuEditAddWorker_Click(object sender, EventArgs e)
        {// add new production workers
            this.Hide();
            frmProductionWorker emp = new frmProductionWorker("add", "worker");
            emp.Show();
        }

        private void mnuEditAddSupervisor_Click(object sender, EventArgs e)
        {// add supervisors
            this.Hide();
            frmShiftSupervisor emp = new frmShiftSupervisor("add", "supervisor");
            emp.Show();
        }

        private void mnuEditAddLeader_Click(object sender, EventArgs e)
        {// add new team leaders
            this.Hide();
            frmTeamLeader emp = new frmTeamLeader("add", "leader");
            emp.Show();
        }
        private void mnuEditAddFormer_Click(object sender, EventArgs e)
        {
            frmArchive archive = new frmArchive();
            archive.ShowDialog();
        }
        private void mnuEditUpdateWorker_Click(object sender, EventArgs e)
        { // update production workers
            this.Hide();
            frmProductionWorker emp = new frmProductionWorker("update","worker");
            emp.Show();
        }

        private void mnuEditUpdateSupervisor_Click(object sender, EventArgs e)
        { // update production workers
            this.Hide();
            frmShiftSupervisor emp = new frmShiftSupervisor("update", "supervisor");
            emp.Show();
        }

        private void mnuEditUpdateLeader_Click(object sender, EventArgs e)
        { // update team leaders
            this.Hide();
            frmTeamLeader emp = new frmTeamLeader("update","leader");
            emp.Show();
        }

        private void mnuEditDeleteWorker_Click(object sender, EventArgs e)
        { // delete production workers
            this.Hide();
            frmProductionWorker emp = new frmProductionWorker("delete","worker");
            emp.Show();
        }

        private void mnuEditDeleteSupervisor_Click(object sender, EventArgs e)
        { // delete supervisors
            this.Hide();
            frmShiftSupervisor emp = new frmShiftSupervisor("delete", "supervisor");
            emp.Show();
        }

        private void mnuEditDeleteLeader_Click(object sender, EventArgs e)
        { // delete team leader
            this.Hide();
            frmTeamLeader emp = new frmTeamLeader("delete","leader");
            emp.Show();
        }
        private void mnuViewWorker_Click(object sender, EventArgs e)
        { // view production workers
            this.Hide();
            frmProductionWorker emp = new frmProductionWorker("view","worker");
            emp.Show();
        }

        private void mnuViewSupervisor_Click(object sender, EventArgs e)
        { // view supervisors
            this.Hide();
            frmShiftSupervisor emp = new frmShiftSupervisor("view","supervisor");
            emp.Show();

        }

        private void mnuViewLeader_Click(object sender, EventArgs e)
        { // view team leaders
            this.Hide();
            frmTeamLeader emp = new frmTeamLeader("view","leader");
            emp.Show();
        }
        private void mnuFileExit_Click(object sender, EventArgs e)
        {
            // close this form, send to closing event for confirm and file write
            this.Close();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            // get close confirmation, write to file
            DialogResult dlg = MessageBox.Show("Are you sure you want to exit?", "Exit Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlg == DialogResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                Directory.CreateDirectory(".../.../Data");
                StreamWriter sw = File.CreateText(PATH);
                sw.WriteLine("typeWorker,name,empNumb,shiftNumb,hrlyPayRt,annualSalary,annualProdBonus,monthlyBonus,ReqTrainHrs,AttndTrainhrs");
                string[] emp = new string[10];
                string sEmp;
                for (int i = 0; i < Employee.Employees.Count; i++)
                {
                    for (int j = 0; j < 10; j++)
                        emp[j] = " ";
                    sEmp = "";
                    emp[0] = Employee.Employees[i].WorkType;
                    emp[1] = Employee.Employees[i].FullName;
                    emp[2] = Employee.Employees[i].EmpNum;
                    if (Employee.Employees[i].WorkType == "Production Worker")
                    {
                        for (int j = 0; j < ProductionWorker.ProductionWorkers.Count; j++)
                        {
                            if (Employee.Employees[i].EmpNum == ProductionWorker.ProductionWorkers[j].EmpNum)
                            {
                                emp[3] = ProductionWorker.ProductionWorkers[j].ShiftNum;
                                emp[4] = ProductionWorker.ProductionWorkers[j].HourlyRate;
                            }
                        }
                    }
                    else if (Employee.Employees[i].WorkType == "Shift Supervisor")
                    {
                        for (int j = 0; j < ShiftSupervisor.ShiftSupervisors.Count; j++)
                        {
                            if (Employee.Employees[i].EmpNum == ShiftSupervisor.ShiftSupervisors[j].EmpNum)
                            {
                                emp[5] = ShiftSupervisor.ShiftSupervisors[j].Salary;
                                emp[6] = ShiftSupervisor.ShiftSupervisors[j].AnnualBonus;
                            }
                        }
                    }
                    else if (Employee.Employees[i].WorkType == "Team Leader")
                    {
                        for (int j = 0; j < TeamLeader.TeamLeaders.Count; j++)
                        {
                            if (Employee.Employees[i].EmpNum == TeamLeader.TeamLeaders[j].EmpNum)
                            {
                                emp[3] = TeamLeader.TeamLeaders[j].ShiftNum;
                                emp[4] = TeamLeader.TeamLeaders[j].HourlyRate;
                                emp[7] = TeamLeader.TeamLeaders[j].MonthlyBonus;
                                emp[8] = TeamLeader.TeamLeaders[j].ReqTrainHrs;
                                emp[9] = TeamLeader.TeamLeaders[j].AttTrainHrs;
                            }
                        }
                    }
                    else
                        MessageBox.Show("Check your worker types. " + Employee.Employees[i].FullName + " has an invalid work type.");
                    for (int j = 0; j < 10; j++)
                    {
                        sEmp += emp[j];
                        if (j < 9)
                            sEmp += ",";
                    }
                    sw.WriteLine(sEmp);
                }
                sw.Close();
                //-------- update archive
                sw = File.CreateText(ARCHIVE);
                sw.WriteLine("typeWorker,name,empNumb,shiftNumb,hrlyPayRt,annualSalary,annualProdBonus,monthlyBonus,ReqTrainHrs,AttndTrainhrs");

                for (int i = 0; i < Employee.Archive.Count; i++)
                {
                    for (int j = 0; j < 10; j++)
                        emp[j] = " ";
                    sEmp = "";
                    emp[0] = Employee.Archive[i].WorkType;
                    emp[1] = Employee.Archive[i].FullName;
                    emp[2] = Employee.Archive[i].EmpNum;
                    emp[3] = Employee.Archive[i].ShiftNum;
                    emp[4] = Employee.Archive[i].HourlyRate;
                    emp[5] = Employee.Archive[i].Salary;
                    emp[6] = Employee.Archive[i].AnnualBonus;
                    emp[7] = Employee.Archive[i].MonthlyBonus;
                    emp[8] = Employee.Archive[i].ReqTrainHrs;
                    emp[9] = Employee.Archive[i].AttTrainHrs;
                    for (int j = 0; j < 10; j++)
                    {
                        sEmp += emp[j];
                        if (j < 9)
                            sEmp += ",";
                    }
                    sw.WriteLine(sEmp);
                }
                sw.Close();
            }
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            // proper exit
            Application.Exit();
        }

        private void mnuFilePreviewWorker_Click(object sender, EventArgs e)
        {
            // creates a print preview
            if (ProductionWorker.ProductionWorkers.Count == 0)
            {
                MessageBox.Show("You don't have any \'Production Workers\' to print");
                return;
            }
            ((Form)dlgPpvWorkers).WindowState = FormWindowState.Maximized;
            ((Form)dlgPpvWorkers).BackColor = Color.Pink;
            ((Form)dlgPpvWorkers).Font = new Font("Times New Roman", 25, FontStyle.Bold);

            if (ProductionWorker.ProductionWorkers.Count < 11)
            {
                dlgPpvWorkers.PrintPreviewControl.Columns = 1;
            }
            else if (ProductionWorker.ProductionWorkers.Count < 21)
            {
                dlgPpvWorkers.PrintPreviewControl.Columns = 2;
            }
            else if (ProductionWorker.ProductionWorkers.Count < 31)
            {
                dlgPpvWorkers.PrintPreviewControl.Columns = 3;
            }
            else if (ProductionWorker.ProductionWorkers.Count < 41)
            {
                dlgPpvWorkers.PrintPreviewControl.Columns = 2;
                dlgPpvWorkers.PrintPreviewControl.Rows = 2;
            }
            else
            {
                dlgPpvWorkers.PrintPreviewControl.Columns = 3;
                dlgPpvWorkers.PrintPreviewControl.Rows = 2;
            }

            dlgPpvWorkers.ShowDialog();
        }

        private void mnuFilePrintWorker_Click(object sender, EventArgs e)
        {
            // prints the page directly
            if (ProductionWorker.ProductionWorkers.Count == 0)
            {
                MessageBox.Show("You don't have any \'Production Workers\' to print");
                return;
            }
            docPrintWorkers.Print();
        }

        private void docPrintWorkers_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            // event handler for print page
            // draws title and horizontal rule to each page and draws
            // all of the employees onto the document
            if (!printMore)
            {
                printerPageCtr = 0;
                printPageNum = 1;
            }
            // String to print out
            string lineStr;
            // Used to Draw Text Division Horizontal Line
            Pen bluePen = new Pen(Color.DarkBlue, 4);
            // Font definitions
            Font printFontNorm = new Font("Times New Roman", 12);
            Font printFontBold = new Font("Times New Roman", 12, FontStyle.Bold);
            // Used to Draw Table Division Lines
            Pen blackPen = new Pen(Color.Black, 2);
            // Line height definition
            float lineHeight = printFontNorm.GetHeight();
            // Page margin definition
            float xPrnLocation = e.MarginBounds.Left;
            float yPrnLocation = e.MarginBounds.Top;
            // Draw Page Number
            lineStr = "Page " + printPageNum;
            e.Graphics.DrawString(lineStr, printFontNorm, Brushes.Black, 400, e.MarginBounds.Bottom + 50);

            // Call Draw Title
            yPrnLocation = DrawTitle(e, xPrnLocation, yPrnLocation, "Employee Data: Production Workers");
            // Draw a Horizontal Division Line across page
            e.Graphics.DrawLine(bluePen, 60, yPrnLocation, 800, yPrnLocation);

            // Adjust Horizontal AND Vertical to prepare for table print
            xPrnLocation = e.MarginBounds.Left;
            yPrnLocation += lineHeight;
            while (printerPageCtr < ProductionWorker.ProductionWorkers.Count)
            {
                if (yPrnLocation > 950)
                {
                    e.HasMorePages = true;
                    printMore = true;
                    printPageNum++;
                    return;
                }
                // Print name (First Last)
                lineStr = "Employee ID: " + ProductionWorker.ProductionWorkers[printerPageCtr].EmpNum;
                e.Graphics.DrawString(lineStr, printFontBold, Brushes.Black, xPrnLocation, yPrnLocation);
                yPrnLocation += (lineHeight + 5);
                xPrnLocation += 25;
                // Print Other Info (Address/Phone/Email)
                lineStr = ("Name: " + ProductionWorker.ProductionWorkers[printerPageCtr].FullName).PadRight(70).Substring(0, 70);
                e.Graphics.DrawString(lineStr, printFontNorm, Brushes.Black, xPrnLocation, yPrnLocation);
                lineStr = "Hourly Pay Rate: " + Double.Parse(ProductionWorker.ProductionWorkers[printerPageCtr].HourlyRate).ToString("c");
                xPrnLocation += 375;
                e.Graphics.DrawString(lineStr, printFontNorm, Brushes.Black, xPrnLocation, yPrnLocation);
                yPrnLocation += (lineHeight + 5);
                xPrnLocation -= 375;
                lineStr = "Shift: " + ProductionWorker.ProductionWorkers[printerPageCtr].ShiftNum;
                e.Graphics.DrawString(lineStr, printFontNorm, Brushes.Black, xPrnLocation, yPrnLocation);
                //lineStr = "Hourly Pay Rate: " + Double.Parse(ProductionWorker.ProductionWorkers[printerPageCtr].HourlyRate).ToString("c");
                //xPrnLocation += 375;
                //e.Graphics.DrawString(lineStr, printFontNorm, Brushes.Black, xPrnLocation, yPrnLocation);
                yPrnLocation += lineHeight;

                // draw a horizontal divider
                xPrnLocation = e.MarginBounds.Left;
                lineStr = "";
                for (int j = 0; j < 65; j++)
                    lineStr += "- ";
                e.Graphics.DrawString(lineStr, printFontNorm, Brushes.ForestGreen, 80, yPrnLocation);

                // Adjust Vertical Position
                yPrnLocation += lineHeight;
                printerPageCtr++;
            }
            e.HasMorePages = false;
            printMore = false;
        }

        private void mnuFilePreviewSupervisor_Click(object sender, EventArgs e)
        {
            // creates a print preview
            if (ShiftSupervisor.ShiftSupervisors.Count == 0)
            {
                MessageBox.Show("You don't have any \'Shift Supervisors\' to print");
                return;
            }
            ((Form)dlgPpvSupervisors).WindowState = FormWindowState.Maximized;
            ((Form)dlgPpvSupervisors).BackColor = Color.Pink;
            ((Form)dlgPpvSupervisors).Font = new Font("Times New Roman", 25, FontStyle.Bold);

            if (ShiftSupervisor.ShiftSupervisors.Count < 11)
            {
                dlgPpvSupervisors.PrintPreviewControl.Columns = 1;
            }
            else if (ShiftSupervisor.ShiftSupervisors.Count < 21)
            {
                dlgPpvSupervisors.PrintPreviewControl.Columns = 2;
            }
            else if (ShiftSupervisor.ShiftSupervisors.Count < 31)
            {
                dlgPpvSupervisors.PrintPreviewControl.Columns = 3;
            }
            else if (ShiftSupervisor.ShiftSupervisors.Count < 41)
            {
                dlgPpvSupervisors.PrintPreviewControl.Columns = 2;
                dlgPpvSupervisors.PrintPreviewControl.Rows = 2;
            }
            else
            {
                dlgPpvSupervisors.PrintPreviewControl.Columns = 3;
                dlgPpvSupervisors.PrintPreviewControl.Rows = 2;
            }

            dlgPpvSupervisors.ShowDialog();
        }

        private void mnuFilePrintSupervisor_Click(object sender, EventArgs e)
        {
            // prints the page directly
            if (ShiftSupervisor.ShiftSupervisors.Count == 0)
            {
                MessageBox.Show("You don't have any \'Shift Supervisors\' to print");
                return;
            }
            docPrintSupervisors.Print();
        }

        private void docPrintSupervisors_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            // event handler for print page
            // draws title and horizontal rule to each page and draws
            // all of the employees onto the document
            if (!printMore)
            {
                printerPageCtr = 0;
                printPageNum = 1;
            }
            // String to print out
            string lineStr;
            // Used to Draw Text Division Horizontal Line
            Pen bluePen = new Pen(Color.DarkBlue, 4);
            // Font definitions
            Font printFontNorm = new Font("Times New Roman", 12);
            Font printFontBold = new Font("Times New Roman", 12, FontStyle.Bold);
            // Used to Draw Table Division Lines
            Pen blackPen = new Pen(Color.Black, 2);
            // Line height definition
            float lineHeight = printFontNorm.GetHeight();
            // Page margin definition
            float xPrnLocation = e.MarginBounds.Left;
            float yPrnLocation = e.MarginBounds.Top;
            // Draw Page Number
            lineStr = "Page " + printPageNum;
            e.Graphics.DrawString(lineStr, printFontNorm, Brushes.Black, 400, e.MarginBounds.Bottom + 50);

            // Call Draw Title
            yPrnLocation = DrawTitle(e, xPrnLocation, yPrnLocation, "Employee Data: Shift Supervisors");
            // Draw a Horizontal Division Line across page
            e.Graphics.DrawLine(bluePen, 60, yPrnLocation, 800, yPrnLocation);

            // Adjust Horizontal AND Vertical to prepare for table print
            xPrnLocation = e.MarginBounds.Left;
            yPrnLocation += lineHeight;
            while (printerPageCtr < ShiftSupervisor.ShiftSupervisors.Count)
            {
                if (yPrnLocation > 950)
                {
                    e.HasMorePages = true;
                    printMore = true;
                    printPageNum++;
                    return;
                }
                // Print name (First Last)
                lineStr = "Employee ID: " + ShiftSupervisor.ShiftSupervisors[printerPageCtr].EmpNum;
                e.Graphics.DrawString(lineStr, printFontBold, Brushes.Black, xPrnLocation, yPrnLocation);
                yPrnLocation += (lineHeight + 5);
                xPrnLocation += 25;
                // Print Other Info (Address/Phone/Email)
                lineStr = ("Name: " + ShiftSupervisor.ShiftSupervisors[printerPageCtr].FullName).PadRight(70).Substring(0, 70);
                e.Graphics.DrawString(lineStr, printFontNorm, Brushes.Black, xPrnLocation, yPrnLocation);
                lineStr = "Annual Salary: " + Double.Parse(ShiftSupervisor.ShiftSupervisors[printerPageCtr].Salary).ToString("c");
                xPrnLocation += 375;
                e.Graphics.DrawString(lineStr, printFontNorm, Brushes.Black, xPrnLocation, yPrnLocation);
                yPrnLocation += (lineHeight + 5);
                xPrnLocation -= 375;
                lineStr = "Annual Bonus: " + Double.Parse(ShiftSupervisor.ShiftSupervisors[printerPageCtr].AnnualBonus).ToString("c");
                e.Graphics.DrawString(lineStr, printFontNorm, Brushes.Black, xPrnLocation, yPrnLocation);

                yPrnLocation += lineHeight;

                // draw a horizontal divider
                xPrnLocation = e.MarginBounds.Left;
                lineStr = "";
                for (int j = 0; j < 65; j++)
                    lineStr += "- ";
                e.Graphics.DrawString(lineStr, printFontNorm, Brushes.ForestGreen, 80, yPrnLocation);

                // Adjust Vertical Position
                yPrnLocation += lineHeight;
                printerPageCtr++;
            }
            e.HasMorePages = false;
            printMore = false;
        }

        private void mnuFilePreviewLeader_Click(object sender, EventArgs e)
        {
            // creates a print preview
            if (TeamLeader.TeamLeaders.Count == 0)
            {
                MessageBox.Show("You don't have any \'Team Leaders\' to print");
                return;
            }
            ((Form)dlgPpvLeaders).WindowState = FormWindowState.Maximized;
            ((Form)dlgPpvLeaders).BackColor = Color.Pink;
            ((Form)dlgPpvLeaders).Font = new Font("Times New Roman", 25, FontStyle.Bold);

            if (TeamLeader.TeamLeaders.Count < 8)
            {
                dlgPpvLeaders.PrintPreviewControl.Columns = 1;
            }
            else if (TeamLeader.TeamLeaders.Count < 15)
            {
                dlgPpvLeaders.PrintPreviewControl.Columns = 2;
            }
            else if (TeamLeader.TeamLeaders.Count < 22)
            {
                dlgPpvLeaders.PrintPreviewControl.Columns = 3;
            }
            else if (TeamLeader.TeamLeaders.Count < 29)
            {
                dlgPpvLeaders.PrintPreviewControl.Columns = 2;
                dlgPpvLeaders.PrintPreviewControl.Rows = 2;
            }
            else
            {
                dlgPpvLeaders.PrintPreviewControl.Columns = 3;
                dlgPpvLeaders.PrintPreviewControl.Rows = 2;
            }
            dlgPpvLeaders.ShowDialog();
        }

        private void mnuFilePrintLeader_Click(object sender, EventArgs e)
        {
            // prints the page directly
            if (TeamLeader.TeamLeaders.Count == 0)
            {
                MessageBox.Show("You don't have any \'Team Leaders\' to print");
                return;
            }
            docPrintLeaders.Print();
        }

        private void docPrintLeaders_PrintPage(object sender, PrintPageEventArgs e)
        {
            // event handler for print page
            // draws title and horizontal rule to each page and draws
            // all of the employees onto the document
            if (!printMore)
            {
                printerPageCtr = 0;
                printPageNum = 1;
            }
            // String to print out
            string lineStr;
            // Used to Draw Text Division Horizontal Line
            Pen bluePen = new Pen(Color.DarkBlue, 4);
            // Font definitions
            Font printFontNorm = new Font("Times New Roman", 12);
            Font printFontBold = new Font("Times New Roman", 12, FontStyle.Bold);
            // Used to Draw Table Division Lines
            Pen blackPen = new Pen(Color.Black, 2);
            // Line height definition
            float lineHeight = printFontNorm.GetHeight();
            // Page margin definition
            float xPrnLocation = e.MarginBounds.Left;
            float yPrnLocation = e.MarginBounds.Top;
            // Draw Page Number
            lineStr = "Page " + printPageNum;
            e.Graphics.DrawString(lineStr, printFontNorm, Brushes.Black, 400, e.MarginBounds.Bottom + 50);

            // Call Draw Title
            yPrnLocation = DrawTitle(e, xPrnLocation, yPrnLocation, "Employee Data: Team Leaders");
            // Draw a Horizontal Division Line across page
            e.Graphics.DrawLine(bluePen, 60, yPrnLocation, 800, yPrnLocation);

            // Adjust Horizontal AND Vertical to prepare for table print
            xPrnLocation = e.MarginBounds.Left;
            yPrnLocation += lineHeight;
            while (printerPageCtr < TeamLeader.TeamLeaders.Count)
            {
                if (yPrnLocation > 950)
                {
                    e.HasMorePages = true;
                    printMore = true;
                    printPageNum++;
                    return;
                }
                // Print name (First Last)
                lineStr = "Employee ID: " + TeamLeader.TeamLeaders[printerPageCtr].EmpNum;
                e.Graphics.DrawString(lineStr, printFontBold, Brushes.Black, xPrnLocation, yPrnLocation);
                yPrnLocation += (lineHeight + 5);
                xPrnLocation += 25;
                // Print Other Info (Address/Phone/Email)
                lineStr = ("Name: " + TeamLeader.TeamLeaders[printerPageCtr].FullName).PadRight(70).Substring(0, 70);
                e.Graphics.DrawString(lineStr, printFontNorm, Brushes.Black, xPrnLocation, yPrnLocation);
                lineStr = "Shift: " + TeamLeader.TeamLeaders[printerPageCtr].ShiftNum;
                xPrnLocation += 375;
                e.Graphics.DrawString(lineStr, printFontNorm, Brushes.Black, xPrnLocation, yPrnLocation);
                yPrnLocation += (lineHeight + 5);
                xPrnLocation -= 375;
                lineStr = "Monthly Bonus: " + Double.Parse(TeamLeader.TeamLeaders[printerPageCtr].MonthlyBonus).ToString("c");
                e.Graphics.DrawString(lineStr, printFontNorm, Brushes.Black, xPrnLocation, yPrnLocation);
                lineStr = "Required Training Hours: " + TeamLeader.TeamLeaders[printerPageCtr].ReqTrainHrs;
                xPrnLocation += 375;
                e.Graphics.DrawString(lineStr, printFontNorm, Brushes.Black, xPrnLocation, yPrnLocation);
                yPrnLocation += (lineHeight + 5);
                xPrnLocation -= 375;
                lineStr = "Hourly Pay Rate: " + Double.Parse(TeamLeader.TeamLeaders[printerPageCtr].HourlyRate).ToString("c");
                e.Graphics.DrawString(lineStr, printFontNorm, Brushes.Black, xPrnLocation, yPrnLocation);
                lineStr = "Attended Training Hours: " + TeamLeader.TeamLeaders[printerPageCtr].AttTrainHrs;
                xPrnLocation += 375;
                e.Graphics.DrawString(lineStr, printFontNorm, Brushes.Black, xPrnLocation, yPrnLocation);

                yPrnLocation += lineHeight;

                // draw a horizontal divider
                xPrnLocation = e.MarginBounds.Left;
                lineStr = "";
                for (int j = 0; j < 65; j++)
                    lineStr += "- ";
                e.Graphics.DrawString(lineStr, printFontNorm, Brushes.ForestGreen, 80, yPrnLocation);

                // Adjust Vertical Position
                yPrnLocation += lineHeight;
                printerPageCtr++;
            }
            e.HasMorePages = false;
            printMore = false;

        }
        private void mnuFilePreviewAll_Click(object sender, EventArgs e)
        {
            // creates a print preview
            if (Employee.Employees.Count == 0)
            {
                MessageBox.Show("You don't have any \'Employees\' to print");
                return;
            }
            ((Form)dlgPpvEmployees).WindowState = FormWindowState.Maximized;
            ((Form)dlgPpvEmployees).BackColor = Color.Pink;
            ((Form)dlgPpvEmployees).Font = new Font("Times New Roman", 25, FontStyle.Bold);

            if (Employee.Employees.Count < 8)
            {
                dlgPpvEmployees.PrintPreviewControl.Columns = 1;
            }
            else if (Employee.Employees.Count < 15)
            {
                dlgPpvEmployees.PrintPreviewControl.Columns = 2;
            }
            else if (Employee.Employees.Count < 22)
            {
                dlgPpvEmployees.PrintPreviewControl.Columns = 3;
            }
            else if (Employee.Employees.Count < 29)
            {
                dlgPpvEmployees.PrintPreviewControl.Columns = 2;
                dlgPpvEmployees.PrintPreviewControl.Rows = 2;
            }
            else
            {
                dlgPpvEmployees.PrintPreviewControl.Columns = 3;
                dlgPpvEmployees.PrintPreviewControl.Rows = 2;
            }

            dlgPpvEmployees.ShowDialog();
        }

        private void mnuFilePrintAll_Click(object sender, EventArgs e)
        {
            // prints the page directly
            if (Employee.Employees.Count == 0)
            {
                MessageBox.Show("You don't have any \'Employees\' to print");
                return;
            }
            docPrintEmployees.Print();
        }

        private void docPrintEmployees_PrintPage(object sender, PrintPageEventArgs e)
        {
            // event handler for print page
            // draws title and horizontal rule to each page and draws
            // all of the employees onto the document
            if (!printMore)
            {
                printerPageCtr = 0;
                printPageNum = 1;
            }
            // String to print out
            string lineStr;
            // Used to Draw Text Division Horizontal Line
            Pen bluePen = new Pen(Color.DarkBlue, 4);
            // Font definitions
            Font printFontNorm = new Font("Times New Roman", 12);
            Font printFontBold = new Font("Times New Roman", 12, FontStyle.Bold);
            // Used to Draw Table Division Lines
            Pen blackPen = new Pen(Color.Black, 2);
            // Line height definition
            float lineHeight = printFontNorm.GetHeight();
            // Page margin definition
            float xPrnLocation = e.MarginBounds.Left;
            float yPrnLocation = e.MarginBounds.Top;
            // Draw Page Number
            lineStr = "Page " + printPageNum;
            e.Graphics.DrawString(lineStr, printFontNorm, Brushes.Black, 400, e.MarginBounds.Bottom + 50);

            // Call Draw Title
            yPrnLocation = DrawTitle(e, xPrnLocation, yPrnLocation, "Employee Data: All Employees");
            // Draw a Horizontal Division Line across page
            e.Graphics.DrawLine(bluePen, 60, yPrnLocation, 800, yPrnLocation);

            // Adjust Horizontal AND Vertical to prepare for table print
            xPrnLocation = e.MarginBounds.Left;
            yPrnLocation += lineHeight;
            while (printerPageCtr < Employee.Employees.Count)
            {
                if (yPrnLocation > 950)
                {
                    e.HasMorePages = true;
                    printMore = true;
                    printPageNum++;
                    return;
                }
                int i;
                if (Employee.Employees[printerPageCtr].WorkType == "Production Worker")
                {
                    for (i = 0; i < ProductionWorker.ProductionWorkers.Count; i++)
                        if (Employee.Employees[printerPageCtr].EmpNum == ProductionWorker.ProductionWorkers[i].EmpNum)
                            break; // saves current index as i to use below
                    // Print name (First Last)
                    lineStr = "Employee ID: " + ProductionWorker.ProductionWorkers[i].EmpNum;
                    e.Graphics.DrawString(lineStr, printFontBold, Brushes.Black, xPrnLocation, yPrnLocation);
                    yPrnLocation += (lineHeight + 5);
                    xPrnLocation += 25;
                    // Print Other Info (Address/Phone/Email)
                    lineStr = ("Name: " + ProductionWorker.ProductionWorkers[i].FullName + "  (Production Worker)").PadRight(70).Substring(0, 70);
                    e.Graphics.DrawString(lineStr, printFontNorm, Brushes.Black, xPrnLocation, yPrnLocation);
                    lineStr = "Hourly Pay Rate: " + Double.Parse(ProductionWorker.ProductionWorkers[i].HourlyRate).ToString("c");
                    xPrnLocation += 375;
                    e.Graphics.DrawString(lineStr, printFontNorm, Brushes.Black, xPrnLocation, yPrnLocation);
                    yPrnLocation += (lineHeight + 5);
                    xPrnLocation -= 375;
                    lineStr = "Shift: " + ProductionWorker.ProductionWorkers[i].ShiftNum;
                    e.Graphics.DrawString(lineStr, printFontNorm, Brushes.Black, xPrnLocation, yPrnLocation);
                }
                else if (Employee.Employees[printerPageCtr].WorkType == "Shift Supervisor")
                {
                    for (i = 0; i < ShiftSupervisor.ShiftSupervisors.Count; i++)
                        if (Employee.Employees[printerPageCtr].EmpNum == ShiftSupervisor.ShiftSupervisors[i].EmpNum)
                            break; // saves current index as i to use below

                    // Print name (First Last)
                    lineStr = "Employee ID: " + ShiftSupervisor.ShiftSupervisors[i].EmpNum;
                    e.Graphics.DrawString(lineStr, printFontBold, Brushes.Black, xPrnLocation, yPrnLocation);
                    yPrnLocation += (lineHeight + 5);
                    xPrnLocation += 25;
                    // Print Other Info (Address/Phone/Email)
                    lineStr = ("Name: " + ShiftSupervisor.ShiftSupervisors[i].FullName + "  (Shift Supervisor)").PadRight(70).Substring(0, 70);
                    e.Graphics.DrawString(lineStr, printFontNorm, Brushes.Black, xPrnLocation, yPrnLocation);
                    lineStr = "Annual Salary: " + Double.Parse(ShiftSupervisor.ShiftSupervisors[i].Salary).ToString("c");
                    xPrnLocation += 375;
                    e.Graphics.DrawString(lineStr, printFontNorm, Brushes.Black, xPrnLocation, yPrnLocation);
                    yPrnLocation += (lineHeight + 5);
                    xPrnLocation -= 375;
                    lineStr = "Annual Bonus: " + Double.Parse(ShiftSupervisor.ShiftSupervisors[i].AnnualBonus).ToString("c");
                    e.Graphics.DrawString(lineStr, printFontNorm, Brushes.Black, xPrnLocation, yPrnLocation);
                }
                else if (Employee.Employees[printerPageCtr].WorkType == "Team Leader")
                {
                    for (i = 0; i < TeamLeader.TeamLeaders.Count; i++)
                        if (Employee.Employees[printerPageCtr].EmpNum == TeamLeader.TeamLeaders[i].EmpNum)
                            break; // saves current index as i to use below

                    // Print name (First Last)
                    lineStr = "Employee ID: " + TeamLeader.TeamLeaders[i].EmpNum;
                    e.Graphics.DrawString(lineStr, printFontBold, Brushes.Black, xPrnLocation, yPrnLocation);
                    yPrnLocation += (lineHeight + 5);
                    xPrnLocation += 25;
                    // Print Other Info (Address/Phone/Email)
                    lineStr = ("Name: " + TeamLeader.TeamLeaders[i].FullName + "  (Team Leader)").PadRight(70).Substring(0, 70);
                    e.Graphics.DrawString(lineStr, printFontNorm, Brushes.Black, xPrnLocation, yPrnLocation);
                    lineStr = "Shift: " + TeamLeader.TeamLeaders[i].ShiftNum;
                    xPrnLocation += 375;
                    e.Graphics.DrawString(lineStr, printFontNorm, Brushes.Black, xPrnLocation, yPrnLocation);
                    yPrnLocation += (lineHeight + 5);
                    xPrnLocation -= 375;
                    lineStr = "Monthly Bonus: " + Double.Parse(TeamLeader.TeamLeaders[i].MonthlyBonus).ToString("c");
                    e.Graphics.DrawString(lineStr, printFontNorm, Brushes.Black, xPrnLocation, yPrnLocation);
                    lineStr = "Required Training Hours: " + TeamLeader.TeamLeaders[i].ReqTrainHrs;
                    xPrnLocation += 375;
                    e.Graphics.DrawString(lineStr, printFontNorm, Brushes.Black, xPrnLocation, yPrnLocation);
                    yPrnLocation += (lineHeight + 5);
                    xPrnLocation -= 375;
                    lineStr = "Hourly Pay Rate: " + Double.Parse(TeamLeader.TeamLeaders[i].HourlyRate).ToString("c");
                    e.Graphics.DrawString(lineStr, printFontNorm, Brushes.Black, xPrnLocation, yPrnLocation);
                    lineStr = "Attended Training Hours: " + TeamLeader.TeamLeaders[i].AttTrainHrs;
                    xPrnLocation += 375;
                    e.Graphics.DrawString(lineStr, printFontNorm, Brushes.Black, xPrnLocation, yPrnLocation);
                }
                yPrnLocation += lineHeight;

                // draw a horizontal divider
                xPrnLocation = e.MarginBounds.Left;
                lineStr = "";
                for (int j = 0; j < 65; j++)
                    lineStr += "- ";
                e.Graphics.DrawString(lineStr, printFontNorm, Brushes.ForestGreen, 80, yPrnLocation);

                // Adjust Vertical Position
                yPrnLocation += lineHeight;
                printerPageCtr++;

            }
            e.HasMorePages = false;
            printMore = false;
        }
        private Single DrawTitle(PrintPageEventArgs e, Single xPrnLocation, Single yPrnLocation, string title)
        {
            // draws title to the document

            // Font definitions
            Font printFontBig = new Font("Times New Roman", 32, FontStyle.Italic);
            Font printFontNorm = new Font("Times New Roman", 12);

            // Line height variable
            float lineHeight;

            // Print the demo title
            string lineStr = title;
            xPrnLocation -= 15; // center text
            e.Graphics.DrawString(lineStr, printFontBig, Brushes.DarkBlue, xPrnLocation, yPrnLocation);
            lineHeight = printFontNorm.GetHeight();
            yPrnLocation += lineHeight * 3;

            return yPrnLocation;
        }

        private void mnuHelpAbout_Click(object sender, EventArgs e)
        { // show about form
            frmAbout about = new frmAbout();
            about.ShowDialog();
        }

       
    }
}
