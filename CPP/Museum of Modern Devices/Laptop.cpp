#include "Laptop.h"

Laptop::Laptop() : PC(), _LB_weight("unknown"), _wifiCardType("unknown")
{
	setType("Laptop");
}

Laptop::Laptop(string p_make, string p_model, string p_cpu, string p_storage, string p_cost, string p_ram, string p_gCard, 
	string p_os, string p_weight, string p_wifi) : PC(p_make, p_model, p_cpu, p_storage, p_cost, p_ram, p_gCard, p_os), 
	_LB_weight(p_weight), _wifiCardType(p_wifi)
{
	setType("Laptop");
}

Laptop::Laptop(string p_fileLine)
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

void Laptop::initializePC(string p_make, string p_model, string p_cpu, string p_storage, string p_cost, string p_ram, string p_gCard,
	string p_os, string p_weight, string p_wifi)
{
	setMake(p_make);
	setModel(p_model);
	setProcessorName(p_cpu);
	setStorageGB(p_storage);
	setCost(p_cost);
	setRAM(p_ram);
	setGraphicsCard(p_gCard);
	setOS(p_os);
	setWeightLB(p_weight);
	setWifiCardType(p_wifi);
	setType("Laptop");
}

void Laptop::setWeightLB(string p_weight)
{
	_LB_weight = p_weight;
}

string Laptop::getWeightLB()
{
	return _LB_weight;
}

void Laptop::setWifiCardType(string p_wifi)
{
	_wifiCardType = p_wifi;
}

string Laptop::getWifiCardType()
{
	return _wifiCardType;
}

string Laptop::getFileString()
{
	return "Laptop," + getMake() + "," + getModel() + "," + getProcessorName() + "," + getStorageGB() + "," + getCost() + "," +
		getRAM() + "," + getGraphicsCard() + "," + getOS() + "," + getWeightLB() + "," + getWifiCardType();
}

void Laptop::printSpecs()
{
	cout << "\t~Chosen Laptop Specs~" << endl << endl
		<< "Make: " << getMake() << endl
		<< "Model: " << getModel() << endl
		<< "Processor: " << getProcessorName() << endl
		<< "Storage: " << getStorageGB() << " GB" << endl
		<< "RAM: " << getRAM() << " GB" << endl
		<< "Graphics: " << getGraphicsCard() << endl
		<< "OS: " << getOS() << endl
		<< "Weight: " << getWeightLB() << " lb(s)" << endl
		<< "Wifi Card: " << getWifiCardType() << endl
		<< "Price (new): $" << getCost() << endl << endl
		<< "Press any key to return to the laptops menu." << endl;
	_getch();
}