using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Threading;
using System.Net;
using System.IO;

namespace Edu_Simulator
{
    public partial class FormTest : Form
    {
        private static System.Configuration.Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        private Classes[] szClasses = new Classes[]{};
        private Dictionary<int, Classes> clist = new Dictionary<int, Classes>();
        private User m_user = new User();
        private Classes m_classes = new Classes();
        private int m_classroomid = 0;
        private int m_schoolid = 0;
        private int m_lessonid = 0;
        private StudentInfo[] szStudent = new StudentInfo[] { };
        Random random = new Random();

        private List<string> m_xitilist12 = new List<string>();
        private List<string> m_xitilist13 = new List<string>();
        private string[] m_key = new string[] {"A","B","C","D","R","W" };
        private string[] m_key0 = new string[] { "A", "B", "C", "D", "对", "错" };
        private string m_url = "";
        public FormTest()
        {
            Global g = new Global();

            m_xitilist12.Add("zzyw62100560,C");
            m_xitilist12.Add("zzyw62100561,B");
            m_xitilist12.Add("zzyw62100562,B");
            m_xitilist12.Add("zzyw62100563,B");
            m_xitilist12.Add("zzyw62100564,C");
            m_xitilist12.Add("zzyw62100565,B");
            m_xitilist12.Add("zzyw62100566,C");
            m_xitilist12.Add("zzyw62100567,A");
            m_xitilist12.Add("zzyw62100568,B");
            m_xitilist12.Add("zzyw62100569,B");
            m_xitilist12.Add("zzyw62100570,C");
            m_xitilist12.Add("zzyw62100571,C");
            m_xitilist12.Add("zzyw62100572,B");
            m_xitilist12.Add("zzyw62100573,A");
            m_xitilist12.Add("zzyw62100574,B");
            m_xitilist12.Add("zzyw62100575,错");
            m_xitilist12.Add("zzyw62100576,对");
            m_xitilist12.Add("zzyw62100577,错");
            m_xitilist12.Add("zzyw62100578,对");
            m_xitilist12.Add("zzyw62100579,对");
            m_xitilist12.Add("zzyw62100580,对");

            m_xitilist13.Add("zzsx62100000,B");
            m_xitilist13.Add("zzsx62100001,C");
            m_xitilist13.Add("zzsx62100002,C");
            m_xitilist13.Add("zzsx62100003,A");
            m_xitilist13.Add("zzsx62100004,A");
            m_xitilist13.Add("zzsx62100008,A");
            m_xitilist13.Add("zzsx62100009,B");
            m_xitilist13.Add("zzsx62100010,A");
            m_xitilist13.Add("zzsx62100011,B");
            m_xitilist13.Add("zzsx62100012,C");
            m_xitilist13.Add("zzsx62100016,C");
            m_xitilist13.Add("zzsx62100017,C");
            m_xitilist13.Add("zzsx62100018,B");
            m_xitilist13.Add("zzsx62100019,A");
            m_xitilist13.Add("zzsx62100020,B");
            m_xitilist13.Add("zzsx62100005,对");
            m_xitilist13.Add("zzsx62100006,错");
            m_xitilist13.Add("zzsx62100007,错");
            m_xitilist13.Add("zzsx62100013,错");
            m_xitilist13.Add("zzsx62100014,错");
            m_xitilist13.Add("zzsx62100015,错");

            InitializeComponent();
            comboBox_lessonIndex.Items.Add("上午 第一节");
            comboBox_lessonIndex.Items.Add("上午 第二节");
            comboBox_lessonIndex.Items.Add("上午 第三节");
            comboBox_lessonIndex.Items.Add("上午 第四节");
            comboBox_lessonIndex.Items.Add("下午 第五节");
            comboBox_lessonIndex.Items.Add("下午 第六节");
            comboBox_lessonIndex.Items.Add("下午 第七节");
            comboBox_lessonIndex.Items.Add("下午 第八节");
            int index = 0;
            int hhmm = Int32.Parse(DateTime.Now.ToString("HHmm"));
            if (hhmm < 900)
                index = 0;
            else if (hhmm < 1000)
                index = 1;
            else if (hhmm < 1100)
                index = 2;
            else if (hhmm < 1200)
                index = 3;
            else if (hhmm < 1400)
                index = 4;
            else if (hhmm < 1500)
                index = 5;
            else if (hhmm < 1600)
                index = 6;
            else
                index = 5;
            comboBox_lessonIndex.SelectedIndex = index;

            comboBox_lessonIndex.Items.Add("下午 第八节");
            comboBox_schoolid.Items.Add("32");
            comboBox_schoolid.Items.Add("33");
            comboBox_schoolid.SelectedIndex = 1;

            textBox_user.Text = "wangqi";
            textBox_pwd.Text = "123456";

            List<string> iplist = GetLocalIPAddress();
            foreach(string ip in iplist)
            {
                comboBox_ip.Items.Add(ip.Substring(0,ip.LastIndexOf("."))+"1");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string account = textBox_user.Text;
            string pwd = textBox_pwd.Text;
            string data = Common.login(account,pwd);
            //MessageBox.Show(data);
            m_user = JsonOper.DeserializeJsonToObject<User>(data);
            textBox_name.Text = m_user.name;
            szClasses = m_user.classlist;
            m_classes = szClasses[0];
            m_schoolid = m_classes.schoolid;
            m_classroomid = m_classes.roomid;
            textBox_classname.Text = m_classes.name;

            comboBox_roomid.Text = m_classroomid+"";
        }
        private void loadClassInfo()
        {
            //从配置文件中获取
            string schoolid = config.AppSettings.Settings["schoolid"].Value;
            string classroomid = config.AppSettings.Settings["classroomid"].Value;
            string hdip = config.AppSettings.Settings["hdip"].Value;
            string updateflag = config.AppSettings.Settings["hdip-autoupdate"].Value;

            string dir = Application.StartupPath + "\\conf\\";
            string filename = schoolid + "-" + classroomid + ".conf";
            string filepath = dir + filename;

            FileOper fo = new FileOper(dir, filename);

            string strClassInfo = Common.getClassInfo();
            if (strClassInfo.Length > 0)
            {
                //更新本地缓存
                fo.DeleteFile();
                fo.Context = strClassInfo;
                fo.WriteFile();
            }
        }
        //选择学校ID
        private void comboBox_schoolid_SelectedIndexChanged(object sender, EventArgs e)
        {
            string i = comboBox_schoolid.SelectedItem.ToString();
            m_schoolid = Int32.Parse(i);

            string data = Common.getClasslist(m_schoolid);
            if(data.Length > 0)
            {
                //MessageBox.Show(data);
                Gradelist glist = JsonOper.DeserializeJsonToObject<Gradelist>(data);
                Grade[] szGrade = glist.gradelist;

                comboBox_roomid.Items.Clear();
                clist.Clear();
                foreach (Grade g in szGrade)
                {
                    foreach (Classes c in g.classlist)
                    {
                        clist.Add(c.roomid, c);
                        comboBox_roomid.Items.Add(c.roomid);
                    }
                }
                comboBox_roomid.SelectedIndex = 0;
            }

        }
        //选择教室 班级
        private void comboBox_roomid_SelectedIndexChanged(object sender, EventArgs e)
        {
            string i = comboBox_roomid.SelectedItem.ToString();
            m_classroomid = Int32.Parse(i);
            m_classes = clist[m_classroomid];
            textBox_classname.Text = m_classes.name;

            //取学生列表
            string strClassInfo = Common.getClassInfo();
            ClassInfo ci = JsonOper.DeserializeJsonToObject<ClassInfo>(strClassInfo);
            szStudent.Initialize();
            szStudent = ci.Data.Student;
            //MessageBox.Show("szStudent.length = "+szStudent.Length);
        }

        //获取lesson
        private void button_getlesson_Click(object sender, EventArgs e)
        {
            int uid = m_user.id;
            int index = comboBox_lessonIndex.SelectedIndex + 1;
            int classid = m_classes.id;
            Global._classid = classid+"";
            Global.classroomid = m_classroomid;
            Global._teacherid = m_user.id+"";
            Global.schoolid = m_schoolid;

            Global._courseid = m_user.courseid + "";

            Classes[] szClasses = m_user.classlist;
            foreach(Classes c in szClasses)
            {
                Global._courseid = c.courseid+"";
                textBox_coursename.Text = c.coursename;
            }
            Common.getLesson(index);
            m_lessonid = Int32.Parse(Global._lessonid);
            textBox_lessonid.Text = m_lessonid + "";
        }
        ///////////////////////////////////////////////////////////////////////////






        ///////////////////////////////////////////////////////////////////////////
        //点名
        private void button1_callname(object sender, EventArgs e)
        {
            if (m_user == null || Global._courseid == "0" || Global._lessonid == "0" || Global._teacherid == "0")
            {
                MessageBox.Show("请先选择班级、登录并点击\"上课\"");
                return;
            }

            //回答人数
            int count = random.Next(15, szStudent.Length - 3);
            HashSet<int> seatlist = new HashSet<int>();
            List<StudentInfo> stulist = new List<StudentInfo>(szStudent);

            string str = "";
            string handon="";//0-1:56B15782,H|0-4:56B15784,H|0-2:56B15786,H|
            string result = "";//11:王涵:11
            while (count > 0)
            {
                int k = random.Next(0, stulist.Count - 1);
                StudentInfo s = stulist[k];
                stulist.RemoveAt(k);
                count--;
                string tmp = s.SEAT + ":" + getUTC() +",H";
                if (handon.Length > 0)
                {
                    handon += "|";
                }
                else
                {
                    result = s.ID + ":" + s.Name + ":" + s.SEAT;
                }
                handon += tmp;
            }
            string ret = Common.uploadCallname(handon, result, false);
            MessageBox.Show(handon+"\r\n"+ret);
        }

        //打开ppt
        private void button_ppt_Click(object sender, EventArgs e)
        {
            if(m_user==null || Global._courseid=="0" || Global._lessonid=="0" || Global._teacherid=="0")
            {
                MessageBox.Show("请先选择班级、登录并点击\"上课\"");
                return;
            }
            string[] szFile12 = { "《东郭先生》课件.ppt","《古诗三首》课件.ppt", "《司马光砸缸》课件.ppt", "《赵州桥》课件.ppt", "《卖火柴的小女孩》课件.ppt","《农夫和蛇》课件.ppt" };
            string[] szFile13 = {  "数学课件_加减法.ppt","数学课件_众数.ppt", "数学课件_三角形.ppt", "数学课件_面积.ppt", "数学课件_除法.ppt" };


            string filename = "";
            if(Global._courseid=="12")
            {
                int i = random.Next(0, szFile12.Length - 1);
                filename = szFile12[i];
            }
            else
            {
                int i = random.Next(0, szFile13.Length - 1);
                filename = szFile13[i];
            }

            //======================================================
            string ret = Common.uploadFileopenEvent(filename,false);
            MessageBox.Show(ret);
        }

        private void button_xitiPPT_Click(object sender, EventArgs e)
        {
            if (m_user == null || Global._courseid == "0" || Global._lessonid == "0" || Global._teacherid == "0")
            {
                MessageBox.Show("请先选择班级、登录并点击\"上课\"");
                return;
            }

            string xiti = DateTime.Now.ToString("yyyyMMddHHmmss") + "," + m_key0[random.Next(0,6)];

            string xiti_id = xiti.Substring(0, xiti.IndexOf(","));
            string xiti_answer = xiti.Substring(xiti.IndexOf(",") + 1);
            int type = 0;
            if (xiti_answer == "对")
            {
                type = 1;
                xiti_answer = "R";
            }
            else if (xiti_answer == "错")
            {
                type = 1;
                xiti_answer = "W";
            }

            //回答人数
            int count = random.Next(15, szStudent.Length - 3);
            HashSet<int> seatlist = new HashSet<int>();
            List<StudentInfo> stulist = new List<StudentInfo>(szStudent);

            string str = "";
            string result = "";//1:A,2:S,3:B,,5:C
            while (count > 0)
            {
                int k = random.Next(0, stulist.Count - 1);
                StudentInfo s = stulist[k];
                stulist.RemoveAt(k);
                count--;

                int keyRand = 0;
                if (type == 0)
                    keyRand = random.Next(0, 4); //[a,b)
                else
                    keyRand = random.Next(4, 6);

                int nSeat = Int32.Parse(s.SEAT.Replace("-", ""));
                string tmp = nSeat + ":" + m_key[keyRand];
                if (result.Length > 0)
                {
                    result += ",";
                }
                result += tmp;
            }
            string ret = Common.uploadXitiResult(xiti_id, xiti_answer, result, false);
            MessageBox.Show(result + "\r\n" + ret);

        }

        private void button_CompetitivePPT_Click(object sender, EventArgs e)
        {
            if (m_user == null || Global._courseid == "0" || Global._lessonid == "0" || Global._teacherid == "0")
            {
                MessageBox.Show("请先选择班级、登录并点击\"上课\"");
                return;
            }
            string createtime = DateTime.Now.ToString("yyyyMMddHHmmss");
            string xiti = createtime + "," + m_key0[random.Next(0, 6)];

            string xiti_id = xiti.Substring(0, xiti.IndexOf(","));
            string xiti_answer = xiti.Substring(xiti.IndexOf(",") + 1);
            int type = 0;
            if (xiti_answer == "对")
            {
                type = 1;
                xiti_answer = "R";
            }
            else if (xiti_answer == "错")
            {
                type = 1;
                xiti_answer = "W";
            }


            //回答人数
            int count = random.Next(15, szStudent.Length - 3);
            int countOK = 0;
            HashSet<int> seatlist = new HashSet<int>();
            List<StudentInfo> stulist = new List<StudentInfo>(szStudent);

            string str = "";
            string result = "";//1:A,2:S,3:B,,5:C
            string result2 = "";
            while (count > 0)
            {
                int k = random.Next(0, stulist.Count - 1);
                StudentInfo s = stulist[k];
                stulist.RemoveAt(k);
                count--;

                int keyRand = 0;
                if (type == 0)
                    keyRand = random.Next(0, 4); //[a,b)
                else
                    keyRand = random.Next(4, 6);

                if (countOK >= 3)
                {
                    break;

                }
                if (m_key[keyRand] == xiti_answer)
                {
                    result2 += s.Name + ",";
                    countOK++;
                }

                int nSeat = Int32.Parse(s.SEAT.Replace("-", ""));
                string tmp = nSeat + ":" + m_key[keyRand];
                if (result.Length > 0)
                {
                    result += ",";
                }
                result += tmp;

            }
            result2 = result2.Substring(0, result2.Length - 1);
            string ret = Common.uploadCompetitiveAnswer(createtime, xiti_id, xiti_answer, result, result2, false);
            MessageBox.Show(result + "\r\n" + result2 + "\r\n" + xiti_answer + "\r\n" + ret);
        }

        private void button_xiti_Click(object sender, EventArgs e)
        {
            if (m_user == null || Global._courseid == "0" || Global._lessonid == "0" || Global._teacherid == "0")
            {
                MessageBox.Show("请先选择班级、登录并点击\"上课\"");
                return;
            }

            string xiti;
            if (Global._courseid == "12")
                xiti = m_xitilist12[random.Next(0, m_xitilist12.Count - 1)];
            else
                xiti = m_xitilist13[random.Next(0, m_xitilist13.Count - 1)];

            string xiti_id = xiti.Substring(0, xiti.IndexOf(","));
            string xiti_answer = xiti.Substring(xiti.IndexOf(",")+1);
            int type = 0;
            if (xiti_answer == "对")
            {
                type = 1;
                xiti_answer = "R";
            }
            else if (xiti_answer == "错")
            {
                type = 1;
                xiti_answer = "W";
            }

            //回答人数
            int count = random.Next(15, szStudent.Length - 3);
            HashSet<int> seatlist = new HashSet<int>();
            List<StudentInfo> stulist = new List<StudentInfo>(szStudent);

            string str = "";
            string result = "";//1:A,2:S,3:B,,5:C
            while (count > 0)
            {
                int k = random.Next(0, stulist.Count - 1);
                StudentInfo s = stulist[k];
                stulist.RemoveAt(k);
                count--;

                int keyRand = 0;
                if(type==0)
                    keyRand = random.Next(0,4); //[a,b)
                else
                    keyRand = random.Next(4,6);

                int nSeat = Int32.Parse(s.SEAT.Replace("-",""));
                string tmp = nSeat + ":" + m_key[keyRand];
                if (result.Length > 0)
                {
                    result += ",";
                }
                result += tmp;
            }
            string ret = Common.uploadXitiResult(xiti_id, xiti_answer, result,false);
            MessageBox.Show(result + "\r\n" + ret);
        }

        private void button_Competitive_Click(object sender, EventArgs e)
        {
            if (m_user == null || Global._courseid == "0" || Global._lessonid == "0" || Global._teacherid == "0")
            {
                MessageBox.Show("请先选择班级、登录并点击\"上课\"");
                return;
            }

            string xiti;
            if (Global._courseid == "12")
                xiti = m_xitilist12[random.Next(0, m_xitilist12.Count - 1)];
            else
                xiti = m_xitilist13[random.Next(0, m_xitilist13.Count - 1)];

            string xiti_id = xiti.Substring(0, xiti.IndexOf(","));
            string xiti_answer = xiti.Substring(xiti.IndexOf(",") + 1);
            int type = 0;
            if (xiti_answer == "对")
            {
                type = 1;
                xiti_answer = "R";
            }
            else if (xiti_answer == "错")
            {
                type = 1;
                xiti_answer = "W";
            }
               

            //回答人数
            int count = random.Next(15, szStudent.Length - 3);
            int countOK = 0;
            HashSet<int> seatlist = new HashSet<int>();
            List<StudentInfo> stulist = new List<StudentInfo>(szStudent);

            string str = "";
            string result = "";//1:A,2:S,3:B,,5:C
            string result2 = "";
            while (count > 0)
            {
                int k = random.Next(0, stulist.Count - 1);
                StudentInfo s = stulist[k];
                stulist.RemoveAt(k);
                count--;

                int keyRand = 0;
                if (type == 0)
                    keyRand = random.Next(0, 4); //[a,b)
                else
                    keyRand = random.Next(4, 6);

                if (countOK >= 3)
                {
                    break;

                }
                if(m_key[keyRand] == xiti_answer)
                {
                    result2 += s.Name + ",";
                    countOK++;
                }

                int nSeat = Int32.Parse(s.SEAT.Replace("-", ""));
                string tmp = nSeat + ":" + m_key[keyRand];
                if (result.Length > 0)
                {
                    result += ",";
                }
                result += tmp;

            }
            result2 = result2.Substring(0,result2.Length-1);
            string createtime = DateTime.Now.ToString("yyyyMMddHHmmss");
            string ret = Common.uploadCompetitiveAnswer(createtime,xiti_id, xiti_answer, result, result2, false);
            MessageBox.Show(result + "\r\n" +result2 + "\r\n"+xiti_answer+"\r\n"+ ret);
        }

        private string getUTC()
        {
            string utctime = DateTime.Now.ToUniversalTime().ToString("r");

            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            TimeSpan toNow = DateTime.Now.Subtract(dtStart);
            long timeStamp = toNow.Ticks;
            timeStamp = long.Parse(timeStamp.ToString().Substring(0, timeStamp.ToString().Length - 7));

            string strHex = Convert.ToString(timeStamp, 16).ToUpper();
            return strHex;
        }
        private string getUTC(int bias=0)
        {
            string utctime = DateTime.Now.ToUniversalTime().ToString("r");

            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            TimeSpan toNow = DateTime.Now.Subtract(dtStart);
            long timeStamp = toNow.Ticks + bias*1000000;
            timeStamp = long.Parse(timeStamp.ToString().Substring(0, timeStamp.ToString().Length - 7));

            string strHex = Convert.ToString(timeStamp, 16).ToUpper();
            return strHex;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Page2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_handon_Click(object sender, EventArgs e)
        {
            String host = comboBox_ip.Text;
            m_url = "http://P1:8986/Handon".Replace("P1", host);
            System.Threading.Thread thread = new System.Threading.Thread(AsyncDoGet);
            thread.Start();
        }

        private void button_handover_0_Click(object sender, EventArgs e)
        {
            String host = comboBox_ip.Text;
            m_url = "http://P1:8986/HandonOver?id=-1&name=".Replace("P1", host);

            ThreadStart starter = delegate { AsyncDoGet(30); };
            new Thread(starter).Start();
        }

        private void button_hondover_Click(object sender, EventArgs e)
        {
            String host = comboBox_ip.Text;
            m_url = "http://P1:8986/HandonOver?id=-1&name=".Replace("P1", host);

            ThreadStart starter = delegate { AsyncDoGet(30); };
            new Thread(starter).Start();
        }

        private void button_reward_Click(object sender, EventArgs e)
        {
            String host = comboBox_ip.Text;
            m_url = "http://P1:8986/Reward".Replace("P1", host);
            System.Threading.Thread thread = new System.Threading.Thread(AsyncDoGet);
            thread.Start();
        }

        delegate void SetTextDelegate(Control Ctrl, string Text);
        delegate void AppendTextDelegate(TextBox testbox, string Text);
        /// <summary>
        /// 跨线程设置控件Text
        /// </summary>
        /// <param name="Ctrl">待设置的控件</param>
        /// <param name="Text">Text</param>
        public static void SetText(Control Ctrl, string Text)
        {
            if (Ctrl.InvokeRequired == true) //非创建线程，用代理进行操作 
            {
                Ctrl.Invoke(new SetTextDelegate(SetText), Ctrl, Text);
            }
            else//不需要唤醒，就是创建控件的线程,直接正常操作
            {
                Ctrl.Text = Text;
            }
        }
        private void AsyncDoGet()
        {

            string url = m_url;

            AppendText(textBoxLog, url);
            try
            {
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.Method = "GET";
                //设置代理UserAgent和超时
                //request.UserAgent = userAgent;
                request.Timeout = 5000;

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream receiveStream = response.GetResponseStream();
                Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
                // Pipes the stream to a higher level stream reader with the required encoding format. 
                StreamReader readStream = new StreamReader(receiveStream, encode);
                string resp = readStream.ReadToEnd();
                string resp2 = resp;

                if (resp.StartsWith("CB"))
                    resp2 = resp.Substring(3, resp.Length - 4);
                AppendText(textBoxLog, resp2 + "\r\n");

                //JObject jo = (JObject)JsonConvert.DeserializeObject(resp2);
                //string ret = jo["ret"].ToString();
                //string msg = jo["msg"].ToString();
                //string data = jo["data"].ToString().Replace("\r\n", "");
                //string count = jo["count"].ToString();
            }
            catch (Exception e)
            {
                AppendText(textBoxLog, e.Message + "\r\n");
            }
        }

        private void AsyncDoGet(int timeout = 2)
        {
            string url = m_url;

            AppendText(textBoxLog, url);
            try
            {
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.Method = "GET";
                //设置代理UserAgent和超时
                //request.UserAgent = userAgent;
                request.Timeout = timeout * 1000;

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream receiveStream = response.GetResponseStream();
                Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
                // Pipes the stream to a higher level stream reader with the required encoding format. 
                StreamReader readStream = new StreamReader(receiveStream, encode);
                string resp = readStream.ReadToEnd();
                string resp2 = resp;

                if (resp.StartsWith("CB"))
                    resp2 = resp.Substring(3, resp.Length - 4);
                AppendText(textBoxLog, resp2 + "\r\n");

                //JObject jo = (JObject)JsonConvert.DeserializeObject(resp2);
                //string ret = jo["ret"].ToString();
                //string msg = jo["msg"].ToString();
                //string data = jo["data"].ToString().Replace("\r\n", "");
                //string count = jo["count"].ToString();
            }
            catch (Exception e)
            {
                AppendText(textBoxLog, e.Message + "\r\n");
            }
        }

        public static void AppendText(TextBox tb, string Text)
        {
            if (tb.InvokeRequired == true) //非创建线程，用代理进行操作
            {
                tb.Invoke(new AppendTextDelegate(AppendText), tb, Text);
            }
            else//不需要唤醒，就是创建控件的线程,直接正常操作
            {
                tb.AppendText(Text + "\r\n");
            }
        }
        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 获取本机IP
        /// </summary>
        /// <returns></returns>
        private List<string> GetLocalIPAddress()
        {
            List<string> iplist = new List<string>();
            System.Net.IPAddress[] addressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
            for (int i = 0; i < addressList.Length; i++)
            {
                if (addressList[i].ToString().Contains("."))
                {
                    if (addressList[i].ToString().Split('.')[0] == "192" || addressList[i].ToString().Split('.')[0] == "172" || addressList[i].ToString().Split('.')[0] == "10")
                        iplist.Add(addressList[i].ToString());
                }
            }
            return iplist;
        }

    }
}
