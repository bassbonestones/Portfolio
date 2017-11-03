#include "PC.h"

PC::PC() : Computer(), _graphicsCard("unknown"), _os("unknown")
{
}

PC::PC(string p_make, string p_model, string p_cpu, string p_storage, string p_cost, string p_ram, string p_gCard, string p_os) :
	Computer(p_make, p_model, p_cpu, p_storage, p_cost, p_ram), _graphicsCard(p_gCard), _os(p_os)
{
}

void PC::setGraphicsCard(string p_gCard)
{
	_graphicsCard = p_gCard;
}

string PC::getGraphicsCard()
{
	return _graphicsCard;
}

void PC::setOS(string p_os)
{
	_os = p_os;
}

string PC::getOS()
{
	return _os;
}

