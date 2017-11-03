// Jeremiah Stones
// Advanced C++
// Rodney Ortigo
// June 4, 2017

// Program Specification: GUI Program
// This program sets up a client chat interface, which will eventually be used to
// send messages back and forth over a network connection.  Twenty-five windows
// messages were handled to make the GUI more interactive. Some others were sent to
// complete specific tasks needed by the program.  Here is a list of the handled and
// used messages

// Messages explicitly handled

// 1) WM_CREATE - makes window controls
// 2) WM_PAINT - paint text that prompts user to test mouse clicks (for sounds)
// 3) WM_DESTROY - sends quit message
// 4) WM_COMMAND - handles button press
// 5) WM_KEYDOWN - handles escape from main window, other typing sound
// 6) WM_ACTIVATE - sends message to user to enter username
// 7) WM_CHAR - executes send code and send sound
// 8) WM_MOVE - keeps user from moving window
// 9) WM_CLOSE - says goodbye to user
// 10) WM_SHOWWINDOW - welcomes user, and sends WM_TIMER message
// 11) WM_LBUTTONDOWN - unique sound, hiccup with pitch going up
// 12) WM_LBUTTONUP - unique sound, hiccup with pitch going down
// 13) WM_RBUTTONDOWN - unique sound, boing with pitch going up
// 14) WM_RBUTTONUP - unique sound, boing with pitch going down
// 15) WM_KEYUP - typing sound
// 16) WM_LBUTTONDBLCLK dub click select all text
// 17) WM_MOUSEWHEEL scrolling readonly area 
// 18) WM_MOUSEHWHEEL - to the left and to the right sounds
// 19) WM_TIMER - sets start to GetTickCount(), used by both HWheel and Move
// 20) WM_SETFOCUS - test if combobox has been opened
// 21) WM_KILLFOCUS - test if combobox has been shut
// 22) WM_CTLCOLOREDIT - used to change system volume when person draws (with timer)
// 23) WM_APPCOMMAND - sends message to user when volume goes up
// 24) WM_MOUSEMOVE - increments counter with mouse moves - updates paint with counter
// 25) CBN_SELCHANGE - handled when user selects a combobox item, sends art in chat


// Other messages that are utilized:

// WM_QUIT - exits loop (thrown not handled)
// EM_SETSEL - sent to select all text in Edit control on doubleclick


// include header with global variables and extra functions
#include "Chat.h"

// prototyping for Window Procedures
LRESULT CALLBACK subEditProc(HWND hwnd, UINT msg, WPARAM wParam, LPARAM lParam);
LRESULT CALLBACK WindowProc(HWND hwnd, UINT msg, WPARAM wparam, LPARAM lparam);

// main
int WINAPI WinMain(HINSTANCE hinstance, HINSTANCE prevhinstance, LPSTR lpcmdline, int CmdShow)
{
	// initialize some variables and seed random
	volumeDownCtr = 9;
	srand((UINT)time(NULL));
	art[0] = "><(((('>";
	art[1] = "@('_')@";
	art[2] = "----{,_,\">";
	art[3] = "@( * O * )@";
	art[4] = "--------{---(@";

	// local variable for message
	MSG msg;

	//set window size and location
	int horizontal = 0; int vertical = 0;
	GetDesktopResolution(horizontal, vertical);
	int windowWidth = (int)(horizontal / 2.3); int windowHeight = vertical / 2;
	startx = horizontal / 2 - windowWidth / 2;
	starty = vertical / 2 - windowHeight / 2;

	//assign values to class variables
	wc.cbClsExtra = 0;
	wc.cbSize = sizeof(WNDCLASSEX);
	wc.cbWndExtra = 0;
	wc.hbrBackground = (HBRUSH)29;
	wc.hCursor = LoadCursor(NULL, IDC_ARROW);
	wc.hIcon = LoadIcon(NULL, IDI_SHIELD);
	wc.hIconSm = LoadIcon(NULL, IDI_SHIELD);
	wc.hInstance = hinstance;
	wc.lpfnWndProc = WindowProc;
	wc.lpszClassName = "MYCLASSNAME";
	wc.lpszMenuName = NULL;
	wc.style = CS_OWNDC | CS_HREDRAW | CS_VREDRAW | CS_DBLCLKS;

	//register main class and create main window
	if (!RegisterClassEx(&wc))
	{
		return 0;
	}
	if (!(hwnd_main = CreateWindowEx(NULL, "MYCLASSNAME", "Shield Chat", WS_CAPTION | WS_SYSMENU | WS_THICKFRAME | WS_VISIBLE, startx, starty, windowWidth, windowHeight, NULL, NULL, hinstance, NULL)))
	{
		return 0;
	}

	// main message loop
	while (true)
	{
		if (PeekMessage(&msg, NULL, 0, 0, PM_REMOVE))
		{
			if (msg.message == WM_QUIT)
			{
				break;
			}
			TranslateMessage(&msg);
			DispatchMessage(&msg);

			//other main program funcationality goes here...
		}
		if (usernameEntered && !connectedAlready)
		{ 
			// setting up program to make sure the client gets connected to the host - for later
			connectedAlready = true;
		}

	}

	return 0;
}

