using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Edu_Simulator
{
    public class FileOper
    {
        private static Log log = new Log();
        private string DirName { get; set; }
        private string FileName { get; set; }
        private string FilePath { get; set; }
        public string Context { get; set; }

        public FileOper()
        {
        }
        public FileOper(string dir, string filename)
        {
            if (dir.LastIndexOf("\\") + 1 != dir.Length)
                dir += "\\";
            DirName = dir;
            FileName = filename;
            FilePath = dir + filename;
        }

        public void CreateDir()
        {
            if (!Directory.Exists(FilePath))
            {
                Directory.CreateDirectory(DirName);
            }
        }

        public void CreateFile()
        {
            if (!File.Exists(FilePath + FileName))
            {
                FileStream fs = File.Create(FilePath + FileName);
                fs.Close();
                fs.Dispose();
            }
        }

        public void DeleteFile()
        {
            if (File.Exists(FilePath + FileName))
            {
                File.Delete(FilePath + FileName);
            }
        }
        
        public void DeleteDir()
        {
            if (Directory.Exists(this.FilePath))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(this.FilePath);
                foreach (FileInfo f in dirInfo.GetFiles())
                {
                    DeleteFile();
                }
                Directory.Delete(this.FilePath);
            }
        }
        public void WriteFile()
        {
            if (CheckHasValue())
            {
                using (FileStream fs = new FileStream(FilePath + FileName, FileMode.Append))
                {
                    if (!fs.CanWrite)
                    {
                        throw new System.Security.SecurityException("read-only");
                    }
                    StreamWriter sw = new StreamWriter(fs);
                    sw.Write(Context);
                    sw.Dispose();
                    sw.Close();
                }
            }
        }

        public void ReplaceFile(string context)
        {
            using (FileStream fs = new FileStream(FilePath + FileName, FileMode.Open))
            {
                if (!fs.CanWrite)
                {
                    throw new System.Security.SecurityException("read-only");
                }
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(context);
                sw.Dispose();
                sw.Close();
            }
        }

        public bool CheckHasValue()
        {
            bool has = true;
            string rf = ReadFile();
            for (int i = 0; i < rf.Split(',').Length; i++)
            {
                string _rf = rf.Split(',')[i];
                if (_rf != "")
                {
                    if (_rf.Split(':')[0] == Context.Split(':')[0])
                    {
                        has = false;
                        rf = rf.Replace(_rf, Context);
                        ReplaceFile(rf);
                    }
                }
            }
            return has;
        }

        public string ReadFile()
        {
            string value = "";
            if (File.Exists(FilePath + FileName))
            {
                using (FileStream fs = new FileStream(FilePath + FileName, FileMode.Open))
                {
                    StreamReader sr = new StreamReader(fs);
                    value = sr.ReadToEnd();
                    sr.Dispose();
                    sr.Close();
                }
            }
            return value;
        }

        public void CopyFile(string sourcePath, string targetPath, bool isrewrite)
        {
            if (!Directory.Exists(FilePath))
            {
                Directory.CreateDirectory(DirName);
            }
            try
            {
                if (System.IO.File.Exists(targetPath))
                {
                    System.IO.File.Delete(targetPath);
                }
                System.IO.File.Copy(sourcePath, targetPath, isrewrite);
                log.Info("CopyFile over. sourcePath=" + sourcePath + ", targetPath=" + targetPath);
            }catch(Exception e1)
            {
                log.Info("copyfile exception. sourcePath=" + sourcePath + ", targetPath=" + targetPath + ", isrewrite=" + isrewrite+", " + e1.Message);
            }
            
        }
    }
}
