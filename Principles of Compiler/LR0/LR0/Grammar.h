#pragma once
#include <vector>
#include "Generative.h"
#include <set>

class Grammar {
public:
	std::set<char> Vn;
	std::set<char> Vt;
	std::set<char> V;
	std::vector<Generative> Generatives;

	Grammar(std::vector<Generative>&);
	Grammar();

	Grammar Expand();
private:
	void genVnVt();
};

