using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Enumeration;
using Windows.Storage.Streams;

namespace BL01Lib
{
    public class BL01
    {
        public class Parameter
        {
            public DateTime DTM = DateTime.Now;
            public string Serial = "";
            public string BDAddress = "";
            public int Illuminance = 0;
            public string ServiceUUID = "";
            public string CharacteristicsUUID = "";
            public byte[] ReadData;
        }

        private const string DeviceInformationServiceUUID = "0000180a-0000-1000-8000-00805f9b34fb";
        private const string CharacteristicsSerialNumberUUID = "00002a25-0000-1000-8000-00805f9b34fb";
        private const string BaseUUIDPublic = "0000xxxx-0000-1000-8000-00805f9b34fb";
        private const string BaseUUID = "0C4Cxxxx-7700-46F4-AA96-D5E974E32A54";
        private const string PairingNameEnv = "EnvSensor-BL01";
        private const string PairingNameEP = "EP-BL01";

        /// <summary>
        /// ペアリングしている"Env"センサーの数
        /// </summary>
        public int Nums { get; set; }
        public bool PairingSensorScanAsyncFlg { get; set; }
        public bool CharacteristicsReadWriteAsyncFlg { get; set; }
        public int TimeoutSec { get; set; }
        public byte[] ReadData;

        private List<Parameter> PairingSensors = new List<Parameter>();

        public BL01()
        {
            PairingSensorScanAsyncFlg = false;
            CharacteristicsReadWriteAsyncFlg = false;
            TimeoutSec = 120;
        }

        /// <summary>
        /// ペアリングされているBL01をスキャンします
        /// </summary>
        /// <returns></returns>
        public List<Parameter> PairingSensorScan()
        {
            PairingSensorScanAsyncFlg = true;
            PairingSensorScanAsync();

            DateTime dtn = DateTime.Now.AddSeconds(this.TimeoutSec);
            while (PairingSensorScanAsyncFlg)
            {
                System.Windows.Forms.Application.DoEvents();
                Thread.Sleep(10);

                if (dtn < DateTime.Now)
                {
                    PairingSensorScanAsyncFlg = false;
                    break;
                }
            }

            this.Nums = PairingSensors.Count;
            return PairingSensors;
        }

        /// <summary>
        /// ペアリングされているBL01を非同期でスキャンします
        /// </summary>
        private async void PairingSensorScanAsync()
        {
            PairingSensorScanAsyncFlg = true;

            PairingSensors.Clear();

            Guid serviceUuid = new Guid(DeviceInformationServiceUUID);
            var services = await DeviceInformation.FindAllAsync(GattDeviceService.GetDeviceSelectorFromUuid(serviceUuid));

            if (services.Count > 0)
            {
                for (int i = 0; i < services.Count; i++)
                {
                    if (services[i].Name == PairingNameEnv || services[i].Name == PairingNameEP)
                    {
                        try
                        {
                            // サービスを作成
                            GattDeviceService service0 = await GattDeviceService.FromIdAsync(services[i].Id);

                            //シリアル番号の
                            Guid uid = new Guid(CharacteristicsSerialNumberUUID);
                            var characteristics = await service0.GetCharacteristicsForUuidAsync(uid, Windows.Devices.Bluetooth.BluetoothCacheMode.Uncached);

                            if (characteristics.Characteristics.Count > 0)
                            {
                                var vv = await characteristics.Characteristics[0].ReadValueAsync(Windows.Devices.Bluetooth.BluetoothCacheMode.Uncached);

                                byte[] readBytes = new byte[vv.Value.Length];
                                using (DataReader reader = DataReader.FromBuffer(vv.Value))
                                {
                                    reader.ReadBytes(readBytes);
                                }

                                Parameter pm = new Parameter();
                                pm.Serial = Encoding.ASCII.GetString(readBytes);

                                pm.BDAddress = GetBDAddress(service0);

                                PairingSensors.Add(pm);
                            }

                            if (service0 != null)
                            {
                                service0.Dispose();
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message);
                            Debug.WriteLine(ex.StackTrace);
                        }
                    }
                }
            }

            PairingSensorScanAsyncFlg = false;
        }

        /// <summary>
        /// Bluetooth Device Addressを取得します
        /// </summary>
        /// <param name="service0"></param>
        /// <returns></returns>
        private string GetBDAddress(GattDeviceService service0)
        {
            string[] dat0, dat1;
            string bdaddress = "";

            if (service0.Session.DeviceId.IsLowEnergyDevice)
            {
                dat0 = service0.Session.DeviceId.Id.Split('-');
                if (dat0.Length > 1)
                {
                    dat1 = dat0[1].Split(':');
                    for (int j = 0; j < dat1.Length; j++)
                    {
                        bdaddress += dat1[j];
                    }
                }
            }
            return bdaddress.Trim();
        }

