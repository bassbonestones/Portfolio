#include "VendingFountain.h"

void VendingFountain::Load()
{
	// overridden load function for fountain machines

	bool loading = true;
	while (loading)
	{
		while (true)
		{
			// set menu to start at 4 because of cup quantities
			int menuNum = 4;

			system("cls");
			string mainMenu[15];
			// add cups to array
			mainMenu[0] = "\t1) Small Cups (10 oz.): " + to_string(getQtySmCups_10oz());
			mainMenu[1] = "\t2) Medium Cups (20 oz.): " + to_string(getQtyMedCups_20oz());
			mainMenu[2] = "\t3) Large Cups (32 oz.): " + to_string(getQtyLgCups_32oz());
	
			for (int i = 0; i < getNumProducts(); i++)
			{
				// loop through products and add options to menu
				mainMenu[menuNum - 1] = "\t";
				mainMenu[menuNum - 1] += char(menuNum+48);
				mainMenu[menuNum - 1] += ") " + product[i].productName;
				menuNum++;
			}
			// add option for stocking a new beverage
			mainMenu[menuNum - 1] = "\t";
			mainMenu[menuNum - 1] += char(menuNum+48);
			mainMenu[menuNum - 1] += ") Stock a New Beverage";
			menuNum++;
			// add an option for returning to previous menu
			mainMenu[menuNum - 1] = "\t";
			mainMenu[menuNum - 1] += char(menuNum + 48);
			mainMenu[menuNum - 1] += ") Return to Previous Menu";

			HideCursor(true);
			system("cls");
			int option = 0;
			string title = "Loading Menu For " + getName() + " (" + type + ")";
			string title2 = "Please select an item to load/restock/update:";

			//variables for current cell
			char ch;
			int x = 5; int y = 1;

			// show menu on screen
			gotoXY(&x, &y, title, true, false);
			y = 4;
			gotoXY(&x, &y, title2, true, false);
			x = 7; y = 5;
			for (int i = 0; i < menuNum; i++)
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
			gotoXY(&x, &y, mainMenu[option], true, false); y -= menuNum; option -= menuNum;
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
						gotoXY(&x, &y, mainMenu[option], insert, false); option += menuNum - 1; y += menuNum - 1;
						gotoXY(&x, &y, mainMenu[option], insert, true);
					}
				}
				else if (ch == 'P') // down
				{
					if (y < menuNum + 4) {
						gotoXY(&x, &y, mainMenu[option], insert, false); y++; option++;
						gotoXY(&x, &y, mainMenu[option], insert, true);
					}
					else {
						gotoXY(&x, &y, mainMenu[option], insert, false); option -= menuNum - 1; y -= menuNum - 1;
						gotoXY(&x, &y, mainMenu[option], insert, true);
					}
				}
				else if (ValidCharRanged(ch, '1', char(menuNum + 48)))
				{
					// if user presses the number instead of using ENTER on the highlighted row
					option = int(ch) - 49; // 49 instead of 48 because option is an index
					break;
				}
				gotoXY(0, 0);
			} while (ch != '\r');

			// set choice to user selection
			int choice = option + 1;

			if (choice == 1)
			{
				// set small cups
				while (true)
				{
					string qty;
					system("cls");
					cout << "Current Number of Small Cups: " << getQtySmCups_10oz() << endl
						<< "Enter a New Quantity (max of 500): ";
					getline(cin, qty);
					if (!ValidInteger(qty, 0, 500))
						continue;
					setQtySmCups_10oz(stoi(qty));
					break;
				}
			}
			else if (choice == 2)
			{
				// set medium cups
				while (true)
				{
					string qty;
					system("cls");
					cout << "Current Number of Medium Cups: " << getQtyMedCups_20oz() << endl
						<< "Enter a New Quantity (max of 500): ";
					getline(cin, qty);
					if (!ValidInteger(qty, 0, 500))
						continue;
					setQtyMedCups_20oz(stoi(qty));
					break;
				}
			}
			else if (choice == 3)
			{
				// set large cups
				while (true)
				{
					string qty;
					system("cls");
					cout << "Current Number of Large Cups: " << getQtyLgCups_32oz() << endl
						<< "Enter a New Quantity (max of 500): ";
					getline(cin, qty);
					if (!ValidInteger(qty, 0, 500))
						continue;
					setQtyLgCups_32oz(stoi(qty));
					break;
				}
			}
			// choice to return
			else if (choice == menuNum)
				loading = false;
			else if (choice == menuNum - 1)
			{
				// choice to add new product
				choice = choice - 3;
				if (getNumProducts() == 10)
				{
					// test for too many products, if so, deny user
					cout << "You have reached your mamimum number of products in this machine." << endl << endl
						<< "Press any key to return to stocking menu." << endl;
					_getch();
				}
				// create new product if there's a spot
				bool creatingNew = true;
				while (creatingNew)
				{
					while (true)
					{
						// update UPC for new beverage
						string id;
						system("cls");
						cout << "Please enter new beverage's UPC (12 Digits): ____________\b\b\b\b\b\b\b\b\b\b\b\b";
						getline(cin, id);
						if (id.length() != 12)
							continue;
						if (!ValidInteger(id))
							continue;
						product[choice - 1].productId = id;
						break;
					}
					while (true)
					{
						// update name for new beverage
						string name;
						system("cls");
						cout << "Please enter new beverage's name: ";
						getline(cin, name);
						if (!ValidName(name))
							continue;
						product[choice - 1].productName = name;
						break;
					}
					while (true)
					{
						// update bag for new beverage
						string answer;
						system("cls");
						cout << "Insert a new bag of this beverage (Y/N): ";
						getline(cin, answer);
						if (answer != "y" && answer != "Y" && answer != "n" && answer != "N")
							continue;
						if (answer == "Y" || answer == "y")
							product[choice - 1].amtOnHand = 640;
						else
							product[choice - 1].amtOnHand = 0;
						break;
					}
					setNumProducts(getNumProducts() + 1);
					creatingNew = false;
				}
			}
			else
			{
				// if user chooses to update existing product
				choice = choice - 3;
				bool stocking = true;
				while (stocking)
				{
					while (true)
					{
						// create dynamic menu for product details
						system("cls");
						string mainMenu[] = {
							"\t1) Product UPC: " + product[choice - 1].productId,
							"\t2) Beverage Name: " + product[choice - 1].productName,
							"\t3) Current Volume Remaining (from 640 oz. bag): " + to_string(product[choice - 1].amtOnHand),
							"\t4) Return to Previous Menu" };
						HideCursor(true);
						system("cls");
						int option = 0;
						string title = "Stock Menu";
						string title2 = "Please select an item to update:";

						//variables for current cell
						char ch;
						int x = 5; int y = 1;
						// show menu
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
						// set choice to user selection
						int itemChoice = option + 1;

						switch (itemChoice)
						{
						case 1:
						{
							// change UPC
							while (true)
							{
								string id;
								system("cls");
								cout << "Current UPC for " << product[choice - 1].productName << ": "
									<< product[choice - 1].productId << endl
									<< "Enter a new UPC (12 Digits): ____________" << "\b\b\b\b\b\b\b\b\b\b\b\b";
								getline(cin, id);
								if (id.length() != 12)
									continue;
								if (!ValidInteger(id))
									continue;
								product[choice - 1].productId = id;
								break;
							}
						}
						break;
						case 2:
						{
							// change name
							while (true)
							{
								string name;
								system("cls");
								cout << "Current Name: " << product[choice - 1].productName << endl
									<< "Enter a New Beverage Name: ";
								getline(cin, name);
								if (!ValidName(name))
									continue;
								product[choice - 1].productName = name;
								break;
							}
						}
						break;
						case 3:
						{
							// change bag for beverage
							while (true)
							{
								string answer;
								system("cls");
								cout << "Current fluid ounces left for " << product[choice - 1].productName << ": "
									<< product[choice - 1].amtOnHand << endl
									<< "Insert a new bag (Y/N): ";
								getline(cin, answer);
								if (answer != "y" && answer != "Y" && answer != "n" && answer != "N")
									continue;
								if (answer == "Y" || answer == "y")
									product[choice - 1].amtOnHand = 640;
								break;
							}
						}
						break;
						default:
							stocking = false;
							break;
						}
						break;
					}
				}

			}
			break;
		}
	}
}

