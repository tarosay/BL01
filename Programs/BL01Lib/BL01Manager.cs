using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL01Lib
{
    public class BL01Manager
    {
        // ペアリング済みセンサーリスト
        private List<SensorInfo> pairedSensors = new List<SensorInfo>();

        // イベント: センサーデータを取得した際の通知
        public event Action<SensorData> OnSensorDataReceived;

        private readonly string ServiceUUID = "3000";
        private readonly string CharacteristicsUUID = "3001";

        // コンストラクタ: 初期化時にセンサー情報を取得
        public BL01Manager()
        {
            pairedSensors = new List<SensorInfo>();
            InitializePairedSensors();
        }
        public void researchPairedSensors()
        {
            InitializePairedSensors();
        }

        // ペアリング済みセンサーの情報を取得
        private void InitializePairedSensors()
        {
            pairedSensors.Clear();

            BL01 bl = new BL01();
            List<BL01.Parameter> parameters = bl.PairingSensorScan();

            // BLEライブラリを使用して実際のデバイス情報を取得する処理に置き換える
            for (int i = 0; i < bl.Nums; i++)
            {
                pairedSensors.Add(new SensorInfo
                {
                    Parameter = parameters[i]
                });
            }

            Console.WriteLine("Paired sensors initialized:");
            foreach (var sensor in pairedSensors)
            {
                Console.WriteLine(sensor.ToString());
            }
        }

        // ペアリング済みセンサーのリストを取得
        public List<SensorInfo> GetPairedSensors()
        {
            return pairedSensors;
        }

        // センサーデータを取得 (BDAddressで指定)
        public void ReadSensorData(string bdAddress)
        {
            // pairedSensorsからBDAddressで検索
            var sensorInfo = pairedSensors.FirstOrDefault(s => s.Parameter.BDAddress == bdAddress);

            if (sensorInfo != null)
            {
                // SensorInfoを用いてデータを取得
                ReadSensorData(sensorInfo);
            }
            else
            {
                Console.WriteLine($"Sensor with BDAddress {bdAddress} not found.");
            }
        }

        public void ReadSensorData(SensorInfo sensorInfo)
        {
            Console.WriteLine($"Reading data from sensor: {sensorInfo.Parameter.BDAddress}");

            BL01 bl = new BL01();

            sensorInfo.Parameter.CharacteristicsUUID = this.CharacteristicsUUID;
            sensorInfo.Parameter.ServiceUUID = this.ServiceUUID;
            sensorInfo.Parameter.ReadData = bl.ReadCharacteristicsData(
                sensorInfo.Parameter.BDAddress,
                this.ServiceUUID,
                this.CharacteristicsUUID);

            sensorInfo.Parameter.DTM = DateTime.Now;

            SensorData data = null;
            if (sensorInfo.Parameter.ReadData != null)
            {
                sensorInfo.isSuccess = true;
                byte[] readData = sensorInfo.Parameter.ReadData;

                // BLEデバイスからデータを
                data = new SensorData
                {
                    Info = sensorInfo, // センサー情報をセット
                    Temperature = ((double)readData[1] + (double)readData[2] * 256.0) / 100.0,
                    Humidity = ((double)readData[3] + (double)readData[4] * 256.0) / 100.0,
                    Illuminance = ((double)readData[5] + (double)readData[6] * 256.0),
                    UV = ((double)readData[7] + (double)readData[8] * 256.0) / 100.0,
                    Pressure = ((double)readData[9] + (double)readData[10] * 256.0) / 10.0,
                    SoundNoise = ((double)readData[11] + (double)readData[12] * 256.0) / 100.0,
                    Discomfort = ((double)readData[13] + (double)readData[14] * 256.0) / 100.0,
                    HeatStrokeRisk = ((double)readData[15] + (double)readData[16] * 256.0) / 100.0,
                    BatteryVoltage = ((double)readData[17] + (double)readData[18] * 256.0) / 1000.0
                };
            }
            else
            {
                sensorInfo.isSuccess = false;

                // BLEデバイスからデータを
                data = new SensorData
                {
                    Info = sensorInfo, // センサー情報をセット
                    Temperature = 0,
                    Humidity = 0,
                    Illuminance = 0,
                    UV = 0,
                    Pressure = 0,
                    SoundNoise = 0,
                    Discomfort = 0,
                    HeatStrokeRisk = 0,
                    BatteryVoltage = 0
                };
            }

            // Unityに通知
            OnSensorDataReceived?.Invoke(data);
        }
    }
}
