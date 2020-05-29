using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Simplified.IO;
namespace system_status.App_code
{
    class hdd_info
    {
        public hdd_info()
        {
        }
        private Form1 _form = null;
        public bool is_running = false;
        public void init(Form1 theform)
        {
           
            _form = theform;
            is_running = true;
            _form.setStatusBar("硬碟資訊載入開始...", 0);
            //From : http://jengting.blogspot.com/2016/07/DataGridView-Sample.html
            _form.hdd_grid.AutoGenerateColumns = false; //這啥
            _form.hdd_grid.AllowUserToAddRows = false; //不能允許使用者自行調整
            _form.hdd_grid.RowHeadersVisible = false; //左邊空欄移除
            _form.hdd_grid.Dock = DockStyle.Fill; //自動展開到最大
            _form.hdd_grid.AllowDrop = false;
            _form.hdd_grid.ReadOnly = true;

            _form.hdd_grid.Columns.Clear();
            string json_columns = @"
[
    {   
        ""hddID"":{""id"":""hddID"",""name"":""磁碟代號"",""width"":80,""display"":true,""headerAlign"":""center"",""cellAlign"":""center""}
    },
    {   
        ""hddType"":{""id"":""hddType"",""name"":""磁碟類型"",""width"":100,""display"":true,""headerAlign"":""center"",""cellAlign"":""center""}
    },
    {   
        ""hddFormatType"":{""id"":""hddFormatType"",""name"":""分割類型"",""width"":80,""display"":true,""headerAlign"":""center"",""cellAlign"":""center""}
    },
    {   
        ""hddModel"":{""id"":""hddModel"",""name"":""硬碟型號"",""width"":80,""display"":true,""headerAlign"":""center"",""cellAlign"":""left""}
    },
    {   
        ""hddTotalSpace"":{""id"":""hddTotalSpace"",""name"":""總空間_資料"",""width"":80,""display"":false,""headerAlign"":""center"",""cellAlign"":""right""}
    },
    {   
        ""hddTotalSpaceDisplay"":{""id"":""hddTotalSpaceDisplay"",""name"":""總空間"",""width"":80,""display"":true,""headerAlign"":""center"",""cellAlign"":""right""}
    },
    {
        ""hddUsageSpace"":{""id"":""hddUsageSpace"",""name"":""已使用空間_資料"",""width"":100,""display"":false,""headerAlign"":""center"",""cellAlign"":""right""}
    },
    {
        ""hddUsageSpaceDisplay"":{""id"":""hddUsageSpaceDisplay"",""name"":""已使用空間"",""width"":100,""display"":true,""headerAlign"":""center"",""cellAlign"":""right""}
    },
    {
        ""hddFreeSpace"":{""id"":""hddFreeSpace"",""name"":""剩餘空間_資料"",""width"":80,""display"":false,""headerAlign"":""center"",""cellAlign"":""right""}
    },
    {
        ""hddFreeSpaceDisplay"":{""id"":""hddFreeSpaceDisplay"",""name"":""剩餘空間"",""width"":80,""display"":true,""headerAlign"":""center"",""cellAlign"":""right""}
    },
    {
        ""hddUsagePercent"":{""id"":""hddUsagePercent"",""name"":""百分比"",""width"":80,""display"":true,""headerAlign"":""center"",""cellAlign"":""right""}
    },
    {
        ""hddUsageHour"":{""id"":""hddUsageHour"",""name"":""使用時間"",""width"":80,""display"":true,""headerAlign"":""center"",""cellAlign"":""right""}
    },
    {
        ""hddOnOffTimes"":{""id"":""hddOnOffTimes"",""name"":""開關機次數"",""width"":80,""display"":true,""headerAlign"":""center"",""cellAlign"":""right""}
    },
    {
        ""hddBadSectors"":{""id"":""hddBadSectors"",""name"":""壞軌"",""width"":80,""display"":true,""headerAlign"":""center"",""cellAlign"":""right""}
    },
    {
        ""hddTemperature"":{""id"":""hddTemperature"",""name"":""溫度"",""width"":80,""display"":true,""headerAlign"":""center"",""cellAlign"":""right""}
    }
]
            ";
            //表格初始化
            _form.my.grid_init(_form.hdd_grid, json_columns);

            //allow sorting
            foreach (DataGridViewColumn column in _form.hdd_grid.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.Automatic;
            }
            run();
        }
        void run()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();

