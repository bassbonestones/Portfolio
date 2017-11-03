#include "Computer.h"

Computer::Computer() : _type("unknown"), _make("unknown"), _model("unknown"), _processorName("unknown"), _GB_storage("unknown"), _costNew("unknown"),
	_GB_RAM("unknown")
{
}

Computer::Computer(string p_make, string p_model, string p_cpu, string p_storage, string p_cost, string p_ram) : _type("unknown"), _make(p_make), 
	_model(p_model), _processorName(p_cpu), _GB_storage(p_storage), _costNew(p_cost), _GB_RAM(p_ram)
{ 
}

void Computer::setMake(string p_make)
{
	_make = p_make;
}

string Computer::getMake()
{
	return _make;
}

void Computer::setModel(string p_model)
{
	_model = p_model;
}

string Computer::getModel()
{
	return _model;
}

void Computer::setProcessorName(string p_cpu)
{
	_processorName = p_cpu;
}

string Computer::getProcessorName()
{
	return _processorName;
}

void Computer::setStorageGB(string p_storage)
{
	_GB_storage = p_storage;
}

string Computer::getStorageGB()
{
	return _GB_storage;
}

void Computer::setCost(string p_cost)
{
	_costNew = p_cost;
}

string Computer::getCost()
{
	bool hasDec = false;
	for each(char c in _costNew)
		if (c == '.')
			hasDec = true;
	if (hasDec)
	{
		if (_costNew.length() == _costNew.find('.') + 1)
			return _costNew + "00";
		if (_costNew.length() == _costNew.find('.') + 2)
			return _costNew + "0";
	}
	else
	{
		return _costNew + ".00";
	}
	return _costNew;
}

void Computer::setRAM(string p_ram)
{
	_GB_RAM = p_ram;
}

string Computer::getRAM()
{
	return _GB_RAM;
}

void Computer::setType(string p_type)
{
	_type = p_type;
}

string Computer::getType()
{
	return _type;
}