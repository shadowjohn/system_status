using Simplified.IO;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace system_status.App_code
{
    internal class hdd_info
    {
        public hdd_info()
        {
        }

        private Form1 _form = null;
        public bool is_running = false;
        public string last_date = "";
        public DataTable dt = new DataTable();
        private bool isGridInit = false;

        public void init(Form1 theform)
        {
            _form = theform;
            if (_form.threads.ContainsKey("hdd_info"))
            {
                _form.threads["hdd_info"].Abort();
                _form.threads["hdd_info"] = null;
            }
            is_running = true;
            last_date = _form.my.time();
            _form.setStatusBar("硬碟資訊載入開始...", 0);
            //From : http://jengting.blogspot.com/2016/07/DataGridView-Sample.html
            _form.hdd_grid.AutoGenerateColumns = false; //這啥
            _form.hdd_grid.AllowUserToAddRows = false; //不能允許使用者自行調整
            _form.hdd_grid.RowHeadersVisible = false; //左邊空欄移除
            _form.hdd_grid.Dock = DockStyle.Fill; //自動展開到最大
            _form.hdd_grid.AllowDrop = false;
            _form.hdd_grid.ReadOnly = true;

            //_form.hdd_grid.Columns.Clear();
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
            if (isGridInit == false)
            {
                _form.my.grid_init(_form.hdd_grid, json_columns);
                isGridInit = true;
            }
            dt = _form.my.datatable_init(json_columns);

            //allow sorting
            foreach (DataGridViewColumn column in _form.hdd_grid.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.Automatic;
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            }
            _form.threads["hdd_info"] = new Thread(() => run());
            _form.threads["hdd_info"].Start();
            //run();
        }

        private void run()
        {
            DataRow row = dt.NewRow();
            int step = 0;
            DriveInfo[] drives = DriveInfo.GetDrives();

            DriveCollection SmartDrives = null;
            try
            {
                SmartDrives = Simplified.IO.Smart.GetDrives();
            }
            catch
            {
            }
            int _stephdd = 0;

            int total_step = drives.Count();
            //_form.hdd_grid.Rows.Clear();
            //_form.UpdateUI_DataGridGrid(_form.hdd_grid, "clear", "", "", -1);
            foreach (var drive in drives)
            {
                row = dt.NewRow();
                _form.setStatusBar("硬碟資訊載入中...", Convert.ToInt32((Convert.ToDouble(_stephdd) / Convert.ToDouble(total_step)) * 100.0));
                _stephdd++;
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
                //_form.hdd_grid.Rows.Add();
                //_form.UpdateUI_DataGridGrid(_form.hdd_grid, "add", "", "", -1);
                //int lastId = _form.hdd_grid.Rows.Count - 1;
                //_form.hdd_grid.Rows[lastId].Cells["hddID"].Value = driverName;
                row["hddID"] = driverName;

                //_form.hdd_grid.Rows[lastId].Cells["hddType"].Value = driverType.ToString();
                row["hddType"] = driverType.ToString();

                //_form.hdd_grid.Rows[lastId].Cells["hddFormatType"].Value = drive.DriveFormat;
                row["hddFormatType"] = drive.DriveFormat;

                //_form.hdd_grid.Rows[lastId].Cells["hddTotalSpace"].Value = totalSize;
                row["hddTotalSpace"] = totalSize;

                //_form.hdd_grid.Rows[lastId].Cells["hddTotalSpaceDisplay"].Value = _form.my.size_hum_read(totalSize);
                //_form.UpdateUI_DataGridGrid(_form.hdd_grid, "set_cell", "hddTotalSpaceDisplay", _form.my.size_hum_read(totalSize), lastId);
                row["hddTotalSpaceDisplay"] = _form.my.size_hum_read(totalSize);

                //_form.hdd_grid.Rows[lastId].Cells["hddUsageSpace"].Value = usageSize;
                row["hddUsageSpace"] = usageSize;

                //_form.hdd_grid.Rows[lastId].Cells["hddUsageSpaceDisplay"].Value = _form.my.size_hum_read(usageSize);
                row["hddUsageSpaceDisplay"] = _form.my.size_hum_read(usageSize);

                //_form.hdd_grid.Rows[lastId].Cells["hddFreeSpace"].Value = freeSpace;
                row["hddFreeSpace"] = freeSpace;

                //_form.hdd_grid.Rows[lastId].Cells["hddFreeSpaceDisplay"].Value = _form.my.size_hum_read(freeSpace);
                //_form.UpdateUI_DataGridGrid(_form.hdd_grid, "set_cell", "hddFreeSpaceDisplay", _form.my.size_hum_read(freeSpace), lastId);
                row["hddFreeSpaceDisplay"] = _form.my.size_hum_read(freeSpace);

                double percent = (Convert.ToDouble(usageSize) / Convert.ToDouble(totalSize)) * 100.0;
                //_form.hdd_grid.Rows[lastId].Cells["hddUsagePercent"].Value = string.Format("{0:0.00}", percent) + " %";
                row["hddUsagePercent"] = string.Format("{0:0.00}", percent) + " %";

                //_form.hdd_grid.Rows[lastId].Cells["hddUsagePercent"].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                if (percent < 60)
                {
                    //_form.hdd_grid.Rows[lastId].Cells["hddUsagePercent"].Style.ForeColor = Color.Green;
                }
                else if (percent >= 60 && percent < 85)
                {
                    //_form.hdd_grid.Rows[lastId].Cells["hddUsagePercent"].Style.ForeColor = Color.Orange;
                }
                else
                {
                    //剩很少空間
                    //_form.hdd_grid.Rows[lastId].Cells["hddUsagePercent"].Style.ForeColor = Color.Red;
                    //_form.hdd_grid.Rows[lastId].Cells["hddUsagePercent"].Style.Font = new Font(_form.hdd_grid.Columns["hddUsagePercent"].DefaultCellStyle.Font, FontStyle.Bold);
                    //_form.UpdateUI_DataGridGrid(_form.hdd_grid, "set_font", "hddUsagePercent", new Font(_form.hdd_grid.Columns["hddUsagePercent"].DefaultCellStyle.Font, FontStyle.Bold), lastId);
                }

                //接下來是跑 smart
                try
                {
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
                        //_form.hdd_grid.Rows[lastId].Cells["hddModel"].Value = SmartDrive.Model;
                        //_form.UpdateUI_DataGridGrid(_form.hdd_grid, "set_cell", "hddModel", SmartDrive.Model, lastId);
                        row["hddModel"] = SmartDrive.Model;

                        //Console.WriteLine(_form.my.json_encode(SmartDrive));
                        //查看有哪些屬性
                        //_form.logError(_form.my.json_encode(SmartDrive));
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
                                        //_form.hdd_grid.Rows[lastId].Cells["hddTemperature"].Value = t.ToString();
                                        row["hddTemperature"] = t.ToString();
                                    }
                                    break;

                                case "Reallocated sector count":
                                    //壞軌數
                                    //_form.hdd_grid.Rows[lastId].Cells["hddBadSectors"].Value = p.Data;
                                    row["hddBadSectors"] = p.Data;
                                    break;

                                case "Power-on hours count":
                                    //開機時數
                                    //_form.hdd_grid.Rows[lastId].Cells["hddUsageHour"].Value = p.Data;

                                    row["hddUsageHour"] = p.Data;
                                    break;

                                case "Power cycle count":
                                    //開關機次數
                                    //_form.hdd_grid.Rows[lastId].Cells["hddOnOffTimes"].Value = p.Data;
                                    row["hddOnOffTimes"] = p.Data;
                                    break;
                            }
                        }
                    }
                }
                catch
                {
                }

                dt.Rows.Add(row);
            } // Drives
            _form.updateDGVUI(_form.hdd_grid, dt);
            _form.setStatusBar("就緒", 0);
            is_running = false;
        }
    }
}