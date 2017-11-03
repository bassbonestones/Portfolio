#ifndef LAPTOP_H
#define LAPTOP_H
#include "PC.h"

class Laptop : public PC
{
public:
	Laptop();
	Laptop(string p_make, string p_model, string p_cpu, string p_storage, string p_cost, string p_ram, string p_gCard, 
		string p_os, string p_weight, string p_wifi);
	Laptop(string p_fileLine);
	void initializePC(string p_make, string p_model, string p_cpu, string p_storage, string p_cost, string p_ram, string p_gCard,
		string p_os, string p_weight, string p_wifi);
	void setWeightLB(string p_weight);
	string getWeightLB();
	void setWifiCardType(string p_wifi);
	string getWifiCardType();
	string getFileString();
	void printSpecs();
	~Laptop() {}
private:
	string _LB_weight;
	string _wifiCardType;
};

#endif // !LAPTOP_H
