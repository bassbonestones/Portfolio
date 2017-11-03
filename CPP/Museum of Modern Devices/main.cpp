// Jeremiah Stones
// Advanced C++
// Rodney Ortigo

// Lab Over Virtual Functions and Polymorphism
// 7/11/2017

// Program Specification
// This program transforms a program that uses inheritance so that it displays polymorphism 
// through virtual functions passed from a base class to its derived classes.  The program
// allows a user to view and update data about different types of computers.  The data is
// loaded from a file, and the file is updated if the user changes any of the computers' 
// information.  

// The base class had two functions transformed into virtual functions and direct subclasses 
// of the Computer base class (PC and MobileDevice) have one function that is transformed to
// virtual.  This is the initialize function, that differs in argument length between PC-type
// computers and MobileDevice-type computers.  

// Include the class header files.
#include "Tablet.h"
#include "CellPhone.h"
#include "Desktop.h"
#include "Laptop.h"
#include <fstream>

int main()
{
	// Variable for array of computer pointers to hold all devices
	Computer * computers[10];

	// Update dimension function declaration.
	void updateSize(Computer * cell);

	// Create a stream reader for getting data from file.
	ifstream reader;
	try
	{
		// Open the reader object on the selected data file.
		reader.open("C:/Program Files/cpp/Resources/Computers.dat");
	}
	catch (exception &ex)
	{
		cout << "Error: " << ex.what() << endl;
	}
	
	// create counter to read from file lines and set elements of computers array
	int computerCtr = 0;
	// create booleans for valid responses from user input and exit choice
	bool valid, exit;
	exit = false;
	// create string for user input processing
	string input;
	if (reader.is_open())
	{
		// read from file to fill computer array
		while (!reader.eof())
		{
			string line;
			getline(reader, line);
			// check for types to assign array elements
			if (line.substr(0, 10).find("Desktop") != string::npos) 
			{
				computers[computerCtr++] = new Desktop(line);
			}
			else if (line.substr(0, 10).find("Laptop") != string::npos)
			{
				computers[computerCtr++] = new Laptop(line);
			}
			else if (line.substr(0, 10).find("CellPhone") != string::npos)
			{
				computers[computerCtr++] = new CellPhone(line);
			}
			else if (line.substr(0, 10).find("Tablet") != string::npos)
			{
				computers[computerCtr++] = new Tablet(line);
			}
		}
		// closer reader 
		reader.close();
	}
	else
	{
		// if not read correctly, display error and exit program
		cout << "File did not properly load: Press key to exit program..." << endl;
		_getch();
		exit = true;
	}

	while (!exit)
	{
		// loop will not execute if file does not load correctly, main loop of program

		// set validation flag for input validation
		valid = false;
		do
		{
		// loop while invalid input
		system("cls");
		// display main menu
		cout << "Welcome to the Museum of Modern Devices!" << endl << endl
			<< "This is the museums device inventory management system. Please use the menu to view device specs and update them."
			<< endl << endl
			<< "Select An Option: " << endl
			<< "\t1) View A Device's Specs" << endl
			<< "\t2) Update A Device's Specs" << endl
			<< "\t3) Save Changes And Exit The Program" << endl;
			cout << "Your Choice: ";
			getline(cin, input);
			if (input.length() > 0)
			{
				valid = ValidWhole(input,1,3);
			}
		} while (!valid);
		switch (input[0] - '0') // case returns int value of choice instead of string
		{
		case 1: // view devices - submenu of device types
		{
			// set exit boolean flag
			bool exitView = false;
			while (!exitView)
			{
				// loop until user exits loop with menu choice
				valid = false;
				do
				{
				// display view menu for devices
				system("cls");
				cout << "Select A Type To View: " << endl
					<< "\t1) Desktops" << endl
					<< "\t2) Laptops" << endl
					<< "\t3) Cell Phones" << endl
					<< "\t4) Tablets" << endl
					<< "\t5) Return To Main Menu" << endl;
					cout << "Your Choice: ";
					getline(cin, input);
					if (input.length() > 0)
					{
						valid = ValidWhole(input,1,5);
					}
				} while (!valid);
				switch (input[0] - '0')
				{
				case 1: // view desktops - submenu of specific desktops
				{
					while (true)
					{
						valid = false;
						do
						{
							// show view menu to pick specific desktop
							system("cls");
							cout << "Select A Desktop To See Full Specs: " << endl;
							for (int i = 0; i < 2; i++)
							{
								cout << "\t" << to_string(i + 1) << ") " << computers[i]->getMake() << " " << computers[i]->getModel() << endl;
							}
							cout << "\t3) Return To View Device Menu" << endl;
							cout << "Your Choice: ";
							getline(cin, input);
							if (input.length() > 0)
							{
								valid = ValidWhole(input,1,3);
							}
						} while (!valid);
						if (input == "3")
							break;
						system("cls");
						// get index by subtracting char 48 ('0') and -1 to account for menu starting with 1 instead of 0
						computers[input[0] - '0' - 1]->printSpecs();
					}
					break;
				}
				case 2: // view laptops - submenu of specific laptops
				{
					while (true)
					{
						valid = false;
						do
						{
							// show menu for view menu of specific laptops
							system("cls");
							cout << "Select A Laptop To See Full Specs: " << endl;
							for (int i = 2; i < 5; i++)
							{
								cout << "\t" << to_string(i - 1) << ") " << computers[i]->getMake() << " " << computers[i]->getModel() << endl;
							}
							cout << "\t4) Return To View Device Menu" << endl;
							cout << "Your Choice: ";
							getline(cin, input);
							if (input.length() > 0)
							{
								valid = ValidWhole(input, 1, 4);
							}
						} while (!valid);
						if (input == "4")
							break;
						system("cls");
						// get index by subtracting char 48 ('0') and + 1 to account for menu starting with 1 instead of 0 and offset by desktops
						computers[input[0] - '0' + 1]->printSpecs();
					}
					break;
				}
				case 3: // view cell phones - submenu of specific cell phones
				{
					while (true)
					{
						valid = false;
						do
						{
							system("cls");
							// show menu of specific cell phones to view
							cout << "Select A Cellphone To See Full Specs: " << endl;
							for (int i = 5; i < 8; i++)
							{
								cout << "\t" << to_string(i - 4) << ") " << computers[i]->getMake() << " " << computers[i]->getModel() << endl;
							}
							cout << "\t4) Return To View Device Menu" << endl;
							cout << "Your Choice: ";
							getline(cin, input);
							if (input.length() > 0)
							{
								valid = ValidWhole(input, 1, 4);
							}
						} while (!valid);
						if (input == "4")
							break;
						system("cls");
						// get index by subtracting char 48 ('0') and -1 to account for menu starting with 1 instead of 0 + 4 to account for offset by other devices
						computers[input[0] - '0' + 4]->printSpecs();
					}
					break;
				}
				case 4: // view tablets - submenu of specific tablets
				{
					while (true)
					{
						valid = false;
						do
						{
							system("cls");
							// view menu of specific tablets to view
							cout << "Select A Tablet To See Full Specs: " << endl;
							for (int i = 8; i < 10; i++)
							{
								cout << "\t" << to_string(i - 7) << ") " << computers[i]->getMake() << " " << computers[i]->getModel() << endl;
							}
							cout << "\t3) Return To View Device Menu" << endl;
							cout << "Your Choice: ";
							getline(cin, input);
							if (input.length() > 0)
							{
								valid = ValidWhole(input, 1, 3);
							}
						} while (!valid);
						if (input == "3")
							break;
						system("cls");
						// get index by subtracting char 48 ('0') and -1 to account for menu starting with 1 instead of 0, + 7 for devices offset
						computers[input[0] - '0' + 7]->printSpecs();
					}

					break;
				}
				default:
					exitView = true;
					break;
				}
			} // end of while loop for viewing devices
			break;
		}
		case 2: // update devices - submenu of device types 
		{
			bool exitUpdate = false;
			while (!exitUpdate)
			{
				valid = false;
				do
				{
				system("cls");
				// menu to pick type of device to update
				cout << "Select A Type To Update: " << endl
					<< "\t1) Desktops" << endl
					<< "\t2) Laptops" << endl
					<< "\t3) Cell Phones" << endl
					<< "\t4) Tablets" << endl
					<< "\t5) Return To Main Menu" << endl;
					cout << "Your Choice: ";
					getline(cin, input);
					if (input.length() > 0)
					{
						valid = ValidWhole(input,1,5);
					}
				} while (!valid);
				switch (input[0] - '0')
				{
				///////////////////////////////////////////////////////////////////////////////////////////////////
				case 1: // update desktops - submenu of specific desktops
				{
					bool exitUpdateDesktop = false;
					while (!exitUpdateDesktop)
					{
						valid = false;
						do
						{
							system("cls");
							cout << "Select A Desktop To Update: " << endl;
							for (int i = 0; i < 2; i++)
							{
								cout << "\t" << to_string(i + 1) << ") " << computers[i]->getMake() << " " << computers[i]->getModel() << endl;
							}
							cout << "\t3) Return To Update Device Menu" << endl;
							cout << "Your Choice: ";
							getline(cin, input);
							if (input.length() > 0)
							{
								valid = ValidWhole(input,1,3);
							}
						} while (!valid);
						switch (input[0] - '0')
						{
						case 1: // update first desktop
						{
							bool exitUpdateDesktop1 = false;
							while (!exitUpdateDesktop1)
							{
								valid = false;
								do
								{
									system("cls");
									cout << "\t~Update Desktop Attribute Menu~" << endl << endl
										<< "Select An Attribute To Update: " << endl
										<< "\t1) Make: " << computers[0]->getMake() << endl
										<< "\t2) Model: " << computers[0]->getModel() << endl
										<< "\t3) Processor: " << computers[0]->getProcessorName() << endl
										<< "\t4) Storage: " << computers[0]->getStorageGB() << " GB" << endl
										<< "\t5) RAM: " << computers[0]->getRAM() << " GB" << endl
										<< "\t6) Graphics: " << dynamic_cast<Desktop*>(computers[0])->getGraphicsCard() << endl
										<< "\t7) OS: " << dynamic_cast<Desktop*>(computers[0])->getOS() << endl
										<< "\t8) Number of Expansion Slots: " << dynamic_cast<Desktop*>(computers[0])->getNumExpansionSlots() << endl
										<< "\t9) Has DVD Drive: " << dynamic_cast<Desktop*>(computers[0])->getHasDVD() << endl
										<< "\t10) Price (new): $" << computers[0]->getCost() << endl
										<< "\t11) Return To Update Desktops Menu" << endl;
									cout << "Your Choice: ";
									getline(cin, input);
									if (input.length() > 0)
									{
										valid = ValidWhole(input, 1, 11);
									}
								} while (!valid);
								switch (atoi(input.c_str()))
								{
								case 1: // update make
								{
									while (true)
									{
										system("cls");
										cout << "Previous Make: " << computers[0]->getMake() << endl
											<< "Enter New Make: ";
										string make;
										getline(cin, make);
										if (!ValidNameChars(make))
										{
											cout << "Invalid input: Please use only letters, numbers, periods, and apostrophes." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											computers[0]->setMake(make);
											break;
										}
									}
									break;
								}
								case 2: // update model
								{

									while (true)
									{
										system("cls");
										cout << "Previous Model: " << computers[0]->getModel() << endl
											<< "Enter New Model: ";
										string model;
										getline(cin, model);
										if (!ValidNameChars(model))
										{
											cout << "Invalid input: Please use only letters, numbers, periods, and apostrophes." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											computers[0]->setModel(model);
											break;
										}
									}
									break;
								}
								case 3: // update cpu
								{
									while (true)
									{
										system("cls");
										cout << "Previous Processor Name: " << computers[0]->getProcessorName() << endl
											<< "Enter New Processor Name: ";
										string cp;
										getline(cin, cp);
										if (!ValidNameChars(cp))
										{
											cout << "Invalid input: Please use only letters, numbers, periods, and apostrophes." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											computers[0]->setProcessorName(cp);
											break;
										}
									}
									break;
								}
								case 4: // update storage
								{
									while (true)
									{
										system("cls");
										cout << "Previous Storage Amount (GB): " << computers[0]->getStorageGB() << endl
											<< "Enter New Storage Amount (GB): ";
										string gb;
										getline(cin, gb);
										if (!ValidDouble(gb))
										{
											cout << "Invalid input: Please use only valid numbers." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											computers[0]->setStorageGB(gb);
											break;
										}
									}
									break;
								}
								case 5: // update ram
								{
									while (true)
									{
										system("cls");
										cout << "Previous RAM (GB): " << computers[0]->getRAM() << endl
											<< "Enter New RAM (GB): ";
										string rm;
										getline(cin, rm);
										if (!ValidDouble(rm))
										{
											cout << "Invalid input: Please use only valid numbers." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											computers[0]->setRAM(rm);
											break;
										}
									}
									break;
								}
								case 6: // update graphics
								{
									while (true)
									{
										system("cls");
										cout << "Previous Graphics Card Name: " << dynamic_cast<Desktop*>(computers[0])->getGraphicsCard() << endl
											<< "Enter New Graphics Card Name: ";
										string card;
										getline(cin, card);
										if (!ValidNameChars(card))
										{
											cout << "Invalid input: Please use only letters, numbers, periods, and apostrophes." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											dynamic_cast<Desktop*>(computers[0])->setGraphicsCard(card);
											break;
										}
									}
									break;
								}
								case 7: // update OS
								{
									while (true)
									{
										system("cls");
										cout << "Previous Operating System Name: " << dynamic_cast<Desktop*>(computers[0])->getOS() << endl
											<< "Enter New Operating System Name: ";
										string os;
										getline(cin, os);
										if (!ValidNameChars(os))
										{
											cout << "Invalid input: Please use only letters, numbers, periods, and apostrophes." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											dynamic_cast<Desktop*>(computers[0])->setOS(os);
											break;
										}
									}
									break;
								}
								case 8: // update slots
								{
									while (true)
									{
										system("cls");
										cout << "Previous Number of Expansion Slots: " << dynamic_cast<Desktop*>(computers[0])->getNumExpansionSlots() << endl
											<< "Enter New Number of Expansion Slots: ";
										string sl;
										getline(cin, sl);
										if (!ValidWhole(sl))
										{
											cout << "Invalid input: Please use only whole numbers." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											dynamic_cast<Desktop*>(computers[0])->setNumExpansionSlots(sl);
											break;
										}
									}
									break;
								}
								case 9: // update hasDVD
								{
									while (true)
									{
										system("cls");
										cout << "Previous Value for \'Device Has DVD\': " << dynamic_cast<Desktop*>(computers[0])->getHasDVD() << endl
											<< "Enter New Value for \'Device Has DVD\': ";
										string dvd;
										getline(cin, dvd);
										if (!ValidYN(dvd))
										{
											cout << "Invalid input: Please use only Y or N." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											dynamic_cast<Desktop*>(computers[0])->setHasDVD(dvd);
											break;
										}
									}
									break;
								}
								case 10: // update cost
								{
									while (true)
									{
										system("cls");
										cout << "Previous Cost: $" << computers[0]->getCost() << endl
											<< "Enter New Cost: $";
										string ct;
										getline(cin, ct);
										if (!ValidMoney(ct))
										{
											cout << "Invalid input: Please use only numbers and maximum of two digits past the decimal." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											computers[0]->setCost(ct);
											break;
										}
									}
									break;
								}
								default: // return to previous menu
									exitUpdateDesktop1 = true;
									break;
								}
							}
							break;
						}
						case 2: // update second desktop
						{
							bool exitUpdateDesktop2 = false;
							while (!exitUpdateDesktop2)
							{
								valid = false;
								do
								{
									system("cls");
									cout << "\t~Update Desktop Attribute Menu~" << endl << endl
										<< "Select An Attribute To Update: " << endl
										<< "\t1) Make: " << computers[1]->getMake() << endl
										<< "\t2) Model: " << computers[1]->getModel() << endl
										<< "\t3) Processor: " << computers[1]->getProcessorName() << endl
										<< "\t4) Storage: " << computers[1]->getStorageGB() << " GB" << endl
										<< "\t5) RAM: " << computers[1]->getRAM() << " GB" << endl
										<< "\t6) Graphics: " << dynamic_cast<Desktop*>(computers[1])->getGraphicsCard() << endl
										<< "\t7) OS: " << dynamic_cast<Desktop*>(computers[1])->getOS() << endl
										<< "\t8) Number of Expansion Slots: " << dynamic_cast<Desktop*>(computers[1])->getNumExpansionSlots() << endl
										<< "\t9) Has DVD Drive: " << dynamic_cast<Desktop*>(computers[1])->getHasDVD() << endl
										<< "\t10) Price (new): $" << computers[1]->getCost() << endl
										<< "\t11) Return To Update Desktops Menu" << endl;
									cout << "Your Choice: ";
									getline(cin, input);
									if (input.length() > 0)
									{
										valid = ValidWhole(input, 1, 11);
									}
								} while (!valid);
								switch (atoi(input.c_str()))
								{
								case 1: // update make
								{
									while (true)
									{
										system("cls");
										cout << "Previous Make: " << computers[1]->getMake() << endl
											<< "Enter New Make: ";
										string make;
										getline(cin, make);
										if (!ValidNameChars(make))
										{
											cout << "Invalid input: Please use only letters, numbers, periods, and apostrophes." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											computers[1]->setMake(make);
											break;
										}
									}
									break;
								}
								case 2: // update model
								{

									while (true)
									{
										system("cls");
										cout << "Previous Model: " << computers[1]->getModel() << endl
											<< "Enter New Model: ";
										string model;
										getline(cin, model);
										if (!ValidNameChars(model))
										{
											cout << "Invalid input: Please use only letters, numbers, periods, and apostrophes." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											computers[1]->setModel(model);
											break;
										}
									}
									break;
								}
								case 3: // update cpu
								{
									while (true)
									{
										system("cls");
										cout << "Previous Processor Name: " << computers[1]->getProcessorName() << endl
											<< "Enter New Processor Name: ";
										string cp;
										getline(cin, cp);
										if (!ValidNameChars(cp))
										{
											cout << "Invalid input: Please use only letters, numbers, periods, and apostrophes." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											computers[1]->setProcessorName(cp);
											break;
										}
									}
									break;
								}
								case 4: // update storage
								{
									while (true)
									{
										system("cls");
										cout << "Previous Storage Amount (GB): " << computers[1]->getStorageGB() << endl
											<< "Enter New Storage Amount (GB): ";
										string gb;
										getline(cin, gb);
										if (!ValidDouble(gb))
										{
											cout << "Invalid input: Please use only valid numbers." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											computers[1]->setStorageGB(gb);
											break;
										}
									}
									break;
								}
								case 5: // update ram
								{
									while (true)
									{
										system("cls");
										cout << "Previous RAM (GB): " << computers[1]->getRAM() << endl
											<< "Enter New RAM (GB): ";
										string rm;
										getline(cin, rm);
										if (!ValidDouble(rm))
										{
											cout << "Invalid input: Please use only valid numbers." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											computers[1]->setRAM(rm);
											break;
										}
									}
									break;
								}
								case 6: // update graphics
								{
									while (true)
									{
										system("cls");
										cout << "Previous Graphics Card Name: " << dynamic_cast<Desktop*>(computers[1])->getGraphicsCard() << endl
											<< "Enter New Graphics Card Name: ";
										string card;
										getline(cin, card);
										if (!ValidNameChars(card))
										{
											cout << "Invalid input: Please use only letters, numbers, periods, and apostrophes." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											dynamic_cast<Desktop*>(computers[1])->setGraphicsCard(card);
											break;
										}
									}
									break;
								}
								case 7: // update OS
								{
									while (true)
									{
										system("cls");
										cout << "Previous Operating System Name: " << dynamic_cast<Desktop*>(computers[1])->getOS() << endl
											<< "Enter New Operating System Name: ";
										string os;
										getline(cin, os);
										if (!ValidNameChars(os))
										{
											cout << "Invalid input: Please use only letters, numbers, periods, and apostrophes." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											dynamic_cast<Desktop*>(computers[1])->setOS(os);
											break;
										}
									}
									break;
								}
								case 8: // update slots
								{
									while (true)
									{
										system("cls");
										cout << "Previous Number of Expansion Slots: " << dynamic_cast<Desktop*>(computers[1])->getNumExpansionSlots() << endl
											<< "Enter New Number of Expansion Slots: ";
										string sl;
										getline(cin, sl);
										if (!ValidWhole(sl))
										{
											cout << "Invalid input: Please use only whole numbers." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											dynamic_cast<Desktop*>(computers[1])->setNumExpansionSlots(sl);
											break;
										}
									}
									break;
								}
								case 9: // update hasDVD
								{
									while (true)
									{
										system("cls");
										cout << "Previous Value for \'Device Has DVD\': " << dynamic_cast<Desktop*>(computers[1])->getHasDVD() << endl
											<< "Enter New Value for \'Device Has DVD\': ";
										string dvd;
										getline(cin, dvd);
										if (!ValidYN(dvd))
										{
											cout << "Invalid input: Please use only Y or N." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											dynamic_cast<Desktop*>(computers[1])->setHasDVD(dvd);
											break;
										}
									}
									break;
								}
								case 10: // update cost
								{
									while (true)
									{
										system("cls");
										cout << "Previous Cost: $" << computers[1]->getCost() << endl
											<< "Enter New Cost: $";
										string ct;
										getline(cin, ct);
										if (!ValidMoney(ct))
										{
											cout << "Invalid input: Please use only numbers and maximum of two digits past the decimal." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											computers[1]->setCost(ct);
											break;
										}
									}
									break;
								}
								default: // return to previous menu
									exitUpdateDesktop2 = true;
									break;
								}
							}
							break;
						}
						default:
							exitUpdateDesktop = true;
							break;
						}
					} // end of loop for updating Desktops
					break;
				}
				///////////////////////////////////////////////////////////////////////////////////////////////////
				case 2: // update laptops - submenu of specific laptops
				{
					bool exitUpdateLaptop = false;
					while (!exitUpdateLaptop)
					{
						valid = false;
						do
						{
							system("cls");
							cout << "Select A Laptop To Update: " << endl;
							for (int i = 2; i < 5; i++)
							{
								cout << "\t" << to_string(i - 1) << ") " << computers[i]->getMake() << " " << computers[i]->getModel() << endl;
							}
							cout << "\t4) Return To Update Device Menu" << endl;
							cout << "Your Choice: ";
							getline(cin, input);
							if (input.length() > 0)
							{
								valid = ValidWhole(input, 1, 4);
							}
						} while (!valid);
						switch (input[0] - '0')
						{
						case 1: // update first laptop
						{
							bool exitUpdateLaptops1 = false;
							while (!exitUpdateLaptops1)
							{
								valid = false;
								do
								{
									system("cls");
									cout << "\t~Update Laptop Attribute Menu~" << endl << endl
										<< "Select An Attribute To Update: " << endl
										<< "\t1) Make: " << computers[2]->getMake() << endl
										<< "\t2) Model: " << computers[2]->getModel() << endl
										<< "\t3) Processor: " << computers[2]->getProcessorName() << endl
										<< "\t4) Storage: " << computers[2]->getStorageGB() << " GB" << endl
										<< "\t5) RAM: " << computers[2]->getRAM() << " GB" << endl
										<< "\t6) Graphics: " << dynamic_cast<Laptop*>(computers[2])->getGraphicsCard() << endl
										<< "\t7) OS: " << dynamic_cast<Laptop*>(computers[2])->getOS() << endl
										<< "\t8) Weight: " << dynamic_cast<Laptop*>(computers[2])->getWeightLB() << endl
										<< "\t9) Wifi Card Type: " << dynamic_cast<Laptop*>(computers[2])->getWifiCardType() << endl
										<< "\t10) Price (new): $" << computers[2]->getCost() << endl
										<< "\t11) Return To Update laptops Menu" << endl;
									cout << "Your Choice: ";
									getline(cin, input);
									if (input.length() > 0)
									{
										valid = ValidWhole(input, 1, 11);
									}
								} while (!valid);
								switch (atoi(input.c_str()))
								{
								case 1: // update make
								{
									while (true)
									{
										system("cls");
										cout << "Previous Make: " << computers[2]->getMake() << endl
											<< "Enter New Make: ";
										string make;
										getline(cin, make);
										if (!ValidNameChars(make))
										{
											cout << "Invalid input: Please use only letters, numbers, periods, and apostrophes." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											computers[2]->setMake(make);
											break;
										}
									}
									break;
								}
								case 2: // update model
								{

									while (true)
									{
										system("cls");
										cout << "Previous Model: " << computers[2]->getModel() << endl
											<< "Enter New Model: ";
										string model;
										getline(cin, model);
										if (!ValidNameChars(model))
										{
											cout << "Invalid input: Please use only letters, numbers, periods, and apostrophes." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											computers[2]->setModel(model);
											break;
										}
									}
									break;
								}
								case 3: // update cpu
								{
									while (true)
									{
										system("cls");
										cout << "Previous Processor Name: " << computers[2]->getProcessorName() << endl
											<< "Enter New Processor Name: ";
										string cp;
										getline(cin, cp);
										if (!ValidNameChars(cp))
										{
											cout << "Invalid input: Please use only letters, numbers, periods, and apostrophes." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											computers[2]->setProcessorName(cp);
											break;
										}
									}
									break;
								}
								case 4: // update storage
								{
									while (true)
									{
										system("cls");
										cout << "Previous Storage Amount (GB): " << computers[2]->getStorageGB() << endl
											<< "Enter New Storage Amount (GB): ";
										string gb;
										getline(cin, gb);
										if (!ValidDouble(gb))
										{
											cout << "Invalid input: Please use only valid numbers." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											computers[2]->setStorageGB(gb);
											break;
										}
									}
									break;
								}
								case 5: // update ram
								{
									while (true)
									{
										system("cls");
										cout << "Previous RAM (GB): " << computers[2]->getRAM() << endl
											<< "Enter New RAM (GB): ";
										string rm;
										getline(cin, rm);
										if (!ValidDouble(rm))
										{
											cout << "Invalid input: Please use only valid numbers." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											computers[2]->setRAM(rm);
											break;
										}
									}
									break;
								}
								case 6: // update graphics
								{
									while (true)
									{
										system("cls");
										cout << "Previous Graphics Card Name: " << dynamic_cast<Laptop*>(computers[2])->getGraphicsCard() << endl
											<< "Enter New Graphics Card Name: ";
										string card;
										getline(cin, card);
										if (!ValidNameChars(card))
										{
											cout << "Invalid input: Please use only letters, numbers, periods, and apostrophes." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											dynamic_cast<Laptop*>(computers[2])->setGraphicsCard(card);
											break;
										}
									}
									break;
								}
								case 7: // update OS
								{
									while (true)
									{
										system("cls");
										cout << "Previous Operating System Name: " << dynamic_cast<Laptop*>(computers[2])->getOS() << endl
											<< "Enter New Operating System Name: ";
										string os;
										getline(cin, os);
										if (!ValidNameChars(os))
										{
											cout << "Invalid input: Please use only letters, numbers, periods, and apostrophes." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											dynamic_cast<Laptop*>(computers[2])->setOS(os);
											break;
										}
									}
									break;
								}
								case 8: // update weight
								{
									while (true)
									{
										system("cls");
										cout << "Previous Weight: " << dynamic_cast<Laptop*>(computers[2])->getWeightLB() << endl
											<< "Enter New Weight: ";
										string w;
										getline(cin, w);
										if (!ValidDouble(w))
										{
											cout << "Invalid input: Please use only valid numbers." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											dynamic_cast<Laptop*>(computers[2])->setWeightLB(w);
											break;
										}
									}
									break;
								}
								case 9: // update wifi
								{
									while (true)
									{
										system("cls");
										cout << "Previous Wifi Card Type: " << dynamic_cast<Laptop*>(computers[2])->getWifiCardType() << endl
											<< "Enter New Wifi Card Type: ";
										string wifi;
										getline(cin, wifi);
										if (!ValidNameChars(wifi))
										{
											cout << "Invalid input: Please use only letters, numbers, periods, and apostrophes." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											dynamic_cast<Laptop*>(computers[2])->setWifiCardType(wifi);
											break;
										}
									}
									break;
								}
								case 10: // update cost
								{
									while (true)
									{
										system("cls");
										cout << "Previous Cost: $" << computers[2]->getCost() << endl
											<< "Enter New Cost: $";
										string ct;
										getline(cin, ct);
										if (!ValidMoney(ct))
										{
											cout << "Invalid input: Please use only numbers and maximum of two digits past the decimal." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											computers[2]->setCost(ct);
											break;
										}
									}
									break;
								}
								default: // return to previous menu
									exitUpdateLaptops1 = true;
									break;
								}
							}
							break;
						}
						case 2://update second laptop
						{
							bool exitUpdateLaptops2 = false;
							while (!exitUpdateLaptops2)
							{
								valid = false;
								do
								{
									system("cls");
									cout << "\t~Update Laptop Attribute Menu~" << endl << endl
										<< "Select An Attribute To Update: " << endl
										<< "\t1) Make: " << computers[3]->getMake() << endl
										<< "\t2) Model: " << computers[3]->getModel() << endl
										<< "\t3) Processor: " << computers[3]->getProcessorName() << endl
										<< "\t4) Storage: " << computers[3]->getStorageGB() << " GB" << endl
										<< "\t5) RAM: " << computers[3]->getRAM() << " GB" << endl
										<< "\t6) Graphics: " << dynamic_cast<Laptop*>(computers[3])->getGraphicsCard() << endl
										<< "\t7) OS: " << dynamic_cast<Laptop*>(computers[3])->getOS() << endl
										<< "\t8) Weight: " << dynamic_cast<Laptop*>(computers[3])->getWeightLB() << endl
										<< "\t9) Wifi Card Type: " << dynamic_cast<Laptop*>(computers[3])->getWifiCardType() << endl
										<< "\t10) Price (new): $" << computers[3]->getCost() << endl
										<< "\t11) Return To Update laptops Menu" << endl;
									cout << "Your Choice: ";
									getline(cin, input);
									if (input.length() > 0)
									{
										valid = ValidWhole(input, 1, 11);
									}
								} while (!valid);
								switch (atoi(input.c_str()))
								{
								case 1: // update make
								{
									while (true)
									{
										system("cls");
										cout << "Previous Make: " << computers[3]->getMake() << endl
											<< "Enter New Make: ";
										string make;
										getline(cin, make);
										if (!ValidNameChars(make))
										{
											cout << "Invalid input: Please use only letters, numbers, periods, and apostrophes." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											computers[3]->setMake(make);
											break;
										}
									}
									break;
								}
								case 2: // update model
								{

									while (true)
									{
										system("cls");
										cout << "Previous Model: " << computers[3]->getModel() << endl
											<< "Enter New Model: ";
										string model;
										getline(cin, model);
										if (!ValidNameChars(model))
										{
											cout << "Invalid input: Please use only letters, numbers, periods, and apostrophes." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											computers[3]->setModel(model);
											break;
										}
									}
									break;
								}
								case 3: // update cpu
								{
									while (true)
									{
										system("cls");
										cout << "Previous Processor Name: " << computers[3]->getProcessorName() << endl
											<< "Enter New Processor Name: ";
										string cp;
										getline(cin, cp);
										if (!ValidNameChars(cp))
										{
											cout << "Invalid input: Please use only letters, numbers, periods, and apostrophes." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											computers[3]->setProcessorName(cp);
											break;
										}
									}
									break;
								}
								case 4: // update storage
								{
									while (true)
									{
										system("cls");
										cout << "Previous Storage Amount (GB): " << computers[3]->getStorageGB() << endl
											<< "Enter New Storage Amount (GB): ";
										string gb;
										getline(cin, gb);
										if (!ValidDouble(gb))
										{
											cout << "Invalid input: Please use only valid numbers." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											computers[3]->setStorageGB(gb);
											break;
										}
									}
									break;
								}
								case 5: // update ram
								{
									while (true)
									{
										system("cls");
										cout << "Previous RAM (GB): " << computers[3]->getRAM() << endl
											<< "Enter New RAM (GB): ";
										string rm;
										getline(cin, rm);
										if (!ValidDouble(rm))
										{
											cout << "Invalid input: Please use only valid numbers." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											computers[3]->setRAM(rm);
											break;
										}
									}
									break;
								}
								case 6: // update graphics
								{
									while (true)
									{
										system("cls");
										cout << "Previous Graphics Card Name: " << dynamic_cast<Laptop*>(computers[3])->getGraphicsCard() << endl
											<< "Enter New Graphics Card Name: ";
										string card;
										getline(cin, card);
										if (!ValidNameChars(card))
										{
											cout << "Invalid input: Please use only letters, numbers, periods, and apostrophes." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											dynamic_cast<Laptop*>(computers[3])->setGraphicsCard(card);
											break;
										}
									}
									break;
								}
								case 7: // update OS
								{
									while (true)
									{
										system("cls");
										cout << "Previous Operating System Name: " << dynamic_cast<Laptop*>(computers[3])->getOS() << endl
											<< "Enter New Operating System Name: ";
										string os;
										getline(cin, os);
										if (!ValidNameChars(os))
										{
											cout << "Invalid input: Please use only letters, numbers, periods, and apostrophes." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											dynamic_cast<Laptop*>(computers[3])->setOS(os);
											break;
										}
									}
									break;
								}
								case 8: // update weight
								{
									while (true)
									{
										system("cls");
										cout << "Previous Weight: " << dynamic_cast<Laptop*>(computers[3])->getWeightLB() << endl
											<< "Enter New Weight: ";
										string w;
										getline(cin, w);
										if (!ValidDouble(w))
										{
											cout << "Invalid input: Please use only valid numbers." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											dynamic_cast<Laptop*>(computers[3])->setWeightLB(w);
											break;
										}
									}
									break;
								}
								case 9: // update wifi
								{
									while (true)
									{
										system("cls");
										cout << "Previous Wifi Card Type: " << dynamic_cast<Laptop*>(computers[3])->getWifiCardType() << endl
											<< "Enter New Wifi Card Type: ";
										string wifi;
										getline(cin, wifi);
										if (!ValidNameChars(wifi))
										{
											cout << "Invalid input: Please use only letters, numbers, periods, and apostrophes." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											dynamic_cast<Laptop*>(computers[3])->setWifiCardType(wifi);
											break;
										}
									}
									break;
								}
								case 10: // update cost
								{
									while (true)
									{
										system("cls");
										cout << "Previous Cost: $" << computers[3]->getCost() << endl
											<< "Enter New Cost: $";
										string ct;
										getline(cin, ct);
										if (!ValidMoney(ct))
										{
											cout << "Invalid input: Please use only numbers and maximum of two digits past the decimal." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											computers[3]->setCost(ct);
											break;
										}
									}
									break;
								}
								default: // return to previous menu
									exitUpdateLaptops2 = true;
									break;
								}
							}
							break;
						}
						case 3://update third laptop
						{
							bool exitUpdateLaptops3 = false;
							while (!exitUpdateLaptops3)
							{
								valid = false;
								do
								{
									system("cls");
									cout << "\t~Update Laptop Attribute Menu~" << endl << endl
										<< "Select An Attribute To Update: " << endl
										<< "\t1) Make: " << computers[4]->getMake() << endl
										<< "\t2) Model: " << computers[4]->getModel() << endl
										<< "\t3) Processor: " << computers[4]->getProcessorName() << endl
										<< "\t4) Storage: " << computers[4]->getStorageGB() << " GB" << endl
										<< "\t5) RAM: " << computers[4]->getRAM() << " GB" << endl
										<< "\t6) Graphics: " << dynamic_cast<Laptop*>(computers[4])->getGraphicsCard() << endl
										<< "\t7) OS: " << dynamic_cast<Laptop*>(computers[4])->getOS() << endl
										<< "\t8) Weight: " << dynamic_cast<Laptop*>(computers[4])->getWeightLB() << endl
										<< "\t9) Wifi Card Type: " << dynamic_cast<Laptop*>(computers[4])->getWifiCardType() << endl
										<< "\t10) Price (new): $" << computers[4]->getCost() << endl
										<< "\t11) Return To Update laptops Menu" << endl;
									cout << "Your Choice: ";
									getline(cin, input);
									if (input.length() > 0)
									{
										valid = ValidWhole(input, 1, 11);
									}
								} while (!valid);
								switch (atoi(input.c_str()))
								{
								case 1: // update make
								{
									while (true)
									{
										system("cls");
										cout << "Previous Make: " << computers[4]->getMake() << endl
											<< "Enter New Make: ";
										string make;
										getline(cin, make);
										if (!ValidNameChars(make))
										{
											cout << "Invalid input: Please use only letters, numbers, periods, and apostrophes." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											computers[4]->setMake(make);
											break;
										}
									}
									break;
								}
								case 2: // update model
								{

									while (true)
									{
										system("cls");
										cout << "Previous Model: " << computers[4]->getModel() << endl
											<< "Enter New Model: ";
										string model;
										getline(cin, model);
										if (!ValidNameChars(model))
										{
											cout << "Invalid input: Please use only letters, numbers, periods, and apostrophes." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											computers[4]->setModel(model);
											break;
										}
									}
									break;
								}
								case 3: // update cpu
								{
									while (true)
									{
										system("cls");
										cout << "Previous Processor Name: " << computers[4]->getProcessorName() << endl
											<< "Enter New Processor Name: ";
										string cp;
										getline(cin, cp);
										if (!ValidNameChars(cp))
										{
											cout << "Invalid input: Please use only letters, numbers, periods, and apostrophes." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											computers[4]->setProcessorName(cp);
											break;
										}
									}
									break;
								}
								case 4: // update storage
								{
									while (true)
									{
										system("cls");
										cout << "Previous Storage Amount (GB): " << computers[4]->getStorageGB() << endl
											<< "Enter New Storage Amount (GB): ";
										string gb;
										getline(cin, gb);
										if (!ValidDouble(gb))
										{
											cout << "Invalid input: Please use only valid numbers." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											computers[4]->setStorageGB(gb);
											break;
										}
									}
									break;
								}
								case 5: // update ram
								{
									while (true)
									{
										system("cls");
										cout << "Previous RAM (GB): " << computers[4]->getRAM() << endl
											<< "Enter New RAM (GB): ";
										string rm;
										getline(cin, rm);
										if (!ValidDouble(rm))
										{
											cout << "Invalid input: Please use only valid numbers." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											computers[4]->setRAM(rm);
											break;
										}
									}
									break;
								}
								case 6: // update graphics
								{
									while (true)
									{
										system("cls");
										cout << "Previous Graphics Card Name: " << dynamic_cast<Laptop*>(computers[4])->getGraphicsCard() << endl
											<< "Enter New Graphics Card Name: ";
										string card;
										getline(cin, card);
										if (!ValidNameChars(card))
										{
											cout << "Invalid input: Please use only letters, numbers, periods, and apostrophes." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											dynamic_cast<Laptop*>(computers[4])->setGraphicsCard(card);
											break;
										}
									}
									break;
								}
								case 7: // update OS
								{
									while (true)
									{
										system("cls");
										cout << "Previous Operating System Name: " << dynamic_cast<Laptop*>(computers[4])->getOS() << endl
											<< "Enter New Operating System Name: ";
										string os;
										getline(cin, os);
										if (!ValidNameChars(os))
										{
											cout << "Invalid input: Please use only letters, numbers, periods, and apostrophes." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											dynamic_cast<Laptop*>(computers[4])->setOS(os);
											break;
										}
									}
									break;
								}
								case 8: // update weight
								{
									while (true)
									{
										system("cls");
										cout << "Previous Weight: " << dynamic_cast<Laptop*>(computers[4])->getWeightLB() << endl
											<< "Enter New Weight: ";
										string w;
										getline(cin, w);
										if (!ValidDouble(w))
										{
											cout << "Invalid input: Please use only valid numbers." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											dynamic_cast<Laptop*>(computers[4])->setWeightLB(w);
											break;
										}
									}
									break;
								}
								case 9: // update wifi
								{
									while (true)
									{
										system("cls");
										cout << "Previous Wifi Card Type: " << dynamic_cast<Laptop*>(computers[4])->getWifiCardType() << endl
											<< "Enter New Wifi Card Type: ";
										string wifi;
										getline(cin, wifi);
										if (!ValidNameChars(wifi))
										{
											cout << "Invalid input: Please use only letters, numbers, periods, and apostrophes." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											dynamic_cast<Laptop*>(computers[4])->setWifiCardType(wifi);
											break;
										}
									}
									break;
								}
								case 10: // update cost
								{
									while (true)
									{
										system("cls");
										cout << "Previous Cost: $" << computers[4]->getCost() << endl
											<< "Enter New Cost: $";
										string ct;
										getline(cin, ct);
										if (!ValidMoney(ct))
										{
											cout << "Invalid input: Please use only numbers and maximum of two digits past the decimal." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											computers[4]->setCost(ct);
											break;
										}
									}
									break;
								}
								default: // return to previous menu
									exitUpdateLaptops3 = true;
									break;
								}
							}
							break;
						}
						default:
							exitUpdateLaptop = true;
							break;
						}
					}
					break;
				}
				///////////////////////////////////////////////////////////////////////////////////////////////////
				case 3: // view cell phones - submenu of specific cell phones
				{
					bool exitUpdateCellPhone = false;
					while (!exitUpdateCellPhone)
					{
						valid = false;
						do
						{
							system("cls");
							cout << "Select A Cell Phone To Update: " << endl;
							for (int i = 5; i < 8; i++)
							{
								cout << "\t" << to_string(i - 4) << ") " << computers[i]->getMake() << " " << computers[i]->getModel() << endl;
							}
							cout << "\t4) Return To Update Device Menu" << endl;
							cout << "Your Choice: ";
							getline(cin, input);
							if (input.length() > 0)
							{
								valid = ValidWhole(input, 1, 4);
							}
						} while (!valid);
						switch (input[0] - '0')
						{
						case 1: // update first cell phone
						{
							bool exitUpdateCellPhone1 = false;
							while (!exitUpdateCellPhone1)
							{
								valid = false;
								do
								{
									system("cls");
									cout << "\t~Update Cell Phone Attribute Menu~" << endl << endl
										<< "Select An Attribute To Update: " << endl
										<< "\t1) Make: " << computers[5]->getMake() << endl
										<< "\t2) Model: " << computers[5]->getModel() << endl
										<< "\t3) Processor: " << computers[5]->getProcessorName() << endl
										<< "\t4) Storage: " << computers[5]->getStorageGB() << " GB" << endl
										<< "\t5) RAM: " << computers[5]->getRAM() << " GB" << endl
										<< "\t6) Dimensions: " << dynamic_cast<CellPhone*>(computers[5])->getAllDimensions() << endl
										<< "\t7) Screen Size: " << dynamic_cast<CellPhone*>(computers[5])->getScreenSizeDiag() << " in." << endl
										<< "\t8) SIM Type: " << dynamic_cast<CellPhone*>(computers[5])->getSIM() << endl
										<< "\t9) Price (new): $" << computers[5]->getCost() << endl
										<< "\t10) Return To Update laptops Menu" << endl;
									cout << "Your Choice: ";
									getline(cin, input);
									if (input.length() > 0)
									{
										valid = ValidWhole(input, 1, 10);
									}
								} while (!valid);
								switch (atoi(input.c_str()))
								{
								case 1: // update make
								{
									while (true)
									{
										system("cls");
										cout << "Previous Make: " << computers[5]->getMake() << endl
											<< "Enter New Make: ";
										string make;
										getline(cin, make);
										if (!ValidNameChars(make))
										{
											cout << "Invalid input: Please use only letters, numbers, periods, and apostrophes." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											computers[5]->setMake(make);
											break;
										}
									}
									break;
								}
								case 2: // update model
								{

									while (true)
									{
										system("cls");
										cout << "Previous Model: " << computers[5]->getModel() << endl
											<< "Enter New Model: ";
										string model;
										getline(cin, model);
										if (!ValidNameChars(model))
										{
											cout << "Invalid input: Please use only letters, numbers, periods, and apostrophes." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											computers[5]->setModel(model);
											break;
										}
									}
									break;
								}
								case 3: // update cpu
								{
									while (true)
									{
										system("cls");
										cout << "Previous Processor Name: " << computers[5]->getProcessorName() << endl
											<< "Enter New Processor Name: ";
										string cp;
										getline(cin, cp);
										if (!ValidNameChars(cp))
										{
											cout << "Invalid input: Please use only letters, numbers, periods, and apostrophes." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											computers[5]->setProcessorName(cp);
											break;
										}
									}
									break;
								}
								case 4: // update storage
								{
									while (true)
									{
										system("cls");
										cout << "Previous Storage Amount (GB): " << computers[5]->getStorageGB() << endl
											<< "Enter New Storage Amount (GB): ";
										string gb;
										getline(cin, gb);
										if (!ValidDouble(gb))
										{
											cout << "Invalid input: Please use only valid numbers." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											computers[5]->setStorageGB(gb);
											break;
										}
									}
									break;
								}
								case 5: // update ram
								{
									while (true)
									{
										system("cls");
										cout << "Previous RAM (GB): " << computers[5]->getRAM() << endl
											<< "Enter New RAM (GB): ";
										string rm;
										getline(cin, rm);
										if (!ValidDouble(rm))
										{
											cout << "Invalid input: Please use only valid numbers." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											computers[5]->setRAM(rm);
											break;
										}
									}
									break;
								}
								case 6: // update dimensions
								{
									updateSize(computers[5]);
									break;
								}
								case 7: //update screen size
								{
									while (true)
									{
										system("cls");
										cout << "Previous Screen Size (in.): " << dynamic_cast<CellPhone*>(computers[5])->getScreenSizeDiag() << endl
											<< "Enter New Screen Size (in.): ";
										string ss;
										getline(cin, ss);
										if (!ValidDouble(ss))
										{
											cout << "Invalid input: Please use only valid numbers." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											dynamic_cast<CellPhone*>(computers[5])->setScreenSizeDiag(ss);
											break;
										}
									}
									break;
								}
								case 8: // update SIM
								{
									while (true)
									{
										system("cls");
										cout << "Previous SIM Type: " << dynamic_cast<CellPhone*>(computers[5])->getSIM() << endl
											<< "Enter New SIM Type: ";
										string sim;
										getline(cin, sim);
										if (!ValidNameChars(sim))
										{
											cout << "Invalid input: Please use only letters, numbers, periods, and apostrophes." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											dynamic_cast<CellPhone*>(computers[5])->setSIM(sim);
											break;
										}
									}
									break;
								}
								case 9: // update cost
								{
									while (true)
									{
										system("cls");
										cout << "Previous Cost: $" << computers[5]->getCost() << endl
											<< "Enter New Cost: $";
										string ct;
										getline(cin, ct);
										if (!ValidMoney(ct))
										{
											cout << "Invalid input: Please use only numbers and maximum of two digits past the decimal." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											computers[5]->setCost(ct);
											break;
										}
									}
									break;
								}
								default: // return to previous menu
									exitUpdateCellPhone1 = true;
									break;
								}
							}
							break;
						}
						case 2://update second cell phone
						{
							bool exitUpdateCellPhone2 = false;
							while (!exitUpdateCellPhone2)
							{
								valid = false;
								do
								{
									system("cls");
									cout << "\t~Update Cell Phone Attribute Menu~" << endl << endl
										<< "Select An Attribute To Update: " << endl
										<< "\t1) Make: " << computers[6]->getMake() << endl
										<< "\t2) Model: " << computers[6]->getModel() << endl
										<< "\t3) Processor: " << computers[6]->getProcessorName() << endl
										<< "\t4) Storage: " << computers[6]->getStorageGB() << " GB" << endl
										<< "\t5) RAM: " << computers[6]->getRAM() << " GB" << endl
										<< "\t6) Dimensions: " << dynamic_cast<CellPhone*>(computers[6])->getAllDimensions() << endl
										<< "\t7) Screen Size: " << dynamic_cast<CellPhone*>(computers[6])->getScreenSizeDiag() << " in." << endl
										<< "\t8) SIM Type: " << dynamic_cast<CellPhone*>(computers[6])->getSIM() << endl
										<< "\t9) Price (new): $" << computers[6]->getCost() << endl
										<< "\t10) Return To Update laptops Menu" << endl;
									cout << "Your Choice: ";
									getline(cin, input);
									if (input.length() > 0)
									{
										valid = ValidWhole(input, 1, 10);
									}
								} while (!valid);
								switch (atoi(input.c_str()))
								{
								case 1: // update make
								{
									while (true)
									{
										system("cls");
										cout << "Previous Make: " << computers[6]->getMake() << endl
											<< "Enter New Make: ";
										string make;
										getline(cin, make);
										if (!ValidNameChars(make))
										{
											cout << "Invalid input: Please use only letters, numbers, periods, and apostrophes." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											computers[6]->setMake(make);
											break;
										}
									}
									break;
								}
								case 2: // update model
								{

									while (true)
									{
										system("cls");
										cout << "Previous Model: " << computers[6]->getModel() << endl
											<< "Enter New Model: ";
										string model;
										getline(cin, model);
										if (!ValidNameChars(model))
										{
											cout << "Invalid input: Please use only letters, numbers, periods, and apostrophes." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											computers[6]->setModel(model);
											break;
										}
									}
									break;
								}
								case 3: // update cpu
								{
									while (true)
									{
										system("cls");
										cout << "Previous Processor Name: " << computers[6]->getProcessorName() << endl
											<< "Enter New Processor Name: ";
										string cp;
										getline(cin, cp);
										if (!ValidNameChars(cp))
										{
											cout << "Invalid input: Please use only letters, numbers, periods, and apostrophes." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											computers[6]->setProcessorName(cp);
											break;
										}
									}
									break;
								}
								case 4: // update storage
								{
									while (true)
									{
										system("cls");
										cout << "Previous Storage Amount (GB): " << computers[6]->getStorageGB() << endl
											<< "Enter New Storage Amount (GB): ";
										string gb;
										getline(cin, gb);
										if (!ValidDouble(gb))
										{
											cout << "Invalid input: Please use only valid numbers." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											computers[6]->setStorageGB(gb);
											break;
										}
									}
									break;
								}
								case 5: // update ram
								{
									while (true)
									{
										system("cls");
										cout << "Previous RAM (GB): " << computers[6]->getRAM() << endl
											<< "Enter New RAM (GB): ";
										string rm;
										getline(cin, rm);
										if (!ValidDouble(rm))
										{
											cout << "Invalid input: Please use only valid numbers." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											computers[6]->setRAM(rm);
											break;
										}
									}
									break;
								}
								case 6: // update dimensions
								{
									updateSize(computers[6]);
									break;
								}
								case 7: //update screen size
								{
									while (true)
									{
										system("cls");
										cout << "Previous Screen Size (in.): " << dynamic_cast<CellPhone*>(computers[6])->getScreenSizeDiag() << endl
											<< "Enter New Screen Size (in.): ";
										string ss;
										getline(cin, ss);
										if (!ValidDouble(ss))
										{
											cout << "Invalid input: Please use only valid numbers." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											dynamic_cast<CellPhone*>(computers[6])->setScreenSizeDiag(ss);
											break;
										}
									}
									break;
								}
								case 8: // update SIM
								{
									while (true)
									{
										system("cls");
										cout << "Previous SIM Type: " << dynamic_cast<CellPhone*>(computers[6])->getSIM() << endl
											<< "Enter New SIM Type: ";
										string sim;
										getline(cin, sim);
										if (!ValidNameChars(sim))
										{
											cout << "Invalid input: Please use only letters, numbers, periods, and apostrophes." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											dynamic_cast<CellPhone*>(computers[6])->setSIM(sim);
											break;
										}
									}
									break;
								}
								case 9: // update cost
								{
									while (true)
									{
										system("cls");
										cout << "Previous Cost: $" << computers[6]->getCost() << endl
											<< "Enter New Cost: $";
										string ct;
										getline(cin, ct);
										if (!ValidMoney(ct))
										{
											cout << "Invalid input: Please use only numbers and maximum of two digits past the decimal." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											computers[6]->setCost(ct);
											break;
										}
									}
									break;
								}
								default: // return to previous menu
									exitUpdateCellPhone2 = true;
									break;
								}
							}
							break;
						}
						case 3://update third cell phone
						{
							bool exitUpdateCellPhone3 = false;
							while (!exitUpdateCellPhone3)
							{
								valid = false;
								do
								{
									system("cls");
									cout << "\t~Update Cell Phone Attribute Menu~" << endl << endl
										<< "Select An Attribute To Update: " << endl
										<< "\t1) Make: " << computers[7]->getMake() << endl
										<< "\t2) Model: " << computers[7]->getModel() << endl
										<< "\t3) Processor: " << computers[7]->getProcessorName() << endl
										<< "\t4) Storage: " << computers[7]->getStorageGB() << " GB" << endl
										<< "\t5) RAM: " << computers[7]->getRAM() << " GB" << endl
										<< "\t6) Dimensions: " << dynamic_cast<CellPhone*>(computers[7])->getAllDimensions() << endl
										<< "\t7) Screen Size: " << dynamic_cast<CellPhone*>(computers[7])->getScreenSizeDiag() << " in." << endl
										<< "\t8) SIM Type: " << dynamic_cast<CellPhone*>(computers[7])->getSIM() << endl
										<< "\t9) Price (new): $" << computers[7]->getCost() << endl
										<< "\t10) Return To Update laptops Menu" << endl;
									cout << "Your Choice: ";
									getline(cin, input);
									if (input.length() > 0)
									{
										valid = ValidWhole(input, 1, 10);
									}
								} while (!valid);
								switch (atoi(input.c_str()))
								{
								case 1: // update make
								{
									while (true)
									{
										system("cls");
										cout << "Previous Make: " << computers[7]->getMake() << endl
											<< "Enter New Make: ";
										string make;
										getline(cin, make);
										if (!ValidNameChars(make))
										{
											cout << "Invalid input: Please use only letters, numbers, periods, and apostrophes." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											computers[7]->setMake(make);
											break;
										}
									}
									break;
								}
								case 2: // update model
								{

									while (true)
									{
										system("cls");
										cout << "Previous Model: " << computers[7]->getModel() << endl
											<< "Enter New Model: ";
										string model;
										getline(cin, model);
										if (!ValidNameChars(model))
										{
											cout << "Invalid input: Please use only letters, numbers, periods, and apostrophes." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											computers[7]->setModel(model);
											break;
										}
									}
									break;
								}
								case 3: // update cpu
								{
									while (true)
									{
										system("cls");
										cout << "Previous Processor Name: " << computers[7]->getProcessorName() << endl
											<< "Enter New Processor Name: ";
										string cp;
										getline(cin, cp);
										if (!ValidNameChars(cp))
										{
											cout << "Invalid input: Please use only letters, numbers, periods, and apostrophes." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											computers[7]->setProcessorName(cp);
											break;
										}
									}
									break;
								}
								case 4: // update storage
								{
									while (true)
									{
										system("cls");
										cout << "Previous Storage Amount (GB): " << computers[7]->getStorageGB() << endl
											<< "Enter New Storage Amount (GB): ";
										string gb;
										getline(cin, gb);
										if (!ValidDouble(gb))
										{
											cout << "Invalid input: Please use only valid numbers." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											computers[7]->setStorageGB(gb);
											break;
										}
									}
									break;
								}
								case 5: // update ram
								{
									while (true)
									{
										system("cls");
										cout << "Previous RAM (GB): " << computers[7]->getRAM() << endl
											<< "Enter New RAM (GB): ";
										string rm;
										getline(cin, rm);
										if (!ValidDouble(rm))
										{
											cout << "Invalid input: Please use only valid numbers." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											computers[7]->setRAM(rm);
											break;
										}
									}
									break;
								}
								case 6: // update dimensions
								{
									updateSize(computers[7]);
									break;
								}
								case 7: //update screen size
								{
									while (true)
									{
										system("cls");
										cout << "Previous Screen Size (in.): " << dynamic_cast<CellPhone*>(computers[7])->getScreenSizeDiag() << endl
											<< "Enter New Screen Size (in.): ";
										string ss;
										getline(cin, ss);
										if (!ValidDouble(ss))
										{
											cout << "Invalid input: Please use only valid numbers." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											dynamic_cast<CellPhone*>(computers[7])->setScreenSizeDiag(ss);
											break;
										}
									}
									break;
								}
								case 8: // update SIM
								{
									while (true)
									{
										system("cls");
										cout << "Previous SIM Type: " << dynamic_cast<CellPhone*>(computers[7])->getSIM() << endl
											<< "Enter New SIM Type: ";
										string sim;
										getline(cin, sim);
										if (!ValidNameChars(sim))
										{
											cout << "Invalid input: Please use only letters, numbers, periods, and apostrophes." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											dynamic_cast<CellPhone*>(computers[7])->setSIM(sim);
											break;
										}
									}
									break;
								}
								case 9: // update cost
								{
									while (true)
									{
										system("cls");
										cout << "Previous Cost: $" << computers[7]->getCost() << endl
											<< "Enter New Cost: $";
										string ct;
										getline(cin, ct);
										if (!ValidMoney(ct))
										{
											cout << "Invalid input: Please use only numbers and maximum of two digits past the decimal." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											computers[7]->setCost(ct);
											break;
										}
									}
									break;
								}
								default: // return to previous menu
									exitUpdateCellPhone3 = true;
									break;
								}
							}
							break;
						}
						default:
							exitUpdateCellPhone = true;
							break;
						}
					}
					break;
				}
				///////////////////////////////////////////////////////////////////////////////////////////////////
				case 4: // view tablets - submenu of specific tablets
				{
					bool exitUpdateTablet = false;
					while (!exitUpdateTablet)
					{
						valid = false;
						do
						{
							system("cls");
							cout << "Select A Tablet To Update: " << endl;
							for (int i = 8; i < 10; i++)
							{
								cout << "\t" << to_string(i - 7) << ") " << computers[i]->getMake() << " " << computers[i]->getModel() << endl;
							}
							cout << "\t3) Return To Update Device Menu" << endl;
							cout << "Your Choice: ";
							getline(cin, input);
							if (input.length() > 0)
							{
								valid = ValidWhole(input, 1, 3);
							}
						} while (!valid);
						switch (input[0] - '0')
						{
						case 1: // update first tablet
						{
							bool exitUpdateTablet1 = false;
							while (!exitUpdateTablet1)
							{
								valid = false;
								do
								{
									system("cls");
									cout << "\t~Update Tablet Attribute Menu~" << endl << endl
										<< "Select An Attribute To Update: " << endl
										<< "\t1) Make: " << computers[8]->getMake() << endl
										<< "\t2) Model: " << computers[8]->getModel() << endl
										<< "\t3) Processor: " << computers[8]->getProcessorName() << endl
										<< "\t4) Storage: " << computers[8]->getStorageGB() << " GB" << endl
										<< "\t5) RAM: " << computers[8]->getRAM() << " GB" << endl
										<< "\t6) Screen Size: " << dynamic_cast<Tablet*>(computers[8])->getScreenSizeDiag() << " in." << endl
										<< "\t7) Density: " << dynamic_cast<Tablet*>(computers[8])->getDensityLCD() << " ppi" << endl
										<< "\t8) Has a Mic: " << dynamic_cast<Tablet*>(computers[8])->getHasMic() << endl
										<< "\t9) Price (new): $" << computers[8]->getCost() << endl
										<< "\t10) Return To Update Tablets Menu" << endl;
									cout << "Your Choice: ";
									getline(cin, input);
									if (input.length() > 0)
									{
										valid = ValidWhole(input, 1, 10);
									}
								} while (!valid);
								switch (atoi(input.c_str()))
								{
								case 1: // update make
								{
									while (true)
									{
										system("cls");
										cout << "Previous Make: " << computers[8]->getMake() << endl
											<< "Enter New Make: ";
										string make;
										getline(cin, make);
										if (!ValidNameChars(make))
										{
											cout << "Invalid input: Please use only letters, numbers, periods, and apostrophes." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											computers[8]->setMake(make);
											break;
										}
									}
									break;
								}
								case 2: // update model
								{

									while (true)
									{
										system("cls");
										cout << "Previous Model: " << computers[8]->getModel() << endl
											<< "Enter New Model: ";
										string model;
										getline(cin, model);
										if (!ValidNameChars(model))
										{
											cout << "Invalid input: Please use only letters, numbers, periods, and apostrophes." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											computers[8]->setModel(model);
											break;
										}
									}
									break;
								}
								case 3: // update cpu
								{
									while (true)
									{
										system("cls");
										cout << "Previous Processor Name: " << computers[8]->getProcessorName() << endl
											<< "Enter New Processor Name: ";
										string cp;
										getline(cin, cp);
										if (!ValidNameChars(cp))
										{
											cout << "Invalid input: Please use only letters, numbers, periods, and apostrophes." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											computers[8]->setProcessorName(cp);
											break;
										}
									}
									break;
								}
								case 4: // update storage
								{
									while (true)
									{
										system("cls");
										cout << "Previous Storage Amount (GB): " << computers[8]->getStorageGB() << endl
											<< "Enter New Storage Amount (GB): ";
										string gb;
										getline(cin, gb);
										if (!ValidDouble(gb))
										{
											cout << "Invalid input: Please use only valid numbers." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											computers[8]->setStorageGB(gb);
											break;
										}
									}
									break;
								}
								case 5: // update ram
								{
									while (true)
									{
										system("cls");
										cout << "Previous RAM (GB): " << computers[8]->getRAM() << endl
											<< "Enter New RAM (GB): ";
										string rm;
										getline(cin, rm);
										if (!ValidDouble(rm))
										{
											cout << "Invalid input: Please use only valid numbers." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											computers[8]->setRAM(rm);
											break;
										}
									}
									break;
								}
								case 6: // update screen size
								{
									while (true)
									{
										system("cls");
										cout << "Previous Screen Size (in.): " << dynamic_cast<Tablet*>(computers[8])->getScreenSizeDiag() << endl
											<< "Enter New Screen Size (in.): ";
										string ss;
										getline(cin, ss);
										if (!ValidDouble(ss))
										{
											cout << "Invalid input: Please use only valid numbers." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											dynamic_cast<Tablet*>(computers[8])->setScreenSizeDiag(ss);
											break;
										}
									}
									break;
								}
								case 7: //update density
								{
									while (true)
									{
										system("cls");
										cout << "Previous Denisty (ppi): " << dynamic_cast<Tablet*>(computers[8])->getDensityLCD() << endl
											<< "Enter New Density (ppi): ";
										string den;
										getline(cin, den);
										if (!ValidDouble(den))
										{
											cout << "Invalid input: Please use only valid numbers." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											dynamic_cast<Tablet*>(computers[8])->setDensityLCD(den);
											break;
										}
									}
									break;
								}
								case 8: //update has mic
								{
									while (true)
									{
										system("cls");
										cout << "Previous Value for \'Device Has Mic\': " << dynamic_cast<Tablet*>(computers[8])->getHasMic() << endl
											<< "Enter New Value for \'Device Has Mic\': ";
										string mic;
										getline(cin, mic);
										if (!ValidYN(mic))
										{
											cout << "Invalid input: Please use only Y or N." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											dynamic_cast<Tablet*>(computers[8])->setHasMic(mic);
											break;
										}
									}
									break;
								}
								case 9: // update cost
								{
									while (true)
									{
										system("cls");
										cout << "Previous Cost: $" << dynamic_cast<Tablet*>(computers[8])->getCost() << endl
											<< "Enter New Cost: $";
										string ct;
										getline(cin, ct);
										if (!ValidMoney(ct))
										{
											cout << "Invalid input: Please use only numbers and maximum of two digits past the decimal." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											dynamic_cast<Tablet*>(computers[8])->setCost(ct);
											break;
										}
									}
									break;
								}
								default: // return to previous menu
									exitUpdateTablet1 = true;
									break;
								}
							}
							break;
						}
						case 2: // update tablet 2
						{
							bool exitUpdateTablet2 = false;
							while (!exitUpdateTablet2)
							{
								valid = false;
								do
								{
									system("cls");
									cout << "\t~Update Tablet Attribute Menu~" << endl << endl
										<< "Select An Attribute To Update: " << endl
										<< "\t1) Make: " << computers[9]->getMake() << endl
										<< "\t2) Model: " << computers[9]->getModel() << endl
										<< "\t3) Processor: " << computers[9]->getProcessorName() << endl
										<< "\t4) Storage: " << computers[9]->getStorageGB() << " GB" << endl
										<< "\t5) RAM: " << computers[9]->getRAM() << " GB" << endl
										<< "\t6) Screen Size: " << dynamic_cast<Tablet*>(computers[9])->getScreenSizeDiag() << " in." << endl
										<< "\t7) Density: " << dynamic_cast<Tablet*>(computers[9])->getDensityLCD() << " ppi" << endl
										<< "\t8) Has a Mic: " << dynamic_cast<Tablet*>(computers[9])->getHasMic() << endl
										<< "\t9) Price (new): $" << computers[9]->getCost() << endl
										<< "\t10) Return To Update Tablets Menu" << endl;
									cout << "Your Choice: ";
									getline(cin, input);
									if (input.length() > 0)
									{
										valid = ValidWhole(input, 1, 10);
									}
								} while (!valid);
								switch (atoi(input.c_str()))
								{
								case 1: // update make
								{
									while (true)
									{
										system("cls");
										cout << "Previous Make: " << computers[9]->getMake() << endl
											<< "Enter New Make: ";
										string make;
										getline(cin, make);
										if (!ValidNameChars(make))
										{
											cout << "Invalid input: Please use only letters, numbers, periods, and apostrophes." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											computers[9]->setMake(make);
											break;
										}
									}
									break;
								}
								case 2: // update model
								{

									while (true)
									{
										system("cls");
										cout << "Previous Model: " << computers[9]->getModel() << endl
											<< "Enter New Model: ";
										string model;
										getline(cin, model);
										if (!ValidNameChars(model))
										{
											cout << "Invalid input: Please use only letters, numbers, periods, and apostrophes." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											computers[9]->setModel(model);
											break;
										}
									}
									break;
								}
								case 3: // update cpu
								{
									while (true)
									{
										system("cls");
										cout << "Previous Processor Name: " << computers[9]->getProcessorName() << endl
											<< "Enter New Processor Name: ";
										string cp;
										getline(cin, cp);
										if (!ValidNameChars(cp))
										{
											cout << "Invalid input: Please use only letters, numbers, periods, and apostrophes." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											computers[9]->setProcessorName(cp);
											break;
										}
									}
									break;
								}
								case 4: // update storage
								{
									while (true)
									{
										system("cls");
										cout << "Previous Storage Amount (GB): " << computers[9]->getStorageGB() << endl
											<< "Enter New Storage Amount (GB): ";
										string gb;
										getline(cin, gb);
										if (!ValidDouble(gb))
										{
											cout << "Invalid input: Please use only valid numbers." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											computers[9]->setStorageGB(gb);
											break;
										}
									}
									break;
								}
								case 5: // update ram
								{
									while (true)
									{
										system("cls");
										cout << "Previous RAM (GB): " << computers[9]->getRAM() << endl
											<< "Enter New RAM (GB): ";
										string rm;
										getline(cin, rm);
										if (!ValidDouble(rm))
										{
											cout << "Invalid input: Please use only valid numbers." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											computers[9]->setRAM(rm);
											break;
										}
									}
									break;
								}
								case 6: // update screen size
								{
									while (true)
									{
										system("cls");
										cout << "Previous Screen Size (in.): " << dynamic_cast<Tablet*>(computers[9])->getScreenSizeDiag() << endl
											<< "Enter New Screen Size (in.): ";
										string ss;
										getline(cin, ss);
										if (!ValidDouble(ss))
										{
											cout << "Invalid input: Please use only valid numbers." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											dynamic_cast<Tablet*>(computers[9])->setScreenSizeDiag(ss);
											break;
										}
									}
									break;
								}
								case 7: //update density
								{
									while (true)
									{
										system("cls");
										cout << "Previous Denisty (ppi): " << dynamic_cast<Tablet*>(computers[9])->getDensityLCD() << endl
											<< "Enter New Density (ppi): ";
										string den;
										getline(cin, den);
										if (!ValidDouble(den))
										{
											cout << "Invalid input: Please use only valid numbers." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											dynamic_cast<Tablet*>(computers[9])->setDensityLCD(den);
											break;
										}
									}
									break;
								}
								case 8: //update has mic
								{
									while (true)
									{
										system("cls");
										cout << "Previous Value for \'Device Has Mic\': " << dynamic_cast<Tablet*>(computers[9])->getHasMic() << endl
											<< "Enter New Value for \'Device Has Mic\': ";
										string mic;
										getline(cin, mic);
										if (!ValidYN(mic))
										{
											cout << "Invalid input: Please use only Y or N." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											dynamic_cast<Tablet*>(computers[9])->setHasMic(mic);
											break;
										}
									}
									break;
								}
								case 9: // update cost
								{
									while (true)
									{
										system("cls");
										cout << "Previous Cost: $" << dynamic_cast<Tablet*>(computers[9])->getCost() << endl
											<< "Enter New Cost: $";
										string ct;
										getline(cin, ct);
										if (!ValidMoney(ct))
										{
											cout << "Invalid input: Please use only numbers and maximum of two digits past the decimal." << endl
												<< "Press any key to try again." << endl;
											_getch();
											continue;
										}
										else
										{
											dynamic_cast<Tablet*>(computers[9])->setCost(ct);
											break;
										}
									}
									break;
								}
								default: // return to previous menu
									exitUpdateTablet2 = true;
									break;
								}
							}
							break;
						}
						default:
							exitUpdateTablet = true;
							break;
						}
					}
					}
				default:
					exitUpdate = true;
					break;
				}
			} // end of while loop for viewing devices
			break;
		}
		case 3: // save to file and exit choice
		{
			exit = true; // set bool flag so program exits main loop
			break;
		}
		default:
		{
			break;
		}
		} // end bracket for outer switch of  main menu
	} // end bracket for main while loop


	////// write to file here /////
	try
	{
		ofstream writer;
		writer.open("C:/Program Files/cpp/Resources/Computers.dat");
		if (writer.is_open())
		{
			for each(Computer * c in computers)
				writer << c->getFileString() << '\n';
		}
		writer.close();
		cout << "File updated successfully. Press any key to exit." << endl;
	}
	catch (exception ex)
	{
		cout << "Error: " << ex.what() << endl << endl
			<< "Press any key to exit." << endl;
	}
	for each(Computer * c in computers)
		delete c;
	_getch();
	return 0;
}

