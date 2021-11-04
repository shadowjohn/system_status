using System;
using System.Threading;
using utility;

namespace runThd_namespace
{
    internal class runThd
    {
        private myinclude my = new myinclude();
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