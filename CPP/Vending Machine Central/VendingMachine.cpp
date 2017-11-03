#include "VendingMachine.h"


void VendingMachine::ThankCustomer() {
	/*This function is a salutation for the customer for using this specific vending machine.
	It verifies if the purchase has been made. */
	cout << "Thank you for your purchase. Please come again!\n\n"
		<< "Press any key to return to purchase menu." << endl;
	_getch();
}

VendingMachine::~VendingMachine() {
}

