using Microsoft.Win32.TaskScheduler;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace system_status.App_code
{
    class schedule
    {
        Form1 _form = null;
        public bool is_running = false;
        public string last_date = "";
        private DataTable dt = new DataTable();
        private bool isGridInit = false;
        public void init(Form1 theform)
        {
            _form = theform;
            if (_form.threads.ContainsKey("schedule"))
            {
                _form.threads["schedule"].Abort();
                _form.threads["schedule"] = null;
            }
            is_running = true;
            last_date = _form.my.time();
            _form.setStatusBar("排程資料載入開始...", 0);
            //From : http://jengting.blogspot.com/2016/07/DataGridView-Sample.html
            _form.schedule_grid.AutoGenerateColumns = false; //這啥
            _form.schedule_grid.AllowUserToAddRows = false; //不能允許使用者自行調整
            _form.schedule_grid.RowHeadersVisible = false; //左邊空欄移除
            _form.schedule_grid.Dock = DockStyle.Fill; //自動展開到最大
            _form.schedule_grid.AllowDrop = false;
            _form.schedule_grid.ReadOnly = true;

            //_form.schedule_grid.Columns.Clear();
            string json_columns = @"
[
    {   
        ""scheduleID"":{""id"":""scheduleID"",""name"":""項次"",""width"":50,""display"":true,""headerAlign"":""center"",""cellAlign"":""center""}
    },           
    {   
        ""scheduleName"":{""id"":""scheduleName"",""name"":""排程名稱"",""width"":300,""display"":true,""headerAlign"":""center"",""cellAlign"":""left""}
    },
            
    {   
        ""scheduleEnable"":{""id"":""scheduleEnable"",""name"":""是否執行"",""width"":80,""display"":true,""headerAlign"":""center"",""cellAlign"":""center""}
    },
    {   
        ""scheduleFolder"":{""id"":""scheduleFolder"",""name"":""目錄"",""width"":350,""display"":false,""headerAlign"":""center"",""cellAlign"":""left""}
    },
    {   
        ""schedulePath"":{""id"":""schedulePath"",""name"":""語法路徑"",""width"":350,""display"":true,""headerAlign"":""center"",""cellAlign"":""left""}
    },
    {   
        ""schedulePrevDateTime"":{""id"":""schedulePrevDateTime"",""name"":""上次執行時間"",""width"":180,""display"":true,""headerAlign"":""center"",""cellAlign"":""center""}
    },
    {   
        ""scheduleNextDateTime"":{""id"":""scheduleNextDateTime"",""name"":""下次執行時間"",""width"":180,""display"":true,""headerAlign"":""center"",""cellAlign"":""center""}
    }
";
            //表格初始化
            if (isGridInit == false)
            {
                _form.my.grid_init(_form.schedule_grid, json_columns);
                isGridInit = true;
            }
            dt = _form.my.datatable_init(json_columns);

            //allow sorting
            foreach (DataGridViewColumn column in _form.schedule_grid.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.Automatic;
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            }

            _form.threads["schedule"] = new Thread(() => run());
            _form.threads["schedule"].Start();
        }
        public void run()
        {
            //_form.UpdateUI_DataGridGrid(_form.schedule_grid, "clear", "", "", -1);
            using (TaskService ts = new TaskService())
            {

                EnumFolderTasks(ts.RootFolder);
                _form.updateDGVUI(_form.schedule_grid, dt);

                //Console.WriteLine(ts.RootFolder);
                _form.setStatusBar("就緒", 0);
                is_running = false;
            }
        }
        void EnumFolderTasks(TaskFolder fld)
        {
            DataRow row = dt.NewRow();
            int step = 0;
            foreach (Task task in fld.Tasks)
            {
                var td = task.Definition;
                string path = string.Join(" ", td.Actions);
                //ActOnTask(task);
                //Console.WriteLine(task);
                /*
                _form.UpdateUI_DataGridGrid(_form.schedule_grid, "add", "", "", -1);
                int lastId = _form.schedule_grid.Rows.Count - 1;
                _form.UpdateUI_DataGridGrid(_form.schedule_grid, "set_cell", "scheduleID", (lastId + 1).ToString(), lastId);
                _form.UpdateUI_DataGridGrid(_form.schedule_grid, "set_cell", "scheduleName", task.Name, lastId);
                _form.UpdateUI_DataGridGrid(_form.schedule_grid, "set_cell", "scheduleEnable", (task.IsActive) ? "是" : "否", lastId);
                _form.UpdateUI_DataGridGrid(_form.schedule_grid, "set_cell", "scheduleFolder", task.Folder, lastId);
                _form.UpdateUI_DataGridGrid(_form.schedule_grid, "set_cell", "schedulePath", path, lastId);
                _form.UpdateUI_DataGridGrid(_form.schedule_grid, "set_cell", "schedulePrevDateTime", task.LastRunTime.ToString("yyyy-MM-dd HH:mm:ss"), lastId);

                */
                row = dt.NewRow();
                row["scheduleID"] = ++step;
                row["scheduleName"] = task.Name;
                row["scheduleEnable"] = (task.IsActive) ? "是" : "否";
                row["scheduleFolder"] = task.Folder;
                row["schedulePath"] = path;
                row["schedulePrevDateTime"] = task.LastRunTime.ToString("yyyy-MM-dd HH:mm:ss");



                if (!task.IsActive)
                {
                    //_form.UpdateUI_DataGridGrid(_form.schedule_grid, "set_cell", "scheduleNextDateTime", "--", lastId);
                    row["scheduleNextDateTime"] = "--";
                }
                else
                {
                    string nt = task.NextRunTime.ToString("yyyy-MM-dd HH:mm:ss");
                    if (nt == "0001-01-01 00:00:00")
                    {
                        //_form.UpdateUI_DataGridGrid(_form.schedule_grid, "set_cell", "scheduleNextDateTime", "--", lastId);
                        row["scheduleNextDateTime"] = "--";
                    }
                    else
                    {
                        //_form.UpdateUI_DataGridGrid(_form.schedule_grid, "set_cell", "scheduleNextDateTime", task.NextRunTime.ToString("yyyy-MM-dd HH:mm:ss"), lastId);
                        row["scheduleNextDateTime"] = task.NextRunTime.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                }
                dt.Rows.Add(row);

            }
            foreach (TaskFolder sfld in fld.SubFolders)
            {
                EnumFolderTasks(sfld);
            }
        }

    }
}
