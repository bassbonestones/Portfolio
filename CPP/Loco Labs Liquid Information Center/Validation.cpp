// definition of validation functions

// include header
#include "Validation.h"

bool Validation::ValidAlpha(char ch)
{
	// checks to make sure it's a letter
	if (!(ch >= 'a' && ch <= 'z') && !(ch >= 'A' && ch <= 'Z'))
		return false;
	return true;
}

bool Validation::ValidDigit(char ch)
{
	// checks to make sure it's a number
	if (!(ch >= '0' && ch <= '9'))
		return false;
	return true;
}

bool Validation::ValidName(string input)
{
	// checks to make sure input is a name with only letters numbers and specific characters (below)
	if (input.length() < 1)
		return false;
	for each(char c in input)
	{
		if (!ValidAlpha(c) && c != ' ' && !ValidDigit(c) && c != '.' && c != '-' && c != '\'' && c != '\"')
			return false;
	}
	return true;
}

bool Validation::ValidFormula(string input)
{
	// checks for a valid formula format, with letters and numbers, but no spaces
	// at least one letter required
	bool hasLetter = false;
	if (input.length() < 1)
		return false;
	for each (char c in input)
	{
		if (!ValidAlpha(c) && !ValidDigit(c))
			return false;
		if (ValidAlpha(c))
			hasLetter = true;
	}
	if (!hasLetter)
		return false;
	return true;
}

bool Validation::ValidReal(string input)
{
	// checks for a valid real number
	bool hasDecimal = false;
	bool hasNumber = false;
	if (input.length() < 1)
		return false;
	for each(char c in input)
	{
		if (!ValidDigit(c) && c != '-' && c != '.')
			return false;
		if (c == '.')
			if (hasDecimal)
				return false;
			else
				hasDecimal = true;
		if (c == '-')
			if (input.find_last_of('-') != 0)
				return false;
		if (ValidDigit(c) && !hasNumber)
			hasNumber = true;
	}
	if (!hasNumber)
		return false;
	return true;
}

bool Validation::ValidPositiveReal(string input)
{
	// checks for a real number but also positive (aka whole)
	if (!ValidReal(input))
		return false;
	for each (char c in input)
		if (c == '-')
			return false;
	return true;
}