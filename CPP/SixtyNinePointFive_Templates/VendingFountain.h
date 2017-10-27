#ifndef VENDINGFOUNTAIN_H
#define VENDINGFOUNTAIN_H
#include "VendingMachine.h"

struct CupCounter
{
	// structure built to keep track of each product's cup count (for generation of reports)
	int numSmallsSold;
	int numMediumsSold;
	int numLargesSold;
};

class VendingFountain : public VendingMachine
{
	// this class is derived from VendingMachine and represents Beverage Fountain
	// vending machines
private:
	// private member data specific to a fountain drink dispensing vending machine

	// cupCounter is a parallel array to product[] in the base class, keep track of each
	// individual product's cup count
	CupCounter cupCounter[10];
	// variables to keep track of how many cups of each size are in the machine
	int qtySmCups_10oz;
	int qtyMedCups_20oz;
	int qtyLgCups_32oz;
	// type string for display only
	string type = "fountain beverage vending machine";
public:
	// set vendingType from base for file write
	VendingFountain(){
		vendingType = "Fountain";
	}
	// default constructor (only shown if _getch() added)
	~VendingFountain() {
	}
		// public getters and setters for the private member data
	int getQtySmCups_10oz() { return qtySmCups_10oz; }
	void setQtySmCups_10oz(int p_qty) { qtySmCups_10oz = p_qty; }

	int getQtyMedCups_20oz() { return qtyMedCups_20oz; }
	void setQtyMedCups_20oz(int p_qty) { qtyMedCups_20oz = p_qty; }

	int getQtyLgCups_32oz() { return qtyLgCups_32oz; }
	void setQtyLgCups_32oz(int p_qty) { qtyLgCups_32oz = p_qty; }

	int getSmallCupsSold(int index) { return cupCounter[index].numSmallsSold; }
	void setSmallCupsSold(int index, int qty) { cupCounter[index].numSmallsSold = qty; }

	int getMediumCupsSold(int index) { return cupCounter[index].numMediumsSold; }
	void setMediumCupsSold(int index, int qty) { cupCounter[index].numMediumsSold = qty; }

	int getLargeCupsSold(int index) { return cupCounter[index].numLargesSold; }
	void setLargeCupsSold(int index, int qty) { cupCounter[index].numLargesSold = qty; }

	// overloaded functions (see base class)
	void Load();
	void SetProductPrices();
	void Test();
	void Report();
};


#endif // !VENDINGFOUNTAIN_H
