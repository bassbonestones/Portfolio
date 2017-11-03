
using MySql.Data.MySqlClient;
using System.Data;
using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;

namespace Stones_LabMDI
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
        
        #endregion
       
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
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            dgv.Columns[12].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Location = new System.Drawing.Point(dgv.Parent.Width / 2 - dgv.Width / 2, dgv.Location.Y);
            // clear initial selection
            dgv.ClearSelection();
        }
        
    }


}
