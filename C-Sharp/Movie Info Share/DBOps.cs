
using MySql.Data.MySqlClient;
using System.Data;
using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;

namespace StonesJ_AdvancedDB
{
    class DBOps
    {
        // create sever variables for connection to database
        private const string CONNECT_STRING = @"Server=********;Database=********;Uid=********;Pwd=********";
        private static MySqlConnection _con = new MySqlConnection(CONNECT_STRING);
        private static MySqlDataAdapter _daITSE2338 = new MySqlDataAdapter();
        private static DataSet _dsMovies = new DataSet();
        private static string _dtMovies = "Movies";
        private static DataSet _dsUsers = new DataSet();
        private static string _dtUsers = "Users";
        // variable to store user of program signed in
        private static string _currentUser = "";
        // variables to store data about a movie for update and add functionality
        private static Int32 _movieID;
        private static string _movieName;
        private static string _genre;
        private static string _director;
        private static string _star1;
        private static string _star2;
        private static string _star3;
        private static string _description;
        private static Int32 _runTime;
        private static Int32 _year;
        private static string _rating;
        private static Int32 _userID;
        private static string _img;
        private static List<string> _imageFilenames = new List<string>();
        private static List<int> _imageIndexes = new List<int>();
        #region "Properties"
        //properties for all of the necessary private variables above
        public static DataSet dsMovies
        {
            get { return _dsMovies; }
            set { _dsMovies = value; }
        }
        public static string dtMovies
        {
            get { return _dtMovies; }
            set { _dtMovies = value; }
        }
        public static DataSet dsUsers
        {
            get { return _dsUsers; }
            set { _dsUsers = value; }
        }
        public static string dtUsers
        {
            get { return _dtUsers; }
            set { _dtUsers = value; }
        }
        public static string currentUser
        {
            get { return _currentUser; }
            set { _currentUser = value; }
        }
        public static Int32 movieID
        {
            get { return _movieID; }
            set { _movieID = value; }
        }
        public static string movieName
        {
            get { return _movieName; }
            set { _movieName = value; }
        }
        public static string genre
        {
            get { return _genre; }
            set { _genre = value; }
        }
        public static string director
        {
            get { return _director; }
            set { _director = value; }
        }
        public static string star1
        {
            get { return _star1; }
            set { _star1 = value; }
        }
        public static string star2
        {
            get { return _star2; }
            set { _star2 = value; }
        }
        public static string star3
        {
            get { return _star3; }
            set { _star3 = value; }
        }
        public static string description
        {
            get { return _description; }
            set { _description = value; }
        }
        public static Int32 runTime
        {
            get { return _runTime; }
            set { _runTime = value; }
        }
        public static Int32 year
        {
            get { return _year; }
            set { _year = value; }
        }
        public static string rating
        {
            get { return _rating; }
            set { _rating = value; }
        }
        public static Int32 userID
        {
            get { return _userID; }
            set { _userID = value; }
        }
        public static string img
        {
            get { return _img; }
            set { _img = value; }
        }
        public static List<String> imageFilenames
        {
            get { return _imageFilenames; }
            set { _imageFilenames = value; }
        }
        public static List<int> imageIndexes
        {
            get { return _imageIndexes; }
            set { _imageIndexes = value; }
        }
        #endregion
        public static void writeImages()
        {
            StreamWriter sw = File.CreateText("..\\..\\Images\\images.dat");
            for(int i = 0; i < _imageIndexes.Count; i++)
            {
                sw.WriteLine(_imageIndexes[i].ToString() + "," + _imageFilenames[i]);
            }
            sw.Close();
        }
        public static void getMovies()
        {
            // queries the database to get all of the rows and columns from the Movies table
            // and fills the dataset with that information
                           
            // create a new command
            using (MySqlCommand cmd = new MySqlCommand())
            {
                // set command to existing connection
                cmd.Connection = _con;
                // specify command type (text)        
                cmd.CommandType = CommandType.Text;
                // set command text
                cmd.CommandText = "SELECT m.MovieID, m.MovieName, m.Genre, m.Director, m.Star1, " +
                    "m.Star2, m.Star3, m.Description, m.RunTime, m.Year, m.Rating, m.UserID, u.Username " +
                    "FROM Movies m INNER JOIN Users u ON m.UserID = u.UserID";
                // assign this connection to data adapter
                _daITSE2338.SelectCommand = cmd;
                try
                {
                    // open connection
                    _con.Open();
                    // clear dataset and fill it with the query info
                    _dsMovies.Clear();
                    _daITSE2338.Fill(_dsMovies, _dtMovies);
                }
                catch (MySqlException)
                {
                    // error occured loading users
                }
                finally
                {
                    // close connection
                    _con.Close();
                }
            }
        }
        public static void getMovies(string username)
        {
            // queries the database to get all of the rows and columns from the Movies table
            // and fills the dataset with that information

            // create a new command
            using (MySqlCommand cmd = new MySqlCommand())
            {
                // set command to existing connection
                cmd.Connection = _con;
                // specify command type (text)        
                cmd.CommandType = CommandType.Text;
                // set command text
                cmd.CommandText = "SELECT m.MovieID, m.MovieName, m.Genre, m.Director, m.Star1, " +
                    "m.Star2, m.Star3, m.Description, m.RunTime, m.Year, m.Rating, m.UserID, u.Username " +
                    "FROM Movies m INNER JOIN Users u ON m.UserID = u.UserID WHERE u.Username = @user";
                // assign this connection to data adapter
                _daITSE2338.SelectCommand = cmd;
                try
                {
                    // open connection
                    _con.Open();
                    // set parameter
                    cmd.Parameters.AddWithValue("@user", username);
                    // clear dataset and fill it with the query info
                    _dsMovies.Clear();
                    _daITSE2338.Fill(_dsMovies, _dtMovies);
                }
                catch (MySqlException)
                {
                    // error occured loading users
                }
                finally
                {
                    // close connection
                    _con.Close();
                }
            }
        }
        public static void getUsers()
        {
            // queries the database to get all of the rows and columns from the Users table
            // and fills the dataset with that information

            // create a new command
            using (MySqlCommand cmd = new MySqlCommand())
            {
                // set command to existing connection
                cmd.Connection = _con;
                // specify command type (text)        
                cmd.CommandType = CommandType.Text;
                // set command text
                cmd.CommandText = "SELECT * FROM Users";
                // assign this connection to data adapter
                _daITSE2338.SelectCommand = cmd;
                try
                {
                    // open connection
                    _con.Open();
                    // clear dataset and fill it with the query info
                    _dsUsers.Clear();
                    _daITSE2338.Fill(_dsUsers, _dtUsers);
                }
                catch(MySqlException)
                {
                    // error occured loading users
                }
                finally
                {
                    // close connection
                    _con.Close();
                }
            }
        }

