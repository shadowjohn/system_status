﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace system_status.App_code
{
    class schedule
    {
        Form1 _form = null;
        public bool is_running = false;

        public void init(Form1 theform)
        {
            _form = theform;
            is_running = true;

        }
    }
}
