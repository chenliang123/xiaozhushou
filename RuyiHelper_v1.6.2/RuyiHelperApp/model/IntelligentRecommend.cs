using RueHelper;
using RueHelper.util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Web.Script.Serialization;

namespace RueHelper.model
{

    class IntelligentRecommend
    {
        private static log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static int m_lessonid = 0;
        private static List<StudentSecond> m_secondList = new List<StudentSecond>();//本次举手时间
        private static List<StudentReward> m_rewardList = new List<StudentReward>();//本堂课奖励次数
        private static List<StudentActive> m_activeList = new List<StudentActive>();//本堂课互动次数，互动时间
        public static LessonSummary m_summary = new LessonSummary();//课堂总结
        private static List<StudentLasttimeReward> m_rewardTimeList = new List<StudentLasttimeReward>();//本堂课互动次数，互动时间
        private static List<StudentLasttimeCallname> m_callnameTimeList = new List<StudentLasttimeCallname>();//本堂课互动次数，互动时间
        private static DateTime m_QustionTime;
        private static XitiStat m_xitistat = new XitiStat();
        private static Stat m_stat = new Stat();
        //最需要，最活跃的，最沉默的，反应最快的，反应最慢的
        #region 新的课堂
        public static void InitLesson()
        {
            int currentId = Global.getLessonID();

            if (m_lessonid != currentId || currentId==0)
            {
                //FormDraw.ClearRecord();

                m_summary.clear();
                m_rewardList = null;
                m_activeList = null;
                m_rewardTimeList = null;
                m_callnameTimeList = null;
                m_xitistat = null;
                m_stat = null;

                m_lessonid = Global.getLessonID();
                m_rewardList = new List<StudentReward>();
                m_activeList = new List<StudentActive>();
                m_rewardTimeList = new List<StudentLasttimeReward>();
                m_callnameTimeList = new List<StudentLasttimeCallname>();
                m_xitistat = new XitiStat();
                m_stat = new Stat();

                string resp = Common.getLastTime();
                Log.Info("getLastTime: " + resp);
                //{"LastTime_Callname":[{"uid":3918,"uptime":"2016-07-25 17:03:07"},{"uid":3919,"uptime":"2016-06-03 15:40:16"},{"uid":3920,"uptime":"2016-06-02 14:15:31"},{"uid":3921,"uptime":"2016-04-28 15:40:28"},{"uid":3922,"uptime":"2016-05-23 16:51:48"},{"uid":3923,"uptime":"2016-05-23 17:39:24"},{"uid":3924,"uptime":"2016-09-18 19:37:06"},{"uid":3925,"uptime":"2016-09-18 19:37:06"},{"uid":3927,"uptime":"2016-05-23 16:51:48"},{"uid":3928,"uptime":"2016-05-24 17:06:37"},{"uid":3929,"uptime":"2016-05-23 17:02:52"},{"uid":3930,"uptime":"2016-05-23 16:51:48"},{"uid":3931,"uptime":"2016-05-24 16:33:35"},{"uid":3932,"uptime":"2016-09-06 12:23:01"},{"uid":3933,"uptime":"2016-06-02 19:13:02"},{"uid":3934,"uptime":"2016-06-02 14:15:32"},{"uid":3935,"uptime":"2016-06-02 14:16:25"},{"uid":3936,"uptime":"2016-06-02 14:16:23"},{"uid":3937,"uptime":"2016-09-06 12:24:03"},{"uid":3938,"uptime":"2016-09-06 12:23:00"},{"uid":3939,"uptime":"2016-05-23 16:51:48"},{"uid":3940,"uptime":"2016-05-23 17:02:52"},{"uid":3941,"uptime":"2016-08-05 16:49:28"},{"uid":3942,"uptime":"2016-09-06 12:22:42"},{"uid":3943,"uptime":"2016-05-23 21:11:17"},{"uid":3944,"uptime":"2016-06-03 16:45:21"},{"uid":3945,"uptime":"2016-06-03 16:56:01"},{"uid":3946,"uptime":"2016-06-02 14:16:49"},{"uid":3947,"uptime":"2016-05-23 21:11:17"},{"uid":3948,"uptime":"2016-09-06 12:24:00"},{"uid":3949,"uptime":"2016-09-06 12:24:02"},{"uid":3950,"uptime":"2016-08-12 18:23:26"},{"uid":3951,"uptime":"2016-06-03 15:36:47"},{"uid":3952,"uptime":"2016-06-16 22:34:02"},{"uid":3953,"uptime":"2016-06-02 14:15:36"},{"uid":3954,"uptime":"2016-09-06 12:24:01"},{"uid":4214,"uptime":"2016-09-06 12:22:59"},{"uid":4215,"uptime":"2016-09-06 12:23:03"},{"uid":4216,"uptime":"2016-06-03 16:55:55"},{"uid":4217,"uptime":"2016-05-23 17:39:09"},{"uid":4218,"uptime":"2016-05-23 17:17:21"},{"uid":4219,"uptime":"2016-09-06 12:22:07"},{"uid":4220,"uptime":"2016-05-24 17:06:37"},{"uid":4221,"uptime":"2016-05-23 16:51:48"},{"uid":4222,"uptime":"2016-06-03 16:02:50"},{"uid":4223,"uptime":"2016-05-23 17:39:09"},{"uid":4224,"uptime":"2016-06-03 16:02:54"},{"uid":4225,"uptime":"2016-09-06 12:22:08"},{"uid":4226,"uptime":"2016-06-03 16:45:22"},{"uid":4227,"uptime":"2016-05-23 17:03:31"},{"uid":4228,"uptime":"2016-05-23 16:51:48"},{"uid":4229,"uptime":"2016-06-03 15:58:03"},{"uid":4230,"uptime":"2016-05-23 17:03:31"},{"uid":4231,"uptime":"2016-05-23 16:51:48"},{"uid":4232,"uptime":"2016-05-23 16:51:48"},{"uid":4233,"uptime":"2016-05-23 17:02:52"},{"uid":4234,"uptime":"2016-05-23 16:51:48"},{"uid":4235,"uptime":"2016-06-02 14:15:41"},{"uid":4236,"uptime":"2016-05-23 16:51:48"}],"LastTime_Reward":[{"uid":3918,"uptime":"2016-07-25 17:03:08"},{"uid":3919,"uptime":"2016-06-02 14:19:21"},{"uid":3925,"uptime":"2016-09-06 12:24:05"},{"uid":3932,"uptime":"2016-09-06 12:23:02"},{"uid":3933,"uptime":"2016-06-02 19:13:05"},{"uid":3937,"uptime":"2016-09-06 12:24:03"},{"uid":3938,"uptime":"2016-09-06 12:23:01"},{"uid":3942,"uptime":"2016-09-06 12:22:43"},{"uid":3945,"uptime":"2016-06-03 16:52:36"},{"uid":3948,"uptime":"2016-09-06 12:24:00"},{"uid":3949,"uptime":"2016-09-06 12:24:02"},{"uid":3954,"uptime":"2016-09-06 12:24:01"},{"uid":4214,"uptime":"2016-09-06 12:22:59"},{"uid":4215,"uptime":"2016-09-06 12:23:03"},{"uid":4219,"uptime":"2016-09-06 12:22:07"},{"uid":4224,"uptime":"2016-06-03 16:02:55"},{"uid":4225,"uptime":"2016-09-06 12:22:08"},{"uid":4226,"uptime":"2016-06-03 16:45:26"},{"uid":4235,"uptime":"2016-06-02 14:15:47"}]}
                if(resp.Length > 0)
                {
                    StudentLasttime sl = JsonOper.DeserializeJsonToObject<StudentLasttime>(resp);
                    m_callnameTimeList = new List<StudentLasttimeCallname>(sl.LastTime_Callname);
                    m_rewardTimeList = new List<StudentLasttimeReward>(sl.LastTime_Reward);

                    DateTime tm_now = DateTime.Now;
                    

                    foreach(StudentLasttimeCallname item in m_callnameTimeList)
                    {
                        DateTime tm_last = DateTime.Parse(item.uptime);
                        TimeSpan lasttimespan = new TimeSpan(tm_last.Ticks);
                        TimeSpan nowtimespan = new TimeSpan(tm_now.Ticks);
                        TimeSpan timespan = nowtimespan.Subtract(lasttimespan).Duration();
                        int timeDiff = timespan.Days * 24 * 60 + timespan.Hours * 60 + timespan.Minutes;
                        item.minute = timeDiff;
                        Log.Info("uptime: " + item.uptime + ", "+ timeDiff);
                    }
                    foreach (StudentLasttimeReward item in m_rewardTimeList)
                    {
                        DateTime tm_last = DateTime.Parse(item.uptime);
                        TimeSpan lasttimespan = new TimeSpan(tm_last.Ticks);
                        TimeSpan nowtimespan = new TimeSpan(tm_now.Ticks);
                        TimeSpan timespan = nowtimespan.Subtract(lasttimespan).Duration();
                        int timeDiff = timespan.Days * 24 * 60 + timespan.Hours * 60 + timespan.Minutes;
                        item.minute = timeDiff;
                        Log.Info("uptime: " + item.uptime + ", " + timeDiff);
                    }
                }

                foreach(StudentInfo si in Global.g_StudentInfoArray)
                {
                    StudentActive sa = new StudentActive();
                    sa.uid = Util.toInt(si.ID);
                    sa.handon = 0;
                    sa.xiti = 0;
                    sa.second = 0;
                    //-------------------------------------------
                    addStudentActive(sa);
                }
            }
        }
        #endregion

