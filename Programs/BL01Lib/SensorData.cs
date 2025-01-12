using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL01Lib
{
    public class SensorInfo
    {
        public BL01.Parameter Parameter { get; set; }
        public bool isSuccess = true;

        public override string ToString()
        {
            return $"BDAddress: {Parameter.BDAddress}, SerialNumber: {Parameter.Serial}";
        }
    }

    public class SensorData
    {
        public SensorInfo Info { get; set; }          // センサー情報（BDアドレス、シリアル番号など）

        // センサーデータを保持するクラス
        public double Temperature { get; set; }       // 温度
        public double Humidity { get; set; }          // 湿度
        public double Illuminance { get; set; }       // 照度
        public double UV { get; set; }                // 紫外線
        public double Pressure { get; set; }          // 気圧
        public double SoundNoise { get; set; }        // 騒音
        public double Discomfort { get; set; }        // 不快指数
        public double HeatStrokeRisk { get; set; }    // 熱中症危険度
        public double BatteryVoltage { get; set; }    // バッテリー電圧

        public override string ToString()
        {
            return $"Temperature: {Temperature}℃, Humidity: {Humidity}%, Illuminance: {Illuminance}lx, UV: {UV}, Pressure: {Pressure}hPa, " +
                   $"SoundNoise: {SoundNoise}dB, Discomfort: {Discomfort}, HeatStrokeRisk: {HeatStrokeRisk}, BatteryVoltage: {BatteryVoltage}V";
        }
    }
}

