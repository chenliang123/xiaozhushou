using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using RueHelper.util;

namespace RueHelper
{
    public class FileOper
    {
        private static log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string m_dirpath { get; set; }
        private string m_filename { get; set; }
        private string m_filepath { get; set; }
        public string Context { get; set; }
        public FileOper(string dir,string filename)
        {
            if (dir.LastIndexOf("\\") + 1 != dir.Length)
                dir += "\\";
            m_dirpath = dir;
            m_filename = filename;
            m_filepath = dir + filename;
        }
        public void CreateFile()
        {
            try
            {
                if (!Directory.Exists(m_dirpath))
                {
                    Directory.CreateDirectory(m_dirpath);
                    Log.Debug("CreateFile() CreateDirectory ok." + m_dirpath);
                }
                else
                {
                    Log.Debug("CreateFile() path exist.");
                }
            }
            catch (Exception e)
            {
                Log.Error("FileOper.CreateDir exception. " + e.Message);
            }

            try
            {
                if (!File.Exists(m_filepath))
                {
                    FileStream fs = File.Create(m_filepath);
                    fs.Close();
                    fs.Dispose();
                    Log.Debug("CreateFile() createfile. path=" + m_filepath);
                }
                else
                {
                    Log.Debug("CreateFile() filepath exist. path=" + m_filepath);
                }
            }
            catch (Exception e)
            {
                Log.Error("FileOper.CreateFile exception. " + e.Message);
                Log.Error(e.ToString());
            }
        }

        public void DeleteFile()
        {
            try
            {
                if (File.Exists(m_filepath))
                {
                    File.Delete(m_filepath);
                }
            }
            catch (Exception e)
            {
                Log.Error("FileOper.DeleteFile exception. " + e.Message);
            }
        }
        
        public void DeleteDir()
        {
            if (Directory.Exists(m_dirpath))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(m_dirpath);
                foreach (FileInfo f in dirInfo.GetFiles())
                {
                    DeleteFile();
                }
                Directory.Delete(m_dirpath);
            }
        }

        public void WriteFile(string content)
        {
            try{
                CreateFile();
                using (FileStream fs = new FileStream(m_filepath, FileMode.Create))
                {
                    if (!fs.CanWrite)
                    {
                        throw new System.Security.SecurityException("read-only");
                    }
                    StreamWriter sw = new StreamWriter(fs);
                    sw.Write(content);
                    sw.Dispose();
                    sw.Close();
                }
            }
            catch(Exception e){
            }
        }

        public void WriteFileAppend(string content)
        {
            try{
                CreateFile();
                using (FileStream fs = new FileStream(m_filepath, FileMode.Append))
                {
                    if (!fs.CanWrite)
                    {
                        throw new System.Security.SecurityException("read-only");
                    }
                    StreamWriter sw = new StreamWriter(fs);
                    sw.Write(content);
                    sw.Dispose();
                    sw.Close();
                }
            }
            catch(Exception e){
            }
        }

        public void ReplaceFile(string context)
        {
            using (FileStream fs = new FileStream(m_filepath, FileMode.Open))
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
        
        public string ReadFile()
        {
            string value = "";
            if (File.Exists(m_filepath))
            {
                try
                {
                    using (FileStream fs = new FileStream(m_filepath, FileMode.Open))
                    {
                        StreamReader sr = new StreamReader(fs);
                        value = sr.ReadToEnd();
                        sr.Dispose();
                        sr.Close();
                    }
                }
                catch (Exception e)
                {
                    
                }

            }
            return value;
        }

        public static void CopyFile(string sourcePath, string targetPath, bool isrewrite)
        {
            try
            {
                if (System.IO.File.Exists(targetPath))
                {
                    System.IO.File.Delete(targetPath);
                }
                System.IO.File.Copy(sourcePath, targetPath, isrewrite);
                Log.Info("CopyFile over. sourcePath=" + sourcePath + ", targetPath=" + targetPath);
            }catch(Exception e1)
            {
                Log.Info("copyfile exception. sourcePath=" + sourcePath + ", targetPath=" + targetPath + ", isrewrite=" + isrewrite+", " + e1.Message);
            }
        }
    }
}
