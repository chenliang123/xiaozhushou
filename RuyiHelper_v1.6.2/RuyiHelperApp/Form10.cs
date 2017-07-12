using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Configuration;
using System.Media;

namespace RueHelper
{
    public partial class Form10 : Form
    {
        private static log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        int screenWidth = Screen.PrimaryScreen.Bounds.Width;
        int screenHeight = Screen.PrimaryScreen.Bounds.Height;
        System.Media.SoundPlayer sp_clap = new SoundPlayer(RueHelper.Properties.Resources.clap); 
        ArrayList al;
        StudentInfo[] si;
        public string STName { get; set; }
        public string STID { get; set; }
        public string STSEAT { get; set; }
        public static List<StudentInfo> studentResult = new List<StudentInfo>();
        public bool bHighSchool = false;
        public Form10(string studentinfo, string data, int id, string name)
        {
            InitializeComponent();
            al = new ArrayList();
            this.Text = "点名";
            this.WindowState = FormWindowState.Maximized;

            this.TopMost = true;
#if DEBUG
            this.TopMost = false;//Form10 CallName,zzz
#endif
            if(Global.getGrade() > 6)
            {
                bHighSchool = true;
            }

            if(bHighSchool)
            {
                //点名提问 姓名框
                this.label1.Size = new System.Drawing.Size(401, 174);//394,167
                this.label1.Image = global::RueHelper.Properties.Resources.nameshow;
                this.label1.Location = new System.Drawing.Point((screenWidth - this.label1.Width) / 2, (screenHeight - this.label1.Height) / 2 + 30);
                //this.label1.ForeColor = System.Drawing.Color.Black;
                //this.label1.ForeColor = System.Drawing.ColorTranslator.FromHtml("#5748A5");
                //this.label1.Font = new System.Drawing.Font("微软雅黑", 42F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));

                //点名提问图片框
                this.pictureBox1.Visible = false;
                this.pictureBox1.Location = new System.Drawing.Point((screenWidth - this.pictureBox1.Width) / 2 - 10, label1.Top - pictureBox1.Height - 20);

                //点名提问文本框
                this.label3.Visible = true;
                this.label3.Location = new System.Drawing.Point((screenWidth - 219) / 2, label1.Top - 90);
                this.label3.Text = "点 名 提 问";
                this.label3.Font = new System.Drawing.Font("微软雅黑", 30F);

                this.label2.Visible = false;
            }
            else
            {
                //点名提问图片框
                this.pictureBox1.Visible = true;
                this.pictureBox1.Location = new System.Drawing.Point((screenWidth - this.pictureBox1.Width) / 2 - 10, label1.Top - pictureBox1.Height - 20);

                this.label1.Size = new System.Drawing.Size(408, 179);//401,172
                this.label1.Location = new System.Drawing.Point((screenWidth - this.label1.Width) / 2, (screenHeight - this.label1.Height) / 2 + 50);
                this.label2.Location = new System.Drawing.Point(label1.Left, label1.Top);

                this.pictureBox2.Location = new System.Drawing.Point((screenWidth - this.pictureBox2.Width) / 2, (screenHeight - this.pictureBox2.Height) / 2 - 200);
                this.label3.Size = new System.Drawing.Size(288, 50);
                this.label3.Location = new System.Drawing.Point((screenWidth - this.label3.Width) / 2 + 10, pictureBox1.Top + 30);
                this.label1.Image = global::RueHelper.Properties.Resources.nameshow;
            }
            
            ClassInfo ci = JsonOper.DeserializeJsonToObject<ClassInfo>(studentinfo);
            si = ci.Data.Student;

            if(name.Length==0 && data.Length > 0)//随机点名
            {
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
                    al.Add(num + ":" + getAnswer);
                }
                Random rd = new Random();
                ArrayList _tempAl = new ArrayList();
                for (int i = 0; i < al.Count; i++)
                {
                    int _num = Convert.ToInt16(al[i].ToString().Split(':')[0].ToString());
                    string s1 = si[_num - 1].Name;
                    if (CheckAL(s1))
                    {
                        _tempAl.Add(al[i].ToString());
                    }
                }
                bool insert = true;
                if (_tempAl.Count == 0)
                {
                    insert = false;
                    _tempAl = al;
                }
                int r = rd.Next(_tempAl.Count);
                string temp = _tempAl[r].ToString();
                int number = Convert.ToInt16(temp.Split(':')[0].ToString());//No. 
                string stname = si[number - 1].Name;
                this.label1.Text = stname;
                STName = stname;
                STID = temp.Split(':')[0].ToString();
                if (insert)
                {
                    StudentInfo newSI = new StudentInfo();
                    newSI.Name = stname;
                    studentResult.Add(newSI);
                }
            }
            else//制定点名
            {
                STName = name;
                STID = id + "";
            }
            this.label1.Text = STName;
            //this.Show();
            //this.BringToFront();


