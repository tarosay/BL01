namespace BL01_BroadcastReceiver
{
    partial class MainForm
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tbxMessages = new System.Windows.Forms.TextBox();
            this.timMessage = new System.Windows.Forms.Timer(this.components);
            this.gbxPanel = new System.Windows.Forms.GroupBox();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnReceive = new System.Windows.Forms.Button();
            this.gbxPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbxMessages
            // 
            this.tbxMessages.Location = new System.Drawing.Point(12, 70);
            this.tbxMessages.Multiline = true;
            this.tbxMessages.Name = "tbxMessages";
            this.tbxMessages.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbxMessages.Size = new System.Drawing.Size(604, 387);
            this.tbxMessages.TabIndex = 2;
            // 
            // timMessage
            // 
            this.timMessage.Enabled = true;
            this.timMessage.Tick += new System.EventHandler(this.timMessage_Tick);
            // 
            // gbxPanel
            // 
            this.gbxPanel.Controls.Add(this.btnStop);
            this.gbxPanel.Controls.Add(this.btnClear);
            this.gbxPanel.Controls.Add(this.btnReceive);
            this.gbxPanel.Location = new System.Drawing.Point(12, 12);
            this.gbxPanel.Name = "gbxPanel";
            this.gbxPanel.Size = new System.Drawing.Size(604, 52);
            this.gbxPanel.TabIndex = 8;
            this.gbxPanel.TabStop = false;
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(156, 18);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(144, 23);
            this.btnStop.TabIndex = 10;
            this.btnStop.Text = "BL01データ受信の停止";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(485, 18);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(113, 23);
            this.btnClear.TabIndex = 7;
            this.btnClear.Text = "表示の消去";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnReceive
            // 
            this.btnReceive.Location = new System.Drawing.Point(6, 18);
            this.btnReceive.Name = "btnReceive";
            this.btnReceive.Size = new System.Drawing.Size(144, 23);
            this.btnReceive.TabIndex = 6;
            this.btnReceive.Text = "BL01データ受信";
            this.btnReceive.UseVisualStyleBackColor = true;
            this.btnReceive.Click += new System.EventHandler(this.btnReceive_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(632, 474);
            this.Controls.Add(this.gbxPanel);
            this.Controls.Add(this.tbxMessages);
            this.Name = "MainForm";
            this.Text = "EP-BL01 Broadcast Receiver";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.gbxPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbxMessages;
        private System.Windows.Forms.Timer timMessage;
        private System.Windows.Forms.GroupBox gbxPanel;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnReceive;
    }
}