void VendingFountain::SetProductPrices()
{
	// overridden function to set prices of fountain beverage machine
	if (getNumProducts() == 0)
	{
		// first test to see if any products, if not, deny user
		system("cls");
		cout << "Sorry, you have not loaded the machine with any products. Please load " << endl
			<< "the machine before setting prices." << endl << endl
			<< "Press any key to return to previous menu.";
		_getch();
		return;
	}
	// if products continue
	bool setting = true;
	while (setting)
	{
		while (true)
		{
			// create dynamic menu of each product for setting price
			int menuNum = 1;
			system("cls");
			string mainMenu[14];

			int selection;

			for (int i = 0; i < getNumProducts(); i++)
			{
				// loop through products and add option for each with price info
				mainMenu[menuNum - 1] = "\t";
				mainMenu[menuNum - 1] += char(menuNum + 48);
				mainMenu[menuNum - 1] += ") " + product[i].productName + ": $";
				mainMenu[menuNum - 1] += MoneyToString(product[i].salesPrice);
				menuNum++;
			}
			// add option for return to previous menu
			mainMenu[menuNum - 1] = "\t";
			mainMenu[menuNum - 1] += char(menuNum + 48);
			mainMenu[menuNum - 1] += ") Return to Previous Menu";

			HideCursor(true);
			system("cls");
			int option = 0;
			string title = "Price Setting Menu For " + getName() + " (" + type + ")";
			string title2 = "Update for small cup (10 oz.) only. Medium (20 oz.) will be $.25 more";
			string title3 = "than small and large (32 oz.) will be $.50 more than small.";
			string title4 = "Please select a fountain beverage to set/update price of small:";

			//variables for current cell
			char ch;
			int x = 5; int y = 0;

			// show menu
			gotoXY(&x, &y, title, true, false);
			x = 9;  y++;
			gotoXY(&x, &y, title2, true, false);
			y++;
			gotoXY(&x, &y, title3, true, false);
			x = 5; y += 2;
			gotoXY(&x, &y, title4, true, false);
			
			x = 7; y = 5;
			for (int i = 0; i < menuNum; i++)
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
			gotoXY(&x, &y, mainMenu[option], true, false); y -= menuNum; option -= menuNum;
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
						gotoXY(&x, &y, mainMenu[option], insert, false); option += menuNum - 1; y += menuNum - 1;
						gotoXY(&x, &y, mainMenu[option], insert, true);
					}
				}
				else if (ch == 'P') // down
				{
					if (y < menuNum + 4) {
						gotoXY(&x, &y, mainMenu[option], insert, false); y++; option++;
						gotoXY(&x, &y, mainMenu[option], insert, true);
					}
					else {
						gotoXY(&x, &y, mainMenu[option], insert, false); option -= menuNum - 1; y -= menuNum - 1;
						gotoXY(&x, &y, mainMenu[option], insert, true);
					}
				}
				else if (ValidCharRanged(ch, '1', char(menuNum + 48)))
				{
					// if user presses the number instead of using ENTER on the highlighted row
					option = int(ch) - 49; // 49 instead of 48 because option is an index
					break;
				}
				gotoXY(0, 0);
			} while (ch != '\r');

			// set choice to user selection
			int choice = option + 1;
			if (choice == menuNum)
				setting = false;
			else
			{
				// if product selected, change price
				while (true)
				{
					string price;
					system("cls");
					cout << "Current Price for small (10 oz.) " << product[choice - 1].productName << ": $"
						<< fixed << setprecision(2) << product[choice - 1].salesPrice << endl
						<< "Enter a New Price (max $3.00): ";
					getline(cin, price);
					if (!ValidMoney(price, 0.0, 3.0))
						continue;
					product[choice - 1].salesPrice = stod(price);
					break;
				}
			}
			break;
		}
	}
}

