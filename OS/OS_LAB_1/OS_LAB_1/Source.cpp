#include<iostream>
#include<Windows.h>
int main(int argc, char* argv[])
{
	STARTUPINFO si;
	PROCESS_INFORMATION pi;

	ZeroMemory(&si, sizeof(si));
	si.cb = sizeof(si);
	ZeroMemory(&pi, sizeof(pi));


	TCHAR name[] = TEXT("notepad.exe");



	unsigned int bid = CreateProcess(NULL,   // No module name (use command line)
		name,        // Command line
		NULL,           // Process handle not inheritable
		NULL,           // Thread handle not inheritable
		FALSE,          // Set handle inheritance to FALSE
		0,              // No creation flags
		NULL,           // Use parent's environment block
		NULL,           // Use parent's starting directory 
		&si,            // Pointer to STARTUPINFO structure
		&pi);          // Pointer to PROCESS_INFORMATION structure


	// Start the child process. 
	if (!bid)
	{
		printf("CreateProcess failed (%d).\n", GetLastError());
		ExitProcess(-1);
	}

	while (true)
	{
		std::cout << "Want close?(y/N)";
		char c;
		std::cin >> c;
		if (c == 'y' || c == 'Y') {
			TerminateProcess(pi.hProcess,0);
			ExitProcess(0);
		}
	}

	ExitProcess(0);
	return 0;
}