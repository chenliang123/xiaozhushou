using Newtonsoft.Json.Linq;
using RueHelper.util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace RueHelper
{
    public partial class FormCamera : Form
    {
        private static log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string PhoneNumber = "13466630815";
        private string AppKey = "86a1e67b74de4ad8955ef62b11d7d2e7";
        private string SecretKey = "bb1e86393bbe206c8b809f0522f40b51";
        private bool bLogin = false;

        int a = 0;//记录用户是否正在播放状态，0为未播放，1为正在播放
        string jsonstr = "";//摄像头列表json
        string cameraid = "";//摄像头id  
        string cameraid2 = "";//存放全屏播放的摄像头id
        string statu = "";//摄像头是否加密，1为加密，0为未加密
        int c = 1;//表示用户选择的是几个窗口播放，默认为1                      
        PictureBox[] picbox = new PictureBox[9];//创建对象数组，存放picbox对象，最多9画面，所以最多9个
        private static IntPtr[] handle = new IntPtr[9];//存放播放句柄
        private string[] cameraid1 = new string[9];//存放cameraid
        IntPtr[] SessionId = new IntPtr[9];//存放申请的session
        int j = 0;
        int d = 0;

        int m_Width = 500;
        int m_Height = 300;

        public FormCamera()
        {
            InitializeComponent();
            createBox();
            {
                Ini ini = new Ini(Application.StartupPath + "\\App.ini");
                PhoneNumber = ini.Read("Camera", "PhoneNumber");
                AppKey = ini.Read("Camera", "AppKey");
                SecretKey = ini.Read("Camera", "SecretKey");
            }


            HkAction.init(PhoneNumber, AppKey, SecretKey);

            HkAction.MainForm = this;
            try
            {
                HkAction.Start();//进入系统自动初始化库                
            }
            catch (Exception ex)//出现异常则提示
            {
                MessageBox.Show("异常！" + ex.ToString(), "提示", MessageBoxButtons.OK);
            }
            //this.Show();
            //this.TopMost = true;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            createBox();
        }
        public bool login()
        {
            bool bret = false;
            try
            {
                if (HkAction.GetAccessToken() != null)
                {
                    
                    bLogin = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("视频用户登录失败！", "警告", MessageBoxButtons.OK);
                bLogin = false;
            }
            return bLogin;
        }

        #region 设备列表按钮
        public int getDeviceList()
        {
            int n = 0;
            this.flowLayoutPanel1.Controls.Clear();
            try
            {
                string getList = HkAction.playList();
                jsonstr = getList;
                n = jsonHandle(jsonstr);
            }
            catch (Exception ex)//出现异常则提示
            {
                MessageBox.Show("异常！" + ex.ToString(), "提示", MessageBoxButtons.OK);
            }
            return n;
        }
        #endregion


        #region 开始播放单画面

        private void Start_Play1()
        {
            SessionId[j] = HkAction.AllocSession();//每次点击存放session
            if (SessionId[j] != null)//每次播放申请会话
            {
                bool play;
                try
                {
                    play = HkAction.Play(handle[0], cameraid1[j], SessionId[j]);
                    if (play == true)
                    {
                        a = 1;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("异常：" + ex.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("申请会话异常！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        #endregion

        #region 用户选择保存截图路径
        //选择保存路径
        private string ShowSaveFileDialog(string time)
        {
            string path1 = "";
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.RestoreDirectory = true;//保存对话框是否记忆上次打开的目录
            if (sfd.ShowDialog() == DialogResult.OK)//点了保存按钮进入 
            {
                path1 = sfd.FileName.ToString(); //获得文件路径 
                return path1;
            }
            else
            {
                return null;
            }

        }

        #endregion

        #region 截图按钮
        private void btnCapt_Click(object sender, EventArgs e)
        {
            HkAction.MainForm.Hide();
            string time = DateTime.Now.ToString("yyyyMMddHHmmss"); //取系统时间
            if (a == 1)
            {
                string path1 = ShowSaveFileDialog(time);//获取用户选择路径
                if (path1 == null)
                {
                    MessageBox.Show("截图取消", "提示", MessageBoxButtons.OK);
                    return;
                }

                string path = path1 + "-" + time + ".jpg";//保存路径和文件名(用户选择路径)
                //string path = @"C:\Users\Administrator\Downloads\"+time + ".jpg";//指定路径

                if (HkAction.CapturePicture(path))//调用SDK截图方法
                {
                    MessageBox.Show("截图成功，保存在：" + path, "提示", MessageBoxButtons.OK);
                }
                else
                {
                    MessageBox.Show("截图失败", "提示", MessageBoxButtons.OK);
                }

            }
            else
            {
                MessageBox.Show("请先点击“开始”按钮！", "提示", MessageBoxButtons.OK);
            }

            HkAction.MainForm.Show();
        }
        #endregion

        #region 截图按钮
        public string CaptureView()
        {
            string day = DateTime.Now.ToString("yyyyMMdd"); //取系统时间
            string time = DateTime.Now.ToString("yyyyMMddHHmmss"); //取系统时间
            string path = day + "\\c-" + time + ".jpg";
            if (a == 1)
            {
                if (HkAction.CapturePicture(path))//调用SDK截图方法
                {
                    //MessageBox.Show("截图成功，保存在：" + path, "提示", MessageBoxButtons.OK);
                }
                else
                {
                    path = "";
                    MessageBox.Show("截图失败", "提示", MessageBoxButtons.OK);
                }

            }
            else
            {
                MessageBox.Show("请先点击“开始”按钮！", "提示", MessageBoxButtons.OK);
                path = "";
            }
            return path;
        }
        #endregion

        #region 停止播放按钮
        private void btnStop_Click(object sender, EventArgs e)
        {
            for (int g = 0; g <= j; g++)
            {
                HkAction.Stop(SessionId[g]);
            }
            Array.Clear(cameraid1, 0, cameraid1.Length);
            Array.Clear(SessionId, 0, SessionId.Length);
            Array.Clear(handle, 0, handle.Length);
            Array.Clear(picbox, 0, picbox.Length);
            createBox();
            j = 0;
            a = 0;
        }
        #endregion

        #region 停止播放方法
        private void stopPlay()
        {
            for (int g = 0; g <= j; g++)
            {
                HkAction.Stop(SessionId[g]);
            }
            Array.Clear(cameraid1, 0, cameraid1.Length);
            Array.Clear(SessionId, 0, SessionId.Length);
            Array.Clear(handle, 0, handle.Length);
            Array.Clear(picbox, 0, picbox.Length);
            createBox();
            j = 0;
            a = 0;
        }
        #endregion        


        #region 创建播放容器并添加双击事件
        private void createBox()
        {
            this.flowLayoutPanel2.Controls.Clear();//清楚所有容器
            if (c == 1)//单画面只需创建一个容器即可
            {
                PictureBox pic = new PictureBox();
                pic.Size = new Size(m_Width, m_Height);//指定播放容器大小
                pic.BackColor = Color.Black;
                pic.Name = "picBox0";
                this.flowLayoutPanel2.Controls.Add(pic);//创建播放容器 
                picbox[0] = pic;
                handle[0] = picbox[0].Handle;
                //picbox[0].MouseDoubleClick += new MouseEventHandler(picbox_MouseDoubleClick);//添加鼠标双击击事件，用于全屏播放
            }
            else//多画面时根据C的值创建容器
            {
                for (int i = 0; i < c; i++)
                {
                    PictureBox pic = new PictureBox();
                    if (c == 4)
                    {
                        pic.Size = new Size(470, 330);//指定播放容器大小4画面
                    }
                    else
                    {
                        pic.Size = new Size(312, 218);//指定播放容器大小9画面
                    }
                    pic.BackColor = Color.Black;
                    pic.Name = "picBox" + i.ToString();
                    this.flowLayoutPanel2.Controls.Add(pic);//创建播放容器 
                    picbox[i] = pic;
                    handle[i] = picbox[i].Handle;
                    pic.Margin = new System.Windows.Forms.Padding(1);
                    //picbox[i].MouseDoubleClick += new MouseEventHandler(picbox_MouseDoubleClick);
                }
            }
        }
        #endregion

        #region 开始所有当前播放
        private void startall()
        {
            try
            {
                for (int s = 0; s < j; s++)
                {
                    HkAction.Play(handle[s], cameraid1[s], SessionId[s]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("异常" + ex.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            a = 1;
        }
        #endregion

        #region 构造实体类
        public class json
        {
            public int resultCode { get; set; }
            public int count { get; set; }
            public List<cameraList> carameList { get; set; }
        }

        public class cameraList
        {
            public string cameraId { get; set; }
            public string cameraName { get; set; }
            public int cameraNo { get; set; }
            public int defence { get; set; }
            public string deviceId { get; set; }
            public string deviceName { get; set; }
            public int deviceSerial { get; set; }
            public int isEncrypt { get; set; }
            public int isShared { get; set; }
            public string picUrl { get; set; }
            public int status { get; set; }
            public int videoLevel { get; set; }
        }

        public class frm//窗体实体类，用来保存全局变量，并在静态方法中调用
        {
            public int a { get; set; }//记录用户点击的是第几个摄像头
            public int b { get; set; }//记录用户是否正在播放状态，0为未播放，1为正在播放
            public int c { get; set; }//表示用户选择的是几个窗口播放，默认为1

            public string jsonstr { get; set; }//摄像头列表json
            public string cameraid { get; set; }//摄像头id        
            public string safeKey { get; set; }//摄像头密码，默认ABCDEF       

        }

        public static JObject jObjInfo { get; set; }
        public static int isOpertion { get; set; }//0未执行   1正在执行  2已经执行
        public static MessageType mType { get; set; }

        public enum MessageType
        {
            INS_PLAY_EXCEPTION = 0,   // 播放异常，通常是设备断线或网络异常造成
            INS_PLAY_RECONNECT = 1,   // 重连，实时流播放时内部会自动重连
            INS_PLAY_RECONNECT_EXCEPTION = 2,   // 重连异常
            INS_PLAY_START = 3,   // 播放开始
            INS_PLAY_STOP = 4,   // 播放终止
            INS_PLAY_ARCHIVE_END = 5,   // 播放结束，回放结束时会有此消息

            INS_RECORD_FILE = 20,  // 查询的录像文件(录像搜索结果)
            INS_RECORD_SEARCH_END = 21,  // 录像查询结束（暂不使用）
            INS_RECORD_SEARCH_FAILED = 22,  // 录像查询失败
        }
        #endregion


        #region 处理json方法，获取设备列表以缩略图显示并显示摄像头一些信息

        public int jsonHandle(string str)
        {
            string jsonStr = str;
            JObject jsonObj = JObject.Parse(jsonStr);
            JArray jar = JArray.Parse(jsonObj["cameraList"].ToString());
            int a = Util.toInt(jsonObj["count"].ToString());
            for (int m = 0; m < a; m++)
            {
                JObject j = JObject.Parse(jar[m].ToString());//取第几个摄像头信息放入对象j
                string picurl = j["picUrl"].ToString();//取出缩略图url 
                Label label1 = new Label();
                label1.Text = j["cameraName"].ToString();//取出摄像头名字
                Label label2 = new Label();
                string isEncrypt = j["isEncrypt"].ToString();//取出摄像头状态
                if (isEncrypt == "0")
                {
                    label2.Text = "加密状态:未加密";//把摄像头加密状态给label
                }
                else if (isEncrypt == "1")
                {
                    label2.Text = "加密状态:已加密";//把摄像头加密状态给label
                }
                string status = j["status"].ToString();
                Label label3 = new Label();
                if (status == "0")
                {
                    label3.Text = "状态:未在线";//把摄像头状态给label
                }
                else
                {
                    label3.Text = "状态:在线";//把摄像头状态给label
                }
                PictureBox picbox = new PictureBox();
                picbox.Size = new Size(120, 120);
                picbox.Name = picbox + m.ToString();//根据i值来取名
                picbox.BackgroundImage = Image.FromStream(WebRequest.Create(picurl).GetResponse().GetResponseStream());//取网络图片
                picbox.BackgroundImageLayout = ImageLayout.Stretch;//背景图自适应控件大小
                this.flowLayoutPanel1.Controls.Add(picbox);//添加控件picturebox
                this.flowLayoutPanel1.Controls.Add(label1);//添加控件label
                this.flowLayoutPanel1.Controls.Add(label2);//添加控件label
                this.flowLayoutPanel1.Controls.Add(label3);//添加控件label
                picbox.MouseClick += new MouseEventHandler(picbox_MouseClick);//添加鼠标点击事件，方便后面确定点击的是哪个摄像头              
                picbox.MouseHover += new System.EventHandler(picbox_MouseHover);
                picbox.MouseLeave += new System.EventHandler(picbox_MouseLeave);
                //picbox.SizeModeChanged += new System.EventHandler(picbox_SizeModeChanged);                
            }
            foreach (Control c in flowLayoutPanel1.Controls)
                c.Margin = new Padding(5);

            return a;
        }
        #endregion

        #region 处理json方法重载(无参数)，获取用户摄像头总数

        public int jsonHandle()
        {
            JObject jsonObj = JObject.Parse(jsonstr);
            JArray jar = JArray.Parse(jsonObj["cameraList"].ToString());
            int a = Util.toInt(jsonObj["count"].ToString());

            return a;
        }
        #endregion
        #region 处理json方法重载(+1)，获取用户摄像头id

        public string jsonHandle(int n)
        {
            JObject jsonObj = JObject.Parse(jsonstr);
            JArray jar = JArray.Parse(jsonObj["cameraList"].ToString());
            JObject j = JObject.Parse(jar[n].ToString());//取第几个摄像头信息放入对象j
            cameraid = j["cameraId"].ToString();//取出
            return cameraid;
        }
        #endregion

        #region 处理json方法重载(+2)，获取用户摄像头加密状态

        public string jsonHandle1(int n)
        {
            JObject jsonObj = JObject.Parse(jsonstr);
            JArray jar = JArray.Parse(jsonObj["cameraList"].ToString());
            JObject j = JObject.Parse(jar[n].ToString());//取第几个摄像头信息放入对象j
            statu = j["isEncrypt"].ToString();//取出
            return statu;
        }
        #endregion

        #region 处理json方法重载(+2)，获取用户摄像头加密状态
        public JObject getCamera(int n)
        {
            JObject jsonObj = JObject.Parse(jsonstr);
            JArray jar = JArray.Parse(jsonObj["cameraList"].ToString());
            JObject j = JObject.Parse(jar[n].ToString());//取第几个摄像头信息放入对象j
            return j;
        }
        #endregion   


        #region 单击该缩略图获取摄像头ID并播放
        private void picbox_MouseClick(object sender, MouseEventArgs e)
        {
            PictureBox picb1 = sender as PictureBox;//取出点击的控件sender
            string name = "";
            if (picb1 != null)//点击则非空，否则为空
            {
                string _name = picb1.Name;
                name = _name.Substring(_name.Length - 1, 1); //取名字中最后一个字，它正好是控件的index
            }
            int n = Util.toInt(name);//string 转换int

            JObject oCamera = getCamera(n);

            cameraid = oCamera["cameraId"].ToString(); ;//取出该摄像头id
            if (a == 1 && c != 1)
            {
                for (int t = 0; t < 9; t++)
                {
                    if (cameraid == cameraid1[t])//检查视频是否已在播放中
                    {
                        MessageBox.Show("该视频已经在播放中，请勿重复点击！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
            }
            cameraid1[j] = cameraid;
            string isEncrypt = oCamera["isEncrypt"].ToString(); ;//取出该摄像头id
            statu = jsonHandle1(n);
            if (isEncrypt == "1")//视频已加密，需要调用萤石接口发送短信验证码
            {
                MessageBox.Show("该视频已加密，暂时不能播放！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //return;
            }
            else
            {
                if (c == 1)//单画面
                {
                    handle[0] = picbox[0].Handle;
                    if (a == 1)
                    {
                        bool close = HkAction.Stop(SessionId[j]);
                    }
                    Start_Play1();
                    j = 0;
                }
            }
        }
        #endregion

        #region
        public void Start_Play(int index)
        {
            JObject oCamera = getCamera(index);
            j=index;
            cameraid = oCamera["cameraId"].ToString(); ;//取出该摄像头id
            cameraid1[j] = cameraid;
            string isEncrypt = oCamera["isEncrypt"].ToString(); ;//取出该摄像头id
            if (isEncrypt == "1")//视频已加密，需要调用萤石接口发送短信验证码
            {
                MessageBox.Show("该视频已加密，暂时不能播放！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                handle[0] = picbox[0].Handle;
                if (a == 1)
                {
                    bool close = HkAction.Stop(SessionId[j]);
                }
                Start_Play1();
            }
        }
        #endregion

        #region 鼠标停在摄像头缩略图上显示播放图标
        private void picbox_MouseHover(object sender, EventArgs e)
        {
            PictureBox picb1 = sender as PictureBox;//取出点击的控件sender            
            if (picb1 != null)//点击则非空，否则为空
            {
                //picb1.Image = Image.FromFile("2.png");
                picb1.SizeMode = PictureBoxSizeMode.CenterImage;
            }
        }
        #endregion

        #region 鼠标离开时清除播放图片，重新显示缩略图
        private void picbox_MouseLeave(object sender, EventArgs e)
        {
            PictureBox picb1 = sender as PictureBox;//取出点击的控件sender            
            if (picb1 != null)//点击则非空，否则为空
            {
                picb1.Image = null;
            }
        }
        #endregion

        #region 鼠标滚动方法
        private void Form_MouseWheel(object sender, MouseEventArgs e)
        {
            //获取光标位置
            Point mousePoint = new Point(e.X, e.Y);
            //换算成相对本窗体的位置
            mousePoint.Offset(this.Location.X, this.Location.Y);
            //判断是否在flowLayoutPanel2内
            if (flowLayoutPanel1.RectangleToScreen(
              flowLayoutPanel1.DisplayRectangle).Contains(mousePoint))
            {
                //滚动
                flowLayoutPanel1.AutoScrollPosition = new Point(0, flowLayoutPanel1.VerticalScroll.Value - e.Delta);
            }
        }
        #endregion

        #region 鼠标滚动事件
        private void flowLayoutPanel2_MouseHover(object sender, EventArgs e)
        {
            this.MouseWheel += new MouseEventHandler(Form_MouseWheel);
        }
        #endregion

        private void btn_GetDevice_Click(object sender, EventArgs e)
        {
            getDeviceList();
        }

        private void FormCamera_FormClosing(object sender, FormClosingEventArgs e)
        {
            int a = 0;
            a++;
        }

        private int WM_SYSCOMMAND = 0x112;
        private long SC_MAXIMIZE = 0xF030;
        private long SC_MINIMIZE = 0xF020;
        private long SC_CLOSE = 0xF060;
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_SYSCOMMAND)
            {
                if (m.WParam.ToInt64() == SC_MAXIMIZE)
                {
                    //MessageBox.Show("MAXIMIZE ");  
                    return;
                }
                if (m.WParam.ToInt64() == SC_MINIMIZE)
                {
                    //MessageBox.Show("MINIMIZE ");  
                    return;
                }
                if (m.WParam.ToInt64() == SC_CLOSE)
                {
                    //MessageBox.Show("CLOSE ");  
                    this.Hide();
                    return;
                }
            }
            base.WndProc(ref m);
        }
    }
}
