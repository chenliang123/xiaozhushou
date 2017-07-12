using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace RueMng.util
{
    class Log
    {
        private static string FilePath;
        private static string fileName;
        private static int FileSize = 1024 * 1000 * 1;

        static Log()
        {
            fileName = Application.StartupPath + "\\ruemng.log";
        }

        public static void Info(string msg)
        {
            WriteFile(string.Format("{0} {1} {2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss"), "Info", msg));
        }
        public static void Error(string msg)
        {
            WriteFile(string.Format("{0} {1} {2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss"), "Error", msg));
        }
        public static void Debug(string msg)
        {
            WriteFile(string.Format("{0} {1} {2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss"), "Debug", msg));
        }
        private static void WriteFile(string msg)
        {
            try
            {
                var logStreamWriter = new StreamWriter(fileName, true, Encoding.GetEncoding("gb2312"));
                logStreamWriter.WriteLine(msg);
                logStreamWriter.Close();
                FileInfo fileInfo = new FileInfo(fileName);
                if (fileInfo.Exists && fileInfo.Length > 1024 * 1000 * 1)
                {
                    File.Delete(fileName);
                    if (!System.IO.File.Exists(fileName))
                    {
                        using (System.IO.FileStream fs = System.IO.File.Create(fileName))
                        {
                            fs.Close();
                        }
                    }
                }
            }
            catch (Exception e)
            {

            }
        }

        private static long GetFileSize(string fileName)
        {
            long strRe = 0;
            if (File.Exists(fileName))
            {
                var myFs = new FileStream(fileName, FileMode.Open);
                strRe = myFs.Length / 1024;
                myFs.Close();
                myFs.Dispose();
            }
            return strRe;
        }
    }
}
