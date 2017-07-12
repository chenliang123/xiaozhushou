using RueHelper.util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace RueHelper
{
    public partial class Form5 : Form
    {
        private static log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public double duration;
        public double currentPositon;
        public string state = "";
        public Form5(string path)
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            this.TopMost = true;
#if DEBUG
            this.TopMost = false;//Form5
#endif
            this.label1.Text = path;

            string fname = Path.GetFileName(path);
            string extName = Path.GetExtension(path).Substring(1);
            string dir = Path.GetDirectoryName(path);


            axWindowsMediaPlayer2.Location = new System.Drawing.Point(0, 0);
            axWindowsMediaPlayer2.URL = dir + "\\" + fname;
            axWindowsMediaPlayer2.Width = this.Width;
            axWindowsMediaPlayer2.Height = this.Height;
            axWindowsMediaPlayer2.stretchToFit = true;

            axWindowsMediaPlayer2.PlayStateChange += new AxWMPLib._WMPOCXEvents_PlayStateChangeEventHandler(player_PlayStateChange);
            PlayVideo();
        }
        
        private void player_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            switch (e.newState)
            {
                case 0:    // Undefined
                    state = "Undefined";
                    break;
                case 1:    // Stopped
                    state = "Stopped";
                    Httpd.OnPublicMsg("Video|Stopped");
                    break;
                case 2:    // Paused
                    state = "Paused";
                    break;
                case 3:    // Playing
                    state = "Playing";
                    duration = axWindowsMediaPlayer2.currentMedia.duration;
                    break;
                case 4:    // ScanForward
                    state = "ScanForward";
                    break;
                case 5:    // ScanReverse
                    state = "ScanReverse";
                    break;
                case 6:    // Buffering
                    state = "Buffering";
                    break;
                case 7:    // Waiting
                    state = "Waiting";
                    break;
                case 8:    // MediaEnded
                    state = "MediaEnded";
                    break;
                case 9:    // Transitioning
                    state = "Transitioning";
                    break;
                case 10:   // Ready
                    state = "Ready";
                    break;
                case 11:   // Reconnecting
                    state = "Reconnecting";
                    break;
                case 12:   // Last
                    state = "Last";
                    break;
                default:
                    state = ("Unknown State: " + e.newState.ToString());
                    break;
            }
        }

        public void StopVideo()
        {
            try
            {
                axWindowsMediaPlayer2.Ctlcontrols.pause();
            }
            catch (Exception ex) { }
        }

        public void PlayVideo()
        {
            try
            {
                axWindowsMediaPlayer2.Ctlcontrols.play();
            }
            catch (Exception ex) { }
        }

        public bool FastForward()
        {
            if (state != "Stopped")
            {
                try
                {
                    double span = duration - axWindowsMediaPlayer2.Ctlcontrols.currentPosition;
                    if (span > 10)
                        axWindowsMediaPlayer2.Ctlcontrols.currentPosition += 10;
                    else
                        axWindowsMediaPlayer2.Ctlcontrols.currentPosition = duration-1;
                    return true;
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message);
                    return false;
                }
            }
            else
            {
                return false;
            }

        }
        public void FastReverse()
        {
            try
            {
                if (axWindowsMediaPlayer2.Ctlcontrols.currentPosition > 10)
                    axWindowsMediaPlayer2.Ctlcontrols.currentPosition -= 10;
                else
                    axWindowsMediaPlayer2.Ctlcontrols.currentPosition = 0;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
        }

        /// <summary>
        /// 调整音量 volume: integer,0-100 
        /// </summary>
        /// <param name="volume"></param>
        public void changeVolume(int volume)//integer; 音量，0-100 
        {
            try
            {
                axWindowsMediaPlayer2.settings.volume = volume;
            }
            catch (Exception ex) { }
        }

        private void Form5_Load(object sender, EventArgs e)
        {

        }

        /*
[基本属性] 　 
URL:String; 指定媒体位置，本机或网络地址 
uiMode:String; 播放器界面模式，可为Full, Mini, None, Invisible 
playState:integer; 播放状态，1=停止，2=暂停，3=播放，6=正在缓冲，9=正在连接，10=准备就绪 
enableContextMenu:Boolean; 启用/禁用右键菜单 
fullScreen:boolean; 是否全屏显示 
[controls] wmp.controls //播放器基本控制 
controls.play; 播放 
controls.pause; 暂停 
controls.stop; 停止 
controls.currentPosition:double; 当前进度 
controls.currentPositionString:string; 当前进度，字符串格式。如“00:23” 
controls.fastForward; 快进 
controls.fastReverse; 快退 
controls.next; 下一曲 
controls.previous; 上一曲 
[settings] wmp.settings //播放器基本设置 
settings.volume:integer; 音量，0-100 
settings.autoStart:Boolean; 是否自动播放 
settings.mute:Boolean; 是否静音 
settings.playCount:integer; 播放次数 
[currentMedia] wmp.currentMedia //当前媒体属性 
currentMedia.duration:double; 媒体总长度 
currentMedia.durationString:string; 媒体总长度，字符串格式。如“03:24” 
currentMedia.getItemInfo(const string); 获取当前媒体信息"Title"=媒体标题，"Author"=艺术家，"Copyright"=版权信息，"Description"=媒体内容描述，"Duration"=持续时间（秒），"FileSize"=文件大小，"FileType"=文件类型，"sourceURL"=原始地址 
currentMedia.setItemInfo(const string); 通过属性名设置媒体信息 
currentMedia.name:string; 同 currentMedia.getItemInfo("Title") 
         */
    }
}
