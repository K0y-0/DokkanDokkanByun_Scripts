//#include <M5Stack.h>
//#include <M5Atom.h>
#include <M5StickC.h>
#include <Wire.h>
#include "VL53L0X.h"

//library   = https://github.com/pololu/vl53l0x-arduino

VL53L0X sensor;
void setup() {
  M5.begin();
  
  //Power chipがgpio21, gpio22, I2Cにつながれたデバイスに接続される。
  //バッテリー動作の場合はこの関数を読んでください（バッテリーの電圧を調べるらしい）
  //M5.Power.begin();
  Wire.begin();// I2C通信を開始する
  
  sensor.init();
  sensor.setTimeout(500);
  M5.Lcd.fillScreen(BLACK);
  M5.Lcd.setCursor(10, 10);
  M5.Lcd.setTextColor(WHITE, BLACK);
  M5.Lcd.setTextSize(3);

  Serial.begin(9600);
}
void loop() {
  int distance = sensor.readRangeSingleMillimeters();

  //delay(500);

  int delayDistance = sensor.readRangeSingleMillimeters();

  int calcDistance = distance - delayDistance;
  
  M5.Lcd.setCursor(0, 0);
  //M5.Lcd.print(calcDistance);
  M5.Lcd.print(distance);
  M5.Lcd.print("mm");
    
  if (sensor.timeoutOccurred()) {
    M5.Lcd.setCursor(0, 50);
    M5.Lcd.println("TIMEOUT");
  }else{
    M5.Lcd.setCursor(0, 50);
    M5.Lcd.println(" ");
  }

  // シリアル通信
  Serial.println(calcDistance);
  
}

// void setup() {
//   M5.begin();
  

//   //Power chipがgpio21, gpio22, I2Cにつながれたデバイスに接続される。
//   //バッテリー動作の場合はこの関数を読んでください（バッテリーの電圧を調べるらしい）
//   //M5.Power.begin();
//   Wire.begin();// I2C通信を開始する
  
//   sensor.init();
//   sensor.setTimeout(500);

//   Serial.begin(9600);

//   //Led();
// }
// void loop() {
//   int distance = sensor.readRangeSingleMillimeters();

//   //delay(50);

//   int delayDistance = sensor.readRangeSingleMillimeters();

//   int calcDistance = distance - delayDistance;
  
//   M5.Lcd.print(calcDistance);
    
//   if (sensor.timeoutOccurred()) {
//     M5.Lcd.setCursor(0, 50);
//     M5.Lcd.println("TIMEOUT");
//   }else{
//     M5.Lcd.setCursor(0, 50);
//     M5.Lcd.println("        ");  
//   }

//   // シリアル通信
//   Serial.println(calcDistance);
  
// }

// // void Led(){
// //   M5.dis.drawpix(6, 0x00ff00);  //red
// //   M5.dis.drawpix(7, 0x00ff00);
// //   M5.dis.drawpix(8, 0x00ff00);
// //   M5.dis.drawpix(16, 0x0000ff);  //blue 0x0000ff
// //   M5.dis.drawpix(18, 0x0000ff);
// // }