        #region 发起新的提问
        public static void InitQuestion()
        {
            m_secondList = null;
            m_secondList = new List<StudentSecond>();

            m_QustionTime = DateTime.Now;
        }
        #endregion
        
        #region 添加举手互动记录
        public static void addHandon(int seatId)
        {
            DateTime tm_now = DateTime.Now;
            TimeSpan createtimespan = new TimeSpan(m_QustionTime.Ticks);
            TimeSpan nowtimespan = new TimeSpan(tm_now.Ticks);
            TimeSpan timespan = nowtimespan.Subtract(createtimespan).Duration();
            int timeDiff = timespan.Minutes * 60 + timespan.Seconds;

            //???Repeated???

            int uid = Global.getUidBySeat(seatId);
            StudentSecond ss = new StudentSecond();
            ss.uid = uid;
            ss.second = timeDiff;
            m_secondList.Add(ss);

            StudentActive sa = new StudentActive();
            sa.uid = uid;
            sa.handon = 1;
            sa.xiti = 0;
            sa.second = timeDiff;
            //-------------------------------------------
            addStudentActive(sa);
        }
        public static void AddHandon(string result)
        {
            m_summary.AddAct(result);//Handon
            m_stat.AddHandon(result);
        }
        #endregion

        #region 添加奖励记录
        public static void addAward(int uid,int point,string reason,string reasonid)
        {
            if (uid == 0)
                return;

            m_stat.AddReward(uid, point);
            m_summary.AddAward(uid, point);

            //清理最后一次奖励时间
            foreach (StudentLasttimeReward item in m_rewardTimeList)
            {
                if (item.uid == uid)
                {
                    item.minute = 0;
                }
            }

            StudentReward sr = new StudentReward();
            sr.uid = uid;
            sr.point = point;

            bool bFound = false;
            foreach (StudentReward _sr in m_rewardList)
            {
                if (_sr.uid == uid)
                {
                    _sr.point += point;
                    bFound = true;
                    break;
                }
            }
            if(!bFound)
            {
                m_rewardList.Add(sr);
            }
        }
        #endregion

