// Jeremiah Stones
// Advanced C++ 
// Pointers Lab

// Program Specification:
// This program uses pointers and dynamic allocation of memory to create Liquid objects as needed as the program
// is running in order to view, alter, add, or remove liquids from data stored in a file.  At the beginning of the 
// program, if the the data file exists, the stored liquid information is loaded into a linked list. The user may
// then choose to view all of the liquid information at once, alter a specific liquid's details, add a new liquid,
// or remove a liquid.  The menus created during run-time are generated from the linked list, not the file, and as
// information is changed at run-time, menu options will update accordingly.  Any changes that are made to the 
// original information will be stored back into the file when the user properly exits the program.  If there is no
// file that exists, the program will create one.

// include header for all liquid object operations
#include "Liquid.h"

int main()
{
	// program variables
	bool running = true;
	bool validChoice = true;
	string input = "";
	// object to access liquid operations
	LiquidOps lOps;
	// load liquids from file
	lOps.LoadLiquids();
	// my sister insisted on being a part of my project
	PlaySound("welcome.wav", NULL, SND_FILENAME | SND_ASYNC);
	cout << "Welcome to the Loco Labs Liquid Information Center:"
		<< endl << "A place where you can keep all your splashy things." << endl << endl;
	while (running)
	{
		// this is the main loop for the program that keeps showing the user the main menu until the choose to exit
		cout << "Please choose an option from the menu below:"
			<< endl << "\t1) View Liquid Info"
			<< endl << "\t2) Alter Liquid Info"
			<< endl << "\t3) Add a Liquid"
			<< endl << "\t4) Remove a Liquid"
			<< endl << "\t5) Exit and Save Changes"
			<< endl << "Your selection: ";
		getline(cin, input);
		if (!input.length() == 1 || input[0] < '1' || input[0] > '5')
		{
			// continue prompting user for a choice until it's a valid one (1-5)
			system("cls");
			cout << "Invalid choice, try again." << endl << endl;
			continue;
		}
		// test the number the user entered
		switch (input[0] - 48)
		{
		// view all of the liquids at once
		case 1:
			system("cls");
			lOps.ViewLiquids();
			cout << endl << "Press any key to return to main menu." << endl;
			_getch();
			system("cls");
			break;
		// change an individual liquid's information
		case 2:
			system("cls");
			lOps.AlterLiquid();
			system("cls");
			break;
		// add a new liquid
		case 3:
			system("cls");
			lOps.AddLiquid();
			break;
		// remove a liquid entirely from the data
		case 4:
			system("cls");
			lOps.RemoveLiquid();
			system("cls");
			break;
		// user selects to exit the program, toggle the flag to false to exit
		case 5:
			running = false;
			break;
		}
	}	
	// main loop has exited
	
	// liquid operataion to save all of the changes stored in the link list back to the file
	lOps.WriteLiquidsToFile();
	// timer variables
	clock_t start = GetTickCount();
	double duration = 0;
	while (duration < 1)
	{
		// user a timer to display the goodbye message for just one second
		duration = (GetTickCount() - start) / (double)CLOCKS_PER_SEC;
	}
	// cleaning up the program by removing all dynamically allocated objects in the linked list
	lOps.KillLiquids();
	return 0;
}