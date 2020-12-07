#include <windows.h>
#include <stdio.h>
#include <time.h>
#include<iostream>
#include<fstream>
using namespace std;
DWORD dwID;

#define C(S) CreateSemaphore(NULL, 1, 3, (S)) 
#define P(S) WaitForSingleObject((S), INFINITE)
#define V(S) ReleaseSemaphore((S), 1, NULL)
#define CreateThreadEasy(func, args) CreateThread(NULL, 0, (func), (args), 0, &dwID)
#define Wait(num, S) WaitForMultipleObjects((num), (S), true, INFINITE)

struct ThreadInfo
{
    int id;
    char type;
    double s;
    double d;
};

ThreadInfo threads[100];
HANDLE hThread[100];
HANDLE s1, s2, s3;
int c = 0;
int Reader_Count = 0;

void Read_ThreadInfo();
void RP();
DWORD WINAPI Reader(LPVOID lpParam);
DWORD WINAPI Writer(LPVOID lpParam);


void Read_ThreadInfo()
{
    ifstream fin("in.io");
    while (fin >> threads[c].id >> threads[c].type >> threads[c].s >> threads[c].d) c++;
}

void RP()
{
    
    s1 = C(TEXT("sig1"));
    s2 = C(TEXT("sig2"));
    for (int i = 0; i < c; ++i) 
        if (threads[i].type == 'W') 
            hThread[i] = CreateThreadEasy(Writer, &threads[i]);
        else 
            hThread[i] = CreateThreadEasy(Reader, &threads[i]);
    
    Wait(c, hThread); //等待所有线程结束
}

DWORD WINAPI Reader(LPVOID lpParam)
{
    ThreadInfo* arg = (ThreadInfo*)lpParam;
    P(s1);
    Reader_Count++;
    if (Reader_Count == 1) 
        P(s2);
    V(s1);

    printf("线程 %d 正在读!\n", arg->id);
    Sleep(arg->d * 1000);
    printf("线程 %d 读完了!\n", arg->id);

    P(s1);
    Reader_Count--;
    if (Reader_Count == 0) 
        V(s2);
    V(s1);
    return 0;
}
DWORD WINAPI Writer(LPVOID lpParam)
{
    ThreadInfo* arg = (ThreadInfo*)lpParam;
    P(s2);
    printf("线程 %d 正在写!\n", arg->id);
    Sleep(arg->d * 1000);
    printf("线程 %d 写完了!\n", arg->id);
    V(s2);
    return 0;
}

int main()
{
    Read_ThreadInfo();
    RP();
    return 0;
}