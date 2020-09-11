#ifndef _EEPROMManager_h
#define _EEPROMManager_h

#if defined(ARDUINO) && ARDUINO >= 100
#include "arduino.h"
#else
#include "WProgram.h"
#endif

#include <EEPROM.h>

class EEPROMManager
{
public:
	EEPROMManager();
	void putString(char address, String data);
	String getString(char address);

	void putBoolean();
	bool getBoolean();
};

#endif