            var SmartDrives = Simplified.IO.Smart.GetDrives();
            int step = 0;
            int total_step = drives.Count();
            _form.hdd_grid.Rows.Clear();
            foreach (var drive in drives)
            {
                _form.setStatusBar("硬碟資訊載入中...", Convert.ToInt32((Convert.ToDouble(step) / Convert.ToDouble(total_step)) * 100.0));
                step++;
                //There are more attributes you can use.
                //Check the MSDN link for a complete example.
                //Console.WriteLine(drive.Name);

                if (!drive.IsReady)
                {
                    continue;
                }
                string driverName = drive.Name;
                driverName = driverName.Replace("\\", "");
                DriveType driverType = drive.DriveType;
                long freeSpace = drive.TotalFreeSpace;
                long totalSize = drive.TotalSize;
                long usageSize = totalSize - freeSpace;
                _form.hdd_grid.Rows.Add();
                int lastId = _form.hdd_grid.Rows.Count - 1;
                _form.hdd_grid.Rows[lastId].Cells["hddID"].Value = driverName;

                _form.hdd_grid.Rows[lastId].Cells["hddType"].Value = driverType.ToString();

                _form.hdd_grid.Rows[lastId].Cells["hddFormatType"].Value = drive.DriveFormat;

                _form.hdd_grid.Rows[lastId].Cells["hddTotalSpace"].Value = totalSize;

                _form.hdd_grid.Rows[lastId].Cells["hddTotalSpaceDisplay"].Value = _form.my.size_hum_read(totalSize);

                _form.hdd_grid.Rows[lastId].Cells["hddUsageSpace"].Value = usageSize;

                _form.hdd_grid.Rows[lastId].Cells["hddUsageSpaceDisplay"].Value = _form.my.size_hum_read(usageSize);


                _form.hdd_grid.Rows[lastId].Cells["hddFreeSpace"].Value = freeSpace;


                _form.hdd_grid.Rows[lastId].Cells["hddFreeSpaceDisplay"].Value = _form.my.size_hum_read(freeSpace);

                double percent = (Convert.ToDouble(usageSize) / Convert.ToDouble(totalSize)) * 100.0;
                _form.hdd_grid.Rows[lastId].Cells["hddUsagePercent"].Value = string.Format("{0:0.00}", percent) + " %";
                //_form.hdd_grid.Rows[lastId].Cells["hddUsagePercent"].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                if (percent < 60)
                {
                    _form.hdd_grid.Rows[lastId].Cells["hddUsagePercent"].Style.ForeColor = Color.Green;
                }
                else if (percent >= 60 && percent < 85)
                {
                    _form.hdd_grid.Rows[lastId].Cells["hddUsagePercent"].Style.ForeColor = Color.Orange;
                }
                else
                {
                    //剩很少空間
                    _form.hdd_grid.Rows[lastId].Cells["hddUsagePercent"].Style.ForeColor = Color.Red;
                    _form.hdd_grid.Rows[lastId].Cells["hddUsagePercent"].Style.Font = new Font(_form.hdd_grid.Columns["hddUsagePercent"].DefaultCellStyle.Font, FontStyle.Bold);
                }

                //接下來是跑 smart
                foreach (var SmartDrive in SmartDrives)
                {
                    if (SmartDrive.DriveLetters.Count == 0)
                    {
                        continue;
                    }

                    if (!_form.my.in_array(driverName, SmartDrive.DriveLetters))
                    {
                        //同名才處理
                        continue;
                    }
                    /*if (SmartDrive.DriveLetters[0] != driverName)
                    {
                        
                        continue;
                    }
                    */
                    //型號
                    _form.hdd_grid.Rows[lastId].Cells["hddModel"].Value = SmartDrive.Model;
                    //Console.WriteLine(_form.my.json_encode(SmartDrive));
                    //Console.WriteLine(SmartDrive.DriveLetters[0] + "," + driverName);
                    foreach (var p in SmartDrive.SmartAttributes)
                    {
                        //Console.WriteLine(p.Name);
                        switch (p.Name)
                        {
                            case "Temperature":
                                //溫度
                                if (p.Register == 194)
                                {
                                    string t = p.Data.ToString();
                                    if (p.Data > 300)
                                    {
                                        //10進制轉16進制，取最後二碼，再還原10進制
                                        //From : https://www.techiedelight.com/conversion-between-integer-and-hexadecimal-csharp/
                                        string hex_str = p.Data.ToString("X");
                                        hex_str = hex_str.Substring(hex_str.Length - 2, 2);
                                        t = Int32.Parse(hex_str, System.Globalization.NumberStyles.HexNumber).ToString();
                                    }
                                    _form.hdd_grid.Rows[lastId].Cells["hddTemperature"].Value = t.ToString();
                                }
                                break;
                            case "Reallocated sector count":
                                //壞軌數
                                _form.hdd_grid.Rows[lastId].Cells["hddBadSectors"].Value = p.Data;
                                break;
                            case "Power-on hours count":
                                //開機時數                                
                                _form.hdd_grid.Rows[lastId].Cells["hddUsageHour"].Value = p.Data;
                                break;
                            case "Power cycle count":
                                //開關機次數
                                _form.hdd_grid.Rows[lastId].Cells["hddOnOffTimes"].Value = p.Data;
                                break;
                        }
                    }
                }
            } // Drives
            _form.setStatusBar("就緒", 0);
            is_running = false;
        }
    }
}
