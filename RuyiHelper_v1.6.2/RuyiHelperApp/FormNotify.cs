using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;

namespace RueHelper
{
    public partial class FormNotify : Form
    {
        private static log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public delegate void CallUpdateForm(string title, string msg);
        public delegate void CallClose();
        public int m_code = 0;
        public System.Windows.Forms.Timer tm_hide;
        public static bool m_PPTImgExporting = false;
        public FormNotify(string title,string msg,int seconds)
        {
            InitializeComponent();
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;
            double taskbarHeight = SystemParameters.PrimaryScreenHeight - SystemParameters.WorkArea.Height;

            //this.Height = panelH;
            //this.Width = screenWidth;

            StartPosition = FormStartPosition.Manual;
            SetDesktopLocation(screenWidth - this.Width, screenHeight - this.Height - (int)taskbarHeight);

            //Bitmap bm = new Bitmap(global::RueHelper.Properties.Resources.notify_background); //fbImage图片路径
            //this.BackgroundImage = bm;//设置背景图片
            //this.BackgroundImageLayout = ImageLayout.Stretch;//设置背景图片自动适应

            label1.Text = title;
            label2.Text = msg;
            updateWifi();
            //this.TopMost = true;
            this.BringToFront();
            //this.ShowInTaskbar = false;

            //tm_hide = new System.Windows.Forms.Timer();
            //tm_hide.Interval = 1000 * seconds;//2秒
            //tm_hide.Tick += new EventHandler(t_Hide);
            //tm_hide.Start();
        }
        void t_Hide(object sender, EventArgs e)
        {
            this.Visible = false;
            ((System.Windows.Forms.Timer)sender).Stop();
        }
        public void updateForm(string title,string msg)
        {
            label1.Text = title;
            label2.Text = msg;
            updateWifi();
            this.TopMost = true;
        }
        public void InvokeUpdate(string title, string msg)
        {
            m_PPTImgExporting = true;
            if (this.InvokeRequired)
            {
                CallUpdateForm cb = new CallUpdateForm(InvokeUpdate);
                this.Invoke(cb, new object[] { title, msg });
                return;
            }
            else
            {
                label1.Text = title;
                label2.Text = msg;
                updateWifi();
                this.TopMost = true;
                this.BringToFront();
                this.Show();
            }
        }

        public void InvokeClose()
        {
            m_PPTImgExporting = false;
            if (this.InvokeRequired)
            {
                CallClose cb = new CallClose(InvokeClose);
                this.Invoke(cb, new object[] {});
                return;
            }
            else
            {
                this.Close();
                this.Dispose();
            }
        }
        private void updateWifi()
        {
            string wifi = Global.getWiFi();
            if (wifi.Length == 0)
                wifi = "               ";//15
            if (wifi.Length < 20)
                wifi = "   " + wifi + "   ";//5
            label_wifi.Text = "【班级WiFi: " + wifi + "】";
        }
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
            Form1.closeFormNotify();
        }
        void t_Tick(object sender, EventArgs e)
        {
            //---hide----
            //this.Visible = false;
            //((System.Windows.Forms.Timer)sender).Stop();

            //---close---
            //this.Dispose();
            this.Close();
        }
    }
}