void VendingFountain::Test()
{
	// overridden function to test a machine 

	if (getNumProducts() == 0)
	{
		// first test for any products, if not, deny user
		system("cls");
		cout << "Sorry, you have not loaded the machine with any products. Please load " << endl
			<< "the machine before testing." << endl << endl
			<< "Press any key to return to testing menu.";
		_getch();
		return;
	}
	for (int i = 0; i < getNumProducts(); i++)
	{
		// test to make sure all products have prices, if not, deny user
		if (product[i].salesPrice == NULL)
		{
			system("cls");
			cout << "Sorry, the vending machine you have chosen does not have prices set" << endl
				<< "for all of its products. Please set the prices before testing." << endl << endl
				<< "Press any key to return to testing menu.";
			_getch();
			return;
		}
	}
	while (true)
	{
		// create dynamic menu for user to enter money or select product for purchase
		int menuNum = 1;
		system("cls");
		string mainMenu[14];

		int selection;
		// first option - add money to balance
		mainMenu[menuNum - 1] = "\t1) Enter Money";
		menuNum++;

		for (int i = 0; i < getNumProducts(); i++)
		{
			// loop through products and add option for each with price info of small
			mainMenu[menuNum - 1] = "\t";
			mainMenu[menuNum - 1] += char(menuNum + 48);
			mainMenu[menuNum - 1] += ") " + product[i].productName + ": $";
			mainMenu[menuNum - 1] += MoneyToString(product[i].salesPrice);
			menuNum++;
		}
		// add option for returning to previous menu
		mainMenu[menuNum - 1] = "\t";
		mainMenu[menuNum - 1] += char(menuNum + 48);
		mainMenu[menuNum - 1] += ") Return to Previous Menu (returns current balance)";

		HideCursor(true);
		system("cls");
		int option = 0;
		string title = "Testing for Vending Machine: " + getName() + " (" + type + ")";
		string title2 = "--Please enter money or select a beverage you wish to purchase--";
		string title3 = "----------------------------------------------------------------";
		string title4 = "          Balance: $" + MoneyToString(getBalance());

		//variables for current cell
		char ch;
		int x = 2; int y = 0;

		// show menu
		gotoXY(&x, &y, title, true, false);
		y++;
		gotoXY(&x, &y, title2, true, false);
		y++;
		gotoXY(&x, &y, title3, true, false);
		y++;
		gotoXY(&x, &y, title4, true, false);
		y++;
		gotoXY(&x, &y, title3, true, false);
		x = 7; y = 5;
		for (int i = 0; i < menuNum; i++)
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
		gotoXY(&x, &y, mainMenu[option], true, false); y -= menuNum; option -= menuNum;
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
					gotoXY(&x, &y, mainMenu[option], insert, false); option += menuNum - 1; y += menuNum - 1;
					gotoXY(&x, &y, mainMenu[option], insert, true);
				}
			}
			else if (ch == 'P') // down
			{
				if (y < menuNum + 4) {
					gotoXY(&x, &y, mainMenu[option], insert, false); y++; option++;
					gotoXY(&x, &y, mainMenu[option], insert, true);
				}
				else {
					gotoXY(&x, &y, mainMenu[option], insert, false); option -= menuNum - 1; y -= menuNum - 1;
					gotoXY(&x, &y, mainMenu[option], insert, true);
				}
			}
			else if (ValidCharRanged(ch, '1', char(menuNum + 48)))
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

		if (choice == menuNum)
		{
			// if user selects to leave, return money
			if (getBalance() > 0)
			{
				system("cls");
				cout << "Here is your money back: " << endl << "[spits out $" << fixed << setprecision(2) << getBalance() << "]" << endl << endl
					<< "[returning money]" << endl;

				// play sounds while returning money
				while (getBalance() > 0.0)
				{
					if (getBalance() == 1.0)
					{
						mciSendString("open \"Sounds\\coinreturn.mp3\" type mpegvideo alias mp3", NULL, 0, NULL);
						mciSendString("play mp3 from 0 wait", NULL, 0, NULL);
						mciSendString("close mp3", NULL, 0, NULL);
						setBalance(0);
					}
					else if (getBalance() > 1.0)
					{
						mciSendString("open \"Sounds\\coinreturn.mp3\" type mpegvideo alias mp3", NULL, 0, NULL);
						mciSendString("play mp3 from 0 wait", NULL, 0, NULL);
						mciSendString("close mp3", NULL, 0, NULL);
						setBalance(getBalance() - 1.0);
					}
					else if (getBalance() >= .25)
					{
						mciSendString("open \"Sounds\\coinreturn.mp3\" type mpegvideo alias mp3", NULL, 0, NULL);
						mciSendString("play mp3 from 0 wait", NULL, 0, NULL);
						mciSendString("close mp3", NULL, 0, NULL);
						setBalance(getBalance() - .25);
					}
					else if (getBalance() >= .1)
					{
						mciSendString("open \"Sounds\\coinreturn.mp3\" type mpegvideo alias mp3", NULL, 0, NULL);
						mciSendString("play mp3 from 0 wait", NULL, 0, NULL);
						mciSendString("close mp3", NULL, 0, NULL);
						setBalance(getBalance() - .1);
					}
					else if (getBalance() >= .05)
					{
						mciSendString("open \"Sounds\\coinreturn.mp3\" type mpegvideo alias mp3", NULL, 0, NULL);
						mciSendString("play mp3 from 0 wait", NULL, 0, NULL);
						mciSendString("close mp3", NULL, 0, NULL);
						setBalance(0.0);
					}
					else
						setBalance(0.0);
				}
			}
			return;
		}
		else if (choice == 1)
		{
			// if user selected to add money create money menu

			double balance = getBalance();
			bool adding = true;
			while (adding)
			{
				system("cls");
				string mainMenu[] = {
					"\t1) Insert a Nickel",
					"\t2) Insert a Dime",
					"\t3) Insert a Quarter",
					"\t4) Insert a Dollar",
					"\t5) Insert Five Dollars",
					"\t6) Return to Previous Menu" };
				HideCursor(true);
				system("cls");
				int option = 0;
				int preOption = 0;
				string title = "Balance: $" + MoneyToString(balance);

				//variables for current cell
				char ch;
				int x = 5; int y = 1;

				// show menu
				gotoXY(&x, &y, title, true, false);
				x = 7; y = 5;
				gotoXY(&x, &y, mainMenu[option++], true, true); y++;
				gotoXY(&x, &y, mainMenu[option++], true, false); y++;
				gotoXY(&x, &y, mainMenu[option++], true, false); y++;
				gotoXY(&x, &y, mainMenu[option++], true, false); y++;
				gotoXY(&x, &y, mainMenu[option++], true, false); y++;
				gotoXY(&x, &y, mainMenu[option], true, false); y -= 5; option -= 5;
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
							gotoXY(&x, &y, mainMenu[option], insert, false); option += 5; y += 5;
							gotoXY(&x, &y, mainMenu[option], insert, true);
						}
					}
					else if (ch == 'P') // down
					{
						if (y < 10) {
							gotoXY(&x, &y, mainMenu[option], insert, false); y++; option++;
							gotoXY(&x, &y, mainMenu[option], insert, true);
						}
						else {
							gotoXY(&x, &y, mainMenu[option], insert, false); option -= 5; y -= 5;
							gotoXY(&x, &y, mainMenu[option], insert, true);
						}
					}
					else if (ValidCharRanged(ch, '1', '6'))
					{
						// if user presses the number instead of using ENTER on the highlighted row
						preOption = option;
						option = int(ch) - 49; // 49 instead of 48 because option is an index
						break;
					}
					gotoXY(0, 0);
					preOption = option;
				} while (ch != '\r');

				gotoXY(&x, &y, mainMenu[preOption], true, false);
				cout << "\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n[Inserting]" << endl;

				// set choice to user selection
				int choice3 = option + 1;
				switch (choice3)
				{
				case 1: // nickel
					mciSendString("open \"Sounds\\coininsert.mp3\" type mpegvideo alias mp3", NULL, 0, NULL);
					mciSendString("play mp3 from 0 wait", NULL, 0, NULL);
					mciSendString("close mp3", NULL, 0, NULL);
					balance += .05;
					break;
				case 2: // dime
					mciSendString("open \"Sounds\\coininsert.mp3\" type mpegvideo alias mp3", NULL, 0, NULL);
					mciSendString("play mp3 from 0 wait", NULL, 0, NULL);
					mciSendString("close mp3", NULL, 0, NULL);
					balance += .10;
					break;
				case 3: // quarter
					mciSendString("open \"Sounds\\coininsert.mp3\" type mpegvideo alias mp3", NULL, 0, NULL);
					mciSendString("play mp3 from 0 wait", NULL, 0, NULL);
					mciSendString("close mp3", NULL, 0, NULL);
					balance += .25;
					break;
				case 4: // dollar
					mciSendString("open \"Sounds\\dollarinsert.mp3\" type mpegvideo alias mp3", NULL, 0, NULL);
					mciSendString("play mp3 from 0 wait", NULL, 0, NULL);
					mciSendString("close mp3", NULL, 0, NULL);
					balance += 1.00;
					break;
				case 5: // five dollars
					mciSendString("open \"Sounds\\dollarinsert.mp3\" type mpegvideo alias mp3", NULL, 0, NULL);
					mciSendString("play mp3 from 0 wait", NULL, 0, NULL);
					mciSendString("close mp3", NULL, 0, NULL);
					balance += 5.00;
					break;
				default: // return to previous
					adding = false;
					break;
				}
				// update balance to current
				setBalance(balance);
			}
		}
		else
		{
			// if product selected
			while (true)
			{
				// first get some product info and balance
				double balance = getBalance();
				bool breakout = false;
				double productSelectedPrice = product[choice - 2].salesPrice;//this
				string productSelectedName = product[choice - 2].productName;//and this are here because i didn't want to spend so much time typing all that

				if (balance < productSelectedPrice)
				{
					// test for insufficient funds, if so, deny user
					system("cls");
					cout << "Insufficient Funds. Please enter more money to purchase this product." << endl
						<< endl << "Press any key to return to previous menu." << endl;
					_getch();
					break;
				}
				// create menu for cup choice
				system("cls");
				string mainMenu[] = {
					"\t1) Small - 10oz: $" + MoneyToString(product[choice - 2].salesPrice),
					"\t2) Medium - 20oz: $" + MoneyToString(product[choice -2].salesPrice + .25),
					"\t3) Large - 32oz: $" + MoneyToString(product[choice - 2].salesPrice + .5),
					"\t4) Return to Previous Menu" };
				HideCursor(true);
				system("cls");
				int option = 0;
				string title = "Balance: $" + MoneyToString(getBalance());
				string title2 = "Please select a cup size for your " + product[choice - 2].productName + ":";

				//variables for current cell
				char ch;
				int x = 5; int y = 1;

				// show menu
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

				//set choice to user selection
				int choice2 = option + 1;

				// break if choice to return to previous
				if (choice2 == 4)
					break;
				while (true)
				{
					// if cup selected
					if (balance >= productSelectedPrice)
					{
						system("cls");

						if (choice2 == 1) 
						{
							// if small cup chosen
							if (getQtySmCups_10oz() < 1)
							{
								// test for enough cups
								cout << "I'm sorry, there are no 10oz cups in stock." << endl
									<< "Please press any key to return to the previous menu." << endl;
								_getch();
								break;
							}
							else if (product[choice - 2].amtOnHand < 10)
							{
								// test for enough liquid
								cout << "I'm sorry, there is no " << productSelectedName << " in stock." << endl
									<< "Please press any key to return to the previous menu." << endl;
								_getch();
								break;
							}
							// if good, dispense cup  / play sound
							system("cls");
							cout << "[dispensing cup]" << endl;
							mciSendString("open \"Sounds\\cupcombined.mp3\" type mpegvideo alias mp3", NULL, 0, NULL);
							mciSendString("play mp3 from 0 wait", NULL, 0, NULL);
							mciSendString("close mp3", NULL, 0, NULL);

							while (true)
							{
								// ask user if they want ice
								system("cls");
								cout << "Would you like to add ice? (Y/N): ";
								string wantsIce;
								getline(cin, wantsIce);
								if (wantsIce != "y" && wantsIce != "Y" && wantsIce != "n" && wantsIce != "N")
									continue;
								else {
									system("cls");
									if (wantsIce == "Y" || wantsIce == "y")
									{
										// if they want ice, play sound- dispense ice
										cout << "[dispensing ice]" << endl;
										mciSendString("open \"Sounds\\ice.mp3\" type mpegvideo alias mp3", NULL, 0, NULL);
										mciSendString("play mp3 from 0 wait", NULL, 0, NULL);
										mciSendString("close mp3", NULL, 0, NULL);
									}
								}
								break;
							}
							system("cls");
							// automatically pour beverage
							cout << "[dispensing beverage]";
							mciSendString("open \"Sounds\\fountainpour.mp3\" type mpegvideo alias mp3", NULL, 0, NULL);
							mciSendString("play mp3 from 0 wait", NULL, 0, NULL);
							mciSendString("close mp3", NULL, 0, NULL);
							// return change if excess money
							if (balance - productSelectedPrice > 0)
							{
								system("cls");
								setBalance(balance - productSelectedPrice);
								balance = getBalance();
								cout << "Here is your change: " << endl << "[spits out $" << fixed << setprecision(2) << balance << "]" << endl << endl
									<< "[returning money]" << endl;
								while (getBalance() > 0.0)
								{
									if (getBalance() == 1.0)
									{
										mciSendString("open \"Sounds\\coinreturn.mp3\" type mpegvideo alias mp3", NULL, 0, NULL);
										mciSendString("play mp3 from 0 wait", NULL, 0, NULL);
										mciSendString("close mp3", NULL, 0, NULL);
										setBalance(0);
									}
									else if (getBalance() > 1.0)
									{
										mciSendString("open \"Sounds\\coinreturn.mp3\" type mpegvideo alias mp3", NULL, 0, NULL);
										mciSendString("play mp3 from 0 wait", NULL, 0, NULL);
										mciSendString("close mp3", NULL, 0, NULL);
										setBalance(getBalance() - 1.0);
									}
									else if (getBalance() >= .25)
									{
										mciSendString("open \"Sounds\\coinreturn.mp3\" type mpegvideo alias mp3", NULL, 0, NULL);
										mciSendString("play mp3 from 0 wait", NULL, 0, NULL);
										mciSendString("close mp3", NULL, 0, NULL);
										setBalance(getBalance() - .25);
									}
									else if (getBalance() >= .1)
									{
										mciSendString("open \"Sounds\\coinreturn.mp3\" type mpegvideo alias mp3", NULL, 0, NULL);
										mciSendString("play mp3 from 0 wait", NULL, 0, NULL);
										mciSendString("close mp3", NULL, 0, NULL);
										setBalance(getBalance() - .1);
									}
									else if (getBalance() >= .05)
									{
										mciSendString("open \"Sounds\\coinreturn.mp3\" type mpegvideo alias mp3", NULL, 0, NULL);
										mciSendString("play mp3 from 0 wait", NULL, 0, NULL);
										mciSendString("close mp3", NULL, 0, NULL);
										setBalance(0.0);
									}
									else
										setBalance(0.0);
								}
							}
							// end transaction
							system("cls");
							ThankCustomer();
							//now to store everything that just happened
							product[choice - 2].amtOnHand = product[choice - 2].amtOnHand - 10;
							product[choice - 2].amtSold = product[choice - 2].amtSold + 10;
							cupCounter[choice - 2].numSmallsSold += 1;
							setQtySmCups_10oz(getQtySmCups_10oz() - 1);
						}
						else if (choice2 == 2) 
						{
							// if medium cup chose
							if (getQtyMedCups_20oz() < 1)
							{
								// check for enough cups
								cout << "I'm sorry, there are no 20oz cups in stock." << endl
									<< "Please press any key to return to the previous menu." << endl;
								_getch();
								break;
							}
							else if (product[choice - 2].amtOnHand < 20)
							{
								// check for enough liquid
								cout << "I'm sorry, there is not enough " << productSelectedName << " in stock for a medium." << endl
									<< "Please press any key to return to the previous menu." << endl;
								_getch();
								break;
							}
							// if good, dispense cup and play sound
							system("cls");
							cout << "[dispensing cup]" << endl;
							mciSendString("open \"Sounds\\cupcombined.mp3\" type mpegvideo alias mp3", NULL, 0, NULL);
							mciSendString("play mp3 from 0 wait", NULL, 0, NULL);
							mciSendString("close mp3", NULL, 0, NULL);

							while (true)
							{
								// ask for ice
								system("cls");
								cout << "Would you like to add ice? (Y/N): ";
								string wantsIce;
								getline(cin, wantsIce);
								if (wantsIce != "y" && wantsIce != "Y" && wantsIce != "n" && wantsIce != "N")
									continue;
								else {
									system("cls");
									if (wantsIce == "Y" || wantsIce == "y")
									{
										// if they want ice, dispense it
										cout << "[dispensing ice]" << endl;
										mciSendString("open \"Sounds\\ice.mp3\" type mpegvideo alias mp3", NULL, 0, NULL);
										mciSendString("play mp3 from 0 wait", NULL, 0, NULL);
										mciSendString("close mp3", NULL, 0, NULL);
									}
								}
								break;
							}
							system("cls");
							// automatically dispense beverage
							cout << "[dispensing beverage]";
							mciSendString("open \"Sounds\\fountainpour.mp3\" type mpegvideo alias mp3", NULL, 0, NULL);
							mciSendString("play mp3 from 0 wait", NULL, 0, NULL);
							mciSendString("close mp3", NULL, 0, NULL);

							// if extra money return it with sounds
							if (balance - (productSelectedPrice + .25) > 0)
							{
								system("cls");
								setBalance(balance - productSelectedPrice - .25);
								balance = getBalance();
								cout << "Here is your change: " << endl << "[spits out $" << fixed << setprecision(2) << balance << "]" << endl << endl
									<< "[returning money]" << endl;
								while (getBalance() > 0.0)
								{
									if (getBalance() == 1.0)
									{
										mciSendString("open \"Sounds\\coinreturn.mp3\" type mpegvideo alias mp3", NULL, 0, NULL);
										mciSendString("play mp3 from 0 wait", NULL, 0, NULL);
										mciSendString("close mp3", NULL, 0, NULL);
										setBalance(0);
									}
									else if (getBalance() > 1.0)
									{
										mciSendString("open \"Sounds\\coinreturn.mp3\" type mpegvideo alias mp3", NULL, 0, NULL);
										mciSendString("play mp3 from 0 wait", NULL, 0, NULL);
										mciSendString("close mp3", NULL, 0, NULL);
										setBalance(getBalance() - 1.0);
									}
									else if (getBalance() >= .25)
									{
										mciSendString("open \"Sounds\\coinreturn.mp3\" type mpegvideo alias mp3", NULL, 0, NULL);
										mciSendString("play mp3 from 0 wait", NULL, 0, NULL);
										mciSendString("close mp3", NULL, 0, NULL);
										setBalance(getBalance() - .25);
									}
									else if (getBalance() >= .1)
									{
										mciSendString("open \"Sounds\\coinreturn.mp3\" type mpegvideo alias mp3", NULL, 0, NULL);
										mciSendString("play mp3 from 0 wait", NULL, 0, NULL);
										mciSendString("close mp3", NULL, 0, NULL);
										setBalance(getBalance() - .1);
									}
									else if (getBalance() >= .05)
									{
										mciSendString("open \"Sounds\\coinreturn.mp3\" type mpegvideo alias mp3", NULL, 0, NULL);
										mciSendString("play mp3 from 0 wait", NULL, 0, NULL);
										mciSendString("close mp3", NULL, 0, NULL);
										setBalance(0.0);
									}
									else
										setBalance(0.0);
								}
							}
							// end transaction
							system("cls");
							ThankCustomer();
							//now to store everything that just happened
							product[choice - 2].amtOnHand = product[choice - 2].amtOnHand - 20;
							product[choice - 2].amtSold = product[choice - 2].amtSold + 20;
							cupCounter[choice - 2].numMediumsSold += 1;
							setQtyMedCups_20oz(getQtyMedCups_20oz() - 1);
						}
						else if (choice2 == 3) 
						{
							// if large cup chosen
							if (getQtyLgCups_32oz() < 1)
							{
								// test for cup quantity
								cout << "I'm sorry, there are no 32oz cups in stock." << endl
									<< "Please press any key to return to the previous menu." << endl;
								_getch();
								break;
							}
							else if (product[choice - 2].amtOnHand < 32)
							{
								// test for enough liquid
								cout << "I'm sorry, there is not enough " << productSelectedName << " in stock for a large." << endl
									<< "Please press any key to return to the previous menu." << endl;
								_getch();
								break;
							}
							// if good dispense cup and play sound
							system("cls");
							cout << "[dispensing cup]" << endl;
							mciSendString("open \"Sounds\\cupcombined.mp3\" type mpegvideo alias mp3", NULL, 0, NULL);
							mciSendString("play mp3 from 0 wait", NULL, 0, NULL);
							mciSendString("close mp3", NULL, 0, NULL);

							while (true)
							{
								// ask for ice
								system("cls");
								cout << "Would you like to add ice? (Y/N): ";
								string wantsIce;
								getline(cin, wantsIce);
								if (wantsIce != "y" && wantsIce != "Y" && wantsIce != "n" && wantsIce != "N")
									continue;
								else {
									system("cls");
									if (wantsIce == "Y" || wantsIce == "y")
									{
										// if wants ice, dispense and play sound
										cout << "[dispensing ice]" << endl;
										mciSendString("open \"Sounds\\ice.mp3\" type mpegvideo alias mp3", NULL, 0, NULL);
										mciSendString("play mp3 from 0 wait", NULL, 0, NULL);
										mciSendString("close mp3", NULL, 0, NULL);
									}
								}
								break;
							}
							system("cls");
							// automatically pour beverage
							cout << "[dispensing beverage]";
							mciSendString("open \"Sounds\\fountainpour.mp3\" type mpegvideo alias mp3", NULL, 0, NULL);
							mciSendString("play mp3 from 0 wait", NULL, 0, NULL);
							mciSendString("close mp3", NULL, 0, NULL);

							// if extra money, return with sounds
							if (balance - (productSelectedPrice + .5) > 0)
							{
								system("cls");
								setBalance(balance - productSelectedPrice - .5);
								balance = getBalance();
								cout << "Here is your change: " << endl << "[spits out $" << fixed << setprecision(2) << balance << "]" << endl << endl
									<< "[returning money]" << endl;
								while (getBalance() > 0.0)
								{
									if (getBalance() == 1.0)
									{
										mciSendString("open \"Sounds\\coinreturn.mp3\" type mpegvideo alias mp3", NULL, 0, NULL);
										mciSendString("play mp3 from 0 wait", NULL, 0, NULL);
										mciSendString("close mp3", NULL, 0, NULL);
										setBalance(0);
									}
									else if (getBalance() > 1.0)
									{
										mciSendString("open \"Sounds\\coinreturn.mp3\" type mpegvideo alias mp3", NULL, 0, NULL);
										mciSendString("play mp3 from 0 wait", NULL, 0, NULL);
										mciSendString("close mp3", NULL, 0, NULL);
										setBalance(getBalance() - 1.0);
									}
									else if (getBalance() >= .25)
									{
										mciSendString("open \"Sounds\\coinreturn.mp3\" type mpegvideo alias mp3", NULL, 0, NULL);
										mciSendString("play mp3 from 0 wait", NULL, 0, NULL);
										mciSendString("close mp3", NULL, 0, NULL);
										setBalance(getBalance() - .25);
									}
									else if (getBalance() >= .1)
									{
										mciSendString("open \"Sounds\\coinreturn.mp3\" type mpegvideo alias mp3", NULL, 0, NULL);
										mciSendString("play mp3 from 0 wait", NULL, 0, NULL);
										mciSendString("close mp3", NULL, 0, NULL);
										setBalance(getBalance() - .1);
									}
									else if (getBalance() >= .05)
									{
										mciSendString("open \"Sounds\\coinreturn.mp3\" type mpegvideo alias mp3", NULL, 0, NULL);
										mciSendString("play mp3 from 0 wait", NULL, 0, NULL);
										mciSendString("close mp3", NULL, 0, NULL);
										setBalance(0.0);
									}
									else
										setBalance(0.0);
								}
							}
							// end transaction
							system("cls");
							ThankCustomer();
							//now to store everything that just happened
							product[choice - 2].amtOnHand = product[choice - 2].amtOnHand - 32;
							product[choice - 2].amtSold = product[choice - 2].amtSold + 32;
							cupCounter[choice - 2].numLargesSold += 1;
							setQtyLgCups_32oz(getQtyLgCups_32oz() - 1);
						}
					}
					// make sure balance is 0
					breakout = true;
					setBalance(0);
					break;
				}

				if (breakout == true) { break; }
			}
		}
	}
}


