// include related specification
#include "Liquid.h"


void LiquidOps::KillLiquids()
{
	// method to clean up any memory allocated at run-time
	while (top != NULL)
	{
		// create a new pointer, shift the top pointer, and delete each link
		Liquid * decimation = top;
		top = top->ptr;
		delete decimation;
	}
}

void LiquidOps::LoadLiquids()
{
	// method to load all of the information from a file dynamically into memory using a linked list
	top = NULL;
	try
	{
		// user an input file stream reader to access the data file
		ifstream reader;
		reader.open(filePath);
		if (!reader.good())
			throw runtime_error("No Liquids File");
		string s;
		while (getline(reader, s))
		{
			// read each line that exists in the file and split the string into tokens using the comma delimiter
			string token = "";
			string tokens[5];
			int delimPlace;
			for (int i = 0; i < 4; i++)
			{
				// loop through the string, find the first comma, place token in array of strings, update string
				delimPlace = s.find_first_of(',');
				tokens[i] = s.substr(0, delimPlace);
				s = s.substr(delimPlace + 1);
			}
			// set last token (not followed by a comma) after loop
			tokens[4] = s;
			// dynamically allocate memory for a new member of the linked list and assign it's values from the last token array
			Liquid * newLiquid = new Liquid;
			newLiquid->name = tokens[0];
			newLiquid->molecularFormula = tokens[1];
			newLiquid->freezingPoint_C = tokens[2];
			newLiquid->boilingPoint_C = tokens[3];
			newLiquid->viscosity_mPas = tokens[4];
			// point new token to the current top link
			newLiquid->ptr = top;
			// assign new liquid to the top link spot
			top = newLiquid;
		}
		// close file reader
		reader.close();
	}
	catch (exception &ex)
	{
		// display message at top of running program if there is no file with information found
		cout << "** the lab is currently out of stock ** please consider restocking it **" << endl << endl;
	}
}

void LiquidOps::WriteLiquidsToFile()
{
	// method to write any changes to the data file or create a new one if it doesn't exist
	try
	{
		// use a output file stream writer to create or replace the data file
		ofstream writer;
		writer.open(filePath);
		// create a pointer to a liquid, and set it to TOP to access the linked list information
		Liquid * tempLiquid = top;
		while (tempLiquid != NULL)
		{
			// until the last link is found, loop through each liquid and write its info to the file
			string s = tempLiquid->name + "," + tempLiquid->molecularFormula + "," + tempLiquid->freezingPoint_C + "," + tempLiquid->boilingPoint_C + "," + tempLiquid->viscosity_mPas + "\n";
			writer.write(s.c_str(), s.size());
			tempLiquid = tempLiquid->ptr;
		}
		// close the writer
		writer.close();
		cout << "Your changes are saved! Goodbye!" << endl;
	}
	catch (exception &ex)
	{
		cout << "File Error" << endl;
	}
}

void LiquidOps::ViewLiquids()
{
	// method to view all the liquid information in an organized fashion

	// create a pointer to the top liquid link
	Liquid * tempLiquid = top;
	while (tempLiquid != NULL)
	{
		// loop through the linked list, for each link display all liquid info
		cout << setw(20) << "Liquid Common Name: " << tempLiquid->name << endl
			<< setw(20) << "Molecular Formula: " << tempLiquid->molecularFormula << endl
			<< setw(20) << "Freezing Point: " << tempLiquid->freezingPoint_C << char(248) << "C" << endl
			<< setw(20) << "Boiling Point: " << tempLiquid->boilingPoint_C << char(248) << "C" << endl
			<< setw(16) << "Viscosity At 25" << char(248) << "C: " << tempLiquid->viscosity_mPas << " mPa" << char(249) << "s" << endl << endl;
		// point to next link
		tempLiquid = tempLiquid->ptr;
	}
}

