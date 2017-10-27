#ifndef VENDINGBOTTLEDBEV_H
#define VENDINGBOTTLEDBEV_H
#include "VendingMachine.h"

class VendingBottledBev : public VendingMachine
{
	// this class is derived from VendingMachine and represents
	// Bottled Beverage vending machines
private:
	// type for display
	string type = "bottled beverage vending machine";
public:
	// set base vendingType for file write
	VendingBottledBev(){
		vendingType = "BottledBev";
	}
	// called (not shown unless _getch() added)
	~VendingBottledBev() {
	}
	// overloaded functions (see base)
	void Load();
	void SetProductPrices();
	void Test();
	void Report();
};
#endif // !VENDINGBOTTLEDBEV_H