// main window procedure
LRESULT CALLBACK WindowProc(HWND hwnd, UINT msg, WPARAM wparam, LPARAM lparam)
{
	// variables for painting
	PAINTSTRUCT ps;
	HDC hdc;

	switch (msg)
	{
	case WM_CREATE:
	{
		// create controls on window
		Button = CreateWindow(TEXT("button"), TEXT("Send"), WS_VISIBLE | WS_CHILD,
			420, 335, 100, 50, hwnd, (HMENU)1, NULL, NULL);
		Button2 = CreateWindow(TEXT("button"), TEXT("Draw Some"), WS_CHILD,
			450, 210, 160, 50, hwnd, (HMENU)3, NULL, NULL);
		TextBox = CreateWindow(TEXT("edit"), TEXT(""), WS_BORDER | WS_VISIBLE | WS_CHILD | ES_AUTOVSCROLL | WS_VSCROLL | ES_MULTILINE | ES_WANTRETURN,
			10, 335, 400, 50, hwnd, (HMENU)2, NULL, NULL);
		ReadOnly = CreateWindow(TEXT("edit"), TEXT(""), WS_VISIBLE | WS_CHILD | WS_BORDER | SS_SUNKEN | ES_MULTILINE | WS_VSCROLL | ES_READONLY,
			10, 10, 400, 315, hwnd, NULL, NULL, NULL);
		Combo = CreateWindow("COMBOBOX", "mycombo", CBS_DROPDOWNLIST | CBS_HASSTRINGS | WS_CHILD | WS_OVERLAPPED,
			530, 335, 100, 120, hwnd, (HMENU)4, NULL, NULL);
		// combobox setup
		TCHAR listWords[5][10] =
		{
			TEXT("Fish"), TEXT("Monkey"), TEXT("Mouse"), TEXT("Koala"),
			TEXT("Rose")
		};

		SendMessage(Combo, (UINT)CB_ADDSTRING, (WPARAM)0, (LPARAM)listWords[0]);
		SendMessage(Combo, (UINT)CB_ADDSTRING, (WPARAM)0, (LPARAM)listWords[1]);
		SendMessage(Combo, (UINT)CB_ADDSTRING, (WPARAM)0, (LPARAM)listWords[2]);
		SendMessage(Combo, (UINT)CB_ADDSTRING, (WPARAM)0, (LPARAM)listWords[3]);
		SendMessage(Combo, (UINT)CB_ADDSTRING, (WPARAM)0, (LPARAM)listWords[4]);

		SendMessage(Combo, CB_SETCURSEL, (WPARAM)0, (LPARAM)0);

		// set up overridden window procedure for messaging area
		oldEditProc = (WNDPROC)SetWindowLongPtr(TextBox, GWLP_WNDPROC, (LONG_PTR)subEditProc);
		// reposition main window (trying to keep the window from randomly hide behind others on startup occasionally)
		SetWindowPos(hwnd, HWND_TOP, startx, starty, 0, 0, SWP_NOSIZE);
		return 0;
	}
	break;

	case WM_COMMAND:
	{
		switch (LOWORD(wparam))
		{
		case 1: // Send Button
		{
			if (usernameEntered) // after username setup
			{
				for (int i = 0; i < 256; i++)
					textSaved[i] = '\0';
				int getTest = GetWindowText(TextBox, &textSaved[0], 256);
				if (getTest == 0)
					break;
				userinput = username + ": " + textSaved;
				AddToChat(userinput);
				SetWindowText(TextBox, "");
				PlaySound(MAKEINTRESOURCE(SENDSOUND), NULL, SND_RESOURCE | SND_ASYNC);
			}
			else // get user name
			{
				char user[25];
				GetWindowText(TextBox, &user[0], 25);
				username = user;
				usernameLength = username.size();
				if (usernameLength < 1)
					return 0;
				usernameEntered = true;
				SetWindowText(ReadOnly, "");
				SetWindowText(TextBox, "");
				AddToChat("Welcome, " + username + "!");
				ShowWindow(Combo, SW_SHOW);
				ShowWindow(Button2, SW_SHOW);
			}
		}
		break;
		case 3: // button for drawing
			draw = true;
			drawButtonPressed = true;
			RedrawWindow(hwnd, 0, 0, RDW_INVALIDATE);
			break;
		case 4: // combo box
			switch (HIWORD(wparam))
			{
			case CBN_SELCHANGE: // when user selects an item
			{
				int index = SendMessage(Combo, (UINT)CB_GETCURSEL, 0, 0);
				AddToChat(username + ": " + art[index]); // use index to print artwork
				playDoorShut = false; // flag so doorshut won't play in thread
				PlaySound(MAKEINTRESOURCE(index + 112), NULL, SND_RESOURCE | SND_ASYNC); // play art accompanying sound
				SendMessage(Combo, CB_SETCURSEL, 0, 0); // reset selection cursor to top so items won't disappear
				SetFocus(TextBox); 
			}
			break;
			case WM_SETFOCUS: // when combobox dropdown is initiated
				PlaySound(MAKEINTRESOURCE(DOOROPEN), NULL, SND_RESOURCE | SND_ASYNC);
				break;
			case WM_KILLFOCUS: // when combobox closes
				playDoorShut = true; // set true (overridden by selection if one occurs)
				CreateThread(NULL, NULL, (LPTHREAD_START_ROUTINE)doDraw, NULL, NULL, NULL); // start thread to wait 0.1 sec for selection message
				break;
			default:
				break;
			}
			break;
		default:
			break;
		}
	}
	break;
	case WM_PAINT:
	{
		// draw text to window
		hdc = BeginPaint(hwnd, &ps);
		TextOut(hdc, 450, 100, "Hello, use this area to", 23);
		TextOut(hdc, 450, 120, "test your mouse clicks!", 23);
		TextOut(hdc, 450, 15, "Hundreds of mouse moves: ", 26);

		if (draw) // if draw button was clicked make an X (randomized x location)
		{
			HPEN hPenTemp; 
			HPEN hLinePen;
			COLORREF qLineColors[] = { RGB(255, 0, 0), RGB(0, 255, 0), RGB(0, 0, 255), RGB(0, 0, 0), RGB(255, 255, 255) };
			hLinePen = CreatePen(PS_DASHDOT, 7, qLineColors[rand() % 5]);
			hPenTemp = (HPEN)SelectObject(hdc, hLinePen);
			int x = rand() % 60 + 470;

			MoveToEx(hdc, x, 150, NULL);
			LineTo(hdc, x + 50, 200);
			MoveToEx(hdc, x, 200, NULL);
			LineTo(hdc, x + 50, 150);
			SelectObject(hdc, hPenTemp);
			DeleteObject(hLinePen);
			draw = false;
		}
		if (moveDraw) // if user has moved a multiple of 100 moves, increments drawn counter
		{
			string s = to_string((int)(mouseMoveCtr / 100));
			char *cs;
			cs = new char[s.size() + 1];
			for (int index = 0; index < (int)s.size(); index++) {
				cs[index] = s[index];
			}
			cs[s.size()] = '\0';
			TextOut(hdc, 450, 35, cs, s.size());
			delete[] cs; // clean up
			moveDraw = false;
		}
		if (playDoorShut) // if nothing was selected in combo box
		{
			TextOut(hdc, 530, 315, "Nothing picked :(", 17);
			playDoorShut = false;
			PlaySound(MAKEINTRESOURCE(DOORCLOSE), NULL, SND_RESOURCE | SND_ASYNC);
			CreateThread(NULL, NULL, (LPTHREAD_START_ROUTINE)doDrawLong, NULL, NULL, NULL); // thread to make text disappear using ELSE below
		}
		else // draws over text from combobox close
		{
			TextOut(hdc, 530, 315, "                          ", 27);
		}
		EndPaint(hwnd, &ps);
		return 0;
	}
	break;
	case WM_CTLCOLOREDIT: // sent by system when color change occurs (when Xs are drawn)
	{
		duration = (GetTickCount() - start) / (double)CLOCKS_PER_SEC; // set timer duration
		if (duration > 5 && drawButtonPressed) // if user has drawn an X in the past 5 seconds 
		{
			start = GetTickCount();
			SendMessage(hwnd, WM_APPCOMMAND, (WPARAM)hwnd, MAKELPARAM(0, APPCOMMAND_VOLUME_UP)); //increment the volume
			drawButtonPressed = false;
		}
	}
	break;
	case WM_APPCOMMAND: // message picked up from volume adjustment
		if (lparam == MAKELPARAM(0, APPCOMMAND_VOLUME_UP)) // if volume was just increased
		{
			MessageBox(NULL, "Drawing makes you deaf!", "BeWaRe!!", MB_OK | MB_SYSTEMMODAL); // show message box, adding a message to
		}																					 // the chat didn't work, keyboard lost focus
		break;
	case WM_MOUSEWHEEL: // if user is in main window area and scrolls the mouse, the ReadOnly message area is affected
	{
		int w = (int)wparam;
		if (w > 0)
			SendMessage(ReadOnly, WM_KEYDOWN, VK_PRIOR, 0); // easiest way to scroll Edit control
		if (w < 0)
			SendMessage(ReadOnly, WM_KEYDOWN, VK_NEXT, 0); // scroll down
	}
	break;
	case WM_MOUSEHWHEEL: // if user swipes or scrolls horizontally in the main window area, play fun sounds
	{
		duration = (GetTickCount() - start) / (double)CLOCKS_PER_SEC;
		int w = (int)wparam;
		int l = (int)lparam;
		if ((w < 0 && duration  > 4) || w == -7864320)
		{
			start = GetTickCount();
			PlaySound(MAKEINTRESOURCE(TOTHELEFT), NULL, SND_RESOURCE);
		}
		if ((w > 0 && duration  > 4) || w == 7864320)
		{
			start = GetTickCount();
			PlaySound(MAKEINTRESOURCE(TOTHERIGHT), NULL, SND_RESOURCE);
		}
		return 0;
	}
	break;
	case WM_MOUSEMOVE: // if the mouse moves, increment the counter and send a redraw message ever 100 move counts
		mouseMoveCtr++;
		if (mouseMoveCtr % 100 - 20 == 0)
		{
			moveDraw = true;
			RedrawWindow(hwnd, 0, 0, RDW_INVALIDATE);
		}
		break;
	case WM_DESTROY: // if window destroyed
	{
		PostQuitMessage(0); // send quit message
	}
	case WM_KEYDOWN: // if key pressed
	{
		switch (wparam)
		{
		case VK_ESCAPE: // escape pressed
		{
			int result = MessageBox(hwnd, "You pressed escape? That's no proper way to leave...", "Goodbye", MB_OKCANCEL | MB_ICONMASK);
			if (result == IDOK)
			{
				MessageBox(NULL, "You pressed okay. Glad you agree. Goodbye!", "Adios", MB_DEFAULT_DESKTOP_ONLY);
				SendMessage(hwnd, WM_DESTROY, 0, 0);
			}
		}
		break;
		case VK_VOLUME_DOWN: // volume down key pressed
		{
			volumeDownCtr++;
			if (volumeDownCtr % 5 == 0) // message added every 5 presses
			{
				AddToChat("Too much drawing, eh?");
			}
		}
		break;
		default:
			break;
		}
	}
	break;
	case WM_ACTIVATE: // uses bool flag to only prompt user once for username at beginning of program
		if (!usernameEntered & !promptedName)
		{
			AddToChat("First enter your username and hit ENTER or press 'Send'.");
			AddToChat("");
			AddToChat("Username may not exceed 25 characters.");
			AddToChat("");
			AddToChat("Messages may not exceed 256 characters.");
			promptedName = true;
		}
		break;
	case WM_CLOSE: // when the window closes
		MessageBox(NULL, "Goodbye!", "fairwell", MB_OK);
		break;
	case WM_SHOWWINDOW: // when the window first loads (only fires once, so perfect for a welcome message)
	{
		int result = MessageBox(NULL, "Welcome!! This is a template for a chat program client window. This program captures 25 various Windows messages and handles them in different ways.", "Greetings", MB_OK);
		if (result == IDOK) // throw a timer message to start he timer AFTER the messagebox has been seen.  This keeps the move message from appearing at the beginning.
			SendMessage(hwnd, WM_TIMER, wparam, lparam);
	}
	break;
	case WM_TIMER: // user timer message to set timer AFTER the initial welcome message
		start = GetTickCount();
		break;
	case WM_MOVE: // if user ATTEMPTS to move the window
		duration2 = (GetTickCount() - start) / (double)CLOCKS_PER_SEC;
		RECT rect;
		GetWindowRect(hwnd, &rect);
		if ((rect.right != startx || rect.bottom != starty) && duration2 > 1) // make sure message doesn't display as window is being created
		{
			SetWindowPos(hwnd, HWND_TOP, startx, starty, 0, 0, SWP_NOSIZE); // set window to ONLY allowed position
			if (!movedBack)
			{
				MessageBox(NULL, "This window cannot be dragged around.", "here to stay", MB_OK);
				movedBack = true;
			}
			else
				movedBack = false;
		}
		break;
	case WM_LBUTTONDOWN: // these next four just play fun sounds with left/right mouse up/down
		PlaySound(MAKEINTRESOURCE(LEFTMOUSEDOWN), NULL, SND_RESOURCE | SND_ASYNC);
		break;
	case WM_LBUTTONUP:
		PlaySound(MAKEINTRESOURCE(LEFTMOUSEUP), NULL, SND_RESOURCE | SND_ASYNC);
		break;
	case WM_RBUTTONDOWN:
		PlaySound(MAKEINTRESOURCE(RIGHTMOUSEDOWN), NULL, SND_RESOURCE | SND_ASYNC);
		break;
	case WM_RBUTTONUP:
		PlaySound(MAKEINTRESOURCE(RIGHTMOUSEUP), NULL, SND_RESOURCE | SND_ASYNC);
		break;
	default:
		break;
	}
	return DefWindowProc(hwnd, msg, wparam, lparam);
}

