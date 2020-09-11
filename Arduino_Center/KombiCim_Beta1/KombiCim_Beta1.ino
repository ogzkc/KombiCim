#define ARDUINO 100

#include <Arduino.h>
#include <ESP8266WiFi.h>
#include <ESP8266HTTPClient.h>
#include <DHT.h>
#include <SettingsManager.h>
#include <RH_ASK.h>
#include <SPI.h>

#define WIFI_SSID "KC"
#define WIFI_PASS "xxxxxxxxx"

#define DEBUG_USB

#define DHT_PIN D7
#define DHT_TYPE DHT22

#define RF_433_TRANSMITTER_PIN D5

#define MEASUREMENT_COUNT 3    // count for deciding combi state
#define RF_SEND_COUNT 10       // count for send RF to make sure deliver
#define RF_SEND_BACKUP_COUNT 3 // count for send RF -backup-

#define MIN_COMBI_CHANGE 8               // mins
#define INTERVAL_COMBI_CONTROL 2         // mins
#define INTERVAL_SEND_TEMP 5             // mins
#define INTERVAL_REFRESH_SETTINGS 3      // mins
#define INTERVAL_SEND_COMBI_RF_BACKUP 10 // mins

#define DELAY_MEASUREMENT 2000

const String DEVICE_ID = "qwer1234";
const char *DEVICE_ID_CHR = "qwer1234";

const char *API_USERNAME = "kc_device"; // basic auth
const char *API_PASSWORD = "KombiCim";

String host = "http://xxxxxxxxx/KombiCim/Arduino/api/Settings?deviceId=" + DEVICE_ID;
String hostSendWeather = "http://xxxxxxxxx/KombiCim/Arduino/api/Weather";
String hostSendCombiLog = "http://xxxxxxxxx/KombiCim/Arduino/api/Log/PostCombiLog";

unsigned long previousMillisSendTemp = 0;
unsigned long intervalSendTemp = INTERVAL_SEND_TEMP * 60 * 1000;

unsigned long previousMillisRefreshSettings = 0;
unsigned long intervalRefreshSettings = INTERVAL_REFRESH_SETTINGS * 60 * 1000;

unsigned long previousMillisCombiControl = 0;
unsigned long intervalCombiControl = INTERVAL_COMBI_CONTROL * 60 * 1000;

unsigned long previousMillisSendCombiRfBackup = 0;
unsigned long intervalSendCombiRfBackup = INTERVAL_SEND_COMBI_RF_BACKUP * 60 * 1000;

SettingsManager settingsManager;

DHT dht(DHT_PIN, DHT_TYPE);
RH_ASK driver(2000, 0, RF_433_TRANSMITTER_PIN);

float temperatures[MEASUREMENT_COUNT];

int measurementIndex = 0;
int totalMeasurement = 0;

bool firstRfSent = false;

unsigned long currentMillis = 0;
int combiState = 0;
int lastSentCombiState = -1;

unsigned long previousCombiChange = -9999999;
unsigned long minCombiChange = MIN_COMBI_CHANGE * 60 * 1000;

void setup()
{
  WiFi.begin(WIFI_SSID, WIFI_PASS);

  while (WiFi.status() != WL_CONNECTED)
  {
    delay(500);
  }

  delay(100);
  dht.begin();
  delay(100);

  Serial.begin(9600);

  delay(100);
  refreshSettings();
  delay(100);

  if (!driver.init())
    SerialLog("init failed");

  delay(100);
  sendTemp();
  delay(100);
  combiControl();
  delay(100);
}

void loop()
{
  if (isTimeUp(previousMillisSendTemp, intervalSendTemp))
  {
    previousMillisSendTemp = currentMillis;
    sendTemp();
  }
  if (isTimeUp(previousMillisRefreshSettings, intervalRefreshSettings))
  {
    previousMillisRefreshSettings = currentMillis;
    refreshSettings();
  }
  if (isTimeUp(previousMillisCombiControl, intervalCombiControl))
  {
    previousMillisCombiControl = currentMillis;
    combiControl();
  }
  if (isTimeUp(previousMillisSendCombiRfBackup, intervalSendCombiRfBackup))
  {
    previousMillisSendCombiRfBackup = currentMillis;
    sendCombiRFBackup();
  }
}

void sendTemp()
{
  float humidity = dht.readHumidity();
  float temp = dht.readTemperature();
  delay(2000);

  HTTPClient client;
  client.setAuthorization(API_USERNAME, API_PASSWORD);
  String jsonText;

  const size_t CAPACITY = JSON_OBJECT_SIZE(3);
  StaticJsonDocument<CAPACITY> doc;
  JsonObject jsonObject = doc.to<JsonObject>();
  jsonObject["Temperature"] = temp;
  jsonObject["Humidity"] = humidity;
  jsonObject["DeviceId"] = DEVICE_ID_CHR;
  serializeJson(doc, jsonText);

  client.begin(hostSendWeather);
  client.addHeader("Content-Type", "application/json");

  int httpCode = client.POST(jsonText);
  String contents = client.getString();

  SerialLog("Sent Temp: " + String(temp));
  SerialLog("Http: " + String(httpCode));
  SerialLog("contents: " + contents);
  client.end();
}

