// Jeremiah Stones
// Advanced C++
// Rodney Ortigo

// Program Specification:
// This program demonstrates two overloaded operators: one unary (-- prefix) and
// one binary (< less than).  A sword class is created in which the operators are 
// overloaded.  -- decrements the sharpness attribute of a sword by one. < compares
// the sharpness of two swords and returns a bool value, depending on if the first
// has a lower sharpness value than the second or not.  The swords battle randomly 
// 5 times.  Each time the program shows you the winner (sharpest sword wins), and 
// how each sword has lowered in sharpness by one. At the end it shows you the final
// sharpness for each sword.


#include <iostream>
#include <conio.h>
#include <string>
#include <ctime>
#include <Windows.h>
#include <mmsystem.h>
#include "winresrc.h"
#include "resource.h"
//#pragma comment(lib, "winmm.lib") -- alternate to additional dependencies in solution

using namespace std;


class Sword
{
	// class for the sword object, has sharpness, a constructor with one arg,
	// a getter, and two overloaded operators
public:
	Sword(int s) : _sharpness(s) {}
	Sword operator --();
	bool operator < (Sword sw) const;
	int getSharpness();
private:
	int _sharpness;
};

void HideCursor(bool hide);

int main()
{
	// seed the random runction with time(); -- seems to be the most recommended way
	srand((unsigned)time(NULL));
	// change console titlebar
	SetConsoleTitle(TEXT("Battle of Blades"));
	// hide the cursor, not needed
	HideCursor(true);
	// timer variables
	clock_t start;
	double duration;
	// create an array of
	Sword swArr[] = { Sword(10),Sword(11),Sword(12) };
	int battleNum = 1;
	int swA, swB, color1,color2;

	// welcome the user to the program, explain how it works
	// opening fanfare sound


	cout << "Welcome to the \"Battle of Blades\"!" << endl << endl
		<< "When a sword is used in battle, its sharpness decreases by one." << endl
		<< "The sharpest sword wins the battle. Equal sharpness results in a tie." << endl << endl;

	cout << "Sword #1 has a sharpness of " << swArr[0].getSharpness() << "." << endl
		<< "Sword #2 has a sharpness of " << swArr[1].getSharpness() << "." << endl
		<< "Sword #3 has a sharpness of " << swArr[2].getSharpness() << "." << endl << endl << endl;
	
	do
	{
		// loops cycles through 5 battles (randomly selected swords and colors
		// uses overloaded operators '<' and '--' to compare and alter Sword obects
		// in the array
		cout << "Hit any key to start battle " << battleNum++ << " of 5.";
		PlaySound(MAKEINTRESOURCE(SOUND1), NULL, SND_RESOURCE | SND_ASYNC);
		start = GetTickCount();
		duration = 1;
		int durCheck = 0;
		do {

			if (durCheck < (int)(duration / 11))
			{
				PlaySound(MAKEINTRESOURCE(SOUND1), NULL, SND_RESOURCE | SND_ASYNC);
				durCheck++;
			}
			duration = (GetTickCount() - start) / (double)CLOCKS_PER_SEC;
		} while (!_kbhit());
		
		_getch();
		// play battle sound while load bar loads
		PlaySound(MAKEINTRESOURCE(SOUND2), NULL, SND_RESOURCE | SND_ASYNC);
					
		system("cls");
		// get  random swords and colors for load bar
		swA = rand() % 3;
		do {
			swB = rand() % 3;
		} while (swA == swB);
		color1 = rand() % 13 + 2;
		do {
			color2 = rand() % 13 + 2;
		} while (color1 == color2);
		// sword battle
		cout << "Battle " << battleNum - 1 << ":  Sword #" << swA + 1 << " VS Sword #" << swB + 1 << endl << endl
			<< "The battle begins.   ";
		SetConsoleTextAttribute(GetStdHandle(STD_OUTPUT_HANDLE), color1);
		for (int i = 0; i < 5; i++) {
			cout << char(176);
		}
		for (int i = 0; i < 5; i++) {
			cout << char(8);
		}
		
		start = GetTickCount();
		duration = 0;
		SetConsoleTextAttribute(GetStdHandle(STD_OUTPUT_HANDLE), color2);
		for (int i = 0; i < 6; i++) {
			while (i * 1 > duration) {
				duration = (GetTickCount() - start) / (double)CLOCKS_PER_SEC;
				if (_kbhit()) {
					_getch(); // catch keypress during battle timer, so won't skip next screen
				}
			}
			if(i != 5)
				cout << char(219);
		}
		SetConsoleTextAttribute(GetStdHandle(STD_OUTPUT_HANDLE), 15); // default white on black
		system("cls");
		// display battle results
		PlaySound(MAKEINTRESOURCE(SOUND1), NULL, SND_RESOURCE | SND_ASYNC);
		if (battleNum == 6)
			cout << "Final Battle Results:  ";
		else
			cout << "Results From Battle " << battleNum - 1 << ":  ";
		if (swArr[swA] < swArr[swB])
			cout << "Sword #" << swB + 1 << " beat Sword #" << swA + 1 << "." << endl << endl;
		else if (swArr[swB] < swArr[swA])
			cout << "Sword #" << swA + 1 << " beat Sword #" << swB + 1 << "." << endl << endl;
		else // if they are the same
			cout << "Sword #" << swA + 1 << " and Sword #" << swB + 1 << " tied, because they have the same sharpness." << endl << endl;
		cout << "Sword #" << swA + 1 << " went from a sharpness of " << swArr[swA].getSharpness() << " to ";
		--swArr[swA];
		cout << swArr[swA].getSharpness() << "." << endl << endl;
		cout << "Sword #" << swB + 1 << " went from a sharpness of " << swArr[swB].getSharpness() << " to ";
		--swArr[swB];
		cout << swArr[swB].getSharpness() << "." << endl << endl;
		
	} while (battleNum < 6);
	// display end results of sword sharpness and keypress to exit program
	// play closing fanfare
	PlaySound(MAKEINTRESOURCE(SOUND3), NULL, SND_RESOURCE | SND_ASYNC);
	start = GetTickCount();
	duration = 0;
	for (int i = 0; i < 8; i++) {
		while (i * 1 > duration) {
			duration = (GetTickCount() - start) / (double)CLOCKS_PER_SEC;
			if (_kbhit()) {
				_getch(); // catch keypress during battle timer, so won't skip next screen
			}
		}
	}

	cout << "The battles are now over." << endl << endl
		<< "Sword #1 now has a sharpness of " << swArr[0].getSharpness() << "." << endl
		<< "Sword #2 now has a sharpness of " << swArr[1].getSharpness() << "." << endl
		<< "Sword #3 now has a sharpness of " << swArr[2].getSharpness() << "." << endl << endl << endl
		<< "Hit any key to leave the battle scene and quit the program.";
	PlaySound(MAKEINTRESOURCE(SOUND4), NULL, SND_RESOURCE | SND_ASYNC);
	start = GetTickCount();
	duration = 1;
	int durCheck = 0;
	do {
		
		if (durCheck < (int)(duration/10))
		{
			PlaySound(MAKEINTRESOURCE(SOUND4), NULL, SND_RESOURCE | SND_ASYNC);
			durCheck++;
		}
		duration = (GetTickCount() - start) / (double)CLOCKS_PER_SEC;
	} while (!_kbhit());
	


		return 0;
}

// define sword class stuff
Sword Sword::operator --()
{
	return Sword(--_sharpness);
}
bool Sword::operator < (Sword sw) const
{
	return (_sharpness < sw._sharpness);
}
int Sword::getSharpness()
{
	return _sharpness;
}

// define hide cursor function
void HideCursor(bool hide)
{
	HANDLE consoleHandle = GetStdHandle(STD_OUTPUT_HANDLE);
	CONSOLE_CURSOR_INFO info;
	info.dwSize = 100;
	info.bVisible = !hide;
	SetConsoleCursorInfo(consoleHandle, &info);
}


