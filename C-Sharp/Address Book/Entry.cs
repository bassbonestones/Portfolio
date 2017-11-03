//Jeremiah Stones 5/15/2017
//ITSE 2338
//Lab09, Address Book Part 2 of 2

//See Program Specification in Specification Folder or See About During Runtime.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StonesJ_Lab09
{
    // class created to hold the individual records for the address book
    class Entry
    {
        // create private member variables
        private string  _strFName, _strLName, _strStreet, _strCity, _strState, _strZip, _strPhone, _strEmail;
        private int _intNum;
        private static List<Entry> _addressBook = new List<Entry>();

        public Entry(string fName, string lName, string street, string city, string state, string zip, string phone, string email, int num)
        {
            // constructor that sets all the properties of the class instance
            _strFName = fName;
            _strLName = lName;
            _strStreet = street;
            _strCity = city;
            _strState = state;
            _strZip = zip;
            _strPhone = phone;
            _strEmail = email;
            _intNum = num;
        }
       
        // create public static list property that holds all the address book Entries
        public static List<Entry> AddressBook
        {
            get { return _addressBook; }
            set { _addressBook = value; }
        }
        // create public properties for getting/setting each member variable
        public string FName
        {
            get { return _strFName; }
            set { _strFName = value; }
        }
        public string LName
        {
            get { return _strLName; }
            set { _strLName = value; }
        }
        public string Street
        {
            get { return _strStreet; }
            set { _strStreet = value; }
        }
        public string City
        {
            get { return _strCity; }
            set { _strCity = value; }
        }
        public string State
        {
            get { return _strState; }
            set { _strState = value; }
        }
        public string Zip
        {
            get { return _strZip; }
            set { _strZip = value; }
        }
        public string Phone
        {
            get { return _strPhone; }
            set { _strPhone = value; }
        }
        public string Email
        {
            get { return _strEmail; }
            set { _strEmail = value; }
        }
        public int Num
        {
            get { return _intNum; }
            set { _intNum = value; }
        }
    }
}
