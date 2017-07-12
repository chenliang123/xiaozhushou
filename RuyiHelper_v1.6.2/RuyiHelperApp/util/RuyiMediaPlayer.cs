using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace RueHelper.util
{
    class RuyiMediaPlayer
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern int GetShortPathName(string lpszLongPath, string shortFile, int cchBuffer);
        [DllImport("winmm.dll", EntryPoint = "mciSendString", CharSet = CharSet.Auto)]
        public static extern int mciSendString(string lpstrCommand, string lpstrReturnString, int uReturnLength, int hwndCallback);


        public class Sound
        {
            private Panel m_PlayPanel;
            public Sound(Panel PanelShow)
            {
                m_PlayPanel = PanelShow;
                _MyTime.Interval = 1;
                _MyTime.Tick += new EventHandler(_MyTime_Tick);
                PanelShow.SizeChanged += new EventHandler(PanelShow_SizeChanged);
            }
            /// <summary>
            /// 控制大小
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            void PanelShow_SizeChanged(object sender, EventArgs e)
            {
                string _Temp = "".PadLeft(128, ' ');
                _Long = mciSendString("put ZgkeMedia window at 0 0 " + m_PlayPanel.Width.ToString() + " " + m_PlayPanel.Height.ToString(), _Temp, 128, 0); //播放音频文件            

            }
            /// <summary>
            /// 时间处理
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            void _MyTime_Tick(object sender, EventArgs e)
            {
                int _TempPoint = PlayPoint;       //获取当前播放时间
                if (_TempPoint != _OldIndex)  //如果播放时间是当前获取的时间 不进行处理
                {
                    if (_TempPoint != m_PlayMillisecond)                  //如果是播放到最后
                    {
                        if (PlayRun != null) PlayRun(this, _TempPoint + 1);         //运行委托
                    }
                    else
                    {
                        while (true)         //一直循环到结束
                        {
                            string _Temp = GetPlayMode();             //获取播放类型
                            if (_Temp == "stopped" || _Temp == "")         //如果是停止状态或则退出状态
                            {
                                Stop();             //停止播放                             
                                if (PlayRun != null) PlayRun(this, m_PlayMillisecond);       //执行委托
                                _MyTime.Enabled = false;          //关闭时间控件
                                break;
                            }
                            else
                            {
                                Application.DoEvents();        //进行界面处理
                            }
                        }
                    }
                    _OldIndex = _TempPoint;      //设置当前时间为处理后
                }
            }

            #region 播放属性
            private IList<string> m_SoundPathList = new List<string>();
            /// <summary>
            /// 播放列表
            /// </summary>
            public IList<string> SountPathList { get { return m_SoundPathList; } }
            /// <summary>
            /// 当前播放ID
            /// </summary>
            private int m_PlayIndex = 0;
            /// <summary>
            /// 播放索引
            /// </summary>
            public int PlayIndex { get { return PlayIndex; } }
            private int m_PlayMillisecond = 0;
            /// <summary>
            /// 返回时间总长度
            /// </summary>
            public int PlayMillisecond { get { return m_PlayMillisecond; } }
            /// <summary>
            /// 当前播放时间
            /// </summary>
            public int PlayPoint
            {
                get
                {
                    string _Temp = "".PadLeft(128, ' ');
                    int TTT = mciSendString("status ZgkeMedia position", _Temp, 128, 0);
                    _Temp = _Temp.Trim().Remove(_Temp.Trim().Length - 1, 1);
                    if (_Temp.Length == 0) return 0;
                    return Int32.Parse(_Temp);
                }
                set
                {
                    if (value > m_PlayMillisecond) return;
                    string _Temp = "".PadLeft(128, ' ');
                    string _PlayType = GetPlayMode();             //获取播放类型  
                    _Long = mciSendString("Seek ZgkeMedia to " + value.ToString(), _Temp, 128, 0); //暂停文件  //移动到那里
                    switch (_PlayType)
                    {
                        case "playing":
                            _Long = mciSendString("play ZgkeMedia", _Temp, 128, 0); //继续播放文件 
                            _MyTime.Enabled = true;
                            break;
                        default:
                            break;
                    }
                }
            }
            /// <summary>
            /// 音量 
            /// </summary>
            public int Volume
            {
                get
                {
                    string _Temp = "".PadLeft(128, ' ');
                    mciSendString("status ZgkeMedia volume", _Temp, 128, 0);
                    _Temp = _Temp.Trim().Remove(_Temp.Trim().Length - 1, 1);
                    if (_Temp.Length == 0) return 0;
                    return Int32.Parse(_Temp);

                }
                set
                {
                    string _Temp = "".PadLeft(128, ' ');
                    mciSendString("setaudio ZgkeMedia volume to " + value.ToString(), _Temp, 128, 0);
                }
            }
            #endregion
            /// <summary>
            /// API的信息 0为正常
            /// </summary>
            private int _Long = 0;
            /// <summary>
            /// 时间控件 WINDOWS.FROM.TIMER 其他的取不到时间
            /// </summary>
            private Timer _MyTime = new Timer();
            public delegate void _PlayRun(Sound sender, int PlayPoint);
            public event _PlayRun PlayRun;
            /// <summary>
            /// 上次记录的时间
            /// </summary>
            private int _OldIndex = 0;
            /// <summary>
            /// 添加文件
            /// </summary>
            /// <param name="FileName">文件路径</param>
            public void AddSoundFile(string FileName)
            {
                m_SoundPathList.Add(FileName);
            }
            /// <summary>
            /// 播放
            /// </summary>
            public void Play()
            {
                _MyTime.Enabled = false;
                if (m_SoundPathList.Count == 0) return;
                string _Temp = GetPlayMode();
                switch (_Temp)  //根据播放状态设置
                {
                    case "playing":
                        _MyTime.Enabled = true;
                        return;
                    case "paused":
                        _Temp = "".PadLeft(128, ' ');
                        _Long = mciSendString("Resume ZgkeMedia", _Temp, 128, 0); //继续播放  
                        _MyTime.Enabled = true;
                        return;
                    case "stopped":
                        _Temp = "".PadLeft(128, ' ');
                        _Long = mciSendString("play ZgkeMedia", _Temp, 128, 0); //继续播放文件 
                        _MyTime.Enabled = true;
                        break;
                    default:
                        break;
                }
                string _FilePath = "".PadLeft(260, ' ');
                string _W32Path = m_SoundPathList[m_PlayIndex];
                int xxx = GetShortPathName(_W32Path, _FilePath, 260);
                _FilePath = _FilePath.Trim().Remove(_FilePath.Trim().Length - 1, 1);
                string _FileType = GetFileType(_W32Path);       //获取文件类型

                if (_FileType.Length == 0) return;
                switch (_FileType)
                {
                    case "digitalvideo":
                        PlayMpeg(_FilePath, "MPEGVideo");
                        break;
                    default:
                        PlaySount(_FilePath, _FileType);
                        break;
                }
                _MyTime.Enabled = true;
            }
            /// <summary>
            /// 播放声音类型
            /// </summary>
            private void PlaySount(string _FilePath, string _FileType)
            {
                string _PlayCommand = "open " + _FilePath + " alias ZgkeMedia type " + _FileType;
                string _Temp = "".PadLeft(128, ' ');
                _Long = mciSendString(_PlayCommand, _Temp, 128, 0); //初始化音频文件   
                _Temp = "".PadLeft(128, ' ');
                _Long = mciSendString("play ZgkeMedia", _Temp, 128, 0); //播放音频文件
                _Temp = "".PadLeft(128, ' ');
                _Long = mciSendString("status ZgkeMedia length", _Temp, 128, 0);  //获取时间                  
                if (_Temp.Trim().Remove(_Temp.Trim().Length - 1, 1).Length == 0) return;
                m_PlayMillisecond = Int32.Parse(_Temp.Trim());
            }
            /// <summary>
            /// 播放AVI类型
            /// </summary>
            /// <param name="_FilePath"></param>
            private void PlayMpeg(string _FilePath, string _FileType)
            {
                string _PlayCommand = "open " + _FilePath + " alias ZgkeMedia type " + _FileType + " parent " + m_PlayPanel.Handle.ToInt32().ToString() + " style child";
                string _Temp = "".PadLeft(128, ' ');
                _Long = mciSendString(_PlayCommand, _Temp, 128, 0); //初始化音频文件 
                //"put AVI 文件名 window at X Y Width Height",                  
                _Temp = "".PadLeft(128, ' ');
                _Long = mciSendString("put ZgkeMedia window at 0 0 " + m_PlayPanel.Width.ToString() + " " + m_PlayPanel.Height.ToString(), _Temp, 128, 0); //播放音频文件            
                _Temp = "".PadLeft(128, ' ');
                _Long = mciSendString("play ZgkeMedia", _Temp, 128, 0); //播放音频文件                 
                _Temp = "".PadLeft(128, ' ');
                _Long = mciSendString("status ZgkeMedia length", _Temp, 128, 0);  //获取时间                  
                if (_Temp.Trim().Remove(_Temp.Trim().Length - 1, 1).Length == 0) return;
                m_PlayMillisecond = Int32.Parse(_Temp.Trim());
            }
            /// <summary>
            /// 停止播放
            /// </summary>
            public void Stop()
            {
                string _Temp = "".PadLeft(128, ' ');
                _Long = mciSendString("close ZgkeMedia", _Temp, 128, 0); //关闭文件     
                _MyTime.Enabled = false;
            }
            /// <summary>
            /// 暂停
            /// </summary>
            public void Pause()
            {
                string _Temp = "".PadLeft(128, ' ');
                _Long = mciSendString("pause ZgkeMedia", _Temp, 128, 0); //暂停文件 
                _MyTime.Enabled = false;
            }


            /// <summary>
            /// 获取文件的播放类型
            /// </summary>
            /// <param name="FileName">文件</param>
            /// <returns>类型</returns>
            private string GetFileType(string FileName)
            {
                string _FilePath = "".PadLeft(260, ' ');
                _Long = GetShortPathName(FileName, _FilePath, 260);
                _FilePath = _FilePath.Trim().Remove(_FilePath.Trim().Length - 1, 1);
                string _Temp = "".PadLeft(128, ' ');
                _Long = mciSendString("capability " + _FilePath + " device type", _Temp, 128, 0);
                if (_Temp.Length == 0 || _Temp.Trim().Remove(_Temp.Trim().Length - 1, 1).Length == 0)
                {
                    return "digitalvideo";
                }
                return _Temp.Trim().Remove(_Temp.Trim().Length - 1, 1);
            }
            /// <summary>
            /// 获取播放状态
            /// </summary>
            /// <returns></returns>
            private string GetPlayMode()
            {
                string _Temp = "".PadLeft(128, ' ');
                mciSendString("status ZgkeMedia mode", _Temp, 128, 0);
                return _Temp.Trim().Remove(_Temp.Trim().Length - 1, 1);
            }
        }
    }
}
