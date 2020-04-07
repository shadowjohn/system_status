using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace system_status.App_code
{
    class hdd_info
    {
        public hdd_info()
        {
        }
        public void hdd_init(Form1 theform)
        {
            //From : http://jengting.blogspot.com/2016/07/DataGridView-Sample.html
            theform.hdd_grid.AutoGenerateColumns = false;
            theform.hdd_grid.AllowUserToAddRows = false;
            theform.hdd_grid.Dock = DockStyle.Fill;
            theform.hdd_grid.Columns.Clear();
            theform.hdd_grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "hddID",
                Name = "hddID",
                HeaderText = "磁碟代號",
                Width = 100,
                Visible = true
            });
            theform.hdd_grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "hddType",
                Name = "hddType",
                HeaderText = "磁碟類型",
                Width = 100,
                Visible = true
            });
            theform.hdd_grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "hddTotalSpace",
                Name = "hddTotalSpace",
                HeaderText = "磁碟總空間",
                Width = 100,
                Visible = false
            });
            theform.hdd_grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "hddTotalSpaceDisplay",
                Name = "hddTotalSpaceDisplay",
                HeaderText = "磁碟總空間",
                Width = 100,
                Visible = true
            });
            theform.hdd_grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "hddUsageSpace",
                Name = "hddUsageSpace",
                HeaderText = "磁碟已用空間",
                Width = 100,
                Visible = false
            });
            theform.hdd_grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "hddUsageSpaceDisplay",
                Name = "hddUsageSpaceDisplay",
                HeaderText = "磁碟已用空間",
                Width = 100,
                Visible = true
            });
            theform.hdd_grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "hddFreeSpace",
                Name = "hddFreeSpace",
                HeaderText = "磁碟剩餘空間",
                Width = 100,
                Visible = false
            });
            theform.hdd_grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "hddFreeSpaceDisplay",
                Name = "hddFreeSpaceDisplay",
                HeaderText = "磁碟剩餘空間",
                Width = 100,
                Visible = true,                
            });
            theform.hdd_grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "hddFreePercent",
                Name = "hddFreePercent",
                HeaderText = "百分比",
                Width = 100,
                Visible = true,
            });
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (DriveInfo drive in drives)
            {
                //There are more attributes you can use.
                //Check the MSDN link for a complete example.
                Console.WriteLine(drive.Name);
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
                theform.hdd_grid.Rows[lastId].Cells["hddID"].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                theform.hdd_grid.Rows[lastId].Cells["hddType"].Value = driverType.ToString();
                theform.hdd_grid.Rows[lastId].Cells["hddType"].Style.Alignment = DataGridViewContentAlignment.MiddleRight;

                theform.hdd_grid.Rows[lastId].Cells["hddTotalSpace"].Value = totalSize;
                theform.hdd_grid.Rows[lastId].Cells["hddTotalSpace"].Style.Alignment = DataGridViewContentAlignment.MiddleRight;

                theform.hdd_grid.Rows[lastId].Cells["hddTotalSpaceDisplay"].Value = theform.my.size_hum_read(totalSize);
                theform.hdd_grid.Rows[lastId].Cells["hddTotalSpaceDisplay"].Style.Alignment = DataGridViewContentAlignment.MiddleRight;

                theform.hdd_grid.Rows[lastId].Cells["hddUsageSpace"].Value = usageSize;
                theform.hdd_grid.Rows[lastId].Cells["hddUsageSpace"].Style.Alignment = DataGridViewContentAlignment.MiddleRight;

                theform.hdd_grid.Rows[lastId].Cells["hddUsageSpaceDisplay"].Value = theform.my.size_hum_read(usageSize);
                theform.hdd_grid.Rows[lastId].Cells["hddUsageSpaceDisplay"].Style.Alignment = DataGridViewContentAlignment.MiddleRight;

                theform.hdd_grid.Rows[lastId].Cells["hddFreeSpace"].Value = freeSpace;
                theform.hdd_grid.Rows[lastId].Cells["hddFreeSpace"].Style.Alignment = DataGridViewContentAlignment.MiddleRight;

                theform.hdd_grid.Rows[lastId].Cells["hddFreeSpaceDisplay"].Value = theform.my.size_hum_read(freeSpace);
                theform.hdd_grid.Rows[lastId].Cells["hddFreeSpaceDisplay"].Style.Alignment = DataGridViewContentAlignment.MiddleRight;

                theform.hdd_grid.Rows[lastId].Cells["hddFreePercent"].Value = (Math.Round((Convert.ToDouble(freeSpace) / Convert.ToDouble(totalSize)) * 100.0,2)).ToString()+" %";
                theform.hdd_grid.Rows[lastId].Cells["hddFreePercent"].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
        }
    }
}
