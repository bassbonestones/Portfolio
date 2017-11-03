#ifndef VENDINGPREPACKSNACK_H
#define VENDINGPREPACKSNACK_H
#include "VendingMachine.h"

class VendingPrepackSnack : public VendingMachine
{
	// class derived from VendingMachine, represents machines that sell packaged goods (snacks or canned/bottled drinks)
private:
	// private member data for this class is all contained in the base class
	string type = "prepackaged snack vending machine";
public:
	// public overridden functions from virtual functions in base class
	VendingPrepackSnack() {
		vendingType = "PrepackSnack";
	}
	// deconstructor (only seen if _getch() added)
	~VendingPrepackSnack() {
	}

	// overloaded function (see base class)
	void Load();
	void SetProductPrices();
	void Test();
	void Report();
};
#endif // !VENDINGPREPACKSNACK_H

