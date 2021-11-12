using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using utility;
using System.Runtime.InteropServices;
namespace system_status_watchdog
{

    public class Program
    {
        static public Program F1 = null;
        public string PWD = null;
        public myinclude my = new myinclude();
        Dictionary<string, Thread> threads = new Dictionary<string, Thread>();
        string LOG_PATH = null;
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;
        private void create_log_dir()
        {
            if (!my.is_dir(F1.LOG_PATH))
            {
                my.mkdir(F1.LOG_PATH);
            }
        }

        public void logError(string data)
        {
            try
            {
                if (!my.is_dir(LOG_PATH))
                {
                    my.mkdir(LOG_PATH);
                }
                my.file_put_contents(LOG_PATH + "\\" + my.date("Y-m-d") + ".txt", my.date("Y-m-d H:i:s") + ":\r\n" + data + "\r\n", true);
            }
            catch
            {

            }
        }
        private void myCrash(object sender, UnhandledExceptionEventArgs args)
        {
            System.Environment.Exit(0);
        }
        public void run_check_update()
        {
            //檢查目前版本，如果版本檔不存在，關程式，下載新版，執行      
            double local_version = 0;
            double remote_version = 0;
            bool isNeedUpdate = false;
            string downloadPath = "";
            //Console.WriteLine("step 1");
            if (!F1.my.is_file(F1.PWD + "\\version.txt"))
            {
                isNeedUpdate = true;
                //Console.WriteLine("step 2");
            }
            else
            {
                //檢查現在版本，跟網站上版本
                local_version = Convert.ToDouble(F1.my.b2s(F1.my.file_get_contents(F1.PWD + "\\version.txt")));
                remote_version = local_version;
                try
                {
                    var jd = my.json_decode(F1.my.b2s(my.file_get_contents(F1.my.getSystemKey("checkVersionURL"))));
                    remote_version = Convert.ToDouble(jd[0]["version"].ToString());
                    downloadPath = jd[0]["downloadPath"].ToString();
                    //Console.WriteLine("step 3");
                }
                catch
                {
                    //無網路
                }
            }
            if (local_version != remote_version || isNeedUpdate)
            {
                //Console.WriteLine("step 4");
                //版本不一樣，如果服務正在跑，就停止，下載，重新啟動程式
                F1.my.killProcess("system_status");
                if (downloadPath != "")
                {
                    //Console.WriteLine("step 5");
                    F1.my.file_put_contents(F1.PWD + "\\system_status.exe", F1.my.file_get_contents(downloadPath));
                    F1.logError("下載更新版本...：" + remote_version.ToString());
                }
                F1.my.system(F1.PWD + "\\system_status.exe");
            }
            if (!F1.my.isProcessRunning("system_status"))
            {
                //Console.WriteLine("step 6");
                F1.my.system(F1.PWD + "\\system_status.exe");
            }
        }
        static void Main(string[] args)
        {
            // Hide
            var handle = GetConsoleWindow();
            ShowWindow(handle, SW_HIDE);
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            F1 = new Program();
            F1.PWD = F1.my.pwd();
            string lockFile = F1.PWD + "\\lock_watchdog.txt";
            if (!F1.my.is_file(lockFile))
            {
                F1.my.file_put_contents(lockFile, "");
            }
            if (F1.my.is_file_lock(lockFile))
            {
                //如果目前已是 lock 就結束
                System.Environment.Exit(0);
                return;
            }
            F1.LOG_PATH = F1.PWD + "\\log";
            F1.create_log_dir();
            //lock file
            FileStream s2 = new FileStream(lockFile, FileMode.Open, FileAccess.Read, FileShare.None);
            //嘗試當掉就中斷離開
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(F1.myCrash);
            while (true)
            {
                F1.threads["mainThread"] = new Thread(F1.run_check_update);
                F1.threads["mainThread"].Start();
                Thread.Sleep(60 * 1000);
            }
        }
    }
}
