#include "Operations.h"


int ShowMainMenu()
{
	// method to show the initial menu (top level) of the program
	system("cls");
	// create array of menu options for dynamic menu
	string mainMenu[] = {
		"\t1) Create a New Vending Machine",
		"\t2) Load an Existing Machine",
		"\t3) Set Pricing on an Existing Machine",
		"\t4) Put an existing Machine into Service",
		"\t5) Generate an Inventory Report",
		"\t6) Toggle Background Music On/Off",
		"\t7) Exit Program" };
		HideCursor(true);
		system("cls");
		int option = 0;
		// create title and subtitle(s)
		string title = "Welcome to Vending Machine Central";
		string title2 = "Select an option from the following menu: ";

		//variables for current cell
		char ch;
		int x = 5; int y = 1;

		// write menu info to screen using custom functions
		gotoXY(&x, &y, title, true, false);
		y = 4;
		gotoXY(&x, &y, title2, true, false);
		x = 7; y = 5;
		gotoXY(&x, &y, mainMenu[option++], true, true); y++;
		gotoXY(&x, &y, mainMenu[option++], true, false); y++;
		gotoXY(&x, &y, mainMenu[option++], true, false); y++;
		gotoXY(&x, &y, mainMenu[option++], true, false); y++;
		gotoXY(&x, &y, mainMenu[option++], true, false); y++;
		gotoXY(&x, &y, mainMenu[option++], true, false); y++;
		gotoXY(&x, &y, mainMenu[option], true, false); y -= 6; option -= 6;
		gotoXY(0, 0);
		do
		{
			// loop lets user go up and down on the menu by selecting the row
			// ENTER chooses the option.
			ch = _getch();
			bool insert = true;
			if (ch == 'H') // up
			{
				if (y > 5) {
					gotoXY(&x, &y, mainMenu[option--], insert, false); y--;
					gotoXY(&x, &y, mainMenu[option], insert, true);
				}
				else {
					gotoXY(&x, &y, mainMenu[option], insert, false); option += 6; y += 6;
					gotoXY(&x, &y, mainMenu[option], insert, true);
				}
			}
			else if (ch == 'P') // down
			{
				if (y < 11) {
					gotoXY(&x, &y, mainMenu[option], insert, false); y++; option++;
					gotoXY(&x, &y, mainMenu[option], insert, true);
				}
				else {
					gotoXY(&x, &y, mainMenu[option], insert, false); option -= 6; y -= 6;
					gotoXY(&x, &y, mainMenu[option], insert, true);
				}
			}
			else if (ValidCharRanged(ch,'1','7'))
			{
				// if user presses the number instead of using ENTER on the highlighted row
				option = int(ch) - 49; // 49 instead of 48 because option is an index
				break;
			}
			gotoXY(0, 0);
		} while (ch != '\r');
		// return value chosen (used in switch-case decision structure of main())
		return option + 1;
}

