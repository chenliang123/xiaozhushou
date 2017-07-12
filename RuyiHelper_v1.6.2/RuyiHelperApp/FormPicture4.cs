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
    public partial class FormPicture4 : Form
    {
        System.Configuration.Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        private static log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string filepath;
        private Bitmap m_bmp1;
        private Bitmap m_bmp2;
        private Bitmap m_bmp3;
        private Bitmap m_bmp4;
        private Point m_pointLast;
        private string m_base64_1;
        private string m_base64_2;
        private string m_base64_3;
        private string m_base64_4;

        int screenWidth = Screen.PrimaryScreen.Bounds.Width;
        int screenHeight = Screen.PrimaryScreen.Bounds.Height;
        int imgWidth = 0;
        int imgHeight = 0;
        int imgWidthTotal = 0;
        int imgHeightTotal = 0;
        int rotate = 0;
        int angle = 0;
        public int index = 0;
        public bool m_bDraw = false;
        /// <summary>
        /// 打开图片
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="mode">1: 数据流 2：本地文件 3:拍照文件 4: 截屏文件</param>
        public FormPicture4()
        {
            InitializeComponent();
            //this.pictureBox1.Size = new System.Drawing.Size(screenWidth, screenHeight);
            this.WindowState = FormWindowState.Maximized;
            this.index = index;
            this.TopMost = true;
#if DEBUG
            this.TopMost = false;//Form3 ShowView
#endif
            this.pictureBox1.Visible = false;
            this.pictureBox1.Visible = true;

            this.Height = screenHeight;
            this.Width = screenWidth;

            int _width = (screenWidth-10)/2;
            int _height = (screenHeight-5)/2;
            this.pictureBox1.Width = _width;
            this.pictureBox1.Height = _height;
            this.pictureBox2.Width = _width;
            this.pictureBox2.Height = _height;
            this.pictureBox3.Width = _width;
            this.pictureBox3.Height = _height;
            this.pictureBox4.Width = _width;
            this.pictureBox4.Height = _height;
            this.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            this.pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            this.pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
            this.pictureBox4.SizeMode = PictureBoxSizeMode.StretchImage;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox2.Location = new System.Drawing.Point(10 + _width, 0);
            this.pictureBox3.Location = new System.Drawing.Point(0, 5 + _height);
            this.pictureBox4.Location = new System.Drawing.Point(10 + _width, 5 + _height);
        }

        public void showPicture(int index)
        {
            angle = 0;

            this.index = index;

            this.pictureBox1.Hide();
            this.pictureBox2.Hide();
            this.pictureBox3.Hide();
            this.pictureBox4.Hide();
            try
            {
                if (index == 1)
                {
                    this.pictureBox1.Location = new System.Drawing.Point(0, 0);
                    this.pictureBox1.Width = screenWidth;
                    this.pictureBox1.Height = screenHeight;
                    this.pictureBox1.Show();
                }
                else if (index == 2)
                {
                    this.pictureBox2.Location = new System.Drawing.Point(0, 0);
                    this.pictureBox2.Width = screenWidth;
                    this.pictureBox2.Height = screenHeight;
                    this.pictureBox2.Show();
                }
                else if (index == 3)
                {
                    this.pictureBox3.Location = new System.Drawing.Point(0, 0);
                    this.pictureBox3.Width = screenWidth;
                    this.pictureBox3.Height = screenHeight;
                    this.pictureBox3.Show();
                }
                else if (index == 4)
                {
                    this.pictureBox4.Location = new System.Drawing.Point(0, 0);
                    this.pictureBox4.Width = screenWidth;
                    this.pictureBox4.Height = screenHeight;
                    this.pictureBox4.Show();
                }
                else
                {
                    int _width = (screenWidth - 10) / 2;
                    int _height = (screenHeight - 5) / 2;
                    this.pictureBox1.Width = _width;
                    this.pictureBox1.Height = _height;
                    this.pictureBox2.Width = _width;
                    this.pictureBox2.Height = _height;
                    this.pictureBox3.Width = _width;
                    this.pictureBox3.Height = _height;
                    this.pictureBox4.Width = _width;
                    this.pictureBox4.Height = _height;
                    this.pictureBox1.Location = new System.Drawing.Point(0, 0);
                    this.pictureBox2.Location = new System.Drawing.Point(10 + _width, 0);
                    this.pictureBox3.Location = new System.Drawing.Point(0, 5 + _height);
                    this.pictureBox4.Location = new System.Drawing.Point(10 + _width, 5 + _height);
                    this.pictureBox1.Show();
                    this.pictureBox2.Show();
                    this.pictureBox3.Show();
                    this.pictureBox4.Show();

                    m_bmp1 = Base64StringToImage(m_base64_1);
                    this.pictureBox1.Image = m_bmp1;
                    m_bmp2 = Base64StringToImage(m_base64_2);
                    this.pictureBox2.Image = m_bmp2;
                    m_bmp3 = Base64StringToImage(m_base64_3);
                    this.pictureBox3.Image = m_bmp3;
                    m_bmp4 = Base64StringToImage(m_base64_4);
                    this.pictureBox4.Image = m_bmp4;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
        }

        public void Rotate(int right)
        {
            if (index == 1)
            {
                Rotate(this.pictureBox1, m_bmp1, right);
            }
            else if (index == 2)
            {
                Rotate(this.pictureBox2, m_bmp2, right);
            }
            else if (index == 3)
            {
                Rotate(this.pictureBox3, m_bmp3, right);
            }
            else if (index == 4)
            {
                Rotate(this.pictureBox4, m_bmp4, right);
            }
        }
        public void Rotate(PictureBox pb, Bitmap bmp, int right)
        {
            if (right == 1)
            {
                angle += 90;
            }
            else
            {
                angle -= 90;
            }
            bool bVerticle = false;
            if (angle!=0 && angle % 90 == 0 && angle % 180 != 0)
                bVerticle = true;

            //this.pictureBox1.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
            //this.pictureBox1.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
            //img.RotateFlip(RotateFlipType.Rotate90FlipNone);
            //顺时针旋转90度 RotateFlipType.Rotate90FlipNone 
            //逆时针旋转90度 RotateFlipType.Rotate270FlipNone 
            //水平翻转 RotateFlipType.Rotate180FlipY 
            //垂直翻转 RotateFlipType.Rotate180FlipX

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
            if(!bVerticle)
            {
                pb.Width = screenWidth;
                pb.Height = screenHeight;
            }else{
                double w1 = screenWidth;
                double h1 = screenHeight;
                double ratio = w1 / h1;
                double w2 = h1;
                double h2 = w2 / ratio;
                pb.Width = (int)h2;
                pb.Height = (int)w2;
            }
            pb.Location = new Point((screenWidth - pb.Width) / 2, (screenHeight - pb.Height) / 2);

            if (right == 1)
            {
                bmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
                pb.Image = bmp;
            }
            else
            {
                bmp.RotateFlip(RotateFlipType.Rotate270FlipNone);
                pb.Image = bmp;
            }
        }

        public void addPicture(int index, string datas)
        {
            if(datas.Length == 0)
            {
                return;
            }

            try
            {
                Bitmap bmp = Base64StringToImage(datas);
                if (index == 1)
                {
                    m_bmp1 = bmp;
                    this.pictureBox1.Image = bmp;
                    m_base64_1 = datas;
                }
                else if (index == 2)
                {
                    m_bmp2 = bmp;
                    this.pictureBox2.Image = bmp;
                    m_base64_2 = datas;
                }
                else if (index == 3)
                {
                    m_bmp3 = bmp;
                    this.pictureBox3.Image = bmp;
                    m_base64_3 = datas;
                }
                else if (index == 4)
                {
                    m_bmp4 = bmp;
                    this.pictureBox4.Image = bmp;
                    m_base64_4 = datas;
                }

                imgHeight = bmp.Height;
                imgWidth = bmp.Width;

                Log.Info("imgHeight=" + imgHeight + " imgWidth=" + imgWidth);
                Log.Info("screenHeight=" + screenHeight + " screenWidth=" + screenWidth);

                //m_bmp = new Bitmap(m_bmp, screenWidth, screenHeight);//不能图片缩放,因为破坏了图片的质量

                ////doUpload
                //if (index == 1 && firsttime == 1)
                //{
                //    string imgName = DateTime.Now.ToString("yyyyMMdd") + "-" + Global.getSchoolID() + "-" + Global.getClassID() + "-" + DateTime.Now.ToString("HHmmss") + ".jpg";
                //    string imgDir = Application.StartupPath + "\\" + DateTime.Now.ToString("yyyyMMdd");
                //    if (!Directory.Exists(imgDir))
                //        Directory.CreateDirectory(imgDir);

                //    string imgPath = imgDir + "\\" + imgName;
                //    bmp.Save(imgPath);
                //    Common.uploadPicture(imgPath);
                //    Common.uploadCameraEvent(imgName);//相机拍照
                //}

            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
        }
        public void Zoom(double ratio, double ratioX, double ratioY)
        {
            bool bVerticle = false;
            if (angle != 0 && angle % 90 == 0 && angle % 180 != 0)
                bVerticle = true;

            int newWidth = (int)(ratio * screenWidth);
            int newHight = (int)(ratio * screenHeight);
            int biasX = (int)(ratioX * screenWidth);
            int biasY = (int)(ratioY * screenHeight);

            if (ratio == 1)
            {
                biasX = 0;
                biasY = 0;
            }

            if (bVerticle)
            {
                double w1 = screenWidth;
                double h1 = screenHeight;
                double ratio0 = w1 / h1;
                double h2 = h1;
                //double w2 = h2 / ratio0;
                double w2 = h1 * h1 / w1;
                newWidth = (int)(ratio * w2);
                newHight = (int)(ratio * h2);

                //旋转后的强制居中
                biasX = (screenWidth - newWidth) / 2;
            }

            if (index == 1)
            {
                pictureBox1.Width = newWidth;
                pictureBox1.Height = newHight;
                pictureBox1.Location = new Point(biasX, biasY);
            }
            else if (index == 2)
            {
                pictureBox2.Width = newWidth;
                pictureBox2.Height = newHight;
                pictureBox2.Location = new Point(biasX, biasY);
            }
            else if (index == 3)
            {
                pictureBox3.Width = newWidth;
                pictureBox3.Height = newHight;
                pictureBox3.Location = new Point(biasX, biasY);
            }
            else if (index == 4)
            {
                //if (pictureBox1.Width < pictureBox1.Height)
                //{
                //    newWidth = (int)(ratio * screenHeight);
                //    newHight = (int)(ratio * screenWidth);
                //}
                pictureBox4.Width = newWidth;
                pictureBox4.Height = newHight;
                pictureBox4.Location = new Point(biasX, biasY);
            }


        }

        public void ShowAndHide()
        {
            this.Show();

            System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
            t.Interval = 3000;//2秒
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

        public void Rotate2(int right)
        {
            if(index == 1)
            {
                if (right == 1)
                {
                    m_bmp1.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    this.pictureBox1.Image = m_bmp1;
                }
                else
                {
                    m_bmp1.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    this.pictureBox1.Image = m_bmp1;
                }
            }
            else if (index == 2)
            {
                if (right == 1)
                {
                    m_bmp2.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    this.pictureBox2.Image = m_bmp2;
                }
                else
                {
                    m_bmp2.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    this.pictureBox2.Image = m_bmp2;
                }
            }
            else if (index == 3)
            {
                if (right == 1)
                {
                    m_bmp3.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    this.pictureBox3.Image = m_bmp3;
                }
                else
                {
                    m_bmp3.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    this.pictureBox3.Image = m_bmp3;
                }
            }
            else if (index == 4)
            {
                if (right == 1)
                {
                    m_bmp4.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    this.pictureBox4.Image = m_bmp4;
                }
                else
                {
                    m_bmp4.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    this.pictureBox4.Image = m_bmp4;
                }
            }

        }
        public void DrawLine(string percentX, string percentY, int mode, string color, int width)
        {
            //if (imgWidth < 300)
            //    width = 1;
            //else if (imgWidth < 600)
            //    width = 4;
            //else if (imgWidth < 800)
            //    width = 6;
            //else if (imgWidth < 1000)
            //    width = 8;
            //else if (imgWidth < 1500)
            //    width = 10;
            //else
            //    width = 12;

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
            Bitmap bmp = m_bmp1;
            if (index == 1)
            {
                bmp = m_bmp1;
            }
            else if (index == 2)
            {
                bmp = m_bmp2;
            }
            else if (index == 3)
            {
                bmp = m_bmp3;
            }
            else if (index == 4)
            {
                bmp = m_bmp4;
            }
            x = bmp.Width * perX;
            y = bmp.Height * perY;
            
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
                    if (IsPixelFormatIndexed(bmp.PixelFormat))
                    {
                        Bitmap _bmp = new Bitmap(bmp.Width, bmp.Height, PixelFormat.Format32bppArgb);
                        using (Graphics g = Graphics.FromImage(_bmp))
                        {
                            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                            g.DrawImage(bmp, 0, 0);

                            //draw.....
                            Color c = ColorTranslator.FromHtml(color);
                            Pen pen = new Pen(c, width);
                            g.DrawLine(pen, m_pointLast, pt2);

                            m_pointLast.X = (int)x;
                            m_pointLast.Y = (int)y;
                        }
                    }
                    else
                    {
                        Graphics g = Graphics.FromImage(bmp);
                        Color c = ColorTranslator.FromHtml(color);
                        Pen pen = new Pen(c, width);

                        g.DrawLine(pen, m_pointLast, pt2);
                        m_pointLast.X = (int)x;
                        m_pointLast.Y = (int)y;
                    }

                    if (index == 1)
                    {
                        pictureBox1.Image = (Image)bmp;
                    }
                    else if (index == 2)
                    {
                        pictureBox2.Image = (Image)bmp;
                    }
                    else if (index == 3)
                    {
                        pictureBox3.Image = (Image)bmp;
                    }
                    else if (index == 4)
                    {
                        pictureBox4.Image = (Image)bmp;
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
            if (index == 1)
            {
                m_bmp1 = Base64StringToImage(m_base64_1);
                this.pictureBox1.Image = m_bmp1;
            }
            else if (index == 2)
            {
                m_bmp2 = Base64StringToImage(m_base64_2);
                this.pictureBox2.Image = m_bmp2;
            }
            else if (index == 3)
            {
                m_bmp3 = Base64StringToImage(m_base64_3);
                this.pictureBox3.Image = m_bmp3;
            }
            else if (index == 4)
            {
                m_bmp4 = Base64StringToImage(m_base64_4);
                this.pictureBox4.Image = m_bmp4;
            }
            Log.Info("ClearView over.");
        }

        public void EmptyView()
        {
            index = 0;
            this.pictureBox1.Image = null;
            this.pictureBox2.Image = null;
            this.pictureBox3.Image = null;
            this.pictureBox4.Image = null;
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
        //public string GetImageBase64()
        //{
        //    MemoryStream ms = new MemoryStream();
        //    m_bmp1.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
        //    byte[] arr = new byte[ms.Length];
        //    ms.Position = 0;
        //    ms.Read(arr, 0, (int)ms.Length);
        //    ms.Close();
        //    string strBase64 = Convert.ToBase64String(arr);
        //    return strBase64;
        //}
        
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