        /// <summary>
        /// 指定されたBDアドレスのセンサからサービスUUID-キャラクタリスティックUUIDのデータを読み込みます
        /// </summary>
        /// <param name="bdAddress"></param>
        /// <param name="serv"></param>
        /// <param name="charisc"></param>
        /// <returns></returns>
        public byte[] ReadCharacteristicsData(string bdAddress, string serv, string charisc)
        {
            CharacteristicsReadWriteAsyncFlg = true;
            ReadCharacteristicsAsync(bdAddress, serv, charisc);

            DateTime dtn = DateTime.Now.AddSeconds(this.TimeoutSec);
            while (CharacteristicsReadWriteAsyncFlg)
            {
                System.Windows.Forms.Application.DoEvents();
                Thread.Sleep(10);

                if (dtn < DateTime.Now)
                {
                    CharacteristicsReadWriteAsyncFlg = false;
                    break;
                }
            }
            return ReadData;
        }

        /// <summary>
        /// 指定されたBDアドレスのセンサからサービスUUID-キャラクタリスティックUUIDのデータを非同期で読み込みます
        /// </summary>
        private async void ReadCharacteristicsAsync(string bdAddress, string serv, string charisc)
        {
            CharacteristicsReadWriteAsyncFlg = true;

            string baseuuid = BaseUUID;

            if (serv.Substring(0, 2) == "18")
            {
                baseuuid = BaseUUIDPublic;
            }

            string sUUID = baseuuid.Replace("xxxx", serv);
            Guid serviceUuid = new Guid(sUUID);
            var services = await DeviceInformation.FindAllAsync(GattDeviceService.GetDeviceSelectorFromUuid(serviceUuid));

            if (services.Count > 0)
            {
                for (int i = 0; i < services.Count; i++)
                {
                    if (services[i].Name == PairingNameEnv || services[i].Name == PairingNameEP)
                    {
                        GattDeviceService service0 = null;
                        try
                        {
                            service0 = await GattDeviceService.FromIdAsync(services[i].Id);

                            if (GetBDAddress(service0) == bdAddress)
                            {
                                string cUUID = baseuuid.Replace("xxxx", charisc);
                                Guid uid = new Guid(cUUID);
                                var characteristics = await service0.GetCharacteristicsForUuidAsync(uid, Windows.Devices.Bluetooth.BluetoothCacheMode.Uncached);

                                if (characteristics.Characteristics.Count > 0)
                                {
                                    var vv = await characteristics.Characteristics[0].ReadValueAsync(Windows.Devices.Bluetooth.BluetoothCacheMode.Uncached);
                                    if (vv.Value != null)
                                    {
                                        this.ReadData = new byte[vv.Value.Length];
                                        using (DataReader reader = DataReader.FromBuffer(vv.Value))
                                        {
                                            reader.ReadBytes(this.ReadData);
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message);
                            Debug.WriteLine(ex.StackTrace);
                        }

                        if (service0 != null)
                        {
                            service0.Dispose();
                        }
                    }
                }
            }

            CharacteristicsReadWriteAsyncFlg = false;
        }

        /// <summary>
        /// 指定されたBDアドレスのセンサのサービスUUID-キャラクタリスティックUUIDにデータを書き込みます
        /// </summary>
        /// <param name="bdAddress"></param>
        /// <param name="serv"></param>
        /// <param name="charisc"></param>
        /// <returns></returns>
        public void WriteCharacteristicsData(string bdAddress, string serv, string charisc, byte[] writeData)
        {
            CharacteristicsReadWriteAsyncFlg = true;
            WriteCharacteristicsAsync(bdAddress, serv, charisc, writeData);

            DateTime dtn = DateTime.Now.AddSeconds(this.TimeoutSec);
            while (CharacteristicsReadWriteAsyncFlg)
            {
                System.Windows.Forms.Application.DoEvents();
                Thread.Sleep(10);

                if (dtn < DateTime.Now)
                {
                    CharacteristicsReadWriteAsyncFlg = false;
                    break;
                }
            }
        }

        private async void WriteCharacteristicsAsync(string bdAddress, string serv, string charisc, byte[] writeData)
        {
            CharacteristicsReadWriteAsyncFlg = true;

            string baseuuid = BaseUUID;

            string sUUID = baseuuid.Replace("xxxx", serv);
            Guid serviceUuid = new Guid(sUUID);
            var services = await DeviceInformation.FindAllAsync(GattDeviceService.GetDeviceSelectorFromUuid(serviceUuid));

            if (services.Count > 0)
            {
                for (int i = 0; i < services.Count; i++)
                {
                    if (services[i].Name == PairingNameEnv || services[i].Name == PairingNameEP)
                    {
                        GattDeviceService service0 = null;
                        try
                        {
                            service0 = await GattDeviceService.FromIdAsync(services[i].Id);

                            if (GetBDAddress(service0) == bdAddress)
                            {
                                string cUUID = baseuuid.Replace("xxxx", charisc);
                                Guid uid = new Guid(cUUID);
                                var characteristics = await service0.GetCharacteristicsForUuidAsync(uid, Windows.Devices.Bluetooth.BluetoothCacheMode.Uncached);

                                if (characteristics.Characteristics.Count > 0)
                                {
                                    var buffer = writeData.AsBuffer();
                                    var result = await characteristics.Characteristics[0].WriteValueAsync(buffer, GattWriteOption.WriteWithoutResponse);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message);
                            Debug.WriteLine(ex.StackTrace);
                        }

                        if (service0 != null)
                        {
                            service0.Dispose();
                        }
                    }
                }
            }

            CharacteristicsReadWriteAsyncFlg = false;
        }
    }
}
