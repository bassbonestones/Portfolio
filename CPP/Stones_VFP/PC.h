#ifndef PC_H
#define PC_H
#include "Computer.h"

class PC : public Computer
{
public:
	PC();
	PC(string p_make, string p_model, string p_cpu, string p_storage, string p_cost, string p_ram, string p_gCard, string p_os);
	void setGraphicsCard(string p_gCard);
	string getGraphicsCard();
	void setOS(string p_os);
	string getOS();
	virtual void printSpecs() = 0;
	virtual void initializePC(string, string, string, string, string, string, string, string, string, string) = 0;
	virtual string getFileString() = 0;
	~PC() {}
private:
	string _graphicsCard;
	string _os;
};


#endif // !PC_H