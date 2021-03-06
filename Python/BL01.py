from bluepy import btle
from omron_env_broadcast import ScanDelegate

#omron_env_broadcast.pyのセンサ値取得デリゲートを、スキャン時実行に設定
scanner = btle.Scanner().withDelegate(ScanDelegate())

for i in[0,1,2]:
  #スキャンしてセンサ値取得（タイムアウト5秒）
  scanner.scan(5.0)
  if scanner.delegate.sensorValue == None:
    continue
  if scanner.delegate.sensorValue['SensorType'] == 'IM':
    json = '{'
    json += '"Temperature":' + str(scanner.delegate.sensorValue['Temperature'])
    json += ',"Humidity":' + str(scanner.delegate.sensorValue['Humidity'])
    json += ',"Light":' + str(scanner.delegate.sensorValue['Light'])
    json += ',"UV":' + str(scanner.delegate.sensorValue['UV'])
    json += ',"Pressure":' + str(scanner.delegate.sensorValue['Pressure'])
    json += ',"Noise":' + str(scanner.delegate.sensorValue['Noise'])
    json += ',"AccelerationX":' + str(scanner.delegate.sensorValue['AccelerationX'])
    json += ',"AccelerationY":' + str(scanner.delegate.sensorValue['AccelerationY'])
    json += ',"AccelerationZ":' + str(scanner.delegate.sensorValue['AccelerationZ'])
    json += ',"BatteryVoltage":' + str(scanner.delegate.sensorValue['BatteryVoltage'])
    json += '}'
  else:
    json = '{'
    json += '"Temperature":' + str(scanner.delegate.sensorValue['Temperature'])
    json += ',"Humidity":' + str(scanner.delegate.sensorValue['Humidity'])
    json += ',"Light":' + str(scanner.delegate.sensorValue['Light'])
    json += ',"UV":' + str(scanner.delegate.sensorValue['UV'])
    json += ',"Pressure":' + str(scanner.delegate.sensorValue['Pressure'])
    json += ',"Noise":' + str(scanner.delegate.sensorValue['Noise'])
    json += ',"Discomfort":' + str(scanner.delegate.sensorValue['Discomfort'])
    json += ',"WBGT":' + str(scanner.delegate.sensorValue['WBGT'])
    json += ',"BatteryVoltage":' + str(scanner.delegate.sensorValue['BatteryVoltage'])
    json += '}'
  print(json)
  break


