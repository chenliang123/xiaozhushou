using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Windows.Forms;
using RueHelper.util;
using System.Drawing.Imaging;

namespace RueHelper
{
    public partial class FormCaptureScreen : Form
    {
        System.Configuration.Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        private static log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string filepath;
        private Bitmap m_bmp;
        private Point m_pointLast;
        private string m_base64;
        int screenWidth = Screen.PrimaryScreen.Bounds.Width;
        int screenHeight = Screen.PrimaryScreen.Bounds.Height;
        int imgWidth = 0;
        int imgHeight = 0;
        int rotate = 0;
        public bool m_bDraw = false;
        /// <summary>
        /// 打开图片
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="mode">1: 数据流 2：本地文件 3:拍照文件</param>
        public FormCaptureScreen()
        {
            InitializeComponent();
            //this.pictureBox1.Size = new System.Drawing.Size(screenWidth, screenHeight);
            this.WindowState = FormWindowState.Maximized;

            this.TopMost = true;
#if DEBUG
            this.TopMost = false;//Form3 ShowView
#endif
            this.pictureBox1.Visible = false;
            this.pictureBox1.Visible = true;
            try
            {
                m_bmp = ScreenCapture.captureScreen(0, 0);
                m_base64 = Util.ImgToBase64String(m_bmp);

                imgHeight = m_bmp.Height;
                imgWidth = m_bmp.Width;

                Log.Info("imgHeight=" + imgHeight + " imgWidth=" + imgWidth);
                Log.Info("screenHeight=" + screenHeight + " screenWidth=" + screenWidth);

                //m_bmp = new Bitmap(m_bmp, screenWidth, screenHeight);//不能图片缩放,因为破坏了图片的质量
                this.pictureBox1.Image = m_bmp;

                this.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                this.pictureBox1.Width = screenWidth;
                this.pictureBox1.Height = screenHeight;

                //doUpload
                if(true)
                {
                    string imgName = DateTime.Now.ToString("yyyyMMdd") + "-" + Global.getSchoolID() + "-" + Global.getClassID() + "-" + DateTime.Now.ToString("HHmmss") + ".jpg";
                    string imgDir = Application.StartupPath + "\\" + DateTime.Now.ToString("yyyyMMdd");
                    if (!Directory.Exists(imgDir))
                        Directory.CreateDirectory(imgDir);

                    string imgPath = imgDir + "\\" + imgName;
                    m_bmp.Save(imgPath);
                    Common.uploadPicture(imgPath);
                    //Common.uploadCameraEvent(imgName);//相机拍照
                }
                
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }



            ////(图片宽>屏幕宽 && 图片高>屏幕高)
            //if ((pictureBox1.Width > Screen.PrimaryScreen.WorkingArea.Width && pictureBox1.Height > Screen.PrimaryScreen.WorkingArea.Height) || 
            //    (pictureBox1.Width > Screen.PrimaryScreen.WorkingArea.Width && pictureBox1.Height < Screen.PrimaryScreen.WorkingArea.Height))
            //{
            //    MakeThumbnail(pictureBox1.Width, pictureBox1.Height, "W");
            //}
            ////(图片宽<屏幕宽 && 图片高>屏幕高) 高度压缩
            //else if (pictureBox1.Width < Screen.PrimaryScreen.WorkingArea.Width && pictureBox1.Height > Screen.PrimaryScreen.WorkingArea.Height)
            //{
            //    MakeThumbnail(pictureBox1.Width, pictureBox1.Height, "H");
            //}
            //else
            //{
            //    pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            //    this.pictureBox1.Left = Screen.PrimaryScreen.WorkingArea.Width / 2 - pictureBox1.Width / 2;
            //    this.pictureBox1.Top = Screen.PrimaryScreen.WorkingArea.Height / 2 - pictureBox1.Height / 2;
            //}
        }
        public void Zoom(double ratio, double ratioX, double ratioY)
        {
            //m_bmp = Base64StringToImage(m_base64);

            int newWidth = (int)(ratio * screenWidth);
            int newHight = (int)(ratio * screenHeight);

            if (pictureBox1.Width < pictureBox1.Height)
            {
                newWidth = (int)(ratio * screenHeight);
                newHight = (int)(ratio * screenWidth);
            }
            //newWidth = (int)(ratio * pictureBox1.Width);
            //newHight = (int)(ratio * pictureBox1.Height);
            //Bitmap bmp = new Bitmap(m_bmp, newWidth, newHight);
            //pictureBox1.Image = bmp;

            int biasWidth = (int)(ratioX * screenWidth);
            int biasHeight = (int)(ratioY * screenHeight);

            //biasWidth = (int)(ratioX * pictureBox1.Width);
            //biasHeight = (int)(ratioY * pictureBox1.Height);
            //biasWidth = (int)(ratioX * newWidth);
            //biasHeight = (int)(ratioY * newHight);

            //pictureBox1.Width = pbMain.Width + int.Parse(Math.Ceiling(pbMain.Width * step).ToString());
            //pictureBox1.Height = pbMain.Height + int.Parse(Math.Ceiling(pbMain.Height * step).ToString());
            //pictureBox1.Location = new Point((panelContainer.Width - pbMain.Width) / 2, (panelContainer.Height - pbMain.Height) / 2);  

            pictureBox1.Width = newWidth;
            pictureBox1.Height = newHight;
            pictureBox1.Location = new Point(biasWidth, biasHeight);

        }
        public void ShowAndHide()
        {
            this.Show();

            System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
            t.Interval = 2000;//2秒
            t.Tick += new EventHandler(t_Tick_Close);
            t.Start();
        }
        void t_Tick_Close(object sender, EventArgs e)
        {
            //System.Threading.Thread.CurrentThread.Abort();
            //System.Environment.Exit(System.Environment.ExitCode);//主程序都退出了。。。
            this.Dispose();
            this.Close();
        }

