using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using static System.Net.WebRequestMethods;

namespace BL01DataService
{
    public partial class MainForm : Form
    {
        private HttpListener _listener;
        private Task _listenerTask;
        private int _port = 35109;

        private BL01Manager bl01Manager = new BL01Manager();

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // BL01Manager のログイベントを購読
            bl01Manager.OnLogAppend += AppendLog;

            //ペアリングしているセンサを検索する
            bl01Manager.researchPairedSensors();

            StartHttpServer();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopHttpServer();
        }
        private void StopHttpServer()
        {
            if (_listener != null && _listener.IsListening)
            {
                _listener.Stop();
                _listener.Close();
                AppendLog("HTTP server stopped.");
            }
        }

        private void AppendLog(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(AppendLog), message);
                return;
            }

            // テキストボックスにログを追記
            tbxMain.AppendText(message + Environment.NewLine);
            Console.WriteLine(message); // Replace with a UI log display if needed
        }

        private void StartHttpServer()
        {
            _listener = new HttpListener();
            AddLocalPrefixes();

            try
            {
                _listener.Start();
                _listenerTask = Task.Run(() => HandleRequests());
                AppendLog($"HTTP server started on http://localhost:{_port}/");

                var localAddresses = Dns.GetHostAddresses(Dns.GetHostName());
                foreach (var address in localAddresses)
                {
                    if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork) // IPv4のみ
                    {
                        string prefix = $"http://{address}:{_port}/";
                        AppendLog($"HTTP server started on {prefix}");
                    }
                }
            }
            catch (Exception ex)
            {
                AppendLog(ex.StackTrace);
                AppendLog($"Failed to start HTTP server: {ex.Message}");
            }
        }

        /// <summary>
        /// _listener.Prefixes.Addするには、
        /// 管理者権限のターミナルで、
        /// netsh http add urlacl url=http://+:35109/ user=Everyone
        /// して、URL予約 を設定する必要があります。
        /// 
        /// コマンドの意味
        /// netsh
        /// Windowsでネットワーク関連の設定を管理するためのコマンド。
        /// 
        /// http
        /// HTTPサービスに関する設定を行うモードを指定。
        /// 
        /// add urlacl
        /// 指定したURLに対するアクセス制御リスト(ACL) を追加するサブコマンド。
        /// URL予約を作成して、特定のユーザーやグループにそのURLへのバインディング権限を付与する。
        /// 
        /// url=http://+:35109/
        /// URL予約の対象となるURL。
        /// 
        /// http:// : HTTPプロトコル。
        ///  + : 任意のホスト名やIPアドレス（全てのIPアドレスを対象）。
        ///  :35109 : ポート番号35109で待ち受ける。
        ///  / : ルートパスを指定。
        ///  
        /// user=Everyone
        /// このURLにバインディングする権限を付与する対象ユーザーまたはグループ。
        /// Everyone : 全てのユーザーに権限を与える。
        /// 
        /// コマンドの効果
        /// このコマンドを実行すると、HTTPリスナーがポート35109で待ち受けるURLに対し、
        /// 全てのユーザーがバインディングできる ようになります。
        /// 例えば、.NETアプリケーションやその他のHTTPリスニングアプリケーションがポート35109で動作できるようになります。
        /// 
        /// 注意点
        /// セキュリティリスク: user=Everyone は全てのユーザーに権限を与えるため、
        /// 不特定多数のユーザーがリソースを使用可能になり、セキュリティリスクが高くなります。
        /// 特定のユーザーやグループに制限することを検討すべきです。
        /// 
        /// 例: user=Administrator
        /// 管理者権限が必要: このコマンドを実行するには、管理者権限でコマンドプロンプトを開く必要があります。
        /// 
        /// URL予約の確認:
        ///  現在のURL予約を確認するには次のコマンドを実行します:
        ///   netsh http show urlacl
        /// 
        /// URL予約の削除:
        ///  設定を削除する場合は以下を使用します:
        ///   netsh http delete urlacl url=http://+:35109/
        /// </summary>
        private void AddLocalPrefixes()
        {
            // ローカルホスト（localhost）を追加
            _listener.Prefixes.Add($"http://+:{_port}/");
            //_listener.Prefixes.Add($"http://localhost:{_port}/");
            //_listener.Prefixes.Add($"http://127.0.0.1:{_port}/");
            //// 自分のIPアドレスを取得し、すべてのアドレスでリスナーを設定
            //var localAddresses = Dns.GetHostAddresses(Dns.GetHostName());
            //foreach (var address in localAddresses)
            //{
            //    if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork) // IPv4のみ
            //    {
            //        string prefix = $"http://{address}:{_port}/";
            //        _listener.Prefixes.Add(prefix);
            //        AppendLog($"Added prefix: {prefix}");
            //    }
            //}
        }

        private async Task HandleRequests()
        {
            while (_listener.IsListening)
            {
                try
                {
                    var context = await _listener.GetContextAsync();
                    ProcessRequest(context);
                }
                catch (Exception ex)
                {
                    AppendLog($"Error handling request: {ex.Message}");
                }
            }
        }

        private void ProcessRequest(HttpListenerContext context)
        {
            string responseString = "";
            string requestPath = context.Request.Url.AbsolutePath;

            if (requestPath == "/read")
            {
                // クエリパラメータを取得
                string query = context.Request.Url.Query;
                if (!string.IsNullOrEmpty(query) && query.StartsWith("?"))
                {
                    string bdAddress = query.Substring(1);
                    responseString = GetSensorData(bdAddress);
                }
                else
                {
                    responseString = GetSensorData();
                }
            }
            else if (requestPath == "/list")
            {
                responseString = GetPairedSensorsData();
            }
            else
            {
                responseString = GetSensorData();
                //context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                //responseString = "Endpoint not found.";
            }

            byte[] responseBytes = Encoding.UTF8.GetBytes(responseString);
            context.Response.ContentType = "application/json";
            context.Response.ContentEncoding = Encoding.UTF8;
            context.Response.ContentLength64 = responseBytes.Length;

            try
            {
                context.Response.OutputStream.Write(responseBytes, 0, responseBytes.Length);
                context.Response.OutputStream.Close();
                AppendLog($"Handled request: {requestPath}");
            }
            catch (Exception ex)
            {
                AppendLog($"Error sending response: {ex.Message}");
            }
        }

        private string GetSensorData(string bdaddress)
        {
            SensorData sensdata = this.bl01Manager.ReadSensorData(bdaddress);

            var serializer = new JavaScriptSerializer();
            string json;
            if (sensdata == null)
            {
                json = serializer.Serialize($"Sensor with BDAddress {bdaddress} not found.");
                AppendLog(json);
                return json;
            }

            json = serializer.Serialize(sensdata);
            AppendLog(json);
            return json;
        }

        private string GetSensorData()
        {
            List<SensorInfo> sensInfos = this.bl01Manager.GetPairedSensors();
            string json;
            var serializer = new JavaScriptSerializer();

            if (sensInfos.Count == 0)
            {
                this.bl01Manager.researchPairedSensors();
                sensInfos = this.bl01Manager.GetPairedSensors();
                if (sensInfos.Count == 0)
                {
                    json = serializer.Serialize("Paired sensors not found.");
                    AppendLog(json);
                    return json;
                }
            }

            List<SensorData> datalist = new List<SensorData>();
            foreach (var sensor in sensInfos)
            {
                SensorData sensdata = this.bl01Manager.ReadSensorData(sensor.Parameter.BDAddress);
                datalist.Add(sensdata);
            }

            json = serializer.Serialize(datalist);
            AppendLog(json);
            return json;
        }

        private string GetPairedSensorsData()
        {
            List<SensorInfo> sensInfos = this.bl01Manager.GetPairedSensors();
            string json;
            var serializer = new JavaScriptSerializer();

            if (sensInfos.Count == 0)
            {
                this.bl01Manager.researchPairedSensors();
                sensInfos = this.bl01Manager.GetPairedSensors();
                if (sensInfos.Count == 0)
                {
                    json = serializer.Serialize("Paired sensors not found.");
                    AppendLog(json);
                    return json;
                }
            }

            // センサー情報を配列としてまとめてJSON形式に変換
            List<BL01.Parameter> parameters = new List<BL01.Parameter>();
            foreach (var sensor in sensInfos)
            {
                parameters.Add(sensor.Parameter);
            }

            json = serializer.Serialize(parameters);
            AppendLog(json);
            return json;
        }
    }
}
