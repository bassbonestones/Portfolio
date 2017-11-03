#ifndef VENDINGMACHINE_H
#define VENDINGMACHINE_H
#include "Product.h"

class VendingMachine
{
	// base class, abstract
		// this class represents all vending machines
private:
		// private member data for all products
	
	double balance;
	int numProducts = 0;
	string name;
	
	// pointer for linked list
	VendingMachine * next;
	// bool takesCreditCard;
public:
	// public variable for file write
	string vendingType;
	// made public for easier access
	Product product[10];
		// default constructor
	VendingMachine(){}
		// virtual base class deconstructor - clean up for all sub-classes
	virtual ~VendingMachine(); 

	// virtual functions (overloaded in all derived classes)
	virtual void Load() {}
	virtual void SetProductPrices() {}
	virtual void Test() {}
	virtual void Report() {}

	// function not overloaded
	void ThankCustomer();

	// getters and setters to private member data
	VendingMachine * getNextPtr() { return next; }
	void setNextPtr(VendingMachine * nextPtr) { next = nextPtr; }

	int getNumProducts() { return numProducts; }
	void setNumProducts(int num) { numProducts = num; }

	string getName() { return name; }
	void setName(string n) { name = n; }

	double getBalance() { return balance; }
	void setBalance(double b) { balance = b; }

};



#endif // !VENDINGMACHINE_H

