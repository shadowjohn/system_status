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
using System.Collections.Concurrent;
using System.Net;

namespace system_status
{
    public partial class Form1 : Form
    {
        private FileStream s2 = null;
        public string LOCK_FILE = "";
        public bool GLOBAL_RUN_AT_START = false;
        public double VERSION = 1.0;
        public string LOG_PATH = "";
        public myinclude my = null;
        system_info cSystem = null;
        hdd_info cHdd = null;
        firewall_info cFirewall = null;
        events cEvents = null;
        system_service cSystemService = null;
        running_program cRunningProgram = null;
        schedule cSchedule = null;
        iis cIis = null;
        //ini cIni = null;
        //public IniData iniData = null; // 儲存 config 設定
        public FileIniDataParser iniParser = new FileIniDataParser();
        static public Form theform;
        public Dictionary<string, Thread> threads = new Dictionary<string, Thread>();
        public void setStatusBar(string title, int percent)
        {
            percent = (percent >= 100) ? 100 : percent;
            //toolStripStatusLabel1.Text = title;
            UpdateUI(title, toolStripStatusLabel1);
            //toolStripProgressBar1.Value = percent;
            UpdateUI_toolStripProgressBar(percent, toolStripProgressBar1);
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
        private void myCrash(object sender, UnhandledExceptionEventArgs args)
        {
            killAllThreads("ALL");
            notifyIcon1.Visible = false;
            notifyIcon1.Dispose();
            Application.Exit();
        }
        private void killAllThreads(string killSingleTime)
        {
            //killSingleTime 可以是 thread index name            
            //killSingleTime 可以是 ALL
            //remove all threads
            //From : http://godleon.blogspot.com/2011/06/linq.html                        
            foreach (string index in this.threads.Keys.ToArray())
            {
                //時間會放在最後 _ 如果 killSingleTime = "ALL" 就全刪
                //平常就是看目前的秒 % killSingleTime
                switch (killSingleTime.ToUpper())
                {
                    case "ALL":
                        {
                            try
                            {
                                threads[index].Abort();
                            }
                            catch { }
                            threads[index] = null;
                        }
                        break;
                    default:
                        {
                            //如果是直接指定 index 就刪直接刪
                            if (index == killSingleTime)
                            {
                                threads[index].Abort();
                                threads[index] = null;
                            }
                        }
                        break;
                }
            }
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

        private delegate void UpdateUICallBack_toolStripProgressBar(int value, ToolStripProgressBar ctl);
        private void UpdateUI_toolStripProgressBar(int value, ToolStripProgressBar ctl)
        {
            if (this.InvokeRequired)
            {
                UpdateUICallBack_toolStripProgressBar uu = new UpdateUICallBack_toolStripProgressBar(UpdateUI_toolStripProgressBar);
                this.Invoke(uu, value, ctl);
            }
            else
            {
                ctl.Value = value;
            }
        }

        private delegate int UpdateUICallBack_DataGridGrid(DataGridView ctl, string kind, string key, object value, int index = -1);
        public int UpdateUI_DataGridGrid(DataGridView ctl, string kind, string key, object value, int index = -1)
        {
            if (this.InvokeRequired)
            {
                UpdateUICallBack_DataGridGrid uu = new UpdateUICallBack_DataGridGrid(UpdateUI_DataGridGrid);
                this.Invoke(uu, ctl, kind, key, value, index);
                return -1;
            }
            else
            {
                switch (kind.ToLower())
                {
                    case "clear":
                        ctl.Rows.Clear();
                        return -1;
                    case "add":
                        ctl.Rows.Add();
                        return ctl.Rows.Count - 1;
                    case "set_cell":
                        if (index != -1)
                        {
                            ctl.Rows[index].Cells[key].Value = value.ToString();
                        }
                        else
                        {
                            ctl.Rows[ctl.Rows.Count - 1].Cells[key].Value = value.ToString();
                        }
                        return -1;
                    case "set_font":
                        if (index != -1)
                        {
                            ctl.Rows[index].Cells[key].Style.Font = (Font)value;
                        }
                        else
                        {
                            ctl.Rows[ctl.Rows.Count - 1].Cells[key].Style.Font = (Font)value;
                        }
                        return -1;
                    default:
                        return -1;
                }
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
            cEvents = new events();
            cIis = new iis();
            //cIni = new ini();

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
                    //threads["tabs_host"] = new Thread(() => cSystem.init(this));
                    //threads["tabs_host"].Start();
                    if (cSystem.last_date == "" || Convert.ToInt32(my.time()) - Convert.ToInt32(cSystem.last_date) >= 5 * 60)
                    {
                        cSystem.init(this);
                    }
                    break;
                case "tabs_hdd":
                    //讀硬碟的
                    log("硬碟");
                    if (cHdd.last_date == "" || Convert.ToInt32(my.time()) - Convert.ToInt32(cHdd.last_date) >= 5 * 60)
                    {
                        cHdd.init(this);
                    }
                    break;
                case "tabs_running_program":
                    //執行緒
                    log("執行緒");
                    if (cRunningProgram.last_date == "" || Convert.ToInt32(my.time()) - Convert.ToInt32(cRunningProgram.last_date) >= 5 * 60)
                    {
                        cRunningProgram.init(this);
                    }
                    break;
                case "tabs_schedule":
                    //排程
                    log("排程");
                    if (cSchedule.last_date == "" || Convert.ToInt32(my.time()) - Convert.ToInt32(cSchedule.last_date) >= 5 * 60)
                    {
                        cSchedule.init(this);
                    }
                    break;
                case "tabs_service":
                    //服務
                    log("服務");
                    if (cSystemService.last_date == "" || Convert.ToInt32(my.time()) - Convert.ToInt32(cSystemService.last_date) >= 5 * 60)
                    {
                        cSystemService.init(this);
                    }
                    break;
                case "tabs_firewall":
                    //防火牆
                    log("防火牆");
                    if (cFirewall.last_date == "" || Convert.ToInt32(my.time()) - Convert.ToInt32(cFirewall.last_date) >= 5 * 60)
                    {
                        cFirewall.init(this);
                    }
                    break;
                case "tabs_events":
                    //events
                    log("事件");
                    if (cEvents.last_date == "" || Convert.ToInt32(my.time()) - Convert.ToInt32(cEvents.last_date) >= 5 * 60)
                    {
                        cEvents.init(this);
                    }
                    break;
                case "tabs_IIS":
                    //IIS
                    log("IIS");
                    if (cIis.last_date == "" || Convert.ToInt32(my.time()) - Convert.ToInt32(cIis.last_date) >= 5 * 60)
                    {
                        cIis.init(this);
                    }
                    break;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            //cIni.ini_save();
        }
        public void alert(string message)
        {
            MessageBox.Show(message);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //setStatusBarTitle(my.getCPUId(), 5000);
        }

        private void btnManual_Click(object sender, EventArgs e)
        {
            switch (run_status_label.Text)
            {
                case "尚未啟動":

                    run_status_label.Text = "啟動中...";
                    run_status_label.ForeColor = Color.Green;
                    threads["RUN_UPLOAD"] = new Thread(() =>
                    {
                        while (true)
                        {
                            setStatusBar("同步開始...", 0);
                            //上傳
                            run_upload();
                            setStatusBar("閒置中...", 0);
                            //每 10 分鐘傳一次
                            //Thread.Sleep(10 * 60);
                            Thread.Sleep(Convert.ToInt32(my.getSystemKey("LOOP_MINUTE")) * 60 * 1000);
                            //Thread.Sleep(30 * 1000);
                        }
                    });
                    threads["RUN_UPLOAD"].Start();

                    break;
                case "啟動中...":
                    run_status_label.Text = "尚未啟動";
                    run_status_label.ForeColor = Color.Red;
                    setStatusBar("同步停止...", 0);
                    if (threads.ContainsKey("RUN_UPLOAD"))
                    {
                        threads["RUN_UPLOAD"].Abort();
                        threads["RUN_UPLOAD"] = null;
                    }
                    break;
            }

        }
        private void run_upload()
        {
            if (textSystemName.Text == "")
            {
                return;
            }
            //手動同步
            //收集所有 gridview 的內容
            //組合成 json 加密後，上傳到伺服器
            Dictionary<string, object> output = new Dictionary<string, object>();
            setStatusBar("同步開始...", 0);
            output["NAME"] = textSystemName.Text;
            setStatusBar("同步開始...取得系統資訊", 20);
            cSystem.init(this);
            cHdd.init(this);
            cEvents.init(this);
            cFirewall.init(this);
            cRunningProgram.init(this);
            cSystemService.init(this);
            cSchedule.init(this);
            cIis.init(this);
            Thread.Sleep(3000);
            while (
                cSystem.is_running == true ||
                cHdd.is_running == true ||
                cEvents.is_running == true ||
                cRunningProgram.is_running == true ||
                cSystemService.is_running == true ||
                cFirewall.is_running == true ||
                cSchedule.is_running == true ||
                cIis.is_running == true)
            {
                Thread.Sleep(1000);
                setStatusBar("等待資料完成...", 0);
            }

            //setStatusBar("同步開始...取得硬碟資訊", 40);            
            output["SYSTEM_INFO"] = my.gridViewToDataTable(system_grid);
            output["SYSTEM_SERVICE_INFO"] = my.gridViewToDataTable(system_service_grid);
            output["HDD_INFO"] = my.gridViewToDataTable(hdd_grid);
            output["FIREWALL_INFO"] = my.gridViewToDataTable(firewall_grid);
            output["EVENTS_INFO"] = my.gridViewToDataTable(events_grid);
            output["TASK_INFO"] = my.gridViewToDataTable(running_program_grid);
            output["SCHEDULE_INFO"] = my.gridViewToDataTable(schedule_grid);
            output["IIS_INFO"] = my.gridViewToDataTable(iis_grid);

            //logError(my.json_encode_formated(output));
            string URL = my.getSystemKey("REPORT_URL") + "?mode=updateStatus";
            ConcurrentDictionary<string, string> o = new ConcurrentDictionary<string, string>();
            o["data"] = my.base64_encode(my.Zip(my.json_encode(output)));
            output = null;
            try
            {
                string data = my.b2s(my.file_get_contents_post(URL, o));
                if (data != "")
                {
                    logError(data);
                }
            }
            catch (Exception ex)
            {
                logError(ex.Message + "\r\n" + ex.StackTrace);
            }
            o = null;
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



        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            /*foreach (var k in threads.Keys)
            {
                if (threads[k] != null)
                {
                    threads[k].Abort();
                }
            }
            */
            try
            {
                killAllThreads("ALL");
            }
            catch
            {

            }
            notifyIcon1.Visible = false;
            notifyIcon1.Dispose();
            exit();
        }
        private void exit()
        {
            System.Environment.Exit(0);
        }
        void create_log_dir()
        {
            if (!my.is_dir(LOG_PATH))
            {
                my.mkdir(LOG_PATH);
            }
        }
        public void logError(string data)
        {
            try
            {
                create_log_dir();
                my.file_put_contents(LOG_PATH + "\\" + my.date("Y-m-d") + ".txt", my.date("Y-m-d H:i:s") + ":\r\n" + data + "\r\n", true);
            }
            catch
            {

            }
        }
        void CLog(string data)
        {
            Console.WriteLine(data);
        }
        private delegate void UpdateUIDGVCallBack(DataGridView dgv, DataTable dt);
        public void updateDGVUI(DataGridView dgv, DataTable dt)
        {
            if (this.InvokeRequired)
            {
                UpdateUIDGVCallBack uu = new UpdateUIDGVCallBack(updateDGVUI);
                this.Invoke(uu, dgv, dt);
            }
            else
            {
                dgv.DataSource = dt;
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            //嘗試當掉就中斷離開
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(myCrash);
            this.LOCK_FILE = my.pwd() + "\\lock.txt";
            textSystemName.Text = my.getSystemKey("COMPUTER_NAME");
            if (!my.is_file(this.LOCK_FILE))
            {
                my.file_put_contents(this.LOCK_FILE, "");
            }
            if (my.is_file_lock(this.LOCK_FILE))
            {
                //如果目前已是 lock 就結束
                CLog("Error ... Another zip process is running... ");
                Form1_FormClosing(sender, null);
                return;
            }
            //lock file
            s2 = new FileStream(this.LOCK_FILE, FileMode.Open, FileAccess.Read, FileShare.None);

            this.LOG_PATH = my.pwd() + "\\log";
            create_log_dir();
            this.Text += string.Format(" - 版本：{0:0.0}", VERSION);

            notifyIcon1.BalloonTipText = "已縮小";
            notifyIcon1.BalloonTipTitle = this.Text;
            notifyIcon1.Text = this.Text;



            this.StartPosition = FormStartPosition.CenterScreen;
            this.TopMost = true;
            this.TopMost = false;
            this.CenterToScreen();
            //載入設定檔
            //cIni.ini_init(this);
            //預設看要帶哪一個
            //cHdd.init(this);
            //cRunningProgram.init(this);
            //tabControl1.SelectTab("tabs_running_program");
            tabControl1.SelectTab("tabs_setting");
            tabControl1_Click(new object(), new EventArgs());


            /*if (iniData["setting"]["NAME"] == "")
            {
                //首次使用，需先設定
                tabControl1.SelectTab("tabs_setting");
                tabControl1_Click(new object(), new EventArgs());
            }
            */

            switch (my.getSystemKey("RUN_AT_START").ToUpper())
            {
                case "YES":
                    {
                        GLOBAL_RUN_AT_START = true;
                    }
                    break;
            }

            if (GLOBAL_RUN_AT_START)
            {
                //自動按
                btnManual_Click(sender, e);
                //自動縮小
                WindowState = FormWindowState.Minimized;
                ShowInTaskbar = false;
                notifyIcon1.Visible = true;
                notifyIcon1.ShowBalloonTip(1000);
            }
        }
    }
}
