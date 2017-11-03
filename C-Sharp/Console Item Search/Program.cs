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

namespace Stones_Lab03
{
    class Program
    {
        static void Main(string[] args)
        {
            // starting function of program

            // get info from database - users and items
            DbOps.GetItems();
            DbOps.GetUsers();
            // make user log in
            DbOps.LogIn();
            // create flag to loop main menu until exit called
            bool onMainMenu = true;
            while (onMainMenu)
            {
                // continue looping menu until exit

                // welcome user by username and give them a menu to choose an option
                Console.WriteLine("Welcome, " + DbOps.Username + " \n\nSelect an option from the menu to proceeed.");
                Console.WriteLine("\t1) View all of your items.");
                Console.WriteLine("\t2) Search item by item name.");
                Console.WriteLine("\t3) Search items by owner (username).");
                Console.WriteLine("\t4) List all items.");
                Console.WriteLine("\t5) List all users.");
                Console.WriteLine("\t6) Exit Application.");
                Console.Write("Your selection: ");
                // variable to hold users option choice
                string choice = "";
                // get choice from user
                choice = Console.ReadLine();
                if (!(choice.Length == 1 && choice[0] < '7' && choice[0] > '0'))
                {
                    // if choice not entered or wrong range of number clear screen and show error message
                    Console.Clear();
                    Console.WriteLine("Invalid selection. Please try again.\n\n");
                    // go back to top of loop
                    continue;
                }
                switch (choice[0] - '0')
                {
                    // if valid choice, this switch is entered
                    case 1: // if show user's items
                        // clear screen
                        Console.Clear();
                        // show all users items
                        DbOps.ItemsByUser(DbOps.Username);
                        // clear screen after user done viewing
                        Console.Clear();
                        // end switch
                        break;
                    case 2:
                        {  // if seach by item name chosen
                            // clear screen
                            Console.Clear();
                            // set temp string to hold search text
                            string tempItem = "";
                            // get search text from user
                            Console.Write("Search for an item: ");
                            tempItem = Console.ReadLine();
                            // function to display items by item name search string
                            DbOps.FindItems(tempItem);
                            // clear screen after user finished viewing items by item name
                            Console.Clear();
                        }
                        // end switch
                        break;
                    case 3:
                        { // if user chooses to search items by owner
                            // clear screen
                            Console.Clear();
                            // create temp user variable to hold search string
                            string tempUser = "";
                            // get search string from user for username
                            Console.Write("Enter a username to search items by user: ");
                            tempUser = Console.ReadLine();
                            // get string from search method
                            tempUser = DbOps.FindUser(tempUser);
                            if (tempUser != "NOTFOUND!")
                            {
                                // if search function returned NOTFOUND!, nothing was found
                                // if this was not returned, get items by username
                                DbOps.ItemsByUser(tempUser);
                                // clear console screen when user finished viewing items
                                Console.Clear();
                            }
                            else
                            { // if NOTFOUND!
                                // clear screen
                                Console.Clear();
                                // tell user nothing was found
                                Console.WriteLine("Sorry, username not found.");
                            }
                        }
                        // end switch
                        break;
                    case 4: // user chooses to list all items
                            // clear screen
                        Console.Clear();
                        Console.WriteLine("List of all items:\n");
                        foreach(Item item in Item.AllItems)
                        {
                            Console.WriteLine(item.ItemName);
                        }
                        // add two empty lines
                        Console.WriteLine("\n\n");
                        // after all items displayed, prompt user to press ENTER to continue
                        Console.WriteLine("Please press the ENTER key to continue...");
                        // wait for ENTER press
                        Console.ReadLine();
                        // clear screen
                        Console.Clear();
                        break;
                    case 5: // user chooses to list all items
                            // clear screen
                        Console.Clear();
                        Console.WriteLine("List of all users:\n");
                        foreach (User user in User.AllUsers)
                        {
                            Console.WriteLine(user.Username);
                        }
                        // add two empty lines
                        Console.WriteLine("\n\n");
                        // after all items displayed, prompt user to press ENTER to continue
                        Console.WriteLine("Please press the ENTER key to continue...");
                        // wait for ENTER press
                        Console.ReadLine();
                        // clear screen
                        Console.Clear();
                        break;
                    case 6: // user chooses to exit program
                        // exit the program
                        Environment.Exit(0);
                        break;
                    default: // never executed
                        // just in case, end program
                        Environment.Exit(0);
                        break;
                }
            }
        }
    }
}
