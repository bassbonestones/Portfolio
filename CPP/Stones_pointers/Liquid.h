// header file for Liquid operations class

#ifndef LIQUID_H
#define LIQUID_H
#include "Validation.h"

struct Liquid
{
	// structure to hold the information about a liquid and the pointer to the next object in the linked list
	string name;
	string molecularFormula;
	string freezingPoint_C;
	string boilingPoint_C;
	string viscosity_mPas;
	Liquid * ptr;
};



class LiquidOps
{
	// class designed to perform all liquid operations, handling the information in the liquids linked list
private:
	// head of the linked list chain
	Liquid * top;
	// path to file where liquid info is stored
	const string filePath = "liquids.dat";
	// validation object to validate user input
	Validation val;
public:
	// see cpp file for descriptions of these functions
	void KillLiquids();
	void LoadLiquids();
	void WriteLiquidsToFile();
	void ViewLiquids();
	void AlterLiquid();
	void AddLiquid();
	void RemoveLiquid();
};
#endif // !LIQUID_H
