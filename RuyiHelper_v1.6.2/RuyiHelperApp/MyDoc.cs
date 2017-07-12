using Microsoft.Office.Core;
using Microsoft.Office.Interop.Word;
using Microsoft.Win32;
using RueHelper.util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using OFFICECORE = Microsoft.Office.Core;
using POWERPOINT = Microsoft.Office.Interop.PowerPoint;
using PPT = Microsoft.Office.Interop.PowerPoint;
using Word = Microsoft.Office.Interop.Word; 
namespace RueHelper
{

    public class MyDoc
    {
        [DllImport("user32.dll", EntryPoint = "PostMessage")]
        public static extern int PostMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int SendMessage(IntPtr HWnd, uint Msg, int WParam, int LParam);

        [DllImport("User32.dll ", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);//关键方法  

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern int ShowWindow(IntPtr hwnd, int nCmdShow);//0 关闭窗口; 1 正常大小显示窗口; 2 最小化窗口; 3 最大化窗口

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int BringWindowToTop(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, IntPtr ProcessId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);  //导入为windows窗体设置焦点的方法

        [DllImport("user32.dll", CallingConvention=CallingConvention.Cdecl)]
        private static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);

        [DllImport("USER32.DLL")]
        public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);  //导入模拟键盘的方法

        [DllImport("USER32.DLL")]
        private static extern int mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        const int MOUSEEVENTF_MOVE = 0x1;
        const int MOUSEEVENTF_LEFTDOWN = 0x2;
        const int MOUSEEVENTF_LEFTUP = 0x4;
        const int MOUSEEVENTF_RIGHTDOWN = 0x8;
        const int MOUSEEVENTF_RIGHTUP = 0x10;
        const int MOUSEEVENTF_MIDDLEDOWN = 0x20;
        const int MOUSEEVENTF_MIDDLEUP = 0x40;
        const int MOUSEEVENTF_WHEEL = 0x800;
        const int MOUSEEVENTF_ABSOLUTE = 0x8000; 

        public const int WM_SYSCOMMAND = 0x112;
        public const int SC_MINIMIZE = 0xF020;
        public const int SC_MAXIMIZE = 0xF030;

        public const int SW_HIDE = 0;
        public const int SW_NORMAL = 1;
        public const int SW_ShowMaximized = 3;
        public const int SW_ShowNOACTIVATE = 4;
        public const int SW_Show = 5;
        public const int SW_Minimize = 6;
        public const int SW_ShowNA = 8;
        public const int SW_Restore = 9;
        public const int SW_ShowDEFAULT = 10;


        private static log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// /////////////////////////////////////////////////////////////////////

        Microsoft.Office.Interop.Word._Application wordApp = null; //Word应用程序变量
        Microsoft.Office.Interop.Word._Document wordDoc = null;    //Word文档变量

        public bool bPageTurning = true;
        public int pageTotal = 0;
        public int pageCurrent = 0;
        public int pageLast = 0;
        public string filename = "";
        public string filepath = "";
        public static Hashtable g_fileImgStatus = new Hashtable();//缩略图生成

        public object missing = System.Reflection.Missing.Value;

        public MyDoc()
        {

        }
        public bool Open(string path)
        {
            try
            {
                Util.ConvertWord2PDF(path, Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path) + ".pdf");
            }
            catch (Exception e)
            {

            }

            //if (Global.getPPTMaximize())
            //{
            minisizeProc();
            //}


            filepath = path;
            filename = Path.GetFileName(path);
            object FileName = filepath;
            object readOnly = true;


