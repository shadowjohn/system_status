using System;
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
        location_info cLocation = null;
        hdd_info cHdd = null;
        firewall_info cFirewall = null;
        system_service cSystemService = null;
        tasks cTasks = null;
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
            var task = Task.Run(() => {                
                Thread.Sleep(Convert.ToInt32(ms));
            });
            task.ContinueWith((completedTask) => {
                UpdateUI("就緒",toolStripStatusLabel1) ;
            });
        }
        private delegate void UpdateUICallBack( string value, ToolStripStatusLabel ctl);
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
            cLocation = new location_info();
            cHdd = new hdd_info();//硬碟資訊
            cFirewall = new firewall_info();
            cSystemService = new system_service();
            cSchedule = new schedule();
            cIni = new ini();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            this.TopMost = true;
            this.TopMost = false;
            this.CenterToScreen();
            //載入設定檔
            cIni.ini_init(this);
            //預設看要帶哪一個
            //cHdd.hdd_init(this);
            tabControl1.SelectTab("tabs_hdd");
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
                    break;
                case "tabs_hdd":
                    //讀硬碟的
                    log("硬碟");
                    cHdd.hdd_init(this);
                    break;
                case "tabs_running_program":
                    //執行緒
                    log("執行緒");
                    break;
                case "tabs_task":
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
            setStatusBarTitle("你好", 5000);
        }
    }
}
