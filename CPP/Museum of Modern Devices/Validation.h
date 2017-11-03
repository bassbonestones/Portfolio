#ifndef VALIDATION_H
#define VALIDATION_H
#include <iostream>
#include <string>
#include <conio.h>

using namespace std;

bool ValidWhole(string input);
bool ValidWhole(char c);
bool ValidWhole(string input, int start, int end);
bool ValidDouble(string input);
bool ValidAlpha(string input);
bool ValidAlpha(char c);
bool ValidNameChars(string input);
bool ValidYN(string input);
bool ValidMoney(string input);
string getMoney(string input);
string getDouble(string input);
#endif // !VALIDATION_H
