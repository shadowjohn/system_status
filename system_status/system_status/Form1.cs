using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using utility;
using system_status.App_code;

namespace system_status
{
    public partial class Form1 : Form
    {
        public myinclude my = null;
        location_info cLocation = null;
        hdd_info cHdd = null;        
        firewall_info cFirewall = null;
        system_service cSystemService = null;
        tasks cTasks = null;
        schedule cSchedule = null;
        static public Form theform;

        public Form1()
        {            
            my = new myinclude();            
            InitializeComponent();
            theform = this;
            cLocation = new location_info();
            cHdd = new hdd_info();//硬碟資訊
            cFirewall = new firewall_info();
            cSystemService = new system_service();
            cSchedule = new schedule();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            cHdd.hdd_init(this);
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }



}
