// Jeremy Stones
// Special Topics
// Brad Willingham

// Lab 4
// 10/1/2017

// Program Specification:
// This program is an application to alter a database by allowing users to add a new database user
// or add/update new database user items.  The application user start at the main navigation page 
// where they can choose from various database operations, exiting the program, or viewing the report
// of user items. Users can see more information about how to use this program in the About/Help menu
// option, which displays a help form.C:\Users\Jeremy\Documents\school stuff\Special Topics\Stones_Lab05\Stones_Lab05\DbOps.cs

using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using System.Drawing;
using System.IO;
// For use of PowerShell commands within the program.
using System.Management.Automation;

namespace Stones_Lab04
{
    public class Item
    {
        // This class creates objects that store Item attributes.
        public int ItemID;
        public string ItemName;
        public string ItemDescription;
        public float ItemCost;
        public int Quantity;
        public int UserID;
        public int Active;
        private byte[] _itemImageBytes;
        private Image _itemImage;
        // This public property sets the ItemImage when the ItemImageBytes is set
        public byte[] ItemImageBytes {
            get
            {
                return _itemImageBytes;
            }
            set
            {
                _itemImageBytes = value;
            }
        }
        // This public property sets the ItemImageBytes when the Image is set
        public Image ItemImage
        {
            get
            {
                return _itemImage;
            }
            set
            {
                _itemImage = value;
            }
        }
        // This list is shared by all Items and holds all Items
        public static List<Item> AllItems = new List<Item>();
    }

    public class User
    {
        // This class creates objects that store user objects
        public int UserID;
        public string FName;
        public string LName;
        public string Username;
        public string Password;
        public bool Admin;
        // This list is shared by all Users and holds all Users
        public static List<User> AllUsers = new List<User>();
    }

    public class DbOps
    {
        // This class is for all database operations including the load application call with PowerShell.

        // Create a connection string.
        private const string CONNECTION_STRING = "Server=********;Database=********;Uid=********;Pwd=********";
        // Use connection string to create a connection object
        private static MySqlConnection _con = new MySqlConnection(CONNECTION_STRING);
        // Create a data adapter to talk with database
        private static MySqlDataAdapter _daAdapter = new MySqlDataAdapter();
        // Create variables for datasets and table names
        private static DataSet _dsUsers = new DataSet();
        private static string _dtUsers = "Users";
        private static DataSet _dsItems = new DataSet();
        private static string _dtItems = "Items";
        // Create a flag to save error state.
        private static bool _ErrorExists;
        // Create a variable to hold the error message.
        private static string _errorMessage;

        // Added region to hold all public properties for above private variables
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

        public static MySqlDataAdapter DaAdapter
        {
            get
            {
                return _daAdapter;
            }

            set
            {
                _daAdapter = value;
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

        public static bool ErrorExists
        {
            get
            {
                return _ErrorExists;
            }

            set
            {
                _ErrorExists = value;
            }
        }

        public static string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }

            set
            {
                _errorMessage = value;
            }
        }
        #endregion Properties

        public static void GetUsers()
        {
            // This method is to query and retrieve users from the database.

            using (MySqlCommand command = new MySqlCommand())
            {
                // Use a temporary command to read from the database into the users data set

                // Set the query string.
                string query = "SELECT * FROM `Users` ORDER BY FName";
                // Set the command type (text).
                command.CommandType = CommandType.Text;
                // Set the command text to equal the query string above.
                command.CommandText = query;
                // Associate the connection object with the command.
                command.Connection = Con;
                // Associate the command with the data adapter object.
                DaAdapter.SelectCommand = command;
                try
                {
                    // Clear the current data set.
                    DsUsers.Clear();
                    // Open the connection.
                    Con.Open();
                    // Fill the users data set table
                    DaAdapter.Fill(DsUsers, DtUsers);
                    // Clear current User list
                    User.AllUsers.Clear();
                    // Fill User list from User class
                    foreach(DataRow row in DsUsers.Tables[DtUsers].Rows)
                    {
                        // Create a temp user object.
                        User user = new User();
                        // Fill temp user with data from table.
                        user.UserID = int.Parse(row[0].ToString());
                        user.FName = row[1].ToString();
                        user.LName = row[2].ToString();
                        user.Username = row[3].ToString();
                        user.Password = row[4].ToString();
                        user.Admin = row[5].ToString() == "1" ? true : false;
                        // Add the user to the users list.
                        User.AllUsers.Add(user);
                    }
                    // Set ErrorExists flag to false;
                    ErrorExists = false;
                }
                catch (Exception ex)
                {
                    // If the database query fails, return the error to the user by setting
                    // the error flag and message.
                    ErrorExists = true;
                    ErrorMessage = "Error while trying to fill Users table. Error: " + ex.Message;
                }
                finally
                {
                    // Always close the connection.
                    Con.Close();
                }
            }
        }

