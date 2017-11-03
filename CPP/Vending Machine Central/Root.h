#ifndef ROOT_H
#define ROOT_H

// includes that will affect the rest of the file heirarchy since this is the 
// trunk of the file tree -- also namespace
#include <iostream>
#include <string>
#include <iomanip>
#include <conio.h>
#include <Windows.h>
#include <Mmsystem.h>
#include <mciapi.h>
using namespace std;

// this header is the true root of the file structure
// it holds all the external library includes and using namespace std

// set constants for color
const WORD BLACK = 0;
const WORD BLUE = 1;
const WORD GREEN = 2;
const WORD CYAN = 3;
const WORD RED = 4;
const WORD PURPLE = 5;
const WORD YELLOW_DARK = 6;
const WORD WHITE = 7;
const WORD GRAY = 8;
const WORD BLUE_BRIGHT = 9;
const WORD GREEN_BRIGHT = 10;
const WORD CYAN_BRIGHT = 11;
const WORD RED_BRIGHT = 12;
const WORD PINK = 13;
const WORD YELLOW = 14;
const WORD WHITE_BRIGHT = 15;
const WORD DEFAULT = 15;
const WORD HIGHLIGHTED = (YELLOW_DARK * 16 + BLUE);

// declare functions for altering cells
void gotoXY(int x, int y);
void gotoXY(int *x, int *y, std::string s, bool insert, bool highlighted);
void gotoXY(int x, int y, char ch);
void HideCursor(bool hide);
WORD CellColorNum(WORD background, WORD text);
void ChangeCellColor(WORD background, WORD text);
void ChangeCellColor(WORD cellColor);
void ChangeCellBackgroundColor(WORD currentCellColor, WORD newBgColor);
void ChangeCellTextColor(WORD currentCellColor, WORD newTextColor);


// declaration of all validation functions - reusable
bool ValidInteger(string input); // test: all characters are numbers
bool ValidInteger(string input, int limit, bool isMin); // false is Max limit
bool ValidInteger(string input, int begin, int end);
bool ValidDouble(string input);
bool ValidDouble(string input, double limit, bool isMin); // false is Max limit
bool ValidDouble(string input, double begin, double end);
bool ValidMoney(string input);
bool ValidMoney(string input, double limit, bool isMin); // false is Max limit
bool ValidMoney(string input, double begin, double end);
bool ValidName(string input);
bool ValidCharRanged(char input, char begin, char end);
bool ValidDigit(char input);
bool ValidLetter(char input);
bool ValidAlpha(string input);
bool ValidAlphaNum(string input);
bool ValidVisibleChar(char input);

// conversion function - great for display and creating string menus
string MoneyToString(double money);

#endif // !ROOT_H