void VendingFountain::Report()
{
	// overridden function to display report specific to a fountain beverage machine
	// includes cup info

	system("cls");
	if (getNumProducts() == 0)
	{
		// first test for products, if none, deny user
		cout << "No Products Available to Report: " << getName() << endl;
	}
	else
	{
		// print report
		cout << "Inventory Report for Vending Machine: " << getName() << endl << endl
			<< "Vending Machine Type: " << type << endl << endl
			<< "\t\t -- Products --" << endl
			<< "--------------------------------------------------------" << endl;
		for (int i = 0; i < getNumProducts(); i++)
		{
			double smallSales = (double)cupCounter[i].numSmallsSold * product[i].salesPrice;
			double mediumSales = (double)cupCounter[i].numMediumsSold * (product[i].salesPrice + .25);
			double largeSales = (double)cupCounter[i].numLargesSold * (product[i].salesPrice + .5);
			double totalSales = smallSales + mediumSales + largeSales;
			cout << setw(35) << "Fountain Beverage Name: " << product[i].productName << endl
				<< setw(35) << "Total Fluid Ounces Sold: " << product[i].amtSold << " oz." << endl
				<< setw(35) << "Total Small Cups (10 oz.) Sold: " << cupCounter[i].numSmallsSold << endl
				<< setw(35) << "Total Medium Cups (20 oz.) Sold: " << cupCounter[i].numMediumsSold << endl
				<< setw(35) << "Total Large Cups (32 oz.) Sold: " << cupCounter[i].numLargesSold << endl
				<< setw(36) << "Revenue From Small Cups: $" << fixed << setprecision(2) << smallSales << endl
				<< setw(36) << "Revenue From Medium Cups: $" << fixed << setprecision(2) << mediumSales << endl
				<< setw(36) << "Revenue From Large Cups: $" << fixed << setprecision(2) << largeSales << endl
				<< setw(36) << "Total Sales Generated: $" << totalSales << endl
				<< "--------------------------------------------------------" << endl;
		}
	}
	cout << endl << "Press any key to return to previous menu." << endl;
	_getch();
}
