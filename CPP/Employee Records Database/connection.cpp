// Jeremy Stones
// Special Topics
// Brad Willingham

// 10/19/2017
// ADO with C++ 
// Lab 06

// Program Specification:
// This program is a console application that interfaces with a MS SQL database to view, add, update,
// and delete information form an Employee table.  The user is presented with a menu to search for
// an employee's information, add a new employee, update an existing employee, delete an employee,
// view all employees' information, or exit the program.  The connecion class handles the vast majority
// of the program functionality.

// Include the header file.
#include "connection.h"

// Use the common namespace.
using namespace std;

// create a connection object and record object from the ADODB class.
ADODB::_ConnectionPtr connDB = NULL;
ADODB::_RecordsetPtr empRec = NULL;

// Constructor.
Connection::Connection(void) {}
// Destructor.
Connection::~Connection(void) {}
// Prototyping for local functions.
bool ValidState(string);
bool ValidZip(string);
bool ValidEmail(string);
bool ValidLetter(char);
bool ValidNumber(char);
bool ValidAlphaNumeric(char);
bool ValidPhone(string);
string FormatPhone(string);
bool ValidYear(string);
int YearToInt(string);

struct Employee
{
	// This structure holds the attributes for an employee object.
	int EmployeeID;
	string FName;
	string LName;
	string Address;
	string City;
	string State;
	string ZipCode;
	string Association;
	string Email;
	string Phone;
	int MemberYear;
};

void Connection::error(_com_error e, char* displayErr)
{
	// This method displays arror details from a database operation.
	sprintf_s(displayErr, 800, "Error Details: \n");
	sprintf_s(displayErr, 800, "%s Code Message: %s \n", displayErr, (char*)e.ErrorMessage());
	sprintf_s(displayErr, 800, "%s Error Source: %s \n", displayErr, (char*)e.Source());
	sprintf_s(displayErr, 800, "%s Error Description: %s \n", displayErr, (char*)e.Description());
}

