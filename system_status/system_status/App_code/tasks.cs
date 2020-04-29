using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace system_status.App_code
{
    class tasks
    {
        private Form1 _form = null;
        public tasks()
        {
        }
        public void init(Form1 theform)
        {
            _form = theform;
        }
        void play()
        {
            while (true)
            {
                //Value++;
                //Console.WriteLine("Value: {0}", Value);
                //MessageBox.Show(Form1.setting_path);
                //string data = _form.my.trim(_form.my.b2s(_form.my.file_get_contents(_form.setting_path)));
                //string[] mdata = my.explode(",", data);
                //Array.Sort<string>(mdata);
                Process[] processlist = Process.GetProcesses();
                foreach (Process theprocess in processlist)
                {
                    /*
                    for (int i = 0, max_i = mdata.Length; i < max_i; i++)
                    {
                        if (_form.my.trim(_form.my.mainname(mdata[i])) == theprocess.ProcessName)
                        {
                            try
                            {
                                theprocess.Kill();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            break;
                        }
                    }
                    */
                }
                Thread.Sleep(100);
            }
        }
    }
}
