namespace BL01_ReaderWriter
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
            this.btnReScan = new System.Windows.Forms.Button();
            this.tbxMessages = new System.Windows.Forms.TextBox();
            this.timWriteMessage = new System.Windows.Forms.Timer(this.components);
            this.tbxServiceUUID = new System.Windows.Forms.TextBox();
            this.tbxCharacteristicsUUID = new System.Windows.Forms.TextBox();
            this.lblServiceUUID = new System.Windows.Forms.Label();
            this.lblCharacteristicsUUID = new System.Windows.Forms.Label();
            this.btnREAD = new System.Windows.Forms.Button();
            this.gbxPanel = new System.Windows.Forms.GroupBox();
            this.btnWrite = new System.Windows.Forms.Button();
            this.tbxWriteData = new System.Windows.Forms.TextBox();
            this.lblWriteData = new System.Windows.Forms.Label();
            this.btnTest = new System.Windows.Forms.Button();
            this.gbxPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnReScan
            // 
            this.btnReScan.Location = new System.Drawing.Point(12, 19);
            this.btnReScan.Name = "btnReScan";
            this.btnReScan.Size = new System.Drawing.Size(162, 23);
            this.btnReScan.TabIndex = 0;
            this.btnReScan.Text = "ペアリングセンサの再読み込み";
            this.btnReScan.UseVisualStyleBackColor = true;
            this.btnReScan.Click += new System.EventHandler(this.btnReScan_Click);
            // 
            // tbxMessages
            // 
            this.tbxMessages.Location = new System.Drawing.Point(12, 101);
            this.tbxMessages.Multiline = true;
            this.tbxMessages.Name = "tbxMessages";
            this.tbxMessages.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbxMessages.Size = new System.Drawing.Size(604, 445);
            this.tbxMessages.TabIndex = 1;
            // 
            // timWriteMessage
            // 
            this.timWriteMessage.Enabled = true;
            this.timWriteMessage.Interval = 15;
            this.timWriteMessage.Tick += new System.EventHandler(this.timWriteMessage_Tick);
            // 
            // tbxServiceUUID
            // 
            this.tbxServiceUUID.Location = new System.Drawing.Point(265, 21);
            this.tbxServiceUUID.Name = "tbxServiceUUID";
            this.tbxServiceUUID.Size = new System.Drawing.Size(42, 19);
            this.tbxServiceUUID.TabIndex = 1;
            this.tbxServiceUUID.Text = "3000";
            this.tbxServiceUUID.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tbxCharacteristicsUUID
            // 
            this.tbxCharacteristicsUUID.Location = new System.Drawing.Point(443, 21);
            this.tbxCharacteristicsUUID.Name = "tbxCharacteristicsUUID";
            this.tbxCharacteristicsUUID.Size = new System.Drawing.Size(42, 19);
            this.tbxCharacteristicsUUID.TabIndex = 3;
            this.tbxCharacteristicsUUID.Text = "3001";
            this.tbxCharacteristicsUUID.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblServiceUUID
            // 
            this.lblServiceUUID.AutoSize = true;
            this.lblServiceUUID.Location = new System.Drawing.Point(185, 24);
            this.lblServiceUUID.Name = "lblServiceUUID";
            this.lblServiceUUID.Size = new System.Drawing.Size(74, 12);
            this.lblServiceUUID.TabIndex = 4;
            this.lblServiceUUID.Text = "Service UUID";
            // 
            // lblCharacteristicsUUID
            // 
            this.lblCharacteristicsUUID.AutoSize = true;
            this.lblCharacteristicsUUID.Location = new System.Drawing.Point(323, 24);
            this.lblCharacteristicsUUID.Name = "lblCharacteristicsUUID";
            this.lblCharacteristicsUUID.Size = new System.Drawing.Size(114, 12);
            this.lblCharacteristicsUUID.TabIndex = 2;
            this.lblCharacteristicsUUID.Text = "Characteristics UUID";
            // 
            // btnREAD
            // 
            this.btnREAD.Location = new System.Drawing.Point(505, 19);
            this.btnREAD.Name = "btnREAD";
            this.btnREAD.Size = new System.Drawing.Size(55, 23);
            this.btnREAD.TabIndex = 4;
            this.btnREAD.Text = "READ";
            this.btnREAD.UseVisualStyleBackColor = true;
            this.btnREAD.Click += new System.EventHandler(this.btnREAD_Click);
            // 
            // gbxPanel
            // 
            this.gbxPanel.Controls.Add(this.btnWrite);
            this.gbxPanel.Controls.Add(this.tbxWriteData);
            this.gbxPanel.Controls.Add(this.lblWriteData);
            this.gbxPanel.Controls.Add(this.btnTest);
            this.gbxPanel.Controls.Add(this.btnREAD);
            this.gbxPanel.Controls.Add(this.btnReScan);
            this.gbxPanel.Controls.Add(this.lblCharacteristicsUUID);
            this.gbxPanel.Controls.Add(this.tbxServiceUUID);
            this.gbxPanel.Controls.Add(this.lblServiceUUID);
            this.gbxPanel.Controls.Add(this.tbxCharacteristicsUUID);
            this.gbxPanel.Location = new System.Drawing.Point(12, 12);
            this.gbxPanel.Name = "gbxPanel";
            this.gbxPanel.Size = new System.Drawing.Size(604, 83);
            this.gbxPanel.TabIndex = 0;
            this.gbxPanel.TabStop = false;
            // 
            // btnWrite
            // 
            this.btnWrite.Location = new System.Drawing.Point(505, 48);
            this.btnWrite.Name = "btnWrite";
            this.btnWrite.Size = new System.Drawing.Size(55, 23);
            this.btnWrite.TabIndex = 7;
            this.btnWrite.Text = "WRITE";
            this.btnWrite.UseVisualStyleBackColor = true;
            this.btnWrite.Click += new System.EventHandler(this.btnWrite_Click);
            // 
            // tbxWriteData
            // 
            this.tbxWriteData.Location = new System.Drawing.Point(265, 50);
            this.tbxWriteData.Name = "tbxWriteData";
            this.tbxWriteData.Size = new System.Drawing.Size(220, 19);
            this.tbxWriteData.TabIndex = 6;
            this.tbxWriteData.Text = "2C 01";
            this.tbxWriteData.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblWriteData
            // 
            this.lblWriteData.AutoSize = true;
            this.lblWriteData.Location = new System.Drawing.Point(89, 53);
            this.lblWriteData.Name = "lblWriteData";
            this.lblWriteData.Size = new System.Drawing.Size(170, 12);
            this.lblWriteData.TabIndex = 5;
            this.lblWriteData.Text = "Writing Hex Data (ex. 0A 03 FF)";
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(12, 48);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(55, 23);
            this.btnTest.TabIndex = 8;
            this.btnTest.Text = "TEST";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(628, 558);
            this.Controls.Add(this.gbxPanel);
            this.Controls.Add(this.tbxMessages);
            this.Name = "MainForm";
            this.Text = "BL01の設定を読み書きします";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.gbxPanel.ResumeLayout(false);
            this.gbxPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnReScan;
        private System.Windows.Forms.TextBox tbxMessages;
        private System.Windows.Forms.Timer timWriteMessage;
        private System.Windows.Forms.TextBox tbxServiceUUID;
        private System.Windows.Forms.TextBox tbxCharacteristicsUUID;
        private System.Windows.Forms.Label lblServiceUUID;
        private System.Windows.Forms.Label lblCharacteristicsUUID;
        private System.Windows.Forms.Button btnREAD;
        private System.Windows.Forms.GroupBox gbxPanel;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Button btnWrite;
        private System.Windows.Forms.TextBox tbxWriteData;
        private System.Windows.Forms.Label lblWriteData;
    }
}