void CreateMachine(VendingMachine *& top)
{
	// method for creating and assigning a new vending machine to the linked list
	// and eventually the file

	bool creating = true;
	int selection;
	// temp machine for adding to linked list
	VendingMachine * vendTemp;
	while (creating)
	{
		// create dynamic menu
		system("cls");
		string mainMenu[] = {
			"\t1) Fountain Drink",
			"\t2) Prepackaged Snack",
			"\t3) Bottled Beverage",
			"\t4) Return to Main Menu",
			"Your Selection: " };
		HideCursor(true);
		system("cls");
		int option = 0;
		string title = "Create Vending Machine Menu";
		string title2 = "Please choose the type of machine you would like to create:";

		//variables for current cell
		char ch;
		int x = 5; int y = 1;

		// write menu 
		gotoXY(&x, &y, title, true, false);
		y = 4;
		gotoXY(&x, &y, title2, true, false);
		x = 7; y = 5;
		gotoXY(&x, &y, mainMenu[option++], true, true); y++;
		gotoXY(&x, &y, mainMenu[option++], true, false); y++;
		gotoXY(&x, &y, mainMenu[option++], true, false); y++;
		gotoXY(&x, &y, mainMenu[option], true, false); y -= 3; option -= 3;
		gotoXY(0, 0);
		do
		{
			// loop lets user go up and down on the menu by selecting the row
			// ENTER chooses the option.
			ch = _getch();
			bool insert = true;
			if (ch == 'H') // up
			{
				if (y > 5) {
					gotoXY(&x, &y, mainMenu[option--], insert, false); y--;
					gotoXY(&x, &y, mainMenu[option], insert, true);
				}
				else {
					gotoXY(&x, &y, mainMenu[option], insert, false); option += 3; y += 3;
					gotoXY(&x, &y, mainMenu[option], insert, true);
				}
			}
			else if (ch == 'P') // down
			{
				if (y < 8) {
					gotoXY(&x, &y, mainMenu[option], insert, false); y++; option++;
					gotoXY(&x, &y, mainMenu[option], insert, true);
				}
				else {
					gotoXY(&x, &y, mainMenu[option], insert, false); option -= 3; y -= 3;
					gotoXY(&x, &y, mainMenu[option], insert, true);
				}
			}
			else if (ValidCharRanged(ch, '1', '4'))
			{
				// if user presses the number instead of using ENTER on the highlighted row
				option = int(ch) - 49; // 49 instead of 48 because option is an index
				break;
			}
			gotoXY(0, 0);
		} while (ch != '\r');
		// depending on selection create appropriate base class and add to temp variable

		HideCursor(false);
		selection = option + 1;
		switch (selection)
		{
		case 1:
			vendTemp = new VendingFountain;
			break;
		case 2:
			vendTemp = new VendingPrepackSnack;
			break;
		case 3:
			vendTemp = new VendingBottledBev;
			break;
		default:
			//create random vending machine so all cases instantiate vendTemp
			vendTemp = new VendingBottledBev;
			creating = false;
			break;
		}
		if (creating)
		{
			// if option chosen, set machine name and add to linked list
			string newName;
			while (true)
			{
				system("cls");
				cout << "Type a name for the new machine and press enter: ";
				getline(cin, newName);
				if (!ValidName(newName))
					continue;
				break;
			}
			// add name
			vendTemp->setName(newName);
			// add temp object to linked list
			vendTemp->setNextPtr(top);
			top = vendTemp;
			// show success message
			cout << "\n\nYour new machine, " << newName << ", has been created." << endl << endl
				<< "Before putting this machine into service, it must be loaded, and all" << endl
				<< "prices must be set." << endl << endl
				<< "Press any key to return to the creation menu." << endl;
			_getch();
		}
		// if user did not select a machine, delete temp object
		else
			delete vendTemp;
	}
}

