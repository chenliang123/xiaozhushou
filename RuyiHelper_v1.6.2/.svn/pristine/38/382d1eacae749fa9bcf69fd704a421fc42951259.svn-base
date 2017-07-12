using Edu_Simulator.util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Forms;

namespace Edu_Simulator
{
    [DataContract]
    class Global
    {
        private static Log log = new Log();
        private static System.Configuration.Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        private static System.Configuration.Configuration configSchool = null;

        public static string m_Exe = "如e小助手.exe";
        private static string m_hdip = "";
        public static string HOST = "";
        public static string url_hd = "";
        public static string url_class = "";
        public static string url_assistant = "";
        public static string url_recv = "";
        private static int m_schoolid = 0;
        private static int m_classroomid = 0;
        private static int m_classid = 0;
        private static string m_classname = "";
        private static string m_schoolname = "";
        private static int m_lessonid = 0;//经常没有
        private static int m_lessonindex = 0;//经常没有
        private static int m_courseid = 0;
        private static string m_coursename = "";

        private static int m_teacherid = 0;
        private static string m_padip = "";

        private static string m_timeon = "";//上课时间
        private static string m_timeoff = "";//下课时间
        private static int m_lessonIndex = 0;//第几节课
        private static bool m_lessonOff = true;
        private static bool m_autorun = false;
        private static bool m_debug = false;
        private static bool m_sound = false;
        private static bool m_soundCallname = false;
        private static bool m_soundReward = false;
        private static int m_hdPassiveMode = 1;//接收机被动模式

        public static string g_ClassInfoStr = "";
        public static string g_SchoolInfoStr = "";
        public static ClassInfo g_ClassInfo;
        public static StudentInfo[] g_StudentInfoArray;
        public static User[] g_TeacherArray;
        public static List<User> g_Studentlist = new List<User>();
        public static string GetAuthcode()
        {
            return "";

        }
        private static void loadSchoolConfig()
        {
            if (configSchool==null)
            {
                //指定config文件读取
                string file = Application.StartupPath + "\\App.config";
                configSchool = ConfigurationManager.OpenExeConfiguration(file);
            }
            log.Info("Global.1");
            setSchoolID(Int32.Parse(configSchool.AppSettings.Settings["schoolid"].Value));
            setClassroomID(Int32.Parse(configSchool.AppSettings.Settings["classroomid"].Value));
            setHDIP(config.AppSettings.Settings["hdip"].Value);
            if (m_hdip == null || m_hdip.Length < 10)
                throw new Exception("m_hdip Error.");

        }
        private void saveSchoolConfig(int schoolid,int classroomid)
        {
            if (configSchool == null)
            {
                //指定config文件读取
                string file = Application.StartupPath + "\\App.config";
                configSchool = ConfigurationManager.OpenExeConfiguration(file);
            }
            log.Info("Global.1");
            setSchoolID(Int32.Parse(configSchool.AppSettings.Settings["schoolid"].Value));
            setClassroomID(Int32.Parse(configSchool.AppSettings.Settings["classroomid"].Value));
           

            log.Info("Global.2");
        }
        public Global()
        {
            loadSchoolConfig();

            try
            {
                string debugMode = config.AppSettings.Settings["debug"].Value;
                if (debugMode == null || debugMode.Length == 0)
                    m_debug = false;
                else if(debugMode == "1")
                    m_debug = true;
            }
            catch (Exception e)
            {
                //...
                m_debug = false;
            }
            log.Info("Global.4");
            try
            {
                string soundMode = config.AppSettings.Settings["sound"].Value;
                if (soundMode == null || soundMode.Length == 0)
                    m_sound = false;
                else if (soundMode == "1")
                    m_sound = true;
            }
            catch (Exception e)
            {
                //...
                m_sound = false;
            }
            try
            {
                string soundCallname = config.AppSettings.Settings["soundCallname"].Value;
                if (soundCallname == null || soundCallname.Length == 0)
                    m_soundCallname = false;
                else if (soundCallname == "1")
                    m_soundCallname = true;
            }
            catch (Exception e)
            {
                //...
                m_sound = false;
            }
            try
            {
                string soundReward = config.AppSettings.Settings["soundReward"].Value;
                if (soundReward == null || soundReward.Length == 0)
                    m_soundReward = false;
                else if (soundReward == "1")
                    m_soundReward = true;
            }
            catch (Exception e)
            {
                //...
                m_sound = false;
            }

            try
            {
                string autorun = config.AppSettings.Settings["autorun"].Value;
                if (autorun == null || autorun.Length == 0)
                    m_autorun = false;
                else if (autorun == "1")
                    m_autorun = true;
            }
            catch (Exception e)
            {
                //...
                m_autorun = false;
            }
            log.Info("Global.6");

            try
            {
                string PassiveMode = configSchool.AppSettings.Settings["hdPassive"].Value;
                if (PassiveMode == null || PassiveMode.Length == 0)
                    PassiveMode = "1";
                m_hdPassiveMode = Int32.Parse(PassiveMode);
            }
            catch (Exception e)
            {
                //...
            }
            log.Info("hdPassive=" + m_hdPassiveMode);

            HOST = config.AppSettings.Settings["host"].Value;
            url_hd = "http://" + HOST + "/hd.do?";
            url_class = "http://" + HOST + "/class.do?";
            url_assistant = "http://" + HOST + "/assistant.do?";

            log.Info("Global.6");

            if (url_hd == null || url_hd.Length < 10)
                throw new Exception("url_hd Error.");
            if (url_class == null || url_class.Length < 10)
                throw new Exception("url_class Error.");
            if (url_assistant == null || url_assistant.Length < 10)
                throw new Exception("url_assistant Error.");
        }

