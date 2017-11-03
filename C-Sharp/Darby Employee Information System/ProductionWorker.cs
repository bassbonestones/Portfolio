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
    class ProductionWorker : Employee
    { // definition of production worker class with private members, public properties, and a public constructor with multiple parameters

        private static List<ProductionWorker> _lstProductionWorkers = new List<ProductionWorker>();
        private static int _intWorkerCtr = 0;
        public ProductionWorker(int intIndex, string strWorkType, string strFullName, string strEmpNum, string strShiftNum, string strHourlyRate, string strSalary, string strAnnualBonus, string strMonthlyBonus, string strReqTrainHrs, string strAttTrainHrs) : base(intIndex, strWorkType, strFullName, strEmpNum, strShiftNum, strHourlyRate, strSalary, strAnnualBonus, strMonthlyBonus, strReqTrainHrs, strAttTrainHrs)
        {
        }
        public static List<ProductionWorker> ProductionWorkers
        {
            get { return _lstProductionWorkers; }
            set { _lstProductionWorkers = value; }
        }
        public static int WorkerCtr
        {
            get { return _intWorkerCtr; }
            set { _intWorkerCtr = value; }
        }
    }
}