        public static string GetUserFromID(int id)
        {
            // This method uses a UserID to find the user's full name.
            foreach(User user in User.AllUsers)
            {
                // Loop through each user, and if the ID matches, return the user's full name.
                if(user.UserID == id)
                {
                    return user.FName + " " + user.LName;
                }
            }
            // If nothing matches, return an empty string.
            return "";
        }

        public static int GetIDFromUser(string user)
        {
            // This method uses a user's full name to find the user's ID number.
            foreach(User u in User.AllUsers)
            {
                // Loop through each user, and if the full name matches, return the user's ID.
                if ((u.FName + " " + u.LName) == user)
                {
                    return u.UserID;
                }
            }
            return -1;
        }

        public static void GetItems()
        {
            // This method is to query and retrieve items from the database.

            using (MySqlCommand command = new MySqlCommand())
            {
                // Use a temporary command to read from the database into the items data set

                // Set the query string.
                string query = "SELECT * FROM `Items` WHERE UserID IS NOT NULL";
                // Set the command type (text).
                command.CommandType = CommandType.Text;
                // Set the command text to equal the query string above.
                command.CommandText = query;
                // Associate the connection object with the command.
                command.Connection = Con;
                // Associate the command with the data adapter object.
                DaAdapter.SelectCommand = command;
                try
                {
                    // Clear the current data set.
                    DsItems.Clear();
                    // Open the connection.
                    Con.Open();
                    // Fill the users data set table
                    DaAdapter.Fill(DsItems, DtItems);
                    // Clear current Items List
                    Item.AllItems.Clear();
                    // Fill Items List in Items class
                    foreach(DataRow row in DsItems.Tables[0].Rows)
                    {
                        // Create a temp item.
                        Item item = new Item();
                        // Store item attributes from the data table.
                        item.ItemID = int.Parse(row[0].ToString());
                        item.ItemName = row[1].ToString();
                        item.ItemDescription = row[2].ToString();
                        item.ItemCost = float.Parse(row[3].ToString());
                        item.Quantity = int.Parse(row[4].ToString());
                        item.UserID = int.Parse(row[5].ToString());
                        // Add the item to the items list.
                        Item.AllItems.Add(item);
                    }
                    
                    // Set ErrorExists flag to false;
                    ErrorExists = false;
                }
                catch (Exception ex)
                {
                    // If the database query fails, return the error to the user by setting
                    // the error flag and message.
                    ErrorExists = true;
                    ErrorMessage = "Error while trying to fill Items table. Error: " + ex.Message;
                }
                finally
                {
                    // Always close the connection.
                    Con.Close();
                    
                }
            }
        }

        public static bool GetMaxUserID(out int id, out string err)
        {
            // This method finds the maximum UserID in the Users table and returns it.
            using (MySqlCommand command = new MySqlCommand())
            {
                // Using a disposable command, query the database for the max ID and store it
                // in the out param, id.

                // Create a query string.
                string query = "SELECT MAX(UserID) FROM `Users`";
                // Set the command type.
                command.CommandType = CommandType.Text;
                // Set the command text to the above query string.
                command.CommandText = query;
                // Associate the connection object with the command.
                command.Connection = Con;
                try
                {
                    // Open the connection to the database.
                    Con.Open();
                    // Retrieve the max ID from the database.
                    id = (int)command.ExecuteScalar();
                    // Clear the error string.
                    err = "";
                    // Close the database connection.
                    Con.Close();
                    // Return true, because the max ID was successfully found and retrieved.
                    return true;
                }
                catch(Exception ex)
                {
                    // If an exception was found, arbitrarily assing the out param, id, a value.
                    id = 1;
                    // Store the error message in the public property, err.
                    err = ex.Message;
                    // Close the connection to the database.
                    Con.Close();
                    // Return true so still returns new ID.
                    return true;
                }
            }
        }

