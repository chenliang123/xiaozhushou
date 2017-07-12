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
using RueHelper.model;
using Newtonsoft.Json.Linq;
using RueHelper.util;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace RueHelper
{
    public partial class FormAward : Form
    {
        private static log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        int screenWidth = Screen.PrimaryScreen.Bounds.Width;
        int screenHeight = Screen.PrimaryScreen.Bounds.Height;
        System.Media.SoundPlayer sp_clap = new SoundPlayer(RueHelper.Properties.Resources.clap); 
        ArrayList al;
        StudentInfo[] si;
        private int point = 0;
        private bool bHighSchool = false;
        public string STName { get; set; }
        public string STID { get; set; }
        public string STSEAT { get; set; }
        public static List<StudentInfo> studentResult = new List<StudentInfo>();
        public FormAward(int id, string name, int point,string reason,string reasonid)
        {
            InitializeComponent();
            al = new ArrayList();
            this.Text = "奖励";
            this.WindowState = FormWindowState.Maximized;
            this.Show();
            this.BringToFront();
            this.TopMost = true;
            this.point = point;
#if DEBUG
            this.TopMost = false;//Form10 CallName,zzz
#endif
            int label2Width = 500;
            this.pictureBox2.Location = new System.Drawing.Point((screenWidth - this.pictureBox2.Width) / 2, (screenHeight - this.pictureBox2.Height) / 2 - 170);
            this.label2.Location = new System.Drawing.Point((screenWidth - label2Width) / 2, screenHeight/ 2 - 50);

            this.label2.Text = name;
            STName = name;
            STID = id + "";

            int grade = Global.getGrade();
            if(grade > 6)
            {
                bHighSchool = true;
            }
            ////自动隐藏
            //System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
            //t.Interval = 4000;
            //t.Tick += new EventHandler(t_Tick);
            //t.Start();
        }
        public void RewardShow()
        {
            if (!bHighSchool)
                RewardStar_PrimarySchool();
            else
                RewardStar_HighSchool2();
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

        private void RewardStar_PrimarySchool()
        {
            if (Global.playReward())
            {
                SetVol(0.4);
                //System.Media.SystemSounds.Asterisk.Play();
                sp_clap.Play();

            }

            this.Visible = true;
            this.label1.Visible = false;
            this.label2.Text = STName;
            this.label2.Visible = true;

            this.pictureBox1.Image = global::RueHelper.Properties.Resources.jiangli;
            this.pictureBox1.Location = new System.Drawing.Point(37, 218);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(397, 189);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;

            this.pictureBox2.Visible = true;
            this.pictureBox2.Size = new System.Drawing.Size(391, 200);
            this.pictureBox2.Location = new System.Drawing.Point((screenWidth - this.pictureBox2.Width) / 2, (screenHeight - this.pictureBox2.Height) / 2 - 170);

            Random rd = new Random();


            int rand = rd.Next(7) + 1;//[0,6]
            switch (point)
            {
                case 1:
                    this.pictureBox2.Image = global::RueHelper.Properties.Resources.star1_1;
                    break;
                case 2:
                    this.pictureBox2.Image = global::RueHelper.Properties.Resources.star1_2;
                    break;
                case 3:
                    this.pictureBox2.Image = global::RueHelper.Properties.Resources.star1_3;
                    break;
                case 4:
                    this.pictureBox2.Image = global::RueHelper.Properties.Resources.star1_4;
                    break;
                case 5:
                    this.pictureBox2.Image = global::RueHelper.Properties.Resources.star1_5;
                    break;
                default:
                    this.pictureBox2.Image = global::RueHelper.Properties.Resources.star1_3;
                    break;
            }
            //int picBoxHight = 0, picBoxWidth = 0;
            //picBoxHight = pictureBox2.Image.Height;
            //picBoxWidth = pictureBox2.Image.Width;
            //this.pictureBox2.Size = new System.Drawing.Size(picBoxWidth, picBoxHight);
            //this.pictureBox2.Location = new System.Drawing.Point((screenWidth - this.pictureBox2.Width) / 2, label2.Top - pictureBox2.Height-80);
            this.Show();

            System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
            t.Interval = 2000;
            t.Tick += new EventHandler(t_Tick_Close);
            t.Start();
        }
        private void RewardStar_HighSchool()
        {
            this.pictureBox1.Visible = false;
            this.Visible = true;
            this.label1.Visible = false;
            this.label2.Visible = false;
            this.pictureBox2.Visible = true;
            this.pictureBox2.Size = new System.Drawing.Size(414, 524);//高中奖励白板尺寸
            this.pictureBox2.Image = global::RueHelper.Properties.Resources.reward_board;
            this.pictureBox2.Location = new System.Drawing.Point((screenWidth - this.pictureBox2.Width) / 2, (screenHeight - this.pictureBox2.Height) / 2  - 50);


            System.Windows.Forms.PictureBox pictureBox_name = new System.Windows.Forms.PictureBox();
            System.Windows.Forms.PictureBox pictureBox_Star1 = new System.Windows.Forms.PictureBox();
            System.Windows.Forms.PictureBox pictureBox_Star2 = new System.Windows.Forms.PictureBox();
            System.Windows.Forms.PictureBox pictureBox_Star3 = new System.Windows.Forms.PictureBox();
            System.Windows.Forms.PictureBox pictureBox_Star4 = new System.Windows.Forms.PictureBox();
            System.Windows.Forms.PictureBox pictureBox_Star5 = new System.Windows.Forms.PictureBox();
            System.Windows.Forms.PictureBox pictureBox_Stars = new System.Windows.Forms.PictureBox();

            pictureBox_Stars.Parent = this.pictureBox2;//指定父级
            pictureBox_Stars.Image = global::RueHelper.Properties.Resources.reward_stars;
            pictureBox_Stars.Location = new Point(90, 265);//在pictureBox1中的坐标
            pictureBox_Stars.Size = new System.Drawing.Size(229, 44);
            pictureBox_Stars.TabIndex = 1;
            pictureBox_Stars.TabStop = false;

            pictureBox_Star1.Parent = pictureBox_Stars;//指定父级
            pictureBox_Star1.Image = global::RueHelper.Properties.Resources.reward_star;
            pictureBox_Star1.Location = new Point(0, 0);//在pictureBox1中的坐标
            pictureBox_Star1.Size = new System.Drawing.Size(46, 44);
            pictureBox_Star1.TabIndex = 1;
            pictureBox_Star1.TabStop = false;

            pictureBox_Star2.Parent = pictureBox_Stars;//指定父级
            pictureBox_Star2.Image = global::RueHelper.Properties.Resources.reward_star;
            pictureBox_Star2.Location = new Point(45, 0);//在pictureBox1中的坐标
            pictureBox_Star2.Size = new System.Drawing.Size(46, 44);
            pictureBox_Star2.TabIndex = 1;
            pictureBox_Star2.TabStop = false;

            pictureBox_Star3.Parent = pictureBox_Stars;//指定父级
            pictureBox_Star3.Image = global::RueHelper.Properties.Resources.reward_star;
            pictureBox_Star3.Location = new Point(90, 0);//在pictureBox1中的坐标
            pictureBox_Star3.Size = new System.Drawing.Size(46, 44);
            pictureBox_Star3.TabIndex = 1;
            pictureBox_Star3.TabStop = false;

            pictureBox_Star4.Parent = pictureBox_Stars;//指定父级
            pictureBox_Star4.Image = global::RueHelper.Properties.Resources.reward_star;
            pictureBox_Star4.Location = new Point(136, 0);//在pictureBox1中的坐标
            pictureBox_Star4.Size = new System.Drawing.Size(46, 44);
            pictureBox_Star4.TabIndex = 1;
            pictureBox_Star4.TabStop = false;

            pictureBox_Star5.Parent = pictureBox_Stars;//指定父级
            pictureBox_Star5.Image = global::RueHelper.Properties.Resources.reward_star;
            pictureBox_Star5.Location = new Point(182, 0);//在pictureBox1中的坐标
            pictureBox_Star5.Size = new System.Drawing.Size(46, 44);
            pictureBox_Star5.TabIndex = 1;
            pictureBox_Star5.TabStop = false;

            if (point == 1)
            {
                pictureBox_Star2.Visible = false;
                pictureBox_Star3.Visible = false;
                pictureBox_Star4.Visible = false;
                pictureBox_Star5.Visible = false;
            }
            else if (point == 2)
            {
                pictureBox_Star3.Visible = false;
                pictureBox_Star4.Visible = false;
                pictureBox_Star5.Visible = false;
            }
            else if (point == 3)
            {
                pictureBox_Star4.Visible = false;
                pictureBox_Star5.Visible = false;
            }
            else if (point == 4)
            {
                pictureBox_Star5.Visible = false;
            }

            pictureBox_name.Parent = this.pictureBox2;//指定父级
            pictureBox_name.Image = global::RueHelper.Properties.Resources.rewardname;
            pictureBox_name.Location = new System.Drawing.Point(98, 380);
            pictureBox_name.Size = new System.Drawing.Size(215, 73);
            pictureBox_name.TabIndex = 2;
            pictureBox_name.TabStop = false;


            Label lb = new Label();//创建一个label
            lb.Text = STName;
            lb.Parent = pictureBox_name;//指定父级
            lb.Size = pictureBox_name.Size;
            lb.BackColor = Color.Transparent;
            lb.ForeColor = System.Drawing.Color.White;
            lb.Font = new System.Drawing.Font("微软雅黑", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            lb.Location = new Point(0, 0);//在pictureBox1中的坐标
            lb.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
            t.Interval = 2000;
            t.Tick += new EventHandler(t_Tick_Close);
            t.Start();

        }
        private void RewardStar_HighSchool2()
        {
            this.Visible = true;
            this.label1.Visible = true;
            this.label2.Visible = false;

            this.pictureBox2.Visible = true;
            this.pictureBox2.Size = new System.Drawing.Size(394, 167);//高中奖励白板尺寸
            this.pictureBox2.Image = global::RueHelper.Properties.Resources.nameshow;
            this.pictureBox2.Location = new System.Drawing.Point((screenWidth - this.pictureBox2.Width) / 2, (screenHeight - this.pictureBox2.Height) / 2 - 40);

            this.label1.Location = new System.Drawing.Point((screenWidth - label1.Width) / 2, pictureBox2.Top - 80);

            this.pictureBox1.Visible = true;
            this.pictureBox1.Image = global::RueHelper.Properties.Resources.reward_stars;
            this.pictureBox1.Size = new System.Drawing.Size(229, 44);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.Location = new System.Drawing.Point((screenWidth - pictureBox1.Width) / 2, pictureBox2.Bottom + 40);

            System.Windows.Forms.PictureBox pictureBox_Star1 = new System.Windows.Forms.PictureBox();
            System.Windows.Forms.PictureBox pictureBox_Star2 = new System.Windows.Forms.PictureBox();
            System.Windows.Forms.PictureBox pictureBox_Star3 = new System.Windows.Forms.PictureBox();
            System.Windows.Forms.PictureBox pictureBox_Star4 = new System.Windows.Forms.PictureBox();
            System.Windows.Forms.PictureBox pictureBox_Star5 = new System.Windows.Forms.PictureBox();

            int _w = 86;
            int _h = 82;
            //int _w = 46;
            //int _h = 44;
            pictureBox_Star1.Parent = pictureBox1;//指定父级
            pictureBox_Star1.Image = global::RueHelper.Properties.Resources.reward_star;
            pictureBox_Star1.Location = new Point(0, 0);//在pictureBox1中的坐标
            pictureBox_Star1.Size = new System.Drawing.Size(_w, _h);
            pictureBox_Star1.TabIndex = 1;
            pictureBox_Star1.TabStop = false;

            pictureBox_Star2.Parent = pictureBox1;//指定父级
            pictureBox_Star2.Image = global::RueHelper.Properties.Resources.reward_star;
            pictureBox_Star2.Location = new Point(_h+2, 0);//在pictureBox1中的坐标
            pictureBox_Star2.Size = new System.Drawing.Size(_w, _h);
            pictureBox_Star2.TabIndex = 1;
            pictureBox_Star2.TabStop = false;

            pictureBox_Star3.Parent = pictureBox1;//指定父级
            pictureBox_Star3.Image = global::RueHelper.Properties.Resources.reward_star;
            pictureBox_Star3.Location = new Point((_h+2)*2, 0);//在pictureBox1中的坐标
            pictureBox_Star3.Size = new System.Drawing.Size(_w, _h);
            pictureBox_Star3.TabIndex = 1;
            pictureBox_Star3.TabStop = false;

            pictureBox_Star4.Parent = pictureBox1;//指定父级
            pictureBox_Star4.Image = global::RueHelper.Properties.Resources.reward_star;
            pictureBox_Star4.Location = new Point((_h + 3) * 3, 0);//在pictureBox1中的坐标
            pictureBox_Star4.Size = new System.Drawing.Size(_w, _h);
            pictureBox_Star4.TabIndex = 1;
            pictureBox_Star4.TabStop = false;

            pictureBox_Star5.Parent = pictureBox1;//指定父级
            pictureBox_Star5.Image = global::RueHelper.Properties.Resources.reward_star;
            pictureBox_Star5.Location = new Point((_h + 3) * 4, 0);//在pictureBox1中的坐标
            pictureBox_Star5.Size = new System.Drawing.Size(_w, _h);
            pictureBox_Star5.TabIndex = 1;
            pictureBox_Star5.TabStop = false;

            if (point == 1)
            {
                pictureBox_Star2.Visible = false;
                pictureBox_Star3.Visible = false;
                pictureBox_Star4.Visible = false;
                pictureBox_Star5.Visible = false;
            }
            else if (point == 2)
            {
                pictureBox_Star3.Visible = false;
                pictureBox_Star4.Visible = false;
                pictureBox_Star5.Visible = false;
            }
            else if (point == 3)
            {
                pictureBox_Star4.Visible = false;
                pictureBox_Star5.Visible = false;
            }
            else if (point == 4)
            {
                pictureBox_Star5.Visible = false;
            }


            Label lb = new Label();//创建一个label
            lb.Text = STName;
            lb.Parent = pictureBox2;//指定父级
            lb.Size = pictureBox2.Size;
            lb.BackColor = Color.Transparent;
            //lb.ForeColor = System.Drawing.Color.Black;
            lb.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(176)))), ((int)(((byte)(102)))));
            lb.Font = new System.Drawing.Font("微软雅黑", 42F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            lb.Location = new Point(0, 0);//在pictureBox1中的坐标
            lb.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

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

        public void CellNameShow()
        {
            this.label2.Visible = false;
            this.pictureBox2.Visible = false;
        }
    }
}
