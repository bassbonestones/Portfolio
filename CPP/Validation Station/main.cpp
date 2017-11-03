/*
Jeremiah Stones
Advanced C++
Rodney Ortigo

05/16/2017

Program Specification:
This program allows the user to select from a list of different data types in order to test an
input for a validity.  The user is prompted to enter an example of that data type.  If they enter
a correct example, they get an appropriate message, displaying their value back to them.  If they 
incorrectly input the data type, they get an error message and a hit about entering in a correct 
format of that data type.
*/

#include "validation.h"

using namespace std;

int main()
{
	// some variables
	int ctr;
	bool inputFinished, inputValid;
	char ch;
	string input;

	// for chosen entry prompt
	string entries[] = { "a valid whole number (examples: 10, 0, 15023): ", "a valid real number (examples: -14.522, 3, 117.2): ",
					   "valid Roman alphabet characters (examples: a, Mack, DxEkOQ): ", "a valid phone number (format: (555)555-5555): ",
					   "a valid social security number (format: 111-22-3333): ", "a valid email address (format: local-part@domain.ext): ",
					   "a valid date (format: MM/DD/YYYY): ", "a valid military time (format - HH:MM:SS): " };
	string entry;
	// for chosen input size
	int inputSizes[] = { 25, 25, 25, 13, 11, 40, 10, 8 };
	int inputSize;
	// for chosen array to hold input
	char *type;

	// for chosen function to validate
	bool(*funcs[])(char *arr) = { ValidWhole, ValidReal, ValidChar, ValidPhone, ValidSSN, ValidEmail, ValidDate, ValidTime };
	bool(*chosenFunc[1])(char *arr);
	// for chosen valid response type
	string validTypes[] = { "whole number", "real number", "character(s)", "phone number", "social security number", "email", "date", "time" };
	string validType;
	

	while (true) // can only exit program via the main menu option 9
	{
		// main menu -- outer loop of program
		inputValid = false;
		bool noExitFlag = false;
		system("cls");
		cout << "Please select a value to validate: " << endl
			<< endl
			<< "\t1) Whole Number" << endl
			<< "\t2) Real Number" << endl
			<< "\t3) Characters" << endl
			<< "\t4) Phone Number" << endl
			<< "\t5) Social Security Number" << endl
			<< "\t6) Email Address" << endl
			<< "\t7) Date (MM/DD/YYYY)" << endl
			<< "\t8) Time (HH:MM:SS)" << endl
			<< "\t9) Exit Program" << endl
			<< endl
			<< "Your Selection: ";
		while (!inputValid)
		{
			// loop until the user has entered a valid input for menu selection
			ch = _getch();
			if (IsCharBetween(ch, '1', '9'))
			{
				int choice = (int)ch - 48;
				if (choice == 9)
				{
					char c;
					cout << endl << endl << "Are you sure you want to exit (y/n): ";
					do
					{
						c = _getch();
						if (c == 'y' || c == 'Y')
							exit(0);
						else if (c == 'n' || c == 'N')
						{
							inputValid = true;
							noExitFlag = true;
						}
						else
							inputValid = false;
					} while (!inputValid);
				} 
				else
				{
					// if valid input, set all vars for user input screen and validation
					entry = entries[choice - 1];
					inputSize = inputSizes[choice - 1];
					chosenFunc[0] = funcs[choice - 1];
					validType = validTypes[choice - 1];
					inputValid = true;
				}
			}
		}
		// return to menu if user hit exit and then chose not to exit
		if (noExitFlag)
			continue;
		// reset valid input and clear screen
		inputValid = false;
		system("cls");
		do
		{
			// loop until a valid input for the data type has been entered
			type = new char[41];
			input = "";
			ctr = 0;
			inputFinished = false;
			cout << "Enter " << entry;
			while (!inputFinished)
			{
				// loops until user hits enter and takes in user input
				// with valid visible characters
				ch = _getch();
				if (IsDelete(type, ch, ctr))
					continue;
				if (IsVisibleChar(ch) && ctr < inputSize)
				{
					UpdateCharArray(type, ch, ctr);
				}
				if (ch == '\r')
				{
					inputFinished = true;
				}
			}
			inputValid = chosenFunc[0](type);
			for (int i = 0; i < ctr; i++)
			{
				input += type[i];
			}
			delete type;
		} while (!inputValid);
		// clear screen and display valid input 
		system("cls");
		cout << "Valid " << validType << " entered: " << input << endl << endl;
		
		cout << "Press any key to return to Main Menu.";
		_getch();
	}
	return 0;
}