void Connection::conn(char choice)
{
	// This method holds most of the program's functionality.  The parameter specifies the user's
	// menu choice, which determines which code is executed.

	// Create a database session object.
	HRESULT hr;
	// Create a variable to hold the maximum ID for inserts.
	int maxID;
	// Create a char array to hold error info.
	char ErrStr[800];
	// Create a char array to hold the query string.
	char QueryStr[500];
	// Create variables to hold employee information.
	string empID;
	string empFName;
	string empLName;
	string empName;
	string empMemberYear;
	string empAssocation;
	string empAddress;
	string empCity;
	string empState;
	string empZip;
	string empEmail;
	string empPhone;
	// Create variables for integer type attributes.
	int enteredID;
	int enteredYear;
	// Create variable to hold generic user input.
	string enteredString;
	// Create flags for loop conditions.
	bool updating;
	bool choosing;
	bool adding;
	// Create more user input variables.
	string enteredAssociation;
	string fullSelection;
	char selection;
	// Create a variable that helps extract information from the database (individual attributes).
	_variant_t vtValue;
	// Initialize the database session objects.
	::CoInitialize(NULL);
	// Create a counter for displaying a title and Employee records column headers.
	int ctr = 0;

	try
	{
		// Attemp a database connection.

		// Create a connection object instance.
		hr = connDB.CreateInstance(__uuidof(ADODB::Connection));
		// Set CursorLocation for retrieval of recordsets from the database.
		connDB->CursorLocation = ADODB::adUseClient;
		// Open the connection using the connection string.
		connDB->Open("Provider = SQLOLEDB; Server=********; Database = ********", "********", "********", NULL);
	}
	catch (const _com_error &e)
	{
		// If the connection failed, display a meaninful error message and then exit the program.

		// Clear the screen.
		system("cls");
		// Display the error.
		cout << " DB Connection Error: " << endl;
		error(e, ErrStr);
		printf(ErrStr);
		// Prompt user to press key to exit the application.
		cout << endl << endl << "Press any key to return to exit the program...";
		_getch();
		// Clean up database objects.
		CoUninitialize();
		// Exit the application.
		exit(0);
	}
	try
	{
		// Use the user menu choice to decide which block of code to execute.

		switch (choice)
		{
		case '1': // Search record by Employee ID
				  // Set the badbit cin flag to enter loop.
			cin.clear(ios::badbit);
			while (!cin.good())
			{
				// Loop until a valid number has been entered.

				// Clear all error flags from cin.
				cin.clear();
				// Ignore up to 256 characters or the point where the newline character was entered.
				cin.ignore(256, '\n');
				// Clear the console screen.
				system("cls");
				// Prompt the user for an employee ID.
				cout << "Searching Records By Employee ID" << endl << endl
					<< "Please enter an ID to search: ";
				// Get the ID from the user.
				cin >> enteredID;
				// Test stdin for error flags.
				if (!cin.good())
				{
					// If there are any errors, set only the badbit flag.
					cin.clear(ios::badbit);
					// Clear the screen.
					system("cls");
					// Message the error to the user and prompt them to try again.
					cout << "Invalid Employee ID -- IDs must be whole numbers." << endl
						<< endl << "Press any key to try again...";
					// Wait for a key stroke.
					_getch();
				}
			}
			// Create a query with parameters to select an Employee based on the given Employee ID.
			sprintf_s(QueryStr, 500, "SELECT EmployeeID, CONCAT(FirstName, ' ', LastName) \"FullName\", Address, City, State, ZipCode FROM [Employee] WHERE EmployeeID = %i", enteredID);
			// Use the query to get records from the database.
			empRec = connDB->Execute(QueryStr, NULL, 1);
			// Print the column headers.
			cout << endl << left << " " << setw(15) << "Employee ID" << setw(20) << "Name" << setw(40) << "Address" << setw(20) << "City" << setw(12) << "State" << setw(12) << "Zip" << endl;
			// Print a horizontal rule.
			cout << "---------------------------------------------------------------------------------------------------------------------\n";
			// Test to see if there are any records in the record set.
			if (!(empRec->EndOfFile))
			{
				// If there are records, cycle through them until they are depleted, printing 
				// pertinant attributes.

				// Move to the first record in the set.
				empRec->MoveFirst();

				// Loop until the last record has been reached.
				while (!(empRec->EndOfFile))
				{
					// While there are still records, get their attributes and display them
					// on the screen.

					// Get the employee ID.
					vtValue = empRec->Fields->GetItem("EmployeeID")->GetValue();
					empID = string((char*)_bstr_t(vtValue));
					// Get the Full Name of the employee.
					vtValue = empRec->Fields->GetItem("FullName")->GetValue();
					empName = string((char*)_bstr_t(vtValue));
					// Get the address.
					vtValue = empRec->Fields->GetItem("Address")->GetValue();
					empAddress = string((char*)_bstr_t(vtValue));
					// Get the city.
					vtValue = empRec->Fields->GetItem("City")->GetValue();
					empCity = string((char*)_bstr_t(vtValue));
					// Get the state.
					vtValue = empRec->Fields->GetItem("State")->GetValue();
					empState = string((char*)_bstr_t(vtValue));
					// Get the zip code.
					vtValue = empRec->Fields->GetItem("ZipCode")->GetValue();
					empZip = string((char*)_bstr_t(vtValue));
					// Print the row of attributes.
					cout << endl << left << " " << setw(15) << empID << setw(20) << empName << setw(40) << empAddress << setw(20) << empCity << setw(12) << empState << setw(12) << empZip << endl;
					// Move the pointer to the next row in the set.
					empRec->MoveNext();
				}
				// Once all rows have been processed, prompt the user to enter a key to return to the main menu.
				cout << endl << "Press any key to return to the Main Menu...";
				// Wait for a key stroke.
				_getch();
				// Clear the screen.
				system("cls");
			}
			else
			{
				cout << "No records found!" << endl
					<< endl << "Press any key to return to the Main Menu...";
				_getch();
				system("cls");
			}
			break;
		case '2': // Search for records by Member Year

				  // Set only the badbit error flag for cin.
			cin.clear(ios::badbit);

			// Loop until there are no errors (valid integer entered).
			while (!cin.good())
			{
				// Clear the cin error flags.
				cin.clear();
				// Ignore up to 256 characters in the stdin buffer or up to where the newline character was entered.
				cin.ignore(256, '\n');
				// Clear the screen.
				system("cls");
				// Prompt the user for a year.
				cout << "Searching Records By Member Year" << endl << endl
					<< "Please enter a year to search: ";
				// Get the user input.
				cin >> enteredYear;
				// Test for error flags.
				if (!cin.good())
				{
					// If there are any error flags, send an error the user, and prompt the user
					// to try again.

					// Set only the badbit error flag.
					cin.clear(ios::badbit);
					// Clear the screen.
					system("cls");
					// Show the error message and prompt the user to try again.
					cout << "Invalid Year -- years must be whole numbers." << endl
						<< endl << "Press any key to try again...";
					// Wait for a key stroke.
					_getch();
				}
			}
			// Create a query string that gets employee records that have the entered member year.
			sprintf_s(QueryStr, 500, "SELECT EmployeeID, CONCAT(FirstName, ' ', LastName) \"FullName\", Address, City, State, ZipCode FROM [Employee] WHERE MemberYear = %i", enteredYear);
			// Execute the command, and return a record set.
			empRec = connDB->Execute(QueryStr, NULL, 1);
			// Display the column headers.
			cout << endl << left << " " << setw(15) << "Employee ID" << setw(20) << "Name" << setw(40) << "Address" << setw(20) << "City" << setw(12) << "State" << setw(12) << "Zip" << endl;
			// Print a horizontal rule.
			cout << "---------------------------------------------------------------------------------------------------------------------\n";
			// Test to see if there are records in the set.
			if (!(empRec->EndOfFile))
			{
				// If records are found, move to the first record in the set.
				empRec->MoveFirst();

				// Loop until all records have been processed.
				while (!(empRec->EndOfFile))
				{

					// If there is a record, obtain its attributes and display them in a row.

					// Get the employee ID.
					vtValue = empRec->Fields->GetItem("EmployeeID")->GetValue();
					empID = string((char*)_bstr_t(vtValue));
					// Get the full name.
					vtValue = empRec->Fields->GetItem("FullName")->GetValue();
					empName = string((char*)_bstr_t(vtValue));
					// Get the address.
					vtValue = empRec->Fields->GetItem("Address")->GetValue();
					empAddress = string((char*)_bstr_t(vtValue));
					// Get the city.
					vtValue = empRec->Fields->GetItem("City")->GetValue();
					empCity = string((char*)_bstr_t(vtValue));
					// Get the state.
					vtValue = empRec->Fields->GetItem("State")->GetValue();
					empState = string((char*)_bstr_t(vtValue));
					// Get the zip code.
					vtValue = empRec->Fields->GetItem("ZipCode")->GetValue();
					empZip = string((char*)_bstr_t(vtValue));
					// Display the row attributes in a printed row.
					cout << endl << left << " " << setw(15) << empID << setw(20) << empName << setw(40) << empAddress << setw(20) << empCity << setw(12) << empState << setw(12) << empZip << endl;
					// Move the record pointer to the next row in the set.
					empRec->MoveNext();
				}
				// Once all rows have been displayed, prompt the user to hit a key to return to the main menu.
				cout << endl << "Press any key to return to the Main Menu...";
				// Wait for a key stroke.
				_getch();
				// Clear the screen.
				system("cls");
			}
			else
			{
				// If nothing was returned from the database, display an error message and prompt
				// the user to hit a key to return to the main menu.

				// Display the message and prompt.
				cout << "No records found!" << endl
					<< endl << "Press any key to return to the Main Menu...";
				// Wait for a key stroke.
				_getch();
				// Clear the screen.
				system("cls");
			}
			break;
		case '3': // Search for records by association name.
				  // Clear the screen
			system("cls");
			// Prompt the user for an association name.
			cout << "Searching Records By Association" << endl << endl
				<< "Please enter an association to search: ";
			// Get the user's input.
			cin >> enteredAssociation;
			// Create a query string that will get records from the table based on a matching association name.
			sprintf_s(QueryStr, 500, "SELECT EmployeeID, CONCAT(FirstName, ' ', LastName) \"FullName\", Address, City, State, ZipCode FROM [Employee] WHERE LOWER([Association]) = LOWER('%s')", enteredAssociation.c_str());
			// Use the string to query the table for records.
			empRec = connDB->Execute(QueryStr, NULL, 1);
			// Display the column headers.
			cout << endl << left << " " << setw(15) << "Employee ID" << setw(20) << "Name" << setw(40) << "Address" << setw(20) << "City" << setw(12) << "State" << setw(12) << "Zip" << endl;
			// Print a horizontal rule.
			cout << "---------------------------------------------------------------------------------------------------------------------\n";
			// Test to see if the command returned any rows.
			if (!(empRec->EndOfFile))
			{
				// If rows were returned, move to the first record in the set.
				empRec->MoveFirst();
				// Loop until all rows have been processed and displayed.
				while (!(empRec->EndOfFile))
				{
					// If a record is found, get it's attributes and dislay them.

					// Get the employee ID.
					vtValue = empRec->Fields->GetItem("EmployeeID")->GetValue();
					empID = string((char*)_bstr_t(vtValue));
					// Get the full name.
					vtValue = empRec->Fields->GetItem("FullName")->GetValue();
					empName = string((char*)_bstr_t(vtValue));
					// Get the address.
					vtValue = empRec->Fields->GetItem("Address")->GetValue();
					empAddress = string((char*)_bstr_t(vtValue));
					// Get the city.
					vtValue = empRec->Fields->GetItem("City")->GetValue();
					empCity = string((char*)_bstr_t(vtValue));
					// Get the state.
					vtValue = empRec->Fields->GetItem("State")->GetValue();
					empState = string((char*)_bstr_t(vtValue));
					// Get the zip code.
					vtValue = empRec->Fields->GetItem("ZipCode")->GetValue();
					empZip = string((char*)_bstr_t(vtValue));
					// Display the row with employee attributes.
					cout << endl << left << " " << setw(15) << empID << setw(20) << empName << setw(40) << empAddress << setw(20) << empCity << setw(12) << empState << setw(12) << empZip << endl;
					// Move the record's pointer to the next record in the set.
					empRec->MoveNext();
				}
				// Once all the rows have been displayed, prompt the user to hit a key to return to the main menu.
				cout << endl << "Press any key to return to the Main Menu...";
				// Wait for a key stroke.
				_getch();
				// Clear the screen.
				system("cls");
			}
			else
			{
				// If no records were returned, show an error message and prompt the user to hit
				// a key to return to the main menu.

				// Display the error and prompt.
				cout << "No records found!" << endl
					<< endl << "Press any key to return to the Main Menu...";
				// Wait for a key stroke.
				_getch();
				// Clear the screen.
				system("cls");
			}
			break;
		case '4': // Add a new employee
		{
			// Set adding flag to true.
			adding = true;
			// Create an employee object to add information.
			Employee tempEmp;
			// Loop while user is still adding an employee.
			while (adding)
			{


				//
				// User enters first name for add.
				//

				// Reset the cin position and clear any error flags.
				cin.seekg(0, ios::end);
				cin.clear();
				// Set choosing flag to true, which indicates user is inputting data.
				choosing = true;
				// Loop until user enters valid data.
				while (choosing)
				{
					// Clear the screen.
					system("cls");
					// Prompt the user for data.
					cout << "Adding a New Employee (to abort at anytime, type <q> with the brackets) - Progress 1 of 10" << endl << endl
						<< "Enter first name: ";
					// Reset the cin position and clear error flags.
					cin.seekg(0, ios::end);
					cin.clear();
					// Get the user's input.
					getline(cin, enteredString);
					if (enteredString == "<q>")
					{
						adding = false;
						break;
					}
					if (enteredString.length() == 0 || enteredString.length() > 15)
					{
						// If not a valid length, return an error and make the user try again.
						// Clear the screen.
						system("cls");
						// Error message.
						cout << "First name is required and cannot be more than 15 characters long including spaces." << endl << endl
							<< "Press any key to try again...";
						// Wait for key stroke.
						_getch();
						// Start loop at next iteration.
						continue;
					}
					choosing = false;
				}
				if (!adding)
					break;

				// Add the first name to the temporary employee object.
				tempEmp.FName = enteredString;

				//
				// User enters last name for add.
				//

				// Reset the cin position and clear any error flags.
				cin.seekg(0, ios::end);
				cin.clear();
				// Set choosing flag to true, which indicates user is inputting data.
				choosing = true;
				// Loop until user enters valid data.
				while (choosing)
				{
					// Clear the screen.
					system("cls");
					// Prompt the user for data.
					cout << "Adding a New Employee (to abort at anytime, type <q> with the brackets) - Progress 2 of 10" << endl << endl
						<< "Enter last name: ";
					// Reset the cin position and clear error flags.
					cin.seekg(0, ios::end);
					cin.clear();
					// Get the user's input.
					getline(cin, enteredString);
					if (enteredString == "<q>")
					{
						adding = false;
						break;
					}
					if (enteredString.length() == 0 || enteredString.length() > 15)
					{
						// If not a valid length, return an error and make the user try again.
						// Clear the screen.
						system("cls");
						// Error message.
						cout << "Last name is required and cannot be more than 15 characters long including spaces." << endl << endl
							<< "Press any key to try again...";
						// Wait for key stroke.
						_getch();
						// Start loop at next iteration.
						continue;
					}
					choosing = false;
				}
				if (!adding)
					break;

				// Add the last name to the temporary employee object.
				tempEmp.LName = enteredString;

				//
				// User enters address for add.
				//

				// Reset the cin position and clear any error flags.
				cin.seekg(0, ios::end);
				cin.clear();
				// Set choosing flag to true, which indicates user is inputting data.
				choosing = true;
				// Loop until user enters valid data.
				while (choosing)
				{
					// Clear the screen.
					system("cls");
					// Prompt the user for data.
					cout << "Adding a New Employee (to abort at anytime, type <q> with the brackets) - Progress 3 of 10" << endl << endl
						<< "Enter address: ";
					// Reset the cin position and clear error flags.
					cin.seekg(0, ios::end);
					cin.clear();
					// Get the user's input.
					getline(cin, enteredString);
					if (enteredString == "<q>")
					{
						adding = false;
						break;
					}
					if (enteredString.length() == 0 || enteredString.length() > 25)
					{
						// If not a valid length, return an error and make the user try again.
						// Clear the screen.
						system("cls");
						// Error message.
						cout << "Address is required and cannot be more than 25 characters long including spaces." << endl << endl
							<< "Press any key to try again...";
						// Wait for key stroke.
						_getch();
						// Start loop at next iteration.
						continue;
					}
					choosing = false;
				}
				if (!adding)
					break;
				// Add the address to the temporary employee object.
				tempEmp.Address = enteredString;

				//
				// User enters city for add.
				//

				// Reset the cin position and clear any error flags.
				cin.seekg(0, ios::end);
				cin.clear();
				// Set choosing flag to true, which indicates user is inputting data.
				choosing = true;
				// Loop until user enters valid data.
				while (choosing)
				{
					// Clear the screen.
					system("cls");
					// Prompt the user for data.
					cout << "Adding a New Employee (to abort at anytime, type <q> with the brackets) - Progress 4 of 10" << endl << endl
						<< "Enter city: ";
					// Reset the cin position and clear error flags.
					cin.seekg(0, ios::end);
					cin.clear();
					// Get the user's input.
					getline(cin, enteredString);
					if (enteredString == "<q>")
					{
						adding = false;
						break;
					}
					if (enteredString.length() == 0 || enteredString.length() > 20)
					{
						// If not a valid length, return an error and make the user try again.
						// Clear the screen.
						system("cls");
						// Error message.
						cout << "City is required and cannot be more than 20 characters long including spaces." << endl << endl
							<< "Press any key to try again...";
						// Wait for key stroke.
						_getch();
						// Start loop at next iteration.
						continue;
					}
					choosing = false;
				}
				if (!adding)
					break;
				// Add the city to the temporary employee object.
				tempEmp.City = enteredString;

				//
				// User enters state for add.
				//

				// Reset the cin position and clear any error flags.
				cin.seekg(0, ios::end);
				cin.clear();
				// Set choosing flag to true, which indicates user is inputting data.
				choosing = true;
				// Loop until user enters valid data.
				while (choosing)
				{
					// Clear the screen.
					system("cls");
					// Prompt the user for data.
					cout << "Adding a New Employee (to abort at anytime, type <q> with the brackets) - Progress 5 of 10" << endl << endl
						<< "Enter state: ";
					// Reset the cin position and clear error flags.
					cin.seekg(0, ios::end);
					cin.clear();
					// Get the user's input.
					getline(cin, enteredString);
					if (enteredString == "<q>")
					{
						adding = false;
						break;
					}
					if (!ValidState(enteredString))
					{
						// If not a valid state input, return an error and make the user try again.
						// Clear the screen.
						system("cls");
						// Error message.
						cout << "State must be 2 letters (example: TX for Texas)." << endl << endl
							<< "Press any key to try again...";
						// Wait for key stroke.
						cout << enteredString;
						_getch();
						// Start loop at next iteration.
						continue;
					}
					choosing = false;
				}
				if (!adding)
					break;
				// Add the state to the temporary employee object.
				tempEmp.State = enteredString;

				//
				// User enters zip for add.
				//

				// Reset the cin position and clear any error flags.
				cin.seekg(0, ios::end);
				cin.clear();
				// Set choosing flag to true, which indicates user is inputting data.
				choosing = true;
				// Loop until user enters valid data.
				while (choosing)
				{
					// Clear the screen.
					system("cls");
					// Prompt the user for data.
					cout << "Adding a New Employee (to abort at anytime, type <q> with the brackets) - Progress 6 of 10" << endl << endl
						<< "Enter zip code: ";
					// Reset the cin position and clear error flags.
					cin.seekg(0, ios::end);
					cin.clear();
					// Get the user's input.
					getline(cin, enteredString);
					if (enteredString == "<q>")
					{
						adding = false;
						break;
					}
					if (!ValidZip(enteredString))
					{
						// If not a valid zip code, return an error and make the user try again.
						// Clear the screen.
						system("cls");
						// Error message.
						cout << "Zip code must consist of 5 digits and may not start with a 0." << endl << endl
							<< "Press any key to try again...";
						// Wait for key stroke.
						_getch();
						// Start loop at next iteration.
						continue;
					}
					choosing = false;
				}
				if (!adding)
					break;
				// Add the zip code to the temporary employee object.
				tempEmp.ZipCode = enteredString;

				//
				// User enters association for add.
				//

				// Reset the cin position and clear any error flags.
				cin.seekg(0, ios::end);
				cin.clear();
				// Set choosing flag to true, which indicates user is inputting data.
				choosing = true;
				// Loop until user enters valid data.
				while (choosing)
				{
					// Clear the screen.
					system("cls");
					// Prompt the user for data.
					cout << "Adding a New Employee (to abort at anytime, type <q> with the brackets) - Progress 7 of 10" << endl << endl
						<< "Enter association: ";
					// Reset the cin position and clear error flags.
					cin.seekg(0, ios::end);
					cin.clear();
					// Get the user's input.
					getline(cin, enteredString);
					if (enteredString == "<q>")
					{
						adding = false;
						break;
					}
					if (enteredString.length() == 0 || enteredString.length() > 10)
					{
						// If not a valid length, return an error and make the user try again.
						// Clear the screen.
						system("cls");
						// Error message.
						cout << "Association is required and cannot be more than 10 characters long including spaces." << endl << endl
							<< "Press any key to try again...";
						// Wait for key stroke.
						_getch();
						// Start loop at next iteration.
						continue;
					}
					choosing = false;
				}
				if (!adding)
					break;
				// Add the association to the temporary employee object.
				tempEmp.Association = enteredString;

				//
				// User enters email for add.
				//

				// Reset the cin position and clear any error flags.
				cin.seekg(0, ios::end);
				cin.clear();
				// Set choosing flag to true, which indicates user is inputting data.
				choosing = true;
				// Loop until user enters valid data.
				while (choosing)
				{
					// Clear the screen.
					system("cls");
					// Prompt the user for data.
					cout << "Adding a New Employee (to abort at anytime, type <q> with the brackets) - Progress 8 of 10" << endl << endl
						<< "Enter email: ";
					// Reset the cin position and clear error flags.
					cin.seekg(0, ios::end);
					cin.clear();
					// Get the user's input.
					getline(cin, enteredString);
					if (enteredString == "<q>")
					{
						adding = false;
						break;
					}
					if (!ValidEmail(enteredString) && enteredString != "")
					{
						// If not a valid length, return an error and make the user try again.
						// Clear the screen.
						system("cls");
						// Error message.
						cout << "Invalid email. The email must be blank or in the format, user@company.com. Email "
							<< "addresses must start with a letter.  \nThey cannot contain numbers in the extension (the .com part)." << endl << endl
							<< "Press any key to try again...";
						// Wait for key stroke.
						_getch();
						// Start loop at next iteration.
						continue;
					}
					choosing = false;
				}
				if (!adding)
					break;
				// Add the email to the temporary employee object.
				tempEmp.Email = enteredString;

				//
				// User enters phone for add.
				//

				// Reset the cin position and clear any error flags.
				cin.seekg(0, ios::end);
				cin.clear();
				// Set choosing flag to true, which indicates user is inputting data.
				choosing = true;
				// Loop until user enters valid data.
				while (choosing)
				{
					// Clear the screen.
					system("cls");
					// Prompt the user for data.
					cout << "Adding a New Employee (to abort at anytime, type <q> with the brackets) - Progress 9 of 10" << endl << endl
						<< "Enter phone number (xxx-xxx-xxxx): ";
					// Reset the cin position and clear error flags.
					cin.seekg(0, ios::end);
					cin.clear();
					// Get the user's input.
					getline(cin, enteredString);
					if (enteredString == "<q>")
					{
						adding = false;
						break;
					}
					if (!ValidPhone(enteredString) && enteredString != "") {
						// If not a valid length, return an error and make the user try again.
						// Clear the screen.
						system("cls");
						// Error message.
						cout << "Phone number must contain 10 digits or be left blank." << endl << endl
							<< "Press any key to try again...";
						// Wait for key stroke.
						_getch();
						// Start loop at next iteration.
						continue;
					}
					// If returned valid as phone number, format the string.
					enteredString = FormatPhone(enteredString);
					choosing = false;
				}
				if (!adding)
					break;
				// Add the phone number to the temporary employee object.
				tempEmp.Phone = enteredString;

				//
				// User enters member year for add.
				//

				// Reset the cin position and clear any error flags.
				cin.seekg(0, ios::end);
				cin.clear();
				// Set choosing flag to true, which indicates user is inputting data.
				choosing = true;
				// Loop until user enters valid data.
				while (choosing)
				{
					// Clear the screen.
					system("cls");
					// Prompt the user for data.
					cout << "Adding a New Employee (to abort at anytime, type <q> with the brackets) - Progress 10 of 10" << endl << endl
						<< "Enter member year: ";
					// Reset the cin position and clear error flags.
					cin.seekg(0, ios::end);
					cin.clear();
					// Get the user's input.
					getline(cin, enteredString);
					if (enteredString == "<q>")
					{
						adding = false;
						break;
					}
					if (!ValidYear(enteredString)) {
						// If not a valid year, return an error and make the user try again.
						// Clear the screen.
						system("cls");
						// Error message.
						cout << "Member year is required and must be a valid year between 0-9999 inclusive." << endl << endl
							<< "Press any key to try again...";
						// Wait for key stroke.
						_getch();
						// Start loop at next iteration.
						continue;
					}
					choosing = false;
					// Set year as an integer.
					enteredYear = YearToInt(enteredString);
				}
				if (!adding)
					break;
				// Add the member year to the temporary employee object.
				tempEmp.MemberYear = enteredYear;
				try
				{
					// Before inserting, we need the max ID from the employees table.

					// Query the database for the Max employeeID.
					sprintf_s(QueryStr, 500, "SELECT MAX(EmployeeID) \"Max\" FROM [Employee]");
					empRec = connDB->Execute(QueryStr, NULL, 1);
					// Check to see if a record was returned.
					if (!(empRec->EndOfFile))
					{
						// If a record was found, go to it.
						empRec->MoveFirst();
						// Retrieve the MAX id value.
						vtValue = empRec->Fields->GetItem("Max")->GetValue();
						// Add one to it and store it in 'maxID'.
						maxID = YearToInt(string((char*)_bstr_t(vtValue))) + 1;
					}
					else
					{
						// If no max ID was returned, there are no records.  Return an error to the
						// user and prompt them to hit a key to return to the main menu.

						// Clear the screen.
						system("cls");
						// Display the error and prompt.
						cout << "No records found!" << endl
							<< endl << "Press any key to return to the Main Menu...";
						// Wait for a key stroke.
						_getch();
						// Clear the screen again.
						system("cls");
						// Break from the switch statement.
						break;
					}
				}
				catch (const _com_error &e)
				{
					// If the max ID operation returned an error, display it to the user
					// and end the switch statement.

					// Clear the screen.
					system("cls");

					// Display the error.
					cout << "Error retrieving MAX EmployeeID: " << endl;
					error(e, ErrStr);
					printf(ErrStr);
					// Prompt the user to hit a key to return to the main menu.
					cout << endl << endl << "Press any key to return to Main Menu...";
					// Wait for a key stroke.
					_getch();
					// End the switch statment.
					break;
				}
				try
				{
					// If everything was successful (Info received from user and MAX employeeID found), then
					// attempt to insert this new record into the Employee table.

					// Create a parameterized query for inserting the record.
					sprintf_s(QueryStr, 500, "INSERT INTO [Employee] (EmployeeID, FirstName, LastName, Address, \
City, State, ZipCode, Association, Email, Phone, MemberYear) VALUES (%i, '%s', '%s', '%s', '%s', '%s', '%s', \
'%s', '%s', '%s', %i);", maxID, tempEmp.FName.c_str(), tempEmp.LName.c_str(), tempEmp.Address.c_str(),
tempEmp.City.c_str(), tempEmp.State.c_str(), tempEmp.ZipCode.c_str(), tempEmp.Association.c_str(),
tempEmp.Email.c_str(), tempEmp.Phone.c_str(), tempEmp.MemberYear);
					// Execute the command with the query string.
					connDB->Execute(QueryStr, NULL, 1);
					// Clear the screen.
					system("cls");
					// Upon a successful insert, display all of the new employee's information.
					cout << "The employee, " << tempEmp.FName << " " << tempEmp.LName << ", was successfully added." << endl << endl
						<< right << setw(20) << "Employee ID: " << left << maxID << endl
						<< right << setw(20) << "First Name: " << left << tempEmp.FName << endl
						<< right << setw(20) << "Last Name: " << left << tempEmp.LName << endl
						<< right << setw(20) << "Address: " << left << tempEmp.Address << endl
						<< right << setw(20) << "City: " << left << tempEmp.City << endl
						<< right << setw(20) << "State: " << left << tempEmp.State << endl
						<< right << setw(20) << "Zip Code: " << left << tempEmp.ZipCode << endl
						<< right << setw(20) << "Association: " << left << tempEmp.Association << endl
						<< right << setw(20) << "Email: " << left << tempEmp.Email << endl
						<< right << setw(20) << "Phone Number: " << left << tempEmp.Phone << endl
						<< right << setw(20) << "Member Year: " << left << tempEmp.MemberYear << endl << endl
						<< "Press any key to return to the Main Menu...";
					// Wait for a key stroke to continue to the main menu.
					_getch();
				}
				catch (const _com_error &e)
				{
					// If the insert operation failed, display the error to the user.

					// Clear the screen.
					system("cls");

					// Display the error.
					cout << " DB Insert Error: " << endl;
					error(e, ErrStr);
					printf(ErrStr);
					// Prompt the user to hit any key to return to the main menu.
					cout << endl << endl << "Press any key to return to Main Menu...";
					// Wait for a key stroke.
					_getch();
					// End the switch statement.
					break;
				}
				// End adding a new employee to exit the loop.
				adding = false;
			}
			// End this case in the switch statement.
			break;
		}
		case '5': // Update an existing employee.
				  // Clear the screen.
			system("cls");
			// Create a query that will get all the employee IDs and names to display.
			sprintf_s(QueryStr, 500, "SELECT EmployeeID, CONCAT(FirstName, ' ', LastName) \"FullName\" FROM [Employee]");
			// Use the string to query the database and return a record set.
			empRec = connDB->Execute(QueryStr, NULL, 1);
			// Check for existing records.
			if (!(empRec->EndOfFile))
			{
				// If there are records found, display them all.

				// Set only the badbit flag to enter the loop.
				cin.clear(ios::badbit);

				while (!cin.good())
				{
					// Loop until the user enters a valid integer.

					// Set the counter to 0, which indicates the first time through the display loop.
					ctr = 0;
					// Move to the first record in the set.
					empRec->MoveFirst();
					while (!(empRec->EndOfFile))
					{
						// If records are found, continue printing them as rows.

						if (ctr == 0)
						{
							// If this is the first loop, display the row headers and horizontal rule.

							// Clear the screen.
							system("cls");
							// Display the title.
							cout << "Update An Employee Record" << endl;
							// Display the row headers.
							cout << endl << left << " " << setw(15) << "Employee ID" << setw(30) << "Name" << endl;
							// Print a horizontal rule.
							cout << "---------------------------------------------------------------------------------------------------------------------\n";

						}
						// Increment the counter so the column headers won't print again.
						ctr++;
						// Get the employee ID.
						vtValue = empRec->Fields->GetItem("EmployeeID")->GetValue();
						empID = string((char*)_bstr_t(vtValue));
						// Get the full name.
						vtValue = empRec->Fields->GetItem("FullName")->GetValue();
						empName = string((char*)_bstr_t(vtValue));
						// Print the row of attributes.
						cout << left << " " << setw(15) << empID << setw(30) << empName << "\n";
						// Move to the next record in the set.
						empRec->MoveNext();
					}
					// Once all rows have been displayed, prompt the user for an employee ID to indicate
					// which record should be updated.
					cout << endl << "Enter the employee ID to update that record: ";
					// Clear all cin error flags.
					cin.clear();
					// Ignore up to 256 characters in the stdin buffer or up to the first newline character.
					cin.ignore(256, '\n');
					// Get the user input.
					cin >> enteredID;

					if (!cin.good())
					{
						// If the user did not enter a valid integer, return an error and prompt the user
						// to try again.

						// Set only the badbit error flag for cin.
						cin.clear(ios::badbit);
						// Ignore up to 256 characters in the stdin buffer or up to the first newline character.
						cin.ignore(256, '\n');
						// Clear the screen.
						system("cls");
						// Display the error and prompt.
						cout << "Invalid ID. Employee IDs must be whole numbers." << endl << endl
							<< "Press any key to try again...";
						// Wait for a key stroke.
						_getch();
						// return to the top of this loop and make the user try again.
						continue;
					}
					// Once a valid integer has been entered, set the updating flag to 
					// indicate there is a recording being altered.
					updating = true;
					// Loop until the user selects to end editing and return to the main menu.
					while (updating)
					{
						// Clear the screen.
						system("cls");
						// Create a query to select the employee record that matches the entered ID number.
						sprintf_s(QueryStr, 500, "SELECT EmployeeID, FirstName, LastName, CONCAT(FirstName, ' ', LastName) \"FullName\", Address, City, State, ZipCode, Association, Email, Phone, MemberYear FROM [Employee] WHERE EmployeeID = %i", enteredID);
						// Use the querty to get a record set from the database.
						empRec = connDB->Execute(QueryStr, NULL, 1);
						// Check to see if a row was returned.
						if (!(empRec->EndOfFile))
						{
							// If the record exists, move the pointer to it, get the data from the row
							// and display a menu for the user to pick which attribute to update.

							// Go the the first record.
							empRec->MoveFirst();
							// Get the employee ID.
							vtValue = empRec->Fields->GetItem("EmployeeID")->GetValue();
							empID = string((char*)_bstr_t(vtValue));
							// Get the first name.
							vtValue = empRec->Fields->GetItem("FirstName")->GetValue();
							empFName = string((char*)_bstr_t(vtValue));
							// Get the last name.
							vtValue = empRec->Fields->GetItem("LastName")->GetValue();
							empLName = string((char*)_bstr_t(vtValue));
							// Get the full name.
							vtValue = empRec->Fields->GetItem("FullName")->GetValue();
							empName = string((char*)_bstr_t(vtValue));
							// Get the address.
							vtValue = empRec->Fields->GetItem("Address")->GetValue();
							empAddress = string((char*)_bstr_t(vtValue));
							// Get the city.
							vtValue = empRec->Fields->GetItem("City")->GetValue();
							empCity = string((char*)_bstr_t(vtValue));
							// Get the state.
							vtValue = empRec->Fields->GetItem("State")->GetValue();
							empState = string((char*)_bstr_t(vtValue));
							// Get the zip code.
							vtValue = empRec->Fields->GetItem("ZipCode")->GetValue();
							empZip = string((char*)_bstr_t(vtValue));
							// Get the association.
							vtValue = empRec->Fields->GetItem("Association")->GetValue();
							empAssocation = string((char*)_bstr_t(vtValue));
							// Attempt to get the email.
							try
							{
								vtValue = empRec->Fields->GetItem("Email")->GetValue();
								empEmail = string((char*)_bstr_t(vtValue));
							}
							catch (const _com_error)
							{
								// If no email was found, set a blank string.
								empEmail = "";
							}
							// Attempt to get the phone number.
							try
							{
								vtValue = empRec->Fields->GetItem("Phone")->GetValue();
								empPhone = string((char*)_bstr_t(vtValue));
							}
							catch (const _com_error)
							{
								// If no phone number was found, set a blank string.
								empPhone = "";
							}
							// Get the member year.
							vtValue = empRec->Fields->GetItem("MemberYear")->GetValue();
							empMemberYear = string((char*)_bstr_t(vtValue));
							// Set the choosing flag to indicate the program is waiting for the user
							// to enter valid input.
							choosing = true;
							while (choosing)
							{
								// Loop until the user has selected a valid response.

								// Clear the screen.
								system("cls");

								// Display a menu of editable attributes for the currently selected employee.
								cout << "Select an attribute to update.\n"
									<< "\tA) First Name (" << empFName << ")\n"
									<< "\tB) Last Name (" << empLName << ")\n"
									<< "\tC) Address (" << empAddress << ")\n"
									<< "\tD) City (" << empCity << ")\n"
									<< "\tE) State (" << empState << ")\n"
									<< "\tF) Zip Code (" << empZip << ")\n"
									<< "\tG) Association (" << empAssocation << ")\n"
									<< "\tH) Email (" << empEmail << ")\n"
									<< "\tI) Phone (" << empPhone << ")\n"
									<< "\tJ) Member Year (" << empMemberYear << ")\n"
									<< "\tK) Return To Main Menu\n"
									<< "Your Selection: ";
								// Get input from the user.
								cin >> fullSelection;
								// Test the input for a valid length.
								if (fullSelection.size() != 1)
								{
									// If the user did not enter a single character, dispaly an error, and
									// prompt the user to try again.

									// Clear the screen.
									system("cls");
									// Display the error and prompt.
									cout << "Invalid selection. The selection must be a letter between A-K inclusive." << endl << endl
										<< "Press any key to try again...";
									// Wait for a key stroke.
									_getch();
									// Go back to the top of the loop for its next iteration.
									continue;
								}
								// If a single character was entered, extract it as a character variable.
								selection = fullSelection[0];
								// Make sure it is uppercase.
								selection = toupper(selection);
								// Test the character for a valid ASCII range.
								if (selection < 'A' || selection > 'K')
								{
									// If the character is out of range, display an error and prompt the user
									// to try again.

									// Clear the screen.
									system("cls");
									// Display the error and prompt.
									cout << "Invalid selection. The selection must be a letter between A-K inclusive." << endl << endl
										<< "Press any key to try again...";
									// Wait for a key stroke.
									_getch();
									// Return to the top of the loop for its next iteration.
									continue;
								}
								// At this point the user has entered correct input, so set the flag to false.
								choosing = false;

								// Use the users selection to edit that specific attribute.
								switch (selection)
								{
								case 'A': // First Name
										  // Set boolean to true for user input validation.
									choosing = true;
									while (choosing)
									{
										// Loop until valid input has been entered.
										// Clear the screen.
										system("cls");
										// Prompt the user to enter a new value.
										cout << "Updating Record -- " << empName << endl
											<< "Current First Name: " << empFName << endl
											<< "Enter New First Name: ";
										// Reset cin position and clear error flags.
										cin.seekg(0, ios::end);
										cin.clear();

										// Get user input.
										getline(cin, enteredString);

										// Test for the correct size of string (because of database column maximums).
										if ((int)enteredString.size() > 15 || (int)enteredString.size() == 0) {
											// If not a valid length, return an error and make the user try again.
											// Clear the screen.
											system("cls");
											// Error message.
											cout << "First name is required and cannot be more than 15 characters long including spaces." << endl << endl
												<< "Press any key to try again...";
											// Wait for key stroke.
											_getch();
											// Start loop at next iteration.
											continue;
										}
										// If valid input, set choosing flag to false.
										choosing = false;
									}
									try
									{
										// Once a valid input has been entered, attempt to update the attribute with
										// the entered string.

										// Set up the query string with parameters.
										sprintf_s(QueryStr, 500, "Update [Employee] SET FirstName = '%s' WHERE EmployeeID = %i", enteredString.c_str(), enteredID);
										// Execute the query.
										connDB->Execute(QueryStr, NULL, 1);
										// If successfully executed, clear the screen.
										system("cls");
										// Show a success message.
										cout << "Updated record successfully.  Changed first name from " << empFName << " to " << enteredString << "." << endl << endl
											<< "Press any key to return to the Update Menu...";
										// Wait for a key stroke.
										_getch();
									}
									catch (const _com_error &e)
									{
										// If record could not be updated, show the user the error that
										// was returned by the database.

										// Clear the screen.
										system("cls");
										// Show the error.
										cout << " Database Error: " << endl;
										// Put the error in a character array.
										error(e, ErrStr);
										// Print the error char array to the console.
										printf(ErrStr);
										cout << endl << endl << "Press any key to return to the Main Menu...";
										// End updating.
										updating = false;
										// Wait for a key stroke.
										_getch();
									}
									break;
								case 'B': // Last Name
										  // Set boolean to true for user input validation.
									choosing = true;
									while (choosing)
									{
										// Loop until valid input has been entered.
										// Clear the screen.
										system("cls");
										// Prompt the user to enter a new value.
										cout << "Updating Record -- " << empName << endl
											<< "Current Last Name: " << empLName << endl
											<< "Enter New Last Name: ";
										// Reset cin position and clear error flags.
										cin.seekg(0, ios::end);
										cin.clear();

										// Get user input.
										getline(cin, enteredString);

										// Test for the correct size of string (because of database column maximums).
										if (enteredString.size() > 15 || (int)enteredString.size() == 0) {
											// If not a valid length, return an error and make the user try again.
											// Clear the screen.
											system("cls");
											// Error message.
											cout << "Last name is required and cannot be more than 15 characters long including spaces." << endl << endl
												<< "Press any key to try again...";
											// Wait for key stroke.
											_getch();
											// Start loop at next iteration.
											continue;
										}
										// If valid input, set choosing flag to false.
										choosing = false;
									}
									try
									{
										// Once a valid input has been entered, attempt to update the attribute with
										// the entered string.

										// Set up the query string with parameters.
										sprintf_s(QueryStr, 500, "Update [Employee] SET LastName = '%s' WHERE EmployeeID = %i", enteredString.c_str(), enteredID);
										// Execute the query.
										connDB->Execute(QueryStr, NULL, 1);
										// If successfully executed, clear the screen.
										system("cls");
										// Show a success message.
										cout << "Updated record successfully.  Changed last name from " << empLName << " to " << enteredString << "." << endl << endl
											<< "Press any key to return to the Update Menu...";
										// Wait for a key stroke.
										_getch();
									}
									catch (const _com_error &e)
									{
										// If record could not be updated, show the user the error that
										// was returned by the database.

										// Clear the screen.
										system("cls");
										// Show the error.
										cout << " Database Error: " << endl;
										// Put the error in a character array.
										error(e, ErrStr);
										// Print the error char array to the console.
										printf(ErrStr);
										cout << endl << endl << "Press any key to return to the Main Menu...";
										// End updating.
										updating = false;
										// Wait for a key stroke.
										_getch();
									}
									break;
								case 'C': // Address
										  // Set boolean to true for user input validation.
									choosing = true;
									while (choosing)
									{
										// Loop until valid input has been entered.
										// Clear the screen.
										system("cls");
										// Prompt the user to enter a new value.
										cout << "Updating Record -- " << empName << endl
											<< "Current Address: " << empAddress << endl
											<< "Enter New Address: ";
										// Reset cin position and clear error flags.
										cin.seekg(0, ios::end);
										cin.clear();

										// Get user input.
										getline(cin, enteredString);

										// Test for the correct size of string (because of database column maximums).
										if (enteredString.size() > 25 || (int)enteredString.size() == 0) {
											// If not a valid length, return an error and make the user try again.
											// Clear the screen.
											system("cls");
											// Error message.
											cout << "Address is required and cannot be more than 25 characters long including spaces." << endl << endl
												<< "Press any key to try again...";
											// Wait for key stroke.
											_getch();
											// Start loop at next iteration.
											continue;
										}
										// If valid input, set choosing flag to false.
										choosing = false;
									}
									try
									{
										// Once a valid input has been entered, attempt to update the attribute with
										// the entered string.

										// Set up the query string with parameters.
										sprintf_s(QueryStr, 500, "Update [Employee] SET Address = '%s' WHERE EmployeeID = %i", enteredString.c_str(), enteredID);
										// Execute the query.
										connDB->Execute(QueryStr, NULL, 1);
										// If successfully executed, clear the screen.
										system("cls");
										// Show a success message.
										cout << "Updated record successfully.  Changed address from " << empAddress << " to " << enteredString << "." << endl << endl
											<< "Press any key to return to the Update Menu...";
										// Wait for a key stroke.
										_getch();
									}
									catch (const _com_error &e)
									{
										// If record could not be updated, show the user the error that
										// was returned by the database.

										// Clear the screen.
										system("cls");
										// Show the error.
										cout << " Database Error: " << endl;
										// Put the error in a character array.
										error(e, ErrStr);
										// Print the error char array to the console.
										printf(ErrStr);
										cout << endl << endl << "Press any key to return to the Main Menu...";
										// End updating.
										updating = false;
										// Wait for a key stroke.
										_getch();
									}
									break;
								case 'D': // City
										  // Set boolean to true for user input validation.
									choosing = true;
									while (choosing)
									{
										// Loop until valid input has been entered.
										// Clear the screen.
										system("cls");
										// Prompt the user to enter a new value.
										cout << "Updating Record -- " << empName << endl
											<< "Current City: " << empCity << endl
											<< "Enter New City: ";
										// Reset cin position and clear error flags.
										cin.seekg(0, ios::end);
										cin.clear();

										// Get user input.
										getline(cin, enteredString);

										// Test for the correct size of string (because of database column maximums).
										if (enteredString.size() > 20 || (int)enteredString.size() == 0) {
											// If not a valid length, return an error and make the user try again.
											// Clear the screen.
											system("cls");
											// Error message.
											cout << "City is required and cannot be more than 20 characters long including spaces." << endl << endl
												<< "Press any key to try again...";
											// Wait for key stroke.
											_getch();
											// Start loop at next iteration.
											continue;
										}
										// If valid input, set choosing flag to false.
										choosing = false;
									}
									try
									{
										// Once a valid input has been entered, attempt to update the attribute with
										// the entered string.

										// Set up the query string with parameters.
										sprintf_s(QueryStr, 500, "Update [Employee] SET City = '%s' WHERE EmployeeID = %i", enteredString.c_str(), enteredID);
										// Execute the query.
										connDB->Execute(QueryStr, NULL, 1);
										// If successfully executed, clear the screen.
										system("cls");
										// Show a success message.
										cout << "Updated record successfully.  Changed city from " << empCity << " to " << enteredString << "." << endl << endl
											<< "Press any key to return to the Update Menu...";
										// Wait for a key stroke.
										_getch();
									}
									catch (const _com_error &e)
									{
										// If record could not be updated, show the user the error that
										// was returned by the database.

										// Clear the screen.
										system("cls");
										// Show the error.
										cout << " Database Error: " << endl;
										// Put the error in a character array.
										error(e, ErrStr);
										// Print the error char array to the console.
										printf(ErrStr);
										cout << endl << endl << "Press any key to return to the Main Menu...";
										// End updating.
										updating = false;
										// Wait for a key stroke.
										_getch();
									}
									break;
								case 'E': // State
										  // Set boolean to true for user input validation.
									choosing = true;
									while (choosing)
									{
										// Loop until valid input has been entered.
										// Clear the screen.
										system("cls");
										// Prompt the user to enter a new value.
										cout << "Updating Record -- " << empName << endl
											<< "Current State: " << empState << endl
											<< "Enter New State Abbreviation: ";
										// Reset cin position and clear error flags.
										cin.seekg(0, ios::end);
										cin.clear();

										// Get user input.
										getline(cin, enteredString);

										// Test for the correct size of string (because of database column maximums).
										if (!ValidState(enteredString)) {
											// If not a valid state input, return an error and make the user try again.
											// Clear the screen.
											system("cls");
											// Error message.
											cout << "State must be 2 letters (example: TX for Texas)." << endl << endl
												<< "Press any key to try again...";
											// Wait for key stroke.
											cout << enteredString;
											_getch();
											// Start loop at next iteration.
											continue;
										}
										// Make sure both characters in the state abbreviation are capital letters.
										enteredString[0] = toupper(enteredString[0]);
										enteredString[1] = toupper(enteredString[1]);
										// If valid input, set choosing flag to false.
										choosing = false;
									}
									try
									{
										// Once a valid input has been entered, attempt to update the attribute with
										// the entered string.

										// Set up the query string with parameters.
										sprintf_s(QueryStr, 500, "Update [Employee] SET State = '%s' WHERE EmployeeID = %i", enteredString.c_str(), enteredID);
										// Execute the query.
										connDB->Execute(QueryStr, NULL, 1);
										// If successfully executed, clear the screen.
										system("cls");
										// Show a success message.
										cout << "Updated record successfully.  Changed state from " << empState << " to " << enteredString << "." << endl << endl
											<< "Press any key to return to the Update Menu...";
										// Wait for a key stroke.
										_getch();
									}
									catch (const _com_error &e)
									{
										// If record could not be updated, show the user the error that
										// was returned by the database.

										// Clear the screen.
										system("cls");
										// Show the error.
										cout << " Database Error: " << endl;
										// Put the error in a character array.
										error(e, ErrStr);
										// Print the error char array to the console.
										printf(ErrStr);
										cout << endl << endl << "Press any key to return to the Main Menu...";
										// End updating.
										updating = false;
										// Wait for a key stroke.
										_getch();
									}
									break;
								case 'F': // Zip
										  // Set boolean to true for user input validation.
									choosing = true;
									while (choosing)
									{
										// Loop until valid input has been entered.
										// Clear the screen.
										system("cls");
										// Prompt the user to enter a new value.
										cout << "Updating Record -- " << empName << endl
											<< "Current Zip Code: " << empZip << endl
											<< "Enter New Zip Code: ";
										// Reset cin position and clear error flags.
										cin.seekg(0, ios::end);
										cin.clear();

										// Get user input.
										getline(cin, enteredString);

										// Test for the correct size of string (because of database column maximums).
										if (!ValidZip(enteredString)) {
											// If not a valid zip code, return an error and make the user try again.
											// Clear the screen.
											system("cls");
											// Error message.
											cout << "Zip code must consist of 5 digits and may not start with a 0." << endl << endl
												<< "Press any key to try again...";
											// Wait for key stroke.
											_getch();
											// Start loop at next iteration.
											continue;
										}
										// If valid input, set choosing flag to false.
										choosing = false;
									}
									try
									{
										// Once a valid input has been entered, attempt to update the attribute with
										// the entered string.

										// Set up the query string with parameters.
										sprintf_s(QueryStr, 500, "Update [Employee] SET ZipCode = '%s' WHERE EmployeeID = %i", enteredString.c_str(), enteredID);
										// Execute the query.
										connDB->Execute(QueryStr, NULL, 1);
										// If successfully executed, clear the screen.
										system("cls");
										// Show a success message.
										cout << "Updated record successfully.  Changed zip code from " << empZip << " to " << enteredString << "." << endl << endl
											<< "Press any key to return to the Update Menu...";
										// Wait for a key stroke.
										_getch();
									}
									catch (const _com_error &e)
									{
										// If record could not be updated, show the user the error that
										// was returned by the database.

										// Clear the screen.
										system("cls");
										// Show the error.
										cout << " Database Error: " << endl;
										// Put the error in a character array.
										error(e, ErrStr);
										// Print the error char array to the console.
										printf(ErrStr);
										cout << endl << endl << "Press any key to return to the Main Menu...";
										// End updating.
										updating = false;
										// Wait for a key stroke.
										_getch();
									}
									break;
								case 'G': // Association
										  // Set boolean to true for user input validation.
									choosing = true;
									while (choosing)
									{
										// Loop until valid input has been entered.
										// Clear the screen.
										system("cls");
										// Prompt the user to enter a new value.
										cout << "Updating Record -- " << empName << endl
											<< "Current Association: " << empAssocation << endl
											<< "Enter New Association: ";
										// Reset cin position and clear error flags.
										cin.seekg(0, ios::end);
										cin.clear();

										// Get user input.
										getline(cin, enteredString);

										// Test for the correct size of string (because of database column maximums).
										if ((int)enteredString.size() > 10 || (int)enteredString.size() == 0) {
											// If not a valid length, return an error and make the user try again.
											// Clear the screen.
											system("cls");
											// Error message.
											cout << "Association is required and cannot be more than 10 characters long including spaces." << endl << endl
												<< "Press any key to try again...";
											// Wait for key stroke.
											_getch();
											// Start loop at next iteration.
											continue;
										}
										// If valid input, set choosing flag to false.
										choosing = false;
									}
									try
									{
										// Once a valid input has been entered, attempt to update the attribute with
										// the entered string.

										// Set up the query string with parameters.
										sprintf_s(QueryStr, 500, "Update [Employee] SET Association = '%s' WHERE EmployeeID = %i", enteredString.c_str(), enteredID);
										// Execute the query.
										connDB->Execute(QueryStr, NULL, 1);
										// If successfully executed, clear the screen.
										system("cls");
										// Show a success message.
										cout << "Updated record successfully.  Changed association from " << empAssocation << " to " << enteredString << "." << endl << endl
											<< "Press any key to return to the Update Menu...";
										// Wait for a key stroke.
										_getch();
									}
									catch (const _com_error &e)
									{
										// If record could not be updated, show the user the error that
										// was returned by the database.

										// Clear the screen.
										system("cls");
										// Show the error.
										cout << " Database Error: " << endl;
										// Put the error in a character array.
										error(e, ErrStr);
										// Print the error char array to the console.
										printf(ErrStr);
										cout << endl << endl << "Press any key to return to the Main Menu...";
										// End updating.
										updating = false;
										// Wait for a key stroke.
										_getch();
									}
									break;
								case 'H': // Email
										  // Set boolean to true for user input validation.
									choosing = true;
									while (choosing)
									{
										// Loop until valid input has been entered.
										// Clear the screen.
										system("cls");
										// Prompt the user to enter a new value.
										cout << "Updating Record -- " << empName << endl
											<< "Current Email: " << empEmail << endl
											<< "Enter New Email: ";
										// Reset cin position and clear error flags.
										cin.seekg(0, ios::end);
										cin.clear();

										// Get user input.
										getline(cin, enteredString);

										// Test for the correct size of string (because of database column maximums).
										if (!ValidEmail(enteredString) && enteredString != "") {
											// If not a valid length, return an error and make the user try again.
											// Clear the screen.
											system("cls");
											// Error message.
											cout << "Invalid email. The email must be blank or in the format, user@company.com. Email "
												<< "addresses must start with a letter. \nThey cannot contain numbers in the extension (the .com part)." << endl << endl
												<< "Press any key to try again...";
											// Wait for key stroke.
											_getch();
											// Start loop at next iteration.
											continue;
										}
										// If valid input, set choosing flag to false.
										choosing = false;
									}
									try
									{
										// Once a valid input has been entered, attempt to update the attribute with
										// the entered string.

										// Set up the query string with parameters.
										sprintf_s(QueryStr, 500, "Update [Employee] SET Email = '%s' WHERE EmployeeID = %i", enteredString.c_str(), enteredID);
										// Execute the query.
										connDB->Execute(QueryStr, NULL, 1);
										// If successfully executed, clear the screen.
										system("cls");
										// Show a success message.
										cout << "Updated record successfully.  Changed email from " << empEmail << " to " << enteredString << "." << endl << endl
											<< "Press any key to return to the Update Menu...";
										// Wait for a key stroke.
										_getch();
									}
									catch (const _com_error &e)
									{
										// If record could not be updated, show the user the error that
										// was returned by the database.

										// Clear the screen.
										system("cls");
										// Show the error.
										cout << " Database Error: " << endl;
										// Put the error in a character array.
										error(e, ErrStr);
										// Print the error char array to the console.
										printf(ErrStr);
										cout << endl << endl << "Press any key to return to the Main Menu...";
										// End updating.
										updating = false;
										// Wait for a key stroke.
										_getch();
									}
									break;
								case 'I': // Phone
										  // Set boolean to true for user input validation.
									choosing = true;
									while (choosing)
									{
										// Loop until valid input has been entered.
										// Clear the screen.
										system("cls");
										// Prompt the user to enter a new value.
										cout << "Updating Record -- " << empName << endl
											<< "Current Phone Number: " << empFName << endl
											<< "Enter New Phone Number (xxx-xxx-xxxx): ";
										// Reset cin position and clear error flags.
										cin.seekg(0, ios::end);
										cin.clear();

										// Get user input.
										getline(cin, enteredString);

										// Test for the correct size of string (because of database column maximums).
										if (!ValidPhone(enteredString) && enteredString != "") {
											// If not a valid length, return an error and make the user try again.
											// Clear the screen.
											system("cls");
											// Error message.
											cout << "Phone number must contain 10 digits or be left blank." << endl << endl
												<< "Press any key to try again...";
											// Wait for key stroke.
											_getch();
											// Start loop at next iteration.
											continue;
										}
										// If valid input, set choosing flag to false.
										choosing = false;
										// Set phone number to formatted version.
										enteredString = FormatPhone(enteredString);
									}
									try
									{
										// Once a valid input has been entered, attempt to update the attribute with
										// the entered string.

										// Set up the query string with parameters.
										sprintf_s(QueryStr, 500, "Update [Employee] SET Phone = '%s' WHERE EmployeeID = %i", enteredString.c_str(), enteredID);
										// Execute the query.
										connDB->Execute(QueryStr, NULL, 1);
										// If successfully executed, clear the screen.
										system("cls");
										// Show a success message.
										cout << "Updated record successfully.  Changed phone number from " << empPhone << " to " << enteredString << "." << endl << endl
											<< "Press any key to return to the Update Menu...";
										// Wait for a key stroke.
										_getch();
									}
									catch (const _com_error &e)
									{
										// If record could not be updated, show the user the error that
										// was returned by the database.

										// Clear the screen.
										system("cls");
										// Show the error.
										cout << " Database Error: " << endl;
										// Put the error in a character array.
										error(e, ErrStr);
										// Print the error char array to the console.
										printf(ErrStr);
										cout << endl << endl << "Press any key to return to the Main Menu...";
										// End updating.
										updating = false;
										// Wait for a key stroke.
										_getch();
									}
									break;
								case 'J': // Member Year
										  // Set boolean to true for user input validation.
									choosing = true;
									while (choosing)
									{
										// Loop until valid input has been entered.
										// Clear the screen.
										system("cls");
										// Prompt the user to enter a new value.
										cout << "Updating Record -- " << empName << endl
											<< "Current Member Year: " << empMemberYear << endl
											<< "Enter New Member Year: ";
										// Reset cin position and clear error flags.
										cin.seekg(0, ios::end);
										cin.clear();

										// Get user input.
										getline(cin, enteredString);

										// Test for the correct size of string (because of database column maximums).
										if (!ValidYear(enteredString)) {
											// If not a valid year, return an error and make the user try again.
											// Clear the screen.
											system("cls");
											// Error message.
											cout << "Member year is required and must be a valid year between 0-9999 inclusive." << endl << endl
												<< "Press any key to try again...";
											// Wait for key stroke.
											_getch();
											// Start loop at next iteration.
											continue;
										}
										// If valid input, set choosing flag to false.
										choosing = false;
										// Set int version of year.
										enteredYear = YearToInt(enteredString);
									}
									try
									{
										// Once a valid input has been entered, attempt to update the attribute with
										// the entered string.

										// Set up the query string with parameters.
										sprintf_s(QueryStr, 500, "Update [Employee] SET MemberYear = %i WHERE EmployeeID = %i", enteredYear, enteredID);
										// Execute the query.
										connDB->Execute(QueryStr, NULL, 1);
										// If successfully executed, clear the screen.
										system("cls");
										// Show a success message.
										cout << "Updated record successfully.  Changed member year from " << empMemberYear << " to " << enteredYear << "." << endl << endl
											<< "Press any key to return to the Update Menu...";
										// Wait for a key stroke.
										_getch();
									}
									catch (const _com_error &e)
									{
										// If record could not be updated, show the user the error that
										// was returned by the database.

										// Clear the screen.
										system("cls");
										// Show the error.
										cout << " Database Error: " << endl;
										// Put the error in a character array.
										error(e, ErrStr);
										// Print the error char array to the console.
										printf(ErrStr);
										cout << endl << endl << "Press any key to return to the Main Menu...";
										// End updating.
										updating = false;
										// Wait for a key stroke.
										_getch();
									}
									break;
								case 'K': // Return to Main Menu
									updating = false;
									break;
								}
							}
						}
						else
						{
							// If no rows were returned, display an error and prompt the user to hit any key
							// to return to the main menu.

							// Clear the screen.
							system("cls");
							// Display the error and prompt.
							cout << "No records were found for this employee ID." << endl << endl
								<< "Press any key to return to Main Menu...";
							// Wait for a key stroke.
							_getch();
							// Set the updating flag to false to exit the loop and return to the main menu.
							updating = false;
						}
					}
				}
			}
			else
			{
				// If no rows were returned, display an error and prompt the user to hit any key
				// to return to the main menu.

				// Display the error and prompt.
				cout << "No records found!" << endl
					<< endl << "Press any key to return to the Main Menu...";
				// Wait for a key stroke.
				_getch();
				// Clear the screen again.
				system("cls");
			}
			break;
		case '6': // Delete a record.
				  // Clear the screen.
			system("cls");
			// Create a query string to display all employee IDs and names.
			sprintf_s(QueryStr, 500, "SELECT EmployeeID, CONCAT(FirstName, ' ', LastName) \"FullName\" FROM [Employee]");
			// Execute the command with the string to retrieve all employee IDs and names.
			empRec = connDB->Execute(QueryStr, NULL, 1);
			// Test to see if records were returned.
			if (!(empRec->EndOfFile))
			{
				// If records were returned, process them and display their info.

				// Clear only the badbit to enter the loop.
				cin.clear(ios::badbit);

				while (!cin.good())
				{
					// Loop until the user has entered a valid integer.

					// Set the counter to indicate if first loop.
					ctr = 0;
					// Move to the first record in the set.
					empRec->MoveFirst();
					// Loop through every record and display its info.
					while (!(empRec->EndOfFile))
					{
						if (ctr == 0)
						{
							// If this is he first iteration of the loop, display the title and column headers.

							// Clear the screen.
							system("cls");

							// Display the title.
							cout << "Delete An Employee Record" << endl;
							// Display the column headers.
							cout << endl << left << " " << setw(15) << "Employee ID" << setw(30) << "Name" << endl;
							// Print a horizontal rule.
							cout << "---------------------------------------------------------------------------------------------------------------------\n";
						}
						// Increment the loop so column headers won't be printed again.
						ctr++;
						// Get the employee ID.
						vtValue = empRec->Fields->GetItem("EmployeeID")->GetValue();
						empID = string((char*)_bstr_t(vtValue));
						// Get the full name.
						vtValue = empRec->Fields->GetItem("FullName")->GetValue();
						empName = string((char*)_bstr_t(vtValue));
						// Print the row.
						cout << left << " " << setw(15) << empID << setw(30) << empName << "\n";
						//Move the pointer for the set to the next record.
						empRec->MoveNext();
					}
					// Once all records have been displayed, prompt the user for an ID that matches the row
					// they would like to delete.
					cout << endl << "Enter the employee ID to delete that record (requires confirmation): ";
					// Clear cin error flags.
					cin.clear();
					// Ignore up to 256 characters or up to the newline character.
					cin.ignore(256, '\n');
					// Get user input.
					cin >> enteredID;
					// Test to see if there are error flags (not a valid integer).
					if (!cin.good())
					{
						// If error flags occur, set the badbit flag only.
						cin.clear(ios::badbit);
						// Ignore up to 256 characters or up to the newline character.
						cin.ignore(256, '\n');
						// Clear the screen.
						system("cls");
						// Display an error and prompt the user to try again.
						cout << "Invalid ID. Employee IDs must be whole numbers." << endl << endl
							<< "Press any key to try again...";
						// Wait for a key stroke.
						_getch();
						// Continue to the top of the loop for its next iteration.
						continue;
					}
					// Once a valid integer is entered, create a query string to get all the employee
					// information for the employee the user has selected to delete.
					sprintf_s(QueryStr, 500, "SELECT * FROM [Employee] WHERE EmployeeID = %i", enteredID);
					// Return that record to the record set.
					empRec = connDB->Execute(QueryStr, NULL, 1);
					// Test to see if the record was found.
					if (!(empRec->EndOfFile))
					{
						// If the record is found, ask the user if they want to delete it (confirm).

						// Create a string to hold the user's answer.
						string yn = "";
						// Loop until the user has selected a valid response.
						while (yn != "y" && yn != "Y" && yn != "n" && yn != "N")
						{
							// Clear the screen.
							system("cls");
							// Ask the user if they are sure they want to delete the record.
							cout << "Are you sure you want to delete the record where Employee ID is " << enteredID << "? (Y or N): ";
							// Reset the cin position and clear any error flags.
							cin.seekg(0, ios::end);
							cin.clear();
							// Get user input.
							getline(cin, yn);
						}
						// If the user selects NO (don't delete), break from the switch, so that the delete
						// query is never reached.
						if (yn == "n" || yn == "N")
							break;
					}
					else
					{
						// If no record is found, display an error message and prompt for the user to 
						// hit any key to return to the main menu.

						// Clear the screen.
						system("cls");
						// Display the error and prompt.
						cout << "No records were found from that Employee ID (" << enteredID << ")." << endl << endl
							<< "Press any key to return to the Main Menu...";
						// Wait for a key stroke.
						_getch();
						// Break from the switch statement.
						break;
					}
					// If the user selects to delete the record, create a query string to delete
					// a record based on the entered employee ID.
					sprintf_s(QueryStr, 500, "DELETE FROM [Employee] WHERE EmployeeID = %i", enteredID);
					// Execute this query to delete the record.
					connDB->Execute(QueryStr, NULL, 1);

					// Print a success message and prompt the user to hit any key to return to the main menu.
					cout << "The employee was successfully deleted." << endl << endl
						<< "Press any key to return to the Main Menu...";
					// Wait for a key stroke.
					_getch();
				}
			}
			else
			{
				// If no records are found, return an error and promp the user to hit any key to return
				// to the main menu.

				// Display the error and prompt.
				cout << "No records found!" << endl
					<< endl << "Press any key to return to the Main Menu...";
				// Wait for a key stroke.
				_getch();
				// Clear the screen again.
				system("cls");
			}
			break;
		case '7': // Display all employee records.
				  // Clear the screen.
			system("cls");
			// Display the title for this section.
			cout << "All Employee Records" << endl << endl;
			// Create a query string to retrieve all employees.
			sprintf_s(QueryStr, 500, "SELECT EmployeeID, CONCAT(FirstName, ' ', LastName) \"FullName\", MemberYear, Association FROM [Employee]");
			// Execute the command with the query string and return the data into a record set.
			empRec = connDB->Execute(QueryStr, NULL, 1);
			// Display the column headers.
			cout << endl << left << " " << setw(15) << "Employee ID" << setw(30) << "Name" << setw(17) << "Member Year" << setw(40) << "Association" << endl;
			// Print a horizontal rule.
			cout << "---------------------------------------------------------------------------------------------------------------------\n";
			// Test to see if records were returned.
			if (!(empRec->EndOfFile))
			{
				// If records exist in the record set, process each of them by retrieving the attributes
				// from the row and displaying them on the screen.

				// Move the record pointer to the first record in the set.
				empRec->MoveFirst();

				// Loop through all records and print their attributes.
				while (!(empRec->EndOfFile))
				{
					// Get the employee ID.
					vtValue = empRec->Fields->GetItem("EmployeeID")->GetValue();
					empID = string((char*)_bstr_t(vtValue));
					// Get the full name.
					vtValue = empRec->Fields->GetItem("FullName")->GetValue();
					empName = string((char*)_bstr_t(vtValue));
					// Get the member year.
					vtValue = empRec->Fields->GetItem("MemberYear")->GetValue();
					empMemberYear = string((char*)_bstr_t(vtValue));
					// Get the assocation.
					vtValue = empRec->Fields->GetItem("Association")->GetValue();
					empAssocation = string((char*)_bstr_t(vtValue));
					// Display the attribute row for the current employee.
					cout << endl << left << " " << setw(15) << empID << setw(30) << empName << setw(17) << empMemberYear << setw(40) << empAssocation << endl;
					// Move the employee record pointer to the next record in the set.
					empRec->MoveNext();
				}
				// Once all records have been displayed, promt the user to 
				// hit any key to return to the main menu.
				cout << endl << "Press any key to return to the Main Menu...";
				// Wait for a key stroke.
				_getch();
				// Clear the screen.
				system("cls");
			}
			else
			{
				// If not records were found, display an error and prompt the user to
				// hit any key to return to the main menu.
				cout << "No records found!" << endl
					<< endl << "Press any key to return to the Main Menu...";
				_getch();
				system("cls");
			}
			break;
		case '8':
			// Clean up database objects before ending program.
			CoUninitialize();
			// Exit the application.
			exit(0);
			break;
		default: // Other input.
				 // If anything other than a 1-8 is entered, return an error and prompt the user to hit
				 // any key to return to the main menu.

				 // Clear the screen.
			system("cls");
			// Display the error and prompt.
			cout << "Invalid selection. Selection must be a number between 1-8 inclusive." << endl
				<< endl << "Press any key to return to the main menu...";
			// Wait for a key stroke.
			_getch();
			// Clear the screen again.
			system("cls");
			// End the switch statement.
			break;
		}
	}
	catch (const _com_error &e)
	{
		// This catch statement is to retreive any exceptions that fell through.  Nothing should
		// reach this point, because the code has been tested, but if something falls through, this
		// block will return a meaningfull error message to the user.

		// Clear the screen.
		system("cls");
		// Diplay the error.
		cout << " Database Error: " << endl;
		error(e, ErrStr);
		printf(ErrStr);
		// Prompt the user to hit any key to return to the main menu.
		cout << endl << endl << "Press any key to return to the Main Menu...";
		// Wait for a key stroke.
		_getch();
		// Clear the screen again.
		system("cls");
	}
	// If the function completes, clean up database objects.
	CoUninitialize();
}

