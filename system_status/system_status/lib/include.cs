using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Net;
using System.Configuration;
using System.Runtime.Serialization;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Management;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;
using System.Collections.Concurrent;

namespace utility
{
    public class myinclude
    {

        public string pwd()
        {
            //return Directory.GetCurrentDirectory();
            return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
        }
        public bool is_dir(string path)
        {
            return Directory.Exists(path);
        }
        public bool is_file(string filepath)
        {
            return File.Exists(filepath);
        }
        public void unlink(string filepath)
        {
            if (is_file(filepath))
            {
                File.Delete(filepath);
            }
        }


        public bool is_string_like(string data, string find_string)
        {
            return (data.IndexOf(find_string) == -1) ? false : true;
        }
        public bool is_istring_like(string data, string find_string)
        {
            return (data.ToUpper().IndexOf(find_string.ToUpper()) == -1) ? false : true;
        }

        public string getSystemKey(string keyindex)
        {
            return ConfigurationManager.AppSettings[keyindex];
        }

        //大小寫
        public string strtoupper(string input)
        {
            return input.ToUpper();
        }
        public string strtolower(string input)
        {
            return input.ToLower();
        }


        public DateTime UnixTimeToDateTime(string text)
        {
            System.DateTime dateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
            // Add the number of seconds in UNIX timestamp to be converted.            
            dateTime = dateTime.AddSeconds(Convert.ToDouble(text));
            return dateTime;
        }
        //仿php的date
        public string time()
        {
            return strtotime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        }
        public string date()
        {
            return date("Y-m-d H:i:s", strtotime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff")));
        }
        public string date(string format)
        {
            return date(format, strtotime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff")));
        }
        public string date(string format, string unixtimestamp)
        {
            DateTime tmp = UnixTimeToDateTime(unixtimestamp);
            tmp = tmp.AddHours(+8);
            switch (format)
            {
                case "Y-m-d H:i:s":
                    return tmp.ToString("yyyy-MM-dd HH:mm:ss");
                case "Y/m/d":
                    return tmp.ToString("yyyy/MM/dd");
                case "Y/m/d H:i:s":
                    return tmp.ToString("yyyy/MM/dd HH:mm:ss");
                case "Y/m/d H:i:s.fff":
                    return tmp.ToString("yyyy/MM/dd HH:mm:ss.fff");
                case "Y-m-d_H_i_s":
                    return tmp.ToString("yyyy-MM-dd_HH_mm_ss");
                case "Y-m-d":
                    return tmp.ToString("yyyy-MM-dd");
                case "H:i:s":
                    return tmp.ToString("HH:mm:ss");
                case "Y-m-d H:i":
                    return tmp.ToString("yyyy-MM-dd HH:mm");
                case "Y_m_d_H_i_s":
                    return tmp.ToString("yyyy_MM_dd_HH_mm_ss");
                case "Y_m_d_H_i_s_fff":
                    return tmp.ToString("yyyy_MM_dd_HH_mm_ss_fff");
                case "w":
                    //回傳week, sun =0 , sat = 6, mon=1.....
                    return Convert.ToInt16(tmp.DayOfWeek).ToString();
                case "Y":
                    return tmp.ToString("yyyy");
                case "m":
                    return tmp.ToString("MM");
                case "d":
                    return tmp.ToString("dd");
                case "H":
                    return tmp.ToString("HH");
                case "i":
                    return tmp.ToString("mm");
                case "s":
                    return tmp.ToString("ss");
                case "Y-m-d H:i:s.fff":
                    return tmp.ToString("yyyy-MM-dd HH:mm:ss.fff");
                case "Y-m-d H:i:s.ffffff":
                    return tmp.ToString("yyyy-MM-dd HH:mm:ss.ffffff");
                case "H:i:s.fff":
                    return tmp.ToString("HH:mm:ss.fff");
                case "H:i:s.ffffff":
                    return tmp.ToString("HH:mm:ss.ffffff");
            }
            return "";
        }
        //strtotime 轉換成 Unix time
        public string strtotime(string value)
        {
            //create Timespan by subtracting the value provided from
            //the Unix Epoch
            TimeSpan span = (Convert.ToDateTime(value) - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());

            //return the total seconds (which is a UNIX timestamp)
            if (is_string_like(value, "."))
            {
                //有小數點               
                double sec = span.Ticks / (TimeSpan.TicksPerMillisecond / 1000.0) / 1000000.0;
                return sec.ToString();
            }
            else
            {
                return span.TotalSeconds.ToString();
            }
        }
        public string strtotime(DateTime value)
        {
            //create Timespan by subtracting the value provided from
            //the Unix Epoch
            TimeSpan span = (value - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());

            //return the total seconds (which is a UNIX timestamp)
            return span.TotalSeconds.ToString();
        }
        //javascript用的吐js資料
        public string jsAddSlashes(string value)
        {
            value = value.Replace("\\", "\\\\");
            value = value.Replace("\n", "\\n");
            value = value.Replace("\r", "\\r");
            value = value.Replace("\"", "\\\"");
            value = value.Replace("&", "\\x26");
            value = value.Replace("<", "\\x3C");
            value = value.Replace(">", "\\x3E");
            return value;
        }

        public string basename(string path)
        {
            return Path.GetFileName(path);
        }
        public string mainname(string path)
        {
            return Path.GetFileNameWithoutExtension(path);
        }
        public string subname(string path)
        {
            return Path.GetExtension(path);
        }
        public long getfilesize(string path)
        {
            FileInfo f = new FileInfo(path);
            return f.Length;
        }
        public string size_hum_read(long bytes)
        {
            string[] Suffix = { "B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
            int i = 0;
            double dblSByte = Convert.ToDouble(bytes);
            if (bytes > 1024)
                for (i = 0; (bytes / 1024) > 0; i++, bytes /= 1024)
                    dblSByte = bytes / 1024.0;
            return String.Format("{0:0.##} {1}", dblSByte, Suffix[i]);
        }

        public void mkdir(string path)
        {
            Directory.CreateDirectory(path);
        }
        public void copy(string sourceFile, string destFile)
        {
            System.IO.File.Copy(sourceFile, destFile, true);
        }
        public string dirname(string path)
        {
            return Directory.GetParent(path).FullName;
        }
        public string basedir()
        {
            //取得專案的起始位置
            return pwd();
        }
        public string getCPUId()
        {
            string cpuInfo = string.Empty;
            ManagementClass mc = new System.Management.ManagementClass("win32_processor");
            ManagementObjectCollection moc = mc.GetInstances();

            foreach (ManagementObject mo in moc)
            {
                if (cpuInfo == "")
                {
                    //Get only the first CPU's ID
                    cpuInfo = mo.Properties["processorID"].Value.ToString();
                    break;
                }
            }
            return cpuInfo;
        }
        public string system(string command)
        {
            StringBuilder sb = new StringBuilder();
            string version = System.Environment.OSVersion.VersionString;//读取操作系统版本  
            if (version.Contains("Windows"))
            {
                using (Process p = new Process())
                {
                    p.StartInfo.FileName = "cmd.exe";

                    p.StartInfo.UseShellExecute = false;//是否指定操作系统外壳进程启动程序  
                    p.StartInfo.RedirectStandardInput = true;
                    p.StartInfo.RedirectStandardOutput = true;
                    p.StartInfo.CreateNoWindow = true;//不显示dos命令行窗口  

                    p.Start();//启动cmd.exe  
                    p.StandardInput.WriteLine(command);//输入命令  
                    p.StandardInput.WriteLine("exit");//退出cmd.exe  
                    p.WaitForExit();//等待执行完了，退出cmd.exe  

                    using (StreamReader reader = p.StandardOutput)//截取输出流  
                    {
                        string line = reader.ReadLine();//每次读取一行  
                        while (!reader.EndOfStream)
                        {
                            sb.Append(line).Append("<br />");//在Web中使用<br />换行  
                            line = reader.ReadLine();
                        }
                        p.WaitForExit();//等待程序执行完退出进程  
                        p.Close();//关闭进程  
                        reader.Close();//关闭流  
                    }
                }
            }
            return sb.ToString();
        }

        public string microtime()
        {
            System.DateTime dt = DateTime.Now;
            System.DateTime UnixEpoch = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan span = dt - UnixEpoch;
            long microseconds = span.Ticks / (TimeSpan.TicksPerMillisecond / 1000);
            return microseconds.ToString();
        }
        public string trim(string input)
        {
            return input.Trim();
        }
        public void file_put_contents(string filepath, string input)
        {
            file_put_contents(filepath, s2b(input), false);
        }
        public byte[] s2b(string input)
        {
            return System.Text.Encoding.UTF8.GetBytes(input);
        }
        public void file_put_contents(string filepath, byte[] input)
        {
            file_put_contents(filepath, input, false);
        }
        public void file_put_contents(string filepath, string input, bool isFileAppend)
        {
            file_put_contents(filepath, s2b(input), isFileAppend);
        }
        public void file_put_contents(string filepath, byte[] input, bool isFileAppend)
        {

            switch (isFileAppend)
            {
                case true:
                    {
                        FileMode FM = new FileMode();
                        if (!is_file(filepath))
                        {
                            FM = FileMode.Create;
                            using (FileStream myFile = File.Open(@filepath, FM, FileAccess.Write, FileShare.Read))
                            {
                                myFile.Seek(myFile.Length, SeekOrigin.Begin);
                                myFile.Write(input, 0, input.Length);
                                myFile.Dispose();
                            }
                        }
                        else
                        {
                            FM = FileMode.Append;
                            using (FileStream myFile = File.Open(@filepath, FM, FileAccess.Write, FileShare.Read))
                            {
                                myFile.Seek(myFile.Length, SeekOrigin.Begin);
                                myFile.Write(input, 0, input.Length);
                                myFile.Dispose();
                            }
                        }


                    }
                    break;
                case false:
                    {
                        using (FileStream myFile = File.Open(@filepath, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
                        {
                            myFile.Write(input, 0, input.Length);
                            myFile.Dispose();
                        };
                    }
                    break;
            }
        }
        public JArray json_decode(string input)
        {
            input = trim(input);
            if (input.Length != 0)
            {
                if (input.Substring(1, 1) != "[")
                {
                    input = "[" + input + "]";
                    return (JArray)JsonConvert.DeserializeObject<JArray>(input);
                }
                else
                {
                    return (JArray)JsonConvert.DeserializeObject<JArray>(input);
                }
            }
            else
            {
                return null;
            }
        }
        public string json_encode(object input)
        {
            return EscapeUnicode(JsonConvert.SerializeObject(input, Formatting.None));
        }
        public string json_encode_formated(object input)
        {
            return EscapeUnicode(JsonConvert.SerializeObject(input, Formatting.Indented));
        }
        public string EscapeUnicode(string input)
        {
            StringBuilder sb = new StringBuilder(input.Length);
            foreach (char ch in input)
            {
                if (ch <= 0x7f)
                    sb.Append(ch);
                else
                    sb.AppendFormat(CultureInfo.InvariantCulture, "\\u{0:x4}", (int)ch);
            }
            return sb.ToString();
        }
        public string json_format(string input)
        {
            JArray jdod = json_decode(input);
            return EscapeUnicode(JsonConvert.SerializeObject(jdod, Formatting.Indented));
        }
        public string json_format_utf8(string input)
        {
            JArray jdod = json_decode(input);
            return JsonConvert.SerializeObject(jdod, Formatting.Indented);
        }
        public DataTable gridViewToDataTable(DataGridView grid)
        {
            //From : http://gisellemurmured.blogspot.com/2012/08/datatable-gridview.html
            DataTable dt = new DataTable();
            for (int i = 0; i < grid.Columns.Count; i++)
            {
                DataColumn dc = new DataColumn();
                //dc.ColumnName = grid.Columns[i].HeaderText;
                dc.ColumnName = grid.Columns[i].DataPropertyName;
                dt.Columns.Add(dc);
            }
            //構造行
            for (int i = 0; i < grid.Rows.Count; i++)
            {
                DataRow dr = dt.NewRow();
                for (int j = 0; j < grid.Columns.Count; j++)
                {
                    dr[j] = grid.Rows[i].Cells[j].Value;
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
        public string b2s(byte[] input)
        {
            return System.Text.Encoding.UTF8.GetString(input);
        }
        public byte[] file_get_contents(string url)
        {
            if (url.ToLower().IndexOf("http:") > -1 || url.ToLower().IndexOf("https:") > -1)
            {
                // URL                 
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                HttpWebRequest request = null;
                HttpWebResponse response = null;
                byte[] byteData = null;

                request = (HttpWebRequest)WebRequest.Create(url);
                request.Timeout = 60000;
                request.Proxy = null;
                request.UserAgent = "user_agent','Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.75 Safari/537.36";
                //request.Referer = getSystemKey("HTTP_REFERER");
                response = (HttpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                byteData = ReadStream(stream, 32765);
                response.Close();
                stream.Close();
                return byteData;
            }
            else
            {
                /*System.IO.StreamReader sr = new System.IO.StreamReader(url);
                
                string sContents = sr.ReadToEnd();
                sr.Close();
                return s2b(sContents);
                */
                /*FileStream fs = new FileStream(url, FileMode.Open);
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                fs.Close();
                return buffer;
                */
                byte[] data;

                using (var fs = new FileStream(url, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    data = ReadStream(fs, 8192);
                    /*using (StreamReader sr = new StreamReader(fs))
                    {
                        data = ReadStream(sr, 8192);
                        /*using (MemoryStream ms = new MemoryStream())
                        {
                            sr.BaseStream.CopyTo(ms);
                            data = ms.ToArray();
                            ms.Close();
                            ms.Dispose();
                        }
                        */
                    //sr.Close();
                    //sr.Dispose();

                    //};
                    fs.Close();
                };

                return data;
            }
        }
        public byte[] file_get_contents_post(string URL, ConcurrentDictionary<string, string> posts)
        {
            //NameValueCollection postParameters = new NameValueCollection();
            string[] mPostData = new string[posts.Keys.Count];
            int step = 0;
            foreach (string k in posts.Keys)
            {
                //postParameters.Add(k, posts[k]);
                mPostData[step] = post_encode_string(k) + "=" + post_encode_string(posts[k]);
                //file_put_contents("C:\\temp\\a.txt", mPostData[step], true);
                step++;

            }
            return file_get_contents_post(URL, implode("&", mPostData));
        }
        public string implode(string keyword, string[] arrays)
        {
            return string.Join(keyword, arrays);
        }
        public string implode(string keyword, List<string> arrays)
        {
            return string.Join<string>(keyword, arrays);
        }
        public string implode(string keyword, ConcurrentDictionary<int, string> arrays)
        {
            string[] tmp = new String[arrays.Keys.Count];
            int i = 0;
            foreach (int k in arrays.Keys)
            {
                tmp[i++] = arrays[k];
            }
            return string.Join(keyword, tmp);
        }
        public string implode(string keyword, Dictionary<int, string> arrays)
        {
            string[] tmp = new String[arrays.Keys.Count];
            int i = 0;
            foreach (int k in arrays.Keys)
            {
                tmp[i++] = arrays[k];
            }
            return string.Join(keyword, tmp);
        }
        public string implode(string keyword, ConcurrentDictionary<string, string> arrays)
        {
            string[] tmp = new String[arrays.Keys.Count];
            int i = 0;
            foreach (string k in arrays.Keys)
            {
                tmp[i++] = arrays[k];
            }
            return string.Join(keyword, tmp);
        }
        public string implode(string keyword, Dictionary<string, string> arrays)
        {
            string[] tmp = new String[arrays.Keys.Count];
            int i = 0;
            foreach (string k in arrays.Keys)
            {
                tmp[i++] = arrays[k];
            }
            return string.Join(keyword, tmp);
        }
        public string implode(string keyword, ArrayList arrays)
        {
            string[] tmp = new String[arrays.Count];
            for (int i = 0; i < arrays.Count; i++)
            {
                tmp[i] = arrays[i].ToString();
            }
            return string.Join(keyword, tmp);
        }
        private string post_encode_string(string value)
        {
            int limit = 2000;

            StringBuilder sb = new StringBuilder();
            int loops = value.Length / limit;

            for (int i = 0; i <= loops; i++)
            {
                if (i < loops)
                {
                    sb.Append(Uri.EscapeDataString(value.Substring(limit * i, limit)));
                }
                else
                {
                    sb.Append(Uri.EscapeDataString(value.Substring(limit * i)));
                }
            }
            return sb.ToString();
        }
        public byte[] file_get_contents_post(string url, string postData)
        {
            HttpWebRequest httpWReq =
            (HttpWebRequest)WebRequest.Create(url);

            //ASCIIEncoding encoding = new ASCIIEncoding();



            httpWReq.UserAgent = "user_agent','Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.75 Safari/537.36";
            httpWReq.Proxy = null;
            httpWReq.Timeout = 60000;
            httpWReq.Referer = null;// HttpContext.Current.Request.ServerVariables["SERVER_NAME"];
            //httpWReq.Referer = url;//getSystemKey("HTTP_REFERER");


            byte[] data = Encoding.UTF8.GetBytes(postData);
            httpWReq.Method = "POST";
            httpWReq.ContentType = "application/x-www-form-urlencoded";
            httpWReq.ContentLength = data.Length;

            using (Stream stream = httpWReq.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
                stream.Close();
            }

            HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();

            Stream streamD = response.GetResponseStream();
            byte[] byteData = ReadStream(streamD, 32767);
            response.Close();
            streamD.Close();
            return byteData;
            //byte[] responseString = new StreamReader(response.GetResponseStream()).ToArray();
        }

        private byte[] ReadStream(Stream stream, int initialLength)
        {
            if (initialLength < 1)
            {
                initialLength = 32768;
            }
            byte[] buffer = new byte[initialLength];
            int read = 0;
            int chunk;
            while ((chunk = stream.Read(buffer, read, buffer.Length - read)) > 0)
            {
                read += chunk;
                if (read == buffer.Length)
                {
                    int nextByte = stream.ReadByte();
                    if (nextByte == -1)
                    {
                        return buffer;
                    }
                    byte[] newBuffer = new byte[buffer.Length * 2];
                    Array.Copy(buffer, newBuffer, buffer.Length);
                    newBuffer[read] = (byte)nextByte;
                    buffer = newBuffer;
                    read++;
                }
            }
            byte[] bytes = new byte[read];
            Array.Copy(buffer, bytes, read);
            return bytes;
        }
        public bool in_array(string find_key, List<string> arr)
        {
            return arr.Contains(find_key);
        }
        public bool in_array(string find_key, string[] arr)
        {
            return arr.Contains(find_key);
        }
        public bool in_array(string find_key, ArrayList arr)
        {
            return arr.Contains(find_key);
        }
        public string preg_replace(String input, string[] pattern, string[] replacements)
        {
            if (replacements.Length != pattern.Length) return input; // throw new ArgumentException(“Replacement and Pattern Arrays must be balanced”);
            for (var i = 0; i < pattern.Length; i++)
            {
                input = Regex.Replace(input, pattern[i], replacements[i]);
            }
            return input;
        }
        public string preg_replace(String input, string pattern, string replacements)
        {
            string[] p = new string[1];
            p[0] = pattern;
            string[] r = new string[1];
            r[0] = replacements;
            return preg_replace(input, p, r);
        }
        public void grid_init(DataGridView g, string json_columns)
        {
            /*
             針對哪個 DataGridView 初使化，json_columns 格式
             [
                {   
                    ""taskID"":{""id"":""taskID"",""name"":""項次"",""width"":80,""display"":true,""headerAlign"":""center"",""cellAlign"":""center""}
                }
             ]
            */
            var jdLists = json_decode(json_columns);
            foreach (JObject item in jdLists[0])
            {
                //var item_dict = item.ToObject<Dictionary<string, Dictionary< string, string>>>();                
                //break;
                foreach (JProperty p in item.Properties())
                {
                    //p.Name;
                    //Console.WriteLine(item.ToString());
                    //Console.WriteLine(p.Name);
                    //Console.WriteLine(p.Value);
                    string key = p.Name;
                    string id = p.Value["id"].ToString();
                    //string key = item_dict.Keys.;
                    g.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = id,
                        Name = id,
                        HeaderText = p.Value["name"].ToString(),
                        Width = Convert.ToInt32(p.Value["width"]),
                        Visible = Convert.ToBoolean(p.Value["display"])
                    });

                    //Console.WriteLine(p.Value["headerAlign"].ToString());
                    //無法排序，標題才能置中
                    g.Columns[key].SortMode = DataGridViewColumnSortMode.NotSortable;
                    g.Columns[key].HeaderCell.Style.Font = new Font("微軟正黑體", 12); //標題字型大小
                    switch (p.Value["headerAlign"].ToString())
                    {
                        case "left":
                            g.Columns[key].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                            break;
                        case "center":
                            g.Columns[key].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            break;
                        case "right":
                            g.Columns[key].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                            break;
                    }
                    g.Columns[key].DefaultCellStyle.Font = new Font("@Fixedsys", 14); //標題字型大小
                    switch (p.Value["cellAlign"].ToString())
                    {
                        case "left":
                            g.Columns[key].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                            break;
                        case "center":
                            g.Columns[key].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            break;
                        case "right":
                            g.Columns[key].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            break;
                    }
                }
            }
        }
    }

}