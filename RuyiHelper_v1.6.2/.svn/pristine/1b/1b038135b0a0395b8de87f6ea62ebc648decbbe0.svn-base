using Newtonsoft.Json;

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using RueHelper.util;
using RueHelper.model;


namespace RueHelper
{
    public partial class FormTimer : Form
    {
        [DllImport("user32.dll", EntryPoint = "AnimateWindow")]
        private static extern bool AnimateWindow(IntPtr handle, int ms, int flags);

        public const int AW_HOR_POSITIVE = 0X1;//左->右
        public const int AW_HOR_NEGATIVE = 0X2;//右->左
        public const int AW_VER_POSITIVE = 0X4;//上->下
        public const int AW_VER_NEGATIVE = 0X8;//下->上
        public const int AW_CENTER = 0X10;
        public const int AW_HIDE = 0X10000;
        public const int AW_ACTIVATE = 0X20000;//逐渐显示
        public const int AW_SLIDE = 0X40000;
        public const int AW_BLEND = 0X80000;
        public const int AW_L2R = 0X40001;
        public const int AW_R2L = 0X40002;
        public const int AW_U2D = 0X40004;
        public const int AW_D2U = 0X40008;

        public enum Effect { Roll, Center, Hide, Slide, Blend }
        private static int[] effmap = { 0, 0x10, 0X10000, 0x40000, 0x80000 };
        private static int[] dirmap = { 1, 5, 4, 6, 2, 10, 8, 9 };
        private Hashtable stuPushCount = new Hashtable();

        //0x20010);   // 居中逐渐显示。
        //0xA0000); // 淡入淡出效果。
        //0x60004); // 自上向下。
        //0x20004); // 自上向下。
        //0x10010);    // 居中逐渐隐藏。
        //0x90000); // 淡入淡出效果。
        //0x50008); // 自下而上。
        //0x10008); // 自下而上。
        public static void Animate(Control ctl, Effect effect, int msec, int angle)
        {
            int flags = effmap[(int)effect];
            if (ctl.Visible) { flags |= 0x10000; angle += 180; }
            else
            {
                if (ctl.TopLevelControl == ctl) flags |= 0x20000;
                else if (effect == Effect.Blend) throw new ArgumentException();
            }
            flags |= dirmap[(angle % 360) / 45];
            bool ok = AnimateWindow(ctl.Handle, msec, flags);
            if (!ok) throw new Exception("Animation failed");
            ctl.Visible = !ctl.Visible;
        }

        private static log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public delegate void InvokeHide(bool hide);

        AutoResetEvent are = new AutoResetEvent(false);
        int screenWidth = Screen.PrimaryScreen.Bounds.Width;
        int screenHeight = Screen.PrimaryScreen.Bounds.Height;
        ArrayList al;
        ArrayList alText;
        ArrayList clickstate;
        public ArrayList _rightList;
        public Form11 f11;
        System.Media.SoundPlayer sp = new SoundPlayer(RueHelper.Properties.Resources.click1);
        string _classid = "";
        string _lessonid = "";
        HashSet<int> _resutlSet = new HashSet<int>();
        public string _xitiId = "";
        public System.Timers.Timer t;
        public System.Timers.Timer t1;
        public string _callnamsStr = "";
        public string _rewardStr = "";
        public string _criticizeStr = "";
        private int _querytimes = 0;
        private int _panelX = 0;
        private System.Timers.Timer _panelTimer;
        int inTimer = 0;
        public DateTime tm_create = DateTime.Now;
        StudentInfo[] si;
        public string RESULT = "";
        public FormTimer(string numberstr)
        {
            Log.Info("FormHandon.create,  numberstr=" + numberstr);
            al = new ArrayList();
            alText = new ArrayList();
            clickstate = new ArrayList();
            _classid = Global.getClassID()+"";
            _lessonid = Global.getLessonID() + "";
Log.Info("debug. FormHandon._classid=" + _classid + ", _lessonid=" + _lessonid);

            _xitiId = Global.getSchoolID() + "-" +_classid + "-" + DateTime.Now.ToString("yyyyMMddHHmmss");

            //TODO: 截屏上传
            if ((EService.myppt != null && EService.myppt.isOpen()) || EService.bShowPicture)
            {
                _xitiId = "H_" + _xitiId;

                Image img = ScreenCapture.captureScreen(0, 0);
                string imgName = _xitiId + ".jpg";
                string imgDir = Application.StartupPath + "\\" + DateTime.Now.ToString("yyyyMMdd");
                if (!Directory.Exists(imgDir))
                    Directory.CreateDirectory(imgDir);

                string imgPath = imgDir + "\\" + imgName;
                img.Save(imgPath);

                Common.uploadPicture(imgPath);
            }

            InitializeComponent();
            this.Text = "提问[" + _xitiId + "]";

            this.Height = screenHeight;
            this.Width = screenWidth;

            StartPosition = FormStartPosition.Manual;
            SetDesktopLocation(0, screenHeight - this.Height);

            this.TopMost = true;
#if DEBUG
            this.TopMost = false;//PPTPractise
#endif
            //this.WindowState = FormWindowState.Maximized;
            this.Hide();
            this.Show();
            this.BringToFront();

            Log.Info("FormHandon Timer_start(Theout) now...");

            //t = new System.Timers.Timer(200);
            //t.Elapsed += new System.Timers.ElapsedEventHandler(Theout);
            //t.Enabled = true;
            //t.AutoReset = true;

            IntelligentRecommend.InitQuestion();
        }