        #region 添加点名记录 清空最后一次点名时间： 已经被点名，下次就不要推荐了
        public static void addCallname(int uid)
        {
            m_summary.AddCallname(uid);
            m_stat.AddCallname(uid);

            //清理最后一次点名时间
            foreach (StudentLasttimeCallname item in m_callnameTimeList)
            {
                if (item.uid == uid)
                {
                    item.minute = 0;
                }
            }
        }
        #endregion

        #region [v1.5]获取点名推荐名单
        public static string getRecommend()
        {
            //string buf = "{\"active\":\"P1\",\"inactive\":\"P2\",\"quick\":\"P3\",\"slow\":\"P4\",\"lasttimeMax\":\"P5\"}";
            string buf = "{\"inactive\":\"P2\",\"lasttimeMax\":\"P5\"}";
            int active = getActive();
            int quick = getQuick();
            int slow = getSlow();
            buf = buf.Replace("P1", Global.getUidBySeat(active)+"");
            buf = buf.Replace("P2", getInActive() + "");
            buf = buf.Replace("P3", Global.getUidBySeat(quick) + "");
            buf = buf.Replace("P4", Global.getUidBySeat(slow) + "");
            buf = buf.Replace("P5", getLasttimeMax() + "");
            return buf;
        }
        #endregion

        #region [v1.5]获取本堂课总结
        public static LessonSummary getLessonSummary()
        {
            //LessonSummary obj = new LessonSummary();
            //obj.resource = "4";
            //obj.actcount = "9";
            //obj.actratio = "23%";
            //obj.callnameratio = "28%";
            //obj.rewardcount = "9";
            //obj.starname1 = "张三";
            //obj.starname2 = "李四";
            //obj.starname3 = "王五";
            //obj.star1 = "18";
            //obj.star2 = "16";
            //obj.star3 = "11";
            return m_summary; 
        }
        #endregion
        //-----------------------------------------------------------

        #region 获取活跃、不活跃、反应快慢的学生
        private static int getActive()
        {
            return getActiveMax();
        }
        private static string getInActive()
        {

            string uids = "";
            if (m_activeList != null)
            {
                m_activeList.Sort((x, y) => (x.handon + x.xiti).CompareTo(y.handon + y.xiti));//从小到大
                for (int i = 0; i < 3 && i < m_activeList.Count; i++)
                    uids += (uids.Length > 0 ? "," : "") + m_activeList[i].uid;
            }
            return uids;
        }
        private static int getQuick()
        {
            int uid = 0;
            if (m_secondList != null && m_secondList.Count>0)
            {
                uid = m_secondList[0].uid;
            }
            return uid;
        }
        private static int getSlow()
        {
            int uid = 0;
            if (m_secondList != null && m_secondList.Count > 0)
            {
                uid = m_secondList[m_secondList.Count-1].uid;
            }
            return uid;
        }
        private static string getLasttimeMax()
        {
            string uids = "";
            if (m_callnameTimeList!= null)
            {
                m_callnameTimeList.Sort((x, y) => -x.minute.CompareTo(y.minute));
                for (int i = 0; i < 3 && i < m_callnameTimeList.Count; i++)
                    uids += (uids.Length > 0 ? "," : "") + m_callnameTimeList[i].uid;
            }
            return uids;
        }