            wordApp = new Microsoft.Office.Interop.Word.Application(); //可以打开word程序 
            wordApp.DisplayAlerts = WdAlertLevel.wdAlertsNone;
            wordApp.Visible = true;


            
            try
            {
                wordDoc = wordApp.Documents.Open(ref FileName,
                                 ref missing, ref readOnly, ref missing, ref missing, ref missing,
                                 ref missing, ref missing, ref missing, ref missing, ref missing,
                                 ref missing, ref missing, ref missing, ref missing, ref missing);

                if (wordApp != null && wordDoc!=null)
                {
                    wordApp.ActiveWindow.View.Type = Microsoft.Office.Interop.Word.WdViewType.wdReadingView;
//Word.WdViewType.wdMasterView //大纲视图模式
//Word.WdViewType.wdNormalView //普通视图模式
//Word.WdViewType.wdOutlineView //和大纲视图模式类似
//Word.WdViewType.wdReadingView //阅读版式
//Word.WdViewType.wdPrintPreview //打印预览模式
//Word.WdViewType.wdPrintView //普通视图模式
//Word.WdViewType.wdWebView // 'Web版式视图

                    
                    maximizeWordProc();
                    Word.WdStatistic stat = Word.WdStatistic.wdStatisticPages;
                    int nPage = wordDoc.ComputeStatistics(stat, ref missing);
                    getPageInfo();
                    return true;
                }else{
                    throw new Exception("openDoc failed.");
                }
            }
            catch (Exception ex)
            {
                int errorcode = 0;
                var w32ex = ex as Win32Exception;
                if (w32ex == null)
                {
                    w32ex = ex.InnerException as Win32Exception;
                }
                if (w32ex != null)
                {
                    errorcode = w32ex.ErrorCode;
                }
                Log.Error("DocOpen exception. " + ex.Message + ", errorcode=" + errorcode);//Error HRESULT E_FAIL has been returned from a call to a COM component.

                Close();
            }
            finally
            {
            }
            return false;
        }
        public void Close()
        {
            try
            {
                if(wordDoc!=null)
                {
                    object saveOption = Word.WdSaveOptions.wdDoNotSaveChanges;
                    wordDoc.Close(ref saveOption, ref missing, ref missing);
                }
                if (wordApp != null)
                {
                    object saveOption = Word.WdSaveOptions.wdDoNotSaveChanges;
                    wordApp.Quit(ref saveOption, ref missing, ref missing);
                }
                if (wordApp != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(wordApp);
                    wordApp = null;
                }
            }
            catch (Exception e1)
            {
                Log.Error("DocOpen exception, and close exception. " + e1.Message);

            }
            finally
            {
                wordDoc = null;
                wordApp = null;
                closeProc(filename);
                GC.Collect();
            }
        }
        public void killWinWordProcess()
        {
            try
            {
                System.Diagnostics.Process[] processes = System.Diagnostics.Process.GetProcessesByName("WINWORD");
                foreach (System.Diagnostics.Process process in processes)
                {
                    bool b = process.MainWindowTitle == "";
                    if (process.MainWindowTitle == "")
                    {
                        process.Kill();
                    }

                }
            }
            catch (Exception ee)
            {

            }
        }

        public void getPageInfo()
        {
            if(wordApp!=null)
            {
                try
                {
                    //当前页数
                    object obj1 = wordApp.Selection.get_Information(WdInformation.wdActiveEndPageNumber);
                    //总页数
                    object obj2 = wordApp.Selection.get_Information(WdInformation.wdNumberOfPagesInDocument);

                    pageCurrent = Util.toInt(obj1.ToString());
                    pageTotal = Util.toInt(obj2.ToString());
                }
                catch (Exception e)
                {

                }
            }
        }



        #region - 页面设置 -
        //public void SetPage(Orientation orientation, double width, double height, double topMargin, double leftMargin, double rightMargin, double bottomMargin)
        //{
        //    wordDoc.PageSetup.PageWidth = wordApp.CentimetersToPoints((float)width);
        //    wordDoc.PageSetup.PageHeight = wordApp.CentimetersToPoints((float)height);
        //    if (orientation == Orientation.横板)
        //    {
        //        wordDoc.PageSetup.Orientation  = Microsoft.Office.Interop.Word.WdOrientation.wdOrientLandscape; 
        //    }
        //    wordDoc.PageSetup.TopMargin = (float)(topMargin * 25);//上边距
        //    wordDoc.PageSetup.LeftMargin = (float)(leftMargin * 25);//左边距
        //    wordDoc.PageSetup.RightMargin = (float)(rightMargin * 25);//右边距
        //    wordDoc.PageSetup.BottomMargin = (float)(bottomMargin * 25);//下边距
        //}
        
        //public void SetPage(Orientation orientation, double topMargin, double leftMargin, double rightMargin, double bottomMargin) 
        //{
        //    SetPage(orientation, 21, 29.7, topMargin, leftMargin, rightMargin, bottomMargin);
        //}
        //public void SetPage(double topMargin, double leftMargin, double rightMargin, double bottomMargin)
        //{
        //    SetPage(Orientation.竖板, 21, 29.7, topMargin, leftMargin, rightMargin, bottomMargin);
        //}
        #endregion


        #region - 关闭当前文档 -
        public bool CloseDocument()
        {
            try
            {
                object Nothing = System.Reflection.Missing.Value;
                object doNotSaveChanges = Word.WdSaveOptions.wdDoNotSaveChanges;
                wordDoc.Close(ref doNotSaveChanges, ref Nothing, ref Nothing);
                wordDoc = null; return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
        #region - 关闭程序 -
        public bool Quit()
        {
            try
            {
                object Nothing = System.Reflection.Missing.Value;
                object saveOption = Word.WdSaveOptions.wdDoNotSaveChanges;
                wordApp.Quit(ref saveOption, ref Nothing, ref Nothing);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion  - 关闭程序 -
        #region - 保存文档 -
        public bool Save(string savePath)
        {
            return Save(savePath, false);
        }
        public bool Save(string savePath, bool isClose)
        {
            try
            {
                object Nothing = System.Reflection.Missing.Value;

                object fileName = savePath;
                wordDoc.SaveAs(ref fileName, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing);
                if (isClose)
                {
                    return CloseDocument();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion  - 保存文档 -

        public void NextSlide()
        {
            string ret = "success";
            if (this.wordApp != null)
            {
                try
                {
                    maximizeWordProc();

                    keybd_event(0x22, 0, 0, 0);
                    keybd_event(0x22, 0, 2, 0);
                    getPageInfo();
                }
                catch (Exception ex)
                {
                    ret = "Doc播放已关闭";
                    Log.Error("Doc.Next() exception. " + ex.Message);
                    Close();
                }
            }
        }
        public void PreviousSlide()
        {
            if (this.wordApp != null)
            {
                try
                {
                    maximizeWordProc();

                    keybd_event(0x21, 0, 0, 0);
                    keybd_event(0x21, 0, 2, 0);
                    getPageInfo();
                }
                catch (Exception ex)
                {
                    Log.Error("Doc.Last() exception. " + ex.Message);
                    Close();
                }
            }
        }

        public void Up()
        {
            string ret = "success";
            if (this.wordApp != null)
            {
                try
                {
                    maximizeWordProc();

                    //keybd_event(0x22, 0, 0, 0);
                    //keybd_event(0x22, 0, 2, 0);

                    mouse_event(MOUSEEVENTF_WHEEL, 0, 0, 100, 0);

                    getPageInfo();
                }
                catch (Exception ex)
                {
                    ret = "Doc播放已关闭";
                    Log.Error("Doc.Next() exception. " + ex.Message);
                    Close();
                }
            }
        }
        public void Down()
        {
            if (this.wordApp != null)
            {
                try
                {
                    maximizeWordProc();

                    //keybd_event(0x21, 0, 0, 0);
                    //keybd_event(0x21, 0, 2, 0);

                    mouse_event(MOUSEEVENTF_WHEEL, 0, 0, -100, 0);
                    getPageInfo();
                }
                catch (Exception ex)
                {
                    Log.Error("Doc.Last() exception. " + ex.Message);
                    Close();
                }
            }
        }
        public void GotoPage(int page)
        {
            try
            {
                object Nothing = System.Reflection.Missing.Value;
                object What = Word.WdGoToItem.wdGoToPage;
                object Which = Word.WdGoToDirection.wdGoToNext;
                object Count = "" + page; // 页数
                wordDoc.ActiveWindow.Selection.GoTo(ref What, ref Which, ref Count, ref Nothing); // 第二个参数可以用Nothing
            }
            catch (Exception e)
            {
                Log.Error("GotoPage excepton: "+ e.Message);
            }
        }
        

        private void closeProc(string name)
        {
            foreach (Process thisProc in Process.GetProcesses())
            {
                if (thisProc.ProcessName == "WINWORD")
                {
                    thisProc.Kill();
                }
            }
        }

        public void minisizeProc()
        {
            //https://msdn.microsoft.com/en-us/library/dd375731(v=vs.85).aspx
            //VK_LWIN 0x5B
            //VK_RWIN 0x5C
            //D key 0x44
            //M key 0x4D
            keybd_event(0x5B, 0, 0, 0);
            keybd_event(0x4D, 0, 0, 0);
            keybd_event(0x4D, 0, 2, 0);
            keybd_event(0x5B, 0, 2, 0);

            //IntPtr hWnd = GetForegroundWindow(); //获取当前窗口句柄
            //Log.Info("minisizeProc , currentWindow=" + hWnd);
            //PostMessage(hWnd, WM_SYSCOMMAND, SC_MINIMIZE, 0);
            //ShowWindow(hWnd, 2);
        }

        public bool maximizeWordProc()
        {
            //-----------------------way 1---------------------------
            bool bFound = false;
            try
            {
                foreach (Process thisProc in Process.GetProcesses())
                {
                    if (thisProc.ProcessName == "WINWORD")
                    {
                        IntPtr hWnd1 = thisProc.MainWindowHandle;
                        IntPtr hWnd = FindWindow(null, thisProc.MainWindowTitle);//old
                        SendMessage(hWnd, WM_SYSCOMMAND, SC_MAXIMIZE, 0);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error("exception. " + e.Message);
            }

            //-----------------------way 2---------------------------
            IntPtr docHandle = FindWindow("OpusApp", null);
            if (docHandle == IntPtr.Zero)
            {
                MessageBox.Show("Word文档已关闭,请重新打开文档!", "Warning!!!");
                throw new Exception("WINWORD is not running");
            }
            else
            {
                IntPtr hForeWnd = GetForegroundWindow();
                if (docHandle != hForeWnd)
                {
                    uint ForeID = (uint)GetWindowThreadProcessId(hForeWnd, IntPtr.Zero);
                    uint CurID = (uint)GetWindowThreadProcessId(docHandle, IntPtr.Zero);
                    //IntPtr ForeID = GetWindowThreadProcessId(hForeWnd, IntPtr.Zero);
                    //IntPtr CurID = GetWindowThreadProcessId(docHandle, IntPtr.Zero);
                    if (ForeID != CurID)
                    {
                        BringWindowToTop(docHandle);
                        //AttachThreadInput(CurID, ForeID, true);
                        ShowWindow(docHandle, SW_ShowMaximized);
                        //SetWindowPos(hWnd, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE);
                        //SetWindowPos(hWnd, HWND_NOTOPMOST, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE);
                        bool bSet = SetForegroundWindow(docHandle);
                        //AttachThreadInput(CurID, ForeID, false);
                    }
                    else
                    {
                        ;//
                    }
                }
            }
            return bFound;
        }
    }
}


/*
 * 
axAcroPDF1.Focus();
mouse_event(MOUSEEVENTF_WHEEL, 0, 0, -100, 0);
1，因为要引用user32.dll,所以命名空间要加上：using System.Runtime.InteropServices;
2，mouse_event：
函数功能：该函数综合鼠标击键和鼠标动作。
函数原型：VOID mouse_event（DWORD dwFlags，DWORD dx，DWORD dwFlags，OWORD dx，DWORD dy， DWORD dwData， DWORD dwExtralnfo）；参数：
dwFlags：标志位集，指定点击按钮和鼠标动作的多种情况。此参数里的各位可以是下列值的任何合理组合：
MOOSEEVENTFMOVE：表明发生移动。
M00SEEVENTF_LEFTDOWN：表明接按下鼠标左键。
M00SEEVENTF_LEFTUP：表明松开鼠标左键。
MOOSEEVENTF_RIGHTDOWN：表明按下鼠标右键。
MOOSEEVENTF_RIGHTUP：表明松开鼠标右键。
MOOSEEVENTF_MIDDLEDOWN：表明按下鼠标中键。
MOOSEEVENTF_MIDDLEUP：表明松开鼠标中键。
MOOSEEVENTF_WHEEL：在Windows NT中如果鼠标有一个轮，表明鼠标轮被移动。移动的数量由dwData给出。
dx：指定鼠标沿x轴的绝对位置或者从上次鼠标事件产生以来移动的数量，依赖于MOOSEEVENTF_ABSOLOTE的设置。给出的绝对数据作为鼠标的实际X坐标；给出的相对数据作为移动的mickeys数。一个mickey表示鼠标移动的数量，表明鼠标已经移动。
dy：指定鼠标沿y轴的绝对位置或者从上次鼠标事件产生以来移动的数量，依赖于MOOSEEVENTF_ABSOLVTE的设置。给出的绝对数据作为鼠标的实际y坐标，给出的相对数据作为移动的mickeys数。
dwData：如果dwFlags为MOOSEEVENTF_WHEEL，则dwData指定鼠标轮移动的数量。正值表明鼠标轮向前转动，即远离用户的方向；负值表明鼠标轮向后转动，即朝向用户。一个轮击定义为WHEEL_DELTA，即120。
如果dwFlagsS不是MOOSEEVENTF_WHEEL，则dWData应为零。
dwExtralnfo：指定与鼠标事件相关的附加32位值。应用程序调用函数GetMessgeExtrajnfo来获得此附加信息。

        //const int MOUSEEVENTF_MOVE = 0x1;
        //const int MOUSEEVENTF_LEFTDOWN = 0x2;
        //const int MOUSEEVENTF_LEFTUP = 0x4;
        //const int MOUSEEVENTF_RIGHTDOWN = 0x8;
        //const int MOUSEEVENTF_RIGHTUP = 0x10;
        //const int MOUSEEVENTF_MIDDLEDOWN = 0x20;
        //const int MOUSEEVENTF_MIDDLEUP = 0x40;
        //const int MOUSEEVENTF_WHEEL = 0x800;
        //const int MOUSEEVENTF_ABSOLUTE = 0x8000;
 * 
 * 
 * 
*/