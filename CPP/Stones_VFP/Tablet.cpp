#include "Tablet.h"

Tablet::Tablet() : MobileDevice(), _LCD_density("unknown"), _hasMic("unknown")
{
	setType("Tablet");
}

Tablet::Tablet(string p_make, string p_model, string p_cpu, string p_storage, string p_cost, string p_ram, string p_sizeScreen,
	string p_density, string p_hasMic) : MobileDevice(p_make, p_model, p_cpu, p_storage, p_cost, p_ram, p_sizeScreen),
	_LCD_density(p_density), _hasMic(p_hasMic)
{
	setType("Tablet");
}

Tablet::Tablet(string p_fileLine)
{
	string line = p_fileLine;
	int numCommas;
	string token;
	int delimPos;
	string tokens[9];
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
	initializeMobile(tokens[0], tokens[1], tokens[2], tokens[3], tokens[4], tokens[5], tokens[6], tokens[7], tokens[8]);
}

void Tablet::initializeMobile(string p_make, string p_model, string p_cpu, string p_storage, string p_cost, string p_ram, string p_sizeScreen,
	string p_density, string p_hasMic)
{
	setMake(p_make);
	setModel(p_model);
	setProcessorName(p_cpu);
	setStorageGB(p_storage);
	setCost(p_cost);
	setRAM(p_ram);
	setScreenSizeDiag(p_sizeScreen);
	setDensityLCD(p_density);
	setHasMic(p_hasMic);
	setType("Tablet");
}

void Tablet::setDensityLCD(string p_density)
{
	_LCD_density = p_density;
}

string Tablet::getDensityLCD()
{
	return _LCD_density;
}

void Tablet::setHasMic(string p_trueORfalse)
{
	if (p_trueORfalse == "n") {
		p_trueORfalse = "N";
	}
	if (p_trueORfalse == "y") {
		p_trueORfalse = "Y";
	}
	

	_hasMic = p_trueORfalse;
}

string Tablet::getHasMic()
{
	return _hasMic;
}

string Tablet::getFileString()
{
	return "Tablet," + getMake() + "," + getModel() + "," + getProcessorName() + "," + getStorageGB() + "," + getCost() + "," +
		getRAM() + "," + getScreenSizeDiag() + "," + getDensityLCD() + "," + getHasMic();
}

void Tablet::printSpecs() {

	cout << "\t~Chosen Tablets Specs~" << endl << endl
		<< "Make: " << getMake() << endl
		<< "Model: " << getModel() << endl
		<< "Processor: " << getProcessorName() << endl
		<< "Storage: " << getStorageGB() << " GB" << endl
		<< "RAM: " << getRAM() << " GB" << endl
		<< "ScreenSize: " << getScreenSizeDiag() << " in." << endl
		<< "Density LCD: " << getDensityLCD() << endl
		<< "Has Mic: " << getHasMic() << endl
		<< "Price (new): $" << getCost() << endl << endl
		<< "Press any key to return to the tablets menu." << endl;
	_getch();


}