        public static bool GetMaxItemID(out int id, out string err)
        {
            // This method finds the maximum ItemID in the Items table and returns it.
            using (MySqlCommand command = new MySqlCommand())
            {
                // Using a disposable command, query the database for the max ID and store it
                // in the out param, id.

                // Create a query string.
                string query = "SELECT MAX(ItemID) FROM `Items`";
                // Set the command type.
                command.CommandType = CommandType.Text;
                // Set the command text to the above query string.
                command.CommandText = query;
                // Associate the connection object with the command.
                command.Connection = Con;
                try
                {
                    // Open the connection to the database.
                    Con.Open();
                    // Retrieve the max ID from the database.
                    id = (int)command.ExecuteScalar();
                    // Clear the error string.
                    err = "";
                    // Close the database connection.
                    Con.Close();
                    // Return true, because the max ID was successfully found and retrieved.
                    return true;
                }
                catch (Exception ex)
                {
                    // If an exception was found, arbitrarily assing the out param, id, a value.
                    id = -1;
                    // Store the error message in the public property, err.
                    err = ex.Message;
                    // Close the connection to the database.
                    Con.Close();
                    // Return false, because the operation failed.
                    return false;
                }
            }
        }

        public static void AddUser(User user)
        {
            // This method is to add a user to the database.

            // First, obtain the Max ID from the Users table.
            int id;
            string err;
            bool obtainedID = GetMaxUserID(out id, out err);
            if(!obtainedID)
            {
                // If nothing was returned form Max ID, then send error to user.
                ErrorExists = true;
                ErrorMessage = "Unable to get max ID from Users table. Error: " + err;
                return;
            }
            // increment the id by 1
            id++;

            using (MySqlCommand command = new MySqlCommand())
            {
                // Use a temporary command to insert data into the database

                // Set the query string.
                string query = "INSERT INTO `Users` (UserID, FName, LName, UserName, Password, Admin) " + 
                    " Values(@id, @fname, @lname, @username, @password, @admin)";
                // Set the command type (text).
                command.CommandType = CommandType.Text;
                // Set the command text to equal the query string above.
                command.CommandText = query;
                // Add Parameters to command
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@fname", user.FName);
                command.Parameters.AddWithValue("@lname", user.LName);
                command.Parameters.AddWithValue("@username", user.Username);
                command.Parameters.AddWithValue("@password", user.Password);
                command.Parameters.AddWithValue("@admin", user.Admin);
                // Associate the connection object with the command.
                command.Connection = Con;
                try
                {
                    // Open the connection.
                    Con.Open();
                    // Execute the Insert Statement
                    command.ExecuteNonQuery();
                    // Update dataset with new User
                    GetUsers();
                    // Set ErrorExists flag to false;
                    ErrorExists = false;
                }
                catch (Exception ex)
                {
                    // If the database query fails, return the error to the user by setting
                    // the error flag and message.
                    ErrorExists = true;
                    ErrorMessage = "Error while trying to insert User into table. Error: " + ex.Message;
                }
                finally
                {
                    // Always close the connection.
                    Con.Close();
                }
            }
        }

        public static void AddItem(Item item)
        {
            // This method is to add an item to the database.

            // First, obtain the Max ID from the Items table.
            int id;
            string err;
            bool obtainedID = GetMaxItemID(out id, out err);
            if (!obtainedID)
            {
                // If nothing was returned form Max ID, then send error to user.
                ErrorExists = true;
                ErrorMessage = "Unable to get max ID from Items table. Error: " + err;
                return;
            }
            // increment the id by 1
            id++;

            using (MySqlCommand command = new MySqlCommand())
            {
                // Use a temporary command to send information to the database

                // Set the query string.
                string query = "INSERT INTO `Items` (ItemID, ItemName, ItemDescription, " + 
                    "ItemCost, Quantity, UserID, Active, ItemImage) VALUES " +
                    "(@id, @name, @description, @cost, @qty, @userID, @active, @img)";
                // Set the command type (text).
                command.CommandType = CommandType.Text;
                // Set the command text to equal the query string above.
                command.CommandText = query;
                // Add Parameters to command
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@name", item.ItemName);
                command.Parameters.AddWithValue("@description", item.ItemDescription);
                command.Parameters.AddWithValue("@cost", item.ItemCost);
                command.Parameters.AddWithValue("@qty", item.Quantity);
                command.Parameters.AddWithValue("@userID", item.UserID);
                command.Parameters.AddWithValue("@active", item.Active);
                command.Parameters.AddWithValue("@img", item.ItemImageBytes);
                // Associate the connection object with the command.
                command.Connection = Con;
                try
                {
                    // Open the connection.
                    Con.Open();
                    // Execute the Insert Statement
                    command.ExecuteNonQuery();
                    // Update dataset with new add
                    GetItems();
                    // Set ErrorExists flag to false;
                    ErrorExists = false;
                }
                catch (Exception ex)
                {
                    // If the database query fails, return the error to the user by setting
                    // the error flag and message.
                    ErrorExists = true;
                    ErrorMessage = "Error while trying to insert Item into table. Error: " + ex.Message;
                }
                finally
                {
                    // Always close the connection.
                    Con.Close();
                }
            }
        }