void updateSize(Computer * cell) {
	//this is supposed to use a nested while loop to get all these things validated and set one after the other.
	while (true)
	{
		system("cls");
		cout << "Previous Length (in.): " << dynamic_cast<CellPhone*>(cell)->getDimensionL() << endl
			<< "Enter New Length (in.): ";
		string dimensionL;
		getline(cin, dimensionL);
		if (!ValidDouble(dimensionL))
		{
			cout << "Invalid input: Please use only valid numbers." << endl
				<< "Press any key to try again." << endl;
			_getch();
			continue;
		}
		else
		{
			while (true)
			{
				system("cls");
				cout << "Previous Width (in.): " << dynamic_cast<CellPhone*>(cell)->getDimensionW() << endl
					<< "Enter New Width (in.): ";
				string dimensionW;
				getline(cin, dimensionW);
				if (!ValidDouble(dimensionW))
				{
					cout << "Invalid input: Please use only valid numbers." << endl
						<< "Press any key to try again." << endl;
					_getch();
					continue;
				}
				else
				{
					while (true)
					{
						system("cls");
						cout << "Previous Height (in.): " << dynamic_cast<CellPhone*>(cell)->getDimensionH() << endl
							<< "Enter New Height (in.): ";
						string dimensionH;
						getline(cin, dimensionH);
						if (!ValidDouble(dimensionH))
						{
							cout << "Invalid input: Please use only valid numbers." << endl
								<< "Press any key to try again." << endl;
							_getch();
							continue;
						}
						else
						{
							dynamic_cast<CellPhone*>(cell)->setDimensions(dimensionL, dimensionW, dimensionH);
							return;
						}
					}
				}
			}
		}
	}
}
