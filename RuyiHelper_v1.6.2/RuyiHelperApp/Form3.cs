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
using System.Runtime.InteropServices;

namespace RueHelper
{
    public partial class Form3 : Form
    {
        System.Configuration.Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        private static log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        [DllImport("user32.dll")]
        static extern void mouse_event(int flags, int dx, int dy, uint data, UIntPtr extraInfo);
        private int MOUSEEVENTF_MOVE = 0x0001;      //移动鼠标 
        private int MOUSEEVENTF_LEFTDOWN = 0x0002; //模拟鼠标左键按下 
        private int MOUSEEVENTF_LEFTUP = 0x0004; //模拟鼠标左键抬起 
        private int MOUSEEVENTF_RIGHTDOWN = 0x0008; //模拟鼠标右键按下 
        private int MOUSEEVENTF_RIGHTUP = 0x0010; //模拟鼠标右键抬起 
        private int MOUSEEVENTF_MIDDLEDOWN = 0x0020; //模拟鼠标中键按下 
        private int MOUSEEVENTF_MIDDLEUP = 0x0040; //模拟鼠标中键抬起 
        private int MOUSEEVENTF_ABSOLUTE = 0x8000; //标示是否采用绝对坐标

        private string filepath;
        private Bitmap m_bmp;
        private Point m_pointLast;
        private string m_base64;
        int screenWidth = Screen.PrimaryScreen.Bounds.Width;
        int screenHeight = Screen.PrimaryScreen.Bounds.Height;
        int imgWidth = 0;
        int imgHeight = 0;
        int rotate = 0;
        int mode = 0;
        int angle = 0;
        public bool m_bDraw = false;
        private DateTime tm_lastdraw = DateTime.Now;
        /// <summary>
        /// 打开图片
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="mode">1: 数据流 2：本地文件 3:拍照文件 4: 截屏文件</param>
        public Form3(string datas, int mode, int firsttime)
        {
            InitializeComponent();
            this.mode = mode;
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
                Bitmap bmp = null;
                if (mode == 1)
                {
                    m_base64 = datas;
                    m_bmp = Base64StringToImage(datas);

                }
                else if (mode == 2)
                {
                    filepath = datas;

                    string filetype = Path.GetExtension(filepath).ToLower();
                    if (filetype == ".png")
                    {
                        string filepath2 = Path.GetDirectoryName(filepath) + "\\" + Path.GetFileNameWithoutExtension(filepath) + ".jpg";
                        try
                        {
                            File.Delete(filepath2);
                            System.Drawing.Bitmap b = (Bitmap)Image.FromFile(filepath);
                            b.Save(filepath2, System.Drawing.Imaging.ImageFormat.Jpeg);
                        }
                        catch (Exception e)
                        {
                            Log.Error(e.Message);
                        }

                        filepath = filepath2;
                    }

                    Log.Info("showPicture2 :" + filepath);
                    m_base64 = Util.ImgToBase64String(filepath);

                    m_bmp = (Bitmap)Image.FromFile(filepath);

                    imgHeight = m_bmp.Height;
                    imgWidth = m_bmp.Width;
                    Log.Info("imgHeight=" + imgHeight + " imgWidth=" + imgWidth);
                    Log.Info("screenHeight=" + screenHeight + " screenWidth=" + screenWidth);
                    //m_bmp = new Bitmap(m_bmp, screenWidth, screenHeight);//不能图片缩放,因为破坏了图片的质量

                }
                else if (mode == 3)
                {
                    filepath = Application.StartupPath + "\\" + datas;

                    string filetype = Path.GetExtension(filepath).ToLower();
                    if (filetype == ".png")
                    {
                        string filepath2 = Path.GetDirectoryName(filepath) + "\\" + Path.GetFileNameWithoutExtension(filepath) + ".jpg";
                        System.Drawing.Bitmap b = (Bitmap)Image.FromFile(filepath);
                        b.Save(filepath2, System.Drawing.Imaging.ImageFormat.Jpeg);
                        filepath = filepath2;
                    }

                    Log.Info("showPicture3 :" + filepath);
                    m_base64 = Util.ImgToBase64String(filepath);
                    m_bmp = (Bitmap)Image.FromFile(filepath);
                    
                }
                else if (mode == 4)
                {
                    m_bmp = ScreenCapture.captureScreen(0, 0);
                    m_base64 = Util.ImgToBase64String(m_bmp);
                }

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
                if (mode == 1 && firsttime==1)
                {
                    string imgName = DateTime.Now.ToString("yyyyMMdd") + "-" + Global.getSchoolID() + "-" + Global.getClassID() + "-" + DateTime.Now.ToString("HHmmss") + ".jpg";
                    string imgDir = Application.StartupPath + "\\" + DateTime.Now.ToString("yyyyMMdd");
                    if (!Directory.Exists(imgDir))
                        Directory.CreateDirectory(imgDir);

                    string imgPath = imgDir + "\\" + imgName;
                    m_bmp.Save(imgPath);
                    Common.uploadPicture(imgPath);
                    Common.uploadCameraEvent(imgName);//相机拍照
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
            if (right == 1)
            {
                angle += 90;
            }
            else
            {
                angle -= 90;
            }
            bool bVerticle = false;
            if (angle != 0 && angle % 90 == 0 && angle % 180 != 0)
                bVerticle = true;

            if (!bVerticle)
            {
                pictureBox1.Width = screenWidth;
                pictureBox1.Height = screenHeight;
            }
            else
            {
                double w1 = screenWidth;
                double h1 = screenHeight;
                double ratio = w1 / h1;
                double w2 = h1;
                double h2 = w2 / ratio;
                pictureBox1.Width = (int)h2;
                pictureBox1.Height = (int)w2;
            }
            pictureBox1.Location = new Point((screenWidth - pictureBox1.Width) / 2, (screenHeight - pictureBox1.Height) / 2);
            ////this.pictureBox1.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
            ////this.pictureBox1.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
            ////img.RotateFlip(RotateFlipType.Rotate90FlipNone);
            ////顺时针旋转90度 RotateFlipType.Rotate90FlipNone 
            ////逆时针旋转90度 RotateFlipType.Rotate270FlipNone 
            ////水平翻转 RotateFlipType.Rotate180FlipY 
            ////垂直翻转 RotateFlipType.Rotate180FlipX

            //double w1 = pictureBox1.Width;
            //double h1 = pictureBox1.Height;
            //double ratio = w1 / h1;
            //double w2 = h1;
            //double h2 = w2 / ratio;
            //pictureBox1.Width = (int)h2;
            //pictureBox1.Height = (int)w2;
            //pictureBox1.Location = new Point((screenWidth - pictureBox1.Width) / 2, (screenHeight - pictureBox1.Height) / 2);  

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
        public void Zoom(double ratio,double ratioX,double ratioY)
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
                double w2 = h1*h1 /w1;
                newWidth = (int)(ratio * w2);
                newHight = (int)(ratio * h2);

                pictureBox1.Width = newWidth;
                pictureBox1.Height = newHight;

                pictureBox1.Location = new Point((screenWidth - pictureBox1.Width) / 2, biasY);
            }
            else
            {
                pictureBox1.Width = newWidth;
                pictureBox1.Height = newHight;
                pictureBox1.Location = new Point(biasX, biasY); 
            }
        }
        public void DrawLine(string percentX, string percentY, int mode, string color, int width)
        {
            DateTime tm_now = DateTime.Now;
            TimeSpan createtimespan = new TimeSpan(tm_lastdraw.Ticks);
            TimeSpan nowtimespan = new TimeSpan(tm_now.Ticks);
            TimeSpan timespan = nowtimespan.Subtract(createtimespan).Duration();
            int timeDiff = timespan.Minutes * 60 + timespan.Seconds;
            tm_lastdraw = DateTime.Now;

            m_bDraw = true;
            double perX = Convert.ToDouble(percentX);
            double perY = Convert.ToDouble(percentY);
            
            double x=0;
            double y=0;

            //double x = (screenWidth-imgWidth)/2+this.imgWidth * perX;
            //double y = (screenHeight-imgHeight)/2+this.imgHeight * perY;
            double screenX = this.screenWidth * perX;
            double screenY = this.screenHeight * perY;
            double screenX2 = 65536 * perX;
            double screenY2 = 65536 * perY;
            x = m_bmp.Width * perX;
            y = m_bmp.Height * perY;
            Log.Info("x :" + x + " y=" + y + ", screenX=" + screenX + ", screenY=" + screenY);

            
            //mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_MOVE, (int)screenX2, (int)screenY2, 0, UIntPtr.Zero);
            //dx和dy指定鼠标坐标系统中的一个绝对位置。在鼠标坐标系统中，屏幕在水平和垂直方向上均匀分割成65535×65535个单元

            //[DllImport("user32.dll")]
            //static extern搜索 int LoadCursorFromFile(string lpFileName); 
            //Cursor myCursor = new Cursor(Cursor.Current.Handle);
            ////加载光标文件：
            //IntPtr colorCursorHandle = LoadCursorFromFile("Cross.cur");
            //myCursor.GetType().InvokeMember("handle",   BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField,   null, myCursor, new Object() { colorCursorHandle });
            //this.Cursor = myCursor;

            if (mode == 0 || timeDiff > 1)
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
