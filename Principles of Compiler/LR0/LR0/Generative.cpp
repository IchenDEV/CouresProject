#include "Generative.h"

Generative::Generative(std::string oriStr)
{
	//if (oriStr.length() < 3)
	//	throw - 1;

	//if (oriStr[1] != '-' || oriStr[1] != '>')
	//	throw - 1;
	
	this->K = oriStr[0];
	this->S = oriStr.substr(3);
}

Generative::Generative()
{
}

bool Generative::operator==(Generative b)
{
	Generative& a = *this;
	if (a.K == b.K && a.S == b.S) 
		return true;

	return false;
}
