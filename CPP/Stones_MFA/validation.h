#ifndef VALIDATION_H
#define VALIDATION_H
#include <iostream>
#include <conio.h>
#include <stdlib.h>
#include <string>
#include <string.h>

// this header file has declarations of functions for validation
bool ValidWhole(char* charArr);
bool ValidReal(char* charArr);
bool ValidChar(char* charArr);
bool ValidPhone(char* charArr);
bool ValidSSN(char* charArr);
bool ValidEmail(char* charArr);
bool ValidDate(char* charArr);
bool ValidTime(char* charArr);
bool IsVisibleChar(char ch);
bool IsDelete(char* charArr, char ch, int &ctr);
void UpdateCharArray(char* charArr, char ch, int &ctr);
bool IsCharBetween(char ch, char min, char max);

#endif
