#include "Desktop.h"

Desktop::Desktop() : PC(), _numExpansionSlots("unknown"), _hasDVD("unknown")
{
	setType("Desktop");
}

Desktop::Desktop(string p_make, string p_model, string p_cpu, string p_storage, string p_cost, string p_ram, string p_gCard,
	string p_os, string p_numSlots, string p_hasDVD) : PC(p_make, p_model, p_cpu, p_storage, p_cost, p_ram, p_gCard, p_os),
	_numExpansionSlots(p_numSlots), _hasDVD(p_hasDVD)
{
	setType("Desktop");
}

Desktop::Desktop(string p_fileLine)
{
	string line = p_fileLine;
	int numCommas;
	string token;
	int delimPos;
	string tokens[10];
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
	initializePC(tokens[0], tokens[1], tokens[2], tokens[3], tokens[4], tokens[5], tokens[6], tokens[7], tokens[8], tokens[9]);
}

void Desktop::initializePC(string p_make, string p_model, string p_cpu, string p_storage, string p_cost, string p_ram, string p_gCard,
	string p_os, string p_numSlots, string p_hasDVD)
{
	setMake(p_make);
	setModel(p_model);
	setProcessorName(p_cpu);
	setStorageGB(p_storage);
	setCost(p_cost);
	setRAM(p_ram);
	setGraphicsCard(p_gCard);
	setOS(p_os);
	setNumExpansionSlots(p_numSlots);
	setHasDVD(p_hasDVD);
	setType("Desktop");
}

void Desktop::setNumExpansionSlots(string p_numSlots)
{
	_numExpansionSlots = p_numSlots;
}

string Desktop::getNumExpansionSlots()
{
	return _numExpansionSlots;
}

void Desktop::setHasDVD(string p_true_OR_false)
{
	if (p_true_OR_false == "n" ) {
		p_true_OR_false = "N";
	}
	if (p_true_OR_false == "y" ) {
		p_true_OR_false = "Y";
	}
	_hasDVD = p_true_OR_false;
}

string Desktop::getHasDVD()
{
	return _hasDVD;
}

string Desktop::getFileString()
{
	return "Desktop," + getMake() + "," + getModel() + "," + getProcessorName() + "," + getStorageGB() + "," + getCost() + "," +
		getRAM() + "," + getGraphicsCard() + "," + getOS() + "," + getNumExpansionSlots() + "," + getHasDVD();
}

void Desktop::printSpecs()
{
	cout << "\t~Chosen Desktop Specs~" << endl << endl
		<< "Make: " << getMake() << endl
		<< "Model: " << getModel() << endl
		<< "Processor: " << getProcessorName() << endl
		<< "Storage: " << getStorageGB() << " GB" << endl
		<< "RAM: " << getRAM() << " GB" << endl
		<< "Graphics: " << getGraphicsCard() << endl
		<< "OS: " << getOS() << endl
		<< "Number of Expansion Slots: " << getNumExpansionSlots() << endl
		<< "Has DVD Drive: " << getHasDVD() << endl
		<< "Price (new): $" << getCost() << endl << endl
		<< "Press any key to return to the desktops menu." << endl;
	_getch();
}