        private static void addStudentActive(StudentActive sa)
        {
            bool bFound = false;
            foreach (StudentActive _sa in m_activeList)
            {
                if (_sa.uid == sa.uid)
                {
                    _sa.second += sa.second;
                    _sa.handon += sa.handon;
                    _sa.xiti += sa.xiti;
                    bFound = true;
                    break;
                }
            }
            if (!bFound)
            {
                m_activeList.Add(sa);
            }
        }
        private static int getActiveMax()
        {
            string uid = "0";
            string uids = "";
            int max = 0;
            foreach (StudentActive _sa in m_activeList)
            {
                int total = _sa.getActiveCount();
                if (total > max)
                {
                    max = total;
                }
            }

            foreach (StudentActive obj in m_activeList)
            {
                if (obj.getActiveCount() == max)
                {
                    uids += (uids.Length > 0 ? "," : "") + obj.uid;
                }
            }
            if (uids.Length > 0)
            {
                string[] szUid = uids.Split(',');
                Random rand = new Random();
                uid = szUid[rand.Next(0, szUid.Length)];
            }
            return Util.toInt(uid);
        }
        private static int getRewardMax()
        {
            string uid = "0";
            string uids = "";
            int max = 0;
            foreach (StudentReward obj in m_rewardList)
            {
                if (obj.point > max)
                {
                    max = obj.point;
                }
            }

            foreach (StudentReward obj in m_rewardList)
            {
                if (obj.point == max)
                {
                    uids += (uids.Length > 0 ? "," : "") + obj.uid;
                }
            }
            if (uids.Length > 0)
            {
                string[] szUid = uids.Split(',');
                Random rand = new Random();
                uid = szUid[rand.Next(0, szUid.Length)];
            }
            return Util.toInt(uid);
        }
        #endregion
        
        #region 习题统计
        public static void addXitiResult(string id, string answer, string result)
        {
            m_summary.AddAct(result);//1.5.3
            m_stat.AddXiti(answer,result);//2.0
            m_xitistat.addXitiResult(id, answer, result);//1.5.4
        }
        public static string getXitiStat()
        {
            return m_xitistat.toJson();
        }
        public static string getStat()
        {
            //提问 总次数，总参与率，每次提问的参与率
            //点名 点名次数，覆盖率，排序
            //奖励 奖励次数，覆盖率，排序
            Stat stat = m_stat.Clone();
            //只保留12个
            while (stat.handon.list.Count < 12)
            {
                Stat_HandonItem item = new Stat_HandonItem();
                item.index = stat.handon.list.Count + 1;
                stat.handon.list.Add(item);
            }
            while (stat.xiti.list.Count < 12)
            {
                Stat_XitiItem item = new Stat_XitiItem();
                item.index = stat.xiti.list.Count + 1;
                stat.xiti.list.Add(item);
            }
            return stat.toJson();
        }
        #endregion
    }

    #region 学生课堂统计
    [DataContract]
    public class StudentHandon
    {
        [DataMember]
        public int uid { get; set; }
        [DataMember]
        public int count { get; set; }
    }

    public class StudentSecond
    {
        public int uid;
        public int second;
    }

    [DataContract]
    public class StudentCallname
    {
        [DataMember]
         public int uid { get; set; }
        [DataMember]
        public int point { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string imageurl { get; set; }
    }

    [DataContract]
    public class StudentReward
    {
        [DataMember]
         public int uid { get; set; }
        [DataMember]
        public int point { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string imageurl { get; set; }
    }
    
    public class StudentActive
    {
        public int uid;
        public int handon;//累计互动次数
        public int xiti;//累计互动次数
        public int second;//累计互动时间
        public int getActiveCount()
        {
            return handon + xiti;
        }
    }

    [DataContract]
    public class StudentLasttimeCallname
    {
        [DataMember]
        public int uid { get; set; }
        [DataMember]
        public string uptime { get; set; }
        [DataMember]
        public int minute { get; set; }
        public string toJson()
        {
            string ret = new JavaScriptSerializer().Serialize(this);
            return ret;
        }
    }

    [DataContract]
    public class StudentLasttimeReward
    {
        [DataMember]
        public int uid { get; set; }
        [DataMember]
        public string uptime { get; set; }
        [DataMember]
        public int minute { get; set; }
        public string toJson()
        {
            string ret = new JavaScriptSerializer().Serialize(this);
            return ret;
        }
    }

    [DataContract]
    public class StudentLasttime
    {
        [DataMember]
        public StudentLasttimeReward[] LastTime_Reward { get; set; }
        [DataMember]
        public StudentLasttimeCallname[] LastTime_Callname { get; set; }
        public string toJson()
        {
            string ret = new JavaScriptSerializer().Serialize(this);
            return ret;
        }
    }

    #endregion

    #region 课堂总结 LessonSummary
    [DataContract]
    public class LessonSummary
    {
        private static log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private HashSet<string> fileset = new HashSet<string>();
        private HashSet<int> stuSet = new HashSet<int>();
        private HashSet<int> callnameSet = new HashSet<int>();
        private Hashtable actMap = new Hashtable();
        private Hashtable rewardMap = new Hashtable();

