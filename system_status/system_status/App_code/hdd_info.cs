using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace system_status.App_code
{
    class hdd_info
    {
        public hdd_info()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (DriveInfo drive in drives)
            {
                //There are more attributes you can use.
                //Check the MSDN link for a complete example.
                Console.WriteLine(drive.Name);
                if (drive.IsReady) Console.WriteLine(drive.TotalSize);
            }
        }
    }
}
