using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AxShockwaveFlashObjects;//引用Flash控件

namespace RueHelper
{
    public partial class Form4 : Form
    {
        public string filename = "";
        public string filepath = "";
        public Form4(string url)
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            this.TopMost = true;
#if DEBUG
            this.TopMost = false;//Form4
#endif

            if (url == null || url == "")
            {
                url = "http://demo.sc.chinaz.com//Files/DownLoad/flash2/201512/flash4198.swf";
            }

            if(url.StartsWith("http"))
            {
                this.webBrowser1.Location = new System.Drawing.Point(0, 0);
                this.webBrowser1.Width = Screen.PrimaryScreen.WorkingArea.Width;
                this.webBrowser1.Height = Screen.PrimaryScreen.WorkingArea.Height + 38;
                this.webBrowser1.Url = new Uri(url);
            }
            else
            {
                filepath = url;
                filename = Path.GetFileName(url);
                axShockwaveFlash1.Location = new System.Drawing.Point(0, 0);
                axShockwaveFlash1.Width = Screen.PrimaryScreen.WorkingArea.Width;
                axShockwaveFlash1.Height = Screen.PrimaryScreen.WorkingArea.Height + 38;
                axShockwaveFlash1.Movie = url;

                axShockwaveFlash1.Play();
            }

        }

        public void NextSlide()
        {
            int totalFrame = axShockwaveFlash1.TotalFrames;
            int span = totalFrame - axShockwaveFlash1.FrameNum;
            if (span > 10)
            {
                axShockwaveFlash1.GotoFrame(axShockwaveFlash1.FrameNum + 10);
                axShockwaveFlash1.Play();
            }
        }
        public void PreviousSlide()
        {
            int span = axShockwaveFlash1.FrameNum - 10;
            if(span > 0 )
            {
                axShockwaveFlash1.GotoFrame(axShockwaveFlash1.FrameNum - 10);
                axShockwaveFlash1.Play();
            }
        }
        public void Pause()
        {
            //axShockwaveFlash1.StopPlay();
            axShockwaveFlash1.Stop();
        }
        public void Play()
        {
            axShockwaveFlash1.Play();
        }
    }
}
