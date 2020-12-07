#pragma once
#include "Generative.h"
class Item {
public:
	Generative raw;
	int active = 0;
	Item(Generative,int=0);
	bool operator==(Item);
};