        public int resource { get; set; }//课堂资源
        public int stuActcount { get; set; }//互动次数
        public int actcount { get; set; }//互动次数
        public string actratio { get; set; }//互动参与率
        public string callnameratio { get; set; }//点名覆盖率
        public int rewardcount { get; set; }//奖励次数
        public string starname1 { get; set; }
        public string starname2 { get; set; }
        public string starname3 { get; set; }
        public int star1 { get; set; }
        public int star2 { get; set; }
        public int star3 { get; set; }
        public int type;
        public string access1 { get; set; }
        public string access2 { get; set; }
        public string access3 { get; set; }
        public string access4 { get; set; }
        public LessonSummary()
        {
            actratio = "0%";
            callnameratio = "0%";
            starname1 = "";
            starname2 = "";
            starname3 = "";
            type = Global.getSummaryType();
            access1 = "教学设计";
            access2 = "目标达成";
            access3 = "课堂气氛";
            access4 = "师生互动";
        }
        public string toJson()
        {
            {
                //先定义两个一维数组，分别用来存储Key和Value
                string[] keyArray = new string[rewardMap.Count];
                int[] valueArray = new int[rewardMap.Count];

                //将HashTable中的Key和Value分别赋给上面两个数组
                rewardMap.Keys.CopyTo(keyArray, 0);
                rewardMap.Values.CopyTo(valueArray, 0);

                //下面就是对Value进行排序，当然需要按排序结果将Keys的值也作对应的排列
                //Sort默认是升序排序，如果想用降序排序请在Sort排序后使用Array.Reverse()进行反向排序
                Array.Sort(valueArray, keyArray);

                if(valueArray.Length>=3)
                {
                    Array.Reverse(valueArray);
                    Array.Reverse(keyArray);
                    star1 = valueArray[0];
                    star2 = valueArray[1];
                    star3 = valueArray[2];
                    starname1 = keyArray[0];
                    starname2 = keyArray[1];
                    starname3 = keyArray[2];
                }
                else if (valueArray.Length == 2)
                {
                    star1 = valueArray[1];
                    star2 = valueArray[0];
                    starname1 = keyArray[1];
                    starname2 = keyArray[0];
                }
                else if (valueArray.Length == 1)
                {
                    star1 = valueArray[0];
                    starname1 = keyArray[0];
                }

                if(starname1.Length > 0)
                {
                    int uid = Util.toInt(starname1.Substring(1));
                    starname1 = Global.getUsernameById(uid);
                }
                if (starname2.Length > 0)
                {
                    int uid = Util.toInt(starname2.Substring(1));
                    starname2 = Global.getUsernameById(uid);
                }
                if (starname3.Length > 0)
                {
                    int uid = Util.toInt(starname3.Substring(1));
                    starname3 = Global.getUsernameById(uid);
                }
            }

            if(Global.g_Studentlist!=null && Global.g_Studentlist.Count > 0)
            {
                double r1 = (double)callnameSet.Count / Global.g_Studentlist.Count;
                if (r1 > 1)
                    r1 = 1;
                callnameratio = Math.Round(r1,0)*100 + "%";
                Log.Info("LessonSummary: nCallname=" + callnameSet.Count + ", nStu=" + Global.g_Studentlist.Count + ", r =" + r1 + ", callnameratio=" + callnameratio);
                
                //double r2 = (double)stuSet.Count / Global.g_Studentlist.Count;

                double r2 = 0;
                if (actcount > 0)
                    r2 = (double)stuActcount / (actcount * Global.g_Studentlist.Count);

                if (r2 > 1)
                    r2 = 1;
                actratio = Math.Round(r2*100,0) + "%";
                Log.Info("LessonSummary: nAct=" + stuSet.Count + ", nStu=" + Global.g_Studentlist.Count + ", r =" + r2 + ", actratio=" + actratio);
            }

            string ret = new JavaScriptSerializer().Serialize(this);
            return ret;
        }
        public void clear()
        {
            fileset.Clear();
            actMap.Clear();
            rewardMap.Clear();

            resource = 0;
            actcount = 0;
            stuActcount = 0;
            actratio = "0%";
            callnameratio = "0%";
            rewardcount = 0;
            starname1 = "";
            starname2 = "";
            starname3 = "";
            star1 = 0;
            star2 = 0;
            star3 = 0;
        }

        public void AddResource(string name)
        {
            if (!fileset.Contains(name))
            {
                resource++;
                fileset.Add(name);
            }
        }
        public void AddAct(string result)
        {
            actcount++;
            string[] szResult = result.Split(',');
            for(int i=0; i<szResult.Length; i++)
            {
                string val = szResult[i];
                if(val.Length < 1)
                    continue;
                string[] szItem = val.Split(':');
                if(szItem.Length > 1)
                {
                    string _uid = szItem[0];
                    int nUid = Util.toInt(_uid);
                    AddStudentAct(nUid);

                    stuActcount++;
                }
            }
        }

        public void AddCallname(int uid)
        {
            if (uid < 100)
                return;

            if (!callnameSet.Contains(uid))
            {
                callnameSet.Add(uid);
            }
        }
        public void AddStudentAct(int uid)
        {
            if (!stuSet.Contains(uid))
            {
                stuSet.Add(uid);
            }
        }
        public void AddAward(int uid, int point) 
        {
            if (uid == 0)
                return;
            rewardcount++; 
            string key = "s"+uid;
            if(rewardMap.Contains("s"+uid))
            {
                int val = (int)rewardMap[key];
                rewardMap[key] = val + point;
            }
            else
            {
                rewardMap.Add(key, point);
            }
        }
    }
    #endregion

