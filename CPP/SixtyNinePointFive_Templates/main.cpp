// Jeremy Stones
// Marshall Ellison
// Janelle King

// Team: Sixty-Nine Point Five

// Lab over Polymorphism

// Program Specification:
// This program is made to let a user set up and save vending machine configurations as well as
// stock and test the machines.  All machine information is saved to file, so machines created
// will persist when the program is opened again.  

// include file at the head of the file organization system (includes all other headers as
// well as the core menu content of main()
#include "Operations.h"

template<class crazy>
bool WriteFileOut(crazy * &top);
template<class crazy2>
bool ReadFileIn(crazy2 * &top);

int main()
{
	// program starts here, this is really just the shell of the program
	// the first and second tier of the menu system are found in Operations.cpp
	// Further menus are located in the overloaded functions of the derived vending classes

	// hide cursor
	HideCursor(true);

	// open mp3 file for background music
	mciSendString("open \"Sounds\\backgroundmusic.mp3\" type mpegvideo alias bgmusic", NULL, 0, NULL);
	// play file
	mciSendString("play bgmusic from 0 repeat", NULL, 0, NULL);
	// show welcome screen
	welcomeScreen();
	// music pauses upon entry to main menu
	mciSendString("pause bgmusic", NULL, 0, NULL);
	bool playingbg = false;

	// create a head to the linked list for Vending Machines
	VendingMachine * top = NULL;

	int selection;
	bool running = true;
	// read contents from data file into the linked list
	ReadFileIn(top);

	while (running)
	{
			// welcome screen and main menu
			selection = ShowMainMenu();
		switch (selection)
		{
			// functions of switch found in Operations.cpp
		case 1:
			CreateMachine(top);
			break;
		case 2:
			LoadMachine(top);
			break;
		case 3:
			SetPricing(top);
			break;
		case 4:
			StartMachine(top);
			break;
		case 5:
			GenerateInventoryReport(top);
			break;
		case 6:
			// this case is for toggling the background music (resume/pause)
			if (playingbg)
			{
				mciSendString("pause bgmusic ", NULL, 0, NULL);
				playingbg = false;
			}
			else
			{
				mciSendString("resume bgmusic", NULL, 0, NULL);
				playingbg = true;
			}
			break;
		default:
			// end program option
			running = false;
			break;
		}
	}

	// write contents of linked list to file
	WriteFileOut(top);

	// delete items of linked list
	VendingMachine * vendTemp = top;
	while (vendTemp != NULL)
	{
		// loop until the pointer is null

		// set the top equal to the pointer it's pointing to
		top = top->getNextPtr();
		// delete the vending machine 
		delete vendTemp;
		// move temp vending machine back to current top
		vendTemp = top;
	}
	// close mp3 file before ending program
	mciSendString("close bgmusic", NULL, 0, NULL);
	return 0;
}


