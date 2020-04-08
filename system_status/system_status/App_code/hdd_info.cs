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

namespace system_status.App_code
{
    class hdd_info
    {
        public hdd_info()
        {
        }
        public bool is_running = false;
        public void init(Form1 theform)
        {
            is_running = true;
            theform.setStatusBar("硬碟資訊載入開始...", 0);
            //From : http://jengting.blogspot.com/2016/07/DataGridView-Sample.html
            theform.hdd_grid.AutoGenerateColumns = false; //這啥
            theform.hdd_grid.AllowUserToAddRows = false; //不能允許使用者自行調整
            theform.hdd_grid.RowHeadersVisible = false; //左邊空欄移除
            theform.hdd_grid.Dock = DockStyle.Fill; //自動展開到最大
            theform.hdd_grid.AllowDrop = false;
            theform.hdd_grid.ReadOnly = true;

            theform.hdd_grid.Columns.Clear();
            string json_columns = @"
[
    {   
        ""hddID"":{""name"":""磁碟代號"",""width"":80,""display"":true,""headerAlign"":""center"",""cellAlign"":""center""}
    },
    {   
        ""hddType"":{""name"":""磁碟類型"",""width"":100,""display"":true,""headerAlign"":""center"",""cellAlign"":""right""}
    },
    {   
        ""hddFormatType"":{""name"":""分割類型"",""width"":80,""display"":true,""headerAlign"":""center"",""cellAlign"":""right""}
    },
    {   
        ""hddTotalSpace"":{""name"":""總空間_資料"",""width"":80,""display"":false,""headerAlign"":""center"",""cellAlign"":""right""}
    },
    {   
        ""hddTotalSpaceDisplay"":{""name"":""總空間"",""width"":80,""display"":true,""headerAlign"":""center"",""cellAlign"":""right""}
    },
    {
        ""hddUsageSpace"":{""name"":""已使用空間_資料"",""width"":100,""display"":false,""headerAlign"":""center"",""cellAlign"":""right""}
    },
    {
        ""hddUsageSpaceDisplay"":{""name"":""已使用空間"",""width"":100,""display"":true,""headerAlign"":""center"",""cellAlign"":""right""}
    },
    {
        ""hddFreeSpace"":{""name"":""剩餘空間_資料"",""width"":80,""display"":false,""headerAlign"":""center"",""cellAlign"":""right""}
    },
    {
        ""hddFreeSpaceDisplay"":{""name"":""剩餘空間"",""width"":80,""display"":true,""headerAlign"":""center"",""cellAlign"":""right""}
    },
    {
        ""hddUsagePercent"":{""name"":""百分比"",""width"":80,""display"":true,""headerAlign"":""center"",""cellAlign"":""right""}
    }
]
            ";
            var jdLists = theform.my.json_decode(json_columns);
            foreach (JObject item in jdLists[0])
            {
                foreach (JProperty p in item.Properties())
                {
                    //p.Name;
                    //Console.WriteLine(item.ToString());
                    //Console.WriteLine(p.Name);
                    //Console.WriteLine(p.Value);
                    string key = p.Name;
                    theform.hdd_grid.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = key,
                        Name = key,
                        HeaderText = p.Value["name"].ToString(),
                        Width = Convert.ToInt32(p.Value["width"]),
                        Visible = Convert.ToBoolean(p.Value["display"])
                    });

                    //Console.WriteLine(p.Value["headerAlign"].ToString());
                    //無法排序，標題才能置中
                    theform.hdd_grid.Columns[key].SortMode = DataGridViewColumnSortMode.NotSortable;
                    theform.hdd_grid.Columns[key].HeaderCell.Style.Font = new Font("微軟正黑體", 16); //標題字型大小
                    switch (p.Value["headerAlign"].ToString())
                    {
                        case "left":
                            theform.hdd_grid.Columns[key].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                            break;
                        case "center":
                            theform.hdd_grid.Columns[key].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            break;
                        case "right":
                            theform.hdd_grid.Columns[key].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                            break;
                    }
                    theform.hdd_grid.Columns[key].DefaultCellStyle.Font = new Font("@Fixedsys", 14); //標題字型大小
                    switch (p.Value["cellAlign"].ToString())
                    {
                        case "left":
                            theform.hdd_grid.Columns[key].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                            break;
                        case "center":
                            theform.hdd_grid.Columns[key].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            break;
                        case "right":
                            theform.hdd_grid.Columns[key].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            break;
                    }
                }
            }


            DriveInfo[] drives = DriveInfo.GetDrives();
            int step = 0;
            int total_step = drives.Count();
            foreach (DriveInfo drive in drives)
            {
                theform.setStatusBar("硬碟資訊載入中...", Convert.ToInt32((Convert.ToDouble(step) / Convert.ToDouble(total_step)) * 100.0));
                step++;
                //There are more attributes you can use.
                //Check the MSDN link for a complete example.
                //Console.WriteLine(drive.Name);
                if (!drive.IsReady)
                {
                    continue;
                }
                string driverName = drive.Name;
                DriveType driverType = drive.DriveType;
                long freeSpace = drive.TotalFreeSpace;
                long totalSize = drive.TotalSize;
                long usageSize = totalSize - freeSpace;
                theform.hdd_grid.Rows.Add();
                int lastId = theform.hdd_grid.Rows.Count - 1;
                theform.hdd_grid.Rows[lastId].Cells["hddID"].Value = driverName;

                theform.hdd_grid.Rows[lastId].Cells["hddType"].Value = driverType.ToString();

                theform.hdd_grid.Rows[lastId].Cells["hddFormatType"].Value = drive.DriveFormat;

                theform.hdd_grid.Rows[lastId].Cells["hddTotalSpace"].Value = totalSize;

                theform.hdd_grid.Rows[lastId].Cells["hddTotalSpaceDisplay"].Value = theform.my.size_hum_read(totalSize);

                theform.hdd_grid.Rows[lastId].Cells["hddUsageSpace"].Value = usageSize;

                theform.hdd_grid.Rows[lastId].Cells["hddUsageSpaceDisplay"].Value = theform.my.size_hum_read(usageSize);


                theform.hdd_grid.Rows[lastId].Cells["hddFreeSpace"].Value = freeSpace;


                theform.hdd_grid.Rows[lastId].Cells["hddFreeSpaceDisplay"].Value = theform.my.size_hum_read(freeSpace);

                double percent = (Convert.ToDouble(usageSize) / Convert.ToDouble(totalSize)) * 100.0;
                theform.hdd_grid.Rows[lastId].Cells["hddUsagePercent"].Value = string.Format("{0:0.00}", percent) + " %";
                //theform.hdd_grid.Rows[lastId].Cells["hddUsagePercent"].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                if (percent < 60)
                {
                    theform.hdd_grid.Rows[lastId].Cells["hddUsagePercent"].Style.ForeColor = Color.Green;
                }
                else if (percent >= 60 && percent < 85)
                {
                    theform.hdd_grid.Rows[lastId].Cells["hddUsagePercent"].Style.ForeColor = Color.Orange;
                }
                else
                {
                    //剩很少空間
                    theform.hdd_grid.Rows[lastId].Cells["hddUsagePercent"].Style.ForeColor = Color.Red;
                    theform.hdd_grid.Rows[lastId].Cells["hddUsagePercent"].Style.Font = new Font(theform.hdd_grid.Columns["hddUsagePercent"].DefaultCellStyle.Font, FontStyle.Bold);
                }
            }
            theform.setStatusBar("就緒", 0);
            is_running = false;
        }
    }
}
