using BL01Lib;
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

namespace UseDLLExample
{
    public partial class Form1 : Form
    {
        private BL01Manager bl01Manager= new BL01Manager();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // ペアリング済みセンサーの情報を取得してログに表示
            foreach (var sensor in bl01Manager.GetPairedSensors())
            {
                this.textBox1.AppendText($"Found sensor: {sensor}\r\n");
            }

            // イベント登録
            bl01Manager.OnSensorDataReceived += OnSensorDataReceived;
        }

        private void OnSensorDataReceived(SensorData data)
        {
            // センサーデータをログに表示
            this.textBox1.AppendText("\r\n" + data.Info + "\r\n");
            string[] sensorResults = data.ToString().Split(',');
            foreach (string text in sensorResults)
            {
                this.textBox1.AppendText(text + "\r\n");
            }
            this.textBox1.AppendText("\r\n");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<SensorInfo> sensorInfos = bl01Manager.GetPairedSensors();

            // 特定のBDAddressを持つセンサーのデータを取得
            if (sensorInfos.Count > 0)
            {
                foreach (SensorInfo sensorInfo in sensorInfos)
                {
                    bl01Manager.ReadSensorData(sensorInfo);
                }
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            // イベント解放
            bl01Manager.OnSensorDataReceived -= OnSensorDataReceived;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.bl01Manager.researchPairedSensors();
            foreach (var sensor in bl01Manager.GetPairedSensors())
            {
                this.textBox1.AppendText($"Found sensor: {sensor}\r\n");
            }
        }
    }
}