void LoadMachine(VendingMachine *& top)
{
	// function to Load a machine with products (and cups for fountain)
	bool loading = true;
	string selection;
	while (loading)
	{
		while (true)
		{
			// use temp machine pointer to create dynamic menu
			int machineNum = 1;
			system("cls");
			string mainMenu[14];
			VendingMachine * vendTemp = top;
			while (vendTemp != NULL)
			{
				// loop through all linked list items
				if (vendTemp->getName().length() > 0)
				{
					// create an option for each machine
					mainMenu[machineNum - 1] = "\t";
					mainMenu[machineNum - 1] += char(machineNum + 48);
					mainMenu[machineNum - 1] += ") " + vendTemp->getName();
					machineNum++;
				}
				vendTemp = vendTemp->getNextPtr();
			}
			// add a return to main menu option
			mainMenu[machineNum - 1] = "\t";
			mainMenu[machineNum - 1] += char(machineNum + 48);
			mainMenu[machineNum - 1] += ") Return to Main Menu";

			HideCursor(true);
			system("cls");
			int option = 0;
			// create titles
			string title = "Load Vending Machine Menu";
			string title2 = "Please choose the machine you would like to load:";

			//variables for current cell
			char ch;
			int x = 5; int y = 1;
			// write menu to screen
			gotoXY(&x, &y, title, true, false);
			y = 4;
			gotoXY(&x, &y, title2, true, false);
			x = 7; y = 5;
			for (int i = 0; i < machineNum; i++)
			{
				if (i == 0) 
				{
					gotoXY(&x, &y, mainMenu[option++], true, true); y++;
				}
				else
				{
					gotoXY(&x, &y, mainMenu[option++], true, false); y++;
				}
			}
			gotoXY(&x, &y, mainMenu[option], true, false); y -= machineNum; option -= machineNum;
			gotoXY(0, 0);
			do
			{
				// loop lets user go up and down on the menu by selecting the row
				// ENTER chooses the option.
				ch = _getch();
				bool insert = true;
				if (ch == 'H') // up
				{
					if (y > 5) {
						gotoXY(&x, &y, mainMenu[option--], insert, false); y--;
						gotoXY(&x, &y, mainMenu[option], insert, true);
					}
					else {
						gotoXY(&x, &y, mainMenu[option], insert, false); option += machineNum - 1; y += machineNum - 1;
						gotoXY(&x, &y, mainMenu[option], insert, true);
					}
				}
				else if (ch == 'P') // down
				{
					if (y < machineNum + 4) {
						gotoXY(&x, &y, mainMenu[option], insert, false); y++; option++;
						gotoXY(&x, &y, mainMenu[option], insert, true);
					}
					else {
						gotoXY(&x, &y, mainMenu[option], insert, false); option -= machineNum - 1; y -= machineNum - 1;
						gotoXY(&x, &y, mainMenu[option], insert, true);
					}
				}
				else if (ValidCharRanged(ch, '1', char(machineNum+48)))
				{
					// if user presses the number instead of using ENTER on the highlighted row
					option = int(ch) - 49; // 49 instead of 48 because option is an index
					break;
				}
				gotoXY(0, 0);
			} while (ch != '\r');
			HideCursor(false);
			// set choice to user selection
			int choice = option + 1;
			// exit loading if user chooses
			if (choice == machineNum)
				loading = false;
			// otherwise call overloaded function on derived class object
			else
			{
				vendTemp = top;
				for (int i = 0; i < choice - 1; i++)
				{
					vendTemp = vendTemp->getNextPtr();
				}
				vendTemp->Load();
			}
			break;
		}
	}
}