        public static void formatGrid(DataGridView dgv)
        {
            // method to set up the datagridview to show only the desired information
            // in a way that is pleasing and useful to the end user

            // restrict user run-time customization of DGV
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToResizeColumns = false;
            dgv.AllowUserToResizeRows = false;
            dgv.AutoGenerateColumns = false;
            dgv.AllowUserToOrderColumns = false;
            dgv.AllowDrop = false;
            dgv.MultiSelect = false;
            foreach (DataGridViewColumn column in dgv.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            // a whole row is selected at a time
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            // hide instead of remove unwanted columns - this allows them to still be used to get info from
            dgv.RowHeadersVisible = false;
            dgv.Columns[0].Visible = false;
            dgv.Columns[2].Visible = false;
            dgv.Columns[3].Visible = false;
            dgv.Columns[4].Visible = false;
            dgv.Columns[5].Visible = false;
            dgv.Columns[6].Visible = false;
            dgv.Columns[7].Visible = false;
            dgv.Columns[8].Visible = false;
            dgv.Columns[9].Visible = false;
            dgv.Columns[10].Visible = false;
            dgv.Columns[11].Visible = false;
            dgv.Columns[1].HeaderText = "Movie Name";
            dgv.Columns[12].HeaderText = "Owner";
            // autosize columns and rows to fit data in them
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.Location = new System.Drawing.Point(dgv.Parent.Width / 2 - dgv.Width / 2, dgv.Location.Y);
            // clear initial selection
            dgv.ClearSelection();
        }

        public static bool ValidUsername(string username)
        {
            if(username.Length < 3)
            {
                // would have set to 6 minimum, but initial user has length of 4
                MessageBox.Show("Username must be at least 3 characters long.");
                return false;
            }
            // bool to test for a letter (required)
            bool hasLetter = false;
            foreach(char ch in username)
            {
                if (!ValidLetter(ch) && !ValidNumber(ch) && ch != '.' && ch != '-' && ch != '_')
                {
                    // user POSIX standards to enforce portability of the username - if standards not met
                    // return error to user and end click event
                    MessageBox.Show("Username may only contain letters, numbers, or the following characters:  . - _");
                    return false;
                }
                // make sure at least one letter included in username
                if (!hasLetter & ValidLetter(ch))
                    hasLetter = true;
            }
            if(!hasLetter)
            {
                // if no letters, display error
                MessageBox.Show("Username must contain at least one letter.");
                return false;
            }
            // username valid at this point
            return true;
        }
        public static bool ValidPassword(string password)
        {
            // method to test for valid password
            
            // test password length first, min 6 characters
            if(password.Length < 6)
            {
                MessageBox.Show("Password must be at least 6 characters long.");
                return false;
            }
            // bools to test for required characters in password
            bool hasLowerLetter = false;
            bool hasUpperLetter = false;
            bool hasNumber = false;
            bool hasSymbol = false;
            bool validPassword = true;

            foreach(char ch in password)
            {
                // test each character
                if (!ValidLetter(ch) && !ValidNumber(ch) && !ValidSymbol(ch))
                {
                    // if not a letter, number or valid symbol, show error and return false
                    MessageBox.Show("Password may only contain letters, numbers and visible symbol characters.");
                    return false;
                }
                // test to make sure password has all required parts: 
                // upper letter, lower letter, number, symbol
                if (!hasLowerLetter & (ch >= 'a' && ch <= 'z'))
                    hasLowerLetter = true;
                if (!hasUpperLetter & (ch >= 'A' && ch <= 'Z'))
                    hasUpperLetter = true;
                if (!hasNumber & ValidNumber(ch))
                    hasNumber = true;
                if (!hasSymbol & ValidSymbol(ch))
                    hasSymbol = true;
            }
            // create string to concat errMessage to
            string errMessage = "Invalid password. Your password is missing: \n";
            if(!hasLowerLetter)
            {
                // if no lower, add to message
                validPassword = false;
                errMessage += "-- a lowercase letter\n";
            }
            if(!hasUpperLetter)
            {
                // if no upper, add to message
                validPassword = false;
                errMessage += "-- an uppercase letter\n";
            }
            if(!hasNumber)
            {
                // if no number, add to message
                validPassword = false;
                errMessage += "-- a number\n";
            }
            if(!hasSymbol)
            {
                // if no symbol, add to message
                validPassword = false;
                errMessage += "-- a non-alphanumeric character (e.g., $ # ! > *  etc.)";
            }
            if (!validPassword)
            {
                // if invalid password, show entire message
                MessageBox.Show(errMessage);
                return false;
            }
            // passed the test at this point, valid password
            else
                return true;
        }

        private static bool ValidLetter(char ch)
        {
            // test a single char to see if it's a letter
            if (ch >= 'a' && ch <= 'z')
                return true;
            else if (ch >= 'A' && ch <= 'Z')
                return true;
            else
                return false;
        }
        private static bool ValidNumber(char ch)
        {
            // test a single char to see if it's a number
            if (ch >= '0' && ch <= '9')
                return true;
            else
                return false;
        }
        private static bool ValidSymbol(char ch)
        {
            // test a single char to see if it's a non-alphanumeric, visible character
            if ((ch >= 33 && ch <= 47) ||
                    (ch >= 58 && ch <= 64) ||
                    (ch >= 91 && ch <= 96) ||
                    (ch >= 123 && ch <= 126))
                return true;
            else
                return false;
        }
        private static Int32 nextMovieIndex()
        {
            // method to return max + 1 index value for insert

            // create new command
            using (MySqlCommand cmd = new MySqlCommand())
            {
                // create variable to hold maxID
                Int32 next = 0;
                // set command to existing connection
                cmd.Connection = _con;
                // specify command type (text)
                cmd.CommandType = CommandType.Text;
                // set command text
                cmd.CommandText = "SELECT MAX(MovieID) FROM `Movies`";
                try
                {
                    // open database connection and execute command
                    _con.Open();
                    next = (Int32)cmd.ExecuteScalar() + 1;
                }
                catch (MySqlException)
                {
                    // error occurred - will return 0 and return error in AddMovie
                }
                finally
                {
                    // close connection
                    _con.Close();
                }
                return next;
            }
        }
        private static Int32 nextUserIndex()
        {
            // method to return max + 1 index value for insert
            
            // create a new command
            using (MySqlCommand cmd = new MySqlCommand())
            {
                // create variable to hold maxID
                Int32 next = 0;
                // set command to existing connection
                cmd.Connection = _con;
                // specify command type (text)        
                cmd.CommandType = CommandType.Text;
                // set command text
                cmd.CommandText = "SELECT MAX(UserID) FROM `Users`";
                try
                {
                    // open database connection and execute command
                    _con.Open();
                    next = (Int32)cmd.ExecuteScalar() + 1; 
                }
                catch (MySqlException)
                {
                    // error occurred - will return 0 and return error in AddUser
                }
                finally
                {
                    // close connection
                    _con.Close();
                }
                return next;
            }

        }
        public static void RemoveMovie(Int32 index)
        {
            // method to remove a user from users if needed

            // create a new command
            using (MySqlCommand cmd = new MySqlCommand())
            {
                // set command to existing connection
                cmd.Connection = _con;
                // specify command type (text)        
                cmd.CommandType = CommandType.Text;
                // set command text
                cmd.CommandText = "DELETE FROM `Movies` WHERE MovieID = @index";
                // fill parameters of command
                cmd.Parameters.AddWithValue("@index", index);
                try
                {
                    // open database connection and execute command
                    _con.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Couldn't remove movie. Error: " + ex.Message);
                }
                finally
                {
                    // close connection
                    _con.Close();
                }
            }
        }
        public static void RemoveUser(string username)
        {
            // method to remove a user from users if needed

            // create a new command
            using (MySqlCommand cmd = new MySqlCommand())
            {
                // set command to existing connection
                cmd.Connection = _con;
                // specify command type (text)        
                cmd.CommandType = CommandType.Text;
                // set command text
                cmd.CommandText = "DELETE FROM `Users` WHERE Username = @user";
                // fill parameters of command
                cmd.Parameters.AddWithValue("@user", username);
                try
                {
                    // open database connection and execute command
                    _con.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Couldn't remove user. Error: " + ex.Message);
                }
                finally
                {
                    // close connection
                    _con.Close();
                }
            }                
        }
        public static void AddMovie()
        {
            // method to add a movie to the movies table

            // create a new command
            using (MySqlCommand cmd = new MySqlCommand())
            {
                // set index and userID
                movieID = nextMovieIndex();
                userID = getCurrentUserID();
                if(userID == 0)
                {
                    MessageBox.Show("Unable to retreive index for current user.");
                    return;
                }
                if (movieID == 0)
                {
                    MessageBox.Show("Error inserting movie because unable to retrieve index for insert.");
                    return;
                }
                //set command to existing connection
                cmd.Connection = _con;
                //specify command type(text)
                cmd.CommandType = CommandType.Text;
                //set command text
                cmd.CommandText = "INSERT INTO Movies (MovieID, MovieName, Genre, Director, Star1, " +
                    "Star2, Star3, Description, RunTime, Year, Rating, UserID) Values (@index, @movie, " + 
                    "@genre, @director, @star1, @star2, @star3, @description, @runTime, " +
                    "@year, @rating, @userID)";
                //fill parameters of command
                cmd.Parameters.AddWithValue("@index", movieID);
                cmd.Parameters.AddWithValue("@movie", movieName);
                cmd.Parameters.AddWithValue("@genre", genre);
                cmd.Parameters.AddWithValue("@director", director);
                cmd.Parameters.AddWithValue("@star1", star1);
                cmd.Parameters.AddWithValue("@star2", star2);
                cmd.Parameters.AddWithValue("@star3", star3);
                cmd.Parameters.AddWithValue("@description", description);
                if (runTime == 0)
                    cmd.Parameters.AddWithValue("@runTime", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@runTime", runTime);
                if (year == 0)
                    cmd.Parameters.AddWithValue("year", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@rating", rating);
                cmd.Parameters.AddWithValue("@userID", userID);
                try
                {
                    //open database connection and execute command
                    _con.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    // display error
                    MessageBox.Show("Couldn't add movie. Error: " + ex.Message);
                }
                finally
                {
                    //close connection
                    _con.Close();
                }
            }
            getMovies(currentUser);
        }

        public static void AddUser(string username, string password)
        {
            // method to add a user to the users table

            // create a new command
            using (MySqlCommand cmd = new MySqlCommand())
            {
                Int32 ID = nextUserIndex();
                if (ID == 0)
                {
                    MessageBox.Show("Error inserting user because unable to retrieve index for insert.");
                    return;
                }
                //set command to existing connection
                cmd.Connection = _con;
                //specify command type(text)
                cmd.CommandType = CommandType.Text;
                //set command text
                cmd.CommandText = "INSERT INTO Users (UserID, Username, Password) Values (@index, @user, @pass)";
                //fill parameters of command
                cmd.Parameters.AddWithValue("@index", ID);
                cmd.Parameters.AddWithValue("@user", username);
                cmd.Parameters.AddWithValue("@pass", password);
                try
                {
                    //open database connection and execute command
                    _con.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Couldn't add user. Error: " + ex.Message);
                }
                finally
                {
                    //close connection
                    _con.Close();
                }
            }

            getUsers(); 
        }
        
        public static void UpdateMovie()
        {
            // method to update a movie to the movies table

            // create a new command
            using (MySqlCommand cmd = new MySqlCommand())
            {
                // set userID
                userID = getCurrentUserID();
                if (userID == 0)
                {
                    MessageBox.Show("Unable to retreive index for current user.");
                    return;
                }
                //set command to existing connection
                cmd.Connection = _con;
                //specify command type(text)
                cmd.CommandType = CommandType.Text;
                //set command text
                cmd.CommandText = "UPDATE Movies SET MovieName = @movie, Genre = @genre, " +
                    "Director = @director, Star1 = @star1, Star2 = @star2, Star3 = @star3, " +
                    "Description = @description, RunTime = @runTime, Year = @year, Rating = @rating, " +
                    "UserID = @userID WHERE MovieID = @index";
                //fill parameters of command
                cmd.Parameters.AddWithValue("@movie", movieName);
                cmd.Parameters.AddWithValue("@genre", genre);
                cmd.Parameters.AddWithValue("@director", director);
                cmd.Parameters.AddWithValue("@star1", star1);
                cmd.Parameters.AddWithValue("@star2", star2);
                cmd.Parameters.AddWithValue("@star3", star3);
                cmd.Parameters.AddWithValue("@description", description);
                if (runTime == 0)
                    cmd.Parameters.AddWithValue("@runTime", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@runTime", runTime);
                if (year == 0)
                    cmd.Parameters.AddWithValue("year", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@rating", rating);
                cmd.Parameters.AddWithValue("@userID", userID);
                cmd.Parameters.AddWithValue("@index", movieID);
                try
                {
                    //open database connection and execute command
                    _con.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    // display error
                    MessageBox.Show("Couldn't add movie. Error: " + ex.Message);
                }
                finally
                {
                    //close connection
                    _con.Close();
                }
            }
            getMovies(currentUser);
        }
        private static Int32 getCurrentUserID()
        {
            // method to return ID value for current user

            // create a new command
            using (MySqlCommand cmd = new MySqlCommand())
            {
                // create variable to hold index
                Int32 index = 0;
                // set command to existing connection
                cmd.Connection = _con;
                // specify command type (text)        
                cmd.CommandType = CommandType.Text;
                // set command text
                cmd.CommandText = "SELECT UserID FROM `Users` WHERE Username = @user";
                // set parameter
                cmd.Parameters.AddWithValue("@user", currentUser);
                try
                {
                    // open database connection and execute command
                    _con.Open();
                    index = (Int32)cmd.ExecuteScalar();
                }
                catch (MySqlException)
                {
                    // error occurred - will return 0 and return error in AddUser
                }
                finally
                {
                    // close connection
                    _con.Close();
                }
                return index;
            }
        }
    }
}
