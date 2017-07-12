﻿using Newtonsoft.Json;

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
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Net;

namespace RueHelper
{
    public partial class FormQiangDa : Form
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

        private static log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        AutoResetEvent are = new AutoResetEvent(false);
        int screenWidth = Screen.PrimaryScreen.Bounds.Width;
        int screenHeight = Screen.PrimaryScreen.Bounds.Height;
        ArrayList al;
        ArrayList alText;
        private ArrayList clickstate;
        public ArrayList _rightList;
        public Form11 f11;
        System.Media.SoundPlayer sp = new SoundPlayer(RueHelper.Properties.Resources.click1);   
        public string _xitiId = "";
        
        public System.Timers.Timer t;
        public System.Timers.Timer t1;
        int inTimer = 0;
        public DateTime tm_create = DateTime.Now;

        private Hashtable m_hashtable = new Hashtable(); //保存学生按键数据
        private Color CircleBackgroundColor = System.Drawing.Color.FromArgb(254, 232, 211);

        private int nCol = 0;
        public delegate void InvokeHandonState(int id, string name, string imgurl, PictureBox pic);
        private List<VoteItem> votelist = new List<VoteItem>();
        public static string RESULT = "";
        public FormQiangDa()
        {
            RESULT = "";
            InitializeComponent();

            tm_create = DateTime.Now;
            al = new ArrayList();
            alText = new ArrayList();
            clickstate = new ArrayList();

            //No xiti.id
            _xitiId = Global.getSchoolID() + "-" + Global.getClassID() + "-" + DateTime.Now.ToString("yyyyMMddHHmmss");

            SetPanel();

            this.Text = "抢答[" + _xitiId + "]";
            
            this.Height = screenHeight;
            this.Width = screenWidth;

            this.TopMost = true;
//#if DEBUG
//            this.TopMost = false;//PPTPractise
//#endif
            this.BringToFront();
            this.Show(); 
            this.Hide();


            t = new System.Timers.Timer(200);
            t.Elapsed += new System.Timers.ElapsedEventHandler(Theout);
            t.Enabled = true;
            t.AutoReset = true;

            //System.Windows.Forms.Timer t1 = new System.Windows.Forms.Timer();
            //t1.Interval = 2000;
            //t1.Tick += new EventHandler(t_Tick_Close);
            //t1.Start();
        }
        void t_Tick_Close(object sender, EventArgs e)
        {
            this.Hide();
            this.Close();
        }
        //public void setResult(string answer)
        //{
        //}
        private void SetPanel()
        {
            #region 标题栏
            label_title.Top = 50;
            label_title.Left = (screenWidth - label_title.Width) / 2;
            //Label lb = new Label();//创建一个label
            //lb.Text = "投 票";
            //lb.Parent = pictureBox_Title;//指定父级
            //lb.Size = pictureBox_Title.Size;
            //lb.BackColor = Color.Transparent;
            //lb.ForeColor = System.Drawing.Color.White;
            //lb.Font = new System.Drawing.Font("微软雅黑", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            //lb.Location = new Point(0, 0);//在pictureBox1中的坐标
            //lb.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            #endregion

            //"✔", "✘"
            #region 相框
            double picCountRatio = 0.30;
            pictureBox_headImage.Visible = false;
            int txtLeft = 0;
            int txtTop = -5;

            
            pictureBox_headImage.Left = (int)(screenWidth - pictureBox_headImage.Width) / 2;
            pictureBox_headImage.Top = (int)(screenHeight * picCountRatio);
            pictureBox_headImage.Visible = true;
            #endregion

            #region 姓名
            label_name.Top = (int)(screenHeight * 0.65);
            label_name.Left = (screenWidth - label_name.Width) / 2;
            label_name.Visible = false;
            #endregion
            
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
                string data = Common.GetHandon();
                if (data.Length>0)
                {

                    Log.Info("xiti.get=" + data);
                    DateTime tm_now = DateTime.Now;
                    TimeSpan createtimespan = new TimeSpan(tm_create.Ticks);
                    TimeSpan nowtimespan = new TimeSpan(tm_now.Ticks);
                    TimeSpan timespan = nowtimespan.Subtract(createtimespan).Duration();
                    int timeDiff = timespan.Minutes * 60 + timespan.Seconds;

                    for (int i = 0; i < data.Split('|').Length; i++)
                    {
                        int num = Util.toInt(data.Split('|')[i]);
                        string context = num + ":" + timeDiff;
                        Log.Info("QiangDa.item=" + context);
                        string name = "";
                        string imgurl = "";
                        StudentInfo si = Global.getUserInfoBySeat(num);
                        if (si != null && i == 0)     ///添加i==0条件，解决抢答前后端显示不一样问题；
                        {
                            name = si.Name;
                            imgurl = si.imageurl;
                        }
                        else
                        {
                            continue;
                        }
                        Log.Info("HandonEvent. id=" + num+", imgurl=" +imgurl);
                        
                        HandonEvent(num,name,imgurl,pictureBox_headImage);
                        StopT();
                    }
                    //Httpd.NotifyVoteEvent();
                }
                Interlocked.Exchange(ref inTimer, 0); 
            }
        }
        
        public void HandonEvent(int id, string name, string imgurl, PictureBox pic)
        {
            if (pic.InvokeRequired)
            {
                InvokeHandonState callback = new InvokeHandonState(HandonEvent);
                pic.Invoke(callback, new object[] {id, name, imgurl, pic});
            } 
            else
            {

                label_title.Text = "抢 答 成 功";

                //imgurl = "http://api.skyeducation.cn/EduApi/upload/profile/ym.png";
                if (imgurl != null && imgurl.Length > 0)
                {
                    GraphicsPath gp = new GraphicsPath();
                    gp.AddEllipse(pictureBox_headImage.ClientRectangle);
                    Region region = new Region(gp);
                    pictureBox_headImage.Region = region;
                    gp.Dispose();
                    region.Dispose();

                    Image picNet = Image.FromStream(WebRequest.Create(imgurl).GetResponse().GetResponseStream(), true, true);
                    pictureBox_headImage.Image = picNet;
                }
                else
                {
                    pictureBox_headImage.Image = global::RueHelper.Properties.Resources.qd_headimg;
                }
                label_name.Visible = true;
                label_name.Text = name;
                this.Show();
                this.BringToFront();
                Log.Info("QiangDa over. name=" + name);



                System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
                t.Interval = 2000;//2秒
                t.Tick += new EventHandler(t_Tick_Close);
                t.Start();
            }
        }

        public static void AddWaterMark(Image copyImage, PictureBox picBox)
        {
            Graphics g = picBox.CreateGraphics();

            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            Rectangle rect = new Rectangle(picBox.Width - copyImage.Width, picBox.Height - copyImage.Height, copyImage.Width, copyImage.Height);
            g.DrawImage(copyImage, rect, 0, 0, copyImage.Width, copyImage.Height, GraphicsUnit.Pixel);
        }

        private void ChangePictureAlpha(Image copyImage, PictureBox picbox, float colorAlpha)
        {
            // Create the Bitmap object and load it with the texture image.
            //Bitmap bitmap = new Bitmap("..//..//test.jpg");C:/Documents and Settings/Administrator/桌面/images
            // Initialize the color matrix.
            // Note the value 0.8 in row 4, column 4.
            float[][] matrixItems ={ 
            new float[] {1, 0, 0, 0, 0},
            new float[] {0, 1, 0,0 , 0},
            new float[] {0, 0, 1, 0, 0},
            new float[] {0, 0, 0, colorAlpha, 0}, 
            new float[] {0, 0, 0, 0, 1}};
            ColorMatrix colorMatrix = new ColorMatrix(matrixItems);
            // Create an ImageAttributes object and set its color matrix.  设置色调整矩阵
            ImageAttributes imageAtt = new ImageAttributes();
            imageAtt.SetColorMatrix(
                colorMatrix,
                ColorMatrixFlag.Default,
                ColorAdjustType.Bitmap);
            // Now draw the semitransparent bitmap image.
            int iWidth = copyImage.Width;
            int iHeight = copyImage.Height;

            Graphics g = picbox.CreateGraphics();
            g.DrawImage(
                copyImage,
                new Rectangle(0, 0, iWidth, iHeight),  // destination rectangle  图片位置
                0.0f,                          // source rectangle x 
                0.0f,                          // source rectangle y
                iWidth,                        // source rectangle width
                iHeight,                       // source rectangle height
                GraphicsUnit.Pixel,
                imageAtt);
        }
         

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            Win32.AnimateWindow(this.Handle, 200, Win32.AW_SLIDE | Win32.AW_HIDE | Win32.AW_BLEND);
        }
        string _req_answer;

        public void StopT()
        {
            //t1.Enabled = false;
            t.Enabled = false;
        }

        public string getResult()
        {
            string result = "";
            return result;
        }
        private void FormVote_MouseDown(object sender, MouseEventArgs e)
        {
        }

        private void FormQiangDa_Load(object sender, EventArgs e)
        {

        }

        private void DrawRoundRect(Graphics graphics, Label label)
        {
            float X = float.Parse(label.Width.ToString()) - 1;
            float Y = float.Parse(label.Height.ToString()) - 1;
            PointF[] points = {
                new PointF(2,     0),
                new PointF(X-2,   0),
                new PointF(X-1,   1),
                new PointF(X,     2),
                new PointF(X,     Y-2),
                new PointF(X-1,   Y-1),
                new PointF(X-2,   Y),
                new PointF(2,     Y),
                new PointF(1,     Y-1),
                new PointF(0,     Y-2),
                new PointF(0,     2),
                new PointF(1,     1)
            };
            GraphicsPath path = new GraphicsPath();
            path.AddLines(points);
            Pen pen = new Pen(Color.FromArgb(150, Color.Blue), 1);
            pen.DashStyle = DashStyle.Solid;
            graphics.DrawPath(pen, path);
        }

        private void label_name_Paint(object sender, PaintEventArgs e)
        {
            //DrawRoundRect(e.Graphics, label_name);
            DrawRoundRect(label_name);
        }
        private void DrawRoundRect(Label label)
        {
            float X = (float)(label.Width);
            float Y = (float)(label.Height);
            PointF[] points =   
           {  
               new PointF(2,0),  
               new PointF(X-2,0),  
               new PointF(X,2),  
               new PointF(X,Y-2),  
               new PointF(X-2,Y),  
               new PointF(2,Y),  
               new PointF(0,Y-2),  
               new PointF(0,2),  
           };
            GraphicsPath path = new GraphicsPath();
            path.AddLines(points);
            label.Region = new Region(path);
        }  
    }
}
