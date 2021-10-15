using Microsoft.Web.Administration;
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
    class iis
    {
        Form1 _form = null;
        public bool is_running = false;
        public string last_date = "";
        private DataTable dt = new DataTable();
        private bool isGridInit = false;
        public void init(Form1 theform)
        {
            _form = theform;
            if (_form.threads.ContainsKey("iis"))
            {
                _form.threads["iis"].Abort();
                _form.threads["iis"] = null;
            }

            is_running = true;
            last_date = _form.my.time();
            _form.iis_grid.AutoGenerateColumns = false; //這啥
            _form.iis_grid.AllowUserToAddRows = false; //不能允許使用者自行調整
            _form.iis_grid.RowHeadersVisible = false; //左邊空欄移除
            _form.iis_grid.Dock = DockStyle.Fill; //自動展開到最大
            _form.iis_grid.AllowDrop = false;
            //_form.iis_grid.ReadOnly = true;

           // _form.iis_grid.Columns.Clear();
            string json_columns = @"
[
    {   
        ""site_id"":{""id"":""site_id"",""name"":""項次"",""width"":80,""display"":true,""headerAlign"":""center"",""cellAlign"":""center""}
    },            
    {   
        ""name"":{""id"":""name"",""name"":""名稱"",""width"":380,""display"":true,""headerAlign"":""center"",""cellAlign"":""left""}
    },            
    {   
        ""PhysicalPath"":{""id"":""PhysicalPath"",""name"":""PhysicalPath"",""width"":380,""display"":true,""headerAlign"":""center"",""cellAlign"":""left""}
    }, 
    {   
        ""Path"":{""id"":""Path"",""name"":""Path"",""width"":80,""display"":true,""headerAlign"":""center"",""cellAlign"":""left""}
    },            
]
";
            //表格初始化
            if (isGridInit == false)
            {
                _form.my.grid_init(_form.iis_grid, json_columns);
                isGridInit = true;
            }
            dt = _form.my.datatable_init(json_columns);

            //allow sorting
            foreach (DataGridViewColumn column in _form.iis_grid.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.Automatic;
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            }
            _form.threads["iis"] = new Thread(() => run());
            _form.threads["iis"].Start();
        }
        void run()
        {
            int step = 0;
            DataRow row = dt.NewRow();
            //https://stackoverflow.com/questions/4593412/list-all-websites-in-iis-c-sharp
            var iisManager = new ServerManager();
            SiteCollection sites = iisManager.Sites;
            _form.my.file_put_contents(_form.my.pwd() + "\\log\\iislog.txt", "");
            foreach (Site site in sites)
            {
                row = dt.NewRow();
                //_form.my.file_put_contents(_form.my.pwd() + "\\log\\iislog.txt", sites[i]., true);
                row["site_id"] = site.Id;
                row["name"] = site.Name;
                row["PhysicalPath"] = site.Applications["/"].VirtualDirectories["/"].PhysicalPath;
                row["Path"] = site.Applications["/"].Path;
                dt.Rows.Add(row);
            }
            //

            

            _form.updateDGVUI(_form.iis_grid, dt);
            _form.my.file_put_contents(_form.my.pwd() + "\\log\\iislog.txt", _form.my.json_encode(dt));
            _form.setStatusBar("就緒", 0);
            is_running = false;
        }
    }
}
