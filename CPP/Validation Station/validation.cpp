#include "validation.h"
#include <thread>

using namespace std;

static void PrintError(string type);

// this source file contains the definitions for the functions
// declared in validation.h


bool ValidWhole(char* charArr)
{
	// function to test for a valid whole number -- only allows characters between 0 and 9
	if ((int)(charArr[0]) <= 0)
	{
		cout << endl << "Nothing was entered.";
		PrintError("whole");
		return false;
	}
	for (int i = 0; i < 25; i++)
	{
		if (charArr[i] <= '\0')
			break;
		else if (!(IsCharBetween(charArr[i], '0', '9')))
		{
			PrintError("whole");
			return false;
		}
	}
	return true;
}

bool ValidReal(char* charArr)
{
	// function to test for real numbers, allows - and .

	bool hasDecimal = false;
	char lastChar = ' ';
	if ((int)(charArr[0]) <= 0)
	{
		cout << endl << "Nothing was entered.";
		PrintError("real");
		return false;
	}
	for (int i = 0; i < 25; i++)
	{
		if (charArr[i] <= 0)
		{
			lastChar = charArr[i - 1];
			break;
		}
		else if ((!(IsCharBetween(charArr[i], '0', '9')) && charArr[i] != '.' && charArr[i] != '-') ||
			(charArr[i] == '-' && i != 0) ||
			(charArr[i] == '.' && hasDecimal == true) ||
			(charArr[i] == 0 && charArr[i]=='.' && !IsCharBetween(charArr[i +1],'0','9')) ||
			(charArr[i] != 0 && charArr[i]=='.' && !IsCharBetween(charArr[i-1],'0','9') && !IsCharBetween(charArr[i+1],'0','9')))
		{
			// if not a number, dec, or negative sign 
			//OR is a negative sign and not at the beginning
			//OR is a decimal sign after one has alread been entered
			//OR the first character is a decimal and not followed by a number
			//OR is a decimal and doesn't have a number before or after it
			PrintError("real");
			return false;
		}
		else if (charArr[i] == '.')
		{
			// test to see when a decimal has been entered to prevent multiple decimals
			hasDecimal = true;
		}
	}
	if ((!IsCharBetween(lastChar, '0', '9') && lastChar != '.' && lastChar != ' ') ||
		(charArr[0] == '-' && charArr[1] <= 0))
	{
		// if the last character is not a number, period or space(default value of lastChar)
		// OR if the user only entered a negative sign
		PrintError("real");
		return false;
	}
	//_getch();
	return true;
}

bool ValidChar(char* charArr)
{
	// function to test for a valid alphabet character or set of them, only allows a-z and A-Z
	if ((int)(charArr[0]) <= 0)
	{
		cout << endl << "Nothing was entered.";
		PrintError("letters");
		return false;
	}
	for (int i = 0; i < 25; i++)
	{
		if (charArr[i] <= '\0')
			break;
		else if (!IsCharBetween(charArr[i], 'a', 'z') && !IsCharBetween(charArr[i],'A','Z'))
		{
			PrintError("letters");
			return false;
		}
	}
	return true;
}

bool ValidPhone(char* charArr)
{
	// function to test for a valid phone number
	// tests every index of the char array for a specific value or range of values
	if ((int)(charArr[0]) <= 0)
	{
		cout << endl << "Nothing was entered.";
		PrintError("phone");
		return false;
	}
	if (charArr[0] != '(' || !IsCharBetween(charArr[1], '2', '9') || !IsCharBetween(charArr[2], '0', '8') ||
		!IsCharBetween(charArr[3], '0', '9') || charArr[4] != ')' || !IsCharBetween(charArr[5], '2', '9') ||
		!IsCharBetween(charArr[6], '0', '9') || !IsCharBetween(charArr[7], '0', '9') || charArr[8] != '-' ||
		!IsCharBetween(charArr[9], '0', '9') || !IsCharBetween(charArr[10], '0', '9') || !IsCharBetween(charArr[11], '0', '9') ||
		!IsCharBetween(charArr[12], '0', '9') || (charArr[6] == '1' && charArr[7] == '1') || (charArr[2] == '1' && charArr[3] == '1') ||
		(charArr[5] == '5' && charArr[6] =='5' && charArr[7] =='5' && charArr[9] =='0' && charArr[10] == '1'))
	{
		PrintError("phone");
		return false;
	}
	return true;
}

