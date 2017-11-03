// Jeremy Stones
// Special Topics
// Brad Willingham, professor

// Lab 02
// 9/8/2017

// Project Specification
// This project allows users to connect to a database, sign-in using information in the Users table, and once properly
// signed in, they can access item information for themself and for other users.

using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

namespace Stones_Lab02_DLL
{
    public class User
    {
        // class to hold user information
        public string UserID;
        public string FName;
        public string LName;
        public string FullName;
        public string UserName;
        public string Password;
        public string Admin;
        // list of all current users 
        public static List<User> CurrentUsers = new List<User>();
    }

    public class Item
    {
        // class to hold item information
        public string ItemID;
        public string ItemName;
        public string ItemDescription;
        public string ItemCost;
        public string Quantity;
        public string UserID;
        public string Active;
        // list of all items
        public static List<Item> CurrentItems = new List<Item>();
    }
    public class DBOps
    {
        // This class is a DLL to handle database operations for Lab02

        // private variables to use with database
        private const string CONNECTION_STRING = "Server=********;Database=********;Uid=********;Pwd=********;";
        private static MySqlConnection _con = new MySqlConnection(CONNECTION_STRING);
        private static MySqlDataAdapter _daDataAdapter = new MySqlDataAdapter();
        private static DataSet _dsItems = new DataSet();
        private static string _dtItems = "Items";
        private static DataSet _dsUsers = new DataSet();
        private static string _dtUsers = "Users";

        // variable to hold error status and message
        public static bool IsError;
        public static string ErrorMessage;

        // public properties region
        #region
        public static MySqlConnection Con
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

        public static MySqlDataAdapter DaDataAdapter
        {
            get
            {
                return _daDataAdapter;
            }

            set
            {
                _daDataAdapter = value;
            }
        }

        public static DataSet DsItems
        {
            get
            {
                return _dsItems;
            }

            set
            {
                _dsItems = value;
            }
        }

        public static string DtItems
        {
            get
            {
                return _dtItems;
            }

            set
            {
                _dtItems = value;
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
        #endregion

        public static bool GetItems()
        {
            // this method connects to the database and retrieves all the rows from the Items table
            // and stores that information into a dataset table

            // Create status boolean.
            bool success = false;
            using (MySqlCommand command = new MySqlCommand())
            {
                // using a disposable command, set the query up and all the command attributes
                string query = "SELECT * FROM `Items`";
                command.CommandType = CommandType.Text;
                command.CommandText = query;
                command.Connection = Con;
                // associate the adapter with the command
                DaDataAdapter.SelectCommand = command;
                try
                {
                    // clear the current items dataset and fill it once the connection is opened
                    DsItems.Clear();
                    // open the connection and fill the dataset
                    Con.Open();
                    DaDataAdapter.Fill(DsItems, DtItems);
                    foreach(DataRow row in DsItems.Tables[DtItems].Rows)
                    {
                        // get each item and add it to the items list
                        Item item = new Item();
                        item.ItemID = row[0].ToString();
                        item.ItemName = row[1].ToString();
                        item.ItemDescription = row[2].ToString();
                        item.ItemCost = row[3].ToString();
                        item.Quantity = row[4].ToString();
                        item.UserID = row[5].ToString();
                        item.Active = row[6].ToString();
                        // add it to the list
                        Item.CurrentItems.Add(item);
                    }
                    success = true;
                }
                catch (Exception ex)
                {
                    // Fill error string.
                    ErrorMessage = ex.Message;
                }
                finally
                {
                    // close the database connection
                    Con.Close();
                }
            }
            return success;
        }

        public static bool GetUsers()
        {
            // this method connects to the database and retrieves all the rows from the Users table
            // and stores that information into a dataset table

            // Create status variable.
            bool success = false;
            using (MySqlCommand command = new MySqlCommand())
            {
                // using a disposable command, set the query up and all the command attributes
                string query = "SELECT * FROM `Users`";
                command.CommandType = CommandType.Text;
                command.CommandText = query;
                command.Connection = Con;
                // associate the adapter with the command
                DaDataAdapter.SelectCommand = command;
                try
                {
                    // clear the current items dataset and fill it once the connection is opened
                    DsUsers.Clear();
                    Con.Open();
                    DaDataAdapter.Fill(DsUsers, DtUsers);
                    foreach(DataRow row in DsUsers.Tables[DtUsers].Rows)
                    {
                        // add each user in the dataset to the users list
                        User user = new Stones_Lab02_DLL.User();
                        user.UserID = row[0].ToString();
                        user.FName = row[1].ToString();
                        user.LName = row[2].ToString();
                        user.FullName = user.FName + " " + user.LName;
                        user.UserName = row[3].ToString();
                        user.Password = row[4].ToString();
                        user.Admin = row[5].ToString();
                        // add user to list
                        User.CurrentUsers.Add(user);
                    }
                    success = true;
                }
                catch (Exception ex)
                {
                    // Fill error string.
                    ErrorMessage = ex.Message;
                }
                finally
                {
                    // close the database connection
                    Con.Close();
                }
            }

            return success;
        }
    }
}
