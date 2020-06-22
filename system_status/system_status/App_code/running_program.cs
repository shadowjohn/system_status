using System;
using System.Collections.Generic;
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

        public void init(Form1 theform)
        {
            _form = theform;
            if (_form.threads.ContainsKey("running_program"))
            {
                _form.threads["running_program"].Abort();
                _form.threads["running_program"] = null;
            }
            is_running = true;
            theform.setStatusBar("工作管理員資訊載入開始...", 0);
            //From : http://jengting.blogspot.com/2016/07/DataGridView-Sample.html
            theform.running_program_grid.AutoGenerateColumns = false; //這啥
            theform.running_program_grid.AllowUserToAddRows = false; //不能允許使用者自行調整
            theform.running_program_grid.RowHeadersVisible = false; //左邊空欄移除
            theform.running_program_grid.Dock = DockStyle.Fill; //自動展開到最大
            theform.running_program_grid.AllowDrop = false;
            theform.running_program_grid.ReadOnly = true;

            theform.running_program_grid.Columns.Clear();
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
    }
]
";
            /*
           
    {   
        ""running_programCPU"":{""id"":""running_programCPU"",""name"":""CPU"",""width"":80,""display"":true,""headerAlign"":""center"",""cellAlign"":""center""}
    },
    {   
        ""running_programRAM"":{""id"":""running_programRAM"",""name"":""記憶體"",""width"":80,""display"":true,""headerAlign"":""center"",""cellAlign"":""center""}
    },
    {   
        ""running_programDISK"":{""id"":""running_programDISK"",""name"":""磁碟"",""width"":80,""display"":true,""headerAlign"":""center"",""cellAlign"":""center""}
    },
    {   
        ""running_programNETWORK"":{""id"":""running_programNETWORK"",""name"":""網路"",""width"":80,""display"":true,""headerAlign"":""center"",""cellAlign"":""center""}
    }
             */
            //grid_init(json_columns);
            _form.my.grid_init(_form.running_program_grid, json_columns);
            //allow sorting
            foreach (DataGridViewColumn column in _form.running_program_grid.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.Automatic;
            }
            _form.threads["running_program"] = new Thread(() => run());
            _form.threads["running_program"].Start();
            //run();
        }

        void run()
        {
            //_form.running_program_grid.Rows.Clear();
            _form.UpdateUI_DataGridGrid(_form.running_program_grid, "clear", "", "", -1);
            is_running = true;
            Process[] processlist = Process.GetProcesses();
            int step = 0;
            int total_step = processlist.Count();
            GetProcessInfo();
            foreach (Process theprocess in processlist)
            {
                _form.setStatusBar("執行緒資訊載入中...", Convert.ToInt32((Convert.ToDouble(step) / Convert.ToDouble(total_step)) * 100.0));
                step++;
                //_form.running_program_grid.Rows.Add();
                _form.UpdateUI_DataGridGrid(_form.running_program_grid, "add", "", "", -1);
                int lastId = _form.running_program_grid.Rows.Count - 1;
                int pid = theprocess.Id;
                Dictionary<string, string> o = GetProcessPath(pid);
                _form.running_program_grid.Rows[lastId].Cells["running_programID"].Value = step.ToString();
                _form.running_program_grid.Rows[lastId].Cells["running_programPID"].Value = pid.ToString();
                _form.running_program_grid.Rows[lastId].Cells["running_programName"].Value = theprocess.ProcessName;
                _form.running_program_grid.Rows[lastId].Cells["running_programBaseName"].Value = _form.my.basename(o["fullPath"]);
                _form.running_program_grid.Rows[lastId].Cells["running_programPath"].Value = o["fullPath"];
                _form.running_program_grid.Rows[lastId].Cells["running_programCommandLine"].Value = o["CommandLine"];
            }
            _form.setStatusBar("執行緒資訊載入完成...", 100);
            is_running = false;
        }
        private ManagementObjectSearcher mos = null;
        private ManagementObjectCollection moc = null;
        private void GetProcessInfo()
        {
            string Query = "SELECT ProcessID,ExecutablePath,CommandLine FROM Win32_Process ";
            mos = new ManagementObjectSearcher(Query);
            moc = mos.Get();
        }
        private Dictionary<string, string> GetProcessPath(int processId)
        {

            Dictionary<string, string> o = new Dictionary<string, string>();
            o["fullPath"] = "";
            o["CommandLine"] = "";
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
                }
            }

            return o;
        }

    }
}