void SetPricing(VendingMachine *& top)
{
	// method to create set price menu on main()

	bool setting = true;
	string selection;
	while (setting)
	{
		while (true)
		{
			// create dynamic menu for setting with temporary machine pointer
			int machineNum = 1;
			system("cls");
			string mainMenu[14];

			VendingMachine * vendTemp = top;
			while (vendTemp != NULL)
			{
				// loop through linked list
				if (vendTemp->getName().length() > 0)
				{
					// create a menu option for each machine
					mainMenu[machineNum - 1] = "\t";
					mainMenu[machineNum - 1] += char(machineNum + 48);
					mainMenu[machineNum - 1] += ") " + vendTemp->getName();
					machineNum++;
				}
				vendTemp = vendTemp->getNextPtr();
			}
			// add an option to return to main menu
			mainMenu[machineNum - 1] = "\t";
			mainMenu[machineNum - 1] += char(machineNum + 48);
			mainMenu[machineNum - 1] += ") Return to Main Menu";

			HideCursor(true);
			system("cls");
			int option = 0;
			string title = "Set Pricing Vending Machine Menu";
			string title2 = "Please choose the machine you would like to set prices for:";

			//variables for current cell
			char ch;
			int x = 5; int y = 1;

			// write menu to screen
			gotoXY(&x, &y, title, true, false);
			y = 4;
			gotoXY(&x, &y, title2, true, false);
			x = 7; y = 5;
			for (int i = 0; i < machineNum; i++)
			{
				if (i == 0)
				{
					gotoXY(&x, &y, mainMenu[option++], true, true); y++;
				}
				else
				{
					gotoXY(&x, &y, mainMenu[option++], true, false); y++;
				}
			}
			gotoXY(&x, &y, mainMenu[option], true, false); y -= machineNum; option -= machineNum;
			gotoXY(0, 0);
			do
			{
				// loop lets user go up and down on the menu by selecting the row
				// ENTER chooses the option.
				ch = _getch();
				bool insert = true;
				if (ch == 'H') // up
				{
					if (y > 5) {
						gotoXY(&x, &y, mainMenu[option--], insert, false); y--;
						gotoXY(&x, &y, mainMenu[option], insert, true);
					}
					else {
						gotoXY(&x, &y, mainMenu[option], insert, false); option += machineNum - 1; y += machineNum - 1;
						gotoXY(&x, &y, mainMenu[option], insert, true);
					}
				}
				else if (ch == 'P') // down
				{
					if (y < machineNum + 4) {
						gotoXY(&x, &y, mainMenu[option], insert, false); y++; option++;
						gotoXY(&x, &y, mainMenu[option], insert, true);
					}
					else {
						gotoXY(&x, &y, mainMenu[option], insert, false); option -= machineNum - 1; y -= machineNum - 1;
						gotoXY(&x, &y, mainMenu[option], insert, true);
					}
				}
				else if (ValidCharRanged(ch, '1', char(machineNum+48)))
				{
					// if user presses the number instead of using ENTER on the highlighted row
					option = int(ch) - 49; // 49 instead of 48 because option is an index
					break;
				}
				gotoXY(0, 0);
			} while (ch != '\r');
			HideCursor(false);
			// set choice to user selection
			int choice = option + 1;
			if (choice == machineNum)
				setting = false;
			else
			{
				// if machine chosen call overloaded function to set prices of its products
				vendTemp = top;
				for (int i = 0; i < choice - 1; i++)
				{
					vendTemp = vendTemp->getNextPtr();
				}
				vendTemp->SetProductPrices();
			}
			break;
		}
	}
}

