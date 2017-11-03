using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stones_MediaLab
{
    struct User
    {
        // create user struct in namespace to hold user info
        public string Username;
        public string Password;
        public string Theme;
        public string HomeURL;
        public string EmailURL;
        public string SearchURL;
    }
    struct UserFavorites
    {
        // create favorites structure in namespace to hold a single favorite site
        public Int32 ID;
        public string URL;
        public string Alias;
        public bool BookmarksBar;
        public Image icon;
    }
    struct UserHistory
    {
        // create history structure in namespace to hold a single history site
        public Int32 ID;
        public string URL;
        public string Stamp;
    }
    class DBOps
    {
        //connection and database variables
        private const string CONNECT = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=..\\..\\Data\\AmpereBrowser.mdb; Persist Security Info=False;";
        private static OleDbConnection _con = new OleDbConnection(CONNECT);
        private static OleDbDataAdapter _dbAdapter = new OleDbDataAdapter();
        private static DataSet _dsUser = new DataSet();
        private static string _dtUser = "User";
        private static DataSet _dsUsers = new DataSet();
        private static string _dtUsers = "Users";
        private static DataSet _dsUserHistory = new DataSet();
        private static string _dtUserHistory = "UserHistory";
        private static DataSet _dsUserFavorites = new DataSet();
        private static string _dtUserFavorites = "UserFavorites";
        // user struct for user to hold current user data
        public static Stones_MediaLab.User CurrentUser;
        // use favorites struct to create list for application (and to read/write from database)
        private static List<UserFavorites> _favorites = new List<UserFavorites>();
        // use history struct to create list for application (and to read/write from database)
        private static List<UserHistory> _history = new List<UserHistory>();

        // properties region so can stay hidden
        #region Properties

        public static OleDbConnection Con
        {
            get
            {
                return _con;
            }

            set
            {
                _con = value;
            }
        }

        public static OleDbDataAdapter DbAdapter
        {
            get
            {
                return _dbAdapter;
            }

            set
            {
                _dbAdapter = value;
            }
        }

        public static DataSet DsUser
        {
            get
            {
                return _dsUser;
            }

            set
            {
                _dsUser = value;
            }
        }

        public static string DtUser
        {
            get
            {
                return _dtUser;
            }

            set
            {
                _dtUser = value;
            }
        }

        public static DataSet DsUserHistory
        {
            get
            {
                return _dsUserHistory;
            }

            set
            {
                _dsUserHistory = value;
            }
        }

        public static string DtUserHistory
        {
            get
            {
                return _dtUserHistory;
            }

            set
            {
                _dtUserHistory = value;
            }
        }

        public static DataSet DsUserFavorites
        {
            get
            {
                return _dsUserFavorites;
            }

            set
            {
                _dsUserFavorites = value;
            }
        }

        public static string DtUserFavorites
        {
            get
            {
                return _dtUserFavorites;
            }

            set
            {
                _dtUserFavorites = value;
            }
        }

        internal static List<UserFavorites> Favorites
        {
            get
            {
                return _favorites;
            }

            set
            {
                _favorites = value;
            }
        }

        internal static List<UserHistory> History
        {
            get
            {
                return _history;
            }

            set
            {
                _history = value;
            }
        }

        public static DataSet DsUsers
        {
            get
            {
                return _dsUsers;
            }

            set
            {
                _dsUsers = value;
            }
        }

        public static string DtUsers
        {
            get
            {
                return _dtUsers;
            }

            set
            {
                _dtUsers = value;
            }
        }


        #endregion Properties

        public static void GetUsers()
        {
            // method to connect to database and pull a single user's information
            using (OleDbCommand command = new OleDbCommand())
            {
                // set up command connection, text and params
                command.Connection = Con;
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT * FROM Users";
                // associate adapter with commmand
                DbAdapter.SelectCommand = command;

                try
                {
                    // open database connection
                    Con.Open();
                    // clear user dataset
                    DsUsers.Clear();
                    // refill dataset
                    DbAdapter.Fill(DsUsers, DtUsers);
                }
                catch (Exception ex)
                {
                    // error message if could not find username, ends method
                    MessageBox.Show("Unable to load users..." + ex.Message);
                }
                finally
                {
                    // ensure connection to database is closed
                    Con.Close();
                }
            }
        }
        public static bool ConnectUser(string p_username, string p_password)
        {
            // method to connect to database and pull a single user's information
            using (OleDbCommand command = new OleDbCommand())
            {
                // set up command connection, text and params
                command.Connection = Con;
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT * FROM Users WHERE Username = ?";
                command.Parameters.AddWithValue("?", p_username);
                // associate adapter with commmand
                DbAdapter.SelectCommand = command;

                try
                {
                    // open database connection
                    Con.Open();
                    // clear user dataset
                    DsUser.Clear();
                    // refill dataset
                    DbAdapter.Fill(DsUser, DtUser);
                    // set current user info if successful
                    CurrentUser.Username = DsUser.Tables[DtUser].Rows[0].Field<string>(0);
                    CurrentUser.Password = DsUser.Tables[DtUser].Rows[0].Field<string>(1);
                    CurrentUser.Theme = DsUser.Tables[DtUser].Rows[0].Field<string>(2);
                    CurrentUser.HomeURL = DsUser.Tables[DtUser].Rows[0].Field<string>(3);
                    CurrentUser.EmailURL = DsUser.Tables[DtUser].Rows[0].Field<string>(4);
                    CurrentUser.SearchURL = DsUser.Tables[DtUser].Rows[0].Field<string>(5);
                }
                catch (Exception)
                {
                    // error message if could not find username, ends method
                    MessageBox.Show("Username Not Found");
                    // close connection in case of error (because ending method)
                    // new user data will not be set in webbrowser instance
                    Con.Close();
                    return false;
                }
                finally
                {
                    // ensure connection to database is closed
                    Con.Close();
                }
            }

            if (CurrentUser.Password != p_password)
            {
                // if password doesn't match that which is in the database
                // return an error and end method
                MessageBox.Show("Username and password do not match.");
                return false;
            }

            // set up counter for history to only run following using
            // if there are records to return.

            int historyCount = 0;
            using (OleDbCommand command = new OleDbCommand())
            {
                // set up command connection, text, and params
                command.Connection = Con;
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT COUNT(*) FROM UserHistory WHERE Username = ?";
                command.Parameters.AddWithValue("?", p_username);
                try
                {
                    // open connection to database
                    Con.Open();
                    // alter history count to display number of history items for 
                    // user signing on
                    historyCount = (int)command.ExecuteScalar();
                }
                catch (Exception)
                {
                    // display error message if something goes wrong
                    MessageBox.Show("Error loading user history.");
                }
                finally
                {
                    // ensure connection is closed to database
                    Con.Close();
                }
            }


            if (historyCount > 0)
            {
                // if there is history to pull, pull it
                using (OleDbCommand command = new OleDbCommand())
                {
                    // set up command connection, text, and params
                    command.Connection = Con;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT * FROM UserHistory WHERE Username = ?";
                    command.Parameters.AddWithValue("?", p_username);
                    // associate data adapter with command
                    DbAdapter.SelectCommand = command;

                    try
                    {
                        // open the connection to the database
                        Con.Open();
                        // clear the history database
                        DsUserHistory.Clear();
                        // refill history dataset from database based on user signing on
                        DbAdapter.Fill(DsUserHistory, DtUserHistory);
                        History.Clear();
                        foreach (DataRow r in DsUserHistory.Tables["UserHistory"].Rows)
                        {
                            // loop throw rows of history table and add history object to
                            // the history list for the user currently signing on

                            // use a temporary object to store data in the history list
                            UserHistory temp = new Stones_MediaLab.UserHistory();
                            temp.ID = r.Field<Int32>(0);
                            temp.URL = r.Field<string>(1);
                            temp.Stamp = r.Field<string>(2);
                            // add temp object to list
                            History.Add(temp);
                        }
                    }
                    catch (Exception)
                    {
                        // if unable to load user history, display error message
                        MessageBox.Show("Unable to load user history");
                    }
                    finally
                    {
                        // ensure the connection is closed to the database
                        Con.Close();
                    }
                }
            }

            // set up counter to test for existence of favorites
            int favoritesCount = 0;
            using (OleDbCommand command = new OleDbCommand())
            {
                // set up command connection, text, and params
                command.Connection = Con;
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT COUNT(*) FROM UserFavorites WHERE Username = ?";
                command.Parameters.AddWithValue("?", p_username);

                try
                {
                    // open database connection
                    Con.Open();
                    // set favorites counter 
                    favoritesCount = (int)command.ExecuteScalar();
                }
                catch (Exception)
                {
                    // if error, display it to user
                    MessageBox.Show("Error loading user favorites.");
                }
                finally
                {
                    // ensure the connection is closed to the database
                    Con.Close();
                }
            }

            if (favoritesCount > 0)
            {
                // if there are favorites to pull in, pull them in
                using (OleDbCommand command = new OleDbCommand())
                {
                    // set up command connection, text, and params
                    command.Connection = Con;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT * FROM UserFavorites WHERE Username = ?";
                    command.Parameters.AddWithValue("?", p_username);
                    // associate data adapter with command
                    DbAdapter.SelectCommand = command;
                    try
                    {
                        // open database connection
                        Con.Open();
                        // clear favorites dataset
                        DsUserFavorites.Clear();
                        // refill favorites from database for user signing on
                        DbAdapter.Fill(DsUserFavorites, DtUserFavorites);
                        Favorites.Clear();
                        Con.Close();
                        foreach (DataRow r in DsUserFavorites.Tables[DtUserFavorites].Rows)
                        {
                            // for every row in the favorites table, add favorite to list
                            // to use in application
                            // user temporary object to store database row info
                            UserFavorites temp = new UserFavorites();
                            temp.ID = r.Field<Int32>(0);
                            temp.URL = r.Field<string>(2);
                            temp.Alias = r.Field<string>(3);
                            temp.icon = GetFavicon(temp.URL);
                            temp.BookmarksBar = true;
                            // insert temp object into list
                            Favorites.Add(temp);
                        }
                    }
                    catch (Exception)
                    {
                        // if error, show user
                        MessageBox.Show("Unable to load user favorites");
                    }
                    finally
                    {
                        // ensure database connection is closed
                        Con.Close();
                    }
                }
            }
            // even if error in pulling in history or favorites, user may still connect
            // may change this later...
            return true;
        }


        public static void DisconnectUser()
        {
            History.Clear();
            Favorites.Clear();
            CurrentUser.Username = "";
            CurrentUser.Password = "";
            CurrentUser.EmailURL = "http://gmail.com";
            CurrentUser.HomeURL = "https://en.wikipedia.org/wiki/Ampere";
            CurrentUser.SearchURL = "http://www.google.com";
            CurrentUser.Theme = "Ampere";
        }

        public static void AddFavorite(string p_url, string p_alias)
        {
            // method to add a favorite to the list (will write to database as user ends session)
            bool add = true;
            string alias = "";
            foreach(UserFavorites uf in Favorites)
            {
                if(uf.URL == p_url)
                {
                    add = false;
                    alias = uf.Alias;
                }
            }
            if (add)
            {
                UserFavorites temp = new UserFavorites();
                temp.URL = p_url;
                temp.Alias = p_alias;
                temp.BookmarksBar = true;
                temp.icon = GetFavicon(p_url);
                Favorites.Add(temp);
            }
            else
            {
                MessageBox.Show("This favorite URL already exists. Please delete the tab that says \"" + alias + "\" before you can add this URL again.");
            }
        }

        public static void RemoveFavorite(string p_url, string p_alias)
        {
            // method to allow a user to remove on of the favorites from their list
            int ctr = 0;
            foreach (UserFavorites uf in Favorites)
            {
                if (uf.URL == p_url && uf.Alias == p_alias)
                {
                    // match both url and alias for secure removal
                    Favorites.RemoveAt(ctr);
                    return;
                }
                ctr++;
            }
        }

        public static void ClearHistory()
        {
            // method so history can be wiped externally from this class
            History.Clear();
        }

        public static void RemoveFromHistory(string p_url, string p_stamp)
        {
            // method to remove a specific item from the user's history
            int ctr = 0;
            foreach (UserHistory h in History)
            {
                if (h.URL == p_url && h.Stamp == p_stamp)
                {
                    // user url and timestamp for secure removal
                    History.RemoveAt(ctr);
                }
                ctr++;
            }
        }
        public static Image GetFavicon(string p_url)
        {
            HttpWebRequest w = (HttpWebRequest)HttpWebRequest.Create("https://www.google.com/s2/favicons?domain=" + p_url);
            w.AllowAutoRedirect = true;
            HttpWebResponse r2 = (HttpWebResponse)w.GetResponse();
            System.Drawing.Image ico;
            using (Stream s = r2.GetResponseStream())
            {
                // set webrequest stream to new image variable
                ico = Image.FromStream(s);
            }
            return ico;
        }
        public static void AddHistory(string p_url)
        {
            int ctr = 0;
            List<int> removeList = new List<int>();
            foreach (UserHistory h in History)
            {
                if (h.URL == p_url)
                {
                    removeList.Insert(0, ctr);
                    break;
                }
                ctr++;
            }
            for (int i = 0; i < removeList.Count; i++)
            {
                History.RemoveAt(removeList[i]);
            }
            UserHistory temp = new UserHistory();
            temp.Stamp = DateTime.Now.ToString();
            temp.URL = p_url;
            History.Insert(0, temp);
        }

        public static void AddUser(string p_username, string p_password)
        {
            // method to add a user to the database.
            // this is performed immediately, default theme and sites are added
            // if user exists, should cuase error which is propagated to user
            using (OleDbCommand command = new OleDbCommand())
            {
                // set command connection, text, and params
                command.Connection = Con;
                command.CommandType = CommandType.Text;
                command.CommandText = "INSERT INTO Users VALUES (?, ?, 'Ampere', 'http://mycourses.tstc.edu', " +
                    "'http://mymail.tstc.edu', 'http://www.google.com')";
                command.Parameters.AddWithValue("?", p_username);
                command.Parameters.AddWithValue("?", p_password);
                try
                {
                    // open connection to database
                    Con.Open();
                    // attempt to execute command
                    command.ExecuteNonQuery();
                }
                catch
                {
                    // if error writing to table, show user this message
                    MessageBox.Show("Username already exists.");
                    // close connection (because ending method early here)
                    Con.Close();
                    return;
                }
                finally
                {
                    // ensure connection is closed
                    Con.Close();
                }
            }
            // if successful insert, set current user information
            CurrentUser.Username = p_username;
            CurrentUser.Password = p_password;
            // defaults, can be changed from application user interface
            CurrentUser.Theme = "Ampere";
            CurrentUser.HomeURL = "http://mycourses.tstc.edu";
            CurrentUser.EmailURL = "http://mymail.tstc.edu";
            CurrentUser.SearchURL = "http://www.google.com";
        }

        public static bool ValidUsername(string p_username)
        {
            // method to validate the username
            if (p_username.Length < 4 || !Char.IsLetter(p_username[0]))
            {
                // check for first character as letter and string length
                MessageBox.Show("Username is too short. It must be at least 4 characters, start with " +
                    "a letter, and only contain letters and numbers.");
                return false;
            }
            foreach (char c in p_username)
            {
                // test for alphanumeric
                if (!Char.IsLetterOrDigit(c))
                {
                    MessageBox.Show("Username must only contain letters and numbers.");
                    return false;
                }
            }
            return true;
        }

        public static bool ValidPassword(string p_password)
        {
            // method to test for a valid password input

            // booleans to test for required input
            bool hasChar = false;
            bool hasNumber = false;
            bool hasLower = false;
            bool hasUpper = false;
            if (p_password.Length < 4)
            {
                // test for required length
                MessageBox.Show("Your password is too short.  It must be at least 4 characters, contain " +
                    "at least one lowercase letter, one uppercase letter, one number, and one special " +
                    "character. The password may not contain spaces.");
                return false;
            }
            foreach (char c in p_password)
            {
                if (c < 33 || c > 126)
                {
                    // test for visible characters only
                    MessageBox.Show("Invalid password. It must be at least 4 characters, contain " +
                    "at least one lowercase letter, one uppercase letter, one number, and one special " +
                    "character. The password may not contain spaces.");
                    return false;
                }
                // test for required policy characters
                if (!Char.IsLetterOrDigit(c))
                    hasChar = true;
                if (Char.IsNumber(c))
                    hasNumber = true;
                if (c >= 'a' && c <= 'z')
                    hasLower = true;
                if (c >= 'A' && c <= 'Z')
                    hasUpper = true;
            }
            if (!hasChar || !hasNumber || !hasLower || !hasUpper)
            {
                // if fails any policy requirements, return false
                MessageBox.Show("Invalid password. It must be at least 4 characters, contain " +
                    "at least one lowercase letter, one uppercase letter, one number, and one special " +
                    "character. The password may not contain spaces.");
                return false;
            }
            return true;
        }

        public static void UpdateFavorites()
        {
            // method called when user signs off or exits to send favorites info
            // to the database
            if (DBOps.CurrentUser.Username == "")
                return;
            using (OleDbCommand command = new OleDbCommand())
            {
                // set command connection, text, and parameters
                command.Connection = Con;
                command.CommandType = CommandType.Text;
                command.CommandText = "DELETE FROM UserFavorites WHERE Username = ?";
                command.Parameters.AddWithValue("?", CurrentUser.Username);
                try
                {
                    // open connection to database and execute delete 
                    Con.Open();
                    command.ExecuteNonQuery();
                }
                catch
                {
                    // if error, show user and ask for contact to fix bug
                    MessageBox.Show("1-Error refreshing favorites correctly. Please contact Jeremy " +
                        "Stones at bassbonestones@yahoo.com to report error. New favorites state will " +
                        "not be added or updated.");
                    return;
                }
                finally
                {
                    // close connection
                    Con.Close();
                }
            }

            foreach (UserFavorites f in Favorites)
            {
                // has to loop through entire history to write one line at a time to database
                // option chosen because unknown quantity of history pages

                using (OleDbCommand command = new OleDbCommand())
                {
                    // set command connection, text and parameters for current list item
                    command.Connection = Con;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "INSERT INTO UserFavorites (Username, URL, Alias) " +
                        "VALUES(?,?,?)";
                    command.Parameters.AddWithValue("?", CurrentUser.Username);
                    command.Parameters.AddWithValue("?", f.URL);
                    command.Parameters.AddWithValue("?", f.Alias);

                    try
                    {
                        // open connection to database and execute insert
                        Con.Open();
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        // if error refreshing history ask user to contact developer (me!)
                        MessageBox.Show("2-Error refreshing favorites correctly. Please contact Jeremy " +
                            "Stones at bassbonestones@yahoo.com to report error. History will be missing.");
                    }
                    finally
                    {
                        // ensure connection is closed to database
                        Con.Close();
                    }
                }
            }
        }

        public static void UpdateUserUponDisconnect()
        {
            using (OleDbCommand command = new OleDbCommand())
            {
                command.Connection = Con;
                command.CommandType = CommandType.Text;
                command.CommandText = "UPDATE Users SET Theme = ?, HomeUrl = ?, EmailURL = ?, SearchURL = ? WHERE Username = ?";
                command.Parameters.AddWithValue("?", CurrentUser.Theme);
                command.Parameters.AddWithValue("?", CurrentUser.HomeURL);
                command.Parameters.AddWithValue("?", CurrentUser.EmailURL);
                command.Parameters.AddWithValue("?", CurrentUser.SearchURL);
                command.Parameters.AddWithValue("?", CurrentUser.Username);
                try
                {
                    Con.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    MessageBox.Show("Couldn't update user preferences for " + CurrentUser.Username + ".");
                }
                finally
                {
                    Con.Close();
                }
            }
            UpdateHistory();
            UpdateFavorites();
        }

        public static void UpdateHistory()
        {
            if (DBOps.CurrentUser.Username == "")
                return;
            // method called when user signs off or exits to send history info
            // to the database
            using (OleDbCommand command = new OleDbCommand())
            {
                // set command connection, text, and parameters
                command.Connection = Con;
                command.CommandType = CommandType.Text;
                command.CommandText = "DELETE FROM UserHistory WHERE Username = ?";
                command.Parameters.AddWithValue("?", CurrentUser.Username);
                try
                {
                    // open connection to database and execute delete 
                    Con.Open();
                    command.ExecuteNonQuery();
                }
                catch
                {
                    // if error, show user and ask for contact to fix bug
                    MessageBox.Show("1-Error refreshing history correctly. Please contact Jeremy " +
                        "Stones at bassbonestones@yahoo.com to report error. New history state will " +
                        "not be added or updated.");
                    return;
                }
                finally
                {
                    // close connection
                    Con.Close();
                }
            }
            foreach (UserHistory h in History)
            {
                // has to loop through entire history to write one line at a time to database
                // option chosen because unknown quantity of history pages

                using (OleDbCommand command = new OleDbCommand())
                {
                    // set command connection, text and parameters for current list item
                    command.Connection = Con;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "INSERT INTO UserHistory (URL, Stamp, Username) " +
                        "VALUES(?,?,?)";
                    command.Parameters.AddWithValue("?", h.URL);
                    command.Parameters.AddWithValue("?", h.Stamp);
                    command.Parameters.AddWithValue("?", CurrentUser.Username);

                    try
                    {
                        // open connection to database and execute insert
                        Con.Open();
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        // if error refreshing history ask user to contact developer (me!)
                        MessageBox.Show("2-Error refreshing history correctly. Please contact Jeremy " +
                            "Stones at bassbonestones@yahoo.com to report error. History will be missing.");
                    }
                    finally
                    {
                        // ensure connection is closed to database
                        Con.Close();
                    }
                }
            }
        }

        

    }

}
