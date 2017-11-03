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
    class ShiftSupervisor : Employee
    { // definition of shift supervisor class with private members, public properties, and a public constructor with multiple parameters

        private static List<ShiftSupervisor> _lstShiftSupervisors = new List<ShiftSupervisor>();
        private static int _intSuperCtr = 0;
        public ShiftSupervisor(int intIndex, string strWorkType, string strFullName, string strEmpNum, string strShiftNum, string strHourlyRate, string strSalary, string strAnnualBonus, string strMonthlyBonus, string strReqTrainHrs, string strAttTrainHrs) : base(intIndex, strWorkType, strFullName, strEmpNum, strShiftNum, strHourlyRate, strSalary, strAnnualBonus, strMonthlyBonus, strReqTrainHrs, strAttTrainHrs)
        {
        }
        public static List<ShiftSupervisor> ShiftSupervisors
        {
            get { return _lstShiftSupervisors; }
            set { _lstShiftSupervisors = value; }
        }
        public static int SuperCtr
        {
            get { return _intSuperCtr; }
            set { _intSuperCtr = value; }
        }
    }
}