            //3秒显示，自动隐藏
            System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
            t.Interval = 2000;//2秒
            t.Tick += new EventHandler(t_Tick);
            t.Start();
        }
        public void ShowCallname()
        {

        }
        public void RewardShow()
        {
            if (Global.playReward())
            {
                //System.Media.SystemSounds.Asterisk.Play();
                sp_clap.Play();
            }

            this.Visible = true;
            this.label2.Text = STName;
            this.label1.Visible = false;
            this.pictureBox1.Visible = false;
            this.label2.Visible = true;
            this.pictureBox2.Visible = true;

            
            this.label3.Visible = true;
            this.label3.Size = new System.Drawing.Size(288, 50);
            this.label3.Location = new System.Drawing.Point((screenWidth - this.label3.Width) / 2 + 10, pictureBox1.Top + 30);
            this.label3.Text = "恭喜获得奖励";

            Random rd = new Random();

            int picBoxHight=0,picBoxWidth=0;
            int rand = rd.Next(7)+1;//[0,6]
            switch (rand)
            {
                case 1:
                    this.pictureBox2.Image = global::RueHelper.Properties.Resources.reward_1;
                    break;
                case 2:
                    this.pictureBox2.Image = global::RueHelper.Properties.Resources.reward_2;
                    break;
                case 3:
                    this.pictureBox2.Image = global::RueHelper.Properties.Resources.reward_3;
                    break;
                case 4:
                    this.pictureBox2.Image = global::RueHelper.Properties.Resources.reward_4;
                    break;
                case 5:
                    this.pictureBox2.Image = global::RueHelper.Properties.Resources.reward_5;
                    break;
                case 6:
                    this.pictureBox2.Image = global::RueHelper.Properties.Resources.reward_6;
                    break;
                case 7:
                    this.pictureBox2.Image = global::RueHelper.Properties.Resources.reward_7;
                    break;
                default :
                    this.pictureBox2.Image = global::RueHelper.Properties.Resources.reward_1;
                    break;
            }
            picBoxHight = pictureBox2.Image.Height;
            picBoxWidth = pictureBox2.Image.Width;
            this.pictureBox2.Size = new System.Drawing.Size(picBoxWidth, picBoxHight);
            this.pictureBox2.Location = new System.Drawing.Point((screenWidth - this.pictureBox2.Width) / 2, label2.Top - pictureBox2.Height-80);

            System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
            t.Interval = 2000;
            t.Tick += new EventHandler(t_Tick_Close);
            t.Start();
        }

        void t_Tick(object sender, EventArgs e)
        {
            this.Visible = false;
            ((System.Windows.Forms.Timer)sender).Stop();
        }
        void t_Tick_Close(object sender, EventArgs e)
        {
            //System.Threading.Thread.CurrentThread.Abort();
            //System.Environment.Exit(System.Environment.ExitCode);//主程序都退出了。。。
            this.Dispose();
            this.Close();
        }

        private bool CheckAL(string name)
        {
            bool b = true;
            for (int j = 0; j < studentResult.Count; j++)
            {
                if (name == studentResult[j].Name)
                {
                    b = false;
                    break;
                }
            }
            return b;
        }

    }
}