bool ValidSSN(char* charArr)
{
	// tests for a valid social security number
	// tests each index of the char array for a specific value or range of values
	if ((int)(charArr[0]) <= 0)
	{
		cout << endl << "Nothing was entered.";
		PrintError("ssn");
		return false;
	}
	if (!IsCharBetween(charArr[0], '0', '8') || !IsCharBetween(charArr[1], '0', '9') || !IsCharBetween(charArr[2], '0', '9') ||
		charArr[3] != '-' || !IsCharBetween(charArr[4], '0', '9') || !IsCharBetween(charArr[5], '0', '9') ||
		charArr[6] != '-' || !IsCharBetween(charArr[7], '0', '9') || !IsCharBetween(charArr[8], '0', '9') ||
		!IsCharBetween(charArr[9], '0', '9') || !IsCharBetween(charArr[10], '0', '9') || 
		(charArr[0] == '6' && charArr[1] == '6' && charArr[2] == '6') || 
		(charArr[0] == '0' && charArr[1] == '0' && charArr[2] == '0') || (charArr[4] == '0' && charArr[5] == '0') ||
		(charArr[7] == '0' && charArr[8] == '0' && charArr[9] == '0' && charArr[10] == '0') ||
		(charArr[5] == '5' && charArr[6] == '5' && charArr[7] == '5' && charArr[9] == '0' && charArr[10] == '1'))
	{
		//
		PrintError("ssn");
		return false;
	}
	return true;
}

bool ValidEmail(char* charArr)
{
	// validation function to see if an input is a valid email or not

	// vars
	bool hasLocalAlphaNum = false;
	bool hasDec = false;
	bool hasAt = false;
	// contains all possible characters for the local-part
	char validChars[] = { '.','!','#','$','%','&','\'','*','+','-','/','=','?','^','_',char(96),'{','|','}','~' };
	char lastChar = ' ';
	int lastDec;
	if ((int)(charArr[0]) <= 0)
	{
		// test for empty input
		cout << endl << "Nothing was entered.";
		PrintError("email");
		return false;
	}
	for (int i = 39; i >= 0; i--)
	{
		// find last decimal for testing text in the extension part of the email and store index
		if (charArr[i] == '.')
		{
			lastDec = i;
			break;
		}
		if (charArr[i] == '@')
		{
			// while going backward, if an at symbol is found before a decimal, then return false, decimal missing
			// if no at sign, a later test will return false, not this one
			PrintError("email");
			return false;
		}
	}
	for (int i = 0; i < 40; i++)
	{
		// loop through entire character array passed to function
		if (charArr[i] <= 0)
		{
			// finds the last character entered
			lastChar = charArr[i - 1];
			break;
		}
		else if (i == 39)
			lastChar = charArr[i];
		else if (hasAt)
		{
			// stuff after @
			if ((charArr[i] != '-' && charArr[i] != '.' && !IsCharBetween(charArr[i], '0', '9') &&
				!IsCharBetween(charArr[i], 'a', 'z') && !IsCharBetween(charArr[i], 'A', 'Z')) ||
				(charArr[i] == '.' && charArr[i - 1] == '-') ||
				(charArr[i] == '.' && (charArr[i - 1] == '.' || charArr[i + 1] == '.')) ||
				(i > lastDec && !IsCharBetween(charArr[i], 'a', 'z') && !IsCharBetween(charArr[i], 'A', 'Z')))
			{
				//if past the @ and is not a hyphen, dot, letter, or number
				//OR is a dot and preceded by a hyphen 
				//OR is a dot and has a dot next to it
				//OR is past the last dot and has anything but a letter or number
				PrintError("email");
				return false;
			}
			else if (charArr[i] == '.')
			{
				// test for a dot after the at to make sure one has been entered
				if (!hasDec)
					hasDec = true;
			}

		}
		else if (!hasAt && charArr[i] == '@')
		{
			// detecting first (and only allowed) @ symbol
			if ((!IsCharBetween(charArr[i + 1], '0', '9') && !IsCharBetween(charArr[i + 1], 'a', 'z') && !IsCharBetween(charArr[i + 1], 'A', 'Z')) || i == 0)
			{
				// if the character after @ isn't a number or letter
				// OR if @ is the first character
				PrintError("email");
				return false;
			}
			hasAt = true;
		}
		else  // all in the local-part of the email
		{
			// set bool and flag as true if an acceptible character, otherwise at the end of this ELSE
			// print an error to user
			bool acceptibleSymbol = false;
			for each(char c in validChars)
				if (charArr[i] == c) acceptibleSymbol = true;
			if (IsCharBetween(charArr[i], '0', '9') || IsCharBetween(charArr[i], 'a', 'z') || IsCharBetween(charArr[i], 'A', 'Z'))
			{
				acceptibleSymbol = true;
				if(!hasLocalAlphaNum)
					hasLocalAlphaNum = true;
			}
			if ((!acceptibleSymbol) ||
				(charArr[i] == '.' && (charArr[i - 1] == '.' || charArr[i + 1] == '.')))
			{
				PrintError("email");
				return false;
			}
		}
	}
	if ((!IsCharBetween(lastChar, 'a', 'z') && !IsCharBetween(lastChar, 'A', 'Z')) ||
		(!hasAt || !hasDec || !hasLocalAlphaNum))
	{
		//if the last character is not a letter
		//OR if theres no at sign, decimal, or local-part letter/number
		PrintError("email");
		return false;
	}
	return true;
}

