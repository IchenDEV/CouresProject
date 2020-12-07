#include<iostream>
#include <fstream>
#include<string>
#include<vector>
#include<set>
#include "Generative.h"
#include "Grammar.h"
#include "Item.h"
#include <map>

class GaM
{
public:
	Grammar grammar;
	std::map<char, std::string>* ACTION;
	std::map <char, std::string>* GOTO;
	std::vector<std::vector<Item>> C;
	GaM(Grammar&);
	void LR0();
private:
	std::vector<Item> Closure(std::vector<Item>);
	std::vector<Item> Go(std::vector<Item>, char);
};

GaM::GaM(Grammar& inputs)
{
	grammar = inputs;
}


void GaM::LR0()
{
	auto expandedGrammer = grammar.Expand();

	Generative gen;
	for (auto i : expandedGrammer.Generatives)
		if (i.K == 'S') {
			gen = i;
			break;
		}

	Item item(gen, 0);
	std::vector<Item> I;
	I.push_back(item);
	auto I0 = Closure(I);
	C.push_back(I0);
	int lc = C.size();
	for (int i = 0; i < C.size(); i++) {
		for (auto t : grammar.V) {
			auto tmp = Go(C[i], t);
			if (tmp.size() > 0) {
				bool isIn = false;
				for (auto p : C)
					if (std::equal(p.begin(), p.end(), tmp.begin(), tmp.end()))
						isIn = true;

				if (!isIn)
					C.push_back(tmp);
			}
		}
	}

	ACTION = new std::map<char, std::string>[C.size()];
	GOTO = new std::map<char, std::string>[C.size()];

	for (int k = 0; k < C.size(); k++) {

		for (auto i : C[k]) {
			if (i.active < i.raw.S.length()) {
				auto a = i.raw.S[i.active];
				if (grammar.Vt.count(a) > 0) {
					auto Ij = Go(C[k], a);
					for (int j = 0; j < C.size(); j++) {
						if (std::equal(C[j].begin(), C[j].end(), Ij.begin(), Ij.end())) {

							ACTION[k][a] = "s"; ACTION[k][a] += std::to_string(j);
						}
					}
				}

			}
			else if (i.active == i.raw.S.length()) {

				for (int j = 1; j < grammar.Generatives.size(); j++) {
					if (grammar.Generatives[j] == i.raw) {
						for (auto a : grammar.Vt) {
							ACTION[k][a] = "r";
							ACTION[k][a] += std::to_string(j);
						}
						ACTION[k]['#'] = "r";
						ACTION[k]['#'] += std::to_string(j);
					}
				}

			}
			if (i.raw.K == 'S' && i.raw.S.length() == i.active) {
				ACTION[k]['#'] = "acc";
			}
		}

		for (int j = 0; j < C.size(); j++) {
			for (auto a : grammar.Vn) {
				auto Ij = Go(C[k], a);
				if (std::equal(C[j].begin(), C[j].end(), Ij.begin(), Ij.end())) {
					GOTO[k][a] = std::to_string(j);
				}
			}
		}
	}
}


std::vector<Item> GaM::Closure(std::vector<Item> i)
{
	std::vector<Item> j;
	j.assign(i.begin(), i.end());
	for (int k = 0; k < j.size(); k++) {
		auto ic = j[k].raw.S[j[k].active];
		for (Generative gm : grammar.Generatives) {
			if (gm.K == ic) {
				bool isInJ = false;
				Item nx(gm);
				for (auto cx : j)
					if (cx == nx)
						isInJ = true;
				if (!isInJ)
					j.push_back(nx);
			}
		}
	}
	return j;
}

std::vector<Item> GaM::Go(std::vector<Item> I, char X)
{
	std::vector<Item> J;
	for (Item item : I)
		if (item.active < item.raw.S.length() && item.raw.S[item.active] == X) {
			Item ic(item.raw, item.active + 1);
			J.push_back(ic);
		}
	return Closure(J);
}





int main(int argc, char** args) {

	std::ifstream fin;
	fin.open("inputs.txt");

	std::ofstream fout;
	fout.open("output.txt");

	std::string line;
	std::vector<Generative> inputs;

	while (fin >> line)
		inputs.push_back(Generative(line));

	Grammar grammar(inputs);
	auto gm = new GaM(grammar);
	gm->LR0();


	fout << "\t\t\tACTION\r\n";
	fout << ' ' << "\t";
	for (auto c : grammar.Vt)
		fout << c << "\t";
	fout << '#' << "\t";

	fout << std::endl;
	for (int j = 0; j < gm->C.size(); j++) {
		fout << j << "\t";
		for (auto c : grammar.Vt)
			fout << gm->ACTION[j][c] << "\t";
		fout << gm->ACTION[j]['#'] << "\t";
		fout << std::endl;
	}


	fout << std::endl;
	fout << "\t\t\tGOTO\r\n";
	grammar.Vn.erase('S');
	fout << std::endl << " \t";
	for (auto c : grammar.Vn)
		fout << c << "\t";
	fout << std::endl;
	for (int j = 0; j < gm->C.size(); j++) {
		fout << j << "\t";
		for (auto c : grammar.Vn)
			fout <<  gm->GOTO[j][c] << "\t";
		fout << std::endl;
	}
	return 0;
}