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

namespace Update
{
    class UpdateBackground
    {
        string HOST;
        string inifileUrl;
        string zipfile;//获取升级压缩包
        string zipfilePath;
        string zipfileUrl;
        string ApplicationName = "如e小助手.exe";
        //http://www.cnblogs.com/thornfield_he/archive/2009/09/22/1571914.html

        public UpdateBackground()
        {
            IniClass iniC = new IniClass(Application.StartupPath + @"\Update.ini");
            string versionC = iniC.IniReadValue("update", "version");
            HOST = "http://" + iniC.IniReadValue("update", "host") + "/EduApi/download/";
            inifileUrl = HOST + "update.ini";
            log("versionC=" + versionC, true);
            if(versionC == null || versionC.Length==0)
            {
                return;
            }

            ////////////////////////////////////////////////
            //step1. check ini
            ////////////////////////////////////////////////
            string sDir = Application.StartupPath;
            string sPath1 = sDir + @"\updateS.ini";
            string versionS = "";
            try
            {
                WebClient wc = new WebClient();
                log("download now..." + inifileUrl);
                wc.DownloadFile(new Uri(inifileUrl), sPath1);//download ini file

                IniClass iniS = new IniClass(sPath1);
                versionS = iniS.IniReadValue("update", "version");
                zipfile = iniS.IniReadValue("update", "zipfile");
                zipfileUrl = HOST + zipfile;
                zipfilePath = sDir + "\\" + zipfile;

                log("versionS=" + versionS + ", zipfile=" + zipfile);

                File.Delete(sPath1);
                if (versionS.CompareTo(versionC) <= 0)
                {
                    log("Exit!");
                    return;
                }
            }
            catch (Exception e1)
            {
                string msg = e1.Message;
                log("download error..." + msg);
#if DEBUG
                MessageBox.Show("获取升级配置文件失败：\r\n"+msg,"Warning!!!");
#endif
                return;
            }
            

            ////////////////////////////////////////////////
            //step2. download
            ////////////////////////////////////////////////
            {
                WebClient _wc = new WebClient();
                log("download now..." + zipfileUrl);
                _wc.DownloadFile(zipfileUrl, zipfilePath);//download zip file
                log("download over..." + zipfilePath);
            }
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
            iniC.IniWriteValue("update", "version", versionS);//更新成功后将版本写入配置文件

            ////////////////////////////////////////////////
            //step5. run
            ////////////////////////////////////////////////
            log("restart RueHelper.exe now...");
            Application.Exit();//退出升级程序
            log("Process.Start("+ sDir + "\\" + ApplicationName+") now...");

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
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
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
}
