//Jeremiah Stones
//Advanced C#
//Kathy Wilganowski

// Program Specification can be found on the about page during runtime.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StonesJ_Lab10
{
    class Employee
    {
        // Class to hold BASE information for all employees
        private string _strWorkType, _strFullName, _strEmpNum, _strShiftNum, _strHourlyRate, _strSalary, _strAnnualBonus, _strMonthlyBonus, _strReqTrainHrs, _strAttTrainHrs;
        private static List<Employee> _lstEmployees = new List<Employee>();
        private static List<Employee> _lstArchive = new List<Employee>();
        private static int _intEmpCtr = 0;
        private static int _intIDCtr = 0;
        public int _intIndex;
        public Employee(int intIndex, string strWorkType, string strFullName, string strEmpNum)
        {
            _strWorkType = strWorkType;
            _strFullName = strFullName;
            _strEmpNum = strEmpNum;
            _intIndex = intIndex;
        }
        public Employee(int intIndex, string strWorkType, string strFullName, string strEmpNum, string strShiftNum, string strHourlyRate, string strSalary, string strAnnualBonus, string strMonthlyBonus, string strReqTrainHrs, string strAttTrainHrs)
        {
            _strWorkType = strWorkType;
            _strFullName = strFullName;
            _strEmpNum = strEmpNum;
            _intIndex = intIndex;
            _strShiftNum = strShiftNum;
            _strHourlyRate = strHourlyRate;
            _strSalary = strSalary;
            _strAnnualBonus = strAnnualBonus;
            _strMonthlyBonus = strMonthlyBonus;
            _strReqTrainHrs = strReqTrainHrs;
            _strAttTrainHrs = strAttTrainHrs;
        }
        public static ProductionWorker EmpToProd(Employee emp)
        {
            return new ProductionWorker(emp._intIndex, emp._strWorkType, emp._strFullName, emp._strEmpNum, emp._strShiftNum, emp._strHourlyRate, emp._strSalary, emp._strAnnualBonus, emp._strMonthlyBonus, emp._strReqTrainHrs, emp._strAttTrainHrs);
        }
        public static ShiftSupervisor EmpToSuper(Employee emp)
        {
            return new ShiftSupervisor(emp._intIndex, emp._strWorkType, emp._strFullName, emp._strEmpNum, emp._strShiftNum, emp._strHourlyRate, emp._strSalary, emp._strAnnualBonus, emp._strMonthlyBonus, emp._strReqTrainHrs, emp._strAttTrainHrs);
        }
        public static TeamLeader EmpToLead(Employee emp)
        {
            return new TeamLeader(emp._intIndex, emp._strWorkType, emp._strFullName, emp._strEmpNum, emp._strShiftNum, emp._strHourlyRate, emp._strSalary, emp._strAnnualBonus, emp._strMonthlyBonus, emp._strReqTrainHrs, emp._strAttTrainHrs);
        }
        public string WorkType
        {
            get { return _strWorkType; }
            set { _strWorkType = value; }
        }
        public string FullName
        {
            get { return _strFullName; }
            set { _strFullName = value; }
        }
        public string EmpNum
        {
            get { return _strEmpNum; }
            set { _strEmpNum = value; }
        }
        public static List<Employee> Employees
        {
            get { return _lstEmployees; }
            set { _lstEmployees = value; }
        }
        public static List<Employee> Archive
        {
            get { return _lstArchive; }
            set { _lstArchive = value; }
        }
        public int EmpIndex
        {
            get { return _intIndex; }
            set { _intIndex = value; }
        }
        public static int EmpCtr
        {
            get { return _intEmpCtr; }
            set { _intEmpCtr = value; }
        }
        public static int IDCtr
        {
            get { return _intIDCtr; }
            set { _intIDCtr = value; }
        }
        public string ShiftNum
        {
            get { return _strShiftNum; }
            set { _strShiftNum = value; }
        }
        public string HourlyRate
        {
            get { return _strHourlyRate; }
            set { _strHourlyRate = value; }
        }
        public string Salary
        {
            get { return _strSalary; }
            set { _strSalary = value; }
        }
        public string AnnualBonus
        {
            get { return _strAnnualBonus; }
            set { _strAnnualBonus = value; }
        }
        public string MonthlyBonus
        {
            get { return _strMonthlyBonus; }
            set { _strMonthlyBonus = value; }
        }
        public string ReqTrainHrs
        {
            get { return _strReqTrainHrs; }
            set { _strReqTrainHrs = value; }
        }
        public string AttTrainHrs
        {
            get { return _strAttTrainHrs; }
            set { _strAttTrainHrs = value; }
        }

    }
}