    #region 习题统计 XitiStat
    [DataContract]
    public class XitiStat
    {
        [DataMember]
        public int count { get; set; }
        [DataMember]
        public int countWithAnswer { get; set; }
        [DataMember]
        public List<XitiStat_Student> listByStudent { get; set; }
        [DataMember]
        public List<XitiStat_Xiti> listByXiti { get; set; }
        [DataMember]
        public double ratioAnswer { get; set; }
        [DataMember]
        public double ratioAnswerRight { get; set; }

        public XitiStat()
        {
            listByStudent = new List<XitiStat_Student>();
            listByXiti = new List<XitiStat_Xiti>();

            //初始化listByStudent
            foreach(User u in Global.g_Studentlist)
            {
                XitiStat_Student stu = new XitiStat_Student();
                stu.name = u.name;
                stu.pinyin = Util.GetPinyin(u.name);
                stu.seat = u.seat.Replace("-", "");
                stu.id = u.id;
                listByStudent.Add(stu);
            }
            listByStudent.Sort((x, y) => x.pinyin.CompareTo(y.pinyin));

        }
        
        public void addXitiResult(string id, string answer, string result)
        {
            count++;
            if (answer.Length > 0)
                countWithAnswer++;

            string[] szResult = result.Split(',');
            
            XitiStat_Xiti xiti = new XitiStat_Xiti(id, answer, result);
            listByXiti.Add(xiti);

            foreach (XitiStat_Student stu in listByStudent)
            {
                bool bFound = false;
                XitiStat_Answer myAnswer = new XitiStat_Answer();
                myAnswer.rid = id;
                myAnswer.rightanswer = answer;
                myAnswer.answer = "";
                for (int i = 0; i < szResult.Length; i++)
                {
                    string[] szItem = szResult[i].Split(':');
                    int _uid = Util.toInt(szItem[0]);
                    string _answer = szItem[1];

                    if (stu.id == _uid)
                    {
                        myAnswer.answer = _answer;//我的答案
                        stu.countAnswer++;
                        if (_answer == answer)
                            stu.countRightAnswer++;

                        bFound = true;
                        break;
                    }
                }
                stu.answerlist.Add(myAnswer);                
            }
        }

        public string toJson()
        {
            foreach (XitiStat_Student stu in listByStudent)
            {
                if (count > 0)
                    stu.ratioAnswer = Math.Round(stu.countAnswer * 100 / count, 2);
                if (countWithAnswer > 0)
                    stu.ratioAnswerRight = Math.Round(stu.countRightAnswer * 100 / countWithAnswer, 2);
            }

            double sumAnswer = 0;
            double sumAnswerRight = 0;
            foreach(XitiStat_Xiti xiti in listByXiti)
            {
                sumAnswer += xiti.countResult;
                sumAnswerRight += xiti.countOK;
            }
            if (count > 0)
                ratioAnswer = Math.Round(sumAnswer*100/(count*Global.g_Studentlist.Count),2);
            if (countWithAnswer>0)
                ratioAnswerRight = Math.Round(sumAnswerRight*100 / (countWithAnswer * Global.g_Studentlist.Count),2);

            string ret = new JavaScriptSerializer().Serialize(this);
            return ret;
        }
    }
    [DataContract]
    public class XitiStat_Student
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string seat { get; set; }
        [DataMember]
        public double countAnswer { get; set; }
        [DataMember]
        public double countRightAnswer { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string pinyin { get; set; }
        [DataMember]
        public double ratioAnswer { get; set; }
        [DataMember]
        public double ratioAnswerRight { get; set; }
        [DataMember]
        public List<XitiStat_Answer> answerlist { get; set; }
        public XitiStat_Student()
        {
            answerlist = new List<XitiStat_Answer>();
        }
        public string toJson()
        {
            string ret = new JavaScriptSerializer().Serialize(this);
            return ret;
        }

    }
    [DataContract]
    public class XitiStat_Answer
    {
        [DataMember]
        public string rid { get; set; }
        [DataMember]
        public string answer { get; set; }
        [DataMember]
        public string rightanswer { get; set; }
    }
    [DataContract]
    public class XitiStat_Xiti
    {
        [DataMember]
        public string rid { get; set; }
        [DataMember]
        public string answer { get; set; }
        [DataMember]
        public int countResult { get; set; }
        [DataMember]
        public int countNoResult { get; set; }
        [DataMember]
        public int countOK { get; set; }
        [DataMember]
        public int countWrong { get; set; }
        [DataMember]
        public List<StudentInfo> studentWithoutResult { get; set; }
        [DataMember]
        public List<StudentInfo> studentWithWrongResult { get; set; }
        [DataMember]
        public double ratioAnswer { get; set; }
        [DataMember]
        public double ratioAnswerRight { get; set; }

