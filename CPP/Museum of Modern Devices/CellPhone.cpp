#include "CellPhone.h"

CellPhone::CellPhone() : MobileDevice(), _SIM_cardSize("unknown")
{
	setDimensions("unknown", "unknown", "unknown");
	setType("CellPhone");
}


CellPhone::CellPhone(string p_make, string p_model, string p_cpu, string p_storage, string p_cost, string p_ram, string p_sizeScreen,
	string p_sizeSIM, string p_length, string p_width, string p_height) : MobileDevice(p_make, p_model, p_cpu, p_storage, p_cost,
	p_ram, p_sizeScreen), _SIM_cardSize(p_sizeSIM)
{
	setDimensions(p_length, p_width, p_height);
	setType("CellPhone");
}

CellPhone::CellPhone(string p_fileLine)
{
	string line = p_fileLine;
	int numCommas;
	string token;
	int delimPos;
	string tokens[11];
	delimPos = line.find_first_of(',');
	line = line.substr(delimPos + 1, line.size() - 1);
	numCommas = (int)count(line.begin(), line.end(), ',');
	for (int i = 0; i < numCommas; i++)
	{
		delimPos = line.find_first_of(',');
		token = line.substr(0, delimPos);
		tokens[i] = token;
		line = line.substr(delimPos + 1, line.size() - 1);
		if (i == numCommas - 1)
		{
			tokens[numCommas] = line;
		}
	}
	initializeMobile(tokens[0], tokens[1], tokens[2], tokens[3], tokens[4], tokens[5], tokens[6], tokens[7], tokens[8] + "," + tokens[9] + "," + tokens[10]);
	
}


void CellPhone::initializeMobile(string p_make, string p_model, string p_cpu, string p_storage, string p_cost, string p_ram, string p_sizeScreen,
	string p_sizeSIM, string p_dimensions)
{
	string dims = p_dimensions;
	string length;
	string width;
	string height;
	int decPosition = dims.find_first_of(',');
	length = dims.substr(0, decPosition);
	dims = dims.substr(decPosition + 1);
	decPosition = dims.find_first_of(',');
	width = dims.substr(0, decPosition);
	height = dims.substr(decPosition + 1);

	setMake(p_make);
	setModel(p_model);
	setProcessorName(p_cpu);
	setStorageGB(p_storage);
	setCost(p_cost);
	setRAM(p_ram);
	setScreenSizeDiag(p_sizeScreen);
	setSIM(p_sizeSIM);
	setDimensions(length, width, height);
	setType("CellPhone");
}


void CellPhone::setSIM(string p_sizeSIM)
{
	_SIM_cardSize = p_sizeSIM;
}

string CellPhone::getSIM()
{
	return _SIM_cardSize;
}

void CellPhone::setDimensions(string p_length, string p_width, string p_height)
{
	_IN_dimensions._length = p_length;
	_IN_dimensions._width = p_width;
	_IN_dimensions._height = p_height;
}

string CellPhone::getDimensionL()
{
	return _IN_dimensions._length;
}

string CellPhone::getDimensionW()
{
	return _IN_dimensions._width;
}

string CellPhone::getDimensionH()
{
	return _IN_dimensions._height;
}

string CellPhone::getAllDimensions()
{
	return _IN_dimensions._length + "in. X " + _IN_dimensions._width + "in. X" + _IN_dimensions._height + "in.";
}

string CellPhone::getFileString()
{
	return "CellPhone," + getMake() + "," + getModel() + "," + getProcessorName() + "," + getStorageGB() + "," + getCost() + "," +
		getRAM() + "," + getScreenSizeDiag() + "," + getSIM() + "," + getDimensionL() + "," + getDimensionW()  + "," + getDimensionH();
}
void CellPhone::printSpecs() {
	cout << "\t~Chosen Cellphone Specs~" << endl << endl
		<< "Make: " << getMake() << endl
		<< "Model: " << getModel() << endl
		<< "Processor: " << getProcessorName() << endl
		<< "Storage: " << getStorageGB() << " GB" << endl
		<< "RAM: " << getRAM() << " GB" << endl
		<< "ScreenSize: " << getScreenSizeDiag() << " in." << endl
		<< "SIM: " << getSIM() << endl
		<< "Length: " << getDimensionL() << " in." << endl
		<< "Width: " << getDimensionW() << " in." << endl
		<< "Height: " << getDimensionH() << " in." << endl
		<< "Price (new): $" << getCost() << endl << endl
		<< "Press any key to return to the cell phone menu." << endl;
	_getch();

}