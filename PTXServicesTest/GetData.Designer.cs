namespace PTXServicesTest
{
    partial class GetData
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.BtnStat = new System.Windows.Forms.Button();
            this.BtnStop = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BtnStat
            // 
            this.BtnStat.Location = new System.Drawing.Point(90, 25);
            this.BtnStat.Name = "BtnStat";
            this.BtnStat.Size = new System.Drawing.Size(75, 23);
            this.BtnStat.TabIndex = 0;
            this.BtnStat.Text = "開始";
            this.BtnStat.UseVisualStyleBackColor = true;
            this.BtnStat.Click += new System.EventHandler(this.BtnStat_Click);
            // 
            // BtnStop
            // 
            this.BtnStop.Location = new System.Drawing.Point(188, 25);
            this.BtnStop.Name = "BtnStop";
            this.BtnStop.Size = new System.Drawing.Size(75, 23);
            this.BtnStop.TabIndex = 1;
            this.BtnStop.Text = "停止";
            this.BtnStop.UseVisualStyleBackColor = true;
            this.BtnStop.Click += new System.EventHandler(this.BtnStop_Click);
            // 
            // GetData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(345, 130);
            this.Controls.Add(this.BtnStop);
            this.Controls.Add(this.BtnStat);
            this.Name = "GetData";
            this.Text = "取得PTX路線資料";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtnStat;
        private System.Windows.Forms.Button BtnStop;
    }
}

