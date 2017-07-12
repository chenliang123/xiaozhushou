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
using System.Runtime.InteropServices;
using RueHelper.util;

namespace RueHelper
{
    public partial class FormScore : Form
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

        public FormScore(string groupnum,string scorenum)
        {
            InitializeComponent();
            SetPanel(groupnum,scorenum);           

            this.Height = 180;
            this.Width = screenWidth;
            panel2.Visible = true;
            panel2.Width = screenWidth;
           // panel2.Height = 150;

            StartPosition = FormStartPosition.Manual;
            SetDesktopLocation(0, screenHeight - this.Height);

            
        }

        public void SetPanel(string groupnum, string numberstr)
        {
            int iL = (screenWidth - 176) / 2;
            panel2.Show();
            panel2.Parent = this;
            bgScore.Parent = this.panel2;
            bgScore.Location = new Point(iL,4);
            lbName.Parent = this.bgScore;
            lbName.Text = groupnum;
            lbScore.Parent = this.bgScore;
            if (numberstr == "1")
            {
                lbScore.Text = "+1分";
            }
            else{
                lbScore.Text = "-1分";
            }
            
            lbName.BringToFront();
            lbScore.BringToFront();
            
            this.TopMost = true;
            this.BringToFront();
            this.Show();
            timer1.Stop();
            timer1.Enabled = true;
            timer1.Start();          
        }

        private void Theout(object sender, EventArgs e)
        {
            this.Hide();
            timer1.Stop();
        } 
    }
}
