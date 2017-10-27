#ifndef CHAT
#define CHAT
// all necessary includes for program
#include <Windows.h>
#include <string>
#include <iostream>
#include <mmsystem.h>
#include <ctime>
#include "resource.h"

using namespace std;

//variables
bool connectedAlready = false;
HWND TextBox;
char textSaved[256];
char history[10000];
HWND ReadOnly;
HWND Button;
HWND Button2;
HWND Combo;
HWND hwnd_main;
WNDPROC oldEditProc;
bool usernameEntered = false;
bool promptedName = false;
string username = "";
int usernameLength = 0;
string userinput = "";
int startx = 0;
int starty = 0;
double duration = 0.6;
double duration2 = 0;
int durCheck = 0;
clock_t start = 0;
bool movedBack = false;
WNDCLASSEX wc;
int volumeDownCtr;
bool draw = false;
int mouseMoveCtr = 0;
bool moveDraw = false;
bool drawButtonPressed = false;
bool playDoorShut = false;
string art[5];

// used to get desktop size
void GetDesktopResolution(int& horizontal, int& vertical)
{
	RECT desktop;
	const HWND hDesktop = GetDesktopWindow();
	GetWindowRect(hDesktop, &desktop);
	horizontal = desktop.right;
	vertical = desktop.bottom;
}

// using an external function to add text to the Edit control makes the control autoscroll to the bottom as desired (otherwise it doesn't)
void AddToChat(string _str)
{
	int _strlen = _str.size();
	for (int i = 0; i < 10000; i++)
		history[i] = '\0';
	int gwtstat = GetWindowText(ReadOnly, &history[0], 10000);
	int j, i;
	if (!(history[0] == '\0'))
	{
		history[gwtstat++] = '\r';
		history[gwtstat++] = '\n';
	}
	for (i = gwtstat, j = 0; j < _strlen; i++, j++)
	{
		history[i] = _str[j];
	}
	SetWindowText(ReadOnly, history);
	SendMessage(ReadOnly, WM_VSCROLL, SB_BOTTOM, NULL);

}

// thread function to delay WM_PAINT, checks to see if user has selected an item
void doDraw()
{
	clock_t myStart = GetTickCount();
	double duration = 0;
	while (duration < 0.1)
	{
		duration = (GetTickCount() - myStart) / (double)CLOCKS_PER_SEC;
	}
	RedrawWindow(hwnd_main, 0, 0, RDW_INVALIDATE);
}
// thread used to make the combobox NO-ITEM-SELECTED message disappear after a few seconds
void doDrawLong()
{
	clock_t myStart = GetTickCount();
	double duration = 0;
	while (duration < 4)
	{
		duration = (GetTickCount() - myStart) / (double)CLOCKS_PER_SEC;
	}
	RedrawWindow(hwnd_main, 0, 0, RDW_INVALIDATE);
}
#endif // !CHAT