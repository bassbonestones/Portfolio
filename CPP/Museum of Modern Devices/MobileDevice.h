#ifndef MOBILEDEVICE_H
#define MOBILEDEVICE_H
#include "Computer.h"

class MobileDevice : public Computer
{
public:
	MobileDevice();
	MobileDevice(string p_make, string p_model, string p_cpu, string p_storage, string p_cost, string p_ram, string p_sizeScreen);
	void setScreenSizeDiag(string p_sizeScreen);
	string getScreenSizeDiag();
	virtual void printSpecs() = 0;
	virtual void initializeMobile(string, string, string, string, string, string, string, string, string) = 0;
	virtual string getFileString() = 0;
	~MobileDevice() {}
private:
	string _IN_screenSizeDiag;
};

#endif // !MOBILEDEVICE_H