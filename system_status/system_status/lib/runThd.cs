using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using utility;
using System.Windows.Forms;
using System.Diagnostics;
namespace runThd_namespace
{
    class runThd
    {
        myinclude my = new myinclude();
        private Thread Thd = null;
        //private int Value = 0;

        public runThd()
        {

        }

        public void Start()
        {
            if (Thd != null)
                throw (new Exception("Thread Already Running"));
            Thd = new Thread(new ParameterizedThreadStart(Run));
            Thd.Start();
        }

        public void Stop()
        {
            if (Thd != null)
            {
                Thd.Abort();
                Thd = null;
            }
            else
            {
                //throw (new Exception("Thread Already Stopped"));
            }
        }

        private void Run(object args)
        {
            //Do something...
            Thd = null;
        }
    }
}