        public void HideEvent(bool bHide)
        {
            if (this.InvokeRequired)
            {
                InvokeHide cb = new InvokeHide(HideEvent);
                this.Invoke(cb, new object[] { bHide });
                return;
            }
            if (bHide)
            {
               // this.Hide();
            }else{
                _querytimes = 0;
                this.Show();
                this.BringToFront();
            }
                
        }
        public void AppendCallname(int uid)
        {
            IntelligentRecommend.addCallname(uid);

            if (_callnamsStr.IndexOf(uid+":") >= 0)
                return;

            DateTime tm_now = DateTime.Now;
            TimeSpan createtimespan = new TimeSpan(tm_create.Ticks);
            TimeSpan nowtimespan = new TimeSpan(tm_now.Ticks);
            TimeSpan timespan = nowtimespan.Subtract(createtimespan).Duration();
            int timeDiff = timespan.Minutes * 60 + timespan.Seconds;

            string pair = uid + ":" + timeDiff;
            if (_callnamsStr.Length > 0)
                _callnamsStr += ",";
            _callnamsStr += pair;
        }
        public void AppendReward(string uid, int point,string reason,string reasonid)
        {
            if (_rewardStr.IndexOf(uid+":") >= 0)
                return;

            DateTime tm_now = DateTime.Now;
            TimeSpan createtimespan = new TimeSpan(tm_create.Ticks);
            TimeSpan nowtimespan = new TimeSpan(tm_now.Ticks);
            TimeSpan timespan = nowtimespan.Subtract(createtimespan).Duration();
            int timeDiff = timespan.Minutes*60 + timespan.Seconds;

            string pair = uid + ":" + timeDiff + ":" + point + ":" + reason + ":" + reasonid;
            if (_rewardStr.Length > 0)
                _rewardStr += ",";
            _rewardStr += pair;
        }
        public void AppendCriticize(int uid)
        {
            if (_criticizeStr.IndexOf(uid + ":") >= 0)
                return;

            DateTime tm_now = DateTime.Now;
            TimeSpan createtimespan = new TimeSpan(tm_create.Ticks);
            TimeSpan nowtimespan = new TimeSpan(tm_now.Ticks);
            TimeSpan timespan = nowtimespan.Subtract(createtimespan).Duration();
            int timeDiff = timespan.Minutes * 60 + timespan.Seconds;

            if (uid > 0)
            {
                string pair = uid + ":" + timeDiff;
                if (_criticizeStr.Length > 0)
                    _criticizeStr += ",";
                _criticizeStr += pair;
            }
        }

        public string GetCallname()
        {
            return _callnamsStr;
        }
        public string GetRewarded()
        {
            return _rewardStr;
        }
        public string GetCriticize()
        {
            return _criticizeStr;
        }
        public string GetResult()
        {
            return RESULT;
        }

        [DllImport("winmm.dll", EntryPoint = "waveOutSetVolume")]
        public static extern int WaveOutSetVolume(IntPtr hwo, uint dwVolume);
        private void SetVol(double arg)
        {
            double newVolume = ushort.MaxValue * arg / 10.0;

            uint v = ((uint)newVolume) & 0xffff;
            uint vAll = v | (v << 16);

            int retVal = WaveOutSetVolume(IntPtr.Zero, vAll);
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;//最小化
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            Win32.AnimateWindow(this.Handle, 200, Win32.AW_SLIDE | Win32.AW_HIDE | Win32.AW_BLEND);
        }
        
        public string StopT()
        {
            if(t.Enabled)
                t.Enabled = false;
            string answerStr = RESULT.Replace(":H","");// fo.ReadFile();
            Log.Info("StopT() answerStr=" + answerStr);
            return answerStr;
        }

    }
}
