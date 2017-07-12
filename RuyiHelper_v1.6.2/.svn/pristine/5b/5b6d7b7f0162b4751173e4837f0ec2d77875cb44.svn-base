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


namespace RueHelper
{
    public delegate void InvokeCompeitiveLabelState(string context, PictureBox pic, PictureBox text, int i);
    public delegate void InvokeCompeitiveReadFile();
    public partial class Form13 : Form
    {
        private static Log log = new Log();
        int screenWidth = Screen.PrimaryScreen.Bounds.Width;
        int screenHeight = Screen.PrimaryScreen.Bounds.Height;
        ArrayList al;
        ArrayList alText;
        ArrayList clickstate;
        public ArrayList _rightList;
        public Form11 f11;
        FileOper fo = new FileOper();

        public System.Timers.Timer t;
        public System.Timers.Timer t1;
        int inTimer = 0;
        string _xitiId = "";
        //int inTimer_Competitive = 0;
        StudentInfo[] si;
        public Form13(string numberstr)
        {
            al = new ArrayList();
            alText = new ArrayList();
            clickstate = new ArrayList();

            //No xiti.id
            _xitiId = Global.getSchoolID() + "-" + Global.getClassroomID() + "-" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
            fo.DirName = DateTime.Now.ToString("yyyyMMdd");
            fo.FilePath = Application.StartupPath + "\\" + fo.DirName + "\\";
            fo.FileName = _xitiId + ".txt";
            
            fo.DeleteFile();
            fo.CreateDir();
            fo.CreateFile();
            InitializeComponent();
            SetPanel(numberstr);
            this.Text = "抢答";
            this.WindowState = FormWindowState.Maximized;
            this.TopMost = true;
#if DEBUG
            this.TopMost = false;//Form13 无题抢答
#endif


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
            int panelHeight = _line * 50 + 10;
            int panelLH = this.screenHeight - panelHeight;
            this.panel1.Location = new System.Drawing.Point(0, panelLH);
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

                string[] szStudentNumber = ci.Data.StudentNumbers.Split(',');
                int nStudent = szStudentNumber.Length;

                for (int j = 1; j <= nStudent; j++)
                {
                    if (i == j)
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
                string json = HTTPReq.HttpGet(Global.url_recv + "action=xiti.get&classid=0&lessonid=0&xitiid=0");
                if(json.Length>55)
                    log.Info("Theout() xiti.get=" + json);
                CBInfo cb = JsonOper.DeserializeJsonToObject<CBInfo>(json.Replace("(", "").Replace(")", ""));
                if (cb != null && cb.data.Length > 0)
                {
                    string data = cb.data;
                    string context = "";
                    for (int i = 0; i < data.Split('|').Length - 1; i++)
                    {
                        int num = Convert.ToInt16(data.Split('|')[i].Split(':')[0].ToString().Replace("-", ""));
                        string answer = data.Split('|')[i].Split(':')[1];
                        int _answerLen = answer.Split(';').Length;
                        string getAnswer = "";
                        if (_answerLen == 1)
                        {
                            getAnswer = answer.Split(';')[_answerLen - 1].Split(',')[1];
                        }
                        else
                        {
                            getAnswer = answer.Split(';')[_answerLen - 2].Split(',')[1];
                        }
                        context = num + ":" + getAnswer;
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
                InvokeCompeitiveLabelState labelCallback = new InvokeCompeitiveLabelState(LabelStateEvent);
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
                System.Media.SystemSounds.Asterisk.Play();
                //sp.Play();
                fo.Context = context+",";
                fo.WriteFile();
            }
        }

        private void CLoseForm(object sender, FormClosingEventArgs e)
        {
            Win32.AnimateWindow(this.Handle, 200, Win32.AW_SLIDE | Win32.AW_HIDE | Win32.AW_BLEND);
        }
        string _req_answer;
        public void Competitive(string req_answer)
        {
            _req_answer = req_answer;
            t1 = new System.Timers.Timer(200);
            t1.Elapsed += new System.Timers.ElapsedEventHandler(CompetitiveTheout);
            t1.Enabled = true;
            t1.AutoReset = true;
        }

        private void CompetitiveTheout(object sender, System.Timers.ElapsedEventArgs e)
        {
            ReadFileEvent();
        }

        public void ReadFileEvent()
        {
            if (this.InvokeRequired)
            {
                InvokeCompeitiveReadFile readCallback = new InvokeCompeitiveReadFile(ReadFileEvent);
                this.Invoke(readCallback, new object[] { });
            }
            else
            {
                string answerStr = fo.ReadFile();
                _rightList = new ArrayList();
                for (int i = 0; i < answerStr.Split(',').Length; i++)
                {
                    if (answerStr.Split(',')[i] != "" && answerStr.Split(',')[i].Split(':')[1] == _req_answer)
                    {
                        _rightList.Add(si[i].Name);
                    }
                }
                if (_rightList.Count > 2 || (answerStr.Split(',').Length - 1) == si.Length)
                {
                    t1.Enabled = false;
                    t.Enabled = false;

                    if(f11==null)
                    {
                        Common.uploadCompetitiveAnswer(_xitiId, _req_answer, answerStr, _rightList);

                        f11 = new Form11(_req_answer, _rightList);
                        f11.Show();
                    }
                    else
                    {

                    }
                }
            }
        }

        public void StopT(string req_answer)
        {
            t1.Enabled = false;
            t.Enabled = false;
            string answerStr = fo.ReadFile();
            _rightList = new ArrayList();
            for (int i = 0; i < answerStr.Split(',').Length; i++)
            {
                if (answerStr.Split(',')[i] != "" && answerStr.Split(',')[i].Split(':')[1] == _req_answer)
                {
                    _rightList.Add(si[i].Name);
                }
            }
            f11 = new Form11(req_answer, _rightList);
            f11.Show();
        }

    }

}
