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

// Includes may only occur once.
#pragma once

// Include necessary header files/libraries
#include <iostream>
#include <iomanip>
#include <string>
#include <conio.h>
#include <Windows.h>

// Use common namespace.
using namespace std;

// Import the ADO DLL.
#import "C:\Program Files\Common Files\System\ado\msado15.dll" \
		rename("EOF","EndOfFile") no_implementation

class Connection
{
	// This class holds all the main functionality of the database program.  All methods and
	// objects needed for this program are contained within this class.
public:
	// Constructor.
	Connection(void);
	// Destructor.
	~Connection(void);
	// Error info comes from this method.
	void error(_com_error e, char* displayErr);
	// This method takes a menu choice from the main menu on main.cpp and 
	// handles the appropriate functionality accordingly.
	void conn(char choice);
};