bool ValidDate(char* charArr)
{
	// function to test for a valid date
	// check each character for a specific format mask value or for a range that is
	// dependent on the value of other characters in the array
	if ((int)(charArr[0]) <= 0)
	{
		cout << endl << "Nothing was entered.";
		PrintError("date");
		return false;
	}
	if ( // spacing done this way for clarity
		((charArr[0] == '0' && IsCharBetween(charArr[1], '1', '9')) || // correct month format
		(charArr[0] == '1' && IsCharBetween(charArr[1], '0', '2')))
		&&
		(charArr[2] == '/' && charArr[5] == '/') // used forward slash
		&&
		((charArr[3] == '0' && IsCharBetween(charArr[4], '1', '9')) || // correct dayOfMonth format
		(IsCharBetween(charArr[3], '1', '2') && IsCharBetween(charArr[4], '0', '9')) ||
			((charArr[3] == '3' && IsCharBetween(charArr[4], '0', '1'))))
		&&
		(IsCharBetween(charArr[6], '0', '9') && IsCharBetween(charArr[7], '0', '9') && // correct year format
			IsCharBetween(charArr[8], '0', '9') && IsCharBetween(charArr[9], '0', '9')))
	{
		// convert chars from month, dateOfMonth, and year to integers
		int month = (charArr[0] - 48) * 10 + (charArr[1] - 48);
		int dateOfMonth = (charArr[3] - 48) * 10 + (charArr[4] - 48);
		int year = (charArr[6] - 48) * 1000 + (charArr[7] - 48) * 100 + (charArr[8] - 48) * 10 + (charArr[9] - 48);

		if (((month == 4 || month == 6 || month == 9 || month == 11) && dateOfMonth > 30)
			||
			(month == 2 && dateOfMonth > 29))
		{
			// test for correct number of days in the month
			cout << endl << "You have an incorrect number of days in the month.";
			PrintError("date");
			return false;
		}
		if (((year % 4 != 0) || (year % 100 == 0 && year % 400 != 0))
			&& (month == 2 && dateOfMonth == 29))
		{
			// test for leap year against Feb 29th
			cout << endl << "The year you typed is not a leap year and cannot have February 29th.";
			PrintError("date");
			return false;
		}
		return true;
	}
	else
	{
		// this else is reached if the user uses a bad format or has numbers that are
		// clearly out of range of the month or day of month
		PrintError("date");
		return false;
	}
}

bool ValidTime(char* charArr)
{
	// a function to test for a valid time
	// check each character for a specific format mask value or for a range that is
	// dependent on the value of other characters in the array
	if ((int)(charArr[0]) <= 0)
	{
		cout << endl << "Nothing was entered.";
		PrintError("time");
		return false;
	}
	if ( // spacing done this way for clarity
		(((charArr[0] == '0' || charArr[0] == '1') && IsCharBetween(charArr[1], '0', '9')) || // correct hour format
		(charArr[0] == '2' && IsCharBetween(charArr[1], '0', '3')))
		&&
		(charArr[2] == ':' && charArr[5] == ':') // used colon
		&&
		(IsCharBetween(charArr[3], '0', '5') && IsCharBetween(charArr[4], '0', '9') && // correct MM:SS format
			IsCharBetween(charArr[6], '0', '5') && IsCharBetween(charArr[7], '0', '9')))
	{
		// if user has entered in a valid format and valid ranges for hours, minutes, and seconds
		return true;
	}
	else
	{
		// if the user has entered a bad format for time or exceeded the range for
		// hours, minutes, and/or seconds send error and return false
		PrintError("time"); 
		return false;
	}
}

