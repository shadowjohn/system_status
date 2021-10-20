using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using WUApiLib;
namespace system_status.App_code
{

    class system_info
    {
        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        #region Obtain memory information API    
        public static extern bool GlobalMemoryStatusEx(ref MEMORY_INFO mi);

        //Define the information structure of memory
        [StructLayout(LayoutKind.Sequential)]
        public struct MEMORY_INFO
        {
            public uint dwLength; //Current structure size
            public uint dwMemoryLoad; //Current memory utilization
            public ulong ullTotalPhys; //Total physical memory size
            public ulong ullAvailPhys; //Available physical memory size
            public ulong ullTotalPageFile; //Total Exchange File Size
            public ulong ullAvailPageFile; //Total Exchange File Size
            public ulong ullTotalVirtual; //Total virtual memory size
            public ulong ullAvailVirtual; //Available virtual memory size
            public ulong ullAvailExtendedVirtual; //Keep this value always zero
        }
        #endregion
        Form1 _form = null;
        public bool is_running = false;
        public string last_date = "";
        private DataTable dt = new DataTable();
        private bool isGridInit = false;
        private MEMORY_INFO GetMemoryStatus()
        {
            MEMORY_INFO mi = new MEMORY_INFO();
            mi.dwLength = (uint)System.Runtime.InteropServices.Marshal.SizeOf(mi);
            GlobalMemoryStatusEx(ref mi);
            return mi;
        }
        private ulong GetUsedMemory()
        {
            //取得已用掉的 ram
            MEMORY_INFO mi = GetMemoryStatus();
            return (mi.ullTotalPhys - mi.ullAvailPhys);
        }
        public void init(Form1 theform)
        {
            _form = theform;
            if (_form.threads.ContainsKey("system_info"))
            {
                _form.threads["system_info"].Abort();
                _form.threads["system_info"] = null;
            }

            is_running = true;
            last_date = _form.my.time();
            _form.system_grid.AutoGenerateColumns = false; //這啥
            _form.system_grid.AllowUserToAddRows = false; //不能允許使用者自行調整
            _form.system_grid.RowHeadersVisible = false; //左邊空欄移除
            _form.system_grid.Dock = DockStyle.Fill; //自動展開到最大
            _form.system_grid.AllowDrop = false;
            //_form.system_grid.ReadOnly = true;

            //_form.system_grid.Columns.Clear();
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
            if (isGridInit == false)
            {
                _form.my.grid_init(_form.system_grid, json_columns);
                isGridInit = true;
            }
            dt = _form.my.datatable_init(json_columns);

            //allow sorting
            foreach (DataGridViewColumn column in _form.system_grid.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.Automatic;
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
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
        Dictionary<string, string> getUsedCPU_RAM()
        {
            //cpu loading
            PerformanceCounter cpuCounter;
            PerformanceCounter ramCounter;

            cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total", Environment.MachineName);
            cpuCounter.NextValue();
            System.Threading.Thread.Sleep(1000); //This avoid that answer always 0                
            ramCounter = new PerformanceCounter("Memory", "Available MBytes");
            Dictionary<string, string> output = new Dictionary<string, string>();
            output["CPU"] = cpuCounter.NextValue() + " %";
            output["RAM"] = ramCounter.NextValue() + " MB";
            return output;
        }
        void run()
        {
            int step = 0;
            //_form.UpdateUI_DataGridGrid(_form.system_grid, "clear", "", "", -1);
            //int lastId = 0;
            //_form.system_grid.Rows.Add();
            /*
            _form.UpdateUI_DataGridGrid(_form.system_grid, "add", "", "", -1);
            lastId = _form.system_grid.Rows.Count - 1;
            _form.system_grid.Rows[lastId].Cells["systemID"].Value = (lastId + 1);
            _form.system_grid.Rows[lastId].Cells["systemName"].Value = "回報系統名稱";
            _form.system_grid.Rows[lastId].Cells["systemData"].Value = _form.textSystemName.Text.ToString();
            */

            DataRow row = dt.NewRow();
            row["systemID"] = ++step;
            row["systemName"] = "回報系統名稱";
            row["systemData"] = _form.textSystemName.Text.ToString();
            dt.Rows.Add(row);



            //From : https://stackoverflow.com/questions/4742389/get-pc-system-information-on-windows-machine
            ManagementObjectSearcher mos = new ManagementObjectSearcher("select * from Win32_OperatingSystem");
            foreach (ManagementObject managementObject in mos.Get())
            {
                //managementObject.C
                if (managementObject["Caption"] != null)
                {
                    //Console.WriteLine("Operating System Name  :  " + managementObject["Caption"].ToString());   //Display operating system caption
                    //_form.system_grid.Rows.Add();
                    /*
                    _form.UpdateUI_DataGridGrid(_form.system_grid, "add", "", "", -1);
                    lastId = _form.system_grid.Rows.Count - 1;
                    _form.system_grid.Rows[lastId].Cells["systemID"].Value = (lastId + 1);
                    _form.system_grid.Rows[lastId].Cells["systemName"].Value = "作業系統";
                    _form.system_grid.Rows[lastId].Cells["systemData"].Value = managementObject["Caption"].ToString();
                    */

                    row = dt.NewRow();
                    row["systemID"] = ++step;
                    row["systemName"] = "作業系統";
                    row["systemData"] = managementObject["Caption"].ToString();
                    dt.Rows.Add(row);

                }
                if (managementObject["OSArchitecture"] != null)
                {
                    //Console.WriteLine("Operating System Architecture  :  " + managementObject["OSArchitecture"].ToString());   //Display operating system architecture.
                    //_form.system_grid.Rows.Add();
                    /*
                    _form.UpdateUI_DataGridGrid(_form.system_grid, "add", "", "", -1);
                    lastId = _form.system_grid.Rows.Count - 1;
                    _form.system_grid.Rows[lastId].Cells["systemID"].Value = (lastId + 1);
                    _form.system_grid.Rows[lastId].Cells["systemName"].Value = "位元";
                    _form.system_grid.Rows[lastId].Cells["systemData"].Value = managementObject["OSArchitecture"].ToString();
                    */

                    row = dt.NewRow();
                    row["systemID"] = ++step;
                    row["systemName"] = "位元";
                    row["systemData"] = managementObject["OSArchitecture"].ToString();
                    dt.Rows.Add(row);
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
                    /*
                    _form.UpdateUI_DataGridGrid(_form.system_grid, "add", "", "", -1);
                    lastId = _form.system_grid.Rows.Count - 1;
                    _form.system_grid.Rows[lastId].Cells["systemID"].Value = (lastId + 1);
                    _form.system_grid.Rows[lastId].Cells["systemName"].Value = "CPU規格";
                    _form.system_grid.Rows[lastId].Cells["systemData"].Value = CPU_NAME;
                    */

                    row = dt.NewRow();
                    row["systemID"] = ++step;
                    row["systemName"] = "CPU規格";
                    row["systemData"] = CPU_NAME;
                    dt.Rows.Add(row);
                }
            }



            //cpu id
            string cpu_id = _form.my.getCPUId();
            /*
            _form.UpdateUI_DataGridGrid(_form.system_grid, "add", "", "", -1);
            lastId = _form.system_grid.Rows.Count - 1;
            _form.system_grid.Rows[lastId].Cells["systemID"].Value = (lastId + 1);
            _form.system_grid.Rows[lastId].Cells["systemName"].Value = "CPUID";            
            _form.system_grid.Rows[lastId].Cells["systemData"].Value = cpu_id;
            */

            row = dt.NewRow();
            row["systemID"] = ++step;
            row["systemName"] = "CPUID";
            row["systemData"] = cpu_id;
            dt.Rows.Add(row);

            //https://docs.microsoft.com/en-us/windows/win32/cimwin32prov/win32-computersystem
            mos = new ManagementObjectSearcher(@"root\CIMV2", @"SELECT * FROM Win32_ComputerSystem");
            foreach (ManagementObject mo in mos.Get())
            {
                //CPU核心數
                //_form.system_grid.Rows.Add();
                /*
                _form.UpdateUI_DataGridGrid(_form.system_grid, "add", "", "", -1);
                lastId = _form.system_grid.Rows.Count - 1;
                _form.system_grid.Rows[lastId].Cells["systemID"].Value = (lastId + 1);
                _form.system_grid.Rows[lastId].Cells["systemName"].Value = "CPU核心數";
                _form.system_grid.Rows[lastId].Cells["systemData"].Value = mo["NumberOfLogicalProcessors"].ToString();
                */

                row = dt.NewRow();
                row["systemID"] = ++step;
                row["systemName"] = "CPU核心數";
                row["systemData"] = mo["NumberOfLogicalProcessors"].ToString();
                dt.Rows.Add(row);

                /*
                _form.UpdateUI_DataGridGrid(_form.system_grid, "add", "", "", -1);
                lastId = _form.system_grid.Rows.Count - 1;
                _form.system_grid.Rows[lastId].Cells["systemID"].Value = (lastId + 1);
                _form.system_grid.Rows[lastId].Cells["systemName"].Value = "RAM大小";
                _form.system_grid.Rows[lastId].Cells["systemData"].Value = mo["TotalPhysicalMemory"].ToString();
                */

                row = dt.NewRow();
                row["systemID"] = ++step;
                row["systemName"] = "RAM大小";
                row["systemData"] = mo["TotalPhysicalMemory"].ToString();
                dt.Rows.Add(row);

                /*
                _form.UpdateUI_DataGridGrid(_form.system_grid, "add", "", "", -1);
                lastId = _form.system_grid.Rows.Count - 1;
                _form.system_grid.Rows[lastId].Cells["systemID"].Value = (lastId + 1);
                _form.system_grid.Rows[lastId].Cells["systemName"].Value = "網域名稱";                
                _form.system_grid.Rows[lastId].Cells["systemData"].Value = mo["Workgroup"] == null ? "" : mo["Workgroup"].ToString();
                */


                row = dt.NewRow();
                row["systemID"] = ++step;
                row["systemName"] = "網域名稱";
                row["systemData"] = mo["Workgroup"] == null ? "" : mo["Workgroup"].ToString();
                dt.Rows.Add(row);

                /*
                _form.UpdateUI_DataGridGrid(_form.system_grid, "add", "", "", -1);
                lastId = _form.system_grid.Rows.Count - 1;
                _form.system_grid.Rows[lastId].Cells["systemID"].Value = (lastId + 1);
                _form.system_grid.Rows[lastId].Cells["systemName"].Value = "使用者名稱";
                _form.system_grid.Rows[lastId].Cells["systemData"].Value = mo["UserName"].ToString();
                */

                row = dt.NewRow();
                row["systemID"] = ++step;
                row["systemName"] = "使用者名稱";
                row["systemData"] = mo["UserName"].ToString();
                dt.Rows.Add(row);

            }

            var host = Dns.GetHostEntry(Dns.GetHostName());


            //_form.system_grid.Rows.Add();
            /*
            _form.UpdateUI_DataGridGrid(_form.system_grid, "add", "", "", -1);
            lastId = _form.system_grid.Rows.Count - 1;
            _form.system_grid.Rows[lastId].Cells["systemID"].Value = (lastId + 1);
            _form.system_grid.Rows[lastId].Cells["systemName"].Value = "電腦名稱";
            _form.system_grid.Rows[lastId].Cells["systemData"].Value = host.HostName.ToString();
            */

            row = dt.NewRow();
            row["systemID"] = ++step;
            row["systemName"] = "電腦名稱";
            row["systemData"] = host.HostName.ToString();
            dt.Rows.Add(row);
            // 取本機 IP 
            // From : https://stackoverflow.com/questions/6803073/get-local-ip-address
            int ip_step = 1;
            foreach (var ip in host.AddressList)
            {

                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    /*
                    _form.UpdateUI_DataGridGrid(_form.system_grid, "add", "", "", -1);
                    lastId = _form.system_grid.Rows.Count - 1;
                    _form.system_grid.Rows[lastId].Cells["systemID"].Value = (lastId + 1);
                    _form.system_grid.Rows[lastId].Cells["systemName"].Value = "IP_" + ip_step.ToString();
                    _form.system_grid.Rows[lastId].Cells["systemData"].Value = ip.ToString();
                    */

                    row = dt.NewRow();
                    row["systemID"] = ++step;
                    row["systemName"] = "IP_" + ip_step.ToString();
                    row["systemData"] = ip.ToString();
                    dt.Rows.Add(row);

                    ip_step++;


                }
            }
            var gateway_address = NetworkInterface.GetAllNetworkInterfaces()
        .Where(e => e.OperationalStatus == OperationalStatus.Up)
        .SelectMany(e => e.GetIPProperties().GatewayAddresses)
        .FirstOrDefault();
            if (gateway_address != null)
            {
                /*
                _form.UpdateUI_DataGridGrid(_form.system_grid, "add", "", "", -1);
                lastId = _form.system_grid.Rows.Count - 1;
                _form.system_grid.Rows[lastId].Cells["systemID"].Value = (lastId + 1);
                _form.system_grid.Rows[lastId].Cells["systemName"].Value = "Gateway";
                _form.system_grid.Rows[lastId].Cells["systemData"].Value = gateway_address.Address.ToString();
                */

                row = dt.NewRow();
                row["systemID"] = ++step;
                row["systemName"] = "Gateway";
                row["systemData"] = gateway_address.Address.ToString();
                dt.Rows.Add(row);
            }

            //Framework 版本
            //_form.system_grid.Rows.Add();

            string framework_version = _getFrameWorkVersion();
            /*
            _form.UpdateUI_DataGridGrid(_form.system_grid, "add", "", "", -1);
            lastId = _form.system_grid.Rows.Count - 1;            
            _form.system_grid.Rows[lastId].Cells["systemID"].Value = (lastId + 1);
            _form.system_grid.Rows[lastId].Cells["systemName"].Value = "Framework版本";
            _form.system_grid.Rows[lastId].Cells["systemData"].Value = framework_version;
            */

            row = dt.NewRow();
            row["systemID"] = ++step;
            row["systemName"] = "Framework版本";
            row["systemData"] = framework_version;
            dt.Rows.Add(row);

            //取系統最後更新時間
            //From : https://stackoverflow.com/questions/9215326/check-when-last-check-for-windows-updates-was-performed
            AutomaticUpdates auc = new AutomaticUpdates();
            //Console.WriteLine(auc.Results.LastInstallationSuccessDate);
            if (auc.Results.LastInstallationSuccessDate is DateTime)
            {
                DateTime _dt = new DateTime(((DateTime)auc.Results.LastInstallationSuccessDate).Ticks, DateTimeKind.Utc);
                string strDt = _form.my.date("Y-m-d H:i:s", (Convert.ToInt32(_form.my.strtotime(_dt.ToString("yyyy-MM-dd HH:mm:ss"))) + 8 * 60 * 60).ToString());
                //_form.system_grid.Rows.Add();
                /*
                _form.UpdateUI_DataGridGrid(_form.system_grid, "add", "", "", -1);
                lastId = _form.system_grid.Rows.Count - 1;
                _form.system_grid.Rows[lastId].Cells["systemID"].Value = (lastId + 1);
                _form.system_grid.Rows[lastId].Cells["systemName"].Value = "WindowsUpdateDate";
                _form.system_grid.Rows[lastId].Cells["systemData"].Value = strDt;
                */

            row = dt.NewRow();
                row["systemID"] = ++step;
                row["systemName"] = "WindowsUpdateDate";
                row["systemData"] = strDt;
                dt.Rows.Add(row);
            }

            var CPU_RAM = getUsedCPU_RAM();
            /*
            _form.UpdateUI_DataGridGrid(_form.system_grid, "add", "", "", -1);
            lastId = _form.system_grid.Rows.Count - 1;
            _form.system_grid.Rows[lastId].Cells["systemID"].Value = (lastId + 1);
            _form.system_grid.Rows[lastId].Cells["systemName"].Value = "UsedRam";
            _form.system_grid.Rows[lastId].Cells["systemData"].Value = CPU_RAM["RAM"];
            */

            row = dt.NewRow();
            row["systemID"] = ++step;
            row["systemName"] = "UsedRam";
            row["systemData"] = CPU_RAM["RAM"];
            dt.Rows.Add(row);

            /*
            _form.UpdateUI_DataGridGrid(_form.system_grid, "add", "", "", -1);
            lastId = _form.system_grid.Rows.Count - 1;
            _form.system_grid.Rows[lastId].Cells["systemID"].Value = (lastId + 1);
            _form.system_grid.Rows[lastId].Cells["systemName"].Value = "UsedCPU";
            _form.system_grid.Rows[lastId].Cells["systemData"].Value = CPU_RAM["CPU"];
            */

            row = dt.NewRow();
            row["systemID"] = ++step;
            row["systemName"] = "UsedCPU";
            row["systemData"] = CPU_RAM["CPU"];
            dt.Rows.Add(row);

            //已用的 ram
            /*
            _form.UpdateUI_DataGridGrid(_form.system_grid, "add", "", "", -1);
            lastId = _form.system_grid.Rows.Count - 1;
            _form.system_grid.Rows[lastId].Cells["systemID"].Value = (lastId + 1);
            _form.system_grid.Rows[lastId].Cells["systemName"].Value = "UsedRam";
            _form.system_grid.Rows[lastId].Cells["systemData"].Value = GetUsedMemory().ToString();
            */

            //ping
            string CMD_PING = "ping 8.8.8.8 -n 1 -w 1";
            string CMD_PING_TMP = _form.my.system(CMD_PING);
            if (_form.my.is_string_like(CMD_PING_TMP, "要求等候逾時"))
            {
                row = dt.NewRow();
                row["systemID"] = ++step;
                row["systemName"] = "ping8888";
                row["systemData"] = "timeout";
                dt.Rows.Add(row);
            }
            else
            {
                row = dt.NewRow();
                row["systemID"] = ++step;
                row["systemName"] = "ping8888";
                /*
Ping 8.8.8.8 (使用 32 位元組的資料):
回覆自 8.8.8.8: 位元組=32 時間=4ms TTL=116

8.8.8.8 的 Ping 統計資料:
    封包: 已傳送 = 1，已收到 = 1, 已遺失 = 0 (0% 遺失)，
大約的來回時間 (毫秒):
    最小值 = 4ms，最大值 = 4ms，平均 = 4ms
                */
                row["systemData"] = _form.my.get_between(CMD_PING_TMP, "時間=", "ms");
                dt.Rows.Add(row);
            }

            // windows defender
            // From : https://github.com/MicrosoftDocs/windows-itpro-docs/issues/6092
            string CMD = "echo ######## && powershell.exe -NoLogo -NoProfile -NonInteractive -Command \"'AMProductVersion: ' + $((Get-MpComputerStatus).AMProductVersion) ; 'AMEngineVersion: ' + $((Get-MpComputerStatus).AMEngineVersion) ; 'AntispywareSignatureVersion: ' + $((Get-MpComputerStatus).AntispywareSignatureVersion) ; 'AntivirusSignatureVersion: ' + $((Get-MpComputerStatus).AntivirusSignatureVersion)\"";
            string tmp = _form.my.system(CMD);
            var mtmp = _form.my.explode("########", tmp);
            string data = mtmp[mtmp.Count() - 1].Trim();
            mtmp = _form.my.explode("\n", data);
            for (int i = 0, max_i = mtmp.Length; i < max_i; i++)
            {
                var d = _form.my.explode(":", mtmp[i]);
                if (d.Count() != 2) continue;
                /*
                _form.UpdateUI_DataGridGrid(_form.system_grid, "add", "", "", -1);
                lastId = _form.system_grid.Rows.Count - 1;
                _form.system_grid.Rows[lastId].Cells["systemID"].Value = (lastId + 1);
                _form.system_grid.Rows[lastId].Cells["systemName"].Value = "WindowDefender_" + d[0].Trim();
                _form.system_grid.Rows[lastId].Cells["systemData"].Value = d[1].Trim();
                */

                row = dt.NewRow();
                row["systemID"] = ++step;
                row["systemName"] = "WindowDefender_" + d[0].Trim();
                row["systemData"] = d[1].Trim();
                dt.Rows.Add(row);
            }

            //版本
            /*
            _form.UpdateUI_DataGridGrid(_form.system_grid, "add", "", "", -1);
            lastId = _form.system_grid.Rows.Count - 1;
            _form.system_grid.Rows[lastId].Cells["systemID"].Value = (lastId + 1);
            _form.system_grid.Rows[lastId].Cells["systemName"].Value = "TOOL_VERSION";
            _form.system_grid.Rows[lastId].Cells["systemData"].Value = _form.VERSION;
            */

            row = dt.NewRow();
            row["systemID"] = ++step;
            row["systemName"] = "TOOL_VERSION";
            row["systemData"] = _form.VERSION;
            dt.Rows.Add(row);

            _form.updateDGVUI(_form.system_grid, dt);

            _form.setStatusBar("就緒", 0);
            is_running = false;
        }
    }
}
