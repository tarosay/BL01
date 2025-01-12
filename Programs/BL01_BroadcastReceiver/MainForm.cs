using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.Devices.Bluetooth.Advertisement;

namespace BL01_BroadcastReceiver
{
    public partial class MainForm : Form
    {
        public static EP_BL01_BroadcastReceiver EPBL01 = new EP_BL01_BroadcastReceiver();

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            btnStop.Enabled = false;
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            EPBL01.Dispose();
        }

        /// <summary>
        /// メッセージテキストを消します
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            tbxMessages.Text = "";
        }

        /// <summary>
        /// ブロードキャスト受信を開始します
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceive_Click(object sender, EventArgs e)
        {
            btnReceive.Enabled = false;
            btnStop.Enabled = true;

            EPBL01.Watcher.Start();

        }
        /// <summary>
        /// ブロードキャスト受信を停止します
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStop_Click(object sender, EventArgs e)
        {
            EPBL01.Watcher.Stop();

            btnStop.Enabled = false;
            btnReceive.Enabled = true;
        }

        /// <summary>
        /// 受信したデータをメッセージボックスに書き込みます
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timMessage_Tick(object sender, EventArgs e)
        {
            timMessage.Enabled = false;

            display();

            timMessage.Enabled = true;
        }

        /// <summary>
        /// メッセージの表示を行います
        /// </summary>
        private void display()
        {
            //メッセージの書き込み
            if (EPBL01.queue.Count > 0)
            {
                BluetoothLEAdvertisementReceivedEventArgs args = null;
                EPBL01.qlock.WaitOne();
                {
                    args = (BluetoothLEAdvertisementReceivedEventArgs)EPBL01.queue.Dequeue();
                }
                EPBL01.qlock.Set();

                BL01Parameter param = EPBL01.GetParam(args);

                if (param.Name != null && param.Name == "EP")
                {
                    tbxMessages.AppendText(param.TimeStamp.ToString() + "\r\n");
                    tbxMessages.AppendText("Name= " + param.Name + "\r\n");
                    tbxMessages.AppendText("Rssi= " + param.Rssi + "\r\n");
                    tbxMessages.AppendText("ManufacturerId= " + param.ManufacturerId.ToString("X04") + "\r\n");
                    tbxMessages.AppendText("BDAddress= " + param.BDAddress + "\r\n");

                    tbxMessages.AppendText("温度= " + param.Temperature.ToString() + "\r\n");
                    tbxMessages.AppendText("湿度= " + param.Humidity.ToString() + "\r\n");
                    tbxMessages.AppendText("照度= " + param.Illuminance.ToString() + "\r\n");
                    tbxMessages.AppendText("紫外線= " + param.UV.ToString() + "\r\n");
                    tbxMessages.AppendText("気圧= " + param.Pressure.ToString() + "\r\n");
                    tbxMessages.AppendText("騒音= " + param.SoundNoise.ToString() + "\r\n");
                    tbxMessages.AppendText("不快指数= " + param.Discomfort.ToString() + "\r\n");
                    tbxMessages.AppendText("熱中症= " + param.HeatStroke.ToString() + "\r\n");
                    tbxMessages.AppendText("電池= " + param.Battery.ToString() + "\r\n");
                    tbxMessages.AppendText("-----------------------------------\r\n");
                }             

                if (tbxMessages.TextLength >= 32000)
                {
                    tbxMessages.Text = tbxMessages.Text.Substring(20000);
                }


            }
        }
    }
}
