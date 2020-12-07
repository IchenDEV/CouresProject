#pragma once
#include <string>

class Generative {
public:
	char K='S';
	std::string S;
	Generative(std::string);
	Generative();

	bool operator==(Generative);
};