void refreshSettings()
{
  HTTPClient client;
  client.setAuthorization(API_USERNAME, API_PASSWORD);
  client.begin(host);

  int httpCode = client.GET();
  if (httpCode == 200)
  {
    String contents = client.getString();
    client.end();
    settingsManager.Refresh(contents);
  }
}

void combiControl()
{
  if (settingsManager.settings.Mode == settingsManager.MODE_AUTO_PROFILE_1)
  {
    temperatures[measurementIndex] = dht.readTemperature();
    delay(DELAY_MEASUREMENT);

    measurementIndex++;
    totalMeasurement++;

    if (measurementIndex >= MEASUREMENT_COUNT)
      measurementIndex = 0;

    if (millis() - previousCombiChange >= minCombiChange)
    {
      float total = 0;
      float avgTemp = 0;
      if (totalMeasurement >= MEASUREMENT_COUNT)
      {
        for (int i = 0; i < MEASUREMENT_COUNT; i++)
        {
          total += temperatures[i];
          SerialLog("Measurement " + String(i) + ": " + String(temperatures[i]));
        }
        SerialLog("Total: " + String(total));

        avgTemp = total / (float)MEASUREMENT_COUNT;
      }
      else
      {
        float temp1 = dht.readTemperature();
        delay(DELAY_MEASUREMENT);
        float temp2 = dht.readTemperature();
        delay(DELAY_MEASUREMENT);
        float temp3 = dht.readTemperature();
        delay(DELAY_MEASUREMENT);
        
        avgTemp = (temp1 + temp2 + temp3) / (float)3;
      }
      float difference = avgTemp - settingsManager.settings.MinTemperature;
      
      SerialLog("CurrentTemp: " + String(avgTemp));
      SerialLog("MinTempValue: " + String(settingsManager.settings.MinTemperature));
      SerialLog("Difference: " + String(difference));

      if (difference <= -0.15)
        rfTransmitCombiControl(1, RF_SEND_COUNT);
      else if (difference >= 0.19)
        rfTransmitCombiControl(0, RF_SEND_COUNT);
      else
      {
        if (!firstRfSent)
        {
          rfTransmitCombiControl(0, RF_SEND_COUNT);
        }
      }
    }
  }
  else if (settingsManager.settings.Mode == settingsManager.MODE_MANUAL_1)
  {
    rfTransmitCombiControl(settingsManager.settings.State, RF_SEND_COUNT);

    SerialLog("State: " + String(settingsManager.settings.State));
  }
}

void rfTransmitCombiControl(int isOpen, int sendCount)
{
  String result = DEVICE_ID + "&" + String(isOpen);

  const char *msg = result.c_str();

  SerialLog("Sending RF: " + result);

  for (int i = 0; i < sendCount; i++)
  {
    driver.send((uint8_t *)msg, strlen(msg));
    driver.waitPacketSent();
    delay(200);
  }

  firstRfSent = true;

  sendCombiLog(isOpen);
  combiState = isOpen;
}

void sendCombiLog(int isOpen)
{
  if (lastSentCombiState != isOpen)
  {
    HTTPClient client;
    client.setAuthorization(API_USERNAME, API_PASSWORD);
    String jsonText;

    const size_t CAPACITY = JSON_OBJECT_SIZE(2);
    StaticJsonDocument<CAPACITY> doc;
    JsonObject jsonObject = doc.to<JsonObject>();

    if (isOpen == 1)
      jsonObject["State"] = "true";
    else
      jsonObject["State"] = "false";
    jsonObject["DeviceId"] = DEVICE_ID_CHR;
    serializeJson(doc, jsonText);

    client.begin(hostSendCombiLog);
    client.addHeader("Content-Type", "application/json");

    int httpCode = client.POST(jsonText);
    String contents = client.getString();

    SerialLog("CurrentCombiState: " + String(isOpen));

    previousCombiChange = millis();

    if (httpCode == 200)
    {
      lastSentCombiState = isOpen;
    }
  }
}

void sendCombiRFBackup()
{
  rfTransmitCombiControl(combiState, RF_SEND_BACKUP_COUNT);
}

void SerialLog(String contents)
{
#ifdef DEBUG_USB
  Serial.println(contents);
#endif
}

bool isTimeUp(unsigned long previous, unsigned long interval)
{
  currentMillis = millis();
  return currentMillis - previous >= interval;
}