bool IsVisibleChar(char ch)
{
	// tests for any visible character, disallowing whitespace and control keys
	if ((int)ch >= 33 && (int)ch <= 126)
		return true;
	else

		return false;
}

bool IsDelete(char* charArr, char ch, int &ctr)
{
	// handles the delete keypress
	if ((int)ch == 8)
	{
		if (ctr > 0)
		{
			cout << char(8) << ' ' << char(8);
			ctr--;
			charArr[ctr] = char(0);
		}
		return true;
	}
	else
		return false;
}

void UpdateCharArray(char* charArr, char ch, int &ctr)
{
	// updates a character array with a character and updates a counter
	charArr[ctr] = ch;
	cout << ch;
	ctr++;
}

bool IsCharBetween(char ch, char min, char max)
{
	// custom function to test for a range of characters
	if ((int)ch >= (int)min && (int)ch <= (int)max)
		return true;
	else
		return false;
}

// private functions

static void PrintError(string type)
{
	// function for handling the error for any invalid data input
	// depending on the type, a different error is displayed.
	if (type == "whole")
	{
		// displays the error for an invalid date
		cout << endl << endl << "The whole number entered is invalid. " << endl
			<< "Hint: You may only enter characters that are numeric digits (0-9)." << endl
			<< endl << "Press any key to try again.";
		_getch();
		system("cls");
	}
	else if (type == "real")
	{
		// displays the error for an invalid date
		cout << endl << endl << "The real number entered is invalid. " << endl
			<< "Hint: Real numbers may be preceded by a negative sign('-') and have up to one decimal('.')." << endl
			<< endl << "Press any key to try again.";
		_getch();
		system("cls");
	}
	else if (type == "letters")
	{
		// displays the error for an invalid date
		cout << endl << endl << "Invalid letter character(s) entered. " << endl
			<< "Hint: Only lowercase and/or uppercase letters of the Roman alphabet are allowed." << endl
			<< endl << "Press any key to try again.";
		_getch();
		system("cls");
	}
	else if (type == "phone")
	{
		// displays the error for an invalid date
		cout << endl << endl << "The phone number entered is invalid. " << endl
			<< "Hint: Area codes and exchange codes may not begin with 0 or 1, the second number of\n" <<
			   "the area code may not be a 9, the second and third digits of either the area code or\n" <<
			   "exchange code may not both be ones, and fictional numbers fall between the range of\n" <<
			   "555-0100 and 555-0199." << endl
			<< endl << "Press any key to try again.";
		_getch();
		system("cls");
	}
	else if (type == "ssn")
	{
		// displays the error for an invalid date
		cout << endl << endl << "The social security number (SSN) entered is invalid. " << endl
			<< "Hint: SSNs may not begin with a 9 or 666, and no digit group may contain all zeros, such\n" <<
			   "as 000-##-####, ###-00-####, or ###-##-0000."  << endl
			<< endl << "Press any key to try again.";
		_getch();
		system("cls");
	}
	else if (type == "email")
	{
		// displays the error for an invalid date
		cout << endl << endl << "The email entered is invalid. " << endl
			<< "Hint: The local-part must contain at least one letter or number and also accepts .!#$%&'*+-/=?^_" << char(96) << "{|}~" << endl
			<< "The domain may contain letters and numbers.  It may also have hyphens, but they may not be\n" 
			<< "the first or last character in the domain. The top level domain (TLD) is the section following\n"
			<< "the last period after the 'at sign', @. It may only contain letters. A period cannot go directly\n"
			<< "next to another period anywhere in the email address." << endl
			<< endl << "Press any key to try again.";
		_getch();
		system("cls");
	}
	else if (type == "date")
	{
		// displays the error for an invalid date
		cout << endl << endl << "The date entered is invalid. " << endl
			<< "Hint: If the month or day is a single digit, make sure it's preceded by a 0." << endl
			<< endl << "Press any key to try again.";
		_getch();
		system("cls");
	}
	else // is type "time" -- only other option
	{
		// displays the error for an invalid time
		cout << endl << endl << "The time entered is invalid." << endl
			<< "Hint: If the hour is a single digit, make sure it's preceded by a 0." << endl
			<< endl << "Press any key to try again.";
		_getch();
		system("cls");
	}
	
}