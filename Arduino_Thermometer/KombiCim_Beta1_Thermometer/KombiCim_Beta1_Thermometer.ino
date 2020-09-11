      #define ARDUINO 100

#include <Arduino.h>
#include <ESP8266WiFi.h>
#include <ESP8266HTTPClient.h>
#include <DHT.h>
#include <ArduinoJson.h>

#define WIFI_SSID "KC"
#define WIFI_PASS "xxxxxxxxxxx"

#define DHT_PIN D4
#define DHT_TYPE DHT22

#define INTERVAL_SEND_TEMP 5

const String DEVICE_ID = "qwer1235";
const char *DEVICE_ID_CHR = "qwer1235";

String hostSendWeather = "http://xxxxxxxxx/KombiCim/Arduino/api/Weather";

unsigned long previousMillisSendTemp = 0;
unsigned long intervalSendTemp = INTERVAL_SEND_TEMP * 60 * 1000;

DHT dht(DHT_PIN, DHT_TYPE);

void setup()
{
  Serial.begin(9600);
  delay(150);

  WiFi.begin(WIFI_SSID, WIFI_PASS);

  while (WiFi.status() != WL_CONNECTED)
  {
    delay(500);
  }

  delay(200);
  dht.begin();

  delay(200);
  sendTemp();
}

void loop()
{
  unsigned long currentMillis = millis();

  if (currentMillis - previousMillisSendTemp >= intervalSendTemp)
  {
    previousMillisSendTemp = currentMillis;
    sendTemp();
  }
}

void sendTemp()
{
  float humidity = dht.readHumidity();
  float temp = dht.readTemperature();

  Serial.println("Temp: " + String(temp));
  Serial.println("Humidity: " + String(humidity));

  HTTPClient client;
  client.setAuthorization("kc_device", "KombiCim");
  String jsonText;

  const size_t CAPACITY = JSON_OBJECT_SIZE(4);
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
  Serial.println("Http: " + String(httpCode));
  Serial.println("contents: " + contents);
  client.end();
}
