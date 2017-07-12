using RueHelper.util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace RueHelper
{
    /// <summary>
    /// U盘选择文件的窗口
    /// </summary>
    public partial class Form12 : Form
    {
        private static log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        private const int CS_DropSHADOW = 0x20000;
        private const int GCL_STYLE = (-26);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SetClassLong(IntPtr hwnd, int nIndex, int dwNewLong);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetClassLong(IntPtr hwnd, int nIndex);
        
        int screenWidth = Screen.PrimaryScreen.Bounds.Width;
        int screenHeight = Screen.PrimaryScreen.Bounds.Height;
        private Point mPoint = new Point();
        private Point btnPoint = new Point();
        private Point formPoint = new Point();
        ArrayList btnList = null;
        public ArrayList btnRightList = new ArrayList();
        bool bMouseDown;
        private bool _isFirstClick = true;
        private bool _isDoubleClick = false;
        private int _milliseconds = 0;
        private System.Windows.Forms.Timer _doubleClickTimer;
        private Rectangle _doubleRec;
        Button copyBtn;
        string _diskPath;
        string submitStr;
        string sourcePath;
        string sourceFilename;
        string currentdir = "";
        FormNotify fNotify;

        public static string g_filenames;
        public static int g_courseid = 0;
        public static Hashtable g_fileMap = new Hashtable();
        public static bool bExporting = false;
        public Form12(List<string> strList, bool bShow = false)
        {
            InitializeComponent();
            UpdateForm(strList, bShow);
        }
        public void showForm(object sender, EventArgs e)
        {
            fNotify = sender as FormNotify;
            fNotify.Show();
        }

        public void Clear()
        {
            //TODO: 拔掉U盘,需要清空U盘上的文件. 其它文件保留
            g_filenames = "";
            g_fileMap.Clear();
            this.Close();
        }
        public void removeUSBFile(List<string> strList)
        {
            List<string> namelist = new List<string>();
            for(int i=0; i<strList.Count; i++)
            {
                string str = strList[i];
                string driver = str.Substring(0, 2);
                foreach (DictionaryEntry de in g_fileMap)
                {
                    string filename = (string)de.Key;
                    string filepath = (string)de.Value;
                    if(filepath!=null && filepath.Length>0 && filepath.IndexOf(driver) > 0)
                    {
                        namelist.Add(filename);
                    }
                }
            }
            for (int i = 0; i < namelist.Count; i++)
            {
                string str = namelist[i];
                delButtonlist_Right(str);
            }
        }
        public void UpdateForm(List<string> strList,bool bShow=false)
        {
            if (this.btnList!=null)
                this.btnList.Clear();//防止左侧重复添加

            try
            {
                this.panel4.Visible = false;
                this.label6.Visible = false;
                //SetClassLong(this.Handle, GCL_STYLE, GetClassLong(this.Handle, GCL_STYLE) | CS_DropSHADOW);

                this.AllowDrop = true;
                //this.panel2.DragEnter += new DragEventHandler(panel2_DragEnter);//拖入边界
                //this.panel2.DragDrop += new DragEventHandler(panel2_DragDrop);//完成拖放
                this.panel2.BringToFront();
                this.panel3.BringToFront();
                formPoint.X = this.Location.X;
                formPoint.Y = this.Location.Y;
                _doubleClickTimer = new System.Windows.Forms.Timer();
                _doubleClickTimer.Interval = 100;
                _doubleClickTimer.Tick += new EventHandler(_doubleClickTimer_Tick);
                if (strList.Count > 0)
                {
                    _diskPath = strList[strList.Count - 1].ToString().Split('|')[0].ToString();
                    this.label7.Text = "当前设备：" + strList[strList.Count - 1].ToString().Split('|')[1].ToString() + "(" + _diskPath + ")";
                    ReadFilePath(_diskPath, 0, true);
                }
                else
                {
                    this.label7.Text = "";
                    this.panel4.Visible = false;
                    RefreshControls();
                }
                CreateDropList(strList);
            }
            catch (Exception ex)
            {
                string ss = ex.Message;
            }

            if (bShow)
            {
                this.Show();
                this.ShowInTaskbar = true;
                this.WindowState = FormWindowState.Normal;
                this.BringToFront();
            }
            else
            {
                this.ShowInTaskbar = false;
                this.WindowState = FormWindowState.Minimized;
                this.Hide();
            }
        }

        private void ReadFilePath(string diskPath, int field, bool bNotify=false)
        {
            currentdir = diskPath;
            if(diskPath.LastIndexOf("\\") == diskPath.Length-1)
            {
                diskPath = diskPath.Substring(0, diskPath.Length - 1);
            }
            ArrayList autoAddedFileList = new ArrayList();
            ArrayList al = Disk.MergrObj(diskPath.Replace("\r\n", ""));
            btnList = new ArrayList();
            if (field == 1)
            {
                Button backBtn = CreateBackBtn();
                btnList.Add(backBtn);
            }

            bool bAutoSync = false;
            string[] szType = {"ppt","pptx","doc","docx","pdf","mp3","wma","wmv","mp4","swf" };
            ArrayList filelist0 = Disk.getFilelist(diskPath.Replace("\r\n", ""), szType);
            if(filelist0.Count<=5)
            {
                bAutoSync = true;
            }
            else
            {
                Httpd.pushFilelistTips("U盘课件文件超过5个！\r\n请手动选择课件文件，并使用鼠标\"右键\"选择\"添加到如e小助手\"");
            }
            bAutoSync = false;

            for (int k = 0; k < al.Count; k++)
            {
                string value = al[k].ToString();
                Button btn = new Button();
                btn.Size = new System.Drawing.Size(70, 70);
                btn.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(153, 209, 255);
                btn.FlatAppearance.BorderSize = 0;
                btn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(204, 232, 255);
                btn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(229, 243, 255);
                btn.TextAlign = ContentAlignment.BottomCenter;
                btn.MouseMove += new System.Windows.Forms.MouseEventHandler(btn_MouseMove);
                btn.MouseDown += new System.Windows.Forms.MouseEventHandler(btn_MouseDown);
                btn.AllowDrop = true;
                if (value.Split('|')[0].ToString() == "0")
                {
                    btn.BackgroundImage = global::RueHelper.Properties.Resources.wenjianjia;
                    btn.Name = "0|" + diskPath + "|" + value.Split('|')[2].ToString();
                }
                else
                {
                    string filename = value.Split('|')[2].ToString();
                    string extName = filename.Substring(filename.LastIndexOf(".") + 1, (filename.Length - filename.LastIndexOf(".") - 1));
                    btn.BackgroundImage = FileICON(extName);//load
                    if(btn.BackgroundImage == null)
                    {
                        continue;
                    }
                    btn.Name = "1|" + diskPath + "|" + value.Split('|')[2].ToString();

                    if (filename.StartsWith("~$"))
                    {
                        continue;
                    }

                    if (field==0 && bAutoSync)
                    {
                        //仅仅在插上U盘时自动加载
                        string _filepath = diskPath + "\\" + filename;
                        autoAddedFileList.Add(_filepath);
                        continue;
                    }
                }
                this.label6.Text = diskPath + "\\" + value.Split('|')[2].ToString();
                btn.Text = value.Split('|')[1].ToString();
                string tooltipStr = value.Split('|')[2].ToString() + "\r\n类型：" + value.Split('|')[3].ToString() + "\r\n修改日期：" + value.Split('|')[5].ToString() + "\r\n大小：" + value.Split('|')[4].ToString() + "";
                this.toolTip1.SetToolTip(btn, tooltipStr);
                addButtonlist_Left(btn);
            }

            try
            {
                for (int i = 0; i < autoAddedFileList.Count; i++)
                {
                    string _path = (string)autoAddedFileList.ToArray()[i];
                    string _name = Path.GetFileName(_path);
                    EService.selectFile(_name, _path);
                    this.AddFile(_path, false);
                }

            }catch(Exception e1){
                ;//
            }

            Thread thread = new Thread(delegate()
            {
                if (autoAddedFileList.Count == 0)
                    return;

                bExporting = true;

                try
                {
                    string title = Global.getSchoolname() + " - " + Global.getClassname() + "";
                    if (bNotify)
                    {
                        if (fNotify==null)
                            fNotify = new FormNotify(title, "\r\n 课件导入中，请稍后！", 30);
                        this.Invoke(new System.EventHandler(this.showForm), new object[] { fNotify, null });
                        fNotify.InvokeUpdate(title, "\r\n 课件导入中，请稍后！");
                    }
                    
                    for (int i = 0; i < autoAddedFileList.Count; i++)
                    {
                        string filepathSrc = (string)autoAddedFileList.ToArray()[i];
                        Log.Info("copy ppt......0:" + filepathSrc);
                        string _fname = Path.GetFileName(filepathSrc);
                        if (_fname.IndexOf("ppt") > 0 || _fname.IndexOf("PPT") > 0)
                        {
                            Log.Info("copy ppt......3");
                            MyPPT.exportImg(filepathSrc);
                            Log.Info("copy ppt......4");
                        }
                        Log.Info("copy ppt......5");
                    }
                    bExporting = false;
                    if (bNotify && fNotify!=null)
                    {
                        fNotify.InvokeUpdate(title, "\r\n 课件导入完毕！");
                        Thread.Sleep(5000);
                        fNotify.InvokeClose();
                        fNotify = null;
                    }
                }catch(Exception e1){
                    Log.Error("Err!!! " + e1.Message);
                    bExporting = false;
                    try
                    {
                        if (fNotify != null)
                        {
                            fNotify.InvokeClose();
                            fNotify = null;
                        }
                    }
                    catch (Exception e2)
                    {
                        Log.Error("Err!!! " + e2.Message);
                    }
                }

            });
            thread.Start();

            RefreshControls();
        }

        private void Form12_Load(object sender, EventArgs e)
        {

        }

        private Button CreateBackBtn()
        {
            Button btn = new Button();
            btn.Size = new System.Drawing.Size(70, 70);
            btn.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(153, 209, 255);
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(204, 232, 255);
            btn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(229, 243, 255);
            btn.TextAlign = ContentAlignment.BottomCenter;
            btn.Click += new System.EventHandler(btn_Click);
            btn.AllowDrop = false;
            btn.BackgroundImage = global::RueHelper.Properties.Resources.back;
            btn.Name = "0|";
            btn.Text = "";
            return btn;
        }

        //返回上一级目录
        private void btn_Click(object sender, EventArgs e)
        {
            string path = currentdir;
            int pos = path.LastIndexOf("\\");
            if (pos > 0)
                path = path.Substring(0, pos);

            int field = 1;
            string disk = _diskPath.Substring(0, _diskPath.LastIndexOf("\\"));
            if (_diskPath == path || disk == path)
            {
                field = 0;
            }
            ReadFilePath(path, field);
        }

        private void _doubleClickTimer_Tick(object sender, EventArgs e)
        {
            _milliseconds += 100;
            if (_milliseconds >= SystemInformation.DoubleClickTime)
            {
                _doubleClickTimer.Stop();
                _isDoubleClick = false;
                _isFirstClick = true;
                _milliseconds = 0;
            }
        }

        /// <summary>
        /// 拖动到右侧，鼠标左键松开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panel2_DragEnter(object sender, DragEventArgs e)
        {
            Button btn = (Button)e.Data.GetData(typeof(Button));
            if (btn.Name.Split('|')[0].ToString() == "1")
            {
                e.Effect = DragDropEffects.Copy;
                bMouseDown = true;
            }
            else
            {
                e.Effect = DragDropEffects.None;//文件夹不能拖动
                //MessageBox.Show("不支持文件夹的拖动！","Warning!");
            }
        }

        /// <summary>
        /// 拖到右侧区域，而且鼠标松开后
        /// 左侧文件列表减少，右侧文件列表增加，同步到云端
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panel2_DragDrop(object sender, DragEventArgs e)
        {
            if (bMouseDown)
            {
                Button btn = (Button)e.Data.GetData(typeof(Button));
                btn.MouseDown -= new System.Windows.Forms.MouseEventHandler(btn_MouseDown);
                btn.MouseDown += new System.Windows.Forms.MouseEventHandler(btnRight_MouseDown);
                if (btnRightList.Count == 0)
                {
                    this.panel2.Controls.Remove(this.label4);
                }
                addButtonlist_Right(btn);//panel2_DragDrop, 从左忘右拖动
                RefreshControls(); //panel2_DragDrop
                bMouseDown = false;
                sourcePath = btn.Name.Split('|')[1].ToString();
                sourceFilename = btn.Name.Split('|')[2].ToString();

                System.Threading.Thread thred = new System.Threading.Thread(AsnycCopy);//拖动
                thred.Start();
            }
            else
            {
                Log.Info("bMouseDown=false, ");
            }
        }

        /// <summary>
        /// 异步拷贝到本地sourcefile目录下
        /// </summary>
        public void AsnycCopy()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();
            string hd = sourcePath.Substring(0, 1);
            string hdtype = "";
            foreach (DriveInfo dri in drives)
            {
                string _name = dri.Name.Substring(0, 1);
                if (_name == hd)
                {
                    hdtype = dri.DriveType.ToString();
                    break;
                }
            }
            string dir = Application.StartupPath + "\\sourcefile\\";
            string filename = sourceFilename;
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            string pathSrc = sourcePath + "\\" + sourceFilename;
            string ext = Path.GetExtension(pathSrc);

            //if (ext == ".ppt" || ext == ".pptx" || ext == ".doc" || ext == ".docx" || ext == ".jpg" || ext == ".jpeg" || ext == ".bmp" || ext == ".png" || ext == ".gif")
            //{
            //    FileOper.CopyFile(pathSrc, dir + filename, true);
            //}
            EService.selectFile(sourceFilename, pathSrc,false);

            if (filename.IndexOf("ppt") > 0 || filename.IndexOf("PPT") > 0)
            {
                bExporting = true;
                MyPPT.exportImg(sourcePath + "\\"+sourceFilename);


                bExporting = false;
            }
            


        }

        /// <summary>
        /// 异步同步文件列表到云端
        /// </summary>
        public void AsnycSubmit()
        {
            int classid = Global.getClassID();
            int schoolid = Global.getSchoolID();
            string apiurl = Global.url_assistant;
            string url_param = "action=assfilenamebyclassroomid.set&filenames=" + submitStr + "&classid=" + classid + "&schoolid=" + schoolid + "";
            HTTPReq.HttpGet(apiurl + url_param);
            Log.Info(apiurl +  url_param);
        }

        /// <summary>
        /// 刷新控件
        /// </summary>
        private void RefreshControls()
        {
            submitStr = "";
            this.panel2.Controls.Clear();
            this.panel3.Controls.Clear();
            int i = 0;
            int x = 8, y = 29;
            if (btnList == null)
                btnList = new ArrayList();
            for (int k = 0; k < btnList.Count; k++)//
            {
                Button btn = (Button)btnList[k];
                btn.FlatAppearance.BorderSize = 0;
                btn.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
                btn.Location = new System.Drawing.Point(x, y);
                x += btn.Width + 8;
                if ((i + 1) % 6 == 0)
                {
                    x = 8;
                    y += btn.Height + 29;
                }
                i++;
                this.panel3.Controls.Add(btn);
            }
            int j = 0;
            int x1 = 32, y1 = 20;
            for (int k = 0; k < btnRightList.Count; k++)
            {
                Button btn = (Button)btnRightList[k];
                btn.FlatAppearance.BorderSize = 0;
                btn.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
                btn.Location = new System.Drawing.Point(x1, y1);
                x1 += btn.Width + 32;
                if ((j + 1) % 3 == 0)
                {
                    x1 = 32;
                    y1 += btn.Height + 20;
                }
                j++;
                this.panel2.Controls.Add(btn);
                if (k == 0)
                {
                    submitStr += btn.Name.Split('|')[2].ToString();
                }
                else
                {
                    submitStr += "|" + btn.Name.Split('|')[2].ToString();
                }
            }

            //同步到云端
            g_filenames = submitStr;

            //同步到云端
            System.Threading.Thread thred = new System.Threading.Thread(AsnycSubmit);
            thred.Start();
        }

        /// <summary>
        /// 刷新控件
        /// </summary>
        public void AddFile(string filepath, bool bCopy)
        {
            string fname = Path.GetFileName(filepath);
            string extName = Path.GetExtension(filepath).Substring(1);
            string dir = Path.GetDirectoryName(filepath);

            Button btn = new Button();
            btn.Size = new System.Drawing.Size(70, 70);
            btn.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(153, 209, 255);
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(204, 232, 255);
            btn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(229, 243, 255);
            btn.TextAlign = ContentAlignment.BottomCenter;
            btn.MouseMove += new System.Windows.Forms.MouseEventHandler(btn_MouseMove);
            btn.MouseDown += new System.Windows.Forms.MouseEventHandler(btn_MouseDown);
            btn.AllowDrop = true;
            btn.BackgroundImage = FileICON(extName);
            btn.Name = "1|" + dir + "|" + fname;
            btn.Text = JsonOper.GetString(fname, 6, "..");
            FileInfo fileInfo = new System.IO.FileInfo(filepath);
            string tooltipStr = fname + "\r\n类型：" + Disk.FileType(extName) + "\r\n修改日期：" + fileInfo.LastWriteTime.ToString("yyyy-MM-dd HH:mm") + "\r\n大小：" + fileInfo.Length + "字节";
            this.toolTip1.SetToolTip(btn, tooltipStr);
            btn.MouseDown -= new System.Windows.Forms.MouseEventHandler(btn_MouseDown);
            btn.MouseDown += new System.Windows.Forms.MouseEventHandler(btnRight_MouseDown);
            addButtonlist_Right(btn);//添加本地文件
            RefreshControls();

            sourcePath = btn.Name.Split('|')[1].ToString();
            sourceFilename = btn.Name.Split('|')[2].ToString();
            if(bCopy)
            {
                System.Threading.Thread thred = new System.Threading.Thread(AsnycCopy);//拖动
                thred.Start();
            }
        }
        public void DeleteFile(string filename)
        {
            if(filename=="*")
            {
                this.emptyButtonlist_Right();
            }
            else
            {
                this.delButtonlist_Right(filename);
            }
            
            RefreshControls();
        }

        /// <summary>
        /// 鼠标移动动作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point position = e.Location;
                if (Math.Abs(position.X - btnPoint.X) > System.Windows.SystemParameters.MinimumHorizontalDragDistance ||
                    Math.Abs(position.Y - btnPoint.Y) > System.Windows.SystemParameters.MinimumVerticalDragDistance)
                {
                    StartDrag(e, (Button)sender);
                }
            }
        }
        DragDropEffects _resultEffects = DragDropEffects.None;
        
        /// <summary>
        /// 开始拖动文件图标按钮
        /// </summary>
        /// <param name="args"></param>
        /// <param name="btn"></param>
        void StartDrag(MouseEventArgs args,Button btn)
        {
            btn.QueryContinueDrag += new QueryContinueDragEventHandler(onQueryContinueDrag);
            btn.GiveFeedback += new GiveFeedbackEventHandler(onGiveFeedback);
            copyBtn = new Button();
            copyBtn.Size = btn.Size;
            copyBtn.BackColor = btn.BackColor;
            copyBtn.FlatStyle = btn.FlatStyle;
            copyBtn.FlatAppearance.BorderColor = btn.FlatAppearance.BorderColor;
            copyBtn.FlatAppearance.BorderSize = btn.FlatAppearance.BorderSize;
            copyBtn.FlatAppearance.MouseDownBackColor = btn.FlatAppearance.MouseDownBackColor;
            copyBtn.FlatAppearance.MouseOverBackColor = btn.FlatAppearance.MouseDownBackColor;
            copyBtn.TextAlign = btn.TextAlign;
            copyBtn.Text = btn.Text;
            copyBtn.Name = btn.Name;
            copyBtn.BackgroundImage = btn.BackgroundImage;
            copyBtn.AllowDrop = btn.AllowDrop;
            this.Controls.Add(copyBtn);
            copyBtn.BringToFront();
            btn.DoDragDrop(btn, DragDropEffects.Copy | DragDropEffects.Move);
            copyBtn.Visible = false;
            copyBtn = null;
        }

        void onQueryContinueDrag(object sender, QueryContinueDragEventArgs args)
        {
            if (copyBtn.Name.Split('|')[0].ToString() == "1")
            {
                copyBtn.Location = new System.Drawing.Point(Control.MousePosition.X - formPoint.X + 1, Control.MousePosition.Y - formPoint.Y + 1);
            }
            else
            {
                this.Controls.Remove(copyBtn);
            }
        }
        void onGiveFeedback(object sender, GiveFeedbackEventArgs args)
        {
            args.UseDefaultCursors = true;
        }

        /// <summary>
        /// 右侧区域 左键单击取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRight_MouseDown(object sender, MouseEventArgs e)
        {
            Button btn = (Button)sender;
            for (int i = 0; i < btnRightList.Count; i++)
            {
                Button lBtn = (Button)btnRightList[i];
                lBtn.FlatAppearance.BorderSize = 0;
                lBtn.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            }
            btn.MouseDown -= new System.Windows.Forms.MouseEventHandler(btnRight_MouseDown);
            btn.MouseDown += new System.Windows.Forms.MouseEventHandler(btn_MouseDown);

            string[] szItem = btn.Name.Split('|');
            if(szItem.Length==3)
            {
                string filename = szItem[2];
                string dir = szItem[1];
                if (g_fileMap.Contains(filename))
                {
                    g_fileMap.Remove(filename);//双击删除
                }

                if (dir == currentdir || dir==currentdir.Replace("\\",""))
                {
                    btnList.Add(btn);
                }
            }
            
            btnRightList.Remove(btn);
            
            RefreshControls();//btnRight_MouseDown
        }

        /// <summary>
        /// 双击左侧的文件夹
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_MouseDown(object sender, MouseEventArgs e)
        {
            Button btn = (Button)sender;
            for (int i = 0; i < btnList.Count; i++)
            {
                Button lBtn = (Button)btnList[i];
                lBtn.FlatAppearance.BorderSize = 0;
                lBtn.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            }
            btn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(153, 209, 255);
            btn.FlatAppearance.BorderSize = 1;
            btn.BackColor = System.Drawing.Color.FromArgb(204, 232, 255);
            if (_isFirstClick)
            {
                _doubleRec = new Rectangle(e.X - SystemInformation.DoubleClickSize.Width / 2,
                    e.Y - SystemInformation.DoubleClickSize.Height / 2,
                    SystemInformation.DoubleClickSize.Width,
                    SystemInformation.DoubleClickSize.Height);
                _isFirstClick = false;
                _doubleClickTimer.Start();
            }
            else
            {
                if (_doubleRec.Contains(e.Location) && _milliseconds < SystemInformation.DoubleClickTime)
                {
                    Button btn1 = (Button)sender;
                    if (btn1.Name.Split('|')[0].ToString() == "0")
                    {
                        string diskPath = btn1.Name.Split('|')[1].ToString() + "\\" + btn1.Name.Split('|')[2].ToString();
                        ReadFilePath(diskPath, 1);
                    }
                    else
                    {
                        //双击选择文件
                        btn.MouseDown -= new System.Windows.Forms.MouseEventHandler(btn_MouseDown);
                        btn.MouseDown += new System.Windows.Forms.MouseEventHandler(btnRight_MouseDown);
                        if (btnRightList.Count == 0)
                        {
                            this.panel2.Controls.Remove(this.label4);
                        }
                        addButtonlist_Right(btn);//双击选择文件

                        sourcePath = btn.Name.Split('|')[1].ToString();
                        sourceFilename = btn.Name.Split('|')[2].ToString();

                        RefreshControls();//double click

                        System.Threading.Thread thred = new System.Threading.Thread(AsnycCopy);//双击
                        thred.Start();
                    }
                    _isDoubleClick = true;
                }
            }
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            mPoint.X = e.X;
            mPoint.Y = e.Y;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point myPosittion = MousePosition;
                myPosittion.Offset(-mPoint.X, -mPoint.Y);
                Location = myPosittion;
                formPoint.X = this.Location.X;
                formPoint.Y = this.Location.Y;
            }
        }

        private Bitmap FileICON(string extName)
        {
            extName = extName.ToLower();
            Bitmap ico = null;// global::RueHelper.Properties.Resources._null;
            if (extName == "png" || extName == "jpg" || extName == "jpeg" || extName == "gif" || extName == "bmp")
            {
                ico = global::RueHelper.Properties.Resources.img;
            }
            if (extName == "ppt" || extName == "pptx")
            {
                ico = global::RueHelper.Properties.Resources.ppt;
            }
            if (extName == "pdf")
            {
                ico = global::RueHelper.Properties.Resources.pdf;
            }
            if (extName == "doc" || extName == "docx")
            {
                ico = global::RueHelper.Properties.Resources.word;
            }
            //if (extName == "xls" || extName == "xlsx")
            //{
            //    ico = global::RueHelper.Properties.Resources.excel;
            //}

            if (extName == "mp3" || extName == "wma" || extName == "wav")
            {
                ico = global::RueHelper.Properties.Resources.audio;
            }
            else if (extName == "mp4"  || extName == "mpg" || extName == "mov" || extName == "mpeg" || extName == "wmv")
            {
                ico = global::RueHelper.Properties.Resources.video;
            }
            else if (extName == "swf" || extName == "flv")
            {
                ico = global::RueHelper.Properties.Resources.video;
            }
            return ico;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
            this.Hide();
        }

        private void label7_MouseHover_1(object sender, EventArgs e)
        {
            Label lb = (Label)sender;
            lb.BackColor = System.Drawing.Color.FromArgb(99, 183, 111);
            lb.ForeColor = System.Drawing.Color.White;
        }

        private void label7_MouseLeave_1(object sender, EventArgs e)
        {
            Label lb = (Label)sender;
            lb.BackColor = System.Drawing.Color.White;
            lb.ForeColor = System.Drawing.Color.FromArgb(102, 102, 102);
        }

        private void label7_MouseClick(object sender, MouseEventArgs e)
        {
            Label lb = (Label)sender;
            if (this.panel4.Visible == false)
            {
                lb.MouseLeave -= new System.EventHandler(label7_MouseLeave_1);
                this.panel4.Visible = true;
            }
            else
            {
                lb.MouseLeave += new System.EventHandler(label7_MouseLeave_1);
                this.panel4.Visible = false;
            }
        }

        private void CreateDropList(List<string> strList)
        {
            this.panel4.Location = new System.Drawing.Point(21, 96 + this.label7.Height);
            this.panel4.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);
            this.panel4.Width = this.label7.Width;
            this.panel4.BringToFront();
            this.panel4.Controls.Clear();
            this.panel4.Height = strList.Count * 40;
            for (int i = 0; i < strList.Count; i++)
            {
                Button btn = new Button();
                btn.Size = new System.Drawing.Size(this.label7.Width - 8, 36);
                btn.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
                btn.FlatStyle = FlatStyle.Flat;
                btn.Cursor = Cursors.Hand;
                btn.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                btn.ForeColor = System.Drawing.Color.FromArgb(57, 123, 67);
                btn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(99, 183, 111);
                btn.FlatAppearance.BorderSize = 1;
                btn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(204, 232, 255);
                btn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(216, 237, 219);
                btn.TextAlign = ContentAlignment.MiddleCenter;
                btn.Text = strList[i].ToString().Split('|')[1] + "(" + strList[i].ToString().Split('|')[0] + ")";
                btn.Name = strList[i].ToString().Split('|')[0];
                btn.Click += new System.EventHandler(diskBtn_Click);
                btn.Location = new System.Drawing.Point(4, (btn.Height * i) + ((i + 1) * 3));
                this.panel4.Controls.Add(btn);
            }
            this.Show();
            this.BringToFront();
        }

        private void diskBtn_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            ReadFilePath(btn.Name, 0);
            this.label7.Text = btn.Text;
            this.panel4.Visible = false;
            this.label7.MouseLeave += new System.EventHandler(label7_MouseLeave_1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string path = "";
            string fname = "";
            string extname = "";
            string ftype = "";
            string dir = "";
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "请选择教学资源文件";
            //op.Filter = "All Files(*.*)|*.*|ppt Files(*.ppt)|*.ppt|word 2007 Files(*.doc)|*.doc|excel 2007 Files(*.xls)|*.xls|word Files(*.docx)|*.docx|excel Files(*.xlsx)|*.xlsx";
            op.Filter = "课件文件(*.ppt,*.pptx,*.doc,*.docx,*.jpg,*.jpeg,*.bmp,*.png,*.gif,*.mp3,*.mp4,*.wav,*.wma,*.wmv,*.mov,*.swf)|*.ppt;*.pptx;*.doc;*.docx;*.jpg;*.jpeg;*.bmp;*.png;*.gif;*.mp3;*.wav;*.wma;*.mp4;*.wmv;*.mov;*.swf";
            //op.Filter = "课件文件(*.ppt,*.pptx,*.doc,*.docx,*.pdf,*.jpg,*.jpeg,*.bmp,*.png,*.gif,*.mp3,*.mp4,*.wav,*.wma,*.wmv,*.mov,*.swf)|*.ppt;*.pptx;*.doc;*.docx;*.pdf;*.jpg;*.jpeg;*.bmp;*.png;*.gif;*.mp3;*.wav;*.wma;*.mp4;*.wmv;*.mov;*.swf";

            if (op.ShowDialog() == DialogResult.OK)
            {
                path = op.FileName;
                dir = path.Substring(0, path.LastIndexOf("\\"));
                fname = op.SafeFileName;
                extname = fname.Substring(fname.LastIndexOf(".") + 1, (fname.Length - fname.LastIndexOf(".") - 1)); ;
            }
            if (path != "")
            {
                Button btn = new Button();
                btn.Size = new System.Drawing.Size(70, 70);
                btn.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(153, 209, 255);
                btn.FlatAppearance.BorderSize = 0;
                btn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(204, 232, 255);
                btn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(229, 243, 255);
                btn.TextAlign = ContentAlignment.BottomCenter;
                btn.MouseMove += new System.Windows.Forms.MouseEventHandler(btn_MouseMove);
                btn.MouseDown += new System.Windows.Forms.MouseEventHandler(btn_MouseDown);
                btn.AllowDrop = true;
                string filename = fname;
                string extName = extname;
                btn.BackgroundImage = FileICON(extName);
                btn.Name = "1|" + dir + "|" + fname;
                btn.Text = JsonOper.GetString(fname, 6, "..");
                FileInfo fileInfo = new System.IO.FileInfo(path);
                string tooltipStr = fname + "\r\n类型：" + Disk.FileType(extName) + "\r\n修改日期：" + fileInfo.LastWriteTime.ToString("yyyy-MM-dd HH:mm") + "\r\n大小：" + fileInfo.Length + "字节";
                this.toolTip1.SetToolTip(btn, tooltipStr);
                btn.MouseDown -= new System.Windows.Forms.MouseEventHandler(btn_MouseDown);
                btn.MouseDown += new System.Windows.Forms.MouseEventHandler(btnRight_MouseDown);
                addButtonlist_Right(btn);//添加本地文件
                RefreshControls();//button3_Click

                sourcePath = btn.Name.Split('|')[1].ToString();
                sourceFilename = btn.Name.Split('|')[2].ToString();
                System.Threading.Thread thred = new System.Threading.Thread(AsnycCopy);//拖动
                thred.Start();
            }
        }

        /// <summary>
        /// 添加右侧文件button，如果右侧已存在，则不添加
        /// </summary>
        /// <param name="btn"></param>
        private void addButtonlist_Right(Button btn)
        {
            
            bool bRightFound = false;
            foreach(Button _btn in btnRightList)
            {
                if(_btn.Name == btn.Name)
                {
                    bRightFound = true;
                    continue;
                }
            }
            if(!bRightFound)
            {
                btnRightList.Add(btn);
                if (btnList!=null)
                    btnList.Remove(btn);

                string[] szItem = btn.Name.Split('|');
                if (szItem.Length == 3)
                {
                    string filename = szItem[2];
                    if (!g_fileMap.Contains(filename))
                    {
                        g_fileMap.Add(filename, btn.Name);//双击删除
                    }
                }
            }
                
        }

        /// <summary>
        /// 添加右侧文件button，如果右侧已存在，则不添加
        /// </summary>
        /// <param name="btn"></param>
        private void delButtonlist_Right(string name)
        {
            if(g_fileMap.Contains(name))
            {
                string filepath = (string)g_fileMap[name];
                g_fileMap.Remove(name);

                bool bRightFound = false;
                foreach (Button _btn in btnRightList)
                {
                    if (_btn.Name == filepath)
                    {
                        btnRightList.Remove(_btn);
                        break;
                    }
                }
            }
        }
        private void emptyButtonlist_Right()
        {
            g_fileMap.Clear();
            btnRightList.Clear();
            g_filenames = "";
        }
        /// <summary>
        /// 添加左侧文件button
        /// </summary>
        /// <param name="btn"></param>
        private void addButtonlist_Left(Button btn)
        {
            foreach (Button _btn in btnRightList)
            {
                string _name = _btn.Name.Substring(_btn.Name.LastIndexOf('|'));
                string name = btn.Name.Substring(btn.Name.LastIndexOf('|'));
                if (_name == name)
                {
                    return;
                }
            }
            btnList.Add(btn);
        }

        private void OnCloseMouseHover(object sender, EventArgs e)
        {
            this.button2.Image = global::RueHelper.Properties.Resources.closeB;
        }

        private void OnCloseMouseLeave(object sender, EventArgs e)
        {
            this.button2.Image = global::RueHelper.Properties.Resources.closeA;
        }

        private void OnMinimizeMouseLeave(object sender, EventArgs e)
        {
            this.button1.Image = global::RueHelper.Properties.Resources.minA;
        }

        private void OnMinimizeMouseHover(object sender, EventArgs e)
        {
            this.button1.Image = global::RueHelper.Properties.Resources.minB;
        }

    }
}
