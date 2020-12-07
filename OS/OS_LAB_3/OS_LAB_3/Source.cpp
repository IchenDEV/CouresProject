#include<stdio.h>
#include<stdlib.h>
#include<string>
#include<iostream>
using namespace std;

int Available[3] = { 3,3,2 };

bool Finish[5] = { false,false, false, false, false };

int Max[5][3] = {
	{ 7,5,3 } ,
	{ 3,2,2 } ,
	{ 9,0,2 } ,
	{ 2,2,2 } ,
	{ 4,3,3 } };

int Allocation[5][3] = {
	{ 0,1,0 },
	{ 2,0,0 },
	{ 3,0,2 },
	{ 2,1,1 },
	{ 0,0,2 } };

int Need[5][3] = {
	{ 7,4,3 },
	{ 1,2,2 },
	{ 6,0,0 },
	{ 0,1,1 },
	{ 4,3,1 } };

int request[3] = { 0,0,0 };
int Work[3] = { 0,0,0 };
int pah[5] = { -1,-1,-1,-1,-1 };//安全序列

int M = 5, N = 3;

void Show() {
	cout << "process    work     Need     Allocation   Work+Allocation  " << endl;
	for (int i = 0; i < M; i++) {
		int n = i;
		cout << "P" << n << "     ";
		for (int j = 0; j < N; j++)
			cout << Work[j] << "  ";

		cout << "|";
		for (int j = 0; j < N; j++)
			cout << Need[n][j] << "  ";

		cout << "|";
		for (int j = 0; j < N; j++)
			cout << Allocation[n][j] << "  ";


		cout << "|";

		for (int j = 0; j < N; j++)
			cout << Work[j]+ Allocation[n][j] << "  ";
		cout << endl;
	}
}
bool compareto(int req[], int arr[]) {
	return req[0] <= arr[0] && req[1] <= arr[1] && req[2] <= arr[2];
}

int security() {
	bool a = true;
	cout << "process\tA\tB\tC" << endl;


	for (int i = 0; i < N; i++)
		Work[i] = Available[i];

	for (int i = 0; i < M; i++)
		Finish[i] = false;

	bool flag = true;
	do 
	{
		flag = false;
		for (int i = 0; i < 5; i++)
		{
			if (Finish[i] == false && compareto(Need[i], Work))
			{
				flag = true;	
				cout << "P" << i << "\t";
				for (int j = 0; j < 3; j++)
					cout << Work[j] << "\t" ;
				cout << endl;
				for (int j = 0; j < 3; j++) {
					Work[j] += Allocation[i][j];
					Finish[i] = true;
				}
				break;
			}
		}

	} while (flag);
	
	for (int i = 0; i < 5; i++)
		if (Finish[i] == false)
			return false;

	return true;
}
void Request() {
	int p;
	while (cin >> p) {
		for (int i = 0; i < 3; i++)
			cin >> request[i];

		if (compareto(request, Need[p])) {
			if (compareto(request, Available)) {
				for (int i = 0; i < 3; i++) {
					Available[i] -= request[i];
					Allocation[p][i] += request[i];
					Need[p][i] -= request[i];
				}
				
				if (!security()) {
					for (int i = 0; i < 3; i++) {
						Available[i] += request[i];
						Allocation[p][i] -= request[i];
						Need[p][i] += request[i];
					}
					cout << "不安全";
				}
				else{
					Show();
				}
			}
			else if (!compareto(request, Available)) {
				cout << "please wait";
			}
		}
		else if (!compareto(request, Need[p])) {
			cout << "value error";
		}
	}


}

int main() {
	Show();
	security();
	Request();
	system("pause");
	return 0;
}
