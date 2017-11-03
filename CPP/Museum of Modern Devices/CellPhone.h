#ifndef CELLPHONE_H
#define CELLPHONE_H
#include "MobileDevice.h"

class CellPhone : public MobileDevice
{
private:
	struct Dimensions
	{
		string _length;
		string _width;
		string _height;
	};
public:
	CellPhone();
	CellPhone(string p_make, string p_model, string p_cpu, string p_storage, string p_cost, string p_ram, string p_sizeScreen, 
		string p_sizeSIM, string p_length, string p_width, string p_height);
	CellPhone(string p_fileLine);
	void initializeMobile(string p_make, string p_model, string p_cpu, string p_storage, string p_cost, string p_ram, string p_sizeScreen,
		string p_sizeSIM, string dimensionsTogether);
	void setSIM(string p_sizeSIM);
	string getSIM();
	void setDimensions(string p_length, string p_width, string p_height);
	string getDimensionL();
	string getDimensionW();
	string getDimensionH();
	string getAllDimensions();
	string getFileString();
	void printSpecs();
	~CellPhone() {}
private:
	string _SIM_cardSize;
	Dimensions _IN_dimensions;
};

#endif // !CELLPHONE_H
