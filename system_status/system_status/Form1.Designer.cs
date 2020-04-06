namespace system_status
{
    public partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabs_host = new System.Windows.Forms.TabPage();
            this.tabs_hdd = new System.Windows.Forms.TabPage();
            this.tabs_firewall = new System.Windows.Forms.TabPage();
            this.tabs_service = new System.Windows.Forms.TabPage();
            this.tabs_running_program = new System.Windows.Forms.TabPage();
            this.tabs_task = new System.Windows.Forms.TabPage();
            this.wb1 = new CefSharp.WinForms.ChromiumWebBrowser();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabs_host);
            this.tabControl1.Controls.Add(this.tabs_hdd);
            this.tabControl1.Controls.Add(this.tabs_firewall);
            this.tabControl1.Controls.Add(this.tabs_service);
            this.tabControl1.Controls.Add(this.tabs_running_program);
            this.tabControl1.Controls.Add(this.tabs_task);
            this.tabControl1.Location = new System.Drawing.Point(6, 156);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(798, 339);
            this.tabControl1.TabIndex = 0;
            // 
            // tabs_host
            // 
            this.tabs_host.Location = new System.Drawing.Point(4, 22);
            this.tabs_host.Name = "tabs_host";
            this.tabs_host.Padding = new System.Windows.Forms.Padding(3);
            this.tabs_host.Size = new System.Drawing.Size(790, 313);
            this.tabs_host.TabIndex = 0;
            this.tabs_host.Text = "本機資訊";
            this.tabs_host.UseVisualStyleBackColor = true;
            // 
            // tabs_hdd
            // 
            this.tabs_hdd.Location = new System.Drawing.Point(4, 22);
            this.tabs_hdd.Name = "tabs_hdd";
            this.tabs_hdd.Padding = new System.Windows.Forms.Padding(3);
            this.tabs_hdd.Size = new System.Drawing.Size(790, 313);
            this.tabs_hdd.TabIndex = 1;
            this.tabs_hdd.Text = "硬碟資訊";
            this.tabs_hdd.UseVisualStyleBackColor = true;
            // 
            // tabs_firewall
            // 
            this.tabs_firewall.Location = new System.Drawing.Point(4, 22);
            this.tabs_firewall.Name = "tabs_firewall";
            this.tabs_firewall.Size = new System.Drawing.Size(790, 313);
            this.tabs_firewall.TabIndex = 2;
            this.tabs_firewall.Text = "防火牆資訊";
            this.tabs_firewall.UseVisualStyleBackColor = true;
            // 
            // tabs_service
            // 
            this.tabs_service.Location = new System.Drawing.Point(4, 22);
            this.tabs_service.Name = "tabs_service";
            this.tabs_service.Size = new System.Drawing.Size(790, 313);
            this.tabs_service.TabIndex = 3;
            this.tabs_service.Text = "系統服務";
            this.tabs_service.UseVisualStyleBackColor = true;
            // 
            // tabs_running_program
            // 
            this.tabs_running_program.Location = new System.Drawing.Point(4, 22);
            this.tabs_running_program.Name = "tabs_running_program";
            this.tabs_running_program.Size = new System.Drawing.Size(790, 313);
            this.tabs_running_program.TabIndex = 4;
            this.tabs_running_program.Text = "執行緒";
            this.tabs_running_program.UseVisualStyleBackColor = true;
            // 
            // tabs_task
            // 
            this.tabs_task.Location = new System.Drawing.Point(4, 22);
            this.tabs_task.Name = "tabs_task";
            this.tabs_task.Size = new System.Drawing.Size(790, 313);
            this.tabs_task.TabIndex = 5;
            this.tabs_task.Text = "排程";
            this.tabs_task.UseVisualStyleBackColor = true;
            // 
            // wb1
            // 
            this.wb1.ActivateBrowserOnCreation = false;
            this.wb1.Location = new System.Drawing.Point(314, 12);
            this.wb1.Name = "wb1";
            this.wb1.Size = new System.Drawing.Size(441, 216);
            this.wb1.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(814, 503);
            this.Controls.Add(this.wb1);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "系統安全後門檢測機";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.TabControl tabControl1;
        public System.Windows.Forms.TabPage tabs_host;
        public System.Windows.Forms.TabPage tabs_hdd;
        private System.Windows.Forms.TabPage tabs_firewall;
        private System.Windows.Forms.TabPage tabs_service;
        private System.Windows.Forms.TabPage tabs_running_program;
        private System.Windows.Forms.TabPage tabs_task;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private CefSharp.WinForms.ChromiumWebBrowser wb1;
    }
}

