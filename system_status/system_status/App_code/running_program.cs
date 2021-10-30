using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace system_status.App_code
{

    class running_program
    {
        Form1 _form = null;
        public bool is_running = false;
        public string last_date = "";
        public DataTable dt = new DataTable();
        private bool isGridInit = false;
        public void init(Form1 theform)
        {
            _form = theform;
            if (_form.threads.ContainsKey("running_program"))
            {
                _form.threads["running_program"].Abort();
                _form.threads["running_program"] = null;
            }
            is_running = true;
            last_date = _form.my.time();
            theform.setStatusBar("工作管理員資訊載入開始...", 0);
            //From : http://jengting.blogspot.com/2016/07/DataGridView-Sample.html
            theform.running_program_grid.AutoGenerateColumns = false; //這啥
            theform.running_program_grid.AllowUserToAddRows = false; //不能允許使用者自行調整
            theform.running_program_grid.RowHeadersVisible = false; //左邊空欄移除
            theform.running_program_grid.Dock = DockStyle.Fill; //自動展開到最大
            theform.running_program_grid.AllowDrop = false;
            theform.running_program_grid.ReadOnly = true;

            //theform.running_program_grid.Columns.Clear();
            is_running = false;
            string json_columns = @"
[
    {   
        ""running_programID"":{""id"":""running_programID"",""name"":""項次"",""width"":80,""display"":true,""headerAlign"":""center"",""cellAlign"":""center""}
    },
{   
        ""running_programPID"":{""id"":""running_programPID"",""name"":""PID"",""width"":80,""display"":true,""headerAlign"":""center"",""cellAlign"":""center""}
    },
    {   
        ""running_programName"":{""id"":""running_programName"",""name"":""工作名稱"",""width"":200,""display"":true,""headerAlign"":""center"",""cellAlign"":""left""}
    },
    {   
        ""running_programIsDanger"":{""id"":""running_programIsDanger"",""name"":""是否可疑"",""width"":80,""display"":true,""headerAlign"":""center"",""cellAlign"":""left""}
    },
    {   
        ""running_programBaseName"":{""id"":""running_programBaseName"",""name"":""執行檔名"",""width"":200,""display"":true,""headerAlign"":""center"",""cellAlign"":""left""}
    },
    {   
        ""running_programPath"":{""id"":""running_programPath"",""name"":""檔案位置"",""width"":450,""display"":true,""headerAlign"":""center"",""cellAlign"":""left""}
    },
    {   
        ""running_programCommandLine"":{""id"":""running_programCommandLine"",""name"":""執行方法"",""width"":550,""display"":true,""headerAlign"":""center"",""cellAlign"":""left""}
    },
    {   
        ""running_programStart_datetime"":{""id"":""running_programStart_datetime"",""name"":""開始時間"",""width"":150,""display"":true,""headerAlign"":""center"",""cellAlign"":""center""}
    },
    {   
        ""running_programRun_times"":{""id"":""running_programRun_times"",""name"":""已跑多久"",""width"":120,""display"":true,""headerAlign"":""center"",""cellAlign"":""center""}
    },
    {   
        ""running_programRun_user"":{""id"":""running_programRun_user"",""name"":""執行身分"",""width"":550,""display"":true,""headerAlign"":""center"",""cellAlign"":""center""}
    }
]
";
            //grid_init(json_columns);
            //表格初始化
            if (isGridInit == false)
            {
                _form.my.grid_init(_form.running_program_grid, json_columns);
                isGridInit = true;
            }
            dt = _form.my.datatable_init(json_columns);

            //allow sorting
            foreach (DataGridViewColumn column in _form.running_program_grid.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.Automatic;
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            }
            _form.threads["running_program"] = new Thread(() => run());
            _form.threads["running_program"].Start();
            //run();
        }

        void run()
        {
            DataRow row = dt.NewRow();
            
            //_form.running_program_grid.Rows.Clear();
            //_form.UpdateUI_DataGridGrid(_form.running_program_grid, "clear", "", "", -1);
            is_running = true;
            Process[] processlist = Process.GetProcesses();
            int step = 0;
            int total_step = processlist.Count();
            GetProcessInfo();
            foreach (Process theprocess in processlist)
            {
                row = dt.NewRow();
                _form.setStatusBar("執行緒資訊載入中...", Convert.ToInt32((Convert.ToDouble(step) / Convert.ToDouble(total_step)) * 100.0));
                step++;
                //_form.running_program_grid.Rows.Add();
                //_form.UpdateUI_DataGridGrid(_form.running_program_grid, "add", "", "", -1);
                //int lastId = _form.running_program_grid.Rows.Count - 1;
                int pid = theprocess.Id;
                Dictionary<string, string> o = GetProcessPath(pid);
                /*_form.running_program_grid.Rows[lastId].Cells["running_programID"].Value = step.ToString();
                _form.running_program_grid.Rows[lastId].Cells["running_programPID"].Value = pid.ToString();
                _form.running_program_grid.Rows[lastId].Cells["running_programName"].Value = theprocess.ProcessName;
                _form.running_program_grid.Rows[lastId].Cells["running_programBaseName"].Value = _form.my.basename(o["fullPath"]);
                _form.running_program_grid.Rows[lastId].Cells["running_programPath"].Value = o["fullPath"];
                _form.running_program_grid.Rows[lastId].Cells["running_programCommandLine"].Value = o["CommandLine"];

                _form.running_program_grid.Rows[lastId].Cells["running_programStart_datetime"].Value = o["CreationDate"];
                _form.running_program_grid.Rows[lastId].Cells["running_programRun_times"].Value = o["ExcuteTimes"];
                _form.running_program_grid.Rows[lastId].Cells["running_programRun_user"].Value = GetProcessOwner(pid);
                */
                row["running_programID"] = ++step;
                row["running_programPID"] = pid.ToString();
                row["running_programName"] = theprocess.ProcessName;
                row["running_programBaseName"] = _form.my.basename(o["fullPath"]);
                row["running_programPath"] = o["fullPath"];
                row["running_programCommandLine"] = o["CommandLine"];
                row["running_programStart_datetime"] = o["CreationDate"];
                row["running_programRun_times"] = o["ExcuteTimes"];
                row["running_programRun_user"] = GetProcessOwner(pid);

                dt.Rows.Add(row);
            }
            _form.updateDGVUI(_form.running_program_grid, dt);
            _form.setStatusBar("執行緒資訊載入完成...", 100);
            is_running = false;
        }
        public string GetProcessOwner(int processId)
        {
            string query = "Select * From Win32_Process Where ProcessID = " + processId;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            ManagementObjectCollection processList = searcher.Get();

            foreach (ManagementObject obj in processList)
            {
                string[] argList = new string[] { string.Empty, string.Empty };
                int returnVal = Convert.ToInt32(obj.InvokeMethod("GetOwner", argList));
                if (returnVal == 0)
                {
                    // return DOMAIN\user
                    return argList[1] + "\\" + argList[0];
                }
            }

            return "NO OWNER";
        }
        private ManagementObjectSearcher mos = null;
        private ManagementObjectCollection moc = null;
        private void GetProcessInfo()
        {
            string Query = "SELECT ProcessID,ExecutablePath,CommandLine,CreationDate FROM Win32_Process ";
            mos = new ManagementObjectSearcher(Query);
            moc = mos.Get();
        }
        private Dictionary<string, string> GetProcessPath(int processId)
        {

            Dictionary<string, string> o = new Dictionary<string, string>();
            o["fullPath"] = "";
            o["CommandLine"] = "";
            o["CreationDate"] = ""; //start time
            o["ExcuteTimes"] = ""; //run seconds
            foreach (ManagementObject searcher in moc)
            {
                //Console.WriteLine(searcher);
                if (Convert.ToInt32(searcher["ProcessID"]) == processId)
                {
                    if (searcher["ExecutablePath"] != null)
                    {
                        o["fullPath"] = searcher["ExecutablePath"].ToString();
                    }
                    if (searcher["CommandLine"] != null)
                    {
                        o["CommandLine"] = searcher["CommandLine"].ToString();
                    }
                    if (searcher["CreationDate"] != null)
                    {
                        string t = _form.my.explode(".", searcher["CreationDate"].ToString())[0];
                        o["CreationDate"] = _form.my.date("Y-m-d H:i:s", _form.my.strtotime(t.Substring(0, 4) + "-" + t.Substring(4, 2) + "-" + t.Substring(6, 2) + " " + t.Substring(8, 2) + ":" + t.Substring(10, 2) + ":" + t.Substring(12, 2)));
                        //Console.WriteLine(o["CreationDate"]);
                        o["ExcuteTimes"] = (Convert.ToInt64(_form.my.time()) - Convert.ToInt64(_form.my.strtotime(o["CreationDate"]))).ToString();
                    }
                }
            }

            return o;
        }

    }
}
