#include "MobileDevice.h"

MobileDevice::MobileDevice() : Computer(), _IN_screenSizeDiag("unknown")
{
}

MobileDevice::MobileDevice(string p_make, string p_model, string p_cpu, string p_storage, string p_cost, string p_ram, 
	string p_sizeScreen) : Computer(p_make, p_model, p_cpu, p_storage, p_cost, p_ram), _IN_screenSizeDiag(p_sizeScreen)
{
}

void MobileDevice::setScreenSizeDiag(string p_sizeScreen)
{
	_IN_screenSizeDiag = p_sizeScreen;
}

string MobileDevice::getScreenSizeDiag()
{
	return _IN_screenSizeDiag;
}