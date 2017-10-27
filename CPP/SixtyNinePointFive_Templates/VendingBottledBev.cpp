#include "VendingBottledBev.h"

void VendingBottledBev::Load()
{
	// overridden Load method

	bool loading = true;
	while (loading)
	{
		while (true)
		{
			// create a menu of products to choose from for loading
			int menuNum = 1;
			system("cls");
			string mainMenu[14];

			int selection;

			for (int i = 0; i < getNumProducts(); i++)
			{
				// loop through products and display name in menu option
				mainMenu[menuNum - 1] = "\t";
				mainMenu[menuNum - 1] += char(menuNum + 48);
				mainMenu[menuNum - 1] += ") " + product[i].productName;
				menuNum++;
			}
			// addional menu option for adding a new beverage
			mainMenu[menuNum - 1] = "\t";
			mainMenu[menuNum - 1] += char(menuNum + 48);
			mainMenu[menuNum - 1] += ") Stock a New Beverage";
			menuNum++;
			// menu option to return to previous menu
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

			// write menu to screen
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
				// if user to add a new product
				if (getNumProducts() == 10)
				{
					// if max number of products reached, deny user
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
						// update new product with UPC
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
						// update new product with name
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
						// update new product with quantity
						string qty;
						system("cls");
						cout << "Please enter new beverage's initial stock (max 50): ";
						getline(cin, qty);
						if (!ValidInteger(qty, 0, 50))
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
				// update existing product
				bool stocking = true;
				while (stocking)
				{
					while (true)
					{
						// create dynamic menu for updating existing product
						system("cls");
						string mainMenu[] = {
							"\t1) Product UPC: " + product[choice - 1].productId,
							"\t2) Beverage Name: " + product[choice - 1].productName,
							"\t3) Number of Bottles Stocked: " + to_string(product[choice - 1].amtOnHand),
							"\t4) Return to Previous Menu" };
						HideCursor(true);
						system("cls");
						int option = 0;
						string title = "Stock Menu:";
						string title2 = "Please select an item to update:";

						//variables for current cell
						char ch;
						int x = 5; int y = 1;
						// show menu to screen
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
							// change quantity
							while (true)
							{
								string qty;
								system("cls");
								cout << "Current Number of Bottles for " << product[choice - 1].productName << ": "
									<< product[choice - 1].amtOnHand << endl
									<< "Enter a New Quantity (max of 50): ";
								getline(cin, qty);
								if (!ValidInteger(qty, 0, 50))
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

void VendingBottledBev::SetProductPrices()
{
	// overridden method for setting prices of bottled bev machine
	if (getNumProducts() == 0)
	{
		// if no products deny user
		system("cls");
		cout << "Sorry, you have not loaded the machine with any products. Please load " << endl
			<< "the machine before setting prices." << endl << endl
			<< "Press any key to return to previous menu.";
		_getch();
		return;
	}
	// if products, allow
	bool setting = true;
	while (setting)
	{
		while (true)
		{
			// create dynamic menu to choose product to set price 
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
			string title2 = "Please select an beverage to set/update price:";

			//variables for current cell
			char ch;
			int x = 5; int y = 1;

			// write menu to screen
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
				// if product chosen, set new price
				while (true)
				{
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

void VendingBottledBev::Test()
{
	// overridden function for testing a machine (bottled bev)
	if (getNumProducts() == 0)
	{
		// first check for existing products, if none, deny user
		system("cls");
		cout << "Sorry, you have not loaded the machine with any products. Please load " << endl
			<< "the machine before testing." << endl << endl
			<< "Press any key to return to testing menu.";
		_getch();
		return;
	}
	for (int i = 0; i < getNumProducts(); i++)
	{
		// then test to make sure all products have prices
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
		// create menu to add money or purchase
		int menuNum = 1;
		system("cls");
		string mainMenu[14];

		int selection;

		mainMenu[menuNum - 1] = "\t1) Enter Money";
		menuNum++;

		for (int i = 0; i < getNumProducts(); i++)
		{
			// loop through products and add each with price info
			mainMenu[menuNum - 1] = "\t";
			mainMenu[menuNum - 1] += char(menuNum + 48);
			mainMenu[menuNum - 1] += ") " + product[i].productName + ": $";
			mainMenu[menuNum - 1] += MoneyToString(product[i].salesPrice);
			menuNum++;
		}
		// add return to previous menu option
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

		// write menu to screen
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
			// if user exits, return money
			if (getBalance() > 0)
			{
				system("cls");
				cout << "Here is your money back: " << endl << "[spits out $" << fixed << setprecision(2) << getBalance() << "]" << endl << endl
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
			return;
		}
		else if (choice == 1)
		{
			// if choice to add money to balance, create dynamic menu for adding bills/coins
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
					// play approciate sound for each money added
				case 1:
					mciSendString("open \"Sounds\\coininsert.mp3\" type mpegvideo alias mp3", NULL, 0, NULL);
					mciSendString("play mp3 from 0 wait", NULL, 0, NULL);
					mciSendString("close mp3", NULL, 0, NULL);
					balance += .05;
					break;
				case 2:
					mciSendString("open \"Sounds\\coininsert.mp3\" type mpegvideo alias mp3", NULL, 0, NULL);
					mciSendString("play mp3 from 0 wait", NULL, 0, NULL);
					mciSendString("close mp3", NULL, 0, NULL);
					balance += .10;
					break;
				case 3:
					mciSendString("open \"Sounds\\coininsert.mp3\" type mpegvideo alias mp3", NULL, 0, NULL);
					mciSendString("play mp3 from 0 wait", NULL, 0, NULL);
					mciSendString("close mp3", NULL, 0, NULL);
					balance += .25;
					break;
				case 4:
					mciSendString("open \"Sounds\\dollarinsert.mp3\" type mpegvideo alias mp3", NULL, 0, NULL);
					mciSendString("play mp3 from 0 wait", NULL, 0, NULL);
					mciSendString("close mp3", NULL, 0, NULL);
					balance += 1.00;
					break;
				case 5:
					mciSendString("open \"Sounds\\dollarinsert.mp3\" type mpegvideo alias mp3", NULL, 0, NULL);
					mciSendString("play mp3 from 0 wait", NULL, 0, NULL);
					mciSendString("close mp3", NULL, 0, NULL);
					balance += 5.00;
					break;
				default:
					adding = false;
					break;
				}
				// reset balance to current
				setBalance(balance);
			}
		}
		else
		{
			// user selects object to buy
			while (true)
			{
				// first pull some info from product
				double balance = getBalance();
				bool breakout = false;
				double productSelectedPrice = product[choice - 2].salesPrice;//this
				string productSelectedName = product[choice - 2].productName;//and this are here because i didn't want to spend so much time typing all that

				if (balance < productSelectedPrice)
				{
					// set to see if balance is sufficient, if not, deny user
					system("cls");
					cout << "Insufficient Funds. Please enter more money to purchase this beverage." << endl
						<< endl << "Press any key to return to previous menu." << endl;
					_getch();
					break;
				}
				if (balance >= productSelectedPrice)//if they put in the right amout of cash or more then yah gotta do something
				{
					// if balance is good, proceed
					system("cls");

					if (product[choice - 2].amtOnHand < 1)
					{
						// check for quantity, if none, deny user
						cout << "I'm sorry, there is no " << productSelectedName << " in stock." << endl;
						break;
					}
					// otherwise dispense product and play sounds
					cout << "[dispensing bottle]";
					mciSendString("open \"Sounds\\productout.mp3\" type mpegvideo alias mp3", NULL, 0, NULL);
					mciSendString("play mp3 from 0 wait", NULL, 0, NULL);
					mciSendString("close mp3", NULL, 0, NULL);
					system("cls");
					setBalance(balance - productSelectedPrice);
					balance = getBalance();
					// return money and play sounds
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
				product[choice - 2].amtOnHand = product[choice - 2].amtOnHand - 1;
				product[choice - 2].amtSold = product[choice - 2].amtSold + 1;
				break;
			}
		}
	}
}

void VendingBottledBev::Report()
{
	// overridden function for report specific to bottled beverage
	system("cls");
	if (getNumProducts() == 0)
	{
		// if no products, deny user
		cout << "No Products Available to Report: " << getName() << endl;
	}
	else
	{
		// if products, list all info for each product in report
		cout << "Inventory Report for Vending Machine: " << getName() << endl << endl
			<< "Vending Machine Type: " << type << endl
			<< "\t -- Products --" << endl
			<< "----------------------------------------" << endl;
		for (int i = 0; i < getNumProducts(); i++)
		{
			double totalSales = (double)product[i].amtSold * product[i].salesPrice;
			cout << setw(26) << "Beverage Name: " << product[i].productName << endl
				<< setw(26) << "Number of Bottles Sold: " << product[i].amtSold << endl
				<< setw(27) << "Sales Generated: $" << fixed << setprecision(2) << totalSales << endl
				<< "----------------------------------------" << endl;
		}
	}
	// prompt user to hit key to return
	cout << endl << "Press any key to return to previous menu." << endl;
	_getch();

}
