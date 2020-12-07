#include "Grammar.h"

Grammar::Grammar(std::vector<Generative>& item)
{
	Generatives.assign(item.begin(), item.end());
	this->genVnVt();
}

Grammar::Grammar()
{
}
Grammar Grammar::Expand()
{
	int countS = 0;
	for (Generative line : Generatives)
		if (line.K == 'S')
			countS++;

	if (countS == 1)
		return Grammar(this->Generatives);

	std::vector<Generative> tmp;
	tmp.assign(Generatives.begin(), Generatives.end());


	for (auto line : tmp)
		if (line.K == 'S')line.K = '$';

	Generative gv("S->$");
	tmp.push_back(gv);
	return Grammar(tmp);
}
void Grammar::genVnVt()
{
	for (auto item : Generatives) {
		Vn.insert(item.K);
		V.insert(item.K);
	}

	for (auto item : Generatives)
		for (auto it : item.S)
			if (Vn.count(it) == 0) {
				Vt.insert(it);
				V.insert(it);
			}

}