void LiquidOps::AlterLiquid()
{
	// method to alter the individual characteristics of a specific liquid chosen by the user
	string input;
	// create a pointer to a liquid to access the linked list
	Liquid * tempLiquid;
	int ctr;
	bool valid;
	bool inAlterMenu = true;
	while (inAlterMenu)
	{
		// loop until user selects to go back to main menu
		while (true)
		{
			// loop until use has selected a valid liquid choice
			// assign pointer to top to access linked list
			tempLiquid = top;
			cout << "Select a liquid to alter:" << endl;
			ctr = 1;
			while (tempLiquid != NULL)
			{
				// loop through linked list using counter to set up dynamic menu
				cout << "\t" << ctr++ << ") " << tempLiquid->name << endl;
				tempLiquid = tempLiquid->ptr;
			}
			// last menu item is to return to main menu
			cout << "\t" << ctr << ") " << "Return to main menu";
			cout << endl << "Your selection: ";
			getline(cin, input);
			if (input.length() != 1 || input[0] < '1' || input[0] > (char)((ctr)+48))
			{
				// if not valid choice, refresh menu and make user try again
				system("cls");
				cout << "Invalid input, try again." << endl << endl;
				continue;
			}
			// breaks once user has entered a valid response to menu choice
			break;
		}
		// set integer to find number of link to make changes to
		int alterPoint = input[0] - 48;
		tempLiquid = top;

		if (alterPoint == ctr)
		{
			// if user selected to return to main menu, change flag to false to exit main alter loop
			inAlterMenu = false;
		}
		else
		{
			// runs if user has selected a liquid to alter
			for (int i = 1; i < alterPoint; i++)
			{
				// find link they have chosen to alter in linked list
				tempLiquid = tempLiquid->ptr;
			}
			bool alteringCurrent = true;
			while (alteringCurrent)
			{
				// loops until the user is done altering current liquid's information
				system("cls");
				cout << "Select an attribute to alter:" << endl
					<< "\t1) Liquid Common Name: " << tempLiquid->name << endl
					<< "\t2) Molecular Formula: " << tempLiquid->molecularFormula << endl
					<< "\t3) Freezing Point: " << tempLiquid->freezingPoint_C << char(248) << "C" << endl
					<< "\t4) Boiling Point: " << tempLiquid->boilingPoint_C << char(248) << "C" << endl
					<< "\t5) Viscosity At 25" << char(248) << "C: " << tempLiquid->viscosity_mPas << " mPa" << char(249) << "s" << endl
					<< "\t6) Back to Alter Liquid Menu" << endl
					<< "Your selection: ";
				getline(cin, input);
				if (input.length() != 1 || input[0] < '1' || input[0] > '6')
				{
					// if invalid menu choice, refresh menu and make user try again
					continue;
				}
				system("cls");
				// test user input of menu choice for attribute to alter
				switch (input[0] - 48)
				{
				// user has selected to alter the liquid's name
				case 1:
				{
					valid = false;
					while (!valid)
					{
						// let user see current and update
						cout << "Current Name: " << tempLiquid->name << endl;
						cout << "Enter New Name: ";
						getline(cin, input);
						valid = val.ValidName(input);
						if (!valid)
						{
							system("cls");
							cout << "Invalid name entered. It must only contain letters, numbers or the following symbols:  .  '  \"  -  " << endl << endl;
						}
					} 
					// set liquid name to input that has been validated
					tempLiquid->name = input;
					break;
				}
				// user has selected to alter the liquid's molecular formula
				case 2:
				{
					valid = false;
					while (!valid)
					{
						// let user see current and update
						cout << "Current Molecular Formula: " << tempLiquid->molecularFormula << endl;
						cout << "Enter New Molecular Formula: ";
						getline(cin, input);
						valid = val.ValidFormula(input);
						if (!valid)
						{
							system("cls");
							cout << "Invalid molecular formula entered. It must only contain at least one letter. Only letters and numbers allowed." << endl << endl;
						}
					}
					// set liquid formula to input that has been validated
					tempLiquid->molecularFormula = input;
					break;
				}
				// user has selected to alter the liquid's freezing point
				case 3:
				{
					valid = false;
					while (!valid)
					{
						// let user see current and update
						cout << "Current Freezing Point (" << char(248) << "C): " << tempLiquid->freezingPoint_C << endl;
						cout << "Enter New Freezing Point (" << char(248) << "C): ";
						getline(cin, input);
						valid = val.ValidReal(input);
						if (!valid)
						{
							system("cls");
							cout << "Invalid freezing point entered. It must only contain real numbers." << endl << endl;
						}
					}
					// set liquid freezing point to input that has been validated
					tempLiquid->freezingPoint_C = input;
					break;
				}
				// user has selected to alter the liquid's boiling point
				case 4:
				{
					valid = false;
					while (!valid)
					{
						// let user see current and update
						cout << "Current Boiling Point (" << char(248) << "C): " << tempLiquid->boilingPoint_C << endl;
						cout << "Enter New Boiling Point (" << char(248) << "C): ";
						getline(cin, input);
						valid = val.ValidReal(input);
						if (!valid)
						{
							system("cls");
							cout << "Invalid boiling point entered. It must only contain real numbers." << endl << endl;
						}
					}
					// set liquid boiling point to input that has been validated
					tempLiquid->boilingPoint_C = input;
					break;
				}
				// user has selected to alter the liquid's viscocity at room temperature
				case 5:
				{
					valid = false;
					while (!valid)
					{
						// let user see current and update
						cout << "Current Viscosity at 25" << char(248) << "C (mPa" << char(249) << "s): " << tempLiquid->viscosity_mPas << endl;
						cout << "Enter New Viscosity at 25" << char(248) << "C (mPa" << char(249) << "s): ";
						getline(cin, input);
						valid = val.ValidPositiveReal(input);
						if (!valid)
						{
							system("cls");
							cout << "Invalid viscosity entered. It must only contain positive real numbers." << endl << endl;
						}
					}
					// set liquid viscosity to input that has been validated
					tempLiquid->viscosity_mPas = input;
					break;
				}
				// user has selected to finish altering current liquid
				case 6:
					alteringCurrent = false;
					break;
				}
			}

		}
	}
}

