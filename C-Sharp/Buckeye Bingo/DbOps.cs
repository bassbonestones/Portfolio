using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stones_Final_Project
{

    #region DatabaseClasses
    class Charity
    {
        // structure to hold the attributes of a single Charity
        public string TaxID;
        public string Name;
        // static
        public static Charity CurrentCharity;
    }
    class Machine
    {
        // structure to hold the attributes of a single Machine
        public Int32 ElectronicsID;
        public Int32 TemplateID;
        public string Description;
        // attributes to use at runtime (not database stored)
        public int QtySold = 0;
        public double IndividualRevenue = 0;
        // total for all machines
        public static double TotalRevenue = 0;
        public static List<Machine> CurrentMachines = new List<Machine>();
    }
    class FloorCard
    {
        // structure to hold the attributes of a single FloorCard
        public Int32 CardID;
        public Int32 TemplateID;
        public string Description;
        public double Price;
        public Int32 Quantity1;
        public Int32 Quantity2;
        public Int32 Quantity3;
        public Int32 Quantity4;
        public Int32 Quantity5;
        public Int32 GameID1;
        public Int32 GameID2;
        public Int32 GameID3;
        // Color separated to store and retreive from database
        public Int32 ColorRed;
        public Int32 ColorGreen;
        public Int32 ColorBlue;
        // Color aggregated to use in program
        public Color BackColor;
        // attributes to use at runtime (not database stored)
        public int QtySerials = 1;
        public string Serial1 = "";
        public string Serial2 = "";
        // static
        public static List<FloorCard> CurrentFloorCards = new List<FloorCard>();
    }
    class PaperSet
    {
        // structure to hold the attributes of a single PaperSet
        public Int32 PaperSetID;
        public Int32 TemplateID;
        public string Description;
        public double Price;
        public bool RedemptionAvailable;
        public double RedemptionPrice;
        // attributes to use at runtime (not database stored)
        public int QtySerials = 1;
        public string Serial1 = "";
        public string Serial2 = "";
        public int QtyIssued1 = 0;
        public int QtyIssued2 = 0;
        public int QtyReturned1 = 0;
        public int QtyReturned2 = 0;
        public int StartNum1 = 0;
        public int StartNum2 = 0;
        public int EndNum1 = 0;
        public int EndNum2 = 0;
        public int QtySold1 = 0;
        public int QtySold2 = 0;
        public double Gross1 = 0;
        public double Gross2 = 0;
        // static variable
        public static int QtyPaperSets = 0;
        public static List<PaperSet> CurrentPaperSets = new List<PaperSet>();
    }
    class PullTab
    {
        // structure to hold the attributes of a single PullTab
        public string FormID = "";
        public string Description = "";
        public double Gross = 0.0;
        public double Prizes = 0.0;
        public double Tax = 0.0;
        public bool Current = true;
        // attributes to use at runtime (not database stored)
        public int QtyWorker1 = 0;
        public int QtyWorker2 = 0;
        public int QtyWorker3 = 0;
        public int QtyWorker4 = 0;
        public int QtyWorker5 = 0;
        public int QtyAllWorkers = 0;
        public string Serial = "";
        // static variables
        public static int TotalGross = 0;
        public static int TotalPrizes = 0;
        public static int TotalOut = 0;
        public static List<PullTab> CurrentPullTabs = new List<PullTab>();
    }
    class SessionBingoGame
    {
        // structure to hold the attributes of a single BingoSessionGame
        public Int32 GameID;
        public Int32 TemplateID;
        public Int32 GameNumber;
        public string Description;
        public double MaxOrSetPrice;
        public Int32 CardID1;
        public Int32 CardID2;
        public Int32 CardID3;
        public bool EarlyBird;
        public bool ChangePaidOut;
        public bool FiftyFifty;
        // Color separated for database storage/retrieval
        public Int32 ColorRed;
        public Int32 ColorGreen;
        public Int32 ColorBlue;
        // Color aggregated for use in program;
        public Color BackColor;
        // attributes to use at runtime (no database storage)
        public int QtyWinners = 0;
        // total winners
        public static int TotalQtyWinners = 0;
        public static List<SessionBingoGame> CurrentSessionBingoGames = new List<SessionBingoGame>();
    }
    class SessionTemplate
    {
        // structure to hold the attributes of a single SessionTemplate
        public Int32 TemplateID;
        public string TemplateName;
        // static
        public static SessionTemplate CurrentTemplate;
    }
    class Worker
    {
        public string Name;
        // static 
        public static List<Worker> CurrentWorkersList = new List<Worker>();
        public static Worker CurrentPTDeskWorker;
        public static Worker CurrentDeskWorker;
        public Worker() { }
        public Worker(string n) { Name = n; }
    }
    #endregion DatabaseClasses

     class DbOps
    {
        // This class is to perform all database operations.  Capability for both access and
        // MSSQL are included.  
        #region PrivateVariables
        // boolean variable to determine which database to use
        private static string _whichDatabase = "Access";
        // attendance variable
        private static int _attendance = 0;
        // what is today?
        private static string _today = DateTime.Now.ToShortDateString();
        private static DayOfWeek _dayOfWeek = DateTime.Now.DayOfWeek;
        private static TimeSpan _timeOfDay = DateTime.Now.TimeOfDay;
        // variables for current admin password, master password and temporary password
        private static string _currentPassword;
        private static string _masterPassword;
        private static string _tempPassword;
        // variables for other defaults
        private static string _email;
        private static Int32 _numFloorWorkers;
        private static bool _startupWizard;
        private static Int32 _templateID;
        private static double _taxRate;
        // variables for holding Charities, Machines, FloorCards, PaperSets, PullTabs, SessionBingoGames, 
        // SessionTemplates, and Workers
        private static List<Charity> _charities = new List<Charity>();
        private static List<Machine> _machines = new List<Machine>();
        private static List<FloorCard> _floorCards = new List<FloorCard>();
        private static List<PaperSet> _paperSets = new List<PaperSet>();
        private static List<PullTab> _pullTabs = new List<PullTab>();
        private static List<SessionBingoGame> _sessionBingoGames = new List<SessionBingoGame>();
        private static List<SessionTemplate> _sessionTemplates = new List<SessionTemplate>();
        private static List<Worker> _workers = new List<Worker>();
        // connection strings
        private const string CONNECT_ACCESS = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=..\\..\\Data\\BuckeyeBingo.mdb";
        private const string CONNECT_MSSQL = "Data Source=********;User ID=********;Password=********";
        // connections
        private static OleDbConnection _conOleDb = new OleDbConnection(CONNECT_ACCESS);
        private static SqlConnection _conSql = new SqlConnection(CONNECT_MSSQL);
        // dataAdapters
        private static OleDbDataAdapter _adapterOleDb = new OleDbDataAdapter();
        private static SqlDataAdapter _adapterSql = new SqlDataAdapter();
        // dataset variables
            // program default options
        private static DataSet _dsDefaults = new DataSet();
        private const string _dtDefaults = "Defaults";
            // pull tabs (current and archived by boolean)
        private static DataSet _dsPullTabs = new DataSet();
        private const string _dtPullTabs = "PullTabs";
            // workers
        private static DataSet _dsWorkers = new DataSet();
        private const string _dtWorkers = "Workers";
            // session bingo games
        private static DataSet _dsSessionBingoGames = new DataSet();
        private const string _dtSessionBingoGames = "SessionBingoGames";
            // taxrate - single value table - global database variable
        private static DataSet _dsTaxRate = new DataSet();
        private const string _dtTaxRate = "TaxRate";
            // charity organizations
        private static DataSet _dsCharities = new DataSet();
        private static string _dtCharities = "Charities";
            // electronics
        private static DataSet _dsElectronics = new DataSet();
        private static string _dtElectronics = "Electronics";
            // session templates
        private static DataSet _dsSessionTemplates = new DataSet();
        private static string _dtSessionTemplates = "SessionTemplates";
            // floor sales cards
        private static DataSet _dsFloorCards = new DataSet();
        private static string _dtFloorCards = "FloorCards";
        // paper sets
        private static DataSet _dsPaperSets = new DataSet();
        private static string _dtPaperSets = "PaperSets";
        #endregion PrivateVariables
        #region Properties
        public static string WhichDatabase
        {
            get
            {
                return _whichDatabase;
            }

            set
            {
                _whichDatabase = value;
            }
        }
        public static string CurrentPassword
        {
            get
            {
                return _currentPassword;
            }

            set
            {
                _currentPassword = value;
            }
        }
        public static string TempPassword
        {
            get
            {
                return _tempPassword;
            }

            set
            {
                _tempPassword = value;
            }
        }
        public static string Email
        {
            get
            {
                return _email;
            }

            set
            {
                _email = value;
            }
        }
        public static string MasterPassword
        {
            get
            {
                return _masterPassword;
            }

            set
            {
                _masterPassword = value;
            }
        }
        public static int NumFloorWorkers
        {
            get
            {
                return _numFloorWorkers;
            }

            set
            {
                _numFloorWorkers = value;
            }
        }
        public static bool StartupWizard
        {
            get
            {
                return _startupWizard;
            }

            set
            {
                _startupWizard = value;
            }
        }
        public static int TemplateID
        {
            get
            {
                return _templateID;
            }

            set
            {
                _templateID = value;
            }
        }
        public static double TaxRate
        {
            get
            {
                return _taxRate;
            }

            set
            {
                _taxRate = value;
            }
        }
        internal static List<Charity> Charities
        {
            get
            {
                return _charities;
            }

            set
            {
                _charities = value;
            }
        }
        internal static List<Machine> Machines
        {
            get
            {
                return _machines;
            }

            set
            {
                _machines = value;
            }
        }
        internal static List<FloorCard> FloorCards
        {
            get
            {
                return _floorCards;
            }

            set
            {
                _floorCards = value;
            }
        }
        internal static List<PaperSet> PaperSets
        {
            get
            {
                return _paperSets;
            }

            set
            {
                _paperSets = value;
            }
        }
        internal static List<PullTab> PullTabs
        {
            get
            {
                return _pullTabs;
            }

            set
            {
                _pullTabs = value;
            }
        }
        internal static List<SessionBingoGame> SessionBingoGames
        {
            get
            {
                return _sessionBingoGames;
            }

            set
            {
                _sessionBingoGames = value;
            }
        }
        internal static List<SessionTemplate> SessionTemplates
        {
            get
            {
                return _sessionTemplates;
            }

            set
            {
                _sessionTemplates = value;
            }
        }
        internal static List<Worker> Workers
        {
            get
            {
                return _workers;
            }

            set
            {
                _workers = value;
            }
        }
        public static DataSet DsPaperSets
        {
            get
            {
                return _dsPaperSets;
            }

            set
            {
                _dsPaperSets = value;
            }
        }
        public static string DtPaperSets
        {
            get
            {
                return _dtPaperSets;
            }

            set
            {
                _dtPaperSets = value;
            }
        }
        public static DataSet DsDefaults
        {
            get
            {
                return _dsDefaults;
            }

            set
            {
                _dsDefaults = value;
            }
        }
        public static string DtDefaults
        {
            get
            {
                return _dtDefaults;
            }
        }
        public static DataSet DsPullTabs
        {
            get
            {
                return _dsPullTabs;
            }

            set
            {
                _dsPullTabs = value;
            }
        }
        public static string DtPullTabs
        {
            get
            {
                return _dtPullTabs;
            }
        }
        public static DataSet DsWorkers
        {
            get
            {
                return _dsWorkers;
            }

            set
            {
                _dsWorkers = value;
            }
        }
        public static string DtWorkers
        {
            get
            {
                return _dtWorkers;
            }
        }
        public static DataSet DsSessionBingoGames
        {
            get
            {
                return _dsSessionBingoGames;
            }

            set
            {
                _dsSessionBingoGames = value;
            }
        }
        public static string DtSessionBingoGames
        {
            get
            {
                return _dtSessionBingoGames;
            }
        }
        public static DataSet DsTaxRate
        {
            get
            {
                return _dsTaxRate;
            }

            set
            {
                _dsTaxRate = value;
            }
        }
        public static string DtTaxRate
        {
            get
            {
                return _dtTaxRate;
            }
        }
        public static DataSet DsCharities
        {
            get
            {
                return _dsCharities;
            }

            set
            {
                _dsCharities = value;
            }
        }
        public static string DtCharities
        {
            get
            {
                return _dtCharities;
            }

            set
            {
                _dtCharities = value;
            }
        }
        public static DataSet DsElectronics
        {
            get
            {
                return _dsElectronics;
            }

            set
            {
                _dsElectronics = value;
            }
        }
        public static string DtElectronics
        {
            get
            {
                return _dtElectronics;
            }

            set
            {
                _dtElectronics = value;
            }
        }
        public static DataSet DsSessionTemplates
        {
            get
            {
                return _dsSessionTemplates;
            }

            set
            {
                _dsSessionTemplates = value;
            }
        }
        public static string DtSessionTemplates
        {
            get
            {
                return _dtSessionTemplates;
            }

            set
            {
                _dtSessionTemplates = value;
            }
        }
        public static DataSet DsFloorCards
        {
            get
            {
                return _dsFloorCards;
            }

            set
            {
                _dsFloorCards = value;
            }
        }
        public static string DtFloorCards
        {
            get
            {
                return _dtFloorCards;
            }

            set
            {
                _dtFloorCards = value;
            }
        }
        public static string Today
        {
            get
            {
                return _today;
            }

            set
            {
                _today = value;
            }
        }
        public static DayOfWeek DayOfWeek
        {
            get
            {
                return _dayOfWeek;
            }

            set
            {
                _dayOfWeek = value;
            }
        }
        public static TimeSpan TimeOfDay
        {
            get
            {
                return _timeOfDay;
            }

            set
            {
                _timeOfDay = value;
            }
        }
        public static int Attendance
        {
            get
            {
                return _attendance;
            }

            set
            {
                _attendance = value;
            }
        }
        #endregion Properties
        #region FillMethods
        // methods for filling DataSets from database and pulling info into program
        public static void FillDefaultsTable()
        {
            // method to fill the Defaults table with information from a database

            if(_whichDatabase == "Access")
            {
                // if the database is the Access database
                using (OleDbCommand command = new OleDbCommand())
                {
                    // using a disposable command query the defaults table
                    command.Connection = _conOleDb;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT * FROM Defaults";
                    _adapterOleDb.SelectCommand = command;
                    try
                    {
                        // open database connection, clear the dataset, and fill the defaults table
                        _conOleDb.Open();
                        DsDefaults.Clear();
                        _adapterOleDb.Fill(DsDefaults, DtDefaults);
                        _currentPassword = DsDefaults.Tables[DtDefaults].Rows[0][1].ToString();
                        _email = DsDefaults.Tables[DtDefaults].Rows[1][1].ToString();
                        _masterPassword = DsDefaults.Tables[DtDefaults].Rows[2][1].ToString();
                        _numFloorWorkers = (Int32)(int.Parse(DsDefaults.Tables[DtDefaults].Rows[3][1].ToString()));
                        string strStartupWizard = DsDefaults.Tables[DtDefaults].Rows[4][1].ToString().ToLower();
                        if (strStartupWizard == "false")
                            _startupWizard = false;
                        else
                            _startupWizard = true;
                        _taxRate = double.Parse(DsDefaults.Tables[DtDefaults].Rows[5][1].ToString());
                        _templateID = int.Parse(DsDefaults.Tables[DtDefaults].Rows[6][1].ToString());
                    }
                    catch(Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("Access fill of Defaults table failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conOleDb.Close();
                    }
                }
            }
            else if (_whichDatabase == "MSSQL")
            {
                // if the database is the MSSQL database
                using (SqlCommand command = new SqlCommand())
                {
                    // using a disposable command query the defaults table
                    command.Connection = _conSql;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT * FROM Defaults";
                    _adapterSql.SelectCommand = command;
                    try
                    {
                        // open database connection, clear the dataset, and fill the defaults table
                        _conSql.Open();
                        DsDefaults.Clear();
                        _adapterSql.Fill(DsDefaults, DtDefaults);
                        _currentPassword = DsDefaults.Tables[DtDefaults].Rows[0][1].ToString();
                        _email = DsDefaults.Tables[DtDefaults].Rows[1][1].ToString();
                        _masterPassword = DsDefaults.Tables[DtDefaults].Rows[2][1].ToString();
                        _numFloorWorkers = (Int32)(int.Parse(DsDefaults.Tables[DtDefaults].Rows[3][1].ToString()));
                        string strStartupWizard = DsDefaults.Tables[DtDefaults].Rows[4][1].ToString();
                        if (strStartupWizard == "0")
                            _startupWizard = false;
                        else
                            _startupWizard = true;
                        _taxRate = double.Parse(DsDefaults.Tables[DtDefaults].Rows[5][1].ToString());
                        _templateID = int.Parse(DsDefaults.Tables[DtDefaults].Rows[6][1].ToString());
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("MSSQL fill of Defaults table failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conSql.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Defaults table not filled. No database type specified.");
            }
        }
        public static void FillCharitiesTable()
        {
            // method to fill the Charities table with information from a database

            if (_whichDatabase == "Access")
            {
                // if the database is the Access database
                using (OleDbCommand command = new OleDbCommand())
                {
                    // using a disposable command query the charities table
                    command.Connection = _conOleDb;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT * FROM Charities";
                    _adapterOleDb.SelectCommand = command;
                    try
                    {
                        // open database connection, clear the dataset, and fill the Charities table
                        _conOleDb.Open();
                        DsCharities.Clear();
                        Charities.Clear();
                        _adapterOleDb.Fill(DsCharities, DtCharities);
                        foreach (DataRow row in DsCharities.Tables[DtCharities].Rows)
                        {
                            // fill Charities list from DataSet
                            Charity tempCharity = new Charity();
                            tempCharity.TaxID = row[0].ToString();
                            tempCharity.Name = row[1].ToString();
                            Charities.Add(tempCharity);
                        }
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("Access fill of Charities table failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conOleDb.Close();
                    }
                }
            }
            else if (_whichDatabase == "MSSQL")
            {
                // if the database is the MSSQL database
                using (SqlCommand command = new SqlCommand())
                {
                    // using a disposable command query the charities table
                    command.Connection = _conSql;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT * FROM Charities";
                    _adapterSql.SelectCommand = command;
                    try
                    {
                        // open database connection, clear the dataset, and fill the Charities table
                        _conSql.Open();
                        DsCharities.Clear();
                        Charities.Clear();
                        _adapterSql.Fill(DsCharities, DtCharities);
                        foreach(DataRow row in DsCharities.Tables[DtCharities].Rows)
                        {
                            // fill Charities list from DataSet
                            Charity tempCharity = new Charity();
                            tempCharity.TaxID = row[0].ToString();
                            tempCharity.Name = row[1].ToString();
                            Charities.Add(tempCharity);                            
                        }
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("MSSQL fill of Charities table failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conSql.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Charities table not filled. No database type specified.");
            }
        }
        public static void FillElectronicsTable(Int32 p_TemplateID)
        {
            // method to fill the Electronics table with information from a database

            if (_whichDatabase == "Access")
            {
                // if the database is the Access database
                using (OleDbCommand command = new OleDbCommand())
                {
                    // using a disposable command query the electronics table
                    command.Connection = _conOleDb;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT * FROM Electronics WHERE TemplateID = ?";
                    command.Parameters.AddWithValue("?", p_TemplateID);
                    _adapterOleDb.SelectCommand = command;
                    try
                    {
                        // open database connection, clear the dataset, and fill the electronics table
                        _conOleDb.Open();
                        DsElectronics.Clear();
                        Machine.CurrentMachines.Clear();
                        _adapterOleDb.Fill(DsElectronics, DtElectronics);
                        foreach (DataRow row in DsElectronics.Tables[DtElectronics].Rows)
                        {
                            // fill Machines list from DataSet
                            Machine tempMachine = new Machine();
                            tempMachine.ElectronicsID = int.Parse(row[0].ToString());
                            tempMachine.TemplateID = p_TemplateID;
                            tempMachine.Description = row[2].ToString();
                            Machine.CurrentMachines.Add(tempMachine);
                        }
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("Access fill of Electronics table failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conOleDb.Close();
                    }
                }
            }
            else if (_whichDatabase == "MSSQL")
            {
                // if the database is the MSSQL database
                using (SqlCommand command = new SqlCommand())
                {
                    // using a disposable command query the electronics table
                    command.Connection = _conSql;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT * FROM Electronics WHERE TemplateID = @templateID";
                    command.Parameters.AddWithValue("@templateID", p_TemplateID);
                    _adapterSql.SelectCommand = command;
                    try
                    {
                        // open database connection, clear the dataset, and fill the Electronics table
                        _conSql.Open();
                        DsElectronics.Clear();
                        Machine.CurrentMachines.Clear();
                        _adapterSql.Fill(DsElectronics, DtElectronics);
                        foreach (DataRow row in DsElectronics.Tables[DtElectronics].Rows)
                        {
                            // fill Machines list from DataSet
                            Machine tempMachine = new Machine();
                            tempMachine.ElectronicsID = int.Parse(row[0].ToString());
                            tempMachine.TemplateID = p_TemplateID;
                            tempMachine.Description = row[2].ToString();
                            Machine.CurrentMachines.Add(tempMachine);
                        }
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("MSSQL fill of Electronics table failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conSql.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Electronics table not filled. No database type specified.");
            }
        }
        public static void FillFloorCardsTable(Int32 p_TemplateID)
        {
            // method to fill the FloorCards table with information from a database

            if (_whichDatabase == "Access")
            {
                // if the database is the Access database
                using (OleDbCommand command = new OleDbCommand())
                {
                    // using a disposable command query the floorcards table
                    command.Connection = _conOleDb;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT * FROM FloorCards WHERE TemplateID = ?";
                    command.Parameters.AddWithValue("?", p_TemplateID);
                    _adapterOleDb.SelectCommand = command;
                    try
                    {
                        // open database connection, clear the dataset, and fill the floorcards table
                        _conOleDb.Open();
                        DsFloorCards.Clear();
                        FloorCard.CurrentFloorCards.Clear();
                        _adapterOleDb.Fill(DsFloorCards, DtFloorCards);
                        foreach (DataRow row in DsFloorCards.Tables[DtFloorCards].Rows)
                        {
                            // fill FloorCards list from DataSet
                            FloorCard tempFloorCard = new FloorCard();
                            tempFloorCard.CardID = int.Parse(row[0].ToString());
                            tempFloorCard.TemplateID = p_TemplateID;
                            tempFloorCard.Description = row[2].ToString();
                            tempFloorCard.Price = double.Parse(row[3].ToString());
                            tempFloorCard.Quantity1 = int.Parse(row[4].ToString());
                            tempFloorCard.Quantity2 = int.Parse(row[5].ToString());
                            tempFloorCard.Quantity3 = int.Parse(row[6].ToString());
                            tempFloorCard.Quantity4 = int.Parse(row[7].ToString());
                            tempFloorCard.Quantity5 = int.Parse(row[8].ToString());
                            tempFloorCard.GameID1 = int.Parse(row[9].ToString());
                            tempFloorCard.GameID2 = int.Parse(row[10].ToString());
                            tempFloorCard.GameID3 = int.Parse(row[11].ToString());
                            tempFloorCard.ColorRed = int.Parse(row[12].ToString());
                            tempFloorCard.ColorGreen = int.Parse(row[13].ToString());
                            tempFloorCard.ColorBlue = int.Parse(row[14].ToString());
                            tempFloorCard.BackColor = Color.FromArgb(tempFloorCard.ColorRed, tempFloorCard.ColorGreen, tempFloorCard.ColorBlue);
                            FloorCard.CurrentFloorCards.Add(tempFloorCard);
                        }
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("Access fill of FloorCards table failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conOleDb.Close();
                    }
                }
            }
            else if (_whichDatabase == "MSSQL")
            {
                // if the database is the MSSQL database
                using (SqlCommand command = new SqlCommand())
                {
                    // using a disposable command query the floorcards table
                    command.Connection = _conSql;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT * FROM FloorCards WHERE TemplateID = @templateID";
                    command.Parameters.AddWithValue("@templateID", p_TemplateID);
                    _adapterSql.SelectCommand = command;
                    try
                    {
                        // open database connection, clear the dataset, and fill the floorcards table
                        _conSql.Open();
                        DsFloorCards.Clear();
                        FloorCard.CurrentFloorCards.Clear();
                        _adapterSql.Fill(DsFloorCards, DtFloorCards);
                        foreach (DataRow row in DsFloorCards.Tables[DtFloorCards].Rows)
                        {
                            // fill FloorCards list from DataSet
                            FloorCard tempFloorCard = new FloorCard();
                            tempFloorCard.CardID = int.Parse(row[0].ToString());
                            tempFloorCard.TemplateID = p_TemplateID;
                            tempFloorCard.Description = row[2].ToString();
                            tempFloorCard.Price = double.Parse(row[3].ToString());
                            tempFloorCard.Quantity1 = int.Parse(row[4].ToString());
                            tempFloorCard.Quantity2 = int.Parse(row[5].ToString());
                            tempFloorCard.Quantity3 = int.Parse(row[6].ToString());
                            tempFloorCard.Quantity4 = int.Parse(row[7].ToString());
                            tempFloorCard.Quantity5 = int.Parse(row[8].ToString());
                            tempFloorCard.GameID1 = int.Parse(row[9].ToString());
                            tempFloorCard.GameID2 = int.Parse(row[10].ToString());
                            tempFloorCard.GameID3 = int.Parse(row[11].ToString());
                            tempFloorCard.ColorRed = int.Parse(row[12].ToString());
                            tempFloorCard.ColorGreen = int.Parse(row[13].ToString());
                            tempFloorCard.ColorBlue = int.Parse(row[14].ToString());
                            tempFloorCard.BackColor = Color.FromArgb(tempFloorCard.ColorRed, tempFloorCard.ColorGreen, tempFloorCard.ColorBlue);
                            FloorCard.CurrentFloorCards.Add(tempFloorCard);
                        }
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("MSSQL fill of FloorCards table failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conSql.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("FloorCards table not filled. No database type specified.");
            }
        }
        public static void FillPaperSetsTable(Int32 p_TemplateID)
        {
            // method to fill the PaperSets table with information from a database

            if (_whichDatabase == "Access")
            {
                // if the database is the Access database
                using (OleDbCommand command = new OleDbCommand())
                {
                    // using a disposable command query the papersets table
                    command.Connection = _conOleDb;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT * FROM PaperSets WHERE TemplateID = ?";
                    command.Parameters.AddWithValue("?", p_TemplateID);
                    _adapterOleDb.SelectCommand = command;
                    try
                    {
                        // open database connection, clear the dataset, and fill the papersets table
                        _conOleDb.Open();
                        _dsPaperSets.Clear();
                        PaperSet.CurrentPaperSets.Clear();
                        PaperSet.QtyPaperSets = 0;
                        _adapterOleDb.Fill(_dsPaperSets, _dtPaperSets);
                        foreach (DataRow row in _dsPaperSets.Tables[_dtPaperSets].Rows)
                        {
                            // fill PaperSets list from DataSet
                            PaperSet tempPaperSet = new PaperSet();
                            tempPaperSet.PaperSetID = int.Parse(row[0].ToString());
                            tempPaperSet.TemplateID = p_TemplateID;
                            tempPaperSet.Description = row[2].ToString();
                            tempPaperSet.Price = double.Parse(row[3].ToString());
                            string strRedemptionAvailable = row[4].ToString().ToLower();
                            if (strRedemptionAvailable == "false")
                                tempPaperSet.RedemptionAvailable = false;
                            else
                            {
                                tempPaperSet.RedemptionAvailable = true;
                                tempPaperSet.RedemptionPrice = double.Parse(row[5].ToString());
                            }
                            PaperSet.CurrentPaperSets.Add(tempPaperSet);
                            PaperSet.QtyPaperSets++;
                        }
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("Access fill of PaperSets table failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conOleDb.Close();
                    }
                }
            }
            else if (_whichDatabase == "MSSQL")
            {
                // if the database is the MSSQL database
                using (SqlCommand command = new SqlCommand())
                {
                    // using a disposable command query the papersets table
                    command.Connection = _conSql;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT * FROM PaperSets WHERE TemplateID = @templateID";
                    command.Parameters.AddWithValue("@templateID", p_TemplateID);
                    _adapterSql.SelectCommand = command;
                    try
                    {
                        // open database connection, clear the dataset, and fill the papersets table
                        _conSql.Open();
                        _dsPaperSets.Clear();
                        PaperSet.CurrentPaperSets.Clear();
                        PaperSet.QtyPaperSets = 0;
                        _adapterSql.Fill(_dsPaperSets, _dtPaperSets);
                        foreach (DataRow row in _dsPaperSets.Tables[_dtPaperSets].Rows)
                        {
                            // fill PaperSets list from DataSet
                            PaperSet tempPaperSet = new PaperSet();
                            tempPaperSet.PaperSetID = int.Parse(row[0].ToString());
                            tempPaperSet.TemplateID = p_TemplateID;
                            tempPaperSet.Description = row[2].ToString();
                            tempPaperSet.Price = double.Parse(row[3].ToString());
                            string strRedemptionAvailable = row[4].ToString().ToLower();
                            if (strRedemptionAvailable == "false")
                            {
                                tempPaperSet.RedemptionAvailable = false;
                                tempPaperSet.RedemptionPrice = 0.0;
                            }
                            else
                            {
                                tempPaperSet.RedemptionAvailable = true;
                                tempPaperSet.RedemptionPrice = double.Parse(row[5].ToString());
                            }
                            PaperSet.CurrentPaperSets.Add(tempPaperSet);
                            PaperSet.QtyPaperSets++;
                        }
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("MSSQL fill of PaperSets table failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conSql.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("PaperSets table not filled. No database type specified.");
            }
        }
        public static void FillPullTabsTable()
        {
            // method to fill the PullTabs table with information from a database

            if (_whichDatabase == "Access")
            {
                // if the database is the Access database
                using (OleDbCommand command = new OleDbCommand())
                {
                    // using a disposable command query the pulltabs table
                    command.Connection = _conOleDb;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT * FROM PullTabs WHERE [Current] = True ORDER BY Description";
                    _adapterOleDb.SelectCommand = command;
                    try
                    {
                        // open database connection, clear the dataset, and fill the PullTabs table
                        _conOleDb.Open();
                        DsPullTabs.Clear();
                        PullTabs.Clear();
                        _adapterOleDb.Fill(DsPullTabs, DtPullTabs);
                        foreach (DataRow row in DsPullTabs.Tables[DtPullTabs].Rows)
                        {
                            // fill PullTabs list from DataSet
                            PullTab tempPullTab = new PullTab();
                            tempPullTab.FormID = row[0].ToString();
                            tempPullTab.Description = row[1].ToString();
                            tempPullTab.Gross = double.Parse(row[2].ToString());
                            tempPullTab.Prizes = double.Parse(row[3].ToString());
                            tempPullTab.Tax = double.Parse(row[4].ToString());
                            string strCurrent = row[5].ToString().ToLower().ToLower();
                            if (strCurrent == "false")
                                tempPullTab.Current = false;
                            else
                                tempPullTab.Current = true;
                            PullTabs.Add(tempPullTab);
                        }
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("Access fill of PullTabs table failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conOleDb.Close();
                    }
                }
            }
            else if (_whichDatabase == "MSSQL")
            {
                // if the database is the MSSQL database
                using (SqlCommand command = new SqlCommand())
                {
                    // using a disposable command query the pulltabs table
                    command.Connection = _conSql;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT * FROM PullTabs WHERE [IsCurrent] = 1 ORDER BY Name";
                    _adapterSql.SelectCommand = command;
                    try
                    {
                        // open database connection, clear the dataset, and fill the PullTabs table
                        _conSql.Open();
                        DsPullTabs.Clear();
                        PullTabs.Clear();
                        _adapterSql.Fill(DsPullTabs, DtPullTabs);
                        foreach (DataRow row in DsPullTabs.Tables[DtPullTabs].Rows)
                        {
                            // fill PullTabs list from DataSet
                            PullTab tempPullTab = new PullTab();
                            tempPullTab.FormID = row[0].ToString();
                            tempPullTab.Description = row[1].ToString();
                            tempPullTab.Gross = double.Parse(row[2].ToString());
                            tempPullTab.Prizes = double.Parse(row[3].ToString());
                            tempPullTab.Tax = double.Parse(row[4].ToString());
                            string strCurrent = row[5].ToString().ToLower().ToLower();
                            if (strCurrent == "false")
                                tempPullTab.Current = false;
                            else
                                tempPullTab.Current = true;
                            PullTabs.Add(tempPullTab);
                        }
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("MSSQL fill of PullTabs table failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conSql.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("PullTabs table not filled. No database type specified.");
            }
        }
        public static void FillSessionBingoGamesTable(Int32 p_TemplateID)
        {
            // method to fill the SessionBingoGames table with information from a database

            if (_whichDatabase == "Access")
            {
                // if the database is the Access database
                using (OleDbCommand command = new OleDbCommand())
                {
                    // using a disposable command query the SessionBingoGames table
                    command.Connection = _conOleDb;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT * FROM SessionBingoGames WHERE TemplateID = ? ORDER BY GameNumber";
                    command.Parameters.AddWithValue("?", p_TemplateID);
                    _adapterOleDb.SelectCommand = command;
                    try
                    {
                        // open database connection, clear the dataset, and fill the SessionBingoGames table
                        _conOleDb.Open();
                        DsSessionBingoGames.Clear();
                        SessionBingoGame.CurrentSessionBingoGames.Clear();
                        _adapterOleDb.Fill(DsSessionBingoGames, DtSessionBingoGames);
                        foreach (DataRow row in DsSessionBingoGames.Tables[DtSessionBingoGames].Rows)
                        {
                            // fill SessionBingoGames list from DataSet
                            SessionBingoGame tempBingoSessionGame = new SessionBingoGame();
                            tempBingoSessionGame.GameID = Int32.Parse(row[0].ToString());
                            tempBingoSessionGame.TemplateID = p_TemplateID;
                            tempBingoSessionGame.GameNumber = Int32.Parse(row[2].ToString());
                            tempBingoSessionGame.Description = row[3].ToString();
                            tempBingoSessionGame.MaxOrSetPrice = double.Parse(row[4].ToString());
                            tempBingoSessionGame.CardID1 = Int32.Parse(row[5].ToString());
                            tempBingoSessionGame.CardID2 = Int32.Parse(row[6].ToString());
                            tempBingoSessionGame.CardID3 = Int32.Parse(row[7].ToString());
                            string strEarlyBird = row[8].ToString().ToLower();
                            if (strEarlyBird == "false")
                                tempBingoSessionGame.EarlyBird = false;
                            else
                                tempBingoSessionGame.EarlyBird = true;
                            string strChangePaidOut = row[9].ToString().ToLower();
                            if (strChangePaidOut == "false")
                                tempBingoSessionGame.ChangePaidOut = false;
                            else
                                tempBingoSessionGame.ChangePaidOut = true;
                            string strFiftyFifty = row[10].ToString().ToLower();
                            if (strFiftyFifty == "false")
                                tempBingoSessionGame.FiftyFifty = false;
                            else
                                tempBingoSessionGame.FiftyFifty = true;
                            tempBingoSessionGame.ColorRed = Int32.Parse(row[11].ToString());
                            tempBingoSessionGame.ColorGreen = Int32.Parse(row[12].ToString());
                            tempBingoSessionGame.ColorBlue = Int32.Parse(row[13].ToString());
                            tempBingoSessionGame.BackColor = Color.FromArgb(tempBingoSessionGame.ColorRed, tempBingoSessionGame.ColorGreen, tempBingoSessionGame.ColorBlue);
                            SessionBingoGame.CurrentSessionBingoGames.Add(tempBingoSessionGame);
                        }
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("Access fill of SessionBingoGames table failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conOleDb.Close();
                    }
                }
            }
            else if (_whichDatabase == "MSSQL")
            {
                // if the database is the MSSQL database
                using (SqlCommand command = new SqlCommand())
                {
                    // using a disposable command query the SessionBingoGames table
                    command.Connection = _conSql;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT * FROM SessionBingoGames WHERE TemplateID = @templateID ORDER BY GameNumber";
                    command.Parameters.AddWithValue("@templateID", p_TemplateID);
                    _adapterSql.SelectCommand = command;
                    try
                    {
                        // open database connection, clear the dataset, and fill the SessionBingoGames table
                        _conSql.Open();
                        DsSessionBingoGames.Clear();
                        SessionBingoGame.CurrentSessionBingoGames.Clear();
                        _adapterSql.Fill(DsSessionBingoGames, DtSessionBingoGames);
                        foreach (DataRow row in DsSessionBingoGames.Tables[DtSessionBingoGames].Rows)
                        {
                            // fill SessionBingoGames list from DataSet
                            SessionBingoGame tempBingoSessionGame = new SessionBingoGame();
                            tempBingoSessionGame.GameID = Int32.Parse(row[0].ToString());
                            tempBingoSessionGame.TemplateID = p_TemplateID;
                            tempBingoSessionGame.GameNumber = Int32.Parse(row[2].ToString());
                            tempBingoSessionGame.Description = row[3].ToString();
                            tempBingoSessionGame.MaxOrSetPrice = double.Parse(row[4].ToString());
                            tempBingoSessionGame.CardID1 = Int32.Parse(row[5].ToString());
                            tempBingoSessionGame.CardID2 = Int32.Parse(row[6].ToString());
                            tempBingoSessionGame.CardID3 = Int32.Parse(row[7].ToString());
                            string strEarlyBird = row[8].ToString().ToLower();
                            if (strEarlyBird == "false")
                                tempBingoSessionGame.EarlyBird = false;
                            else
                                tempBingoSessionGame.EarlyBird = true;
                            string strChangePaidOut = row[9].ToString().ToLower();
                            if (strChangePaidOut == "false")
                                tempBingoSessionGame.ChangePaidOut = false;
                            else
                                tempBingoSessionGame.ChangePaidOut = true;
                            string strFiftyFifty = row[10].ToString().ToLower();
                            if (strFiftyFifty == "false")
                                tempBingoSessionGame.FiftyFifty = false;
                            else
                                tempBingoSessionGame.FiftyFifty = true;
                            tempBingoSessionGame.ColorRed = Int32.Parse(row[11].ToString());
                            tempBingoSessionGame.ColorGreen = Int32.Parse(row[12].ToString());
                            tempBingoSessionGame.ColorBlue = Int32.Parse(row[13].ToString());
                            tempBingoSessionGame.BackColor = Color.FromArgb(tempBingoSessionGame.ColorRed, tempBingoSessionGame.ColorGreen, tempBingoSessionGame.ColorBlue);
                            SessionBingoGame.CurrentSessionBingoGames.Add(tempBingoSessionGame);
                        }
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("MSSQL fill of SessionBingoGames table failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conSql.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("SessionBingoGames table not filled. No database type specified.");
            }
        }
        public static void FillSessionTemplatesTable()
        {
            // method to fill the SessionTemplates table with information from a database

            if (_whichDatabase == "Access")
            {
                // if the database is the Access database
                using (OleDbCommand command = new OleDbCommand())
                {
                    // using a disposable command query the SessionTemplates table
                    command.Connection = _conOleDb;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT * FROM SessionTemplates";
                    _adapterOleDb.SelectCommand = command;
                    try
                    {
                        // open database connection, clear the dataset, and fill the SessionTemplates table
                        _conOleDb.Open();
                        DsSessionTemplates.Clear();
                        SessionTemplates.Clear();
                        _adapterOleDb.Fill(DsSessionTemplates, DtSessionTemplates);
                        foreach (DataRow row in DsSessionTemplates.Tables[DtSessionTemplates].Rows)
                        {
                            // fill SessionTemplates list from DataSet
                            SessionTemplate tempSessionTemplate = new SessionTemplate();
                            tempSessionTemplate.TemplateID = Int32.Parse(row[0].ToString());
                            tempSessionTemplate.TemplateName = row[1].ToString();
                            SessionTemplates.Add(tempSessionTemplate);
                        }
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("Access fill of SessionTemplates table failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conOleDb.Close();
                    }
                }
            }
            else if (_whichDatabase == "MSSQL")
            {
                // if the database is the MSSQL database
                using (SqlCommand command = new SqlCommand())
                {
                    // using a disposable command query the SessionTemplates table
                    command.Connection = _conSql;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT * FROM SessionTemplates";
                    _adapterSql.SelectCommand = command;
                    try
                    {
                        // open database connection, clear the dataset, and fill the SessionTemplates table
                        _conSql.Open();
                        DsSessionTemplates.Clear();
                        SessionTemplates.Clear();
                        _adapterSql.Fill(_dsSessionTemplates, _dtSessionTemplates);
                        foreach (DataRow row in DsSessionTemplates.Tables[DtSessionTemplates].Rows)
                        {
                            // fill SessionTemplates list from DataSet
                            SessionTemplate tempSessionTemplate = new SessionTemplate();
                            tempSessionTemplate.TemplateID = Int32.Parse(row[0].ToString());
                            tempSessionTemplate.TemplateName = row[1].ToString();
                            SessionTemplates.Add(tempSessionTemplate);
                        }
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("MSSQL fill of SessionTemplates table failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conSql.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("SessionTemplates table not filled. No database type specified.");
            }
        }
        public static void FillWorkersTable()
        {
            // method to fill the Workers table with information from a database

            if (_whichDatabase == "Access")
            {
                // if the database is the Access database
                using (OleDbCommand command = new OleDbCommand())
                {
                    // using a disposable command query the Workers table
                    command.Connection = _conOleDb;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT * FROM Workers";
                    _adapterOleDb.SelectCommand = command;
                    try
                    {
                        // open database connection, clear the dataset, and fill the SessionTemplates table
                        _conOleDb.Open();
                        DsWorkers.Clear();
                        Workers.Clear();
                        _adapterOleDb.Fill(DsWorkers, DtWorkers);
                        foreach (DataRow row in DsWorkers.Tables[DtWorkers].Rows)
                        {
                            // fill Workers list from DataSet
                            Worker tempWorker = new Worker();
                            tempWorker.Name = row[0].ToString();
                            Workers.Add(tempWorker);
                        }
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("Access fill of Workers table failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conOleDb.Close();
                    }
                }
            }
            else if (_whichDatabase == "MSSQL")
            {
                // if the database is the MSSQL database
                using (SqlCommand command = new SqlCommand())
                {
                    // using a disposable command query the Workers table
                    command.Connection = _conSql;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT * FROM Workers";
                    _adapterSql.SelectCommand = command;
                    try
                    {
                        // open database connection, clear the dataset, and fill the Workers table
                        _conSql.Open();
                        DsWorkers.Clear();
                        Workers.Clear();
                        _adapterSql.Fill(DsWorkers, DtWorkers);
                        foreach (DataRow row in DsWorkers.Tables[DtWorkers].Rows)
                        {
                            // fill Workers list from DataSet
                            Worker tempWorker = new Worker();
                            tempWorker.Name = row[0].ToString();
                            Workers.Add(tempWorker);
                        }
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("MSSQL fill of Workers table failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conSql.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Workers table not filled. No database type specified.");
            }
        }
        #endregion FillMethods
        #region UpdateMethods
        // methods to update database row info
        public static void UpdateDefault(string p_value, string p_type)
        {
            // method to change the current default in the Defaults table

            if (_whichDatabase == "Access")
            {
                // if the database is the Access database
                using (OleDbCommand command = new OleDbCommand())
                {
                    // using a disposable command set new default value
                    command.Connection = _conOleDb;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "UPDATE Defaults SET [Value] = ? WHERE DefaultType = ?";
                    command.Parameters.AddWithValue("?", p_value);
                    command.Parameters.AddWithValue("?", p_type);
                    try
                    {
                        // open database connection, execute the command
                        _conOleDb.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("Access change of default " + p_type + " failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conOleDb.Close();
                    }
                }
            }
            else if (_whichDatabase == "MSSQL")
            {
                // if the database is the MSSQL database
                using (SqlCommand command = new SqlCommand())
                {
                    // using a disposable command set new default value
                    command.Connection = _conSql;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "UPDATE Defaults SET [Value] = @val WHERE [DefaultType] = @type";
                    command.Parameters.AddWithValue("@val", p_value);
                    command.Parameters.AddWithValue("@type", p_type);
                    try
                    {
                        // open database connection, execute the command
                        _conSql.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("MSSQL change of default " + p_type + " failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conSql.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Default " + p_type + " not changed. No database type specified.");
            }
        }
        public static void UpdateCharity(string p_taxID, string p_taxIDNew, string p_charity)
        {
            // method to change a charity's information

            if (_whichDatabase == "Access")
            {
                // if the database is the Access database
                using (OleDbCommand command = new OleDbCommand())
                {
                    // using a disposable command update info on a charity
                    command.Connection = _conOleDb;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "UPDATE Charities SET TaxID = ?, Charity = ? WHERE TaxID = ?";
                    command.Parameters.AddWithValue("?", p_taxIDNew);
                    command.Parameters.AddWithValue("?", p_charity);
                    command.Parameters.AddWithValue("?", p_taxID);
                    try
                    {
                        // open database connection, execute the command
                        _conOleDb.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("Access change of charity " + p_charity + " failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conOleDb.Close();
                    }
                }
            }
            else if (_whichDatabase == "MSSQL")
            {
                // if the database is the MSSQL database
                using (SqlCommand command = new SqlCommand())
                {
                    // using a disposable command update info on a charity
                    command.Connection = _conSql;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "UPDATE Charities SET TaxID = @taxnew, Charity = @charity WHERE TaxID = @tax";
                    command.Parameters.AddWithValue("@taxnew", p_taxIDNew);
                    command.Parameters.AddWithValue("@charity", p_charity);
                    command.Parameters.AddWithValue("@tax", p_taxID);
                    try
                    {
                        // open database connection, execute the command
                        _conSql.Open();
                        command.ExecuteNonQuery();
                        _conSql.Close();
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("MSSQL change of charity " + p_charity + " failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conSql.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Charity " + p_charity + " not changed. No database type specified.");
            }
        }
        public static void UpdateMachine(Int32 p_eID, string p_description)
        {
            // method to change a machine's information

            if (_whichDatabase == "Access")
            {
                // if the database is the Access database
                using (OleDbCommand command = new OleDbCommand())
                {
                    // using a disposable command update info on a machine
                    command.Connection = _conOleDb;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "UPDATE Electronics SET Description = ? WHERE ElectronicsID = ?";
                    command.Parameters.AddWithValue("?", p_description);
                    command.Parameters.AddWithValue("?", p_eID);
                    try
                    {
                        // open database connection, execute the command
                        _conOleDb.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("Access change of machine " + p_description + " failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conOleDb.Close();
                    }
                }
            }
            else if (_whichDatabase == "MSSQL")
            {
                // if the database is the MSSQL database
                using (SqlCommand command = new SqlCommand())
                {
                    // using a disposable command update info on a charity
                    command.Connection = _conSql;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "UPDATE Electronics SET Description = @desc WHERE ElectronicsID = @eid";
                    command.Parameters.AddWithValue("@desc", p_description);
                    command.Parameters.AddWithValue("@eid", p_eID);
                    try
                    {
                        // open database connection, execute the command
                        _conSql.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("MSSQL change of machine " + p_description + " failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conSql.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Machine " + p_description + " not changed. No database type specified.");
            }
        }
        public static void UpdateFloorCard(Int32 p_cardID, string p_description, double p_price, 
            Int32 p_qty1, Int32 p_qty2, Int32 p_qty3, Int32 p_qty4, Int32 p_qty5, Int32 p_gameID1, Int32 p_gameID2, 
            Int32 p_gameID3, Int32 p_colorRed, Int32 p_colorGreen, Int32 p_colorBlue)
        {
            // method to change a floor card's information

            if (_whichDatabase == "Access")
            {
                // if the database is the Access database
                using (OleDbCommand command = new OleDbCommand())
                {
                    // using a disposable command update info on a floor card
                    command.Connection = _conOleDb;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "UPDATE FloorCards SET Description = ?, " +
                        "Price = ?, Quantity1 = ?, Quantity2 = ?, Quantity3 = ?, Quantity4 = ?, Quantity5 = ?, " + 
                        "GameID1 = ?, GameID2 = ?, GameID3 = ?, ColorRed = ?, ColorGreen = ?, ColorBlue = ? " + 
                        "WHERE CardID = ?";
                    command.Parameters.AddWithValue("?", p_description);
                    command.Parameters.AddWithValue("?", p_price);
                    command.Parameters.AddWithValue("?", p_qty1);
                    command.Parameters.AddWithValue("?", p_qty2);
                    command.Parameters.AddWithValue("?", p_qty3);
                    command.Parameters.AddWithValue("?", p_qty4);
                    command.Parameters.AddWithValue("?", p_qty5);
                    command.Parameters.AddWithValue("?", p_gameID1);
                    command.Parameters.AddWithValue("?", p_gameID2);
                    command.Parameters.AddWithValue("?", p_gameID3);
                    command.Parameters.AddWithValue("?", p_colorRed);
                    command.Parameters.AddWithValue("?", p_colorGreen);
                    command.Parameters.AddWithValue("?", p_colorBlue);
                    command.Parameters.AddWithValue("?", p_cardID);
                    try
                    {
                        // open database connection, execute the command
                        _conOleDb.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("Access change of floor card " + p_description + " failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conOleDb.Close();
                    }
                }
            }
            else if (_whichDatabase == "MSSQL")
            {
                // if the database is the MSSQL database
                using (SqlCommand command = new SqlCommand())
                {
                    // using a disposable command update info on a floor card
                    command.Connection = _conSql;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "UPDATE FloorCards SET Description = @desc, " +
                        "Price = @price, Quantity1 = @qty1, Quantity2 = @qty2, Quantity3 = @qty3, Quantity4 = @qty4, Quantity5 = @qty5, " +
                        "GameID1 = @game1, GameID2 = @game2, GameID3 = @game3, ColorRed = @red, ColorGreen = @green, ColorBlue = @blue " +
                        "WHERE CardID = @card";
                    command.Parameters.AddWithValue("@desc", p_description);
                    command.Parameters.AddWithValue("@price", p_price);
                    command.Parameters.AddWithValue("@qty1", p_qty1);
                    command.Parameters.AddWithValue("@qty2", p_qty2);
                    command.Parameters.AddWithValue("@qty3", p_qty3);
                    command.Parameters.AddWithValue("@qty4", p_qty4);
                    command.Parameters.AddWithValue("@qty5", p_qty5);
                    command.Parameters.AddWithValue("@game1", p_gameID1);
                    command.Parameters.AddWithValue("@game2", p_gameID2);
                    command.Parameters.AddWithValue("@game3", p_gameID3);
                    command.Parameters.AddWithValue("@red", p_colorRed);
                    command.Parameters.AddWithValue("@green", p_colorGreen);
                    command.Parameters.AddWithValue("@blue", p_colorBlue);
                    command.Parameters.AddWithValue("@card", p_cardID);
                    try
                    {
                        // open database connection, execute the command
                        _conSql.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("MSSQL change of floor card " + p_description + " failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conSql.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Floor card " + p_description + " not changed. No database type specified.");
            }
        }
        public static void UpdatePaperSet(Int32 p_paperSetID, string p_description, double p_price, 
            bool p_redemptionAvailable, double p_redemptionPrice)
        {
            // method to change a paper set's information

            if (_whichDatabase == "Access")
            {
                // if the database is the Access database
                using (OleDbCommand command = new OleDbCommand())
                {
                    // using a disposable command update info on a paper set
                    command.Connection = _conOleDb;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "UPDATE PaperSets SET [Description] = ?, [Price] = ?, [RedemptionAvailable] = ?, " +
                        "RedemptionPrice = ? WHERE PaperSetID = ?";
                    command.Parameters.AddWithValue("?", p_description);
                    command.Parameters.AddWithValue("?", p_price);
                    command.Parameters.AddWithValue("?", p_redemptionAvailable);
                    command.Parameters.AddWithValue("?", p_redemptionPrice);
                    command.Parameters.AddWithValue("?", p_paperSetID);
                    try
                    {
                        // open database connection, execute the command
                        _conOleDb.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("Access change of paper set " + p_description + " failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conOleDb.Close();
                    }
                }
            }
            else if (_whichDatabase == "MSSQL")
            {
                // if the database is the MSSQL database
                using (SqlCommand command = new SqlCommand())
                {
                    // using a disposable command update info on a paper set card
                    command.Connection = _conSql;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "UPDATE PaperSets SET Description = @desc, Price = @price, RedemptionAvailable = @avail, " +
                        "RedemptionPrice = @rprice WHERE PaperSetID = @paper";
                    command.Parameters.AddWithValue("@desc", p_description);
                    command.Parameters.AddWithValue("@price", p_price);
                    if (p_redemptionAvailable)
                        command.Parameters.AddWithValue("@avail", 1);
                    else
                        command.Parameters.AddWithValue("@avail", 0);
                    command.Parameters.AddWithValue("@rprice", p_redemptionPrice);
                    command.Parameters.AddWithValue("@paper", p_paperSetID);
                    try
                    {
                        // open database connection, execute the command
                        _conSql.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("MSSQL change of paper set " + p_description + " failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conSql.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Paper set " + p_description + " not changed. No database type specified.");
            }
        }
        public static void UpdatePullTab(string p_formID, string p_formIDNew, string p_description, double p_gross, double p_prizes, 
            double p_tax, bool p_current)
        {
            // method to change a PullTab's information

            if (_whichDatabase == "Access")
            {
                // if the database is the Access database
                using (OleDbCommand command = new OleDbCommand())
                {
                    // using a disposable command update info on a pulltab
                    command.Connection = _conOleDb;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "UPDATE PullTabs SET [FormID] = ?, [Description] = ?, [Gross] = ?, " +
                        "[Prizes] = ?, [Tax] = ?, [Current] = ? WHERE [FormID] = ?";
                    command.Parameters.AddWithValue("?", p_formIDNew);
                    command.Parameters.AddWithValue("?", p_description);
                    command.Parameters.AddWithValue("?", p_gross);
                    command.Parameters.AddWithValue("?", p_prizes);
                    command.Parameters.AddWithValue("?", p_tax);
                    command.Parameters.AddWithValue("?", p_current);
                    command.Parameters.AddWithValue("?", p_formID);
                    try
                    {
                        // open database connection, execute the command
                        _conOleDb.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("Access change of pulltab " + p_description + " failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conOleDb.Close();
                    }
                }
            }
            else if (_whichDatabase == "MSSQL")
            {
                // if the database is the MSSQL database
                using (SqlCommand command = new SqlCommand())
                {
                    // using a disposable command update info on a pulltab
                    command.Connection = _conSql;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "UPDATE PullTabs SET [FormID] = @formnew, [Name] = @desc, [Gross] = @gross, " +
                        "[Prizes] = @prizes, [Tax] = @tax, [IsCurrent] = @curr WHERE [FormID] = @form";
                    command.Parameters.AddWithValue("@formnew", p_formIDNew);
                    command.Parameters.AddWithValue("@desc", p_description);
                    command.Parameters.AddWithValue("@gross", p_gross);
                    command.Parameters.AddWithValue("@prizes", p_prizes);
                    command.Parameters.AddWithValue("@tax", p_tax);
                    if (p_current)
                        command.Parameters.AddWithValue("@curr", 1);
                    else
                        command.Parameters.AddWithValue("@curr", 0);
                    command.Parameters.AddWithValue("@form", p_formID);
                    try
                    {
                        // open database connection, execute the command
                        _conSql.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("MSSQL change of pulltab " + p_description + " failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conSql.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Pulltab " + p_description + " not changed. No database type specified.");
            }
        }
        public static void UpdateSessionBingoGame(Int32 p_gameID, Int32 p_gameNumber, string p_description,
            double p_maxOrSetPrize, Int32 p_cardID1, Int32 p_cardID2, Int32 p_cardID3, bool p_earlyBird, bool p_changePaidOut, 
            bool p_FiftyFifty, Int32 p_colorRed, Int32 p_colorGreen, Int32 p_colorBlue)
        {
            // method to change a session bingo game's information

            if (_whichDatabase == "Access")
            {
                // if the database is the Access database
                using (OleDbCommand command = new OleDbCommand())
                {
                    // using a disposable command update info on a session bingo game
                    command.Connection = _conOleDb;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "UPDATE SessionBingoGames SET [GameNumber] = ?, [Description] = ?, [MaxOrSetPrize] = ?, " +
                        "[CardID1] = ?, [CardID2] = ?, [CardID3] = ?, [EarlyBird] = ?, [ChangePaidOut] = ?, [FiftyFifty] = ?, [ColorRed] = ?, " +
                        "[ColorGreen] = ?, [ColorBlue] = ? WHERE [GameID] = ?";
                    command.Parameters.AddWithValue("?", p_gameNumber);
                    command.Parameters.AddWithValue("?", p_description);
                    command.Parameters.AddWithValue("?", p_maxOrSetPrize);
                    command.Parameters.AddWithValue("?", p_cardID1);
                    command.Parameters.AddWithValue("?", p_cardID2);
                    command.Parameters.AddWithValue("?", p_cardID3);
                    command.Parameters.AddWithValue("?", p_earlyBird);
                    command.Parameters.AddWithValue("?", p_changePaidOut);
                    command.Parameters.AddWithValue("?", p_FiftyFifty);
                    command.Parameters.AddWithValue("?", p_colorRed);
                    command.Parameters.AddWithValue("?", p_colorGreen);
                    command.Parameters.AddWithValue("?", p_colorBlue);
                    command.Parameters.AddWithValue("?", p_gameID);
                    try
                    {
                        // open database connection, execute the command
                        _conOleDb.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("Access change of session bingo game " + p_description + " failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conOleDb.Close();
                    }
                }
            }
            else if (_whichDatabase == "MSSQL")
            {
                // if the database is the MSSQL database
                using (SqlCommand command = new SqlCommand())
                {
                    // using a disposable command update info on a session bingo game
                    command.Connection = _conSql;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "UPDATE SessionBingoGames SET GameNumber = @gameno, Description = @desc, MaxOrSetPrize = @prize, " +
                        "CardID1 = @card1, CardID2 = @card2, CardID3 = @card3, EarlyBird = @eb, ChangePaidOut = @change, FiftyFifty = @fifty, ColorRed = @red, " +
                        "ColorGreen = @green, ColorBlue = @blue WHERE GameID = @game";
                    command.Parameters.AddWithValue("@gameno", p_gameNumber);
                    command.Parameters.AddWithValue("@desc", p_description);
                    command.Parameters.AddWithValue("@prize", p_maxOrSetPrize);
                    command.Parameters.AddWithValue("@card1", p_cardID1);
                    command.Parameters.AddWithValue("@card2", p_cardID2);
                    command.Parameters.AddWithValue("@card3", p_cardID3);
                    if (p_earlyBird)
                        command.Parameters.AddWithValue("@eb", 1);
                    else
                        command.Parameters.AddWithValue("@eb", 0);
                    if(p_changePaidOut)
                        command.Parameters.AddWithValue("@change", 1);
                    else
                        command.Parameters.AddWithValue("@change", 0);
                    if(p_FiftyFifty)
                        command.Parameters.AddWithValue("@fifty", 1);
                    else
                        command.Parameters.AddWithValue("@fifty", 0);
                    command.Parameters.AddWithValue("@red", p_colorRed);
                    command.Parameters.AddWithValue("@green", p_colorGreen);
                    command.Parameters.AddWithValue("@blue", p_colorBlue);
                    command.Parameters.AddWithValue("@game", p_gameID);
                    try
                    {
                        // open database connection, execute the command
                        _conSql.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("MSSQL change of session bingo game " + p_description + " failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conSql.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Session Bingo Game " + p_description + " not changed. No database type specified.");
            }
        }
        public static void UpdateSessionTemplate(Int32 p_templateID, string p_templateName)
        {
            // method to change a session template's information

            if (_whichDatabase == "Access")
            {
                // if the database is the Access database
                using (OleDbCommand command = new OleDbCommand())
                {
                    // using a disposable command update info on a session template
                    command.Connection = _conOleDb;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "UPDATE SessionTemplates SET [TemplateName] = ? WHERE [TemplateID] = ?";
                    command.Parameters.AddWithValue("?", p_templateName);
                    command.Parameters.AddWithValue("?", p_templateID);
                    try
                    {
                        // open database connection, execute the command
                        _conOleDb.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("Access change of session template " + p_templateName + " failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conOleDb.Close();
                    }
                }
            }
            else if (_whichDatabase == "MSSQL")
            {
                // if the database is the MSSQL database
                using (SqlCommand command = new SqlCommand())
                {
                    // using a disposable command update info on a session template
                    command.Connection = _conSql;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "UPDATE SessionTemplates SET TemplateName = @name WHERE TemplateID = @id";
                    command.Parameters.AddWithValue("@name", p_templateName);
                    command.Parameters.AddWithValue("@id", p_templateID);
                    try
                    {
                        // open database connection, execute the command
                        _conSql.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("MSSQL change of session template " + p_templateName + " failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conSql.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Session template " + p_templateName + " not changed. No database type specified.");
            }
        }
        public static void UpdateWorker(string p_name, string p_nameNew)
        {
            // method to change a worker's information

            if (_whichDatabase == "Access")
            {
                // if the database is the Access database
                using (OleDbCommand command = new OleDbCommand())
                {
                    // using a disposable command update info on a worker
                    command.Connection = _conOleDb;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "UPDATE Workers SET [Name] = ? WHERE [Name] = ?";
                    command.Parameters.AddWithValue("?", p_nameNew);
                    command.Parameters.AddWithValue("?", p_name);
                    try
                    {
                        // open database connection, execute the command
                        _conOleDb.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("Access change of worder " + p_name + " failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conOleDb.Close();
                    }
                }
            }
            else if (_whichDatabase == "MSSQL")
            {
                // if the database is the MSSQL database
                using (SqlCommand command = new SqlCommand())
                {
                    // using a disposable command update info on a worker
                    command.Connection = _conSql;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "UPDATE Workers SET Name = @new WHERE Name = @old";
                    command.Parameters.AddWithValue("@new", p_nameNew);
                    command.Parameters.AddWithValue("@old", p_name);
                    try
                    {
                        // open database connection, execute the command
                        _conSql.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("MSSQL change of worker " + p_name + " failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conSql.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Worker " + p_name + " not changed. No database type specified.");
            }
        }
        #endregion UpdateMethods
        #region DeleteMethods
        // methods to delete database rows
        public static void DeleteCharity(string p_taxID, string p_charity)
        {
            // method to remove a charity

            if (_whichDatabase == "Access")
            {
                // if the database is the Access database
                using (OleDbCommand command = new OleDbCommand())
                {
                    // using a disposable command delete a charity
                    command.Connection = _conOleDb;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "DELETE FROM Charities WHERE TaxID = ?";
                    command.Parameters.AddWithValue("?", p_taxID);
                    command.Parameters.AddWithValue("?", p_charity);
                    try
                    {
                        // open database connection, execute the command
                        _conOleDb.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("Access deletion of charity " + p_charity + " failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conOleDb.Close();
                    }
                }
            }
            else if (_whichDatabase == "MSSQL")
            {
                // if the database is the MSSQL database
                using (SqlCommand command = new SqlCommand())
                {
                    // using a disposable command delete a charity
                    command.Connection = _conSql;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "DELETE FROM Charities WHERE TaxID = @tax AND Charity = @charity";
                    command.Parameters.AddWithValue("@tax", p_taxID);
                    command.Parameters.AddWithValue("@charity", p_charity);
                    try
                    {
                        // open database connection, execute the command
                        _conSql.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("MSSQL deletion of charity " + p_charity + " failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conSql.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Charity " + p_charity + " not deleted. No database type specified.");
            }
        }
        public static void DeleteMachine(Int32 p_electronicsID, string p_description)
        {
            // method to remove a machine

            if (_whichDatabase == "Access")
            {
                // if the database is the Access database
                using (OleDbCommand command = new OleDbCommand())
                {
                    // using a disposable command delete a machine
                    command.Connection = _conOleDb;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "DELETE FROM Electronics WHERE ElectronicsID = ?";
                    command.Parameters.AddWithValue("?", p_electronicsID);
                    try
                    {
                        // open database connection, execute the command
                        _conOleDb.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("Access deletion of machine " + p_description + " failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conOleDb.Close();
                    }
                }
            }
            else if (_whichDatabase == "MSSQL")
            {
                // if the database is the MSSQL database
                using (SqlCommand command = new SqlCommand())
                {
                    // using a disposable command delete a machine
                    command.Connection = _conSql;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "DELETE FROM Electronics WHERE ElectronicsID = @eid";
                    command.Parameters.AddWithValue("@eid", p_electronicsID);
                    try
                    {
                        // open database connection, execute the command
                        _conSql.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("MSSQL deletion of machine " + p_description + " failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conSql.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Machine " + p_description + " not deleted. No database type specified.");
            }
        }
        public static void DeleteTemplateMachines(int p_templateID)
        {
            // method to remove a machine

            if (_whichDatabase == "Access")
            {
                // if the database is the Access database
                using (OleDbCommand command = new OleDbCommand())
                {
                    // using a disposable command delete a machine
                    command.Connection = _conOleDb;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "DELETE FROM Electronics WHERE TemplateID = ?";
                    command.Parameters.AddWithValue("?", p_templateID);
                    try
                    {
                        // open database connection, execute the command
                        _conOleDb.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("Access deletion of machine failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conOleDb.Close();
                    }
                }
            }
            else if (_whichDatabase == "MSSQL")
            {
                // if the database is the MSSQL database
                using (SqlCommand command = new SqlCommand())
                {
                    // using a disposable command delete a machine
                    command.Connection = _conSql;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "DELETE FROM Electronics WHERE TemplateID = @eid";
                    command.Parameters.AddWithValue("@eid", p_templateID);
                    try
                    {
                        // open database connection, execute the command
                        _conSql.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("MSSQL deletion of machine failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conSql.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Machine not deleted. No database type specified.");
            }
        }
        public static void DeleteFloorCard(Int32 p_cardID, string p_description)
        {
            // method to remove a floor card

            if (_whichDatabase == "Access")
            {
                // if the database is the Access database
                using (OleDbCommand command = new OleDbCommand())
                {
                    // using a disposable command delete a floor card
                    command.Connection = _conOleDb;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "DELETE FROM FloorCards WHERE CardID = ?";
                    command.Parameters.AddWithValue("?", p_cardID);
                    try
                    {
                        // open database connection, execute the command
                        _conOleDb.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("Access deletion of floor card " + p_description + " failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conOleDb.Close();
                    }
                }
            }
            else if (_whichDatabase == "MSSQL")
            {
                // if the database is the MSSQL database
                using (SqlCommand command = new SqlCommand())
                {
                    // using a disposable command delete a floor card
                    command.Connection = _conSql;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "DELETE FROM FloorCards WHERE CardID = @card";
                    command.Parameters.AddWithValue("@card", p_cardID);
                    try
                    {
                        // open database connection, execute the command
                        _conSql.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("MSSQL deletion of floor card " + p_description + " failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conSql.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Floor card " + p_description + " not deleted. No database type specified.");
            }
        }
        public static void DeleteTemplateFloorCards(Int32 p_templateID)
        {
            // method to remove a floor card

            if (_whichDatabase == "Access")
            {
                // if the database is the Access database
                using (OleDbCommand command = new OleDbCommand())
                {
                    // using a disposable command delete a floor card
                    command.Connection = _conOleDb;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "DELETE FROM FloorCards WHERE TemplateID = ?";
                    command.Parameters.AddWithValue("?", p_templateID);
                    try
                    {
                        // open database connection, execute the command
                        _conOleDb.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("Access deletion of floor card failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conOleDb.Close();
                    }
                }
            }
            else if (_whichDatabase == "MSSQL")
            {
                // if the database is the MSSQL database
                using (SqlCommand command = new SqlCommand())
                {
                    // using a disposable command delete a floor card
                    command.Connection = _conSql;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "DELETE FROM FloorCards WHERE TemplateID = @id";
                    command.Parameters.AddWithValue("@id", p_templateID);
                    try
                    {
                        // open database connection, execute the command
                        _conSql.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("MSSQL deletion of floor card failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conSql.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Floor card not deleted. No database type specified.");
            }
        }
        public static void DeletePaperSet(Int32 p_paperSetID, string p_description)
        {
            // method to remove a machine

            if (_whichDatabase == "Access")
            {
                // if the database is the Access database
                using (OleDbCommand command = new OleDbCommand())
                {
                    // using a disposable command delete a paper set
                    command.Connection = _conOleDb;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "DELETE FROM PaperSets WHERE PaperSetID = ?";
                    command.Parameters.AddWithValue("?", p_paperSetID);
                    try
                    {
                        // open database connection, execute the command
                        _conOleDb.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("Access deletion of paper set " + p_description + " failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conOleDb.Close();
                    }
                }
            }
            else if (_whichDatabase == "MSSQL")
            {
                // if the database is the MSSQL database
                using (SqlCommand command = new SqlCommand())
                {
                    // using a disposable command delete a paper set
                    command.Connection = _conSql;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "DELETE FROM PaperSets WHERE PaperSetID = @paper";
                    command.Parameters.AddWithValue("@paper", p_paperSetID);
                    try
                    {
                        // open database connection, execute the command
                        _conSql.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("MSSQL deletion of paper set " + p_description + " failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conSql.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Paper set " + p_description + " not deleted. No database type specified.");
            }
        }
        public static void DeleteTemplatePaperSets(Int32 p_templateID)
        {
            // method to remove a machine

            if (_whichDatabase == "Access")
            {
                // if the database is the Access database
                using (OleDbCommand command = new OleDbCommand())
                {
                    // using a disposable command delete a paper set
                    command.Connection = _conOleDb;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "DELETE FROM PaperSets WHERE TemplateID = ?";
                    command.Parameters.AddWithValue("?", p_templateID);
                    try
                    {
                        // open database connection, execute the command
                        _conOleDb.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("Access deletion of paper set failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conOleDb.Close();
                    }
                }
            }
            else if (_whichDatabase == "MSSQL")
            {
                // if the database is the MSSQL database
                using (SqlCommand command = new SqlCommand())
                {
                    // using a disposable command delete a paper set
                    command.Connection = _conSql;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "DELETE FROM PaperSets WHERE TemplateID = @paper";
                    command.Parameters.AddWithValue("@paper", p_templateID);
                    try
                    {
                        // open database connection, execute the command
                        _conSql.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("MSSQL deletion of paper set failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conSql.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Paper set not deleted. No database type specified.");
            }
        }
        public static void DeletePullTab(string p_formID, string p_description)
        {
            // method to remove a pulltab

            if (_whichDatabase == "Access")
            {
                // if the database is the Access database
                using (OleDbCommand command = new OleDbCommand())
                {
                    // using a disposable command delete a pulltab
                    command.Connection = _conOleDb;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "DELETE FROM PullTabs WHERE FormID = ?";
                    command.Parameters.AddWithValue("?", p_formID);
                    try
                    {
                        // open database connection, execute the command
                        _conOleDb.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("Access deletion of pulltab " + p_description + " failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conOleDb.Close();
                    }
                }
            }
            else if (_whichDatabase == "MSSQL")
            {
                // if the database is the MSSQL database
                using (SqlCommand command = new SqlCommand())
                {
                    // using a disposable command delete a pulltab
                    command.Connection = _conSql;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "DELETE FROM PullTabs WHERE FormID = @form";
                    command.Parameters.AddWithValue("@form", p_formID);
                    try
                    {
                        // open database connection, execute the command
                        _conSql.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("MSSQL deletion of pulltab " + p_description + " failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conSql.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("PullTab " + p_description + " not deleted. No database type specified.");
            }
        }
        public static void DeleteSessionBingoGame(Int32 p_gameID, string p_description)
        {
            // method to remove a machine

            if (_whichDatabase == "Access")
            {
                // if the database is the Access database
                using (OleDbCommand command = new OleDbCommand())
                {
                    // using a disposable command delete a session bingo game
                    command.Connection = _conOleDb;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "DELETE FROM SessionBingoGames WHERE GameID = ?";
                    command.Parameters.AddWithValue("?", p_gameID);
                    try
                    {
                        // open database connection, execute the command
                        _conOleDb.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("Access deletion of session bingo game " + p_description + " failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conOleDb.Close();
                    }
                }
            }
            else if (_whichDatabase == "MSSQL")
            {
                // if the database is the MSSQL database
                using (SqlCommand command = new SqlCommand())
                {
                    // using a disposable command delete a session bingo game
                    command.Connection = _conSql;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "DELETE FROM SessionBingoGames WHERE GameID = @game";
                    command.Parameters.AddWithValue("@game", p_gameID);
                    try
                    {
                        // open database connection, execute the command
                        _conSql.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("MSSQL deletion of session bingo game " + p_description + " failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conSql.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Session Bingo Game " + p_description + " not deleted. No database type specified.");
            }
        }
        public static void DeleteTemplateSessionBingoGame(Int32 p_templateID)
        {
            // method to remove a machine

            if (_whichDatabase == "Access")
            {
                // if the database is the Access database
                using (OleDbCommand command = new OleDbCommand())
                {
                    // using a disposable command delete a session bingo game
                    command.Connection = _conOleDb;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "DELETE FROM SessionBingoGames WHERE TemplateID = ?";
                    command.Parameters.AddWithValue("?", p_templateID);
                    try
                    {
                        // open database connection, execute the command
                        _conOleDb.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("Access deletion of session bingo game failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conOleDb.Close();
                    }
                }
            }
            else if (_whichDatabase == "MSSQL")
            {
                // if the database is the MSSQL database
                using (SqlCommand command = new SqlCommand())
                {
                    // using a disposable command delete a session bingo game
                    command.Connection = _conSql;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "DELETE FROM SessionBingoGames WHERE TemplateID = @id";
                    command.Parameters.AddWithValue("@id", p_templateID);
                    try
                    {
                        // open database connection, execute the command
                        _conSql.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("MSSQL deletion of session bingo game failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conSql.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Session Bingo Game not deleted. No database type specified.");
            }
        }
        public static void DeleteSessionTemplate(Int32 p_templateID, string p_templateName)
        {
            // method to remove a machine

            // first remove all template foreign relationships
            DeleteTemplateMachines(p_templateID);
            DeleteTemplateFloorCards(p_templateID);
            DeleteTemplatePaperSets(p_templateID);
            DeleteTemplateSessionBingoGame(p_templateID);
            // now delete template
            if (_whichDatabase == "Access")
            {
                // if the database is the Access database
                using (OleDbCommand command = new OleDbCommand())
                {
                    // using a disposable command delete a session template
                    command.Connection = _conOleDb;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "DELETE FROM SessionTemplates WHERE [TemplateID] = ?";
                    command.Parameters.AddWithValue("?", p_templateID);
                    try
                    {
                        // open database connection, execute the command
                        _conOleDb.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("Access deletion of session template " + p_templateName + " failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conOleDb.Close();
                    }
                }
            }
            else if (_whichDatabase == "MSSQL")
            {
                // if the database is the MSSQL database
                using (SqlCommand command = new SqlCommand())
                {
                    // using a disposable command delete a session template
                    command.Connection = _conSql;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "DELETE FROM SessionTemplates WHERE TemplateID = @template";
                    command.Parameters.AddWithValue("@template", p_templateID);
                    try
                    {
                        // open database connection, execute the command
                        _conSql.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("MSSQL deletion of session template " + p_templateName + " failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conSql.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Session Template " + p_templateName + " not deleted. No database type specified.");
            }
        }
        public static void DeleteWorker(string p_name)
        {
            // method to remove a machine

            if (_whichDatabase == "Access")
            {
                // if the database is the Access database
                using (OleDbCommand command = new OleDbCommand())
                {
                    // using a disposable command delete a worker
                    command.Connection = _conOleDb;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "DELETE FROM Workers WHERE Name = ?";
                    command.Parameters.AddWithValue("?", p_name);
                    try
                    {
                        // open database connection, execute the command
                        _conOleDb.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("Access deletion of worker " + p_name + " failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conOleDb.Close();
                    }
                }
            }
            else if (_whichDatabase == "MSSQL")
            {
                // if the database is the MSSQL database
                using (SqlCommand command = new SqlCommand())
                {
                    // using a disposable command delete a worker
                    command.Connection = _conSql;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "DELETE FROM Workers WHERE Name = @name";
                    command.Parameters.AddWithValue("@name", p_name);
                    try
                    {
                        // open database connection, execute the command
                        _conSql.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("MSSQL deletion of worker " + p_name + " failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conSql.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Worker " + p_name + " not deleted. No database type specified.");
            }
        }
        #endregion DeleteMethods
        #region AddMethods
        public static void AddCharity(string p_taxID, string p_charity)
        {
            // method to add a charity

            if (_whichDatabase == "Access")
            {
                // if the database is the Access database
                using (OleDbCommand command = new OleDbCommand())
                {
                    // using a disposable command add a charity
                    command.Connection = _conOleDb;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "INSERT INTO Charities VALUES (?, ?)";
                    command.Parameters.AddWithValue("?", p_taxID);
                    command.Parameters.AddWithValue("?", p_charity);
                    try
                    {
                        // open database connection, execute the command
                        _conOleDb.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("Access addition of charity " + p_charity + " failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conOleDb.Close();
                    }
                }
            }
            else if (_whichDatabase == "MSSQL")
            {
                // if the database is the MSSQL database
                using (SqlCommand command = new SqlCommand())
                {
                    // using a disposable command add a charity
                    command.Connection = _conSql;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "INSERT INTO Charities VALUES (@taxID,@charity)";
                    command.Parameters.AddWithValue("@taxID", p_taxID);
                    command.Parameters.AddWithValue("@charity", p_charity);
                    try
                    {
                        // open database connection, execute the command
                        _conSql.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("MSSQL addition of charity " + p_charity + " failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conSql.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Charity " + p_charity + " not added. No database type specified.");
            }
        }
        public static void AddMachine(Int32 p_templateID, string p_description)
        {
            // method to add a machine

            if (_whichDatabase == "Access")
            {
                // if the database is the Access database
                using (OleDbCommand command = new OleDbCommand())
                {
                    // using a disposable command add a machine
                    command.Connection = _conOleDb;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "INSERT INTO Electronics (TemplateID, Description) VALUES (?, ?)";
                    command.Parameters.AddWithValue("?", p_templateID);
                    command.Parameters.AddWithValue("?", p_description);
                    try
                    {
                        // open database connection, execute the command
                        _conOleDb.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("Access addition of machine " + p_description + " failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conOleDb.Close();
                    }
                }
            }
            else if (_whichDatabase == "MSSQL")
            {
                // if the database is the MSSQL database
                using (SqlCommand command = new SqlCommand())
                {
                    // using a disposable command add a machine
                    command.Connection = _conSql;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "INSERT INTO Electronics VALUES (@template, @desc)";
                    command.Parameters.AddWithValue("@template", p_templateID);
                    command.Parameters.AddWithValue("@desc", p_description);
                    try
                    {
                        // open database connection, execute the command
                        _conSql.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("MSSQL addition of machine " + p_description + " failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conSql.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Machine " + p_description + " not added. No database type specified.");
            }
        }
        public static void AddFloorCard(Int32 p_templateID, string p_description)
        {
            // method to add a floor card

            if (_whichDatabase == "Access")
            {
                // if the database is the Access database
                using (OleDbCommand command = new OleDbCommand())
                {
                    // using a disposable command add a floor card
                    command.Connection = _conOleDb;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "INSERT INTO FloorCards (TemplateID,Description,Price,Quantity1,Quantity2,"+
                        "Quantity3,Quantity4,Quantity5,GameID1,GameID2,GameID3,ColorRed,ColorGreen,ColorBlue) " +
                        "VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";
                    command.Parameters.AddWithValue("?", p_templateID);
                    command.Parameters.AddWithValue("?", p_description);
                    command.Parameters.AddWithValue("?", 0);
                    command.Parameters.AddWithValue("?", 0);
                    command.Parameters.AddWithValue("?", 0);
                    command.Parameters.AddWithValue("?", 0);
                    command.Parameters.AddWithValue("?", 0);
                    command.Parameters.AddWithValue("?", 0);
                    command.Parameters.AddWithValue("?", -1);
                    command.Parameters.AddWithValue("?", -1);
                    command.Parameters.AddWithValue("?", -1);
                    command.Parameters.AddWithValue("?", 0);
                    command.Parameters.AddWithValue("?", 0);
                    command.Parameters.AddWithValue("?", 0);
                    try
                    {
                        // open database connection, execute the command
                        _conOleDb.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("Access addition of floor card " + p_description + " failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conOleDb.Close();
                    }
                }
            }
            else if (_whichDatabase == "MSSQL")
            {
                // if the database is the MSSQL database
                using (SqlCommand command = new SqlCommand())
                {
                    // using a disposable command add a floor card
                    command.Connection = _conSql;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "INSERT INTO FloorCards VALUES (@template,@desc,@price,@qty1,@qty2,@qty3,@qty4,@qty5,@game1,@game2,@game3,@red,@green,@blue)";
                    command.Parameters.AddWithValue("@template", p_templateID);
                    command.Parameters.AddWithValue("@desc", p_description);
                    command.Parameters.AddWithValue("@price", 0);
                    command.Parameters.AddWithValue("@qty1", 0);
                    command.Parameters.AddWithValue("@qty2", 0);
                    command.Parameters.AddWithValue("@qty3", 0);
                    command.Parameters.AddWithValue("@qty4", 0);
                    command.Parameters.AddWithValue("@qty5", 0);
                    command.Parameters.AddWithValue("@game1", -1);
                    command.Parameters.AddWithValue("@game2", -1);
                    command.Parameters.AddWithValue("@game3", -1);
                    command.Parameters.AddWithValue("@red", 0);
                    command.Parameters.AddWithValue("@green", 0);
                    command.Parameters.AddWithValue("@blue", 0);
                    try
                    {
                        // open database connection, execute the command
                        _conSql.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("MSSQL addition of floor card " + p_description + " failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conSql.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("FloorCard " + p_description + " not added. No database type specified.");
            }
        }
        public static void AddPaperSet(Int32 p_templateID, string p_description)
        {
            // method to add a paper set

            if (_whichDatabase == "Access")
            {
                // if the database is the Access database
                using (OleDbCommand command = new OleDbCommand())
                {
                    // using a disposable command add a paper set
                    command.Connection = _conOleDb;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "INSERT INTO PaperSets (TemplateID,Description,Price,RedemptionAvailable," +
                        "RedemptionPrice) VALUES (?, ?, ?, ?, ?)";
                    command.Parameters.AddWithValue("?", p_templateID);
                    command.Parameters.AddWithValue("?", p_description);
                    command.Parameters.AddWithValue("?", 0.0);
                    command.Parameters.AddWithValue("?", false);
                    command.Parameters.AddWithValue("?", 0.0);
                    try
                    {
                        // open database connection, execute the command
                        _conOleDb.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("Access addition of paper set " + p_description + " failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conOleDb.Close();
                    }
                }
            }
            else if (_whichDatabase == "MSSQL")
            {
                // if the database is the MSSQL database
                using (SqlCommand command = new SqlCommand())
                {
                    // using a disposable command add a paper set
                    command.Connection = _conSql;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "INSERT INTO PaperSets VALUES (@template,@desc,@price,@avail,@rprice)";
                    command.Parameters.AddWithValue("@template", p_templateID);
                    command.Parameters.AddWithValue("@desc", p_description);
                    command.Parameters.AddWithValue("@price", 0.0);
                    command.Parameters.AddWithValue("@avail", 0);
                    command.Parameters.AddWithValue("@rprice", 0.0);
                    try
                    {
                        // open database connection, execute the command
                        _conSql.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("MSSQL addition of paper set " + p_description + " failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conSql.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("PaperSet " + p_description + " not added. No database type specified.");
            }
        }
        public static void AddPullTab(string p_formID, string p_description)
        {
            // method to add a pulltab

            if (_whichDatabase == "Access")
            {
                // if the database is the Access database
                using (OleDbCommand command = new OleDbCommand())
                {
                    // using a disposable command add a pulltab
                    command.Connection = _conOleDb;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "INSERT INTO PullTabs VALUES (?,?,?,?,?,?)";
                    command.Parameters.AddWithValue("?", p_formID);
                    command.Parameters.AddWithValue("?", p_description);
                    command.Parameters.AddWithValue("?", 0.01);
                    command.Parameters.AddWithValue("?", 0.01);
                    command.Parameters.AddWithValue("?", 0.0);
                    command.Parameters.AddWithValue("?", true);
                    try
                    {
                        // open database connection, execute the command
                        _conOleDb.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("Access addition of pulltab " + p_description + " failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conOleDb.Close();
                    }
                }
            }
            else if (_whichDatabase == "MSSQL")
            {
                // if the database is the MSSQL database
                using (SqlCommand command = new SqlCommand())
                {
                    // using a disposable command add a pulltab
                    command.Connection = _conSql;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "INSERT INTO PullTabs VALUES (@formID,@desc,@gross,@prizes,@tax,@curr)";
                    command.Parameters.AddWithValue("@formID", p_formID);
                    command.Parameters.AddWithValue("@desc", p_description);
                    command.Parameters.AddWithValue("@gross", 0.01);
                    command.Parameters.AddWithValue("@prizes", 0.01);
                    command.Parameters.AddWithValue("@tax", 0.0);
                    command.Parameters.AddWithValue("@curr", 1);

                    try
                    {
                        // open database connection, execute the command
                        _conSql.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("MSSQL addition of pulltab " + p_description + " failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conSql.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("PullTab " + p_description + " not added. No database type specified.");
            }
        }
        public static int GetMaxGameNumber(Int32 p_templateID)
        {
            // method to add a return the max game number from Session Games based on template
            int maxNum = 1;
            if (_whichDatabase == "Access")
            {
                // if the database is the Access database
                using (OleDbCommand command = new OleDbCommand())
                {
                    // using a disposable command add a session bingo game
                    command.Connection = _conOleDb;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT (GameNumber + 1) FROM SessionBingoGames WHERE TemplateID = ?";
                    command.Parameters.AddWithValue("?", p_templateID);
                    try
                    {
                        // open database connection, execute the command
                        _conOleDb.Open();
                        maxNum = (int)command.ExecuteScalar();
                    }
                    catch (Exception)
                    {
                        // if error return maxNum 1
                        return maxNum;
                    }
                    finally
                    {
                        // close the database connection
                        _conOleDb.Close();
                    }
                }
            }
            else if (_whichDatabase == "MSSQL")
            {
                // if the database is the MSSQL database
                using (SqlCommand command = new SqlCommand())
                {
                    // using a disposable command add a session bingo game
                    command.Connection = _conSql;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT (GameNumber + 1) FROM SessionBingoGames WHERE TemplateID = @id";
                    command.Parameters.AddWithValue("@id", p_templateID);
                    try
                    {
                        // open database connection, execute the command
                        _conSql.Open();
                        maxNum = (int)command.ExecuteScalar();
                    }
                    catch (Exception)
                    {
                        // if error return maxNum 1
                        return maxNum;
                    }
                    finally
                    {
                        // close the database connection
                        _conSql.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Fill of max game number failed. No database type specified.");
            }

            return maxNum;
        }
        public static void AddSessionBingoGame(Int32 p_templateID, string p_description)
        {
            // method to add a session bingo game
            int maxGameNum = GetMaxGameNumber(p_templateID);
            if (_whichDatabase == "Access")
            {
                // if the database is the Access database
                using (OleDbCommand command = new OleDbCommand())
                {
                    // using a disposable command add a session bingo game
                    command.Connection = _conOleDb;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "INSERT INTO SessionBingoGames (TemplateID,GameNumber,Description," +
                        "MaxOrSetPrize,CardID1,CardID2,CardID3,EarlyBird,ChangePaidOut,FiftyFifty,ColorRed,ColorGreen," +
                        "ColorBlue) VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?)";
                    command.Parameters.AddWithValue("?", p_templateID);
                    command.Parameters.AddWithValue("?", maxGameNum);
                    command.Parameters.AddWithValue("?", p_description);
                    command.Parameters.AddWithValue("?", 0.0);
                    command.Parameters.AddWithValue("?", -1);
                    command.Parameters.AddWithValue("?", -1);
                    command.Parameters.AddWithValue("?", -1);
                    command.Parameters.AddWithValue("?", false);
                    command.Parameters.AddWithValue("?", true);
                    command.Parameters.AddWithValue("?", false);
                    command.Parameters.AddWithValue("?", 0);
                    command.Parameters.AddWithValue("?", 0);
                    command.Parameters.AddWithValue("?", 0);
                    try
                    {
                        // open database connection, execute the command
                        _conOleDb.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("Access addition of session bingo game " + p_description + " failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conOleDb.Close();
                    }
                }
            }
            else if (_whichDatabase == "MSSQL")
            {
                // if the database is the MSSQL database
                using (SqlCommand command = new SqlCommand())
                {
                    // using a disposable command add a session bingo game
                    command.Connection = _conSql;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "INSERT INTO SessionBingoGames VALUES (@temp,@gameno,@desc,@prize,@card1,@card2,@card3,@eb,@change,@fifty,@red,@green,@blue)";
                    command.Parameters.AddWithValue("@temp", p_templateID);
                    command.Parameters.AddWithValue("@gameno", GetMaxGameNumber(p_templateID));
                    command.Parameters.AddWithValue("@desc", p_description);
                    command.Parameters.AddWithValue("@prize", 0);
                    command.Parameters.AddWithValue("@card1", 0);
                    command.Parameters.AddWithValue("@card2", 0);
                    command.Parameters.AddWithValue("@card3", 0);
                    command.Parameters.AddWithValue("@eb", 0);
                    command.Parameters.AddWithValue("@change", 1);
                    command.Parameters.AddWithValue("@fifty", 0);
                    command.Parameters.AddWithValue("@red", 0);
                    command.Parameters.AddWithValue("@green", 0);
                    command.Parameters.AddWithValue("@blue", 0);
                    try
                    {
                        // open database connection, execute the command
                        _conSql.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("MSSQL addition of session bingo game " + p_description + " failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conSql.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("SessionBingoGame " + p_description + " not added. No database type specified.");
            }
        }
        public static void AddSessionTemplate(string p_templateName)
        {
            // method to add a session template

            if (_whichDatabase == "Access")
            {
                // if the database is the Access database
                using (OleDbCommand command = new OleDbCommand())
                {
                    // using a disposable command add a session template
                    command.Connection = _conOleDb;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "INSERT INTO SessionTemplates (TemplateName) VALUES (?)";
                    command.Parameters.AddWithValue("?", p_templateName);
                    try
                    {
                        // open database connection, execute the command
                        _conOleDb.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("Access addition of session template " + p_templateName + " failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conOleDb.Close();
                    }
                }
            }
            else if (_whichDatabase == "MSSQL")
            {
                // if the database is the MSSQL database
                using (SqlCommand command = new SqlCommand())
                {
                    // using a disposable command add a session template
                    command.Connection = _conSql;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "INSERT INTO SessionTemplates VALUES (@template)";
                    command.Parameters.AddWithValue("@template", p_templateName);
                    try
                    {
                        // open database connection, execute the command
                        _conSql.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("MSSQL addition of session template " + p_templateName + " failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conSql.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("SessionTemplate " + p_templateName + " not added. No database type specified.");
            }
        }
        public static void AddWorker(string p_name)
        {
            // method to add a worker

            if (_whichDatabase == "Access")
            {
                // if the database is the Access database
                using (OleDbCommand command = new OleDbCommand())
                {
                    // using a disposable command add a worker
                    command.Connection = _conOleDb;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "INSERT INTO Workers (Name) VALUES (?)";
                    command.Parameters.AddWithValue("?", p_name);
                    try
                    {
                        // open database connection, execute the command
                        _conOleDb.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("Access addition of worker " + p_name + " failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conOleDb.Close();
                    }
                }
            }
            else if (_whichDatabase == "MSSQL")
            {
                // if the database is the MSSQL database
                using (SqlCommand command = new SqlCommand())
                {
                    // using a disposable command add a worker
                    command.Connection = _conSql;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "INSERT INTO Workers VALUES (@name)";
                    command.Parameters.AddWithValue("@name", p_name);
                    try
                    {
                        // open database connection, execute the command
                        _conSql.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // if error in try, send error message
                        MessageBox.Show("MSSQL addition of worker " + p_name + " failed. Error: " + ex.Message);
                    }
                    finally
                    {
                        // close the database connection
                        _conSql.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Worker " + p_name + " not added. No database type specified.");
            }
        }
        #endregion AddMethods
    }
}
