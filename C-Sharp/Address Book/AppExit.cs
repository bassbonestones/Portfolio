//Jeremiah Stones 5/15/2017
//ITSE 2338
//Lab09, Address Book Part 2 of 2

//See Program Specification in Specification Folder or See About During Runtime.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StonesJ_Lab09
{
    class AppExit
    {
        // class created to write to file when the program exits completely

        private const string _PATH = @"C:\Users\Administrator\AppData\Roaming\Jeremy Stones\TextFiles\AddressBook.csv";
        private static bool _closeToNew = false;
        public static void WriteToFile()
        {
            // writes the address book from the list into the file
            if (File.Exists(_PATH))
            {
                StreamWriter sw;
                string _strEntry;
                sw = File.CreateText(_PATH);
                for (int i = 0; i < Entry.AddressBook.Count; i++)
                {
                    _strEntry = Entry.AddressBook[i].FName + "," + Entry.AddressBook[i].LName + "," + Entry.AddressBook[i].Street + "," + Entry.AddressBook[i].City + "," +
                        Entry.AddressBook[i].State + "," + Entry.AddressBook[i].Zip + "," + Entry.AddressBook[i].Phone + "," + Entry.AddressBook[i].Email;
                    sw.WriteLine(_strEntry);
                }
                sw.Close();
            }
        }
        public static bool CloseToNew
        {
            get { return _closeToNew; }
            set { _closeToNew = value; }
        }

        public static void ResetCloseFlag()
        {
            _closeToNew = false;
        }
    }
}
