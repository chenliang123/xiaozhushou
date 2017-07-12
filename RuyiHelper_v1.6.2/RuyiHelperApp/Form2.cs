using Newtonsoft.Json;
using RueHelper.util;
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
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RueHelper
{
    public delegate void InvokeLabelState(string context, PictureBox pic, PictureBox text, int i);
    public delegate void InvokeLabelState2(string context);
    public delegate void InvokeLabelStateClear(PictureBox pic, PictureBox text, int i);
    public delegate void InvokeReadFile();
    public delegate void InvokeDrawingArcs(Label label, string KEY, int n, int Total, Label labelr);
    public partial class Form2 : Form
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
        public const int AW_L2R = 0X80001;
        public const int AW_R2L = 0X80002;
        public const int AW_U2D = 0X80004;
        public const int AW_D2U = 0X80008;
        public enum Effect { Roll, Center, Hide, Slide, Blend }
        private static int[] effmap = { 0, 0x10, 0X10000, 0x40000, 0x80000 };
        private static int[] dirmap = { 1, 5, 4, 6, 2, 10, 8, 9 };
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

        System.Configuration.Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        private static log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        AutoResetEvent are = new AutoResetEvent(false);
        int screenWidth = Screen.PrimaryScreen.Bounds.Width;
        int screenHeight = Screen.PrimaryScreen.Bounds.Height;
        ArrayList al;
        ArrayList alText;
        ArrayList clickstate;
        public ArrayList _rightList;
        public Form11 f11;

        System.Media.SoundPlayer sp = new SoundPlayer(RueHelper.Properties.Resources.click1);
        public string _id = "";
        public System.Timers.Timer t;
        public System.Timers.Timer t1;

        public string m_answer = "";
        private string top3_name1 = "";
        private string top3_name2 = "";
        private string top3_name3 = "";
        private string top3_answer1 = "";
        private string top3_answer2 = "";
        private string top3_answer3 = "";

        public string _callnamsStr = "";
        public string _rewardStr = "";
        public string _criticizeStr = "";
        
        public DateTime tm_create = DateTime.Now;
        int inTimer = 0;
        int mode = 1;//1：做题 2：抢答
        //int inTimer_Competitive = 0;
        StudentInfo[] si;
        private Hashtable m_hashtable = new Hashtable(); //保存学生按键数据
        private Color  CircleBackgroundColor= System.Drawing.Color.FromArgb(254, 232, 211);
        public Form2(string questionstr, string id, string numberstr, string classid, string lessonid)
        {
            mode = 1;
            tm_create = DateTime.Now;

            Log.Info("From2.create, questionstr=" + questionstr + ", id=" + id + ", classid=" + classid + ", lessonid=" + lessonid);
            al = new ArrayList();
            alText = new ArrayList();
            clickstate = new ArrayList();
            _id = id;

            InitializeComponent();
            SetPanel(numberstr);
            SetDesktopLocation(0, screenHeight - this.Height);

            this.Text = "习题[" + id + "]";
            this.WindowState = FormWindowState.Maximized;
            this.TopMost = true;
#if DEBUG
            this.TopMost = false;//Form2.xiti. zzz
#endif

            Xiti xiti = JsonOper.DeserializeJsonToObject<Xiti>(questionstr);
            m_answer = xiti.answer;
            string content = xiti.content;
            string html = "<div style=\"border-top:solid 3px #0B95C8; width:100%; margin-top:30px; padding-top:70px;\"><table align=\"center\"><tr><td style=\"font-family:微软雅黑; font-size:32px;\">" + content + "</td></tr></table></div>";
            this.webBrowser1.DocumentText = html;

            t = new System.Timers.Timer(200);
            t.Elapsed += new System.Timers.ElapsedEventHandler(Theout);
            t.Enabled = true;
            t.AutoReset = true;
        }

        private void SetPanel(string numberstr)
        {
            ClassInfo ci = JsonOper.DeserializeJsonToObject<ClassInfo>(numberstr);
            si = ci.Data.Student;
            int _count = screenWidth / 51;
            int _line = ci.Data.StudentCount % _count > 0 ? ci.Data.StudentCount / _count + 1 : ci.Data.StudentCount / _count;
            int panelHeight = _line * 50 + 10 ;///////////////////////
            if (_line == 2)
                panelHeight = _line * 50 + 60;
            else if (_line == 1)
                panelHeight = _line * 50 + 110;

            int panelLH = this.screenHeight - panelHeight;
            this.webBrowser1.Size = new System.Drawing.Size(screenWidth, screenHeight - panelHeight - 80);
            this.panel1.Location = new System.Drawing.Point(0, panelLH);
            this.panel1.Size = new System.Drawing.Size(this.screenWidth, panelHeight);

            this.panel2.Location = new System.Drawing.Point(0, panelLH);
            this.panel2.Size = new System.Drawing.Size(this.screenWidth, panelHeight);

            this.panel3.Location = new System.Drawing.Point(0, panelLH);
            this.panel3.Size = new System.Drawing.Size(this.screenWidth, panelHeight);

            int top3_top = (panelHeight - label_top3_1.Size.Height) / 2  -10 ;
            label_top3_1.Text = " ";
            label_top3_2.Text = " ";
            label_top3_3.Text = " ";
            label_top3_1.Top = top3_top;
            label_top3_2.Top = top3_top;
            label_top3_3.Top = top3_top;

            //"✔", "✘"
            int picWidth = 200;
            double picHightRatio = 0.12;
            pictureBox_A.Top = (int)(panelHeight * picHightRatio);
            pictureBox_B.Top = (int)(panelHeight * picHightRatio);
            pictureBox_C.Top = (int)(panelHeight * picHightRatio);
            pictureBox_D.Top = (int)(panelHeight * picHightRatio);
            pictureBox_R.Top = (int)(panelHeight * picHightRatio);
            pictureBox_W.Top = (int)(panelHeight * picHightRatio);
            double picLeft = 0.08;
            pictureBox_A.Left = (int)(screenWidth * (picLeft + 0.15*0));
            pictureBox_B.Left = (int)(screenWidth * (picLeft + 0.15*1));
            pictureBox_C.Left = (int)(screenWidth * (picLeft + 0.15*2));
            pictureBox_D.Left = (int)(screenWidth * (picLeft + 0.15*3));
            pictureBox_R.Left = (int)(screenWidth * (picLeft + 0.15*4));
            pictureBox_W.Left = (int)(screenWidth * (picLeft + 0.15*5));
            if (screenWidth <= 1024)
            {
                pictureBox_A.Left = 50;
                pictureBox_B.Left = 210;
                pictureBox_C.Left = 370;
                pictureBox_D.Left = 530;
                pictureBox_R.Left = 690;
                pictureBox_W.Left = 850;
            }

            int textLeftBias = 20;
            label_A.Left = pictureBox_A.Left - textLeftBias;
            label_B.Left = pictureBox_B.Left - textLeftBias;
            label_C.Left = pictureBox_C.Left - textLeftBias;
            label_D.Left = pictureBox_D.Left - textLeftBias;
            label_R.Left = pictureBox_R.Left - textLeftBias;
            label_W.Left = pictureBox_W.Left - textLeftBias;
            int textTopBias = 3;
            label_A.Top = textTopBias;
            label_B.Top = textTopBias;
            label_C.Top = textTopBias;
            label_D.Top = textTopBias;
            label_R.Top = textTopBias;
            label_W.Top = textTopBias;

            int text2LeftBias = 78;
            label_Ar.Left = pictureBox_A.Left + text2LeftBias;
            label_Br.Left = pictureBox_B.Left + text2LeftBias;
            label_Cr.Left = pictureBox_C.Left + text2LeftBias;
            label_Dr.Left = pictureBox_D.Left + text2LeftBias;
            label_Rr.Left = pictureBox_R.Left + text2LeftBias;
            label_Wr.Left = pictureBox_W.Left + text2LeftBias;
            label_Ar.BackColor = CircleBackgroundColor;
            label_Br.BackColor = CircleBackgroundColor;
            label_Cr.BackColor = CircleBackgroundColor;
            label_Dr.BackColor = CircleBackgroundColor;
            label_Rr.BackColor = CircleBackgroundColor;
            label_Wr.BackColor = CircleBackgroundColor;
            int text2TopBias = 60;
            label_Ar.Top = text2TopBias;
            label_Br.Top = text2TopBias;
            label_Cr.Top = text2TopBias;
            label_Dr.Top = text2TopBias;
            label_Rr.Top = text2TopBias;
            label_Wr.Top = text2TopBias;


            this.pictureBox_A.Size = new System.Drawing.Size(160, 160);
            this.pictureBox_B.Size = new System.Drawing.Size(160, 160);
            this.pictureBox_C.Size = new System.Drawing.Size(160, 160);
            this.pictureBox_D.Size = new System.Drawing.Size(160, 160);
            this.pictureBox_R.Size = new System.Drawing.Size(160, 160);
            this.pictureBox_W.Size = new System.Drawing.Size(160, 160);
            //pictureBox_A.Location = new System.Drawing.Point(picWidth, panelLH + 39);
            //pictureBox_B.Location = new System.Drawing.Point(picWidth * 2, panelLH + 39);
            //pictureBox_C.Location = new System.Drawing.Point(picWidth * 3, panelLH + 39);
            //pictureBox_D.Location = new System.Drawing.Point(picWidth * 4, panelLH + 39);
            //pictureBox_R.Location = new System.Drawing.Point(picWidth * 5, panelLH + 39);
            //pictureBox_W.Location = new System.Drawing.Point(picWidth * 6, panelLH + 39);
            statisticABCD();
            this.panel1.BringToFront();

            int highInterval = 10;//三行的行间距
            if (_line == 2)
                highInterval = 25;//两行变三行,两行的行间距
            else if(_line==1)
                highInterval = 55;

            int _lw = 0;
            int _br = 1;
            int _locationWidth = 0;
            int _locationHeight = 40;
            for (int i = 1; i <= ci.Data.StudentCount; i++)
            {
                _locationHeight = (_lw * 40) + ((_lw + 1) * highInterval);
                if (i == 1 || _br % 2 == 0)
                {
                    _locationWidth = (screenWidth - _count * 51) / 2 + 5;
                }
                else
                {
                    _locationWidth += 51;
                }
                if (i % _count == 0)
                {
                    _br *= 2;
                    if (_lw <= _line)
                    {
                        _lw++;
                    }
                }
                else
                {
                    _br=1;
                }
                PictureBox pic = new PictureBox();
                AnswerCount ac = new AnswerCount();
                ac.ImageWidth = 40;
                ac.ImagesHeight = 40;
                ac.FontStyle = System.Drawing.FontStyle.Bold;
                ac.AnswerFamily = "微软雅黑";
                ac.AnswerFontSize = 15.75F;
                Image lbimg = ac.DrawingArcFill(1, 1, System.Drawing.Color.FromArgb(254, 80, 79), 0, 41, System.Drawing.Color.FromArgb(254, 80, 79));
                Image textimg = ac.DrawingString(Brushes.White, i + "");
                bool ishasnum = false;

                int nStudentCount = ci.Data.Student.Length;
                for (int j = 0; j < nStudentCount; j++)
                {
                    string seat0 = ci.Data.Student[j].SEAT.Replace("-", "");
                    if (seat0 == "")
                        seat0 = "0";
                    int nSeat0 = Util.toInt(seat0);
                    if (i == nSeat0)
                    {
                        ishasnum = true;
                        break;
                    }
                }

                if (ishasnum)
                {
                    textimg = ac.DrawingString(Brushes.DimGray, i + "");
                    lbimg = ac.DrawingArcFill(1, 1, System.Drawing.Color.FromArgb(204, 204, 204), 2, 41, System.Drawing.Color.White);
                }
                pic.Location = new System.Drawing.Point(_locationWidth, _locationHeight);
                pic.Name = "click_" + i;
                pic.Size = new System.Drawing.Size(41, 41);
                pic.TabIndex = 0;
                pic.Image = lbimg;
                PictureBox text = new PictureBox();
                text.BackColor = System.Drawing.Color.Transparent;
                text.Location = new System.Drawing.Point(0, 0);
                text.Name = "text_" + i;
                text.Size = new System.Drawing.Size(41, 41);
                text.TabIndex = 0;
                text.Image = textimg;
                pic.Controls.Add(text);
                al.Add(pic);
                alText.Add(text);
                clickstate.Add(0);
                this.panel1.Controls.Add(pic);
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;//最小化
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void Theout(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (Interlocked.Exchange(ref inTimer, 1) == 0)
            {
                string data = Common.GetXitiResult();//Form2
                if (data != null && data.Length > 0)
                {
                    Log.Info("Form2_xiti.get=" + data);

                    DateTime tm_now = DateTime.Now;
                    TimeSpan createtimespan = new TimeSpan(tm_create.Ticks);
                    TimeSpan nowtimespan = new TimeSpan(tm_now.Ticks);
                    TimeSpan timespan = nowtimespan.Subtract(createtimespan).Duration();
                    int timeDiff = timespan.Minutes * 60 + timespan.Seconds;

                    for (int i = 0; i < data.Split('|').Length; i++)
                    {
                        int num = Convert.ToInt16(data.Split('|')[i].Split(':')[0].ToString().Replace("-", ""));
                        string answer = data.Split('|')[i].Split(':')[1];
                        string context = num + ":" + answer + ":" + timeDiff;
                        if (m_hashtable.Contains(num))
                        {
                            m_hashtable.Remove(num);
                        }
                        m_hashtable.Add(num, answer + ":" + timeDiff);
                        //Httpd.setPracticeResult(context);//同步给httpd
                        statisticABCD();

                        for (int j = 0; j < al.Count; j++)
                        {
                            PictureBox lb = (PictureBox)al[j];
                            PictureBox text = (PictureBox)alText[j];
                            if ((num - 1) == j )//&& (int)clickstate[j] == 0
                            {
                                Log.Info("Form2_xiti. LabelStateEvent index=" + (j + 1));
                                LabelStateEvent(context, lb, text, j + 1);
                                clickstate[j] = 1;
                            }
                        }
                    }
                }
                Interlocked.Exchange(ref inTimer, 0); 
            }
        }

        public void LabelStateEvent(string context, PictureBox pic, PictureBox text,int i)
        {
            if (pic.InvokeRequired)
            {
                InvokeLabelState labelCallback = new InvokeLabelState(LabelStateEvent);
                pic.Invoke(labelCallback, new object[] { context, pic, text, i });
            }
            else
            {
                AnswerCount ac = new AnswerCount();
                ac.ImageWidth = 40;
                ac.ImagesHeight = 40;
                ac.FontStyle = System.Drawing.FontStyle.Bold;
                ac.AnswerFamily = "微软雅黑";
                ac.AnswerFontSize = 15.75F;
                pic.Image = ac.DrawingArcFill(1, 1, System.Drawing.Color.FromArgb(69, 175, 101), 0, 41, System.Drawing.Color.FromArgb(69, 175, 101));
                text.Image = ac.DrawingString(Brushes.White, i + "");
                if(Global.Sound())
                {
                    System.Media.SystemSounds.Asterisk.Play();
                    //sp.Play();
                }
                updateTop3(i, context);
            }
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            Win32.AnimateWindow(this.Handle, 200, Win32.AW_SLIDE | Win32.AW_HIDE | Win32.AW_BLEND);
        }
       

        public string getResult()
        {
            string dir = Application.StartupPath + "\\"+DateTime.Now.ToString("yyyyMMdd")+"\\";
            string filename = _id + ".txt";
            string result="";
            ArrayList akeys=new ArrayList(m_hashtable.Keys);
            akeys.Sort(); //按字母顺序进行排序
            foreach(int skey in akeys)
            {
                int uid = skey;
                if (skey < 100)
                    uid = Global.getUidBySeat(skey);
                string pair = uid + ":" + m_hashtable[skey];
                result += (result.Length > 0 ? "," : "") + pair;
            }

            FileOper fo = new FileOper(dir, filename);
            fo.WriteFile(result);
            return result;
        }

        public void setAnswer(string answer)
        {
            m_answer = answer;
            updateTop3Status();
        }
        private void updateTop3Status()
        {
            Color colorWrong = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(72)))), ((int)(((byte)(86)))));
            Color colorRight = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(187)))), ((int)(((byte)(98)))));

            if (m_answer == "")
            {
                label_top3_1.Text = top3_name1;
                label_top3_2.Text = top3_name2;
                label_top3_3.Text = top3_name3;
                label_top3_1.ForeColor = colorWrong;
                label_top3_2.ForeColor = colorWrong;
                label_top3_3.ForeColor = colorWrong;
            }
            else
            {
                if (top3_answer1.Length > 0)
                {
                    if (top3_answer1 == m_answer && top3_answer1.Length > 0)
                    {
                        label_top3_1.ForeColor = colorRight;
                        label_top3_1.Text = top3_name1 + "(✔)";
                    }
                    else
                    {
                        label_top3_1.ForeColor = colorWrong;
                        label_top3_1.Text = top3_name1 + "(✘)";
                    }
                }

                if (top3_answer2.Length > 0)
                {
                    if (top3_answer2 == m_answer && top3_answer2.Length > 0)
                    {
                        label_top3_2.ForeColor = colorRight;
                        label_top3_2.Text = top3_name2 + "(✔)";
                    }
                    else
                    {
                        label_top3_2.ForeColor = colorWrong;
                        label_top3_2.Text = top3_name2 + "(✘)";
                    }
                }

                if (top3_answer3.Length > 0)
                {
                    if (top3_answer3 == m_answer)
                    {
                        label_top3_3.ForeColor = colorRight;
                        label_top3_3.Text = top3_name3 + "(✔)";
                    }
                    else
                    {
                        label_top3_3.ForeColor = colorWrong;
                        label_top3_3.Text = top3_name3 + "(✘)";
                    }
                }

            }

            int left1 = screenWidth / 3 - label_top3_1.Size.Width / 2 - 50;
            int left2 = (screenWidth - label_top3_2.Size.Width) / 2;
            int left3 = screenWidth * 2 / 3 - label_top3_3.Size.Width / 2 + 50;
            label_top3_1.Left = left1;
            label_top3_2.Left = left2;
            label_top3_3.Left = left3;
        }

        private void updateTop3(int index, string context)
        {
            string[] szBuf = context.Split(':');
            string answer = "";
            if (szBuf.Length == 3)
                answer = szBuf[1];
            string result = "";

            //①②③
            string name = si[index - 1].Name;// +answer;
            if (label_top3_1.Text.Length <= 1)
            {
                top3_name1 = "① " + name;
                top3_answer1 = answer;
            }
            else if (label_top3_2.Text.Length <= 1)
            {
                top3_name2 = "② " + name;
                top3_answer2 = answer;
            }
            else if (label_top3_3.Text.Length <= 1)
            {
                top3_name3 = "③ " + name;
                top3_answer3 = answer;
            }

            updateTop3Status();
        }

        private void statisticABCD()
        {
            ArrayList akeys = new ArrayList(m_hashtable.Keys);
            akeys.Sort(); //按字母顺序进行排序
            int nA = 0, nB = 0, nC = 0, nD = 0, nR = 0, nW = 0;
            foreach (int skey in akeys)
            {
                string value = (string)m_hashtable[skey];
                string[] szV = value.Split(':');
                string KEYs = szV[0];
                for(int i=0; i<KEYs.Length; i++)
                {
                    char ch = KEYs.ElementAt(i);
                    if (ch == 'A')
                        nA++;
                    else if (ch == 'B')
                        nB++;
                    else if (ch == 'C')
                        nC++;
                    else if (ch == 'D')
                        nD++;
                    else if (ch == 'R')
                        nR++;
                    else if (ch == 'W')
                        nW++;
                }
            }
            try
            {
                DrawingArcs(label_A, "A", nA, si.Length, label_Ar);
                DrawingArcs(label_B, "B", nB, si.Length, label_Br);
                DrawingArcs(label_C, "C", nC, si.Length, label_Cr);
                DrawingArcs(label_D, "D", nD, si.Length, label_Dr);
                DrawingArcs(label_R, "R", nR, si.Length, label_Rr);
                DrawingArcs(label_W, "W", nW, si.Length, label_Wr);
            }
            catch (Exception e1)
            {
                Log.Error(e1.Message);
            }
        }
        public void SwitchView(int index)
        {
            this.Show();
            this.BringToFront();
            Log.Info("**************SwitchView************");
            SetDesktopLocation(0, screenHeight - this.Height);
            if (index == 1)
            {
                AnimateWindow(this.panel1.Handle, 200, AW_U2D);//AW_U2D//AW_L2R
                this.panel1.BringToFront();
            }
            else if (index == 2)
            {
                AnimateWindow(this.panel2.Handle, 200, AW_R2L);//AW_D2U//AW_R2L
                this.panel2.BringToFront();
            }
            else if (index == 3)
            {
                //this.panel3.BringToFront();//答题前三名
                //AnimateWindow(this.panel3.Handle, 200, AW_L2R);//AW_D2U//AW_R2L
            }
        }
        public void DrawingArcs(Label label, string KEY, int n, int Total, Label labelr)
        {
            if (label.InvokeRequired)
            {
                InvokeDrawingArcs labelCallback = new InvokeDrawingArcs(DrawingArcs);
                label.Invoke(labelCallback, new object[] { label, KEY, n, Total ,labelr});
            }
            else
            {
                label_A.BringToFront();
                label_B.BringToFront();
                label_C.BringToFront();
                label_D.BringToFront();
                label_W.BringToFront();
                label_R.BringToFront();

                //Log.Info("DrawingArcs Key=" + KEY + ", n=" + n + ", Total=" + Total);
                int r = 120;
                double ratio = (double)n / Total;
                if (ratio > 100)
                    ratio = 100.00;
                string strRatio = string.Format("{0:0.0%}", ratio);//得到5.88%

                AnswerCount ac = new AnswerCount();
                ac.AnswerBackColor = System.Drawing.Color.FromArgb(212, 214, 213);
                ac.AnswerBorderColor = System.Drawing.Color.FromArgb(0, 0, 0);
                ac.AnswerBorderWidth = 10;
                ac.AnswerW = r + 20;
                ac.AnswerH = r + 20;
                ac.AnswerX = 4;
                ac.AnswerY = 4;
                float nArc = (float)n * 360 / Total;
                ac.AnswerSweep = (int)nArc;

                Color colorBackground = CircleBackgroundColor;
                Color colorText = System.Drawing.Color.FromArgb(249, 142, 56);

                string text1 = "" + n;
                //if (n == 0)
                //    text1 = KEY;

                Image img = ac.DrawingArcWithText(r, colorBackground, colorText, text1, strRatio);
                if (KEY == "A")
                    pictureBox_A.Image = img;
                else if (KEY == "B")
                    pictureBox_B.Image = img;
                else if (KEY == "C")
                    pictureBox_C.Image = img;
                else if (KEY == "D")
                    pictureBox_D.Image = img;
                else if (KEY == "R")
                    pictureBox_R.Image = img;
                else if (KEY == "W")
                    pictureBox_W.Image = img;
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
        public string GetCriticized()
        {
            return _criticizeStr;
        }
        public void AppendCallname(int uid)
        {
            if (_callnamsStr.IndexOf(uid + ":") >= 0)
                return;

            DateTime tm_now = DateTime.Now;
            TimeSpan createtimespan = new TimeSpan(tm_create.Ticks);
            TimeSpan nowtimespan = new TimeSpan(tm_now.Ticks);
            TimeSpan timespan = nowtimespan.Subtract(createtimespan).Duration();
            int timeDiff = timespan.Minutes * 60 + timespan.Seconds;

            if (uid > 0)
            {
                string pair = uid + ":" + timeDiff;
                if (_callnamsStr.Length > 0)
                    _callnamsStr += ",";
                _callnamsStr += pair;
            }
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

            if(uid > 0)
            {
                string pair = uid + ":" + timeDiff;
                if (_criticizeStr.Length > 0)
                    _criticizeStr += ",";
                _criticizeStr += pair;
            }

        }
        public void AppendReward(string uid, int point,string reason,string reasonid)
        {
            if (_rewardStr.IndexOf(uid + ":") >= 0)
                return;

            DateTime tm_now = DateTime.Now;
            TimeSpan createtimespan = new TimeSpan(tm_create.Ticks);
            TimeSpan nowtimespan = new TimeSpan(tm_now.Ticks);
            TimeSpan timespan = nowtimespan.Subtract(createtimespan).Duration();
            int timeDiff = timespan.Minutes * 60 + timespan.Seconds;

            int nId = Util.toInt(uid);
            if (nId < 100)
                nId = Global.getUidBySeat(nId);
            if(nId>0)
            {
                string pair = nId + ":" + timeDiff + ":" + point + ":" + reason + ":" + reasonid;
                if (_rewardStr.Length > 0)
                    _rewardStr += ",";
                _rewardStr += pair;
            }
        }

        private void panel1_Click(object sender, EventArgs e)
        {

        }

        private void panel1_DoubleClick(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void panel2_DoubleClick(object sender, EventArgs e)
        {
            this.Hide();
            Global.panelshow = 0;
        }

    }

}
