#include "SettingsManager.h"

SettingsManager::SettingsManager()
{
    MODE_AUTO_SIMPLE_1 = "auto_simple_1";
    MODE_MANUAL_1 = "manual_1";
}

void SettingsManager::Refresh(String jsonContent)
{
    DynamicJsonDocument root(4096);
    DeserializationError error = deserializeJson(root, jsonContent);
    if (error)
    {
        Serial.print(F("deserializeJson() failed: "));
        Serial.println(error.c_str());
        return;
    }

    settings.State = root["Settings"]["State"].as<bool>();
    settings.Mode = root["Settings"]["Mode"].as<String>();
    settings.MinOperator = root["Settings"]["MinOperator"].as<float>();

    int locationIndex = 0;
    JsonArray locationArr = root["Settings"]["Locations"].as<JsonArray>();
    for (JsonVariant locationObj : locationArr)
    {
        settings.Locations[locationIndex].Name = locationObj["Name"].as<String>();
        settings.Locations[locationIndex].DeviceId = locationObj["DeviceId"].as<String>();
        settings.Locations[locationIndex].IsCenter = locationObj["IsCenter"].as<bool>();
        settings.Locations[locationIndex].MinTempValue = locationObj["MinTempValue"].as<float>();

        Serial.println("locationObj:" + locationObj["Name"].as<String>());

        int minTempIndex = 0;
        JsonArray minTempArr = locationObj["MinTemperatures"].as<JsonArray>();
        for (JsonVariant minTempObj : minTempArr)
        {
            settings.Locations[locationIndex].MinTemperatures[minTempIndex].Value = minTempObj["Value"].as<float>();
            settings.Locations[locationIndex].MinTemperatures[minTempIndex].DayOfWeek = minTempObj["DayOfWeek"].as<int>();
            settings.Locations[locationIndex].MinTemperatures[minTempIndex].Hour = minTempObj["Hour"].as<int>();
            settings.Locations[locationIndex].MinTemperatures[minTempIndex].Minutes = minTempObj["Minutes"].as<int>();
            minTempIndex++;
        }
        locationIndex++;
    }
}