// Jeremy Stones
// Special Topics
// Brad Willingham

// Lab 03
// 9/15/2017

// Program Specification
// This program is a C# console application that connects to a database and accesses
// tables that hole user and item information.  The user must log in to see any info.
// Once the user has logged in, they may select to view all of their items, search for 
// items by name, search for items by user, or exit the application.

using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

namespace Stones_Lab03
{
    class User
    {
        // class to hold user information
        public string Username;
        public string Password;
        public string UserID;
        // static list of all users for program functionality
        public static List<User> AllUsers = new List<User>();
    }
    class Item
    {
        // class to hold item information
        public string ItemName;
        public string ItemDescription;
        public string ItemCost;
        public string Quantity;
        public string UserID;
        // static list of all items for program functionality
        public static List<Item> AllItems = new List<Item>();
    }
    class DbOps
    {
        // variables created for getting data from database
        const string CONNECTION_STRING = "Server=********;Database=********;Uid=********;Pwd=********";
        private static MySqlConnection _con = new MySqlConnection(CONNECTION_STRING);
        private static MySqlDataAdapter _daDbOps = new MySqlDataAdapter();
        // data sets/tables
        private static DataSet _dsItems = new DataSet();
        private static string _dtItems = "Items";
        private static DataSet _dsUsers = new DataSet();
        private static string _dtUsers = "Users";
        // variables to hold current username and password
        private static string _username = "";
        private static string _password = "";

        #region Properties
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

