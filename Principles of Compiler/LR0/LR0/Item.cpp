#include "Item.h"

Item::Item(Generative g, int a)
{
	raw = g;
	active = a;
}

bool Item::operator==(Item i)
{
	if (this->active == i.active && this->raw == i.raw)
		return true;
	return false;
}
