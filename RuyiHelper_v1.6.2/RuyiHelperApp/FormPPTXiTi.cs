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
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace RueHelper
{
    public delegate void InvokeCompeitiveLabelState_Xiti(string context, PictureBox pic, PictureBox text, int i);
    public delegate void InvokeCompeitiveReadFile_Xiti();
    //todo
    public delegate void InvokeCompeitiveLBLTime(int time);

    public partial class FormPPTXiTi : Form
    {
        private static log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        string _xitiId = "";
        int screenWidth = Screen.PrimaryScreen.Bounds.Width;
        int screenHeight = Screen.PrimaryScreen.Bounds.Height;
        ArrayList al;
        ArrayList alText;
        ArrayList clickstate;
        public ArrayList _rightList;
        public Form11 f11;

        public System.Timers.Timer t;
        public System.Timers.Timer t1;
        int inTimer = 0;
        //int inTimer_Competitive = 0;
        StudentInfo[] si;

        //todo
        public System.Timers.Timer tlbl;
        int lbltimeint = 0;
        public DateTime tm_create = DateTime.Now;

        int mode = 0;//0：无题抢答，数秒； 1：PPT抢答
        private Hashtable m_hashtable = new Hashtable(); //保存学生按键数据
        string _req_answer;

        public FormPPTXiTi(string numberstr,int m)
        {
            mode = m;
            al = new ArrayList();
            alText = new ArrayList();
            clickstate = new ArrayList();

            //创建习题ID
            _xitiId = Global.getSchoolID() + "-" + Global.getClassID() + "-" + DateTime.Now.ToString("yyyyMMddHHmmss");

            //TODO: 截屏上传
            if(m==1)
            {
                _xitiId = "T_" + _xitiId;
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
            int panelH = SetPanel(numberstr);
            this.Text = "抢答";

            if(mode==1)//PPT 抢答
            {
                this.Height = panelH;
                this.Width = screenWidth;
                
                StartPosition = FormStartPosition.Manual;
                SetDesktopLocation(0, screenHeight - this.Height);

                label1.Visible = false;
                label3.Visible = false;
                lbltime.Visible = false;
            }
            else if(mode==0)
            {
                this.WindowState = FormWindowState.Maximized;
                pictureBox1.Visible = true;

                //--------
                int posY_timer = (screenHeight - lbltime.Height) / 2;//居中
                label1.Location = new Point((screenWidth - label1.Width) / 2, posY_timer - lbltime.Height);
                lbltime.Location = new Point((screenWidth - lbltime.Width) / 2, posY_timer);
                label3.Location = new Point((screenWidth + lbltime.Width + label3.Width) / 2, posY_timer + lbltime.Height-label3.Height);

                //todo
                tlbl = new System.Timers.Timer(1000);
                tlbl.Elapsed += new System.Timers.ElapsedEventHandler(lblstart);
                tlbl.Enabled = true;
                tlbl.AutoReset = true;
            }

            this.TopMost = true;
#if DEBUG
            this.TopMost = false;//PPTXiti,zzz
#endif

            //this.WindowState = FormWindowState.Maximized;
            this.Hide();
            this.Show();
            this.BringToFront();

            t = new System.Timers.Timer(200);
            t.Elapsed += new System.Timers.ElapsedEventHandler(Theout);
            t.Enabled = true;
            t.AutoReset = true;

            pictureBox2.Visible = false;
            pictureBox3.Visible = false;
            pictureBox4.Visible = false;
            //pictureBox3.Location = new System.Drawing.Point(screenWidth - 60, 0);
            //pictureBox4.Location = new System.Drawing.Point(screenWidth - 30, 0);



        }

        /// <summary>
        /// 无题抢答，开始数秒
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblstart(object sender, System.Timers.ElapsedEventArgs e)
        {
            lbltimeint++;
            changtimelbl(lbltimeint);
        }

        /// <summary>
        /// 无题抢答，更改秒数
        /// </summary>
        /// <param name="t"></param>
        public void changtimelbl(int t)
        {
            if (lbltime.InvokeRequired)
            {
                InvokeCompeitiveLBLTime labelCallback = new InvokeCompeitiveLBLTime(changtimelbl);
                lbltime.Invoke(labelCallback, new object[] { t });
            }
            else
            {
                lbltime.Text = t.ToString();

                int posY_timer = (screenHeight - lbltime.Height) / 2;//居中
                lbltime.Location = new Point((screenWidth - lbltime.Width) / 2, posY_timer);
                label3.Location = new Point((screenWidth + lbltime.Width + label3.Width) / 2, posY_timer + lbltime.Height - label3.Height);

            }
        }

        private int SetPanel(string numberstr)
        {
            ClassInfo ci = JsonOper.DeserializeJsonToObject<ClassInfo>(numberstr);
            si = ci.Data.Student;
            int _count = screenWidth / 51;
            int _line = ci.Data.StudentCount % _count > 0 ? ci.Data.StudentCount / _count + 1 : ci.Data.StudentCount / _count;

            int panelHeight = _line * 50 + 10;
            int panelLH = this.screenHeight - panelHeight;//上半部分的高度

            if(mode==0)//PPT抢答
            {
                this.panel1.Location = new System.Drawing.Point(0, panelLH); //相对于form的位置坐标
            }
            else
            {
                this.panel1.Location = new System.Drawing.Point(0, 0);//跟form同高
            }
            
            this.panel1.Size = new System.Drawing.Size(this.screenWidth, panelHeight);
            
            int _lw = 0;
            int _br = 1;
            int _locationWidth = 0;
            int _locationHeight = 40;
            for (int i = 1; i <= ci.Data.StudentCount; i++)
            {
                _locationHeight = (_lw * 40) + ((_lw + 1) * 10);
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
                    _br = 1;
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
            return panelHeight;
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
                string data = Common.GetXitiResult();//FormPPTXiti
                if (data.Length > 0)
                {
                    DateTime tm_now = DateTime.Now;
                    TimeSpan createtimespan = new TimeSpan(tm_create.Ticks);
                    TimeSpan nowtimespan = new TimeSpan(tm_now.Ticks);
                    TimeSpan timespan = nowtimespan.Subtract(createtimespan).Duration();
                    int timeDiff = timespan.Minutes * 60 + timespan.Seconds;

                    for (int i = 0; i < data.Split('|').Length; i++)
                    {
                        int num = Convert.ToInt16(data.Split('|')[i].Split(':')[0].ToString().Replace("-", ""));
                        string answer = data.Split('|')[i].Split(':')[1];
                        string context = num + ":" + answer;
                        if (m_hashtable.Contains(num))
                        {
                            m_hashtable.Remove(num);
                        }
                        m_hashtable.Add(num, answer + ":" + timeDiff);
                        //Httpd.setPracticeResult(context);//同步给httpd

                        Log.Info("context=" + context);
                        for (int j = 0; j < al.Count; j++)
                        {
                            PictureBox lb = (PictureBox)al[j];
                            PictureBox text = (PictureBox)alText[j];
                            if ((num - 1) == j && (int)clickstate[j] == 0)
                            {
                                LabelStateEvent(context, lb, text, j + 1);
                                clickstate[j] = 1;
                            }
                        }
                    }
                }
                Interlocked.Exchange(ref inTimer, 0);
            }
        }

        public void LabelStateEvent(string context, PictureBox pic, PictureBox text, int i)
        {
            if (pic.InvokeRequired)
            {
                InvokeCompeitiveLabelState_Xiti labelCallback = new InvokeCompeitiveLabelState_Xiti(LabelStateEvent);
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
                if (Global.Sound())
                {
                    System.Media.SystemSounds.Asterisk.Play();
                    //sp.Play();
                }
            }
        }

        private void CloseForm(object sender, FormClosingEventArgs e)
        {
            Win32.AnimateWindow(this.Handle, 200, Win32.AW_SLIDE | Win32.AW_HIDE | Win32.AW_BLEND);
        }
        
        public string getResult()
        {
            string result = "";
            ArrayList akeys = new ArrayList(m_hashtable.Keys);
            akeys.Sort(); //按字母顺序进行排序
            foreach (int skey in akeys)
            {
                string pair = skey + ":" + m_hashtable[skey];
                result += (result.Length > 0 ? "," : "") + pair;
            }

            string dir = Application.StartupPath + "\\" + DateTime.Now.ToString("yyyyMMdd") + "\\";
            string filename = _xitiId + ".txt";
            FileOper fo = new FileOper(dir, filename);
            fo.WriteFile(result);
            return result;
        }
    }

}
