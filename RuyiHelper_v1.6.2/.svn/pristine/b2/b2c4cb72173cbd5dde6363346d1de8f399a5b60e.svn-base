using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace Edu_Simulator
{
    /// <summary>
    /// 各Form的共用上传类
    /// </summary>
    class Common
    {
        private static Log log = new Log();
        private static System.Configuration.Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

        private static string schoolid = Global.getSchoolID();
        private static string classroomid = Global.getClassroomID();
        private static string url_class = Global.url_class;
        private static string url_assistant = Global.url_assistant;
        private static string KEY = "ckj75fhd9Jk$";
        public static string School_KEY="xkdje4-9fk5ue-2kd4fs";
        /// <summary>
        /// 事件上传----抢答结果上传
        /// </summary>
        /// <param name="result"></param>
        public static void uploadCompetitiveAnswer(string createtime,string xitiid, string answer, string answerStr, ArrayList _rightList)
        {
            string result = "";
            for (int i = 0; i < _rightList.Count; i++)
            {
                if (i > 0)
                {
                    result += ",";
                }
                result += _rightList[i];
            }
            uploadCompetitiveAnswer(createtime,xitiid, answer, answerStr, result);
            return;
        }
        /// <summary>
        /// 事件上传----抢答结果上传
        /// </summary>
        /// <param name="result"></param>
        public static string uploadCompetitiveAnswer(string createtime, string xitiid, string answer, string answerStr, string result, bool bAsync=true)
        {
            string CompetitiveAnswer_data = "action=CompetitiveAnswer.upload";
            CompetitiveAnswer_data += "&rid=" + xitiid;
            CompetitiveAnswer_data += "&createtime=" + createtime;
            CompetitiveAnswer_data += "&answer=" + answer;
            CompetitiveAnswer_data += "&answerStr=" + answerStr;
            CompetitiveAnswer_data += "&result=" + result;
            string ret = doPost(CompetitiveAnswer_data, bAsync);

            return ret;
        }
        

        /// <summary>
        /// 事件上传----做题结果上传
        /// </summary>
        /// <param name="result"></param>
        public static string uploadXitiResult(string xitiid, string answer, string result, bool bAsync=true)
        {
            string xitiResult_data = "action=xitiResult.upload";
            xitiResult_data += "&rid=" + xitiid;
            xitiResult_data += "&answer=" + answer;
            xitiResult_data += "&result=" + result;
            string ret = doPost(xitiResult_data, bAsync);
            return ret;
        }
        
        /// <summary>
        /// 事件上传----学生举手和老师点名
        /// </summary>
        /// <param name="result"></param>
        public static void uploadHandon(string result)
        {
            ////异步同步到云服务
            //string url_param = "action=handon.upload";
            //url_param += "&schoolid=" + schoolid;
            //url_param += "&classroomid=" + classroomid;
            //url_param += "&classid=" + Global._classid;
            //url_param += "&courseid=" + Global._courseid;
            //url_param += "&lessonid=" + Global._lessonid;
            //url_param += "&teacherid=" + Global._teacherid;
            //url_param += "&result=" + result;
            //Thread thread = new Thread(delegate()
            //{
            //    string ret = HTTPReq.HttpGet(url_assistant +  url_param);
            //    log.Info(url_assistant + url_param);
            //});
            //thread.Start();

            string handon_data = "action=handon.upload";
            handon_data += "&result=" + result;
            doPost(handon_data, true);
            return;
        }

        /// <summary>
        /// 事件上传----老师点名， 改用uploadHandon
        /// </summary>
        /// <param name="filename"></param>
        public static string uploadCallname(string handon,string result,bool bAsync=true)
        {
            string callname_data = "action=callname";
            callname_data += "&handon=" + handon;
            callname_data += "&result=" + result;
            string ret = doPost(callname_data, bAsync);
            return ret;
        }

        /// <summary>
        /// 获取班级列表
        /// </summary>
        /// <param name="schoolid"></param>
        /// <returns></returns>
        public static string getClasslist(int schoolid)
        {
            //http://apitest.radaredu.cn/EduApi/user.do?action=class.listBySchoolId&schoolid=1&schoolcode=034621ab74087bdce597f67642bf99af
            string schoolcode =MD5(School_KEY+schoolid);
            string url = Global.url_user + "action=class.listBySchoolId&schoolid="+schoolid+"&schoolcode="+schoolcode;
            string ret = HTTPReq.HttpGet(url);
            log.Info(url + ", ret: " + ret);
            try
            {
                JObject jo = (JObject)JsonConvert.DeserializeObject(ret);
                string _ret = jo["ret"].ToString();
                string _msg = jo["msg"].ToString();
                string _data = jo["data"].ToString().Replace("\r\n", "");
                string _count = jo["count"].ToString();
                ret = _data;
            }
            catch (Exception e1)
            {
                log.Error(e1.Message);
            }
            return ret;
        }

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="schoolid"></param>
        /// <returns></returns>
        public static string login(string user,string pwd)
        {
            //http://apitest.radaredu.cn/EduApi/user.do?action=class.listBySchoolId&schoolid=1&schoolcode=034621ab74087bdce597f67642bf99af
            string pwdMd5 = MD5(pwd);
            string url = Global.url_user + "action=login&user=" + user + "&pwd=" + pwd;
            string ret = HTTPReq.HttpGet(url);
            log.Info(url + ", ret: " + ret);
            try
            {
                JObject jo = (JObject)JsonConvert.DeserializeObject(ret);
                string _ret = jo["ret"].ToString();
                string _msg = jo["msg"].ToString();
                string _data = jo["data"].ToString().Replace("\r\n", "");
                string _count = jo["count"].ToString();
                ret = _data;
            }
            catch (Exception e1)
            {
                log.Error(e1.Message);
            }
            return ret;
        }
        
        /// <summary>
        /// 事件上传----老师打开PPT
        /// </summary>
        /// <param name="filename"></param>
        public static string uploadFileopenEvent(string filename,bool bAsync=true)
        {
            string fileopen_data = "action=fileopen";
            fileopen_data += "&filename=" + filename;
            fileopen_data += "&fileid=" + 0;
            string ret = doPost(fileopen_data, bAsync);
            return ret;
        }
        
        /// <summary>
        /// 文件上传----老师PPT做题或抢答
        /// </summary>
        /// <param name="filename"></param>
        public static void uploadPicture(string path)
        {
            if (true)
                return;

            int pos = path.LastIndexOf("\\");
            string fileName = path.Substring(pos + 1);

            //异步同步到云服务
            string url_param = "action=fileupload";
            url_param += "&schoolid=" + schoolid;
            url_param += "&classroomid=" + classroomid;
            url_param += "&classid=" + Global._classid;
            url_param += "&courseid=" + Global._courseid;
            url_param += "&lessonid=" + Global._lessonid;
            url_param += "&teacherid=" + Global._teacherid;
            url_param += "&filename=" + fileName;
            Thread thread = new Thread(delegate()
            {
                string url = "http://" + Global.HOST + "/EduApi/upload.do" + "?" + url_param;
                string ret = HTTPReq.UploadFile(url, path);
                log.Info(url + "," + path);
            });
            thread.Start();
            return;
        }

        /// <summary>
        /// 获取Lesson
        /// </summary>
        public static void getLesson(int index)
        {
            string data = "action=lesson.get";
            if(index>0)
                data += "&index="+index;

            string jsonText = doPost(data, false);
            if (jsonText.Length > 10)
            {
                JObject jo = (JObject)JsonConvert.DeserializeObject(jsonText);
                string _lessonid = jo["data"]["lessonid"].ToString();
                string _classid = jo["data"]["classid"].ToString();

                Global._lessonid = _lessonid;
                if (Global._classid == "0")
                    Global._classid = _classid;
            }
        }
        
        /// <summary>
        /// getClassInfo
        /// </summary>
        public static string getClassInfo()
        {
            return doPost("action=classinfo.get", false);
        }

        /// <summary>
        /// 接收机----清空之前的按键信息
        /// </summary>
        public static void HD_AsyncClear()
        {
            //异步同步到云服务
            string url = Global.url_recv;
            string url_param = "action=xiti.get";
            url_param += "&classid=" + Global._classid;
            url_param += "&courseid=" + Global._courseid;
            url_param += "&lessonid=" + Global._lessonid;

            Thread thread = new Thread(delegate()
            {
                string ret = HTTPReq.HttpGet(url + url_param);
                log.Info("HD_AsyncClear: " + url + url_param + ", ret=" + ret);
            });
            thread.Start();
            return;
        }



        public static string doPost(string data,bool bAsync)
        {
            string data0 = "classid=" + Global._classid;
            data0 += "&courseid=" + Global._courseid;
            data0 += "&lessonid=" + Global._lessonid;
            data0 += "&teacherid=" + Global._teacherid;
            data = data0 + "&" + data;

            string tm = DateTime.Now.ToString("yyyyMMddHHmmss");
            string src = tm +"_" + Global.schoolid+"_"+Global.classroomid +"_"+KEY;
            string sign = MD5(src);
            string url = Global.url_assistant + "s=" + Global.schoolid + "&r=" + Global.classroomid + "&t=" + tm + "&c=" + sign;
            string ret = "";
            string keyTemp = sign.Substring(4, 16);
            string dataEncrypt = AesEncrypt(data, keyTemp);//跟java的substring不一样
            if(bAsync)
            {
                Thread thread = new Thread(delegate()
                {
                    string retCrypt = HTTPReq.HttpPost(url, dataEncrypt);
                    ret = AesDecrypt(retCrypt, keyTemp);
                    log.Info("doPostAsync: " + url + ",data=" + data + ", ret=" + ret);
                });
                thread.Start();
            }
            else
            {
                string retCrypt = HTTPReq.HttpPost(url, dataEncrypt);
                ret = AesDecrypt(retCrypt, keyTemp);
                log.Info("doPost: " + url + ",data="+data + ", ret=" + ret);
            }
            if (ret == null)
                ret = "";
            return ret;
        }

        ////////////////////////////////////////////////////////////////////////////////////////
        public static string MD5(string sDataIn)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bytValue, bytHash;
            bytValue = System.Text.Encoding.UTF8.GetBytes(sDataIn);
            bytHash = md5.ComputeHash(bytValue);
            md5.Clear();
            string sTemp = "";
            for (int i = 0; i < bytHash.Length; i++)
            {
                sTemp += bytHash[i].ToString("X").PadLeft(2, '0');
            }
            return sTemp.ToLower();
        }  
        /// <summary>
        /// AES加密程序，与JAVA通用
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string AesEncrypt(string str, string key)
        {
            if (string.IsNullOrEmpty(str)) return null;
            Byte[] toEncryptArray = Encoding.UTF8.GetBytes(str);

            System.Security.Cryptography.RijndaelManaged rm = new System.Security.Cryptography.RijndaelManaged
            {
                Key = Encoding.UTF8.GetBytes(key),
                Mode = System.Security.Cryptography.CipherMode.ECB,
                Padding = System.Security.Cryptography.PaddingMode.PKCS7
            };

            System.Security.Cryptography.ICryptoTransform cTransform = rm.CreateEncryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>
        /// AES解密，与JAVA通用
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string AesDecrypt(string str, string key)
        {
            if (string.IsNullOrEmpty(str)) return null;
            Byte[] toEncryptArray = Convert.FromBase64String(str);

            System.Security.Cryptography.RijndaelManaged rm = new System.Security.Cryptography.RijndaelManaged
            {
                Key = Encoding.UTF8.GetBytes(key),
                Mode = System.Security.Cryptography.CipherMode.ECB,
                Padding = System.Security.Cryptography.PaddingMode.PKCS7
            };

            System.Security.Cryptography.ICryptoTransform cTransform = rm.CreateDecryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Encoding.UTF8.GetString(resultArray);
        }
    }
}
