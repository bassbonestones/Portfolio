#include "VendingPrepackSnack.h"

void VendingPrepackSnack::Load()
{
	// overridden function to load machine products for prepack snack
	bool loading = true;
	while (loading)
	{
		while (true)
		{
			// create dynamic menu for products to load to
			int menuNum = 1;
			system("cls");
			string mainMenu[14];

			int selection;

			for (int i = 0; i < getNumProducts(); i++)
			{
				// loop through products and add with name
				mainMenu[menuNum - 1] = "\t";
				mainMenu[menuNum - 1] += char(menuNum + 48);
				mainMenu[menuNum - 1] += ") " + product[i].productName;
				menuNum++;
			}

			// add option for stocking new
			mainMenu[menuNum - 1] = "\t";
			mainMenu[menuNum - 1] += char(menuNum + 48);
			mainMenu[menuNum - 1] += ") Stock a New Snack";
			menuNum++;
			// add return option to previous menu
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

			// show menu
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
			HideCursor(false);
			// set choice to user selection
			int choice = option + 1;
			if (choice == menuNum)
				loading = false;

			else if (choice == menuNum - 1)
			{
				// if choice to add new
				if (getNumProducts() == 10)
				{
					// test for maxed out products, if so, deny user
					system("cls");
					cout << "You have reached your mamimum number of products in this machine." << endl << endl
						<< "Press any key to return to stocking menu." << endl;
					_getch();
					break;
				}
				// create new product if there's a spot
				bool creatingNew = true;
				while (creatingNew)
				{
					while (true)
					{
						// update UPC for new snack
						string id;
						system("cls");
						cout << "Please enter new snack's UPC (12 Digits): ____________\b\b\b\b\b\b\b\b\b\b\b\b";
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
						// update name for new snack
						string name;
						system("cls");
						cout << "Please enter new snack's name: ";
						getline(cin, name);
						if (!ValidName(name))
							continue;
						product[choice - 1].productName = name;
						break;
					}
					while (true)
					{
						// update qty for new snack
						string qty;
						system("cls");
						cout << "Please enter new snack's initial stock (max 30): ";
						getline(cin, qty);
						if (!ValidInteger(qty, 0, 30))
							continue;
						product[choice - 1].amtOnHand = stoi(qty);
						break;
					}
					setNumProducts(getNumProducts() + 1);
					creatingNew = false;
				}
			}
			else
			{
				// if updating exising snack

				bool stocking = true;
				while (stocking)
				{
					while (true)
					{
						// create dynamic menu to choose which attribute to update
						system("cls");
						string mainMenu[] = {
							"\t1) Product UPC: " + product[choice - 1].productId,
							"\t2) Snack Name: " + product[choice - 1].productName,
							"\t3) Quanitity of Snacks Stocked: " + to_string(product[choice - 1].amtOnHand),
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
							// update UPC
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
							// update name
							while (true)
							{
								string name;
								system("cls");
								cout << "Current Name: " << product[choice - 1].productName << endl
									<< "Enter a New Snack Name: ";
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
							// update qty
							while (true)
							{
								string qty;
								system("cls");
								cout << "Current Quantity for " << product[choice - 1].productName << ": "
									<< product[choice - 1].amtOnHand << endl
									<< "Enter a New Quantity (max of 30): ";
								getline(cin, qty);
								if (!ValidInteger(qty, 0, 30))
									continue;
								product[choice - 1].amtOnHand = stoi(qty);
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


void VendingPrepackSnack::SetProductPrices()
{
	// overridden function to set prices of prepackages snack machines
	if (getNumProducts() == 0)
	{
		// first test to make sure products exists, if not, deny user
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
			// create menu of all products to select for price update
			int menuNum = 1;
			system("cls");
			string mainMenu[14];

			int selection;

			for (int i = 0; i < getNumProducts(); i++)
			{
				// loop through all products and add option for each with price info
				mainMenu[menuNum - 1] = "\t";
				mainMenu[menuNum - 1] += char(menuNum + 48);
				mainMenu[menuNum - 1] += ") " + product[i].productName + ": $";
				mainMenu[menuNum - 1] += MoneyToString(product[i].salesPrice);
				menuNum++;
			}
			// add option to return to previous menu
			mainMenu[menuNum - 1] = "\t";
			mainMenu[menuNum - 1] += char(menuNum + 48);
			mainMenu[menuNum - 1] += ") Return to Previous Menu";

			HideCursor(true);
			system("cls");
			int option = 0;
			string title = "Price Setting Menu For " + getName() + "(" + type + ")";
			string title2 = "Please select an snack to set/update price:";

			//variables for current cell
			char ch;
			int x = 5; int y = 1;
			// show menu
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
			HideCursor(false);
			// set choice to user selection
			int choice = option + 1;

			if (choice == menuNum)
				setting = false;
			else
			{
				while (true)
				{
					// if product chosen, update price
					string price;
					system("cls");
					cout << "Current Price for " << product[choice - 1].productName << ": $"
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

void VendingPrepackSnack::Test()
{
	// overridden function to test prepackages snack machine
	if (getNumProducts() == 0)
	{
		// first test to make sure there are products, if not, deny user
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
		// create dynamic menu to allow user to enter money or select product to purchase
		int menuNum = 1;
		system("cls");
		string mainMenu[14];

		int selection;
		// add option for entering money
		mainMenu[menuNum - 1] = "\t1) Enter Money";
		menuNum++;

		for (int i = 0; i < getNumProducts(); i++)
		{
			// loop through all products, and option for each with price info
			mainMenu[menuNum - 1] = "\t";
			mainMenu[menuNum - 1] += char(menuNum + 48);
			mainMenu[menuNum - 1] += ") " + product[i].productName + ": $";
			mainMenu[menuNum - 1] += MoneyToString(product[i].salesPrice);
			menuNum++;
		}
		// add option to return to previous menu
		mainMenu[menuNum - 1] = "\t";
		mainMenu[menuNum - 1] += char(menuNum + 48);
		mainMenu[menuNum - 1] += ") Return to Previous Menu (returns current balance)";

		HideCursor(true);
		system("cls");
		int option = 0;
		string title = "Testing for Vending Machine: " + getName() + " (" + type + ")";
		string title2 = "--Please enter money or select a snack you wish to purchase--";
		string title3 = "-------------------------------------------------------------";
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
		//set choice to user selection
		int choice = option + 1;

		if (choice == menuNum)
		{
			// if user selects to return to previous, give back any balance, and play sounds
			if (getBalance() > 0)
			{
				system("cls");
				cout << "Here is your money back: " << endl << "[spits out $" << fixed << setprecision(2) << getBalance() << "]" << endl << endl
					<< "[returning money]" << endl;
				while (getBalance() > 0.0)
				{
					if (getBalance() == 1.0)
					{
						mciSendString("open \"C:\\Program Files\\cpp\\Resources\\Sounds\\coinreturn.mp3\" type mpegvideo alias mp3", NULL, 0, NULL);
						mciSendString("play mp3 from 0 wait", NULL, 0, NULL);
						mciSendString("close mp3", NULL, 0, NULL);
						setBalance(0);
					}
					else if (getBalance() > 1.0)
					{
						mciSendString("open \"C:\\Program Files\\cpp\\Resources\\Sounds\\coinreturn.mp3\" type mpegvideo alias mp3", NULL, 0, NULL);
						mciSendString("play mp3 from 0 wait", NULL, 0, NULL);
						mciSendString("close mp3", NULL, 0, NULL);
						setBalance(getBalance() - 1.0);
					}
					else if (getBalance() >= .25)
					{
						mciSendString("open \"C:\\Program Files\\cpp\\Resources\\Sounds\\coinreturn.mp3\" type mpegvideo alias mp3", NULL, 0, NULL);
						mciSendString("play mp3 from 0 wait", NULL, 0, NULL);
						mciSendString("close mp3", NULL, 0, NULL);
						setBalance(getBalance() - .25);
					}
					else if (getBalance() >= .1)
					{
						mciSendString("open \"C:\\Program Files\\cpp\\Resources\\Sounds\\coinreturn.mp3\" type mpegvideo alias mp3", NULL, 0, NULL);
						mciSendString("play mp3 from 0 wait", NULL, 0, NULL);
						mciSendString("close mp3", NULL, 0, NULL);
						setBalance(getBalance() - .1);
					}
					else if (getBalance() >= .05)
					{
						mciSendString("open \"C:\\Program Files\\cpp\\Resources\\Sounds\\coinreturn.mp3\" type mpegvideo alias mp3", NULL, 0, NULL);
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
			// if user chooses to enter money
			double balance = getBalance();
			bool adding = true;
			while (adding)
			{
				// add dynamic menu for entering coins and bills
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
					// play sound appropriate and add to balance
				case 1:
					mciSendString("open \"C:\\Program Files\\cpp\\Resources\\Sounds\\coininsert.mp3\" type mpegvideo alias mp3", NULL, 0, NULL);
					mciSendString("play mp3 from 0 wait", NULL, 0, NULL);
					mciSendString("close mp3", NULL, 0, NULL);
					balance += .05;
					break;
				case 2:
					mciSendString("open \"C:\\Program Files\\cpp\\Resources\\Sounds\\coininsert.mp3\" type mpegvideo alias mp3", NULL, 0, NULL);
					mciSendString("play mp3 from 0 wait", NULL, 0, NULL);
					mciSendString("close mp3", NULL, 0, NULL);
					balance += .10;
					break;
				case 3:
					mciSendString("open \"C:\\Program Files\\cpp\\Resources\\Sounds\\coininsert.mp3\" type mpegvideo alias mp3", NULL, 0, NULL);
					mciSendString("play mp3 from 0 wait", NULL, 0, NULL);
					mciSendString("close mp3", NULL, 0, NULL);
					balance += .25;
					break;
				case 4:
					mciSendString("open \"C:\\Program Files\\cpp\\Resources\\Sounds\\dollarinsert.mp3\" type mpegvideo alias mp3", NULL, 0, NULL);
					mciSendString("play mp3 from 0 wait", NULL, 0, NULL);
					mciSendString("close mp3", NULL, 0, NULL);
					balance += 1.00;
					break;
				case 5:
					mciSendString("open \"C:\\Program Files\\cpp\\Resources\\Sounds\\dollarinsert.mp3\" type mpegvideo alias mp3", NULL, 0, NULL);
					mciSendString("play mp3 from 0 wait", NULL, 0, NULL);
					mciSendString("close mp3", NULL, 0, NULL);
					balance += 5.00;
					break;
				default:
					adding = false;
					break;
				}
				// update actual balance to current
				setBalance(balance);
			}
		}
		else
		{
			// if product selected for purchase
			while (true)
			{
				// first get some product info and balance
				double balance = getBalance();
				bool breakout = false;
				double productSelectedPrice = product[choice - 2].salesPrice;//this
				string productSelectedName = product[choice - 2].productName;//and this are here because i didn't want to spend so much time typing all that

				if (balance < productSelectedPrice)
				{
					// test insufficient funds, if so, deny user
					system("cls");
					cout << "Insufficient Funds. Please enter more money to purchase this snack." << endl
						<< endl << "Press any key to return to previous menu." << endl;
					_getch();
					break;
				}
				if (balance >= productSelectedPrice)
				{
					// if sufficient funds
					system("cls");

					if (product[choice - 2].amtOnHand < 1)
					{
						// set for enough qty, if not, deny user
						cout << "I'm sorry, there is no " << productSelectedName << " in stock." << endl;
						break;
					}
					// if good, dispense snack and play sound
					cout << "[dispensing snack]";
					mciSendString("open \"C:\\Program Files\\cpp\\Resources\\Sounds\\productout.mp3\" type mpegvideo alias mp3", NULL, 0, NULL);
					mciSendString("play mp3 from 0 wait", NULL, 0, NULL);
					mciSendString("close mp3", NULL, 0, NULL);
					system("cls");
					setBalance(balance - productSelectedPrice);
					// return any change
					cout << "Here is your change: " << endl << "[spits out $" << fixed << setprecision(2) << balance - productSelectedPrice << "]" << endl << endl
						<< "[returning money]" << endl;

					while (getBalance() > 0.0)
					{
						if (getBalance() == 1.0)
						{
							mciSendString("open \"C:\\Program Files\\cpp\\Resources\\Sounds\\coinreturn.mp3\" type mpegvideo alias mp3", NULL, 0, NULL);
							mciSendString("play mp3 from 0 wait", NULL, 0, NULL);
							mciSendString("close mp3", NULL, 0, NULL);
							setBalance(0);
						}
						else if (getBalance() > 1.0)
						{
							mciSendString("open \"C:\\Program Files\\cpp\\Resources\\Sounds\\coinreturn.mp3\" type mpegvideo alias mp3", NULL, 0, NULL);
							mciSendString("play mp3 from 0 wait", NULL, 0, NULL);
							mciSendString("close mp3", NULL, 0, NULL);
							setBalance(getBalance() - 1.0);
						}
						else if (getBalance() >= .25)
						{
							mciSendString("open \"C:\\Program Files\\cpp\\Resources\\Sounds\\coinreturn.mp3\" type mpegvideo alias mp3", NULL, 0, NULL);
							mciSendString("play mp3 from 0 wait", NULL, 0, NULL);
							mciSendString("close mp3", NULL, 0, NULL);
							setBalance(getBalance() - .25);
						}
						else if (getBalance() >= .1)
						{
							mciSendString("open \"C:\\Program Files\\cpp\\Resources\\Sounds\\coinreturn.mp3\" type mpegvideo alias mp3", NULL, 0, NULL);
							mciSendString("play mp3 from 0 wait", NULL, 0, NULL);
							mciSendString("close mp3", NULL, 0, NULL);
							setBalance(getBalance() - .1);
						}
						else if (getBalance() >= .05)
						{
							mciSendString("open \"C:\\Program Files\\cpp\\Resources\\Sounds\\coinreturn.mp3\" type mpegvideo alias mp3", NULL, 0, NULL);
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
				product[choice - 2].amtOnHand = product[choice - 2].amtOnHand - 1;
				product[choice - 2].amtSold = product[choice - 2].amtSold + 1;
				break;
			}
		}
	}
}

void VendingPrepackSnack::Report()
{
	// overridden function for displaying report specific to a prepackaged snack machine
	system("cls");
	if (getNumProducts() == 0)
	{
		// test for products, if none, deny user
		cout << "No Products Available to Report: " << getName() << endl;
	}
	else
	{
		// if snacks, display report
		cout << "Inventory Report for Vending Machine: " << getName() << endl << endl
			<< "Vending Machine Type: " << type << endl << endl
			<< "\t -- Products --" << endl
			<< "----------------------------------------" << endl;
		for (int i = 0; i < getNumProducts(); i++)
		{
			double totalSales = (double)product[i].amtSold * product[i].salesPrice;
			cout << setw(19) << "Snack Name: " << product[i].productName << endl
				<< setw(19) << "Number Sold: " << product[i].amtSold << endl
				<< setw(20) << "Sales Generated: $" << fixed << setprecision(2) << totalSales << endl
				<< "----------------------------------------" << endl;
		}
	}
	cout << endl << "Press any key to return to previous menu." << endl;
	_getch();
}
