using Microsoft.Web.Administration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace system_status.App_code
{
    internal class iis
    {
        private Form1 _form = null;
        public bool is_running = false;
        public string last_date = "";
        public DataTable dt = new DataTable();
        private bool isGridInit = false;

        public void init(Form1 theform)
        {
            _form = theform;
            if (_form.threads.ContainsKey("iis"))
            {
                _form.threads["iis"].Abort();
                _form.threads["iis"] = null;
            }

            is_running = true;
            last_date = _form.my.time();
            _form.iis_grid.AutoGenerateColumns = false; //這啥
            _form.iis_grid.AllowUserToAddRows = false; //不能允許使用者自行調整
            _form.iis_grid.RowHeadersVisible = false; //左邊空欄移除
            _form.iis_grid.Dock = DockStyle.Fill; //自動展開到最大
            _form.iis_grid.AllowDrop = false;
            //_form.iis_grid.ReadOnly = true;

            // _form.iis_grid.Columns.Clear();
            string json_columns = @"
[
    {
        ""iis_site_id"":{""id"":""iis_site_id"",""name"":""項次"",""width"":80,""display"":true,""headerAlign"":""center"",""cellAlign"":""center""}
    },
    {
        ""iis_ApplicationPoolName"":{""id"":""iis_ApplicationPoolName"",""name"":""iis_ApplicationPoolName"",""width"":380,""display"":true,""headerAlign"":""center"",""cellAlign"":""left""}
    },
    {
        ""iis_site_name"":{""id"":""iis_site_name"",""name"":""站台名稱"",""width"":380,""display"":true,""headerAlign"":""center"",""cellAlign"":""left""}
    },
    {
        ""iis_PhysicalPath"":{""id"":""iis_PhysicalPath"",""name"":""PhysicalPath"",""width"":380,""display"":true,""headerAlign"":""center"",""cellAlign"":""left""}
    },
    {
        ""iis_Path"":{""id"":""iis_Path"",""name"":""Path"",""width"":380,""display"":true,""headerAlign"":""center"",""cellAlign"":""left""}
    },
    {
        ""iis_IsWebconfigEncrypt"":{""id"":""iis_IsWebconfigEncrypt"",""name"":""Web.config是否加密"",""width"":180,""display"":true,""headerAlign"":""center"",""cellAlign"":""center""}
    },
    {
        ""iis_customErrors"":{""id"":""iis_customErrors"",""name"":""customErrors設定"",""width"":180,""display"":true,""headerAlign"":""center"",""cellAlign"":""center""}
    },
    {
        ""iis_sessionTimeout"":{""id"":""iis_sessionTimeout"",""name"":""sessionTimeout"",""width"":130,""display"":true,""headerAlign"":""center"",""cellAlign"":""center""}
    },
    {
        ""iis_mimeMap"":{""id"":""iis_mimeMap"",""name"":""mimeMap"",""width"":250,""display"":true,""headerAlign"":""center"",""cellAlign"":""left""}
    },
    {
        ""iis_defaultDocument"":{""id"":""iis_defaultDocument"",""name"":""defaultDocument"",""width"":250,""display"":true,""headerAlign"":""center"",""cellAlign"":""left""}
    }
]
";
            //表格初始化
            if (isGridInit == false)
            {
                _form.my.grid_init(_form.iis_grid, json_columns);
                isGridInit = true;
            }
            dt = _form.my.datatable_init(json_columns);

            //allow sorting
            foreach (DataGridViewColumn column in _form.iis_grid.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.Automatic;
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            }
            _form.threads["iis"] = new Thread(() => run());
            _form.threads["iis"].Start();
        }

        private void run()
        {
            try
            {
                //int step = 0;
                DataRow row = dt.NewRow();
                //https://stackoverflow.com/questions/4593412/list-all-websites-in-iis-c-sharp
                var iisManager = new ServerManager();
                SiteCollection sites = iisManager.Sites;
                //_form.my.file_put_contents(_form.my.pwd() + "\\log\\iislog.txt", "");
                List<string> field_same_arr = new List<string>();
                foreach (Microsoft.Web.Administration.Site site in sites)
                {
                    row = dt.NewRow();
                    //_form.my.file_put_contents(_form.my.pwd() + "\\log\\iislog.txt", sites[i]., true);
                    row["iis_site_id"] = site.Id;
                    row["iis_site_name"] = site.Name;
                    //row["iis_site_name"] = site.Applications["/"].ApplicationPoolName;
                    row["iis_Path"] = site.Applications["/"].Path;
                    row["iis_PhysicalPath"] = site.Applications["/"].VirtualDirectories["/"].PhysicalPath;
                    row["iis_PhysicalPath"] = row["iis_PhysicalPath"].ToString().Replace("%SystemDrive%", _form.my.getenv("SystemDrive"));
                    //row["iis_Path"] = row["iis_Path"].ToString().Replace("/", "\\");
                    row["iis_IsWebconfigEncrypt"] = "檔案不存在";
                    row["iis_customErrors"] = "檔案不存在";
                    row["iis_sessionTimeout"] = "檔案不存在";
                    row["iis_mimeMap"] = "檔案不存在";
                    row["iis_defaultDocument"] = "檔案不存在";
                    string webconfig = row["iis_PhysicalPath"] + "\\web.config";
                    if (_form.my.is_file(webconfig))
                    {
                        string data = _form.my.b2s(_form.my.file_get_contents(webconfig));
                        var m = _form.my.explode("\n", data);
                        if (_form.my.is_istring_like(data, "EncryptedKey"))
                        {
                            row["iis_IsWebconfigEncrypt"] = "Y";
                        }
                        else
                        {
                            row["iis_IsWebconfigEncrypt"] = "N";
                        }
                        if (_form.my.is_istring_like(data, "customErrors"))
                        {
                            row["iis_customErrors"] = "Y";
                        }
                        else
                        {
                            row["iis_customErrors"] = "N";
                        }
                        if (!_form.my.is_istring_like(data, "sessionState"))
                        {
                            row["iis_sessionTimeout"] = "未設定";
                        }
                        else
                        {
                            row["iis_sessionTimeout"] = "未設定";
                            XmlDocument doc = new XmlDocument();
                            doc.Load(webconfig);
                            if (doc != null)
                            {
                                if (doc.GetElementsByTagName("sessionState").Count != 0)
                                {
                                    if (doc.GetElementsByTagName("sessionState")[0].Attributes["timeout"] != null)
                                    {
                                        row["iis_sessionTimeout"] = doc.GetElementsByTagName("sessionState")[0].Attributes["timeout"].Value;
                                    }
                                }
                            }
                        }
                        if (!_form.my.is_istring_like(data, "mimeMap"))
                        {
                            row["iis_mimeMap"] = "未設定";
                        }
                        else
                        {
                            row["iis_mimeMap"] = "未設定";
                            XmlDocument doc = new XmlDocument();
                            doc.Load(webconfig);
                            if (doc != null)
                            {
                                List<string> mimeMapArr = new List<string>();
                                for (int i = 0; i < doc.GetElementsByTagName("mimeMap").Count; i++)
                                {
                                    string d = doc.GetElementsByTagName("mimeMap")[i].Attributes["fileExtension"].Value + "|" + doc.GetElementsByTagName("mimeMap")[i].Attributes["mimeType"].Value;
                                    mimeMapArr.Add(d);
                                }
                                row["iis_mimeMap"] = _form.my.implode("|||", mimeMapArr);
                            }
                        }
                        if (!_form.my.is_istring_like(data, "defaultDocument"))
                        {
                            row["iis_defaultDocument"] = "未設定";
                        }
                        else
                        {
                            row["iis_defaultDocument"] = "未設定";
                            XmlDocument doc = new XmlDocument();
                            doc.Load(webconfig);
                            if (doc != null)
                            {
                                XmlNodeList f = doc.SelectNodes("configuration/system.webServer/defaultDocument/files/add");
                                List<string> mm = new List<string>();
                                for (int i = 0; i < f.Count; i++)
                                {
                                    XmlElement element = (XmlElement)f[i];
                                    //XmlAttribute attribute = element.GetAttributeNode("value")
                                    XmlAttributeCollection attributes = element.Attributes;
                                    foreach (XmlAttribute item in attributes)
                                    {
                                        if (item.Name == "value")
                                        {
                                            mm.Add(item.Value);
                                        }
                                    }
                                }
                                row["iis_defaultDocument"] = _form.my.implode("|||", mm);
                            }
                        }
                    }
                    string check_same_concat = row["iis_site_name"].ToString() + row["iis_PhysicalPath"].ToString() + row["iis_Path"].ToString();
                    //如果 name、PhysicalPath、Path 相同，就跳過
                    if (!_form.my.in_array(check_same_concat, field_same_arr))
                    {
                        field_same_arr.Add(check_same_concat);
                        dt.Rows.Add(row);
                    }
                    foreach (Microsoft.Web.Administration.Application app in iisManager.Sites[site.Name].Applications)
                    {
                        row = dt.NewRow();
                        row["iis_site_id"] = site.Id;
                        row["iis_ApplicationPoolName"] = app.ApplicationPoolName;
                        row["iis_site_name"] = site.Name;
                        row["iis_Path"] = app.Path;
                        if (row["iis_Path"] == null) continue;
                        row["iis_PhysicalPath"] = site.Applications[row["iis_Path"].ToString()].VirtualDirectories["/"].PhysicalPath;
                        row["iis_PhysicalPath"] = row["iis_PhysicalPath"].ToString().Replace("%SystemDrive%", Environment.GetEnvironmentVariable("SystemDrive"));

                        //if (row["iis_PhysicalPath"].ToString().Substring(row["iis_PhysicalPath"].ToString().Length - 1, 1) == "\\")
                        //{
                        //    row["iis_PhysicalPath"] = row["iis_PhysicalPath"].ToString().Substring(0, row["iis_PhysicalPath"].ToString().Length - 1);
                        //}

                        webconfig = row["iis_PhysicalPath"] + "\\web.config";
                        row["iis_IsWebconfigEncrypt"] = "檔案不存在";
                        row["iis_customErrors"] = "檔案不存在";
                        row["iis_sessionTimeout"] = "檔案不存在";
                        row["iis_mimeMap"] = "檔案不存在";
                        row["iis_defaultDocument"] = "檔案不存在";
                        if (_form.my.is_file(webconfig))
                        {
                            string data = _form.my.b2s(_form.my.file_get_contents(webconfig));
                            data = data.Replace("\r", "");
                            var m = _form.my.explode("\n", data);
                            if (_form.my.is_istring_like(data, "EncryptedKey"))
                            {
                                row["iis_IsWebconfigEncrypt"] = "Y";
                            }
                            else
                            {
                                row["iis_IsWebconfigEncrypt"] = "N";
                            }
                            if (_form.my.is_istring_like(data, "customErrors"))
                            {
                                row["iis_customErrors"] = "Y";
                            }
                            else
                            {
                                row["iis_customErrors"] = "N";
                            }
                            if (!_form.my.is_istring_like(data, "sessionState"))
                            {
                                row["iis_sessionTimeout"] = "未設定";
                            }
                            else
                            {
                                row["iis_sessionTimeout"] = "未設定";
                                XmlDocument doc = new XmlDocument();
                                doc.Load(webconfig);
                                if (doc != null)
                                {
                                    if (doc.GetElementsByTagName("sessionState").Count != 0)
                                    {
                                        if (doc.GetElementsByTagName("sessionState")[0].Attributes["timeout"] != null)
                                        {
                                            row["iis_sessionTimeout"] = doc.GetElementsByTagName("sessionState")[0].Attributes["timeout"].Value;
                                        }
                                    }
                                    //row["iis_sessionTimeout"] = (doc.GetElementsByTagName("sessionState") != null) ?
                                    //(doc.GetElementsByTagName("sessionState")[0].Attributes["timeout"] == null) ? row["iis_sessionTimeout"] : doc.GetElementsByTagName("sessionState")[0].Attributes["timeout"].ToString() :
                                    //row["iis_sessionTimeout"];
                                }
                                /*
                                for (int i = 0, max_i = m.Count(); i < max_i; i++)
                                {
                                    if (_form.my.is_istring_like(m[i], "<sessionState"))
                                    {
                                        row["iis_sessionTimeout"] = _form.my.get_between(m[i], "timeout=\"", "\"");
                                        break;
                                    }
                                }
                                */
                            }
                            if (!_form.my.is_istring_like(data, "mimeMap"))
                            {
                                row["iis_mimeMap"] = "未設定";
                            }
                            else
                            {
                                row["iis_mimeMap"] = "未設定";
                                XmlDocument doc = new XmlDocument();
                                doc.Load(webconfig);
                                if (doc != null)
                                {
                                    List<string> mimeMapArr = new List<string>();
                                    for (int i = 0; i < doc.GetElementsByTagName("mimeMap").Count; i++)
                                    {
                                        string d = doc.GetElementsByTagName("mimeMap")[i].Attributes["fileExtension"].Value + "|" + doc.GetElementsByTagName("mimeMap")[i].Attributes["mimeType"].Value;
                                        mimeMapArr.Add(d);
                                    }
                                    row["iis_mimeMap"] = _form.my.implode("|||", mimeMapArr);
                                }
                            }
                            if (!_form.my.is_istring_like(data, "defaultDocument"))
                            {
                                row["iis_defaultDocument"] = "未設定";
                            }
                            else
                            {
                                row["iis_defaultDocument"] = "未設定";
                                XmlDocument doc = new XmlDocument();
                                doc.Load(webconfig);
                                if (doc != null)
                                {
                                    XmlNodeList f = doc.SelectNodes("configuration/system.webServer/defaultDocument/files/add");
                                    List<string> mm = new List<string>();
                                    for (int i = 0; i < f.Count; i++)
                                    {
                                        XmlElement element = (XmlElement)f[i];
                                        //XmlAttribute attribute = element.GetAttributeNode("value")
                                        XmlAttributeCollection attributes = element.Attributes;
                                        foreach (XmlAttribute item in attributes)
                                        {
                                            if (item.Name == "value")
                                            {
                                                mm.Add(item.Value);
                                            }
                                        }
                                    }
                                    row["iis_defaultDocument"] = _form.my.implode("|||", mm);
                                }
                            }
                        }

                        //如果 site_name、PhysicalPath、Path 相同，就跳過
                        check_same_concat = row["iis_site_name"].ToString() + row["iis_PhysicalPath"].ToString() + row["iis_Path"].ToString();
                        if (!_form.my.in_array(check_same_concat, field_same_arr))
                        {
                            field_same_arr.Add(check_same_concat);
                            dt.Rows.Add(row);
                        }
                    }
                }
                //

                _form.updateDGVUI(_form.iis_grid, dt);
                //_form.my.file_put_contents(_form.my.pwd() + "\\log\\iislog.txt", _form.my.json_encode(dt));
                _form.setStatusBar("就緒", 0);
                is_running = false;
            }
            catch (Exception ex)
            {
                //_form.logError("IIS Error...：\r\n" + ex.Message + "\r\n" + ex.StackTrace);
                is_running = false;
            }
        }
    }
}