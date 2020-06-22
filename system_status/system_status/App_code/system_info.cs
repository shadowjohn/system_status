using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using WUApiLib;
namespace system_status.App_code
{
    class system_info
    {
        Form1 _form = null;
        public bool is_running = false;
        public void init(Form1 theform)
        {
            _form = theform;
            if (_form.threads.ContainsKey("system_info"))
            {
                _form.threads["system_info"].Abort();
                _form.threads["system_info"] = null;
            }
            
            is_running = true;
            _form.system_grid.AutoGenerateColumns = false; //這啥
            _form.system_grid.AllowUserToAddRows = false; //不能允許使用者自行調整
            _form.system_grid.RowHeadersVisible = false; //左邊空欄移除
            _form.system_grid.Dock = DockStyle.Fill; //自動展開到最大
            _form.system_grid.AllowDrop = false;
            //_form.system_grid.ReadOnly = true;

            _form.system_grid.Columns.Clear();
            string json_columns = @"
[
    {   
        ""systemID"":{""id"":""systemID"",""name"":""項次"",""width"":80,""display"":true,""headerAlign"":""center"",""cellAlign"":""center""}
    },            
    {   
        ""systemName"":{""id"":""systemName"",""name"":""功能名稱"",""width"":220,""display"":true,""headerAlign"":""center"",""cellAlign"":""left""}
    },
    {   
        ""systemData"":{""id"":""systemData"",""name"":""數值"",""width"":400,""display"":true,""headerAlign"":""center"",""cellAlign"":""left""}
    }
]";
            //表格初始化
            _form.my.grid_init(_form.system_grid, json_columns);
            //allow sorting
            foreach (DataGridViewColumn column in _form.system_grid.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.Automatic;
            }
            _form.threads["system_info"] = new Thread(() => run());
            _form.threads["system_info"].Start();
            //run();
        }
        private string _getFrameWorkVersion()
        {
            //取得 Framework 版本
            const string subkey = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\";

            using (var ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(subkey))
            {
                if (ndpKey != null && ndpKey.GetValue("Release") != null)
                {
                    //Console.WriteLine($".NET Framework Version: {_CheckFor45PlusVersion((int)ndpKey.GetValue("Release"))}");
                    return _CheckFor45PlusVersion((int)ndpKey.GetValue("Release"));
                }
                else
                {
                    Console.WriteLine(".NET Framework Version 4.5 or later is not detected.");
                    return "未知的版本或未安裝";
                }
            }
        }
        // Checking the version using >= enables forward compatibility.
        private string _CheckFor45PlusVersion(int releaseKey)
        {
            if (releaseKey >= 528040)
                return "4.8 or later";
            if (releaseKey >= 461808)
                return "4.7.2";
            if (releaseKey >= 461308)
                return "4.7.1";
            if (releaseKey >= 460798)
                return "4.7";
            if (releaseKey >= 394802)
                return "4.6.2";
            if (releaseKey >= 394254)
                return "4.6.1";
            if (releaseKey >= 393295)
                return "4.6";
            if (releaseKey >= 379893)
                return "4.5.2";
            if (releaseKey >= 378675)
                return "4.5.1";
            if (releaseKey >= 378389)
                return "4.5";
            // This code should never execute. A non-null release key should mean
            // that 4.5 or later is installed.
            return "No 4.5 or later version detected";
        }
        void run()
        {
            _form.UpdateUI_DataGridGrid(_form.system_grid, "clear", "", "", -1);
            int lastId = 0;
            //_form.system_grid.Rows.Add();
            _form.UpdateUI_DataGridGrid(_form.system_grid, "add", "", "", -1);


            lastId = _form.system_grid.Rows.Count - 1;
            _form.system_grid.Rows[lastId].Cells["systemID"].Value = (lastId + 1);
            _form.system_grid.Rows[lastId].Cells["systemName"].Value = "回報系統名稱";
            _form.system_grid.Rows[lastId].Cells["systemData"].Value = _form.textSystemName.Text.ToString();


            //From : https://stackoverflow.com/questions/4742389/get-pc-system-information-on-windows-machine
            ManagementObjectSearcher mos = new ManagementObjectSearcher("select * from Win32_OperatingSystem");
            foreach (ManagementObject managementObject in mos.Get())
            {
                //managementObject.C
                if (managementObject["Caption"] != null)
                {
                    //Console.WriteLine("Operating System Name  :  " + managementObject["Caption"].ToString());   //Display operating system caption
                    //_form.system_grid.Rows.Add();
                    _form.UpdateUI_DataGridGrid(_form.system_grid, "add", "", "", -1);
                    lastId = _form.system_grid.Rows.Count - 1;
                    _form.system_grid.Rows[lastId].Cells["systemID"].Value = (lastId + 1);
                    _form.system_grid.Rows[lastId].Cells["systemName"].Value = "作業系統";
                    _form.system_grid.Rows[lastId].Cells["systemData"].Value = managementObject["Caption"].ToString();
                }
                if (managementObject["OSArchitecture"] != null)
                {
                    //Console.WriteLine("Operating System Architecture  :  " + managementObject["OSArchitecture"].ToString());   //Display operating system architecture.
                    //_form.system_grid.Rows.Add();
                    _form.UpdateUI_DataGridGrid(_form.system_grid, "add", "", "", -1);
                    lastId = _form.system_grid.Rows.Count - 1;
                    _form.system_grid.Rows[lastId].Cells["systemID"].Value = (lastId + 1);
                    _form.system_grid.Rows[lastId].Cells["systemName"].Value = "位元";
                    _form.system_grid.Rows[lastId].Cells["systemData"].Value = managementObject["OSArchitecture"].ToString();
                }
                if (managementObject["CSDVersion"] != null)
                {
                    Console.WriteLine("Operating System Service Pack   :  " + managementObject["CSDVersion"].ToString());     //Display operating system version.
                }
            }
            Console.WriteLine("\n\nDisplaying Processor Name....");
            RegistryKey processor_name = Registry.LocalMachine.OpenSubKey(@"Hardware\Description\System\CentralProcessor\0", RegistryKeyPermissionCheck.ReadSubTree);   //This registry entry contains entry for processor info.

            if (processor_name != null)
            {
                if (processor_name.GetValue("ProcessorNameString") != null)
                {
                    //Console.WriteLine(processor_name.GetValue("ProcessorNameString"));   //Display processor ingo.
                    string CPU_NAME = processor_name.GetValue("ProcessorNameString").ToString();
                    CPU_NAME = _form.my.preg_replace(CPU_NAME, "[ ]{2,}", " ");
                    CPU_NAME = CPU_NAME.Trim();
                    //_form.system_grid.Rows.Add();
                    _form.UpdateUI_DataGridGrid(_form.system_grid, "add", "", "", -1);
                    lastId = _form.system_grid.Rows.Count - 1;
                    _form.system_grid.Rows[lastId].Cells["systemID"].Value = (lastId + 1);
                    _form.system_grid.Rows[lastId].Cells["systemName"].Value = "CPU規格";
                    _form.system_grid.Rows[lastId].Cells["systemData"].Value = CPU_NAME;
                }
            }



            //取網域名稱
            //https://docs.microsoft.com/en-us/windows/win32/cimwin32prov/win32-computersystem
            mos = new ManagementObjectSearcher(@"root\CIMV2", @"SELECT * FROM Win32_ComputerSystem");
            foreach (ManagementObject mo in mos.Get())
            {
                //CPU核心數
                //_form.system_grid.Rows.Add();
                _form.UpdateUI_DataGridGrid(_form.system_grid, "add", "", "", -1);
                lastId = _form.system_grid.Rows.Count - 1;
                _form.system_grid.Rows[lastId].Cells["systemID"].Value = (lastId + 1);
                _form.system_grid.Rows[lastId].Cells["systemName"].Value = "CPU核心數";
                _form.system_grid.Rows[lastId].Cells["systemData"].Value = mo["NumberOfLogicalProcessors"].ToString();


                //_form.system_grid.Rows.Add();
                _form.UpdateUI_DataGridGrid(_form.system_grid, "add", "", "", -1);
                lastId = _form.system_grid.Rows.Count - 1;
                _form.system_grid.Rows[lastId].Cells["systemID"].Value = (lastId + 1);
                _form.system_grid.Rows[lastId].Cells["systemName"].Value = "RAM大小";
                _form.system_grid.Rows[lastId].Cells["systemData"].Value = _form.my.size_hum_read(Convert.ToInt64(mo["TotalPhysicalMemory"]));


                //_form.system_grid.Rows.Add();
                _form.UpdateUI_DataGridGrid(_form.system_grid, "add", "", "", -1);
                lastId = _form.system_grid.Rows.Count - 1;
                _form.system_grid.Rows[lastId].Cells["systemID"].Value = (lastId + 1);
                _form.system_grid.Rows[lastId].Cells["systemName"].Value = "網域名稱";
                _form.system_grid.Rows[lastId].Cells["systemData"].Value = mo["Workgroup"].ToString();

                //_form.system_grid.Rows.Add();
                _form.UpdateUI_DataGridGrid(_form.system_grid, "add", "", "", -1);
                lastId = _form.system_grid.Rows.Count - 1;
                _form.system_grid.Rows[lastId].Cells["systemID"].Value = (lastId + 1);
                _form.system_grid.Rows[lastId].Cells["systemName"].Value = "使用者名稱";
                _form.system_grid.Rows[lastId].Cells["systemData"].Value = mo["UserName"].ToString();



            }

            var host = Dns.GetHostEntry(Dns.GetHostName());


            //_form.system_grid.Rows.Add();
            _form.UpdateUI_DataGridGrid(_form.system_grid, "add", "", "", -1);
            lastId = _form.system_grid.Rows.Count - 1;
            _form.system_grid.Rows[lastId].Cells["systemID"].Value = (lastId + 1);
            _form.system_grid.Rows[lastId].Cells["systemName"].Value = "電腦名稱";
            _form.system_grid.Rows[lastId].Cells["systemData"].Value = host.HostName.ToString();
            // 取本機 IP 
            // From : https://stackoverflow.com/questions/6803073/get-local-ip-address
            int ip_step = 1;
            foreach (var ip in host.AddressList)
            {

                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    //_form.system_grid.Rows.Add();
                    _form.UpdateUI_DataGridGrid(_form.system_grid, "add", "", "", -1);
                    lastId = _form.system_grid.Rows.Count - 1;
                    _form.system_grid.Rows[lastId].Cells["systemID"].Value = (lastId + 1);
                    _form.system_grid.Rows[lastId].Cells["systemName"].Value = "IP_" + ip_step.ToString();
                    _form.system_grid.Rows[lastId].Cells["systemData"].Value = ip.ToString();
                    ip_step++;
                }
            }
            var gateway_address = NetworkInterface.GetAllNetworkInterfaces()
        .Where(e => e.OperationalStatus == OperationalStatus.Up)
        .SelectMany(e => e.GetIPProperties().GatewayAddresses)
        .FirstOrDefault();
            if (gateway_address != null)
            {
                //_form.system_grid.Rows.Add();
                _form.UpdateUI_DataGridGrid(_form.system_grid, "add", "", "", -1);
                lastId = _form.system_grid.Rows.Count - 1;
                _form.system_grid.Rows[lastId].Cells["systemID"].Value = (lastId + 1);
                _form.system_grid.Rows[lastId].Cells["systemName"].Value = "Gateway";
                _form.system_grid.Rows[lastId].Cells["systemData"].Value = gateway_address.Address.ToString();
            }