        public void Rotate(int right)
        {
            //this.pictureBox1.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
            //this.pictureBox1.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
            //img.RotateFlip(RotateFlipType.Rotate90FlipNone);
            //顺时针旋转90度 RotateFlipType.Rotate90FlipNone 
            //逆时针旋转90度 RotateFlipType.Rotate270FlipNone 
            //水平翻转 RotateFlipType.Rotate180FlipY 
            //垂直翻转 RotateFlipType.Rotate180FlipX          
//1,3
//?,1              
            //if (rotate == 0)
            //{
            //    rotate = 1;
            //    double ratio = (double)screenHeight / screenWidth;
            //    int newWidth = screenHeight;
            //    int newHight = (int)(ratio * newWidth);
            //    m_bmp = new Bitmap(m_bmp, newWidth, newHight);
            //}
            //else
            //{
            //    rotate = 0;
            //    m_bmp = new Bitmap(m_bmp, screenHeight, screenWidth);
            //}

            if (right == 1)
            {
                m_bmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
                this.pictureBox1.Image = m_bmp;
            }
            else
            {
                m_bmp.RotateFlip(RotateFlipType.Rotate270FlipNone);
                this.pictureBox1.Image = m_bmp;
            }
        }
        public void DrawLine(string percentX, string percentY, int mode, string color, int width)
        {
            //if (width <= 6)
            //    width = 2;
            //else if (width < 12)
            //    width = 6;
            //else
            //    width = 10;

            m_bDraw = true;
            double perX = Convert.ToDouble(percentX);
            double perY = Convert.ToDouble(percentY);
            
            double x=0;
            double y=0;

            //double x = (screenWidth-imgWidth)/2+this.imgWidth * perX;
            //double y = (screenHeight-imgHeight)/2+this.imgHeight * perY;
            //x = this.screenWidth * perX;
            //y = this.screenHeight * perY;
            x = m_bmp.Width * perX;
            y = m_bmp.Height * perY;
            Log.Info("x :" + x + " y=" + y);

            if(mode==0)
            {
                m_pointLast.X = (int)x;
                m_pointLast.Y = (int)y;
                Log.Info("Color: " + color + " width=" + width);
            }
            else
            {
                Point pt2 = new Point();
                pt2.X = (int)x;
                pt2.Y = (int)y;

                try
                {
                    if (IsPixelFormatIndexed(m_bmp.PixelFormat))
                    {
                        Bitmap bmp = new Bitmap(m_bmp.Width, m_bmp.Height, PixelFormat.Format32bppArgb);
                        using (Graphics g = Graphics.FromImage(bmp))
                        {
                            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                            g.DrawImage(m_bmp, 0, 0);

                            //draw.....
                            Color c = ColorTranslator.FromHtml(color);
                            Pen pen = new Pen(c, width);
                            g.DrawLine(pen, m_pointLast, pt2);

                            pictureBox1.Image = (Image)m_bmp;
                            m_pointLast.X = (int)x;
                            m_pointLast.Y = (int)y;
                        }
                    }
                    else
                    {
                        Graphics g = Graphics.FromImage(m_bmp);
                        Color c = ColorTranslator.FromHtml(color);
                        Pen pen = new Pen(c, width);

                        g.DrawLine(pen, m_pointLast, pt2);
                        pictureBox1.Image = (Image)m_bmp;

                        m_pointLast.X = (int)x;
                        m_pointLast.Y = (int)y;
                    }
                }
                catch (Exception ee)
                {
                    Log.Info(ee.Message);
                }
            }
            
        }
        private static PixelFormat[] indexedPixelFormats = {
            PixelFormat.Undefined,
            PixelFormat.DontCare,
            PixelFormat.Format16bppArgb1555,
            PixelFormat.Format1bppIndexed,
            PixelFormat.Format4bppIndexed,
            PixelFormat.Format8bppIndexed
        };