        public static bool loadClassInfo()
        {
            log.Info("Global.loadClassInfo() now...");
            loadSchoolConfig();

            log.Info("Global.loadClassInfo() ...1...");
            Classes c = m_db.getClassByRoomid(m_classroomid);
            if (c != null)
            {
                Global.setClassID(c.id);
                Global.setClassname(c.name);
            }

            string dir = Application.StartupPath + "\\conf\\";
            string filename = m_schoolid + "-" + m_classroomid + ".conf";
            FileOper fo = new FileOper(dir,filename);

            string strClassInfo = Common.getClassInfo();
            if (strClassInfo.Length > 0)
            {
                //更新本地缓存
                fo.WriteFile(strClassInfo);

                g_ClassInfoStr = strClassInfo;
                log.Info("Global.loadClassInfo() ...2-ok..." + g_ClassInfoStr);
            }else{
                log.Info("getClassInfo error.");
                g_ClassInfoStr = fo.ReadFile();
                log.Info("Global.loadClassInfo() ...2-err..." + g_ClassInfoStr);
            }

            if (g_ClassInfoStr.Length > 0)
            {
                g_ClassInfo = JsonOper.DeserializeJsonToObject<ClassInfo>(g_ClassInfoStr);
                g_StudentInfoArray = g_ClassInfo.Data.Student;
                g_TeacherArray = g_ClassInfo.Data.Teacher;
                int nClassID = g_ClassInfo.Data.ID;
                Global.setClassID(nClassID);
                Global.setClassname(g_ClassInfo.Data.Name); 
                g_Studentlist.Clear();

                foreach (StudentInfo s in g_StudentInfoArray)
                {
                    User u = new User();
                    u.id = Int32.Parse(s.ID);
                    u.name = s.Name;
                    u.seat = s.SEAT.Replace("-", "");
                    u.type = 2;//student
                    u.schoolid = m_schoolid;
                    u.classid = nClassID;
                    u.classname = Global.getClassname();
                    g_Studentlist.Add(u);
                }

                //write to RueSqlite.db
                {
                    //更新班级教室信息
                    {
                        int nDel = m_db.delClass(m_classroomid);
                        int nAdd = m_db.addClass(nClassID, 0, Global.getClassname(), m_schoolid, m_classroomid, m_hdip);
                        int a = nAdd;
                    }
                    log.Info("Global.loadClassInfo() ...3.1...");
                    //更新学生信息
                    
                    log.Info("Global.loadClassInfo() ...3.2...");
                    if (g_Studentlist.Count > 0)
                    {
                        int nDel = m_db.delUser(nClassID, 2);
                        log.Info("Global.loadClassInfo() ...3.3...");
                        int nAdd = m_db.addUser(g_Studentlist);
                        log.Info("Global.loadClassInfo() ...3.4...");
                        int a = nDel;
                    }
                }
            }
            log.Info("Global.loadClassInfo() Over.");
            return true;
        }
        public static bool loadSchoolInfo()
        {
            log.Info("Global.loadSchoolInfo() Now.");
            loadSchoolConfig();
            
            string dir = Application.StartupPath + "\\conf\\";
            string filename = m_schoolid + "-" + m_schoolid + ".conf";
            FileOper fo = new FileOper(dir, filename);
            
            string strSchoolInfo = Common.getSchoolInfo();
            if (strSchoolInfo.Length > 0)
            {
                //更新本地缓存
                fo.WriteFile(strSchoolInfo);
                g_SchoolInfoStr = strSchoolInfo;
                log.Info("Global.loadSchoolInfo() ...2-ok..." + g_ClassInfoStr);
            }
            else
            {
                log.Info("getSchoolInfo error.");
                g_SchoolInfoStr = fo.ReadFile();
                log.Info("Global.loadClassInfo() ...2-err..." + g_ClassInfoStr);
            }

            if (strSchoolInfo.Length > 0)
            {
                SchoolInfo info = JsonOper.DeserializeJsonToObject<SchoolInfo>(strSchoolInfo);
                User[] si = info.teacherlist;
                List<User> teacherlist = new List<User>();
                foreach (User u in si)
                {
                    teacherlist.Add(u);
                }

                m_coursetable = info.coursetable;
                int nDel = m_db.delTeachers(m_schoolid);
                int nAdd = m_db.addUser(teacherlist);
                int a = nDel;
                Global.setSchoolname(info.schoolname);
                m_db.delSchool(info.schoolid);
                m_db.addSchool(info.schoolid, info.schoolname);

                string coursetimes = info.coursetime;
                m_db.delCourseTime();
                m_db.addCourseTime(coursetimes);
                log.Info("Global.loadSchoolInfo() Over.");
                g_SchoolInfoStr = strSchoolInfo;
                return true;
            }
            else
            {
                log.Info("getSchoolInfo error.");
                m_schoolname =  m_db.getSchoolname(m_schoolid);
                return false;
            }
        }

