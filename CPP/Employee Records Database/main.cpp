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

// Include iostream to prevent ambigious cout.
#include <iostream>
// Include header file.
#include "connection.h"

// Import as "implementation only" - for ADO.
#import "C:\Program Files\Common Files\System\ado\msado15.dll" \
		rename("EOF","EndOfFile") implementation_only

// Use the common namespace.
using namespace std;

int main() {
	// This function is called first.  It holds only the main menu, and sends the
	// user choice to the connection class for program functionality.

	// Create a connection object.
	Connection c;
	// Create a variable that will hold the user's choice.
	char choice = 0;
	// Create a variable that will hold all of the user's input, even invalid input.
	string fullInput = "";
	// Change the font to neon green.
	system("COLOR 0A");
	// Loop the menu until the user selects to exit.
	while (true)
	{
		// Clear the screen.
		system("cls");
		// Display the main menu.
		cout << "Employee Database Menu\n";
		cout << "\t1) Search Records By Employee ID\n"
			<< "\t2) Search Records By Member Year\n"
			<< "\t3) Search Records By Association\n"
			<< "\t4) Insert a New Employee Record\n"
			<< "\t5) Update an Existing Employee Record\n"
			<< "\t6) Delete an Employee Record\n"
			<< "\t7) Retrieve All Employee Records\n"
			<< "\t8) Exit Program\n"
			<< "Your Selection: ";
		// Get input from the user.
		cin >> fullInput;
		// Get the first character from the input.
		choice = fullInput[0];
		// Send the user choice to the connection class object for processing.
		c.conn(choice);
	}
	// End the program.
	return 0;
}