bool ValidState(string p_state)
{
	// This method tests a string to see if it is a valid state input.

	// If the size of the string is not two characters long, it's not the right size. Return false.
	if (p_state.size() != 2)
		return false;
	// Create booleans to test the two characters
	bool firstLetterValid = false;
	bool secondLetterValid = false;
	// If the first character is a lowercase letter, set the first letter to valid.
	if (p_state[0] >= 'a' && p_state[0] <= 'z')
		firstLetterValid = true;
	// If the first character is an uppercase letter, set the first letter to valid.
	else if (p_state[0] >= 'A' && p_state[0] <= 'Z')
		firstLetterValid = true;
	// If the second character is a lowercase letter, set the second letter to valid.
	if (p_state[1] >= 'a' && p_state[1] <= 'z')
		secondLetterValid = true;
	// If the second character is an uppercase letter, set the second letter to valid.
	else if (p_state[1] >= 'A' && p_state[1] <= 'Z')
		secondLetterValid = true;
	// Return true if both letters are valid; otherwise, return false.
	return (firstLetterValid && secondLetterValid);
}

bool ValidZip(string p_zip)
{
	// This method returns true only if the string is a valid 5-digit zip code.

	// Check to make sure the string is five characters in length.
	if (p_zip.size() != 5)
		return false;
	// Loop through the five characters and make sure they are valid digits.
	for (int i = 0; i < 5; i++)
	{
		if (p_zip[i] < '0' || p_zip[i] > '9')
			return false;
	}
	// First number cannot be a 0.
	if (p_zip[0] == '0')
		return false;
	// Otherwise it's a valid zip code.
	return true;
}

