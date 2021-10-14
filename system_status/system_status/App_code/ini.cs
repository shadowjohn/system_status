/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IniParser;
using IniParser.Model;

namespace system_status.App_code
{
    class ini
    {
        string ini_path = "";
        Form1 theform = null;
        public void ini_init(Form1 theform)
        {
            this.theform = theform;
            ini_path = theform.my.pwd() + @"\setting.ini";
            if (!theform.my.is_file(ini_path))
            {
                //theform.my.file_put                
                theform.my.file_put_contents(ini_path,"[setting]\nNAME = ");                
            }
            //From : https://github.com/rickyah/ini-parser
            try
            {
                theform.iniData = theform.iniParser.ReadFile(ini_path);
                theform.setStatusBar("就緒", 0);
            }
            catch(Exception ex)
            {
                theform.alert("setting.ini 無法解晰...自動還原成空值");
                string t = theform.my.date("Y_m_d_H_i_s");
                theform.setStatusBar("備份 setting.ini -> setting.ini.bak_"+t,0);
                theform.my.copy(ini_path, ini_path + ".bak_"+t);
                theform.my.unlink(ini_path);
                ini_init(theform);
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
                return;
            }
            if(theform.iniData["setting"]["NAME"] == null)
            {
                theform.iniData["setting"]["NAME"] = "";
            }
            theform.textSystemName.Text = theform.iniData["setting"]["NAME"];            
        }
        public void ini_save()
        {
            theform.iniData["setting"]["NAME"] = theform.textSystemName.Text.Trim();
            theform.iniParser.WriteFile(ini_path,theform.iniData);
            theform.setStatusBarTitle("儲存完成...", 5000);
        }
    }
}
*/