using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceProcess;
using System.Windows.Forms;
using System.Management;
using System.Threading;
namespace system_status.App_code
{
    class system_service
    {
        Form1 _form = null;
        public bool is_running = false;
        public string last_date = "";

        public void init(Form1 theform)
        {
            _form = theform;
            if (_form.threads.ContainsKey("system_service"))
            {
                _form.threads["system_service"].Abort();
                _form.threads["system_service"] = null;
            }
            is_running = true;
            last_date = _form.my.time();
            _form.system_service_grid.AutoGenerateColumns = false; //這啥
            _form.system_service_grid.AllowUserToAddRows = false; //不能允許使用者自行調整
            _form.system_service_grid.RowHeadersVisible = false; //左邊空欄移除
            _form.system_service_grid.Dock = DockStyle.Fill; //自動展開到最大
            _form.system_service_grid.AllowDrop = false;
            _form.system_service_grid.ReadOnly = true;


            _form.system_service_grid.Columns.Clear();
            string json_columns = @"
[
    {   
        ""system_serviceID"":{""id"":""system_serviceID"",""name"":""項次"",""width"":50,""display"":true,""headerAlign"":""center"",""cellAlign"":""center""}
    },           
    {   
        ""system_serviceName"":{""id"":""system_serviceName"",""name"":""功能名稱"",""width"":300,""display"":true,""headerAlign"":""center"",""cellAlign"":""left""}
    },
    {   
        ""system_serviceDescription"":{""id"":""system_serviceDescription"",""name"":""功能描述"",""width"":300,""display"":true,""headerAlign"":""center"",""cellAlign"":""left""}
    },
    {   
        ""system_serviceStatus"":{""id"":""system_serviceStatus"",""name"":""狀態"",""width"":80,""display"":true,""headerAlign"":""center"",""cellAlign"":""center""}
    },
    {   
        ""system_serviceStartupStatus"":{""id"":""system_serviceStartupStatus"",""name"":""啟動類型"",""width"":80,""display"":true,""headerAlign"":""center"",""cellAlign"":""center""}
    },
    {   
        ""system_servicePathName"":{""id"":""system_servicePathName"",""name"":""路徑"",""width"":350,""display"":true,""headerAlign"":""center"",""cellAlign"":""left""}
    }
]            
";
            //表格初始化
            _form.my.grid_init(_form.system_service_grid, json_columns);

            //allow sorting
            foreach (DataGridViewColumn column in _form.system_service_grid.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.Automatic;
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            }
            _form.threads["system_service"] = new Thread(() => run());
            _form.threads["system_service"].Start();
            //run();
        }
        public void run()
        {
            int lastId = 0;
            // get list of Windows services
            //https://docs.microsoft.com/zh-tw/dotnet/api/system.serviceprocess.servicecontroller?view=dotnet-plat-ext-3.1
            //https://www.csharp-examples.net/windows-service-list/
            ServiceController[] services = ServiceController.GetServices();
            // try to find service name

            _form.UpdateUI_DataGridGrid(_form.system_service_grid, "clear", "", "", -1);
            foreach (ServiceController service in services)
            {
                //if (service.ServiceName == serviceName)
                //    return true;
                Console.WriteLine(service.ServiceName + "," + service.DisplayName);
                //_form.system_service_grid.Rows.Add();
                _form.UpdateUI_DataGridGrid(_form.system_service_grid, "add", "", "", -1);
                lastId = _form.system_service_grid.Rows.Count - 1;
                _form.system_service_grid.Rows[lastId].Cells["system_serviceID"].Value = (lastId + 1);
                _form.system_service_grid.Rows[lastId].Cells["system_serviceName"].Value = service.DisplayName;
                ManagementObject serviceObject = new ManagementObject(new ManagementPath(string.Format("Win32_Service.Name='{0}'", service.ServiceName)));
                //https://stackoverflow.com/questions/16547216/read-the-description-of-a-windows-service
                if (serviceObject["Description"] != null)
                {
                    _form.system_service_grid.Rows[lastId].Cells["system_serviceDescription"].Value = serviceObject["Description"].ToString();
                }
                else
                {
                    _form.system_service_grid.Rows[lastId].Cells["system_serviceDescription"].Value = "";
                }

                switch (service.Status.ToString())
                {
                    case "Running":
                        _form.system_service_grid.Rows[lastId].Cells["system_serviceStatus"].Value = "執行中";
                        break;
                    case "Stopped":
                        _form.system_service_grid.Rows[lastId].Cells["system_serviceStatus"].Value = "停止";
                        break;
                }

                var st = service.ServiceType;

                /*
                Automatic 	2 	
                Boot 	0 	
                Disabled 	4 	
                Manual 	3 	
                System 	1 	
                */
                // From : https://www.itdaan.com/tw/753d0af27c5447f3eb5ca7ff90a5bd09
                _form.system_service_grid.Rows[lastId].Cells["system_serviceStartupStatus"].Value = serviceObject.Properties["StartMode"].Value;
                _form.system_service_grid.Rows[lastId].Cells["system_servicePathName"].Value = serviceObject.Properties["PathName"].Value;


                //_form.system_service_grid.Rows[lastId].Cells["system_serviceLoginAccount"].Value = service.MachineName;

            }
            is_running = false;
        }
    }
}
