using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Update
{
    class IniClass
    {
        public string inipath;
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        /// <summary> 
        /// 构造方法 
        /// </summary> 
        /// <param name="INIPath">文件路径</param>
        public IniClass(string INIPath)
        {
            inipath = INIPath;
        }
        /// <summary> 
        /// 写入INI文件 
        /// </summary> 
        /// <param name="Section">项目名称(如 [TypeName] )</param> 
        /// <param name="Key">键</param> 
        /// <param name="Value">值</param> 
        public void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.inipath);
        }
        /// <summary> 
        /// 读出INI文件 
        /// </summary> 
        /// <param name="Section">项目名称(如 [TypeName] )</param> 
        /// <param name="Key">键</param> 
        public string IniReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(500);
            int i = GetPrivateProfileString(Section, Key, "", temp, 500, this.inipath);
            return temp.ToString();
        }
        /// <summary> 
        /// 验证文件是否存在 
        /// </summary> 
        /// <returns>布尔值</returns> 
        public bool ExistINIFile()
        {
            return File.Exists(inipath);
        }

        /// <summary>获得相应文件名所有名称
        /// 
        /// </summary>
        /// <param name="srcPath">目录</param>
        /// <param name="sFileName">对应文件</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public List<string> fFileList(
            string srcPath)
        {
            List<string> fList = new List<string>();

            // 得到源目录的文件列表，该里面是包含文件以及目录路径的一个数组
            string[] fileList = Directory.GetFiles(srcPath);

            //'添加相同的文件
            foreach (string sItem in fileList)
            {
                if ("*.jpg *.bmp *.gif".IndexOf(sItem.Substring(sItem.Length - 3, 3)) > 0)
                {
                    fList.Add(sItem);
                }

            }

            return fList;
        }

    }
}