        public XitiStat_Xiti(string _id,string _answer,string _result)
        {
            studentWithoutResult = new List<StudentInfo>();
            studentWithWrongResult = new List<StudentInfo>();
            rid = _id;
            answer = _answer;
             
            if(Global.g_Studentlist==null || Global.g_Studentlist.Count==0)
                return;

            string[] szResult = _result.Split(',');
            countNoResult = Global.g_Studentlist.Count - szResult.Length;
            countResult = szResult.Length;

            foreach (StudentInfo si in Global.g_StudentInfoArray)
            {
                bool bFound = false;
                for (int i = 0; i < szResult.Length; i++)
                {
                    string[] szItem = szResult[i].Split(':');

                    string uid = szItem[0];
                    string myanswer = szItem[1];
                    if (si.ID == uid)
                    {
                        if(myanswer == answer)
                        {
                            countOK++;
                        }else{
                            studentWithWrongResult.Add(si);
                            countWrong++;
                        }
                        bFound = true;
                        break;
                    }
                }
                if(!bFound)
                {
                    studentWithoutResult.Add(si);
                }
            }
            ratioAnswer = (double)countResult/Global.g_Studentlist.Count;
            ratioAnswer = Math.Round(ratioAnswer*100, 2);
            if (_answer.Length > 0)
                ratioAnswerRight = (double)countOK / Global.g_Studentlist.Count;
            ratioAnswerRight = Math.Round(ratioAnswerRight*100, 2);
            
        }


        public string toJson()
        {
            string ret = new JavaScriptSerializer().Serialize(this);
            return ret;
        }
    }
    #endregion

    #region 统计
    [DataContract]
    public class Stat
    {
        private static log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [DataMember]
        public Stat_Handon handon { get; set; }
        [DataMember]
        public Stat_Xiti xiti { get; set; }
        [DataMember]
        public Stat_Reward reward { get; set; }
        [DataMember]
        public Stat_Callname callname { get; set; }
        public Stat Clone()
        {
            Stat obj = new Stat();
            obj.handon = this.handon.Clone();
            obj.xiti = this.xiti.Clone();
            obj.reward = this.reward;
            obj.callname = this.callname;
            return obj;
        }
        public Stat()
        {
            handon = new Stat_Handon();
            xiti = new Stat_Xiti();
            reward = new Stat_Reward();
            callname = new Stat_Callname();
            foreach(User si in Global.g_Studentlist)
            {
                StudentReward item = new StudentReward();
                item.uid = si.id;
                item.name = si.name;
                item.imageurl = si.imageurl;
                reward.list.Add(item);

                StudentCallname sc = new StudentCallname();
                sc.uid = si.id;
                sc.name = si.name;
                sc.imageurl = si.imageurl;
                sc.point = 0;
                callname.list.Add(sc);
            }
        }
        public void AddHandon(string result)
        {
            Log.Info("AddHandon_0" + result + ", handon.count=" + handon.count+", list.size="+ handon.list.Count);
            handon.count++;
            string[] szItem = result.Split(',');            
            Stat_HandonItem item = new Stat_HandonItem();
            item.index = handon.count;

            if (result.Length == 0)
                item.count = 0;
            else
                item.count = szItem.Length;

            item.ratio = Math.Round((double)100 * item.count / Global.g_Studentlist.Count, 2);
            handon.list.Add(item);
            Log.Info("AddHandon_1, handon.count=" + handon.count + ", list.size=" + handon.list.Count+", ratio="+item.ratio);
        }
        public void AddCallname(int uid)
        {
            foreach (StudentCallname item in callname.list)
            {
                if (item.uid == uid)
                {
                    item.point++;
                    break;
                }
            }
        }
        public void AddReward(int uid,int point)
        {
            reward.count++;
            foreach(StudentReward item in  reward.list)
            {
                if(item.uid == uid)
                {
                    item.point += point;
                    break;
                }
            }
        }
        public void AddXiti(string answer,string result)
        {
            string[] szItem = result.Split(',');
            Stat_XitiItem item = new Stat_XitiItem();
            item.index = ++xiti.count;
            if (result.Length > 0)
                item.count = szItem.Length;
            else
                item.count = 0;
            item.ratioAnswer = Math.Round((double)100 * item.count / Global.g_Studentlist.Count, 2);

            int nright = 0;
            foreach(string buf in szItem)
            {
                string[] szP = buf.Split(':');
                string _answer = szP[1];
                if(_answer == answer && answer.Length>0)
                {
                    nright++;
                }
            }
            item.countRight = nright;
            item.ratioRight = Math.Round((double)100 * nright / Global.g_Studentlist.Count, 2);
            xiti.list.Add(item);
        }
        public string toJson()
        {
            //计算总的指标 平均参与率
            {
                int handonStuCount = 0;
                foreach(Stat_HandonItem item  in handon.list)
                {
                    handonStuCount += item.count;
                }
                if(handon.count > 0)
                    handon.ratio = Math.Round((double)100 * handonStuCount / (handon.count * Global.g_Studentlist.Count), 2);
                if (handon.ratio > 100)
                    handon.ratio = 100;
            }

            {
                int xitiStuCount = 0;
                int xitiStuCountRight = 0;
                foreach (Stat_XitiItem item in xiti.list)
                {
                    xitiStuCount += item.count;
                    xitiStuCountRight += item.countRight;
                }
                if (xiti.count > 0)
                {
                    xiti.ratioAnswer = Math.Round((double)100 * xitiStuCount / (xiti.count * Global.g_Studentlist.Count), 2);
                    xiti.ratioRight = Math.Round((double)100 * xitiStuCountRight / (xiti.count * Global.g_Studentlist.Count), 2);
                } 
            }
            //点名覆盖率
            {
                int count = 0;
                int stucount = 0;
                foreach (StudentCallname item in callname.list)
                {
                    if (item.point > 0)
                        stucount++;
                    count += item.point;
                }
                callname.count = count;
                callname.ratio = Math.Round((double)100 * stucount / Global.g_Studentlist.Count, 2);
                if (callname.ratio > 100)
                    callname.ratio = 100;
            }
            //奖励覆盖率
            {
                int nstu = 0;
                foreach (StudentReward item in reward.list)
                {
                    if (item.point > 0)
                        nstu++;
                }
                //reward.count = nstu;
                reward.ratio = Math.Round((double)100 * nstu / Global.g_Studentlist.Count, 2);
                if (reward.ratio > 100)
                    reward.ratio = 100;
            }
            //list排序
            handon.list.Sort((x, y) => x.index.CompareTo(y.index));
            xiti.list.Sort((x, y) => x.index.CompareTo(y.index));
            reward.list.Sort((x, y) => -x.point.CompareTo(y.point));
            callname.list.Sort((x, y) => -x.point.CompareTo(y.point));

            string ret = new JavaScriptSerializer().Serialize(this);
            return ret;
        }
    }

