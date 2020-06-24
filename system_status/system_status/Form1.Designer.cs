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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabs_host = new System.Windows.Forms.TabPage();
            this.system_grid = new System.Windows.Forms.DataGridView();
            this.tabs_hdd = new System.Windows.Forms.TabPage();
            this.hdd_grid = new System.Windows.Forms.DataGridView();
            this.tabs_running_program = new System.Windows.Forms.TabPage();
            this.running_program_grid = new System.Windows.Forms.DataGridView();
            this.tabs_schedule = new System.Windows.Forms.TabPage();
            this.schedule_grid = new System.Windows.Forms.DataGridView();
            this.tabs_service = new System.Windows.Forms.TabPage();
            this.system_service_grid = new System.Windows.Forms.DataGridView();
            this.tabs_firewall = new System.Windows.Forms.TabPage();
            this.firewall_grid = new System.Windows.Forms.DataGridView();
            this.tabs_IIS = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tabs_setting = new System.Windows.Forms.TabPage();
            this.btnManual = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.textSystemName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.tabControl1.SuspendLayout();
            this.tabs_host.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.system_grid)).BeginInit();
            this.tabs_hdd.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.hdd_grid)).BeginInit();
            this.tabs_running_program.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.running_program_grid)).BeginInit();
            this.tabs_schedule.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.schedule_grid)).BeginInit();
            this.tabs_service.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.system_service_grid)).BeginInit();
            this.tabs_firewall.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.firewall_grid)).BeginInit();
            this.tabs_IIS.SuspendLayout();
            this.tabs_setting.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabs_host);
            this.tabControl1.Controls.Add(this.tabs_hdd);
            this.tabControl1.Controls.Add(this.tabs_running_program);
            this.tabControl1.Controls.Add(this.tabs_schedule);
            this.tabControl1.Controls.Add(this.tabs_service);
            this.tabControl1.Controls.Add(this.tabs_firewall);
            this.tabControl1.Controls.Add(this.tabs_IIS);
            this.tabControl1.Controls.Add(this.tabs_setting);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(808, 446);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.Click += new System.EventHandler(this.tabControl1_Click);
            // 
            // tabs_host
            // 
            this.tabs_host.Controls.Add(this.system_grid);
            this.tabs_host.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.tabs_host.Location = new System.Drawing.Point(4, 22);
            this.tabs_host.Name = "tabs_host";
            this.tabs_host.Padding = new System.Windows.Forms.Padding(3);
            this.tabs_host.Size = new System.Drawing.Size(800, 420);
            this.tabs_host.TabIndex = 0;
            this.tabs_host.Text = "本機資訊";
            this.tabs_host.UseVisualStyleBackColor = true;
            // 
            // system_grid
            // 
            this.system_grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.system_grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.system_grid.Location = new System.Drawing.Point(3, 3);
            this.system_grid.Name = "system_grid";
            this.system_grid.RowTemplate.Height = 24;
            this.system_grid.Size = new System.Drawing.Size(794, 414);
            this.system_grid.TabIndex = 0;
            // 
            // tabs_hdd
            // 
            this.tabs_hdd.Controls.Add(this.hdd_grid);
            this.tabs_hdd.Location = new System.Drawing.Point(4, 22);
            this.tabs_hdd.Name = "tabs_hdd";
            this.tabs_hdd.Padding = new System.Windows.Forms.Padding(3);
            this.tabs_hdd.Size = new System.Drawing.Size(800, 420);
            this.tabs_hdd.TabIndex = 1;
            this.tabs_hdd.Text = "硬碟資訊";
            this.tabs_hdd.UseVisualStyleBackColor = true;
            // 
            // hdd_grid
            // 
            this.hdd_grid.AllowUserToAddRows = false;
            this.hdd_grid.AllowUserToDeleteRows = false;
            this.hdd_grid.AllowUserToResizeRows = false;
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(3);
            this.hdd_grid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
            this.hdd_grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.hdd_grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.hdd_grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hdd_grid.EnableHeadersVisualStyles = false;
            this.hdd_grid.Location = new System.Drawing.Point(3, 3);
            this.hdd_grid.MultiSelect = false;
            this.hdd_grid.Name = "hdd_grid";
            this.hdd_grid.RowTemplate.Height = 24;
            this.hdd_grid.ShowEditingIcon = false;
            this.hdd_grid.Size = new System.Drawing.Size(794, 414);
            this.hdd_grid.TabIndex = 1;
            // 
            // tabs_running_program
            // 
            this.tabs_running_program.Controls.Add(this.running_program_grid);
            this.tabs_running_program.Location = new System.Drawing.Point(4, 22);
            this.tabs_running_program.Name = "tabs_running_program";
            this.tabs_running_program.Size = new System.Drawing.Size(800, 420);
            this.tabs_running_program.TabIndex = 4;
            this.tabs_running_program.Text = "執行緒";
            this.tabs_running_program.UseVisualStyleBackColor = true;
            // 
            // running_program_grid
            // 
            this.running_program_grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.running_program_grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.running_program_grid.Location = new System.Drawing.Point(0, 0);
            this.running_program_grid.Name = "running_program_grid";
            this.running_program_grid.RowTemplate.Height = 24;
            this.running_program_grid.Size = new System.Drawing.Size(800, 420);
            this.running_program_grid.TabIndex = 1;
            // 
            // tabs_schedule
            // 
            this.tabs_schedule.Controls.Add(this.schedule_grid);
            this.tabs_schedule.Location = new System.Drawing.Point(4, 22);
            this.tabs_schedule.Name = "tabs_schedule";
            this.tabs_schedule.Size = new System.Drawing.Size(800, 420);
            this.tabs_schedule.TabIndex = 5;
            this.tabs_schedule.Text = "排程";
            this.tabs_schedule.UseVisualStyleBackColor = true;
            // 
            // schedule_grid
            // 
            this.schedule_grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.schedule_grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.schedule_grid.Location = new System.Drawing.Point(0, 0);
            this.schedule_grid.Name = "schedule_grid";
            this.schedule_grid.RowTemplate.Height = 24;
            this.schedule_grid.Size = new System.Drawing.Size(800, 420);
            this.schedule_grid.TabIndex = 1;
            // 
            // tabs_service
            // 
            this.tabs_service.Controls.Add(this.system_service_grid);
            this.tabs_service.Location = new System.Drawing.Point(4, 22);
            this.tabs_service.Name = "tabs_service";
            this.tabs_service.Size = new System.Drawing.Size(800, 420);
            this.tabs_service.TabIndex = 3;
            this.tabs_service.Text = "系統服務";
            this.tabs_service.UseVisualStyleBackColor = true;
            // 
            // system_service_grid
            // 
            this.system_service_grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.system_service_grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.system_service_grid.Location = new System.Drawing.Point(0, 0);
            this.system_service_grid.Name = "system_service_grid";
            this.system_service_grid.RowTemplate.Height = 24;
            this.system_service_grid.Size = new System.Drawing.Size(800, 420);
            this.system_service_grid.TabIndex = 1;
            // 
            // tabs_firewall
            // 
            this.tabs_firewall.Controls.Add(this.firewall_grid);
            this.tabs_firewall.Location = new System.Drawing.Point(4, 22);
            this.tabs_firewall.Name = "tabs_firewall";
            this.tabs_firewall.Size = new System.Drawing.Size(800, 420);
            this.tabs_firewall.TabIndex = 2;
            this.tabs_firewall.Text = "防火牆資訊";
            this.tabs_firewall.UseVisualStyleBackColor = true;
            // 
            // firewall_grid
            // 
            this.firewall_grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.firewall_grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.firewall_grid.Location = new System.Drawing.Point(0, 0);
            this.firewall_grid.Name = "firewall_grid";
            this.firewall_grid.RowTemplate.Height = 24;
            this.firewall_grid.Size = new System.Drawing.Size(800, 420);
            this.firewall_grid.TabIndex = 1;
            // 
            // tabs_IIS
            // 
            this.tabs_IIS.Controls.Add(this.groupBox1);
            this.tabs_IIS.Location = new System.Drawing.Point(4, 22);
            this.tabs_IIS.Name = "tabs_IIS";
            this.tabs_IIS.Padding = new System.Windows.Forms.Padding(3);
            this.tabs_IIS.Size = new System.Drawing.Size(800, 420);
            this.tabs_IIS.TabIndex = 6;
            this.tabs_IIS.Text = "IIS檢測";
            this.tabs_IIS.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(580, 37);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(214, 244);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // tabs_setting
            // 
            this.tabs_setting.Controls.Add(this.btnManual);
            this.tabs_setting.Controls.Add(this.button1);
            this.tabs_setting.Controls.Add(this.btnSave);
            this.tabs_setting.Controls.Add(this.textSystemName);
            this.tabs_setting.Controls.Add(this.label1);
            this.tabs_setting.Location = new System.Drawing.Point(4, 22);
            this.tabs_setting.Name = "tabs_setting";
            this.tabs_setting.Size = new System.Drawing.Size(800, 420);
            this.tabs_setting.TabIndex = 7;
            this.tabs_setting.Text = "功能設定";
            this.tabs_setting.UseVisualStyleBackColor = true;
            // 
            // btnManual
            // 
            this.btnManual.Location = new System.Drawing.Point(652, 313);
            this.btnManual.Name = "btnManual";
            this.btnManual.Size = new System.Drawing.Size(75, 23);
            this.btnManual.TabIndex = 4;
            this.btnManual.Text = "手動同步";
            this.btnManual.UseVisualStyleBackColor = true;
            this.btnManual.Click += new System.EventHandler(this.btnManual_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(489, 313);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "test";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnSave.Location = new System.Drawing.Point(476, 238);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(106, 34);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "儲存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // textSystemName
            // 
            this.textSystemName.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.textSystemName.Location = new System.Drawing.Point(154, 112);
            this.textSystemName.Name = "textSystemName";
            this.textSystemName.Size = new System.Drawing.Size(359, 36);
            this.textSystemName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(27, 124);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(130, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "系統名稱：";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.statusStrip1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 51F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(814, 503);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripProgressBar1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 481);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(814, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(31, 17);
            this.toolStripStatusLabel1.Text = "就緒";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.AutoSize = false;
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(400, 16);
            this.toolStripProgressBar1.Step = 1;
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(814, 503);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "系統安全小工具";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.tabControl1.ResumeLayout(false);
            this.tabs_host.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.system_grid)).EndInit();
            this.tabs_hdd.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.hdd_grid)).EndInit();
            this.tabs_running_program.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.running_program_grid)).EndInit();
            this.tabs_schedule.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.schedule_grid)).EndInit();
            this.tabs_service.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.system_service_grid)).EndInit();
            this.tabs_firewall.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.firewall_grid)).EndInit();
            this.tabs_IIS.ResumeLayout(false);
            this.tabs_setting.ResumeLayout(false);
            this.tabs_setting.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.TabControl tabControl1;
        public System.Windows.Forms.TabPage tabs_host;
        public System.Windows.Forms.TabPage tabs_hdd;
        public System.Windows.Forms.TabPage tabs_firewall;
        public System.Windows.Forms.TabPage tabs_service;
        public System.Windows.Forms.TabPage tabs_running_program;
        public System.Windows.Forms.TabPage tabs_schedule;
        public System.Windows.Forms.DataGridView system_grid;
        public System.Windows.Forms.DataGridView hdd_grid;
        public System.Windows.Forms.DataGridView firewall_grid;
        public System.Windows.Forms.DataGridView system_service_grid;
        public System.Windows.Forms.DataGridView running_program_grid;
        public System.Windows.Forms.DataGridView schedule_grid;
        public System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        public System.Windows.Forms.StatusStrip statusStrip1;
        public System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        public System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        public System.Windows.Forms.TabPage tabs_IIS;
        public System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.TabPage tabs_setting;
        public System.Windows.Forms.Button btnSave;
        public System.Windows.Forms.TextBox textSystemName;
        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.Button button1;
        public System.Windows.Forms.Button btnManual;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
    }
}

