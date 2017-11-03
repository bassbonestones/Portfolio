#include "Root.h"

void gotoXY(int x, int y)
{
	// go to coordinate on screen
	COORD coord = { x, y };
	SetConsoleCursorPosition(GetStdHandle(STD_OUTPUT_HANDLE), coord);
}

void gotoXY(int *x, int *y, string s, bool insert, bool highlighted)
{
	// go to coordinate on screen, write message with insert and highlight options
	COORD coord = { *x, *y };
	SetConsoleCursorPosition(GetStdHandle(STD_OUTPUT_HANDLE), coord);
	if (highlighted)
		ChangeCellColor(WHITE_BRIGHT, BLACK);

	cout << s;
	if (insert)
		SetConsoleCursorPosition(GetStdHandle(STD_OUTPUT_HANDLE), coord);
	else
	{
		*x = *x + 1;
	}
	if (highlighted)
		ChangeCellColor(WHITE_BRIGHT);
}

void gotoXY(int x, int y, char ch)
{
	// go to coordinate on screen and write a single character
	COORD coord = { x, y };
	SetConsoleCursorPosition(GetStdHandle(STD_OUTPUT_HANDLE), coord);
	cout << ch;
	SetConsoleCursorPosition(GetStdHandle(STD_OUTPUT_HANDLE), coord);
}





void HideCursor(bool hide)
{
	// function to hide screen cursor
	HANDLE consoleHandle = GetStdHandle(STD_OUTPUT_HANDLE);
	CONSOLE_CURSOR_INFO info;
	info.dwSize = 100;
	info.bVisible = !hide;
	SetConsoleCursorInfo(consoleHandle, &info);
}


WORD CellColorNum(WORD background, WORD text)
{
	// method to return a color number
	return (background * 16 + text);
}

void ChangeCellColor(WORD background, WORD text)
{
	// method to change the cell color
	HANDLE h = GetStdHandle(STD_OUTPUT_HANDLE);
	SetConsoleTextAttribute(h, CellColorNum(background, text));
}
void ChangeCellColor(WORD cellColor)
{
	// overloaded method to change the cell color
	HANDLE h = GetStdHandle(STD_OUTPUT_HANDLE);
	SetConsoleTextAttribute(h, cellColor);
}
void ChangeCellBackgroundColor(WORD currentCellColor, WORD newBgColor)
{
	// method to change the background color
	WORD textColor = currentCellColor % 16;
	WORD newCellColor = newBgColor * 16 + textColor;
	ChangeCellColor(newCellColor);
}
void ChangeCellTextColor(WORD currentCellColor, WORD newTextColor)
{
	// method to change the foreground text color
	WORD bgColor = currentCellColor - currentCellColor % 16;
	WORD newCellColor = bgColor * 16 + newTextColor;
	ChangeCellColor(newCellColor);
}

bool ValidInteger(string input){
	//validates each character in input to see if it is a valid digit
	if (input.length() < 1)
		return false;
	for each (char ch in input)
		if (!ValidDigit(ch))
			return false;
	return true;
}

bool ValidInteger(string input, int limit, bool isMin = true){// false is Max limit
	//checks to see if the integer is valid, then checks the min or max of the input
	if (!ValidInteger(input))
		return false;
	int intInput = stoi(input);
	if (isMin && intInput < limit) 
		return false;
	if (!isMin && intInput > limit)
		return false;
	return true;
}

bool ValidInteger(string input, int begin, int end){
	//checks to see if the integer is valid, then checks if input is between begin and end
	if (!ValidInteger(input))
		return false;
	int intInput = stoi(input);
	if (intInput < begin || intInput > end)
		return false;
	return true;
}

bool ValidDouble(string input){
	//validates to make sure a string is a double
	bool hasDec = false;
	if (input.length() < 1)
		return false;
	for each (char ch in input) {
		if (!(ch >= 48 && ch <= 57) && ch != '.')
			return false;
		if (ch == '.') {
			if (hasDec)
				return false;
			else
				hasDec = true;
		}
	}
	return true;
}

bool ValidDouble(string input, double limit, bool isMin = true){// false is Max limit
	//validates to make sure a string is a double and tests for a min or max
	if (!ValidDouble(input))
		return false;
	double doubleInput = stod(input);
	if (isMin && doubleInput < limit)
		return false;
	if (!isMin && doubleInput > limit)
		return false;
	return true;
}

bool ValidDouble(string input, double begin, double end){
	//validates to make sure a string is a double and tests for a min and max

	if (!ValidDouble(input))
		return false;
	double doubleInput = stod(input);
	if (doubleInput < begin || doubleInput > end)
		return false;
	return true;
}

bool ValidMoney(string input){
	//validates to make sure a string is divisible by a penny
	if (!ValidDouble(input))
		return false;
	double moneyInput = stod(input); 
	int decPlace = to_string(moneyInput / .05).find('.');
	for each (char ch in to_string(moneyInput /.05).substr(decPlace+1))
	{
		if (ch != '0')
		{
			system("cls");
			cout << "Invalid price: Please enter a price that is divisible by a nickel (.05)." << endl
				<< "Press any key to try again." << endl;
			_getch();
			return false;
		}
	}
	return true;
}

bool ValidMoney(string input, double limit, bool isMin = true){// false is Max limit
	//validates divisibility by a penny and tests for a min or max
	if (!ValidMoney(input))
		return false;
	double moneyInput = stod(input);
	if (isMin && moneyInput < limit)
		return false;
	if (!isMin && moneyInput > limit)
		return false;
	return true;
}

bool ValidMoney(string input, double begin, double end){
	//validates divisibility by a penny and tests for a min and max
	if (!ValidMoney(input))
		return false;
	double moneyInput = stod(input);
	if (moneyInput < begin || moneyInput > end)
		return false;
	return true;
}

bool ValidName(string input){
	//validates a string for visible characters and a space
	if (input.length() < 1)
		return false;
	for each(char ch in input)
		if (!ValidVisibleChar(ch) && ch != ' ')
			return false;
	return true;
}

bool ValidCharRanged(char input, char begin, char end){
	//validates that a character is between two other characters (inclusive)
	return (input >= begin && input <= end) ? true : false;
}

bool ValidDigit(char input){
	//validates a character to see if it is a number
	return (ValidCharRanged(input, 48, 57)) ? true : false;
}

bool ValidLetter(char input){
	//validates a character to see if it is a letter
	return ((ValidCharRanged(input, 'A', 'Z')) || (ValidCharRanged(input, 'a', 'z'))) ? true : false;
}

bool ValidAlpha(string input){
	//validates a string to see if it is alphabetic
	if (input.length() < 1)
		return false;
	for each(char ch in input)
		if (!ValidLetter(ch)) 
			return false;
	return true;
}

bool ValidAlphaNum(string input){
	//validates a string to ensure that it only contains letters and numbers
	if (input.length() < 1)
		return false;
	for each(char ch in input)
		if (!ValidLetter(ch))
			return false;
	for each (char ch in input)
		if (!ValidDigit(ch))
			return false;
	return true;
}

bool ValidVisibleChar(char input){
	//makes sure a character isn't whitespace
	return ValidCharRanged(input, 33, 126);
}

string MoneyToString(double money)
{
	string moneyString = to_string(money);
	int decPlace = moneyString.find('.');
	return moneyString.substr(0, decPlace + 3);
}
