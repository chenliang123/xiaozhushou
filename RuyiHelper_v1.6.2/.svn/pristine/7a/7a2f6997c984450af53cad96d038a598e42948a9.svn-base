using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace RueHelper.model
{
    class OfflineProcessor
    {
        public static LessonEvents curLessonEvents;
        private static log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static int UploadHistoryData() {
            //遍历近七天的目录
            int count = 0;
            for(int i=0; i<7; i++)
            {
                string dir = Application.StartupPath + "\\" + DateTime.Now.AddDays(0 - i).ToString("yyyyMMdd");
                if(!Directory.Exists(dir))
                {
                    continue;
                }
                string[] szPath = Directory.GetFiles(dir);
                int result = 0;
                foreach(string path in szPath)
                {
                    string filename = Path.GetFileName(path);
                    if(filename.StartsWith("lesson_") && filename.EndsWith(".txt"))
                    {
                        FileOper fo = new FileOper(dir, filename);
                        string data = fo.ReadFile();

                        //base64
                        LessonEvents le = JsonOper.DeserializeJsonToObject<LessonEvents>(data);
                        if(le.eventlist==null || le.eventlist.Count<3)
                        {
                            File.Delete(path);
                            continue;
                        }
                        int ret = Common.uploadOfflineData(le);
                        if(ret > 0)
                        {
                            File.Delete(path);
                        }
                    }
                }
            }
            return count;
        }

        private static int WriteFile(LessonEvents le)
        {
            if (le.eventlist == null)
                return 0;

            if (le.eventlist.Count < 3)
                return 0;

            string data = le.toJson();
            string strBase64 = RueHelper.util.Util.toBase64(data);
            string filename = "lesson_"+curLessonEvents.createtime.Replace(" ","").Replace("-","").Replace(":","")+".txt";
            string filepath = Application.StartupPath + "\\" + DateTime.Now.ToString("yyyyMMdd");
            FileOper fo = new FileOper(filepath, filename);
            fo.WriteFile(data);
            return 0;
        }

        public static int AddEvent(string action,string request,string filepath, string tm) 
        {
            if(curLessonEvents == null)
            {
                curLessonEvents = new LessonEvents();
                curLessonEvents.lessonid = Global.getLessonID();//之前有网，上课过程中突然中断。。。
            }

            LessonEvent ev = new LessonEvent(action, request, filepath, tm);
            string strEvent = ev.toJson();
            LessonEventType type = (LessonEventType)ev.type;
            if (type == LessonEventType.Unknown)
            {
                Log.Info("Offline LessonEvent_Ignore: " + strEvent);
                return 0;
            }

            Log.Info("Offline LessonEvent: " + strEvent);
            if(type == LessonEventType.ClassOn)
            {
                if (curLessonEvents != null)
                {
                    WriteFile(curLessonEvents);
                    curLessonEvents = null;
                }
                curLessonEvents = new LessonEvents();
                curLessonEvents.addEvent(ev);
            }
            else if (type == LessonEventType.ClassOff)
            {
                curLessonEvents.addEvent(ev);
                //TODO: 写文件
                WriteFile(curLessonEvents);
                curLessonEvents = null;
            }
            else
            {
                curLessonEvents.addEvent(ev);
            }
            return 0;
        }
    }
    
    [DataContract]
    class LessonEvents
    {
        [DataMember]
        public int lessonid { get; set; }
        [DataMember]
        public int classid { get; set; }
        [DataMember]
        public int teacherid { get; set; }
        [DataMember]
        public int courseid { get; set; }
        [DataMember]
        public string createtime { get; set; }
        [DataMember]
        public List<LessonEvent> eventlist { get; set; }

        public string toJson()
        {
            string str = new JavaScriptSerializer().Serialize(this);
            return str;
        }
        public LessonEvents()
        {
            classid = Global.getClassID();
            teacherid = Global.getTeacherID();
            courseid = Global.getCourseID();
            createtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
        public void addEvent(LessonEvent ev)
        {
            if (eventlist == null)
            {
                eventlist = new List<LessonEvent>();
            }
            eventlist.Add(ev);
        }
    }

    class LessonEvent
    {
        [DataMember]
        public string action { get; set; }
        [DataMember]
        public LessonEventType type { get; set; }
        [DataMember]
        public string request { get; set; }
        [DataMember]
        public string filepath { get; set; }
        [DataMember]
        public string tm { get; set; }

        public LessonEvent(string action,string request,string filepath, string tm)
        {
            this.tm = tm;
            this.action = action;
            this.type = getEventType(action);
            this.request = request;
            this.filepath = filepath;
        }

        public string toJson()
        {
            string str = new JavaScriptSerializer().Serialize(this);
            return str;
        }

        public LessonEventType getEventType(string action)
        {
            LessonEventType val = 0;
            if (action == "lesson.get")
            {
                return LessonEventType.ClassOn;
            }
            else if (action == "lesson.off")
            {
                return LessonEventType.ClassOff;
            }
            else if (action == "XitiResult" || action == "xitiResult.upload")
            {
                return LessonEventType.Xiti;
            }
            else if (action == "addHandon")
            {
                return LessonEventType.Handon;
            }
            else if (action == "addCallname")
            {
                return LessonEventType.CallName;
            }
            else if (action == "addReward")
            {
                return LessonEventType.Reward;
            }
            else if (action == "fileupload")
            {
                return LessonEventType.UploadFile;
            }
            else if (action == "addRecordEvent")
            {
                return LessonEventType.RecordAudio;
            }
            else if (action == "DrawView")
            {
                return LessonEventType.DrawPicture;
            }
            else if (action == "fileopen" ||action == "fileclose"|| action == "FileEvent")
            {
                return LessonEventType.FileOpen;
            }
            else if (action == "GroupHandon")
            {
                return LessonEventType.GroupHandon;
            }
            else if (action == "GroupXiti")
            {
                return LessonEventType.GroupXiti;
            }
            return LessonEventType.Unknown;
        }
    }
    enum LessonEventType { 
        ClassOn,		//0 上课
		BookOutline,	//1 获取大纲
		JiaoAnList,		//2 获取教案
		FilePage,	//3 文件翻页
		FileOpen,	//4 文件打开
		Xiti,		//5 题库做题
		Competitive,//6 抢答
		Handon,		//7 举手
		CallName,	//8 点名
		DrawPicture,//9 画笔
		ClassOff,	//10 下课
		Reward,	//11 奖励
		Photo,	//12 拍照
		InvalidXiti,	//13 
		InvalidHandon,	//14 
		Homework,	//15 布置家庭作业
		Criticize,   //16 批评
		RecordVideo, //17
		RecordAudio, //18
		GroupHandon, //19 组提问
		GroupXiti,	//20 组出题
		GroupCallName,	//21 组点名
		GroupReward,
        UploadFile,
        Unknown
    }
}
