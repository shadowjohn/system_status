using System;
using System.Data;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace system_status.App_code
{
    internal class events
    {
        private Form1 _form = null;
        public bool is_running = false;
        public string last_date = "";
        public DataTable dt = new DataTable();
        private bool isGridInit = false;

        public void init(Form1 theform)
        {
            _form = theform;
            if (_form.threads.ContainsKey("events"))
            {
                _form.threads["events"].Abort();
                _form.threads["events"] = null;
            }
            is_running = true;
            last_date = _form.my.time();
            _form.setStatusBar("事件紀錄表載入開始...", 0);
            //From : http://jengting.blogspot.com/2016/07/DataGridView-Sample.html
            _form.events_grid.AutoGenerateColumns = false; //這啥
            _form.events_grid.AllowUserToAddRows = false; //不能允許使用者自行調整
            _form.events_grid.RowHeadersVisible = false; //左邊空欄移除
            _form.events_grid.Dock = DockStyle.Fill; //自動展開到最大
            _form.events_grid.AllowDrop = false;
            _form.events_grid.ReadOnly = true;

            //_form.events_grid.Columns.Clear();
            string json_columns = @"
[
    {
        ""eventsID"":{""id"":""eventsID"",""name"":""項次"",""width"":50,""display"":true,""headerAlign"":""center"",""cellAlign"":""center""}
    },
    {
        ""eventsIndex"":{""id"":""eventsIndex"",""name"":""編號"",""width"":90,""display"":true,""headerAlign"":""center"",""cellAlign"":""center""}
    },
    {
        ""eventsDateTime"":{ ""id"":""eventsDateTime"",""name"":""事件時間"",""width"":190,""display"":true,""headerAlign"":""center"",""cellAlign"":""center""}
    },
    {
        ""eventsCategory"":{""id"":""eventsCategory"",""name"":""Category"",""width"":80,""display"":true,""headerAlign"":""center"",""cellAlign"":""center""}
    },
    {
        ""eventsMessage"":{""id"":""eventsMessage"",""name"":""內容"",""width"":550,""display"":true,""headerAlign"":""center"",""cellAlign"":""left""}
    }
";
            //表格初始化
            if (isGridInit == false)
            {
                _form.my.grid_init(_form.events_grid, json_columns);
                isGridInit = true;
            }
            dt = _form.my.datatable_init(json_columns);

            //allow sorting
            foreach (DataGridViewColumn column in _form.events_grid.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.Automatic;
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            }

            _form.threads["events"] = new Thread(() => run());
            _form.threads["events"].Start();
        }

        public void run()
        {
            //_form.UpdateUI_DataGridGrid(_form.events_grid, "clear", "", "", -1);
            //string eventLogName = "Application";
            //"Application"
            System.Diagnostics.EventLog eventLog = new System.Diagnostics.EventLog();
            eventLog.Log = "Application";
            //eventLog.Log = eventLogName;
            int step = 0;
            DataRow row = dt.NewRow();
            //_form.my.file_put_contents(_form.my.pwd() + "\\log\\events.txt", _form.my.json_encode(eventLog.Entries));
            //_form.my.file_put_contents(_form.my.pwd() + "\\log\\events.txt", "");
            for (int i = eventLog.Entries.Count - 1; i >= 0; i--)
            {
                EventLogEntry log = eventLog.Entries[i];
                //Dictionary<string, string> d = new Dictionary<string, string>();
                //d["Index"] = log.Index.ToString();
                //d["Category"] = log.Category;
                //d["Message"] = log.Message;
                //d["DateTime"] = log.TimeGenerated.ToString("yyyy-MM-dd HH:mm:ss");
                //var twtzinfo = TimeZoneInfo.FindSystemTimeZoneById("Taipei Standard Time");
                string eventDateTime = log.TimeGenerated.ToString("yyyy-MM-dd HH:mm:ss");

                //_form.my.file_put_contents(_form.my.pwd() + "\\log\\events.txt", eventDateTime, true);
                //改成現在時間跟上一次回報的時間之間才傳
                if (Convert.ToInt64(_form.my.strtotime(eventDateTime)) < Convert.ToInt64(_form.my.time()) - Convert.ToInt64(_form.my.getSystemKey("LOOP_MINUTE")) * 60)
                {
                    continue;
                }

                /*
                _form.UpdateUI_DataGridGrid(_form.events_grid, "add", "", "", -1);
                int lastId = _form.events_grid.Rows.Count - 1;
                _form.UpdateUI_DataGridGrid(_form.events_grid, "set_cell", "eventsID", (lastId + 1).ToString(), lastId);
                _form.UpdateUI_DataGridGrid(_form.events_grid, "set_cell", "eventsIndex", log.Index.ToString(), lastId);
                _form.UpdateUI_DataGridGrid(_form.events_grid, "set_cell", "eventsCategory", log.Category, lastId);
                _form.UpdateUI_DataGridGrid(_form.events_grid, "set_cell", "eventsMessage", log.Message, lastId);
                _form.UpdateUI_DataGridGrid(_form.events_grid, "set_cell", "eventsDateTime", eventDateTime, lastId);
                */

                row = dt.NewRow();
                row["eventsID"] = ++step;
                row["eventsIndex"] = log.Index.ToString();
                row["eventsCategory"] = log.Category;
                row["eventsMessage"] = log.Message;
                row["eventsDateTime"] = eventDateTime;
                dt.Rows.Add(row);

                //my.echo(log.Message + "\n");
                //step++;
                //if (step >= 200)
                //{
                //    break;
                //}
            }
            _form.updateDGVUI(_form.events_grid, dt);
            _form.setStatusBar("就緒", 0);
            is_running = false;
        }
    }
}