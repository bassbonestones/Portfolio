// header file for validation object class 

#ifndef VALIDATION_H
#define VALIDATION_H
#include <iostream>
#include <string>
#include <conio.h>
#include <fstream>
#include <iomanip>
#include <Windows.h>
using namespace std;

class Validation
{
	// class to hold validation functions for checking user input
public:
	bool ValidAlpha(char ch);
	bool ValidDigit(char ch);
	bool ValidName(string input);
	bool ValidFormula(string input);
	bool ValidReal(string input);
	bool ValidPositiveReal(string input);
};
#endif // !VALIDATION_H
