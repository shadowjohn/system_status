using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace system_status.App_code
{
    internal class installed_software
    {
        private Form1 _form = null;
        public bool is_running = false;
        public string last_date = "";
        public DataTable dt = new DataTable();
        private bool isGridInit = false;

        public void init(Form1 theform)
        {
            _form = theform;
            if (_form.threads.ContainsKey("installed_software"))
            {
                _form.threads["installed_software"].Abort();
                _form.threads["installed_software"] = null;
            }

            is_running = true;
            last_date = _form.my.time();
            _form.installed_software_grid.AutoGenerateColumns = false; //這啥
            _form.installed_software_grid.AllowUserToAddRows = false; //不能允許使用者自行調整
            _form.installed_software_grid.RowHeadersVisible = false; //左邊空欄移除
            _form.installed_software_grid.Dock = DockStyle.Fill; //自動展開到最大
            _form.installed_software_grid.AllowDrop = false;
            //_form.installed_software_grid.ReadOnly = true;

            // _form.installed_software_grid.Columns.Clear();
            string json_columns = @"
[
    {
        ""installed_software_id"":{""id"":""installed_software_id"",""name"":""項次"",""width"":80,""display"":true,""headerAlign"":""center"",""cellAlign"":""center""}
    },
    {
        ""installed_software_DisplayName"":{""id"":""installed_software_DisplayName"",""name"":""程式名稱"",""width"":380,""display"":true,""headerAlign"":""center"",""cellAlign"":""left""}
    },
    {
        ""installed_software_OSBit"":{""id"":""installed_software_OSBit"",""name"":""位元"",""width"":120,""display"":true,""headerAlign"":""center"",""cellAlign"":""center""}
    },
    {
        ""installed_software_InstallDate"":{""id"":""installed_software_InstallDate"",""name"":""安裝日期"",""width"":250,""display"":true,""headerAlign"":""center"",""cellAlign"":""center""}
    },
    {
        ""installed_software_DisplayVersion"":{""id"":""installed_software_DisplayVersion"",""name"":""版本"",""width"":150,""display"":true,""headerAlign"":""center"",""cellAlign"":""center""}
    },
    {
        ""installed_software_Publisher"":{""id"":""installed_software_Publisher"",""name"":""Publisher"",""width"":380,""display"":true,""headerAlign"":""center"",""cellAlign"":""center""}
    },
    {
        ""installed_software_InstallLocation"":{""id"":""installed_software_InstallLocation"",""name"":""安裝位置"",""width"":380,""display"":true,""headerAlign"":""center"",""cellAlign"":""left""}
    }
]
";
            //表格初始化
            if (isGridInit == false)
            {
                _form.my.grid_init(_form.installed_software_grid, json_columns);
                isGridInit = true;
            }
            dt = _form.my.datatable_init(json_columns);

            //allow sorting
            foreach (DataGridViewColumn column in _form.installed_software_grid.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.Automatic;
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            }
            _form.threads["installed_software_grid"] = new Thread(() => run());
            _form.threads["installed_software_grid"].Start();
        }
        private List<Dictionary<string, string>> ReadRegistryUninstall(RegistryView view)
        {
            //view = RegistryView.Registry32
            //view = RegistryView.Registry64
            List<Dictionary<string, string>> output = new List<Dictionary<string, string>>();
            const string REGISTRY_KEY = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            var baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, view);
            var subKey = baseKey.OpenSubKey(REGISTRY_KEY);
            foreach (string subkey_name in subKey.GetSubKeyNames())
            {
                RegistryKey key = subKey.OpenSubKey(subkey_name);
                if (!string.IsNullOrEmpty(key.GetValue("DisplayName") as string))
                {
                    Dictionary<string, string> d = new Dictionary<string, string>();
                    if (view == RegistryView.Registry32)
                    {
                        d["OSBit"] = "x86";
                    }
                    else
                    {
                        d["OSBit"] = "x64";
                    }
                    d["DisplayName"] = key.GetValue("DisplayName").ToString();
                    d["DisplayVersion"] = "";
                    try
                    {
                        d["DisplayVersion"] = key.GetValue("DisplayVersion").ToString();
                    }
                    catch { }
                    d["Publisher"] = "";
                    try
                    {
                        d["Publisher"] = key.GetValue("Publisher").ToString();
                    }
                    catch { }
                    d["InstallDate"] = "";
                    try
                    {
                        d["InstallDate"] = key.GetValue("InstallDate").ToString();
                    }
                    catch { }
                    d["InstallLocation"] = "";
                    try
                    {
                        d["InstallLocation"] = key.GetValue("InstallLocation").ToString();
                    }
                    catch { }
                    output.Add(d);
                }
            }
            subKey.Close();
            baseKey.Close();
            return output;
        }
        private void run()
        {
            try
            {
                DataRow row = dt.NewRow();

                List<Dictionary<string, string>> x86Programs = ReadRegistryUninstall(RegistryView.Registry32);
                List<Dictionary<string, string>> x64Programs = ReadRegistryUninstall(RegistryView.Registry64);
                int step = 1;
                for (int i = 0, max_i = x86Programs.Count; i < max_i; i++)
                {
                    row = dt.NewRow();
                    row["installed_software_id"] = step.ToString();
                    row["installed_software_DisplayName"] = x86Programs[i]["DisplayName"];
                    row["installed_software_OSBit"] = x86Programs[i]["OSBit"];
                    row["installed_software_DisplayVersion"] = x86Programs[i]["DisplayVersion"];
                    row["installed_software_Publisher"] = x86Programs[i]["Publisher"];
                    row["installed_software_InstallDate"] = x86Programs[i]["InstallDate"];
                    row["installed_software_InstallLocation"] = x86Programs[i]["InstallLocation"];
                    dt.Rows.Add(row);
                    step++;

                }
                for (int i = 0, max_i = x64Programs.Count; i < max_i; i++)
                {
                    row = dt.NewRow();
                    row["installed_software_id"] = step.ToString();
                    row["installed_software_DisplayName"] = x64Programs[i]["DisplayName"];
                    row["installed_software_OSBit"] = x64Programs[i]["OSBit"];
                    row["installed_software_DisplayVersion"] = x64Programs[i]["DisplayVersion"];
                    row["installed_software_Publisher"] = x64Programs[i]["Publisher"];
                    row["installed_software_InstallDate"] = x64Programs[i]["InstallDate"];
                    row["installed_software_InstallLocation"] = x64Programs[i]["InstallLocation"];
                    dt.Rows.Add(row);
                    step++;
                }


                _form.updateDGVUI(_form.installed_software_grid, dt);
                //_form.my.file_put_contents(_form.my.pwd() + "\\log\\installed_software_log.txt", _form.my.json_encode(dt));
                _form.setStatusBar("就緒", 0);
                is_running = false;
            }
            catch (Exception ex)
            {
                _form.logError("installed_software_grid Error...：\r\n" + ex.Message + "\r\n" + ex.StackTrace);
                is_running = false;
            }
        }
    }
}