bool ValidEmail(string p_email)
{
	// This method returns true if a valid email format was entered.

	// Create bools that test for the necessary parts of an email.
	bool usernameLetterFound, snailFound, companyAlphaNumFound, periodFound;
	// Set the flags to false, because they most all be switched to true to be a valid email address.
	usernameLetterFound = snailFound = companyAlphaNumFound = periodFound = false;

	// Loop through the characters of the email string to test them for their necessary parts.
	for (int i = 0; i < (int)p_email.size(); i++)
	{
		// This looks backwards, because all prior conditions must be met before a the next
		// flag can be switched to true.

		// Lastly, the period found condition will be tested, which tests to make sure nothing after the
		// final period is anything other than a letter.
		if (periodFound)
		{
			if (!ValidLetter(p_email[i]) && p_email[i] != '.')
			{
				// If any non-letter, non-period is found, return false.
				return false;
			}
		}
		// Before the periodFound is set, test for the period.  The letters or number of the
		// company name must be found before testing for the period.
		if (companyAlphaNumFound)
			if (p_email[i] == '.')
			{
				if ((int)p_email.size() < i + 2)
					return false;
				else if (!ValidLetter(p_email[i + 1]))
					return false;
				else
					periodFound = true;
			}
		// Before the company can be search, the Snail symbol is needed.
		if (snailFound)
			if (ValidAlphaNumeric(p_email[i]))
				companyAlphaNumFound = true;
		// Before the snail symbol, the letters in the username must be found.
		if (usernameLetterFound)
			if (p_email[i] == '@')
				snailFound = true;
		// First test to see if there is a username letter.
		if (ValidLetter(p_email[i]))
			usernameLetterFound = true;

	}
	// If the email doesn't end with a letter, return false.
	if (!ValidLetter(p_email[(int)p_email.size() - 1]))
		return false;
	// If the email doesn't start with a letter, return false.
	if (!ValidLetter(p_email[0]))
		return false;
	// If all conditions are met, return true.
	return usernameLetterFound && snailFound && companyAlphaNumFound && periodFound;
}


