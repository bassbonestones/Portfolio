#include "Validation.h"

bool ValidWhole(string input)
{
	for each(char c in input)
		if (c < '0' || c > '9')
			return false;
	return true;
}
bool ValidWhole(char c)
{
	if (c < '0' || c > '9')
		return false;
	return true;
}
bool ValidWhole(string input, int start, int end)
{
	if (!ValidWhole(input))
		return false;
	if (atoi(input.c_str()) < start || atoi(input.c_str()) > end)
		return false;
	return true;
}

bool ValidDouble(string input)
{
	bool hasDec = false;
	bool hasNum = false;
	for each(char c in input)
		if (!ValidWhole(c) && c != '.')
			return false;
	for each(char c in input)
	{
		if (hasDec && c == '.')
			return false;
		if (!hasNum && ValidWhole(c))
			hasNum = true;
		if (!hasDec && c == '.')
			hasDec = true;
	}
	if (!hasNum)
		return false;
	return true;
}

bool ValidAlpha(string input)
{
	for each(char c in input)
		if (!(c >= 'a' && c <= 'z') && !(c >= 'A' && c <= 'Z'))
			return false;
	return true;
}

bool ValidAlpha(char c)
{
	if (!(c >= 'a' && c <= 'z') && !(c >= 'A' && c <= 'Z'))
		return false;
	return true;
}

bool ValidNameChars(string input)
{
	for (int i = 0; i < input.length(); i++)
		if (input[i] != '.' && input[i] != '\'' && input[i] != ' ')
			if ( (ValidWhole(input[i]) == false))
				if((ValidAlpha(input[i]) == false))
				return false;
	return true;
}
bool ValidYN(string input) {
	if (input != "n" && input != "N" && input != "y" && input != "Y")
		return false;
	return true;

}

bool ValidMoney(string input) {
	bool hasDec = false;
	bool hasNum = false;
	int decIndex;
	for each(char c in input)
		if (!ValidWhole(c) && c != '.')
			return false;
	for each(char c in input)
	{
		if (hasDec && c == '.')
			return false;
		if (!hasNum && ValidWhole(c))
			hasNum = true;
		if (!hasDec && c == '.')
			hasDec = true;
	}
	if (!hasNum)
		return false;
	if (hasDec)
	{
		decIndex = input.find('.');
		if (input.length() > decIndex + 3)
			return false;
	}
	return true;

}

string getDouble(string input)
{
	bool hasDec = false;
	for each(char c in input)
		if (c == '.')
			hasDec = true;
	if (hasDec)
	{
		if (input.length() == input.find('.') + 1)
			return input.substr(0,input.length()-2);
	}
}

string getMoney(string input)
{
	bool hasDec = false;
	for each(char c in input)
		if (c == '.')
			hasDec = true;
	if (hasDec)
	{
		if (input.length() == input.find('.') + 1)
			return input + "00";
		if (input.length() == input.find('.') + 2)
			return input + "0";
	}
	else
	{
		return input + ".00";
	}
}