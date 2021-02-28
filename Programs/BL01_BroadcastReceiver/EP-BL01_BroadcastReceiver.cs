using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Storage.Streams;

namespace BL01_BroadcastReceiver
{
    public class BL01Parameter
    {
        public DateTime TimeStamp = DateTime.Now;
        public string Name = "";
        public short Rssi = 0;
        public int ManufacturerId = 0;
        public double Temperature = 0.0;
        public double Humidity = 0.0;
        public double Illuminance = 0.0;
        public double UV = 0.0;
        public double Pressure = 0.0;
        public double SoundNoise = 0.0;
        public double Discomfort = 0.0;
        public double HeatStroke = 0.0;
        public int Battery = 0;
        public string BDAddress = "";
    }

    public class EP_BL01_BroadcastReceiver
    {
        private const string NameEP = "EP";

        public BluetoothLEAdvertisementWatcher Watcher = new BluetoothLEAdvertisementWatcher();

        public Queue queue = new Queue();                              //モニタメッセージを保存するキュー
        public AutoResetEvent qlock = new AutoResetEvent(true);        //キュー読み書き排他処理用

        private EventRegistrationToken Received_token;

        public EP_BL01_BroadcastReceiver()
        {
            //サンプリング間隔を5000msに指定
            this.Watcher.SignalStrengthFilter.SamplingInterval = TimeSpan.FromMilliseconds(5000);

            //// rssi >= -60のとき受信開始するっぽい
            //Watcher.SignalStrengthFilter.InRangeThresholdInDBm = -60;
            //// rssi <= -65が2秒続いたら受信終わるっぽい
            //Watcher.SignalStrengthFilter.OutOfRangeThresholdInDBm = -65;
            //Watcher.SignalStrengthFilter.OutOfRangeTimeout = TimeSpan.FromMilliseconds(5000);

            //ブロードキャストのレシーバーを用意します
            this.Watcher.Received += this.Watcher_Received;
            //this.Received_token = this.Watcher.add_Received(this.Watcher_Received);
        }

        ~EP_BL01_BroadcastReceiver()
        {
            // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
            Dispose(false);
        }

        private bool DdisposedValue = false;    //重複する呼び出しの検知用

        protected virtual void Dispose(bool disposing)
        {
            if (!DdisposedValue)
            {
                if (disposing)
                {
                    // TODO: マネージ状態を破棄します (マネージ オブジェクト)。
                }

                // TODO: アンマネージ リソース (アンマネージ オブジェクト) を解放し、下のファイナライザーをオーバーライドします。
                // TODO: 大きなフィールドを null に設定します。
                if (Watcher.Status == BluetoothLEAdvertisementWatcherStatus.Started)
                {
                    Watcher.Stop();
                }

                //ブロードキャストのレシーバーを開放します
                Watcher.Received -= Watcher_Received;
                //this.Watcher.remove_Received(this.Received_token);
                DdisposedValue = true;
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
            Dispose(true);
            // TODO: 上のファイナライザーがオーバーライドされる場合は、次の行のコメントを解除してください。
            // GC.SuppressFinalize(this);
        }

        /// <summary>
        /// BL01の取得パラメータを返します
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public BL01Parameter GetParam(BluetoothLEAdvertisementReceivedEventArgs eventArgs)
        {
            BL01Parameter bl01 = new BL01Parameter();

            //出力されているbyteデータから各値を抽出する
            var manufacturerSections = eventArgs.Advertisement.ManufacturerData;
            bl01.TimeStamp = eventArgs.Timestamp.DateTime;

            //long g = DateTime.Now.ToBinary();
            //DateTime dtm = DateTime.FromBinary(g);


            //Debug.WriteLine("manufacturerSections.Count= " + manufacturerSections.Count.ToString());

            if (manufacturerSections.Count > 0)
            {
                var manufacturerData = manufacturerSections[0];
                var data = new byte[manufacturerData.Data.Length];

                using (var reader = DataReader.FromBuffer(manufacturerData.Data))
                {
                    reader.ReadBytes(data);
                }

                bl01.BDAddress = eventArgs.BluetoothAddress.ToString("X016");
                bl01.Rssi = eventArgs.RawSignalStrengthInDBm;
                bl01.ManufacturerId = manufacturerData.CompanyId;
                bl01.Name = eventArgs.Advertisement.LocalName;

                if (bl01.Name == NameEP)
                {
                    bl01.Temperature = ((double)data[1] + (double)data[2] * 256.0) / 100.0;
                    bl01.Humidity = ((double)data[3] + (double)data[4] * 256.0) / 100.0;
                    bl01.Illuminance = ((double)data[5] + (double)data[6] * 256.0);
                    bl01.UV = ((double)data[7] + (double)data[8] * 256.0) / 100.0;
                    bl01.Pressure = ((double)data[9] + (double)data[10] * 256.0) / 10.0;
                    bl01.SoundNoise = ((double)data[11] + (double)data[12] * 256.0) / 100.0;
                    bl01.Discomfort = ((double)data[13] + (double)data[14] * 256.0) / 100.0;
                    bl01.HeatStroke = ((double)data[15] + (double)data[16] * 256.0) / 100.0;
                    bl01.Battery = data[19];
                }
            }
            return bl01;
        }

        /// <summary>
        /// ブロードキャストのレシーバーです
        /// 5000個スタックしたら古いものを削除していきます
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void Watcher_Received(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementReceivedEventArgs args)
        {
            qlock.WaitOne();
            {
                if (queue.Count > 5000)
                {
                    queue.Dequeue();
                }
                queue.Enqueue(args);
            }
            qlock.Set();
        }
    }
}