            //Framework 版本
            //_form.system_grid.Rows.Add();
            _form.UpdateUI_DataGridGrid(_form.system_grid, "add", "", "", -1);
            lastId = _form.system_grid.Rows.Count - 1;
            _form.system_grid.Rows[lastId].Cells["systemID"].Value = (lastId + 1);
            _form.system_grid.Rows[lastId].Cells["systemName"].Value = "Framework版本";
            _form.system_grid.Rows[lastId].Cells["systemData"].Value = _getFrameWorkVersion();


            //取系統最後更新時間
            //From : https://stackoverflow.com/questions/9215326/check-when-last-check-for-windows-updates-was-performed
            AutomaticUpdates auc = new AutomaticUpdates();
            //Console.WriteLine(auc.Results.LastInstallationSuccessDate);
            if (auc.Results.LastInstallationSuccessDate is DateTime)
            {
                DateTime dt = new DateTime(((DateTime)auc.Results.LastInstallationSuccessDate).Ticks, DateTimeKind.Utc);
                string strDt = _form.my.date("Y-m-d H:i:s", (Convert.ToInt32(_form.my.strtotime(dt.ToString("yyyy-MM-dd HH:mm:ss"))) + 8 * 60 * 60).ToString());
                //_form.system_grid.Rows.Add();
                _form.UpdateUI_DataGridGrid(_form.system_grid, "add", "", "", -1);
                lastId = _form.system_grid.Rows.Count - 1;
                _form.system_grid.Rows[lastId].Cells["systemID"].Value = (lastId + 1);
                _form.system_grid.Rows[lastId].Cells["systemName"].Value = "WindowsUpdateDate";
                _form.system_grid.Rows[lastId].Cells["systemData"].Value = strDt;
            }




            _form.setStatusBar("就緒", 0);
            is_running = false;
        }
    }
}
