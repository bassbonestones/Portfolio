#ifndef COMPUTER_H
#define COMPUTER_H
#include "Validation.h"

class Computer
{
public:
	Computer();
	Computer(string p_make, string p_model, string p_cpu, string p_storage, string p_cost, string p_ram);
	void setMake(string p_make);
	string getMake();
	void setModel(string p_model);
	string getModel();
	void setProcessorName(string p_cpu);
	string getProcessorName();
	void setStorageGB(string p_storage);
	string getStorageGB();
	void setCost(string p_cost);
	string getCost();
	void setRAM(string p_ram);
	string getRAM();
	void setType(string p_type);
	string getType();
	virtual void printSpecs() = 0;
	virtual string getFileString() = 0;
	virtual ~Computer() {}
private:
	string _type;
	string _make;
	string _model;
	string _processorName;
	string _GB_storage;
	string _costNew;
	string _GB_RAM;
};

#endif // !COMPUTER_H