    [DataContract]
    public class Stat_Handon
    {
        [DataMember]
        public int count { get; set; }
        [DataMember]
        public double ratio { get; set; }
        public List<Stat_HandonItem> list { get; set; }
        public Stat_Handon()
        {
            list = new List<Stat_HandonItem>();
        }
        public Stat_Handon Clone()
        {
            Stat_Handon ret = new Stat_Handon();
            ret.count = this.count;
            ret.ratio = this.ratio;
            foreach (Stat_HandonItem item in this.list)
            {
                ret.list.Add(item);
            }
            return ret;
        }
        public string toJson()
        {
            string ret = new JavaScriptSerializer().Serialize(this);
            return ret;
        }
    }
    [DataContract]
    public class Stat_HandonItem
    {
        [DataMember]
        public int index { get; set; }
        [DataMember]
        public double ratio { get; set; }
        [DataMember]
        public string rid { get; set; }
        [DataMember]
        public int count { get; set; }

        public string toJson()
        {
            string ret = new JavaScriptSerializer().Serialize(this);
            return ret;
        }
    }
    [DataContract]
    public class Stat_Callname
    {
        [DataMember]
        public int count { get; set; }//总次数
        [DataMember]
        public double ratio { get; set; }//总覆盖率
        [DataMember]
        public List<StudentCallname> list { get; set; }
        public Stat_Callname()
        {
            list = new List<StudentCallname>();
        }
        public string toJson()
        {
            string ret = new JavaScriptSerializer().Serialize(this);
            return ret;
        }
    }

    [DataContract]
    public class Stat_Reward
    {
        [DataMember]
        public int count { get; set; }//总次数
        [DataMember]
        public double ratio { get; set; }//总覆盖率
        //学生奖励排名
        [DataMember]
        public List<StudentReward> list { get; set; }
        public Stat_Reward()
        {
            list = new List<StudentReward>();
        }
        public string toJson()
        {
            string ret = new JavaScriptSerializer().Serialize(this);
            return ret;
        }
    }

    [DataContract]
    public class Stat_XitiItem
    {
        [DataMember]
        public int index { get; set; }
        [DataMember]
        public string rid { get; set; }
        [DataMember]
        public int count { get; set; }
        [DataMember]
        public int countRight { get; set; }
        [DataMember]
        public double ratioAnswer { get; set; }
        [DataMember]
        public double ratioRight { get; set; }
        public string toJson()
        {
            string ret = new JavaScriptSerializer().Serialize(this);
            return ret;
        }
    }

    [DataContract]
    public class Stat_Xiti
    {
        [DataMember]
        public int count { get; set; }
        [DataMember]
        public double ratioAnswer { get; set; }
        [DataMember]
        public double ratioRight { get; set; }
        public List<Stat_XitiItem> list { get; set; }//每道题的正确率和参与率
        public Stat_Xiti()
        {
            list = new List<Stat_XitiItem>();
        }
        public Stat_Xiti Clone()
        {
            Stat_Xiti obj = new Stat_Xiti();
            obj.count = this.count;
            obj.ratioAnswer = this.ratioAnswer;
            obj.ratioRight = this.ratioRight;
            foreach(Stat_XitiItem item in list )
            {
                obj.list.Add(item);
            }
            return obj;
        }
        public string toJson()
        {
            string ret = new JavaScriptSerializer().Serialize(this);
            return ret;
        }
    }
    
    #endregion
}
