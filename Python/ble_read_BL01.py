import asyncio
import struct
import json
from bleak import BleakClient

# Omron BL01 の MAC アドレス
DEVICE_ADDRESS = "D1:63:2E:18:8F:C6"
# キャラクタリスティック UUID (char0018)
CHARACTERISTIC_UUID = "0c4c3001-7700-46f4-aa96-d5e974e32a54"

def decodeSensorData_EP(valueBinary):
    """センサデータをバイナリから解析し、dict 形式に変換（C# のデコード方法に統一）"""
    if len(valueBinary) < 19:
        print(json.dumps({"error": "Insufficient data length"}))
        return None

    # **C# の計算方法に統一（2バイトごとに数値変換）**
    sensorValue = {
        'Temperature': (valueBinary[1] + valueBinary[2] * 256) / 100.0,  # °C
        'Humidity': (valueBinary[3] + valueBinary[4] * 256) / 100.0,  # %
        'Illuminance': (valueBinary[5] + valueBinary[6] * 256),  # lx
        'UV': (valueBinary[7] + valueBinary[8] * 256) / 100.0,  # UV Index
        'Pressure': (valueBinary[9] + valueBinary[10] * 256) / 10.0,  # hPa
        'SoundNoise': (valueBinary[11] + valueBinary[12] * 256) / 100.0,  # dB
        'Discomfort': (valueBinary[13] + valueBinary[14] * 256) / 100.0,  # 不快指数
        'HeatStrokeRisk': (valueBinary[15] + valueBinary[16] * 256) / 100.0,  # 暑さ指数
        'BatteryVoltage': (valueBinary[17] + valueBinary[18] * 256) / 1000.0  # V
    }

    return sensorValue

async def read_ble_data():
    """BLE からセンサデータを取得し、解析する"""
    
    attempts = 0
    last_error = None

    while attempts < 5:
        client = BleakClient(DEVICE_ADDRESS)
        
        try:
            await client.connect()
            if not client.is_connected:
                raise Exception("Failed to connect to device")

            # キャラクタリスティックのデータを取得
            data = await client.read_gatt_char(CHARACTERISTIC_UUID)

            # 成功したらループを抜ける
            break  
        
        except Exception as e:
            last_error = str(e)
            attempts += 1
            await asyncio.sleep(1)  # 1秒待機してリトライ

        finally:
            await client.disconnect()

    if attempts >= 5:
        print(json.dumps({"error": f"Data retrieval failed: {last_error}", "Retry": attempts}))
        return

    # JSON 形式で出力
    sensor_data = decodeSensorData_EP(data)
    sensor_data["Error"] = None if last_error is None else last_error
    sensor_data["Retry"] = attempts

    print(json.dumps(sensor_data, ensure_ascii=False, indent=2))


asyncio.run(read_ble_data())