template<class crazy>
bool WriteFileOut(crazy * &top)
{
	// This writes out all of the data for the vending machine to the file, machines.dat

	// create stream writer and temporary Machine for file writing
	fstream writer;
	crazy * tempMachine = top;

	try
	{
		// open file
		writer.open("Data\\machines.dat", ios::binary | ios::out | ios::trunc);

		while (tempMachine != NULL)
		{
			int typeInt = 0;
			// loop until last machine
			if (tempMachine->vendingType == "BottledBev")
			{
				// is the machine type is a bottled beverage machine, write a 1 and then the object
				typeInt = 1;
				writer.write(reinterpret_cast<char*>(&typeInt), sizeof(int));
				writer.write(reinterpret_cast<char*>(tempMachine), sizeof(VendingBottledBev));
			}
			else if (tempMachine->vendingType == "Fountain")
			{
				// is the machine type is a fountain machine, write a 2 and then the object
				typeInt = 2;
				writer.write(reinterpret_cast<char*>(&typeInt), sizeof(int));
				writer.write(reinterpret_cast<char*>(tempMachine), sizeof(VendingFountain));
			}
			else if (tempMachine->vendingType == "PrepackSnack")
			{
				// is the machine type is a prepacked snack machine, write a 3 and then the object
				typeInt = 3;
				writer.write(reinterpret_cast<char*>(&typeInt), sizeof(int));
				writer.write(reinterpret_cast<char*>(tempMachine), sizeof(VendingPrepackSnack));
			}
			else
				break;
			// go to next pointer
			tempMachine = tempMachine->getNextPtr();
		}
		// close file for writing
		writer.close();
		// end successful
		return true;
	}
	catch (exception &ex)
	{
		// give error message if file cannot write
		cout << "Something went wrong. Error: " << ex.what() << endl << "Press any key to exit the program.";
		_getch();
		// end fail
		return false;
	}
}
template<class crazy2>
bool ReadFileIn(crazy2 * &top)
{
	// method to read all contents of file into the program's linked list of vending machines

	// create input file stream reader
	fstream reader;

	try
	{
		// open reader and test for a good file connection

		reader.open("Data\\machines.dat", ios::binary | ios::in);
		// end if error
		if (!reader.good())
			throw runtime_error("No File");
		int typeInt;
		reader.read(reinterpret_cast<char*>(&typeInt), sizeof(int));
		while (!reader.eof())
		{
			// while lines can still be read continue processing data

			//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			////////////////////																					//////////////////////		
			//                      Virtual functions within an object hold memory information that is only								//
			//						meant to exist within the runtime which it was created; therefore, the								//
			//						object pulled directly from the binary file (holding the original binary)							//
			//						will not be able to be used in a virtual capacity, because of the incorrect							//
			//						memory information.  A second object is used here to transfer all of the							//
			//						original pieces of data from the first object.  I then use the second object,						//
			//						which holds memory information from the current runtime instance, with the							//
			//						linked list.  It is able to perform the necessary virtual functions.								//
			//                      There's more to this, because the program still breaks when the program is							//
			//						recompiled and then tries to read the same binary file. Many more things to							//
			//						learn ;)																							//
			/////////////////////																					//////////////////////
			//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

			// create temp machine for setting to linked list
			crazy2 * vendTemp; // temp object A

							   // create extra object to store attributes of first object
			crazy2 * vendForLinkedList; // temp object B

			if (typeInt == 1)
			{
				// how to add to linked list for bottled beverage machines

				// instantiate 'new' BottledBev machines in both temp objects
				vendTemp = new VendingBottledBev;
				vendForLinkedList = new VendingBottledBev;

				// read in the file object into temp object A - A not holds both attributes and corrupt memory information of virtual functions
				reader.read(reinterpret_cast<char*>(vendTemp), sizeof(VendingBottledBev));

				// transfer all attributes from object A to object B - B still has the virtual memory info from this runtime

				// class attributes
				vendForLinkedList->setBalance(vendTemp->getBalance());
				vendForLinkedList->setName(vendTemp->getName());
				vendForLinkedList->setNumProducts(vendTemp->getNumProducts());
				for (int i = 0; i < 10; i++)
				{
					// product attributes from within array of products in class object
					vendForLinkedList->product[i].amtOnHand = vendTemp->product[i].amtOnHand;
					vendForLinkedList->product[i].amtSold = vendTemp->product[i].amtSold;
					vendForLinkedList->product[i].productId = vendTemp->product[i].productId;
					vendForLinkedList->product[i].productName = vendTemp->product[i].productName;
					vendForLinkedList->product[i].salesPrice = vendTemp->product[i].salesPrice;
				}
			}
			else if (typeInt == 2)
			{
				// how to add to linked list for fountain machines

				// instantiate 'new' Fountain machines in both temp objects
				vendTemp = new VendingFountain;
				vendForLinkedList = new VendingFountain;

				// read in the file object into temp object A - A not holds both attributes and corrupt memory information of virtual functions
				reader.read(reinterpret_cast<char*>(vendTemp), sizeof(VendingFountain));

				// transfer all attributes from object A to object B - B still has the virtual memory info from this runtime

				// base class attributes
				vendForLinkedList->setBalance(vendTemp->getBalance());
				vendForLinkedList->setName(vendTemp->getName());
				vendForLinkedList->setNumProducts(vendTemp->getNumProducts());

				// derived class specific attributes
				dynamic_cast<VendingFountain*>(vendForLinkedList)->setQtyLgCups_32oz(dynamic_cast<VendingFountain*>(vendTemp)->getQtyLgCups_32oz());
				dynamic_cast<VendingFountain*>(vendForLinkedList)->setQtyMedCups_20oz(dynamic_cast<VendingFountain*>(vendTemp)->getQtyMedCups_20oz());
				dynamic_cast<VendingFountain*>(vendForLinkedList)->setQtySmCups_10oz(dynamic_cast<VendingFountain*>(vendTemp)->getQtySmCups_10oz());
				for (int i = 0; i < 10; i++)
				{
					// bass class product attributes
					vendForLinkedList->product[i].amtOnHand = vendTemp->product[i].amtOnHand;
					vendForLinkedList->product[i].amtSold = vendTemp->product[i].amtSold;
					vendForLinkedList->product[i].productId = vendTemp->product[i].productId;
					vendForLinkedList->product[i].productName = vendTemp->product[i].productName;
					vendForLinkedList->product[i].salesPrice = vendTemp->product[i].salesPrice;

					// derived class specific product attributes
					(dynamic_cast<VendingFountain*>(vendForLinkedList))->setLargeCupsSold(i, (dynamic_cast<VendingFountain*>(vendTemp))->getLargeCupsSold(i));
					(dynamic_cast<VendingFountain*>(vendForLinkedList))->setMediumCupsSold(i, (dynamic_cast<VendingFountain*>(vendTemp))->getMediumCupsSold(i));
					(dynamic_cast<VendingFountain*>(vendForLinkedList))->setSmallCupsSold(i, (dynamic_cast<VendingFountain*>(vendTemp))->getSmallCupsSold(i));
				}

			}
			else if (typeInt == 3)
			{
				// how to add to linked list for prepacked snack machines

				// instantiate 'new' prepacked Snack machines in both temp objects
				vendTemp = new VendingPrepackSnack;
				vendForLinkedList = new VendingPrepackSnack;

				// read in the file object into temp object A - A not holds both attributes and corrupt memory information of virtual functions
				reader.read(reinterpret_cast<char*>(vendTemp), sizeof(VendingPrepackSnack));

				// transfer all attributes from object A to object B - B still has the virtual memory info from this runtime

				// class attributes
				vendForLinkedList->setBalance(vendTemp->getBalance());
				vendForLinkedList->setName(vendTemp->getName());

				// read in the file object into temp object A - A not holds both attributes and corrupt memory information of virtual functions
				vendForLinkedList->setNumProducts(vendTemp->getNumProducts());
				for (int i = 0; i < 10; i++)
				{
					// product attributes from within array of products in class object
					vendForLinkedList->product[i].amtOnHand = vendTemp->product[i].amtOnHand;
					vendForLinkedList->product[i].amtSold = vendTemp->product[i].amtSold;
					vendForLinkedList->product[i].productId = vendTemp->product[i].productId;
					vendForLinkedList->product[i].productName = vendTemp->product[i].productName;
					vendForLinkedList->product[i].salesPrice = vendTemp->product[i].salesPrice;
				}
			}
			else
			{
				// break if invalid type
				break;
			}
			// add machine to linked list
			vendForLinkedList->setNextPtr(top);
			top = vendForLinkedList;
			reader.read(reinterpret_cast<char*>(&typeInt), sizeof(int));

			//!!!!!
			// cannot delete object with corrupt memory information...
			// This is definitely a learning experience.  We know now not to use binary storage for objects
			// that contain virtual functions.  Binary storage works great otherwise, but virtual functions
			// corrupt the object being written.
			//
			////delete vendTemp; // won't work, runtime error
			//!!!!!
		}
		// when done close reader and return success
		reader.close();
		return true;
	}
	catch (exception &ex)
	{
		// if file error show message and return fail
		cout << "Unable to open data file. Error: " << ex.what() << "\n";
		_getch();
		return false;
	}
}