        public static void UpdateItem(Item item)
        {
            // This method is to update an item in the database.

            using (MySqlCommand command = new MySqlCommand())
            {
                // Use a temporary command to send information to the database

                // Set the query string.
                string query = "UPDATE `Items` SET ItemName = @name, ItemDescription = @description, " +
                    "ItemCost = @cost, Quantity = @qty, UserID = @userID, Active = @active, " +
                    "ItemImage = @img WHERE ItemID = @id";
                // Set the command type (text).
                command.CommandType = CommandType.Text;
                // Set the command text to equal the query string above.
                command.CommandText = query;
                // Add Parameters to command
                command.Parameters.AddWithValue("@name", item.ItemName);
                command.Parameters.AddWithValue("@description", item.ItemDescription);
                command.Parameters.AddWithValue("@cost", item.ItemCost);
                command.Parameters.AddWithValue("@qty", item.Quantity);
                command.Parameters.AddWithValue("@userID", item.UserID);
                command.Parameters.AddWithValue("@active", item.Active);
                command.Parameters.AddWithValue("@img", item.ItemImageBytes);
                command.Parameters.AddWithValue("@id", item.ItemID);
                // Associate the connection object with the command.
                command.Connection = Con;
                try
                {
                    // Open the connection.
                    Con.Open();
                    // Execute the Insert Statement
                    command.ExecuteNonQuery();
                    // Update dataset with new add
                    GetItems();
                    // Set ErrorExists flag to false;
                    ErrorExists = false;
                }
                catch (Exception ex)
                {
                    // If the database query fails, return the error to the user by setting
                    // the error flag and message.
                    ErrorExists = true;
                    ErrorMessage = "Error while trying to update Item in table. Error: " + ex.Message;
                }
                finally
                {
                    // Always close the connection.
                    Con.Close();
                }
            }
        }
         public static Image GetPic(int id)
        {
            // This method was necessary to individually grab the User's IDs as byte arrays from
            // the database.  This makes the code a lot cleaner than creating a defining new command
            // attributes inside of the GetItems method.

            // Create a temporary null image.
            Image img = null;
            using (MySqlCommand command = new MySqlCommand())
            {
                // Using a disposable SQL Command, retrieve the Image from a specific Item.

                // Set up the query string.
                string query = "SELECT ItemImage FROM Items WHERE ItemID = @id";
                // Define the command type.
                command.CommandType = CommandType.Text;
                // Set the command text to the above query string.
                command.CommandText = query;
                // Associate the connection object with the command.
                command.Connection = Con;
                // Add the ID to the query params.
                command.Parameters.AddWithValue("@id", SqlDbType.Int).Value = id;
                try
                {
                    // Open the database connection.
                    Con.Open();
                    // Retrieve the image as a byte array from the database.
                    byte[] myImgBytes = (byte[])command.ExecuteScalar();
                    // Create a stream object to stream the byte array.
                    MemoryStream ms = new MemoryStream(myImgBytes);
                    // Store the new image in the temporary image object as an Image 'FromStream'.
                    img = Image.FromStream(ms);
                }
                catch(Exception)
                {
                    // If the image retrieval fails, no image will be set to the img variable,
                    // and the method will return a null image, which will be handled by the calling
                    // method.
                }
                finally
                {
                    // Close the connection to the database.
                    Con.Close();
                }
            }
            // Return the image.
            return img;
        }


        public static void ShowLoad()
        {
            // This method is used to start a new process of the Loading Screen application.
            using (PowerShell PowerShellInstance = PowerShell.Create())
            {
                // Retrieve the current working directory path of the current executable.
                string p = System.IO.Directory.GetCurrentDirectory();
                // This script will start a new instance of the LoadForm executable.
                PowerShellInstance.AddScript("Set-Location -path $path; Start-Process ../../EXE/LoadForm.exe");
                // Apply the path parameter to the script.
                PowerShellInstance.AddParameter("path", p);
                // Begin invoke execution on the pipeline.
                var results = PowerShellInstance.Invoke();
            }
        }
        public static void CloseLoad()
        {
            // This method is used to end any new process of the Loading Screen application type.
            using (PowerShell PowerShellInstance = PowerShell.Create())
            {
                // Retrieve the current working directory path of the current executable.
                string p = System.IO.Directory.GetCurrentDirectory();
                // This script will end any instance of the LoadForm executable.
                PowerShellInstance.AddScript("Get-Process *loadform* | Stop-Process");
                // begin invoke execution on the pipeline
                var results = PowerShellInstance.Invoke();
            }
        }

    }
}