bool ValidPhone(string p_phone)
{
	// This method returns true if the user has entered a number with 10 digits in it
	// and is of an acceptable length. The lack of a strict format is due to the fact
	// that I will extract the digits and format the number as long as it has 10 digits.

	// If the phone number is too long, return false.
	if (p_phone.length() > 15)
		return false;

	// Create a counter for the number of digits in the string.
	int ctr = 0;
	for (int i = 0; i < (int)p_phone.size(); i++)
	{
		// Loop through the characters and increment the counter each time a number is found.
		if (ValidNumber(p_phone[i]))
			ctr++;
	}
	// If the phone number does not contain 10 numbers, it is not valid.
	if (ctr != 10)
		return false;
	else
		return true;
}

bool ValidYear(string p_year)
{
	// This method only returns true if a 4 (or less) digit year has been entered.

	// Check for valid length of the string.
	if ((int)p_year.size() > 4 || (int)p_year.size() == 0)
		return false;
	// Loop through each character to see if any are not numbers.
	for (int i = 0; i < (int)p_year.size(); i++)
	{
		// If a non-digit is found, return false.
		if (!ValidNumber(p_year[i]))
			return false;
	}
	// If year was entered and all characters are valid digits, return true.
	return true;
}

int YearToInt(string p_year)
{
	// This method is used to return an already verified year string as an integer, so it can
	// be added to the database column for year.

	// Create an integer to add years.
	int yearAsInt = 0;
	// Loop through each character of the year and add it's proper power of 10 to the yearAsInt integer.
	for (int i = (int)p_year.size() - 1, z = 0; i >= 0; i--, z++)
	{
		// Create a power variable which will be increased depending on the index.
		int power = 1;
		for (int j = 0; j < i; j++)
		{
			// For each successive index, multiply power by another multiple of 10.
			power *= 10;
		}
		// Add the character multiplied by it's proper power of 10 to the yearAsInt integer.
		yearAsInt += (int)(p_year[z] - '0') * power;
	}
	// Return the final yearAsInt integer.
	return yearAsInt;
}

