using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace RueHelper
{
    public struct SHFILEINFO
    {
        public IntPtr hIcon;
        public int iIcon;
        public int dwAttributes;
        public string szDisplayName;
        public string szTypeName;
    }

    public class Disk
    {
        public static uint SHGFI_ICON = 0x100;
        public static uint SHGFI_DISPLAYNAME = 0x150;
        public static uint SHGFI_TYPENAME = 0x400;
        public static uint SHGFI_ATTRIBUTES = 0x800;
        public static uint SHGFI_ICONLOCATION = 0x1000;
        public static uint SHGFI_EXETYPE = 0x1500;
        public static uint SHGFI_SYSICONINDEX = 0x4000;
        public static uint SHGFI_LINKOVERLAY = 0x8000;
        public static uint SHGFI_SELECTED = 0x10000;
        public static uint SHGFI_LARGEICON = 0x0;
        public static uint SHGFI_SMALLICON = 0x1;
        public static uint SHGFI_OPENICON = 0x2;
        public static uint SHGFI_SHELLICONSIZE = 0x4;
        public static uint SHGFI_PIDL = 0x8;
        public static uint SHGFI_USEFILEATTRIBUTES = 0x10;
        public static uint FILE_ATTRIBUTE_NORMAL = 0x80;
        public static uint LVM_FIRST = 0x1000;
        public static uint LVM_SETIMAGELIST = LVM_FIRST + 3;
        public static uint LVSIL_NORMAL = 0;
        public static uint LVSIL_SMALL = 1;

        private static IList<DirectoryInfo> GetDirectorys(string strPath)
        {
            DirectoryInfo diFliles = new DirectoryInfo(strPath);
            IList<DirectoryInfo> diArr = diFliles.GetDirectories();
            return diArr;
        }

        private static IList<FileInfo> GetFiles(string strPath)
        {
            DirectoryInfo diFliles = new DirectoryInfo(strPath);
            IList<FileInfo> files = diFliles.GetFiles();
            return files;
        }

        private static void GetDirectorys(string strPath, ref List<string> lstDirect)
        {
            DirectoryInfo diFliles = new DirectoryInfo(strPath);
            if (diFliles.Attributes != FileAttributes.Hidden)
            {
                try
                {
                    DirectoryInfo[] diArr = diFliles.GetDirectories();
                    foreach (DirectoryInfo di in diArr)
                    {
                        try
                        {
                            //if (di.Attributes== FileAttributes.)
                            lstDirect.Add(di.FullName);
                            GetDirectorys(di.FullName, ref lstDirect);
                        }
                        catch (Exception ex)
                        {
                            string sr = ex.Message;
                            continue;
                        }
                    }
                }
                catch (Exception ex)
                {
                    string s = ex.Message;
                }
            }
        }  

        private static IList<FileInfo> GetAllFiles(string strPath)
        {
            List<FileInfo> lstFiles = new List<FileInfo>();
            List<string> lstDirect = new List<string>();
            lstDirect.Add(strPath);
            DirectoryInfo diFliles = null;
            GetDirectorys(strPath, ref lstDirect);
            foreach (string str in lstDirect)
            {
                try
                {
                    diFliles = new DirectoryInfo(str);
                    lstFiles.AddRange(diFliles.GetFiles());
                }
                catch
                {
                    continue;
                }
            }
            return lstFiles;
        }

        public static string GetFileLength(string strPath)
        {
            long size = 0;
            IList<FileInfo> fi = GetAllFiles(strPath);
            foreach (FileInfo f in fi)
            {
                size += f.Length;
            }
            string sizeM = size + "字节";
            if (size >= 1024)
            {
                sizeM = Convert.ToSingle(((decimal)size / (1024 * 1024))).ToString("0.00") + "MB";
            }
            return sizeM;
        }


        public static ArrayList MergrObj(string strPath)
        {
            IList<DirectoryInfo> dir=GetDirectorys(strPath);
            IList<FileInfo> fi = GetFiles(strPath);
            ArrayList al = new ArrayList();
            foreach (DirectoryInfo d in dir)
            {
                if (d.Attributes.HasFlag(FileAttributes.System) || d.Attributes.HasFlag(FileAttributes.Hidden))
                {
                    ;
                }else
                {
                    string fullname = d.Name;// JsonOper.ReturnSplitBrString(d.Name);
                    string name = JsonOper.GetString(d.Name, 6, "..");
                    string filetype = d.Attributes.ToString();
                    string size = Disk.GetFileLength(d.FullName);
                    string lastTime = d.LastWriteTime.ToString("yyyy-MM-dd HH:mm");
                    al.Add("0|" + name + "|" + fullname + "|文件夹|" + size + "|" + lastTime);
                }
            }
            foreach (FileInfo f in fi)
            {
                string fullname = f.Name; //JsonOper.ReturnSplitBrString(f.Name);
                string name = JsonOper.GetString(f.Name, 6, "..");
                string filetype = f.Attributes.ToString();
                string size = f.Length+"";
                string lastTime = f.LastWriteTime.ToString("yyyy-MM-dd HH:mm");
                string extName = fullname.Substring(fullname.LastIndexOf(".") + 1, (fullname.Length - fullname.LastIndexOf(".") - 1));
                al.Add("1|" + name + "|" + fullname + "|" + FileType(extName) + "|" + size + "|" + lastTime);
            }
            return al;
        }

        public static ArrayList getFilelist(string strPath,string[] szType)
        {
            IList<FileInfo> fi = GetFiles(strPath);
            ArrayList al = new ArrayList();
            foreach (FileInfo f in fi)
            {
                string fullname = f.Name;
                string name = JsonOper.GetString(f.Name, 6, "..");
                string filetype = f.Attributes.ToString();
                string size = f.Length + "";
                string lastTime = f.LastWriteTime.ToString("yyyy-MM-dd HH:mm");
                string extName = fullname.Substring(fullname.LastIndexOf(".") + 1, (fullname.Length - fullname.LastIndexOf(".") - 1));
                foreach (string type in szType)
                {
                    if(fullname.IndexOf("."+type) >0)
                    {
                        al.Add("1|" + name + "|" + fullname + "|" + FileType(extName) + "|" + size + "|" + lastTime);
                        break;
                    }
                }
                
            }
            return al;
        }

        public static string FileType(string extName)
        {
            string filetype = "未知";
            if (extName == "png" || extName == "jpg" || extName == "gif" || extName == "bmp" || extName == "jpeg")
            {
                filetype = "图片";
            }
            if (extName == "ppt" || extName == "pptx")
            {
                filetype = "幻灯片";
            }
            if (extName == "pdf")
            {
                filetype = "PDF";
            }
            if (extName == "doc" || extName == "docx")
            {
                filetype = "文档";
            }
            if (extName == "xls" || extName == "xlsx")
            {
                filetype = "表格";
            }
            if (extName == "mp3" || extName == "wma" || extName == "wav")
            {
                filetype = "音频";
            }
            if (extName == "mp4" || extName == "wmv" || extName == "flv" || extName == "rmvb" || extName == "mpg" || extName == "mov" || extName == "mpeg" || extName == "rm" || extName == "ram")
            {
                filetype = "视频";
            }
            return filetype;
        }
    }
}