        public static bool Debug()
        {
            return m_debug;
        }
        public static bool Sound()
        {
            return m_sound;
        }
        public static bool playCallname()
        {
            return m_soundCallname;
        }
        public static bool playReward()
        {
            return m_soundReward;
        }
        public static string getHDIP()
        {
            return m_hdip;
        }
        public static void setHDIP(string v)
        {
            m_hdip = v;
            url_recv = "http://" + m_hdip + "/EduApi/hd.do?";
        }
        public static bool getAutorun()
        {
            return m_autorun;
        }
        public static void setAutorun(int v)
        {
            if (v == 1)
                m_autorun = true;
            else
                m_autorun = false;
        }
        public static void setSoundHandon(int v)
        {
            if (v == 1)
                m_sound = true;
            else
                m_sound = false;
            writeCfg("sound", v + "");
        }
        public static void setSoundCallname(int v)
        {
            if (v == 1)
                m_soundCallname = true;
            else
                m_soundCallname = false;
            writeCfg("soundCallname", v+"");
        }
        public static void setSoundReward(int v)
        {
            if (v == 1)
                m_soundReward = true;
            else
                m_soundReward = false;
            writeCfg("soundReward", v+"");
        }
        public static string getSoundConfig()
        {
            string v1 = m_sound ? "1" : "0";
            string v2 = m_soundCallname ? "1" : "0";
            string v3 = m_soundReward ? "1" : "0";

            string ret = "{\"handon\":\"P1\",\"callname\":\"P2\",\"reward\":\"P3\"}".Replace("P1", v1).Replace("P2", v2).Replace("P3", v3);
            return ret;
        }
        public static int getSchoolID()
        {
            return m_schoolid;
        }
        public static void setSchoolID(int id)
        {
            m_schoolid = id;
        }
        public static int getClassroomID()
        {
            return m_classroomid;
        }
        public static void setClassroomID(int id)
        {
            m_classroomid = id;
        }
        public static string getPadIP()
        {
            return m_padip;
        }
        public static void setPadIP(string ip)
        {
            m_padip = ip;
        }

        public static string getTimeOn()
        {
            return m_timeon;
        }
        public static void setTimeOn(string str)
        {
            m_timeon = str;
        }
        public static string getTimeOff()
        {
            return m_timeoff;
        }
        public static void setTimeOff(string str)
        {
            m_timeoff = str;
        }

        public static int getClassID()
        {
            return m_classid;
        }
        public static void setClassID(int id)
        {
            m_classid = id;
        }
        public static string getClassname()
        {
            return m_classname;
        }
        public static void setClassname(string str)
        {
            m_classname = str;
        }
        public static string getSchoolname()
        {
            return m_schoolname;
        }
        public static void setSchoolname(string str)
        {
            m_schoolname = str;
        }
        public static int getLessonID()
        {
            return m_lessonid;
        }
        public static void setLessonID(int id)
        {
            if (id == 0)
            {
                m_lessonOff = true;
            }
            else
            {
                m_lessonOff = false;
            }
            m_lessonid = id;
        }
        public static void setLessonOff()
        {
            m_lessonOff = true;
        }

        public static bool isLessonOff()
        {
            return m_lessonOff;
        }

        public static int getLessonIndex()
        {
            return m_lessonindex;
        }
        public static void setLessonIndex(int n)
        {
            m_lessonindex = n;
            CourseTime ct = m_db.getCourseTime(n);
            if(ct!=null)
            {
                m_timeon = ct.timeOn;
                m_timeoff = ct.timeOff;
            }

        }
        public static int getCourseID()
        {
            return m_courseid;
        }
        public static void setCourseID(int id)
        {
            m_courseid = id;
        }

        public static int getTeacherID()
        {
            return m_teacherid;
        }
        public static void setTeacherID(int id)
        {
            m_teacherid = id;
        }

        public static string getCourseName()
        {
            return m_coursename;
        }
        public static void setCourseName(string str)
        {
            m_coursename = str;
        }
        public static bool isHDPassive()
        {
            if (m_hdPassiveMode == 1)
                return true;
            else
                return false;
        }

        private static void writeCfg(string key,string value)
        {
            Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);            try
            {
                cfa.AppSettings.Settings[key].Value = value;
            }
            catch (Exception e)
            {
                cfa.AppSettings.Settings.Add(key, value);
            }
            cfa.Save();
            ConfigurationManager.RefreshSection("appSettings");// 刷新命名节，在下次检索它时将从磁盘重新读取它。记住应用程序要刷新节点
        }
    }
}