void StartMachine(VendingMachine *& top)
{
	// method to create a menu for putting a machine into service for testing
	bool starting = true;
	string selection;
	while (starting)
	{
		while (true)
		{
			// use a temp machine pointer to create a dynamic menu
			int machineNum = 1;
			system("cls");
			string mainMenu[14];

			VendingMachine * vendTemp = top;
			while (vendTemp != NULL)
			{
				// loop through linked list
				if (vendTemp->getName().length() > 0)
				{
					// add an optino for each machine
					mainMenu[machineNum - 1] = "\t";
					mainMenu[machineNum - 1] += char(machineNum + 48);
					mainMenu[machineNum - 1] += ") " + vendTemp->getName();
					machineNum++;
				}
				vendTemp = vendTemp->getNextPtr();
			}
			// add an additional option for returning to main menu
			mainMenu[machineNum - 1] = "\t";
			mainMenu[machineNum - 1] += char(machineNum + 48);
			mainMenu[machineNum - 1] += ") Return to Main Menu";

			HideCursor(true);
			system("cls");
			int option = 0;
			string title = "Put Vending Machine Into Service Menu";
			string title2 = "Please choose the machine you would like to test:";

			//variables for current cell
			char ch;
			int x = 5; int y = 1;
			// write menu to screen
			gotoXY(&x, &y, title, true, false);
			y = 4;
			gotoXY(&x, &y, title2, true, false);
			x = 7; y = 5;
			for (int i = 0; i < machineNum; i++)
			{
				if (i == 0)
				{
					gotoXY(&x, &y, mainMenu[option++], true, true); y++;
				}
				else
				{
					gotoXY(&x, &y, mainMenu[option++], true, false); y++;
				}
			}
			gotoXY(&x, &y, mainMenu[option], true, false); y -= machineNum; option -= machineNum;
			gotoXY(0, 0);
			do
			{
				// loop lets user go up and down on the menu by selecting the row
				// ENTER chooses the option.
				ch = _getch();
				bool insert = true;
				if (ch == 'H') // up
				{
					if (y > 5) {
						gotoXY(&x, &y, mainMenu[option--], insert, false); y--;
						gotoXY(&x, &y, mainMenu[option], insert, true);
					}
					else {
						gotoXY(&x, &y, mainMenu[option], insert, false); option += machineNum - 1; y += machineNum - 1;
						gotoXY(&x, &y, mainMenu[option], insert, true);
					}
				}
				else if (ch == 'P') // down
				{
					if (y < machineNum + 4) {
						gotoXY(&x, &y, mainMenu[option], insert, false); y++; option++;
						gotoXY(&x, &y, mainMenu[option], insert, true);
					}
					else {
						gotoXY(&x, &y, mainMenu[option], insert, false); option -= machineNum - 1; y -= machineNum - 1;
						gotoXY(&x, &y, mainMenu[option], insert, true);
					}
				}
				else if (ValidCharRanged(ch, '1', char(machineNum+48)))
				{
					// if user presses the number instead of using ENTER on the highlighted row
					option = int(ch) - 49; // 49 instead of 48 because option is an index
					break;
				}
				gotoXY(0, 0);
			} while (ch != '\r');
			HideCursor(false);
			// set choice to user selection
			int choice = option + 1;
			if (choice == machineNum)
				starting = false;

			else
			{
				// if machine chosen, call overloaded test function on derived class
				vendTemp = top;
				for (int i = 0; i < choice - 1; i++)
				{
					vendTemp = vendTemp->getNextPtr();
				}
				vendTemp->Test();
			}
			break;
		}
	}
}

void GenerateInventoryReport(VendingMachine *& top)
{
	// method to create a menu to choose a machine to generate a report on
	// a chosen machine

	bool reporting = true;
	string selection;
	while (reporting)
	{
		while (true)
		{
			// use a temp machine pointer to create a dynamic menu
			int machineNum = 1;
			system("cls");
			string mainMenu[14];

			VendingMachine * vendTemp = top;
			while (vendTemp != NULL)
			{
				// loop through linked list
				if (vendTemp->getName().length() > 0)
				{
					// add an option for each  machine
					mainMenu[machineNum - 1] = "\t";
					mainMenu[machineNum - 1] += char(machineNum + 48);
					mainMenu[machineNum - 1] += ") " + vendTemp->getName();
					machineNum++;
				}
				vendTemp = vendTemp->getNextPtr();
			}
			// add an additional option for returning to main menu
			mainMenu[machineNum - 1] = "\t";
			mainMenu[machineNum - 1] += char(machineNum + 48);
			mainMenu[machineNum - 1] += ") Return to Main Menu";

			HideCursor(true);
			system("cls");
			int option = 0;
			string title = "Vending Machine Inventory Report Menu";
			string title2 = "Please choose a machine to generate a report:";

			//variables for current cell
			char ch;
			int x = 5; int y = 1;

			// write menu to screen
			gotoXY(&x, &y, title, true, false);
			y = 4;
			gotoXY(&x, &y, title2, true, false);
			x = 7; y = 5;
			for (int i = 0; i < machineNum; i++)
			{
				if (i == 0)
				{
					gotoXY(&x, &y, mainMenu[option++], true, true); y++;
				}
				else
				{
					gotoXY(&x, &y, mainMenu[option++], true, false); y++;
				}
			}
			gotoXY(&x, &y, mainMenu[option], true, false); y -= machineNum; option -= machineNum;
			gotoXY(0, 0);
			do
			{
				// loop lets user go up and down on the menu by selecting the row
				// ENTER chooses the option.
				ch = _getch();
				bool insert = true;
				if (ch == 'H') // up
				{
					if (y > 5) {
						gotoXY(&x, &y, mainMenu[option--], insert, false); y--;
						gotoXY(&x, &y, mainMenu[option], insert, true);
					}
					else {
						gotoXY(&x, &y, mainMenu[option], insert, false); option += machineNum - 1; y += machineNum - 1;
						gotoXY(&x, &y, mainMenu[option], insert, true);
					}
				}
				else if (ch == 'P') // down
				{
					if (y < machineNum + 4) {
						gotoXY(&x, &y, mainMenu[option], insert, false); y++; option++;
						gotoXY(&x, &y, mainMenu[option], insert, true);
					}
					else {
						gotoXY(&x, &y, mainMenu[option], insert, false); option -= machineNum - 1; y -= machineNum - 1;
						gotoXY(&x, &y, mainMenu[option], insert, true);
					}
				}
				else if (ValidCharRanged(ch, '1', char(machineNum+48)))
				{
					// if user presses the number instead of using ENTER on the highlighted row
					option = int(ch) - 49; // 49 instead of 48 because option is an index
					break;
				}
				gotoXY(0, 0);
			} while (ch != '\r');
			HideCursor(false);

			int choice = option + 1;
			if (choice == machineNum)
				reporting = false;
			else
			{
				// if machine selected, call overloaded report function on derived class
				vendTemp = top;
				for (int i = 0; i < choice - 1; i++)
				{
					vendTemp = vendTemp->getNextPtr();
				}
				vendTemp->Report();
			}
			break;
		}
	}
}