        public static MySqlDataAdapter DaDbOps
        {
            get
            {
                return _daDbOps;
            }

            set
            {
                _daDbOps = value;
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

        public static string Username
        {
            get
            {
                return _username;
            }

            set
            {
                _username = value;
            }
        }

        public static string Password
        {
            get
            {
                return _password;
            }

            set
            {
                _password = value;
            }
        }
        #endregion Properites

        public static void GetItems()
        {
            // function to get all items from database table, Items
            using (MySqlCommand command = new MySqlCommand())
            {
                // using a temp command, query Items table

                // set query string
                string query = "SELECT ItemName, ItemDescription, ItemCost, Quantity, UserID FROM `Items` WHERE UserID IS NOT NULL";
                // set command type
                command.CommandType = CommandType.Text;
                // set command text to query above
                command.CommandText = query;
                // associate the connection with the command
                command.Connection = Con;
                // associate the command with the data adapter
                DaDbOps.SelectCommand = command;
                try
                {
                    // open the database connection
                    Con.Open();
                    // clear all data from items DataSet
                    DsItems.Clear();
                    // user the adapter to fill the Items DataSet
                    DaDbOps.Fill(DsItems, DtItems);
                    foreach (DataRow row in DsItems.Tables[0].Rows)
                    {
                        // loop through ever row of the dataset and create new items from the info

                        // create a temp item
                        Item item = new Item();
                        // set item info with row info
                        item.ItemName = row[0].ToString();
                        item.ItemDescription = row[1].ToString();
                        item.ItemCost = row[2].ToString();
                        item.Quantity = row[3].ToString();
                        item.UserID = row[4].ToString();
                        // add item to items list
                        Item.AllItems.Add(item);
                    }
                }
                catch (Exception ex)
                {
                    // message user if data not filled in from database
                    Console.WriteLine("Error filling in items from database. \nMessage: " + ex.Message);
                }
                finally
                {
                    // always close the connection 
                    Con.Close();
                }
            }
        }

        public static void GetUsers()
        {
            // function to get all users from database table, Users
            using (MySqlCommand command = new MySqlCommand())
            {
                // using a temp command, query Users table

                // set query string
                string query = "SELECT UserName, Password, UserID FROM `Users`";
                // set command type
                command.CommandType = CommandType.Text;
                // set command text to query above
                command.CommandText = query;
                // associate connection with command
                command.Connection = Con;
                // associate command with data adapter
                DaDbOps.SelectCommand = command;
                try
                {
                    // open the database connection
                    Con.Open();
                    // clear all data from Users DataSet
                    DsUsers.Clear();
                    DaDbOps.Fill(DsUsers, DtUsers);
                    foreach (DataRow row in DsUsers.Tables[0].Rows)
                    {
                        // loop through each row in the DataSet and create an item from row info

                        // create a temp user
                        User user = new User();
                        // set user info from row info
                        user.Username = row[0].ToString();
                        user.Password = row[1].ToString();
                        user.UserID = row[2].ToString();
                        // add user to list
                        User.AllUsers.Add(user);
                    }
                }
                catch (Exception ex)
                {
                    // if error, tell user
                    Console.WriteLine("Error filling in users from database. \nMessage: " + ex.Message);
                }
                finally
                {
                    // always close connection to database
                    Con.Close();
                }
            }
        }

        public static string FindUser(string p_search)
        {
            // static method to find a user from a search string

            // first create a temp list of string to hold user names
            List<string> foundUsers = new List<string>();
            foreach (User user in User.AllUsers)
            {
                // loop through every user and add user names
                if (user.Username.ToLower().Contains(p_search.ToLower()))
                {
                    // add found user to list of strings
                    foundUsers.Add(user.Username);
                }
            }
            if (foundUsers.Count == 0)
                // if no use found return specific string
                return "NOTFOUND!";
            else if (foundUsers.Count == 1)
            {
                // if one user found, clear the screen and return that users name
                Console.Clear();
                return foundUsers[0];
            }
            else
            {
                // if more than one user found, give the user the option of which user to pick
                while (true)
                {
                    // while loop to loop until user selects a valid username choice

                    // clear screen
                    Console.Clear();
                    // prompt user to select a user
                    Console.WriteLine("More than one user found. Please choose a user.\n\n");
                    //create counter to show menu numbers
                    int ctr = 1;
                    foreach (string str in foundUsers)
                    {
                        // loop through user names found and create list of all found
                        Console.WriteLine("\t" + ctr + ") " + str); ctr++;
                    }
                    // prompt user for number
                    Console.Write("Your selection: ");
                    // get number from user
                    string selection = Console.ReadLine();
                    if (selection.Length == 1 && selection[0] > '0' && selection[0] <= foundUsers.Count + '0')
                    {
                        // if valid number chosen, clear screen and return the selected user
                        Console.Clear();
                        return foundUsers[selection[0] - 1 - '0'];
                    }
                    // if invalid choice, loop again
                    continue;
                }
            }
        }

        public static void FindItems(string p_search)
        {
            // static method to find items and show them, based on search string entered as parameter

            // create temp list to hold items found from search string
            List<string> items = new List<string>();
            foreach (Item item in Item.AllItems)
            {
                // loop through items and test against search string
                if (item.ItemName.ToLower().Contains(p_search.ToLower()))
                {
                    // if search string found in item name, add it to the items list
                    items.Add(item.ItemName);
                }
            }
            if (items.Count < 1)
            {
                // if no items found, clear screen and tell user
                Console.Clear();
                Console.WriteLine("No items found.\n");
            }
            else
            {
                // if items found - show them

                // clear screen
                Console.Clear();
                // display title for output with search string included
                Console.WriteLine("Items found by search \"" + p_search + "\"\n");
                foreach (string str in items)
                {
                    // loop through every item in list of item name strings
                    foreach (Item item in Item.AllItems)
                    {
                        // loop through all items each time and see if there's a matching item name
                        if (item.ItemName == str)
                        {
                            // if a matching item name was found display all the item's info, including owner

                            // show item name
                            SetW_LeftPad("Item Name: ", 20);
                            SetW_RightPad(item.ItemName, 20);
                            // show item cost
                            SetW_LeftPad("Cost: ", 20);
                            SetW_RightPad("$" + item.ItemCost, 20);
                            // go to next line
                            Console.WriteLine();
                            // who item quantity
                            SetW_LeftPad("Quantity: ", 20);
                            SetW_RightPad(item.Quantity, 20);
                            // show item description
                            SetW_LeftPad("Description: ", 20);
                            SetW_RightPad(item.ItemDescription, 30);
                            // go to next line
                            Console.WriteLine();
                            // show item owner
                            SetW_LeftPad("Owner: ", 20);
                            SetW_RightPad(GetUserByID(item.UserID), 20);
                            // go to next line and add an extra line after all info is displayed for this single item
                            Console.WriteLine();
                            Console.WriteLine();
                        }
                    }
                }
            }
            // after all items displayed, prompt user to press ENTER to continue
            Console.WriteLine("Please press the ENTER key to continue...");
            // wait for ENTER press
            Console.ReadLine();
        }

        private static string GetUserByID(string p_userID)
        {
            // method to return username by userID
            foreach (User user in User.AllUsers)
            {
                // loop through every user and test for a match on userID
                if (user.UserID == p_userID)
                {
                    // if match found for userID, return the name of the user
                    return user.Username;
                }
            }
            // return blank string if none found (should never return this, just here for required return on all paths)
            return "";
        }
        public static void ItemsByUser(string p_username)
        {
            // static method to get all items of a particular user

            // show title for items owned by particular user
            Console.WriteLine("\n\tItems owned by " + p_username + "\n-------------------------------------------\n");

            foreach (User user in User.AllUsers)
            {
                // loop throug all users to get matching user and display their items
                if (user.Username == p_username)
                {
                    // if match found
                    foreach (Item item in Item.AllItems)
                    {
                        // loop through all items to find matching UserIDs
                        if (item.UserID == user.UserID)
                        {
                            // if userID match found, display that particular item

                            // display item name
                            SetW_LeftPad("Item Name: ", 20);
                            SetW_RightPad(item.ItemName, 20);
                            // display item cost
                            SetW_LeftPad("Cost: ", 20);
                            SetW_RightPad("$" + item.ItemCost, 20);
                            // go to next line
                            Console.WriteLine();
                            // display item quantity
                            SetW_LeftPad("Quantity: ", 20);
                            SetW_RightPad(item.Quantity, 20);
                            // display item description
                            SetW_LeftPad("Description: ", 20);
                            SetW_RightPad(item.ItemDescription, 30);
                            // go to next line and add an extra line
                            Console.WriteLine();
                            Console.WriteLine();
                        }
                    }
                    // only one user may be returned, so break foreach loop
                    break;
                }
            }
            // after all items for this user are displayed, prompt the user to
            // press the ENTER key to continue
            Console.WriteLine("\n\nPlease press the ENTER key to continue...");
            // wait for ENTER press
            Console.ReadLine();
            // clear screen
            Console.Clear();
        }

        private static void SetW_LeftPad(string p_text, int p_totalLength)
        {
            // private static method to set padding characters to the left of a string
            for (int i = 0; i < (p_totalLength - p_text.Length); i++)
            {
                // add spaces before the given text to the length desired
                Console.Write(" ");
            }
            // now write the given text
            Console.Write(p_text);
        }
        private static void SetW_RightPad(string p_text, int p_totalLength)
        {
            // private static method to set padding characters to the right of a string

            // write given string first
            Console.Write(p_text);
            for (int i = 0; i < (p_totalLength - p_text.Length); i++)
            {
                // add spaces after the given text to the length desired
                Console.Write(" ");
            }
        }

        public static void LogIn()
        {
            // method to make a user log in before continuing to view items in database

            // create flag to loop until valid log-in
            bool loggingIn = true;
            while (loggingIn)
            {
                // loops until a valid login is entered
                Console.WriteLine("Welcome to the I.I.C (Item Information Center)\n\n");
                // flag to loop until user finished entered username
                bool enteringUsername = true;
                while (enteringUsername)
                {
                    // loops until username entered
                    Console.Write("Enter a username and password to continue. \nUsername (admin): ");
                    // get username from user
                    DbOps.Username = Console.ReadLine();
                    if (DbOps.Username.Length < 1)
                    {
                        // if no username entered, clear screen and make them type it again
                        enteringUsername = false;
                        Console.Clear();
                        break;
                    }
                    else
                    {
                        // if username entered, get password

                        // create flag to loop until password entered
                        bool enteringPassword = true;
                        while (enteringPassword)
                        {
                            // loop until password entered

                            // prompt for password
                            Console.Write("Password (admin): ");
                            // clear current password string
                            DbOps.Password = "";
                            // create variable to hold key info
                            ConsoleKeyInfo key;
                            do
                            {
                                // loops until ENTER pressed
                                // get key from user
                                key = Console.ReadKey(true);
                                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                                {
                                    // if not backspace or enter, write a '*' instead of the character and 
                                    // store the actual character in the password string
                                    DbOps.Password += key.KeyChar;
                                    Console.Write("*");
                                }
                                else if (key.Key == ConsoleKey.Backspace && DbOps.Password.Length > 0)
                                {
                                    // if backspace, then clear the last '*' and remove the last character
                                    // from the password string
                                    Console.Write("\b \b");
                                    DbOps.Password = DbOps.Password.Substring(0, DbOps.Password.Length - 1);
                                }
                            } while (key.Key != ConsoleKey.Enter);

                            if (DbOps.Password.Length < 1)
                            {
                                // if the entered password is empty, clear the screen and have them try again
                                Console.Clear();
                                Console.WriteLine("Welcome to the I.I.C (Item Information Center)\n\n");
                                Console.WriteLine("Enter a username and password to continue. \nUsername (admin): " + DbOps.Username);
                                continue;
                            }
                            else
                            {
                                // if the password was entered, set the "entering..." loops to false
                                enteringPassword = false;
                                enteringUsername = false;
                                // create a flag to loop to indicate match of username and password
                                bool loginSuccess = false;
                                foreach (User user in User.AllUsers)
                                {
                                    // loop through all users to search for username/password match
                                    if (user.Username == DbOps.Username && user.Password == DbOps.Password && user.Username != "")
                                    {
                                        // if both username and password match, set loginSuccess to true and break current loop
                                        // also make sure loggingIn is set to false, so outer loop will end
                                        loginSuccess = true;
                                        loggingIn = false;
                                        break;
                                    }
                                }
                                // clear the screen
                                Console.Clear();
                                if (!loginSuccess)
                                {
                                    // if log-in was unsuccessful, tell user before displaying the rest of the welcome screen
                                    Console.WriteLine("Invalid username/password combination. Try again.\n\n");
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
