#ifndef OPERATIONS_H
#define OPERATIONS_H
// include derived classes necessary for menu functions
#include "VendingBottledBev.h"
#include "VendingFountain.h"
#include "VendingPrepackSnack.h"
// include fstream for file read/write
#include <fstream>

// methods for manipulating file data


// Menu Operations of main()
int ShowMainMenu();
void CreateMachine(VendingMachine * &top);
void LoadMachine(VendingMachine * &top);
void SetPricing(VendingMachine * &top);
void StartMachine(VendingMachine * &top);
void GenerateInventoryReport(VendingMachine * &top);

// special intro screen
void welcomeScreen();

// operations for screen manipulation
void gotoXY(int x, int y);
void gotoXY(int *x, int *y, string s, bool insert, bool highlighted);
void gotoXY(int x, int y, char ch);
WORD CellColorNum(WORD background, WORD text);
void ChangeCellColor(WORD background, WORD text);
void ChangeCellColor(WORD cellColor);
void ChangeCellBackgroundColor(WORD currentCellColor, WORD newBgColor);
void ChangeCellTextColor(WORD currentCellColor, WORD newTextColor);

#endif // !OPERATIONS_H