void welcomeScreen() {
	// cool welcome screen animation (upon keypress)

	// set up array of strings (uses escapes for backslashes, so looks strange here
	string intro[] = {
		"__     __             _ _             ",
		"\\ \\   / /__ _ __   __| (_)_ __   __ _ ",
		" \\ \\ / / _ \\ '_ \\ / _` | | '_ \\ / _` |",
		"  \\ V /  __/ | | | (_| | | | | | (_| |",
		"   \\_/ \\___|_| |_|\\__,_|_|_| |_|\\__, |",
		" __  __            _     _      |___/ ",
		"|  \\/  | __ _  ___| |__ (_)_ __   ___ ",
		"| |\\/| |/ _` |/ __| '_ \\| | '_ \\ / _ \\",
		"| |  | | (_| | (__| | | | | | | |  __/",
		"|_|  |_|\\__,_|\\___|_| |_|_|_| |_|\\___|",
		"    ____           _             _    ",
		"   / ___|___ _ __ | |_ _ __ __ _| |   ",
		"  | |   / _ \\ '_ \\| __| '__/ _` | |   ",
		"  | |__|  __/ | | | |_| | | (_| | |   ",
		"   \\____\\___|_| |_|\\__|_|  \\__,_|_|   "
	};
	// initial display of name to screen
	for each(string s in intro)
	{
		for (int j = 0; j < 38; j++)
			cout << " ";
		cout << s.substr(0, 38) << endl;
	}
	// prompt user to hit a key to enter
	cout << endl << endl << endl << "\t\t\t\t\t      Press any key to enter!";
	// when key hit continue
	while(!_kbhit())
	{ }
	// grabs key from buffer
	_getch();
	// set up a timer variable (much smoother than Sleep())
	double tick = GetTickCount();
	system("cls");
	HideCursor(true);
	for (int i = 0; i < 38; i++)
	{
		// loop for animation
		for each(string s in intro)
		{
			// loop to move string over
			for (int j = i; j < 38 + i * 2; j++)
				cout << " ";
			// substring math to clip end during move
			cout << s.substr(0, 38 - i) << endl;
		}
		// set interval to 10 milliseconds for fast animation off screen
		while ((GetTickCount() - tick) < 10) {}
		tick = GetTickCount();
		system("cls");
	}
	HideCursor(false);
}
