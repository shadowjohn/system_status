﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using utility;
using system_status.App_code;
using IniParser;
using IniParser.Model;
using System.Threading.Tasks;

namespace system_status
{
    public partial class Form1 : Form
    {
        public myinclude my = null;
        system_info cSystem = null;
        hdd_info cHdd = null;
        firewall_info cFirewall = null;
        system_service cSystemService = null;
        running_program cRunningProgram = null;
        schedule cSchedule = null;
        ini cIni = null;
        public IniData iniData = null; // 儲存 config 設定
        public FileIniDataParser iniParser = new FileIniDataParser();
        static public Form theform;
        public void setStatusBar(string title, int percent)
        {
            toolStripStatusLabel1.Text = title;
            toolStripProgressBar1.Value = percent;
        }
        public void setStatusBarTitle(string title)
        {
            setStatusBarTitle(title, -1);
        }
        public void setStatusBarTitle(string title, int ms) //設定 startbar title 一段時間後自動回歸 就緒
        {
            toolStripStatusLabel1.Text = title;
            if (ms == -1)
            {
                return;
            }
            //https://dotblogs.com.tw/rainmaker/2016/09/04/232541 這樣寫就不會卡 UI 了
            var task = Task.Run(() =>
            {
                Thread.Sleep(Convert.ToInt32(ms));
            });
            task.ContinueWith((completedTask) =>
            {
                UpdateUI("就緒", toolStripStatusLabel1);
            });
        }
        private delegate void UpdateUICallBack(string value, ToolStripStatusLabel ctl);
        private void UpdateUI(string value, ToolStripStatusLabel ctl)
        {
            if (this.InvokeRequired)
            {
                UpdateUICallBack uu = new UpdateUICallBack(UpdateUI);
                this.Invoke(uu, value, ctl);
            }
            else
            {
                ctl.Text = value;
            }
        }
        public Form1()
        {
            my = new myinclude();
            InitializeComponent();
            theform = this;
            cSystem = new system_info();
            cHdd = new hdd_info();//硬碟資訊
            cRunningProgram = new running_program(); //工作管理員
            cFirewall = new firewall_info();
            cSystemService = new system_service();
            cSchedule = new schedule();
            cIni = new ini();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            notifyIcon1.BalloonTipText = "已縮小";
            notifyIcon1.BalloonTipTitle = this.Text;
            notifyIcon1.Text = this.Text;

            this.StartPosition = FormStartPosition.CenterScreen;
            this.TopMost = true;
            this.TopMost = false;
            this.CenterToScreen();
            //載入設定檔
            cIni.ini_init(this);
            //預設看要帶哪一個
            //cHdd.init(this);
            //cRunningProgram.init(this);
            //tabControl1.SelectTab("tabs_running_program");
            tabControl1.SelectTab("tabs_setting");
            tabControl1_Click(new object(), new EventArgs());
            if (iniData["setting"]["NAME"] == "")
            {
                //首次使用，需先設定
                tabControl1.SelectTab("tabs_setting");
                tabControl1_Click(new object(), new EventArgs());
            }

        }
        void log(string input)
        {
            Console.WriteLine(input);
        }



        private void tabControl1_Click(object sender, EventArgs e)
        {
            //Console.WriteLine(tabControl1.SelectedTab.Name);
            switch (tabControl1.SelectedTab.Name)
            {
                case "tabs_host":
                    //本機資訊
                    log("本機資訊");
                    cSystem.init(this);
                    break;
                case "tabs_hdd":
                    //讀硬碟的
                    log("硬碟");
                    cHdd.init(this);
                    break;
                case "tabs_running_program":
                    //執行緒
                    log("執行緒");
                    cRunningProgram.init(this);
                    break;
                case "tabs_schedule":
                    //排程
                    log("排程");
                    break;
                case "tabs_service":
                    //服務
                    log("服務");
                    break;
                case "tabs_firewall":
                    //防火牆
                    log("防火牆");
                    cFirewall.init(this);
                    break;
                case "tabs_IIS":
                    //IIS
                    log("IIS");
                    break;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            cIni.ini_save();
        }
        public void alert(string message)
        {
            MessageBox.Show(message);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            setStatusBarTitle(my.getCPUId(), 5000);
        }

        private void btnManual_Click(object sender, EventArgs e)
        {
            //手動同步
            //收集所有 gridview 的內容
            //組合成 json 加密後，上傳到伺服器
            Dictionary<string, object> output = new Dictionary<string, object>();
            setStatusBar("同步開始...", 0);
            output["NAME"] = textSystemName.Text;
            output["CPUID"] = my.getCPUId();
            setStatusBar("同步開始...取得系統資訊", 20);

            cSystem.init(this);
            output["SYSTEM_INFO"] = my.gridViewToDataTable(system_grid);
            setStatusBar("同步開始...取得硬碟資訊", 40);
            cHdd.init(this);
            output["HDD_INFO"] = my.gridViewToDataTable(hdd_grid);
            log(my.json_encode_formated(output));
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                ShowInTaskbar = false;
                notifyIcon1.Visible = true;
                notifyIcon1.ShowBalloonTip(1000);
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ShowInTaskbar = true;
            notifyIcon1.Visible = true;
            WindowState = FormWindowState.Normal;
        }

        private void Form1_Leave(object sender, EventArgs e)
        {
            notifyIcon1.Visible = false;
        }
    }
}
