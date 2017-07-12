using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;

namespace Update
{
    class Update
    {
        string ApplicationName = "如e小助手.exe";
        public Update(string versionS, string zipfileUrl)
        {
            string filename = zipfileUrl.Substring(zipfileUrl.LastIndexOf('/') + 1);
            string zipfilePath = Application.StartupPath + "\\" + filename;
            string sDir = Application.StartupPath;

            ////////////////////////////////////////////////
            //step2. download
            ////////////////////////////////////////////////
            try{
                WebClient _wc = new WebClient();
                log("download now..." + zipfileUrl);
                _wc.DownloadFile(zipfileUrl, zipfilePath);//download zip file
                log("download over..." + zipfilePath);

                ////////////////////////////////////////////////
                //step3. kill & unzip
                ////////////////////////////////////////////////
                log("kill process of ruehelper.exe now...");
                KillProcess(ApplicationName);
                {
                    Thread.Sleep(1000);
                    log("unzip now...");
                    ZipClass.UnZip(zipfilePath, "", 1, null);
                    File.Delete(zipfilePath);
                }

                ////////////////////////////////////////////////
                //step4. replace
                ////////////////////////////////////////////////
                log("set update.version=" + versionS);
                IniClass iniC = new IniClass(Application.StartupPath + @"\Update.ini");
                iniC.IniWriteValue("update", "version", versionS);//更新成功后将版本写入配置文件


                ////////////////////////////////////////////////
                //step5. run
                ////////////////////////////////////////////////
                log("restart RueHelper.exe now...");
                Application.Exit();//退出升级程序
                log("Process.Start(" + sDir + "\\" + ApplicationName + ") now...");
            }
            catch (Exception e)
            {
                log("update error, exit!!! " + e.Message);
                return;
            }

            try
            {
                Process.Start(sDir + "\\" + ApplicationName);//打开主程序Main.exe
            }
            catch (Exception e)
            {
                log("Process.Start exception. " + e.Message);
            }
           
        }


        private void KillProcess(string processName)
        {
            Process[] myproc = Process.GetProcesses();
            foreach (Process item in myproc)
            {
                if (item.ProcessName == processName || (item.ProcessName+".exe") == processName)
                {
                    item.Kill();
                }
            }
        }
 
        private string HttpPost(string Url, string postDataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = Encoding.UTF8.GetByteCount(postDataStr);
            //request.CookieContainer = cookie;
            Stream myRequestStream = request.GetRequestStream();
            StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("gb2312"));
            myStreamWriter.Write(postDataStr);
            myStreamWriter.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            //response.Cookies = cookie.GetCookies(response.ResponseUri);
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }

        public string HttpGet(string Url, string postDataStr)
        {
            string url = Url + (postDataStr == "" ? "" : "?") + postDataStr;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }

        public void log(string str,bool bCreate=false)
        {
            string tm = DateTime.Now.ToString("yyyyMMdd_HHmmss\t");
            string path = Application.StartupPath + @"\update.log";
            if(bCreate && File.Exists(path))
            {
                File.Delete(path);
            }
            FileStream fs = new FileStream(path, FileMode.Append);
            StreamWriter sw = new StreamWriter(fs, Encoding.Default);
            sw.Write(tm + str + "\r\n");
            sw.Close();
            fs.Close();
        }
    }

    //{"ret":"0","msg":"\u83b7\u53d6\u6700\u65b0\u7248\u672c\u6210\u529f","data":[
    //{"schoolid":"1","id":"1","type":"1","softname":"1","version":"1.0.16.0630","date":"2016-08-05 10:43:46","content":"abc","path":"http://api.skyeducation.cn/EduApi/download/update-20160630.zip"}
    //],"count":"1"}
    
    [DataContract]
    class UpdateInfo
    {
        [DataMember]
        public UpdateItem[] updateinfolist { get; set; }
    }

    [DataContract]
    class UpdateItem
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public int type { get; set; }
        [DataMember]
        public string softname { get; set; }
        [DataMember]
        public string version { get; set; }
        [DataMember]
        public string path { get; set; }
        [DataMember]
        public string date { get; set; }
        [DataMember]
        public string content { get; set; }

        public string toJson()
        {
            string str = new JavaScriptSerializer().Serialize(this);
            return str;
        }
    }
}
