#ifndef PRODUCT_H
#define PRODUCT_H
// include root of file organization for all libraries needed
#include "Root.h"

struct Product
{
		// variables to hold in all products
	string productId;
	string productName;
	double salesPrice;
	int amtOnHand;
	int amtSold;
};

#endif // !PRODUCT_H
