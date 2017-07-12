using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace RueHelper
{
    public partial class Form7 : Form
    {
        private static log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Form7(string url)
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            this.TopMost = true;
#if DEBUG
            this.TopMost = false;//Form7
#endif
            if (url == "" || url == null)
            {
                url = "http://teacher.joinsino.com/UploadFile/201601030135132168.pdf_1.jpeg";
            }
            Image img = GetNetImg(url);
            if (img != null)
            {
                string style = "";
                if (img.Width > Screen.PrimaryScreen.WorkingArea.Width)
                {
                    style = "style=\"width:100%;\"";
                }
                string html = "<center><img src=\"" + url + "\" " + style + " /></center>";
                this.webBrowser1.DocumentText = html;
            }
            else
            {
                MessageBox.Show("网络错误！");
            }
        }

        public void ScrollTop()
        {
            int curtop = this.webBrowser1.Document.Body.ScrollTop;
            if (curtop > 60)
            {
                this.webBrowser1.Document.Window.ScrollTo(0, curtop - 60);
            }
            else
            {
                this.webBrowser1.Document.Window.ScrollTo(0, 0);
            }
        }

        public void ScrollBottom()
        {
            int curtop = this.webBrowser1.Document.Body.ScrollTop;
            this.webBrowser1.Document.Window.ScrollTo(0, curtop + 60);
        }

        public void ScrollTo(int y,int screenH)
        {
            int scrollHeight = this.webBrowser1.Document.Body.ScrollRectangle.Height;
            int _scrollto = y * scrollHeight / screenH;
            this.webBrowser1.Document.Window.ScrollTo(0, _scrollto);
        }

        private Image GetNetImg(string url)
        {
            Image image = null;
            try
            {
                WebRequest request = WebRequest.Create(url);
                request.Credentials = CredentialCache.DefaultCredentials;
                Stream s = request.GetResponse().GetResponseStream();
                byte[] b = new byte[74373];
                MemoryStream mes_keleyi_com = new MemoryStream(b);
                s.Read(b, 0, 74373);
                s.Close();
                image = Image.FromStream(mes_keleyi_com);
            }
            catch (Exception ex) {
                MessageBox.Show("下载图片失败:" + url+"\r\n"+ex.Message);
            }
            return image;
        }
    }
}
