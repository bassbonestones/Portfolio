#ifndef DESKTOP_H
#define DESKTOP_H
#include "PC.h"


class Desktop : public PC
{
public:
	Desktop();
	Desktop(string p_make, string p_model, string p_cpu, string p_storage, string p_cost, string p_ram, string p_gCard, 
		string p_os, string p_numSlots, string p_hasDVD);
	Desktop(string p_fileLine);
	void initializePC(string p_make, string p_model, string p_cpu, string p_storage, string p_cost, string p_ram, string p_gCard,
		string p_os, string p_numSlots, string p_hasDVD);
	void setNumExpansionSlots(string p_numSlots);
	string getNumExpansionSlots();
	void setHasDVD(string p_true_OR_false);
	string getHasDVD();
	string getFileString();
	void printSpecs();
	~Desktop() {}
private:
	string _numExpansionSlots;
	string _hasDVD;
};

#endif // !DESKTOP_H