        private bool IsPixelFormatIndexed(PixelFormat imgPixelFormat)
        {
            foreach (PixelFormat pf in indexedPixelFormats)
            {
                if (pf.Equals(imgPixelFormat)) return true;
            }

            return false;
        }

        public void ClearView()
        {
            m_bmp = Base64StringToImage(m_base64);
            this.pictureBox1.Image = m_bmp;
            Log.Info("ClearView over.");
        }
        
        
        public void CloseView()
        {
            Log.Info("CloseView over.");
            if (m_bDraw)
            {
                //上传画过的图片
                Image img = ScreenCapture.captureScreen(0, 0);
                string imgName = DateTime.Now.ToString("yyyyMMdd") + "-" + Global.getSchoolID() + "-" + Global.getClassID() + "-" + DateTime.Now.ToString("HHmmss")+".jpg";
                string imgDir = Application.StartupPath + "\\" + DateTime.Now.ToString("yyyyMMdd");
                if (!Directory.Exists(imgDir))
                    Directory.CreateDirectory(imgDir);

                string imgPath = imgDir + "\\" + imgName;
                img.Save(imgPath);
                Common.uploadPicture(imgPath);//drawView
                Common.uploadDrawView(imgName);
            }
            m_bDraw = false;
            this.Close();
        }
        public Bitmap Base64StringToImage(string datas)
        {
            byte[] arr = Convert.FromBase64String(datas);
            MemoryStream ms = new MemoryStream(arr);
            Bitmap _bmp = new Bitmap(ms);
            return _bmp;
        }
        public string GetImageBase64()
        {
            MemoryStream ms = new MemoryStream();
            m_bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            byte[] arr = new byte[ms.Length];
            ms.Position = 0;
            ms.Read(arr, 0, (int)ms.Length);
            ms.Close();
            string strBase64 = Convert.ToBase64String(arr);
            return strBase64;
        }
        
        public void MakeThumbnail(int width,int height,string mode)
        {
            try
            {
                int HIGHT = screenHeight;
                int WIDTH = screenWidth;
                //int WIDTH = Screen.PrimaryScreen.WorkingArea.Width - 60;
                //int HIGHT = Screen.PrimaryScreen.WorkingArea.Height - 60;
                int g_width = WIDTH;
                int g_height = HIGHT;
                if (mode == "W")
                {
                    int new_height = height * g_width / width;
                    if (new_height > g_height)
                    {
                        g_width = g_width * g_height / new_height;
                        new_height = g_height;
                    }
                    this.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    this.pictureBox1.Width = g_width;
                    this.pictureBox1.Height = new_height;
                }
                else
                {
                    int new_width = width * g_height / height;
                    this.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    this.pictureBox1.Width = new_width;
                    this.pictureBox1.Height = g_height;
                }
                this.pictureBox1.Left = WIDTH / 2 - pictureBox1.Width / 2;
                this.pictureBox1.Top = HIGHT / 2 - pictureBox1.Height / 2;
            }
            catch (Exception ex) { }
        }
    }
}
