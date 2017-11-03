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
    class TeamLeader : ProductionWorker
    { // definition of team leader class with private members, public properties, and a public constructor with multiple parameters

        private static List<TeamLeader> _lstTeamLeaders = new List<TeamLeader>();
        private static int _intLeaderCtr = 0;
        public TeamLeader(int intIndex, string strWorkType, string strFullName, string strEmpNum, string strShiftNum, string strHourlyRate, string strSalary, string strAnnualBonus, string strMonthlyBonus, string strReqTrainHrs, string strAttTrainHrs) : base(intIndex, strWorkType, strFullName, strEmpNum, strShiftNum, strHourlyRate, strSalary, strAnnualBonus, strMonthlyBonus, strReqTrainHrs, strAttTrainHrs)
        {
        }
        public static List<TeamLeader> TeamLeaders
        {
            get { return _lstTeamLeaders; }
            set { _lstTeamLeaders = value; }
        }
        public static int LeaderCtr
        {
            get { return _intLeaderCtr; }
            set { _intLeaderCtr = value; }
        }

    }
}
