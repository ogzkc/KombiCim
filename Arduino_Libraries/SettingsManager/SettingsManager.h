#ifndef _SettingsManager_h
#define _SettingsManager_h

#if defined(ARDUINO) && ARDUINO >= 100
#include "arduino.h"
#endif

#include <ArduinoJson.h>

struct MinTemperature
{
    float Value;
    int Hour;
    int Minutes;
    int DayOfWeek;
};

struct Location
{
    String Name;
    String DeviceId;
    bool IsCenter;
    MinTemperature MinTemperatures[21];
    float MinTempValue;
};

struct Settings
{
    String Mode;
    String MinOperator;
    Location Locations[2];
    bool State;
};

class SettingsManager
{
public:
    SettingsManager();
    void Refresh(String jsonContent);
    Settings settings;
    String MODE_AUTO_SIMPLE_1;
    String MODE_MANUAL_1;
};

#endif