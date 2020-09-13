#include <RH_ASK.h>
#include <SPI.h>

#define RELAY_PIN 6

RH_ASK driver(2000, 12); // Use any pin for Arduino boards
// RH_ASK driver(2000, 2, 4, 5); // if ESP8266 -> do not use pin 11

String id = "zxcv1234";

void setup()
{
  pinMode(RELAY_PIN, OUTPUT);
  digitalWrite(RELAY_PIN, HIGH);

  Serial.begin(9600);
  if (!driver.init())
    Serial.println("init failed");

  Serial.println("Setup");
}

void loop()
{
  uint8_t buf[10];
  uint8_t buflen = sizeof(buf);
  if (driver.recv(buf, &buflen))
  {
    String data = String((char*)buf);
    String receivedDeviceId = data.substring(0, data.indexOf("&", 0));
    String receivedData = data.substring(data.indexOf("&", 0) + 1, data.length());
    if (receivedDeviceId == id)
    {
      if (receivedData.startsWith("1"))
        digitalWrite(RELAY_PIN, HIGH);
      else if (receivedData.startsWith("0"))
        digitalWrite(RELAY_PIN, LOW);
    }
    Serial.print("Message: ");
    Serial.println(data);
  }
}