void LiquidOps::AddLiquid()
{
	// method to add a new liquid
	// create temporary pointer to hold new info
	Liquid * tempLiquid = new Liquid;
	string input = "";
	bool valid = false;
	while (!valid)
	{
		// loop until a valid input has been entered
		cout << "Enter Liquid Name: ";
		getline(cin, input);
		valid = val.ValidName(input);
		if (!valid)
		{
			cout << "Invalid name entered. It must only contain letters, numbers and the following symbols:  .  '  \"  -  " << endl;
		}
	}
	// set liquid name to input that has been validated
	tempLiquid->name = input;

	valid = false;
	while (!valid)
	{
		// loop until a valid input has been entered
		cout << "Enter Molecular Formula: ";
		getline(cin, input);
		valid = val.ValidFormula(input);
		if (!valid)
		{
			cout << "Invalid molecular formula entered. It must only contain at least one letter. Only letters and numbers allowed." << endl;
		}
	}
	// set liquid formula to input that has been validated
	tempLiquid->molecularFormula = input;

	valid = false;
	while (!valid)
	{
		// loop until a valid input has been entered
		cout << "Enter Freezing Point (" << char(248) << "C): ";
		getline(cin, input);
		valid = val.ValidReal(input);
		if (!valid)
		{
			cout << "Invalid freezing point entered. It must only contain real numbers." << endl;
		}
	}
	// set liquid freezing point to input that has been validated
	tempLiquid->freezingPoint_C = input;

	valid = false;
	while (!valid)
	{
		// loop until a valid input has been entered
		cout << "Enter Boiling Point (" << char(248) << "C): ";
		getline(cin, input);
		valid = val.ValidReal(input);
		if (!valid)
		{
			cout << "Invalid boiling point entered. It must only contain real numbers." << endl;
		}
	}
	// set liquid boiling point to input that has been validated
	tempLiquid->boilingPoint_C = input;

	valid = false;
	while (!valid)
	{
		// loop until a valid input has been entered
		cout << "Enter Viscosity at 25" << char(248) << "C (mPa" << char(249) << "s): ";
		getline(cin, input);
		valid = val.ValidPositiveReal(input);
		if (!valid)
		{
			cout << "Invalid viscosity entered. It must only contain positive real numbers." << endl;
		}
	}

	// set liquid viscosity to input that has been validated
	tempLiquid->viscosity_mPas = input;
	// set pointer to top link
	tempLiquid->ptr = top;
	// set top link to current temp object
	top = tempLiquid;
	// clear screen and display success message
	system("cls");
	cout << "Liquid \"" << top->name << "\" Added" << endl << endl;
}

void LiquidOps::RemoveLiquid()
{
	// method to remove a liquid
	string input;
	Liquid * tempLiquid;
	int ctr;
	while (true)
	{
		// loop unil user picks a liquid to remove or chooses to exit
		tempLiquid = top;
		// generate removal menu from linked list
		cout << "Select a liquid to remove:" << endl;
		ctr = 1;
		while (tempLiquid != NULL)
		{
			// loop until all list displayed
			cout << "\t" << ctr++ << ") " << tempLiquid->name << endl;
			tempLiquid = tempLiquid->ptr;
		}
		cout << "\t" << ctr << ") " << "Return to main menu";
		cout << endl << "Your selection: ";
		getline(cin, input);
		if (input.length() != 1 || input[0] < '1' || input[0] > (char)((ctr)+48))
		{
			// make user redo input if invalid
			system("cls");
			cout << "Invalid input, try again." << endl << endl;
			continue;
		}
		break;
	}
	// set removal point based on user input
	int removePoint = input[0] - 48;
	// set temp to top link
	tempLiquid = top;

	if (removePoint == ctr)
	{
		// if user selected to exit this menu, end method
		return;
	}
	else if (removePoint == 1)
	{
		// if user selects top as removal point, set pointer and delete
		top = top->ptr;
		delete tempLiquid;
	}
	else
	{
		// otherwise loop to one less than point of removal, have that object point two over, move once more, then delete :) that's all
		for (int i = 2; i < removePoint; i++)
		{
			tempLiquid = tempLiquid->ptr;
		}
		Liquid * decimate = tempLiquid->ptr;
		tempLiquid->ptr = tempLiquid->ptr->ptr;
		delete decimate;
	}
}