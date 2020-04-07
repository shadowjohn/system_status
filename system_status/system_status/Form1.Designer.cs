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
            this.location_grid = new System.Windows.Forms.DataGridView();
            this.tabs_hdd = new System.Windows.Forms.TabPage();
            this.hdd_grid = new System.Windows.Forms.DataGridView();
            this.tabs_firewall = new System.Windows.Forms.TabPage();
            this.firewall_grid = new System.Windows.Forms.DataGridView();
            this.tabs_service = new System.Windows.Forms.TabPage();
            this.system_service_grid = new System.Windows.Forms.DataGridView();
            this.tabs_running_program = new System.Windows.Forms.TabPage();
            this.tasks = new System.Windows.Forms.DataGridView();
            this.tabs_task = new System.Windows.Forms.TabPage();
            this.schedule_grid = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabs_IIS = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tabControl1.SuspendLayout();
            this.tabs_host.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.location_grid)).BeginInit();
            this.tabs_hdd.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.hdd_grid)).BeginInit();
            this.tabs_firewall.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.firewall_grid)).BeginInit();
            this.tabs_service.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.system_service_grid)).BeginInit();
            this.tabs_running_program.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tasks)).BeginInit();
            this.tabs_task.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.schedule_grid)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.tabs_IIS.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabs_host);
            this.tabControl1.Controls.Add(this.tabs_hdd);
            this.tabControl1.Controls.Add(this.tabs_running_program);
            this.tabControl1.Controls.Add(this.tabs_task);
            this.tabControl1.Controls.Add(this.tabs_service);
            this.tabControl1.Controls.Add(this.tabs_firewall);
            this.tabControl1.Controls.Add(this.tabs_IIS);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(808, 389);
            this.tabControl1.TabIndex = 0;
            // 
            // tabs_host
            // 
            this.tabs_host.Controls.Add(this.location_grid);
            this.tabs_host.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.tabs_host.Location = new System.Drawing.Point(4, 22);
            this.tabs_host.Name = "tabs_host";
            this.tabs_host.Padding = new System.Windows.Forms.Padding(3);
            this.tabs_host.Size = new System.Drawing.Size(800, 363);
            this.tabs_host.TabIndex = 0;
            this.tabs_host.Text = "本機資訊";
            this.tabs_host.UseVisualStyleBackColor = true;
            // 
            // location_grid
            // 
            this.location_grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.location_grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.location_grid.Location = new System.Drawing.Point(3, 3);
            this.location_grid.Name = "location_grid";
            this.location_grid.RowTemplate.Height = 24;
            this.location_grid.Size = new System.Drawing.Size(794, 357);
            this.location_grid.TabIndex = 0;
            this.location_grid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // tabs_hdd
            // 
            this.tabs_hdd.Controls.Add(this.hdd_grid);
            this.tabs_hdd.Location = new System.Drawing.Point(4, 22);
            this.tabs_hdd.Name = "tabs_hdd";
            this.tabs_hdd.Padding = new System.Windows.Forms.Padding(3);
            this.tabs_hdd.Size = new System.Drawing.Size(800, 363);
            this.tabs_hdd.TabIndex = 1;
            this.tabs_hdd.Text = "硬碟資訊";
            this.tabs_hdd.UseVisualStyleBackColor = true;
            // 
            // hdd_grid
            // 
            this.hdd_grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.hdd_grid.Location = new System.Drawing.Point(6, 6);
            this.hdd_grid.Name = "hdd_grid";
            this.hdd_grid.RowTemplate.Height = 24;
            this.hdd_grid.Size = new System.Drawing.Size(778, 445);
            this.hdd_grid.TabIndex = 1;
            // 
            // tabs_firewall
            // 
            this.tabs_firewall.Controls.Add(this.firewall_grid);
            this.tabs_firewall.Location = new System.Drawing.Point(4, 22);
            this.tabs_firewall.Name = "tabs_firewall";
            this.tabs_firewall.Size = new System.Drawing.Size(800, 363);
            this.tabs_firewall.TabIndex = 2;
            this.tabs_firewall.Text = "防火牆資訊";
            this.tabs_firewall.UseVisualStyleBackColor = true;
            // 
            // firewall_grid
            // 
            this.firewall_grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.firewall_grid.Location = new System.Drawing.Point(6, 6);
            this.firewall_grid.Name = "firewall_grid";
            this.firewall_grid.RowTemplate.Height = 24;
            this.firewall_grid.Size = new System.Drawing.Size(778, 445);
            this.firewall_grid.TabIndex = 1;
            // 
            // tabs_service
            // 
            this.tabs_service.Controls.Add(this.system_service_grid);
            this.tabs_service.Location = new System.Drawing.Point(4, 22);
            this.tabs_service.Name = "tabs_service";
            this.tabs_service.Size = new System.Drawing.Size(800, 363);
            this.tabs_service.TabIndex = 3;
            this.tabs_service.Text = "系統服務";
            this.tabs_service.UseVisualStyleBackColor = true;
            // 
            // system_service_grid
            // 
            this.system_service_grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.system_service_grid.Location = new System.Drawing.Point(6, 6);
            this.system_service_grid.Name = "system_service_grid";
            this.system_service_grid.RowTemplate.Height = 24;
            this.system_service_grid.Size = new System.Drawing.Size(778, 445);
            this.system_service_grid.TabIndex = 1;
            // 
            // tabs_running_program
            // 
            this.tabs_running_program.Controls.Add(this.tasks);
            this.tabs_running_program.Location = new System.Drawing.Point(4, 22);
            this.tabs_running_program.Name = "tabs_running_program";
            this.tabs_running_program.Size = new System.Drawing.Size(800, 363);
            this.tabs_running_program.TabIndex = 4;
            this.tabs_running_program.Text = "執行緒";
            this.tabs_running_program.UseVisualStyleBackColor = true;
            // 
            // tasks
            // 
            this.tasks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tasks.Location = new System.Drawing.Point(6, 6);
            this.tasks.Name = "tasks";
            this.tasks.RowTemplate.Height = 24;
            this.tasks.Size = new System.Drawing.Size(778, 445);
            this.tasks.TabIndex = 1;
            // 
            // tabs_task
            // 
            this.tabs_task.Controls.Add(this.schedule_grid);
            this.tabs_task.Location = new System.Drawing.Point(4, 22);
            this.tabs_task.Name = "tabs_task";
            this.tabs_task.Size = new System.Drawing.Size(800, 363);
            this.tabs_task.TabIndex = 5;
            this.tabs_task.Text = "排程";
            this.tabs_task.UseVisualStyleBackColor = true;
            // 
            // schedule_grid
            // 
            this.schedule_grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.schedule_grid.Location = new System.Drawing.Point(6, 6);
            this.schedule_grid.Name = "schedule_grid";
            this.schedule_grid.RowTemplate.Height = 24;
            this.schedule_grid.Size = new System.Drawing.Size(778, 445);
            this.schedule_grid.TabIndex = 1;
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
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 108F));
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
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(31, 17);
            this.toolStripStatusLabel1.Text = "就緒";
            // 
            // tabs_IIS
            // 
            this.tabs_IIS.Controls.Add(this.groupBox1);
            this.tabs_IIS.Location = new System.Drawing.Point(4, 22);
            this.tabs_IIS.Name = "tabs_IIS";
            this.tabs_IIS.Padding = new System.Windows.Forms.Padding(3);
            this.tabs_IIS.Size = new System.Drawing.Size(800, 363);
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
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(814, 503);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Form1";
            this.Text = "系統安全小工具";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabs_host.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.location_grid)).EndInit();
            this.tabs_hdd.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.hdd_grid)).EndInit();
            this.tabs_firewall.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.firewall_grid)).EndInit();
            this.tabs_service.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.system_service_grid)).EndInit();
            this.tabs_running_program.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tasks)).EndInit();
            this.tabs_task.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.schedule_grid)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tabs_IIS.ResumeLayout(false);
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
        private System.Windows.Forms.DataGridView location_grid;
        private System.Windows.Forms.DataGridView hdd_grid;
        private System.Windows.Forms.DataGridView firewall_grid;
        private System.Windows.Forms.DataGridView system_service_grid;
        private System.Windows.Forms.DataGridView tasks;
        private System.Windows.Forms.DataGridView schedule_grid;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.TabPage tabs_IIS;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}

