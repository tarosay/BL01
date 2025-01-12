using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Enumeration;
using Windows.Storage.Streams;

namespace BL01_ReaderWriter
{
    public partial class MainForm : Form
    {
        private List<BL01.Parameter> BL01_Serials = null;


        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            gbxPanel.Enabled = false;
        }
        private void MainForm_Shown(object sender, EventArgs e)
        {
            PairingSensorScand();

            gbxPanel.Enabled = true;
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {

        }



        /// <summary>
        /// ThrowMessageのキューの内容を表示します
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timWriteMessage_Tick(object sender, EventArgs e)
        {
            //メッセージの書き込み
            if (ThrowMessage.queue.Count > 0)
            {
                ThrowMessage.qlock.WaitOne();
                {
                    tbxMessages.AppendText((string)ThrowMessage.queue.Dequeue());

                    if (tbxMessages.TextLength >= 32000)
                    {
                        tbxMessages.Text = "";
                    }
                }
                ThrowMessage.qlock.Set();
            }
            else
            {
                //if (!MoniMesg.AlertFlag)
                //{
                //    lblMessage.Text = "";
                //}
            }
            //System.Windows.Forms.Application.DoEvents();
        }

        /// <summary>
        /// ペアリングしているBL01の再読み込み
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReScan_Click(object sender, EventArgs e)
        {
            btnReScan.Enabled = false;

            PairingSensorScand();

            btnReScan.Enabled = true;
        }

        /// <summary>
        /// ペアリングしているBL01の再読み込み
        /// </summary>
        private void PairingSensorScand()
        {
            ThrowMessage.mesg("ペアリングしている BL01 の検索中\r\n");

            BL01 bl = new BL01();

            BL01_Serials = bl.PairingSensorScan();

            ThrowMessage.mesg("ペアリングしているBL01の数= " + bl.Nums + "\r\n");
            ThrowMessage.mesg("\r\n");

            for (int i = 0; i < bl.Nums; i++)
            {
                ThrowMessage.mesg("BDアドレス= " + BL01_Serials[i].BDAddress + "\r\n");
                ThrowMessage.mesg("SERIAL= " + BL01_Serials[i].Serial + "\r\n");
                ThrowMessage.mesg("-----------------------------------\r\n");
            }
        }

        private void btnREAD_Click(object sender, EventArgs e)
        {
            gbxPanel.Enabled = false;

            string suuid = tbxServiceUUID.Text.Trim();
            string cuuid = tbxCharacteristicsUUID.Text.Trim();

            ThrowMessage.mesg("データの読み込み中: " + suuid + "-" + cuuid + " \r\n");

            BL01 bl = new BL01();

            //for (int j = 0; j < 2; j++)
            //{
            for (int i = 0; i < BL01_Serials.Count; i++)
            {
                BL01_Serials[i].CharacteristicsUUID = cuuid;
                BL01_Serials[i].ServiceUUID = suuid;
                BL01_Serials[i].ReadData = bl.ReadCharacteristicsData(BL01_Serials[i].BDAddress, suuid, cuuid);
                BL01_Serials[i].DTM = DateTime.Now;
                //        if (j > 0)
                //        {
                ThrowMessage.mesg("BL01-Serial=" + BL01_Serials[i].Serial + ": ");
                string strData = "";
                if (BL01_Serials[i].ReadData != null)
                {
                    strData = BitConverter.ToString(BL01_Serials[i].ReadData);
                }
                ThrowMessage.mesg(strData + "\r\n");

                if (cuuid == "3001" && BL01_Serials[i].ReadData != null)
                {
                    double temperature = ((double)BL01_Serials[i].ReadData[1] + (double)BL01_Serials[i].ReadData[2] * 256.0) / 100.0;
                    double humidity = ((double)BL01_Serials[i].ReadData[3] + (double)BL01_Serials[i].ReadData[4] * 256.0) / 100.0;
                    BL01_Serials[i].Illuminance = (int)(((double)BL01_Serials[i].ReadData[5] + (double)BL01_Serials[i].ReadData[6] * 256.0) + 0.5);
                    double uv = ((double)BL01_Serials[i].ReadData[7] + (double)BL01_Serials[i].ReadData[8] * 256.0) / 100.0;
                    double pressure = ((double)BL01_Serials[i].ReadData[9] + (double)BL01_Serials[i].ReadData[10] * 256.0) / 10.0;
                    double soundnoise = ((double)BL01_Serials[i].ReadData[11] + (double)BL01_Serials[i].ReadData[12] * 256.0) / 100.0;
                    double discomfort = ((double)BL01_Serials[i].ReadData[13] + (double)BL01_Serials[i].ReadData[14] * 256.0) / 100.0;
                    double heatstrokerisk = ((double)BL01_Serials[i].ReadData[15] + (double)BL01_Serials[i].ReadData[16] * 256.0) / 100.0;
                    double batteryvoltage = ((double)BL01_Serials[i].ReadData[17] + (double)BL01_Serials[i].ReadData[18] * 256.0) / 1000.0;

                    ThrowMessage.mesg($"温度= {temperature}\r\n");
                    ThrowMessage.mesg($"湿度= { humidity}\r\n");
                    ThrowMessage.mesg($"照度= {BL01_Serials[i].Illuminance}\r\n");
                    ThrowMessage.mesg($"UV= {uv}\r\n");
                    ThrowMessage.mesg($"騒音= {soundnoise}\r\n");
                    ThrowMessage.mesg($"不快指数= {discomfort}\r\n"); ;
                    ThrowMessage.mesg($"熱中症危険度= {heatstrokerisk}\r\n");
                    ThrowMessage.mesg($"電池電圧= {batteryvoltage}\r\n");
                    ThrowMessage.mesg("-----------------------------------\r\n");
                }
                //        }
            }

            //    for (int k = 0; k < 10; k++)
            //    {
            //        System.Windows.Forms.Application.DoEvents();
            //        Thread.Sleep(5);
            //    }
            //}
            gbxPanel.Enabled = true;
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            //for (int i = 0; i < 10000; i++)
            //{
            //SensorWriteAsync();

            //    //ThrowMessage.mesg("Count= " + (i + 1).ToString() + "\r\n");
            //    ThrowMessage.mesg("-----------------------------------\r\n");
            //    while (!SensorScanAsyncStopFlg)
            //    {
            //        System.Windows.Forms.Application.DoEvents();
            //        Thread.Sleep(10);                
            //    }                
            //}
        }

        //public static bool SensorScanAsyncStopFlg = true;

        //private async void SensorWriteAsync()
        //{
        //    SensorScanAsyncStopFlg = false;

        //    Guid serviceUuid = new Guid("0C4C3010-7700-46F4-AA96-D5E974E32A54");
        //    var services = await DeviceInformation.FindAllAsync(GattDeviceService.GetDeviceSelectorFromUuid(serviceUuid));

        //    if (services.Count > 0)
        //    {
        //        for (int i = 0; i < services.Count; i++)
        //        {
        //            if (services[i].Name == "EnvSensor-BL01")
        //            {
        //                GattDeviceService service0 = null;
        //                try
        //                {
        //                    service0 = await GattDeviceService.FromIdAsync(services[i].Id);

        //                    // キャラクタリスティックを取得
        //                    Guid uid = new Guid("0C4C3011-7700-46F4-AA96-D5E974E32A54");
        //                    var characteristics = await service0.GetCharacteristicsForUuidAsync(uid, Windows.Devices.Bluetooth.BluetoothCacheMode.Uncached);

        //                    if (characteristics.Characteristics.Count > 0)
        //                    {
        //                        byte[] dat = { 0x00, 0x01 };
        //                        var buffer = dat.AsBuffer();
        //                        var result = await characteristics.Characteristics[0].WriteValueAsync(buffer, GattWriteOption.WriteWithoutResponse);
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    Debug.WriteLine(ex.Message);
        //                    Debug.WriteLine(ex.StackTrace);
        //                }

        //                if (service0 != null)
        //                {
        //                    service0.Dispose();
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        Debug.WriteLine("サービス not found");
        //    }

        //    SensorScanAsyncStopFlg = true;
        //}

        /// <summary>
        /// 書き込みます
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnWrite_Click(object sender, EventArgs e)
        {
            gbxPanel.Enabled = false;
            string suuid = tbxServiceUUID.Text.Trim();
            string cuuid = tbxCharacteristicsUUID.Text.Trim();

            string[] dat = tbxWriteData.Text.Trim().Split(' ');
            byte[] writedata = new byte[dat.Length];

            ThrowMessage.mesg("データの書き込み中: " + suuid + "-" + cuuid + " \r\n");

            for (int i = 0; i < dat.Length; i++)
            {
                writedata[i] = Convert.ToByte(dat[i], 16);
            }            

            BL01 bl = new BL01();

            for (int i = 0; i < BL01_Serials.Count; i++)
            {
                bl.WriteCharacteristicsData(BL01_Serials[i].BDAddress, suuid, cuuid, writedata);
                ThrowMessage.mesg("BL01-Serial=" + BL01_Serials[i].Serial + ": Finished.\r\n");
            }
            gbxPanel.Enabled = true;
        }
      
    }
}
