using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL01DataService
{
    public class BL01Manager
    {
        // ログイベントの宣言
        public event Action<string> OnLogAppend;

        // ペアリング済みセンサーリスト
        private List<SensorInfo> pairedSensors = new List<SensorInfo>();

        private readonly string ServiceUUID = "3000";
        private readonly string CharacteristicsUUID = "3001";

        // コンストラクタ: 初期化時にセンサー情報を取得
        public BL01Manager()
        {
            pairedSensors = new List<SensorInfo>();
            //InitializePairedSensors();
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

            if (bl.Nums == 0)
            {
                AppendLog("Paired sensors not found.");
            }

            // BLEライブラリを使用して実際のデバイス情報を取得する処理に置き換える
            for (int i = 0; i < bl.Nums; i++)
            {
                parameters[i].ServiceUUID = this.ServiceUUID;
                parameters[i].CharacteristicsUUID = this.CharacteristicsUUID;
                pairedSensors.Add(new SensorInfo
                {
                    Parameter = parameters[i]
                });
            }

            AppendLog("Paired sensors initialized:");
            foreach (var sensor in pairedSensors)
            {
                AppendLog(sensor.ToString());
            }
        }

        // ペアリング済みセンサーのリストを取得
        public List<SensorInfo> GetPairedSensors()
        {
            return pairedSensors;
        }

        // センサーデータを取得 (BDAddressで指定)
        public SensorData ReadSensorData(string bdAddress)
        {
            // pairedSensorsからBDAddressで検索
            var sensorInfo = pairedSensors.FirstOrDefault(s => s.Parameter.BDAddress == bdAddress);

            if (sensorInfo != null)
            {
                // SensorInfoを用いてデータを取得
                return ReadSensorData(sensorInfo);
            }
            else
            {
                AppendLog($"Sensor with BDAddress {bdAddress} not found.");
            }
            return null;
        }

        public SensorData ReadSensorData(SensorInfo sensorInfo)
        {
            AppendLog($"Reading data from sensor: {sensorInfo.Parameter.BDAddress}");

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
                    DateTime = sensorInfo.Parameter.DTM.ToString("yyyy/MM/dd hh:mm:ss"),
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
                    DateTime = sensorInfo.Parameter.DTM.ToString("yyyy/MM/dd hh:mm:ss"),
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
            return data;
        }

        // ログを生成するメソッド
        public void AppendLog(string message)
        {
            Console.WriteLine(message);
            // ログが生成されたときにイベントを発生させる
            OnLogAppend?.Invoke(message);
        }
    }
}