bool ValidLetter(char ch)
{
	// This method only returns true if the character entered is a valid letter.

	// Create a boolean for validity of the letter.
	bool valid = false;
	// If the character is a lowercase letter, change valid to true.
	if (ch >= 'a' && ch <= 'z')
		valid = true;
	// If the character is an uppercase letter, change valid to true.
	if (ch >= 'A' && ch <= 'Z')
		valid = true;
	// Return the validity of this character as a letter.
	return valid;
}

bool ValidNumber(char ch)
{
	// This method only returns true if the character is a valid number.

	return (ch >= '0' && ch <= '9');
}

bool ValidAlphaNumeric(char ch)
{
	// This method only returns true if the character is a valid letter or number.

	return (ValidLetter(ch) || ValidNumber(ch));
}

string FormatPhone(string p_phone) {
	// This method returns the phone number formatted the correct way for this database (xxx-xxx-xxxx).

	// Create an array of characters to hold each of the numbers in the phone number.
	char digits[10];
	// Create a counter to keep track of the digits index.
	int ctr = 0;
	// Loop through the phone number and add the numbers from it to the digits array.
	for (int i = 0; i < (int)p_phone.size(); i++)
	{
		// If the character is a number add it to the next digits array element.
		if (ValidNumber(p_phone[i]))
			digits[ctr++] = p_phone[i];
	}
	// Create a string for the formatted version of the phone number.
	string formattedPhone = "";
	// Add each character to the new string.
	formattedPhone += digits[0];
	formattedPhone += digits[1];
	formattedPhone += digits[2];
	formattedPhone += "-";
	formattedPhone += digits[3];
	formattedPhone += digits[4];
	formattedPhone += digits[5];
	formattedPhone += "-";
	formattedPhone += digits[6];
	formattedPhone += digits[7];
	formattedPhone += digits[8];
	formattedPhone += digits[9];
	// Return the newly formatted phone string.
	return formattedPhone;
}