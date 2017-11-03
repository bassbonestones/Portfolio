#ifndef TABLET_H
#define TABLET_H
#include "MobileDevice.h"

class Tablet : public MobileDevice
{
public:
	Tablet();
	Tablet(string p_make, string p_model, string p_cpu, string p_storage, string p_cost, string p_ram, string p_sizeScreen, 
		string p_density, string p_hasMic);
	Tablet(string p_fileLine);
	void initializeMobile(string p_make, string p_model, string p_cpu, string p_storage, string p_cost, string p_ram, string p_sizeScreen,
		string p_density, string p_hasMic);
	void setDensityLCD(string p_density);
	string getDensityLCD();
	void setHasMic(string p_trueORfalse);
	string getHasMic();
	string getFileString();
	void printSpecs();
	~Tablet(){}
private:
	string _LCD_density;
	string _hasMic;
};

#endif // !TABLET_H
