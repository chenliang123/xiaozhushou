using Newtonsoft.Json.Linq;
using RueMng.util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace RueMng
{

    [DataContract]
    public class Response
    {
        [DataMember]
        public int ret { get; set; }
        [DataMember]
        public string msg { get; set; }
        [DataMember]
        public string data { get; set; }

        public Response(int ret, string msg, string data)
        {
            this.ret = ret;
            this.msg = msg;
            this.data = data;
        }
        public string toJson()
        {
            string str = new JavaScriptSerializer().Serialize(this);
            return str;
        }
    }

    [DataContract]
    public class Response2
    {
        [DataMember]
        public int ret { get; set; }
        [DataMember]
        public string msg { get; set; }
        [DataMember]
        public Object data { get; set; }

        public Response2(int ret, string msg, Object data)
        {
            this.ret = ret;
            this.msg = msg;
            this.data = data;
        }
        public string toJson()
        {
            string str = new JavaScriptSerializer().Serialize(this);
            return str;
        }
    }

    [DataContract]
    public class Grade
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string name { get; set; }
        public string toJson()
        {
            string str = new JavaScriptSerializer().Serialize(this);
            return str;
        }
    }
    class Program
    {
        [DllImport("user32.dll", EntryPoint = "ShowWindow", SetLastError = true)]
        static extern bool ShowWindow(IntPtr hWnd, uint nCmdShow);
        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        static void Main(string[] args)
        {
            Console.Title = "RueMng";
            IntPtr intptr = FindWindow("ConsoleWindowClass", "RueMng");
            if (intptr != IntPtr.Zero)
            {
                ShowWindow(intptr, 0);//隐藏这个窗口  //隐藏本dos窗体, 0: 后台执行；1:正常启动；2:最小化到任务栏；3:最大化
            }
            string strArg = "";
            foreach (string str in args)
            {
                Log.Info("args: " + str + ".");
                strArg += (strArg.Length>0?" ":"") + str;
            }
            Log.Info("strArg=" + strArg);

            int dirpos = strArg.LastIndexOf("\\");
            string dir = strArg.Substring(0, dirpos);
            string filename = strArg.Substring(dirpos+1);
            string[] szFiles = Directory.GetFiles(dir);
            string realpath = "";
            if (File.Exists(strArg))
            {
                realpath = strArg;
            }
            else
            {
                for (int i = 0; i < szFiles.Length; i++)
                {
                    string _name = szFiles[i].Replace("  ", " ").Replace("  ", " ").Replace("  ", " ").Replace("  ", " ").Replace("  ", " ");
                    int _r1 = String.Compare(_name, strArg);
                    int _r2 = String.CompareOrdinal(_name, strArg);
                    if (_r1==0)
                    {
                        realpath = szFiles[i];
                        break;
                    }

                }
            }
            Log.Info("realpath=" + realpath);
            
            try
            {
                if (realpath.Length > 0)
                {
                    DirectoryInfo di = new DirectoryInfo(realpath);
                    //Path.GetTempFileName

                    FileInfo file = new FileInfo(realpath);

                    string filepath = di.FullName;
                    Log.Info("strArg=" + strArg + ", filapath=" + filepath);

                    string filepath2 = System.Web.HttpUtility.UrlEncode(filepath);
                    string ext = Path.GetExtension(filepath).ToLower();
                    if (".ppt" == ext || ".pptx" == ext || ".bmp" == ext || ".jpg" == ext || ".jpeg" == ext || ".gif" == ext || ".png" == ext)
                    {
                    }
                    else if (".doc" == ext || ".docx" == ext)
                    {
                    }
                    else if (".pdf" == ext)
                    {
                    }
                    else if (".mp3" == ext || ".wav" == ext || ".wma" == ext)
                    {
                    }
                    else if (".mp4" == ext || ".wmv" == ext || ".mov" == ext)
                    {
                    }
                    else if (".swf" == ext)
                    {
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("当前课件文件类型(" + ext.Substring(1) + ")暂不支持!", "Warning");
                        return;
                    }

                    string url = "http://localhost:8986/SelectPPT?filepath=" + filepath2;
                    Log.Info(url);

                    string resp = HTTPReq.HttpGet(url);
                }
            }
            catch (Exception e)
            {

            }

            Application.Exit();
        }

        static void process(string[] args)
        {
            var arguments = CmdArgParser.Parse(args);
        
            //if (arguments.Has("-u"))
            //{
            //    Console.WriteLine("u:{0}", arguments.Get("-u").Next);
            //}
        
            //if (arguments.Has("-p"))
            //{
            //    Console.WriteLine("p：{0}", arguments.Get("-p").Next);
            //}

            if (arguments.Has("-f"))
            {
                string filepath = arguments.Get("-f").Next;
                string filepath2 = System.Web.HttpUtility.UrlEncode(filepath);
                string ext = Path.GetExtension(filepath);
                string ext2 = ext.ToLower();
                if (".ppt" != ext2 && ".pptx" != ext2 && ".jpg" != ext2 && ".jpeg" != ext2 && ".bmp" != ext2 && ".png" != ext2 && ".gif" != ext2)
                {
                    System.Windows.Forms.MessageBox.Show("请选择PPT文件或图片文件！", "Warning");
                    return;
                }

                string url="http://localhost:8986/SelectPPT?filepath="+filepath2;
                string resp = HTTPReq.HttpGet(url);
            }
        }
    }
}