// window procedure to hand messages caught in the Edit (TextBox) for sending messages
LRESULT CALLBACK subEditProc(HWND hwnd, UINT msg, WPARAM wParam, LPARAM lParam)
{
	switch (msg)
	{
	case WM_LBUTTONDBLCLK: // if user double-clicks, select all the text in that box
		SendMessage(hwnd, EM_SETSEL, 0, 100);
		break;
	case WM_KEYUP: // handle key up for lower pitch typing sound
		if (wParam != VK_RETURN)
			PlaySound(MAKEINTRESOURCE(TYPESOUND2), NULL, SND_RESOURCE | SND_NOSTOP | SND_ASYNC);
		break;
	case WM_KEYDOWN: // handle key down for higher pitch typing sound
		if (wParam != VK_RETURN)
			PlaySound(MAKEINTRESOURCE(TYPESOUND), NULL, SND_RESOURCE | SND_NOSTOP | SND_ASYNC);
		break;
	case WM_CHAR: // if user enters a character
		switch (wParam)
		{
		case VK_RETURN: // if that character was ENTER send the message (alternative to send button)
		{
			if (usernameEntered) // handles regular sending of messages
			{
				for (int i = 0; i < 256; i++)
					textSaved[i] = '\0';
				int gwtstat = GetWindowText(TextBox, &textSaved[0], 256);
				if (gwtstat == 0)
					break;
				userinput = username + ": " + textSaved;
				AddToChat(userinput);
				SetWindowText(TextBox, "");
				PlaySound(MAKEINTRESOURCE(SENDSOUND), NULL, SND_RESOURCE | SND_ASYNC);
			}
			else // get username at beginning
			{
				char user[25];
				GetWindowText(hwnd, &user[0], 25);
				username = user;
				usernameLength = username.size();
				if (usernameLength < 1)
					return 0;
				usernameEntered = true;
				SetWindowText(ReadOnly, "");
				SetWindowText(TextBox, "");
				AddToChat("Welcome, " + username + "!");
				ShowWindow(Combo, SW_SHOW);
				ShowWindow(Button2, SW_SHOW);
			}
		}
		return 0;  //or return 0; if you don't want to pass it further to def proc
				   //If not your key, skip to default:
		}
	default:
		return CallWindowProc(oldEditProc, hwnd, msg, wParam, lParam);
	}
	return 0;
}



