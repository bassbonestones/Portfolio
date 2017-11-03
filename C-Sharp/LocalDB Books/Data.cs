// See frmMain for overview

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StonesJ_Lab11
{
    struct Book
    {
        // structure that holds info for a single book
        public string ISBN, Title, CopyDate, Author, Publisher, Pages, SubCode;
    }
    class Data
    { 
        // data members for Data class
        private const string CONNECT_STRING = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=..\..\DBFiles\Books_2010.mdb;Persist Security Info=True;Mode=Share Deny None";
        private OleDbConnection _conBooks = new OleDbConnection(CONNECT_STRING);
        private OleDbDataAdapter _daBooks = new OleDbDataAdapter();
        private string _strDBQuery = "";
        private OleDbCommand _cmdBooks;
        private DataSet _dsBooks = new DataSet();
        private string _dtBooks = "BooksTable";
        private DataSet _dsSubjects = new DataSet();
        private string _dtSubjects = "SubjectsTable";
        private Book book = new Book();

        public void setBook(string isbn, string title, string copyDate, string author, string publisher, string pages, string subCode)
        {
            // used to update a single book struct with info
            book.ISBN = isbn; book.Title = title; book.CopyDate = copyDate;
            book.Author = author; book.Publisher = publisher; book.Pages = pages;
            book.SubCode = subCode;
        }
        public void addBook()
        {
            // Adds a book to the database
            try
            {
                DBQuery = "INSERT INTO [Books] (ISBN,TItle,CopyDate,Author,Publisher,Pages,SubCode) VALUES ('" + book.ISBN 
                    + "','" + book.Title + "'," + book.CopyDate + ",'" + book.Author + "','" + book.Publisher 
                    + "', " + book.Pages + ",'" + book.SubCode + "')";
                     
                CmdBooks.CommandText = DBQuery;
                DaBooks.SelectCommand = CmdBooks;
                ConBooks.Open();
                CmdBooks.ExecuteNonQuery();
                MessageBox.Show(book.Title + " by " + book.Author + " was added.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Add Book Failed:\n\n" + ex.Message);
            }
            finally
            { 
                // closes no matter what
                ConBooks.Close();
            }
        }
        public void updateBook(string p_isbn)
        {
            // update a book in the database
            try
            {
                DBQuery = "UPDATE [Books] SET ISBN='" + book.ISBN + "',TItle='" + book.Title 
                    + "',CopyDate=" + book.CopyDate + ",Author='" + book.Author + "',Publisher='" 
                    + book.Publisher + "',Pages=" + book.Pages + ",SubCode='" + book.SubCode + "'"
                    + " WHERE ISBN ='" + p_isbn + "'";
                CmdBooks.CommandText = DBQuery;
                DaBooks.SelectCommand = CmdBooks;
                ConBooks.Open();
                CmdBooks.ExecuteNonQuery();
                MessageBox.Show(book.Title + " by " + book.Author + " was updated.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Update Book Failed:\n\n" + ex.Message);
            }
            finally
            {
                // close connection no matter what
                ConBooks.Close();
            }
        }
        public void deleteBook()
        {
            // delete book from database
            try
            {
                DBQuery = "DELETE FROM Books WHERE Author = '" + book.Author + "' AND TItle = '" + book.Title + "'";
                CmdBooks.CommandText = DBQuery;
                DaBooks.SelectCommand = CmdBooks;
                ConBooks.Open();
                CmdBooks.ExecuteNonQuery();
                MessageBox.Show(book.Title + " by " + book.Author + " has been deleted.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Delete Book Failed:\n\n" + ex.Message);
            }
            finally
            {
                // always close connection
                ConBooks.Close();
            }
        }

        public string getSubCode(string _subject)
        {
            // uses subject to return sub code
            string s;
            try
            {
                DBQuery = "SELECT SubjectCode FROM Subjects WHERE Subject = '" + _subject + "';";
                CmdBooks.CommandText = DBQuery;
                DaBooks.SelectCommand = CmdBooks;
                ConBooks.Open();
                s = CmdBooks.ExecuteScalar().ToString();
                ConBooks.Close(); // must close before return otherwise close not reached
                return s;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Get Subject Code Failed:\n\n" + ex.Message);
                return "";
            }
        }
        
        public string getSubject(string _code)
        {
            // uses sub code to get subject
            string s;
            try
            {
                DBQuery = "SELECT Subject FROM Subjects WHERE SubjectCode = '" + _code +"'";
                CmdBooks.CommandText = DBQuery;
                DaBooks.SelectCommand = CmdBooks;
                ConBooks.Open();
                s = CmdBooks.ExecuteScalar().ToString();
                ConBooks.Close(); // must close before return otherwise close not reached
                return s;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Get Subject Failed:\n\n" + ex.Message);
                return "";
            }
        }
        
        public void CommandConnection()
        {
            // separated setting the command for the database so it would only be called once at load of
            // frmMain class instance
            CmdBooks = new OleDbCommand(DBQuery, ConBooks);
        }
        public void LoadDataBooks()
        {
            // non-parameterized Load of ALL books
            try
            {
                DBQuery = "SELECT * FROM Books";
                CmdBooks.CommandText = DBQuery;
                DaBooks.SelectCommand = CmdBooks;
                ConBooks.Open();
                DsBooks.Clear();
                DaBooks.Fill(DsBooks, DtBooks);
            }
            catch (Exception ex)
            {
                MessageBox.Show("LoadDataBooks Failed:\n\n" + ex.Message);
            }
            finally
            {
                // always close connection
                ConBooks.Close();
            }
        }
        public void LoadDataBooks(string _code)
        {
            // parameterized overload of LoadDataBooks that takes in a subCode to 
            // perform a narrowing select on books with a specific subCode.
            try
            {
                DBQuery = "SELECT * FROM Books WHERE SubCode = '" + _code + "'";
                CmdBooks.CommandText = DBQuery;
                DaBooks.SelectCommand = CmdBooks;
                ConBooks.Open();
                DsBooks.Clear();
                DaBooks.Fill(DsBooks, DtBooks);
            }
            catch (Exception ex)
            {
                MessageBox.Show("LoadDataBooks (overload) Failed:\n\n" + ex.Message);
            }
            finally
            {
                // always close connection
                ConBooks.Close();
            }
        }
        public void LoadDataSubjects()
        {
            // Loads info from Subjects table in database
            try
            {
                DBQuery = "SELECT * FROM Subjects";
                CmdBooks.CommandText = DBQuery;
                DaBooks.SelectCommand = CmdBooks;
                ConBooks.Open();
                DsSubjects.Clear();
                DaBooks.Fill(DsSubjects, DtSubjects);
            }
            catch (Exception ex)
            {
                MessageBox.Show("LoadDataSubjects Failed:\n\n" + ex.Message);
            }
            finally
            {
                // always close connection
                ConBooks.Close();
            }
        }
       // constructor for class
        public Data(){}
        // public parameters
        public OleDbConnection ConBooks
        {
            get { return _conBooks; }
            set { _conBooks = value; }
        }
        public OleDbDataAdapter DaBooks
        {
            get { return _daBooks; }
            set { _daBooks = value; }
        }
        public OleDbCommand CmdBooks
        {
            get { return _cmdBooks; }
            set { _cmdBooks = value; }
        }
        public string DBQuery
        {
            get { return _strDBQuery; }
            set { _strDBQuery = value; }
        }
        public DataSet DsBooks
        {
            get { return _dsBooks; }
            set { _dsBooks = value; }
        }
        public string DtBooks
        {
            get { return _dtBooks; }
            set { _dtBooks = value; }
        }
        public DataSet DsSubjects
        {
            get { return _dsSubjects; }
            set { _dsSubjects = value; }
        }
        public string DtSubjects
        {
            get { return _dtSubjects; }
            set { _dtSubjects = value; }
        }
    }
}
