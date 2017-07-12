using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Forms;

namespace RueHelper.util
{
    class MyMouse
    {
        #region Win API

        [Flags]
        enum MouseEventFlag : uint
        {
            Move = 0x0001,
            LeftDown = 0x0002,
            LeftUp = 0x0004,
            RightDown = 0x0008,
            RightUp = 0x0010,
            MiddleDown = 0x0020,
            MiddleUp = 0x0040,
            XDown = 0x0080,
            XUp = 0x0100,
            Wheel = 0x0800,
            VirtualDesk = 0x4000,
            Absolute = 0x8000
        }
        /// <summary>
        /// 功能比较全面的鼠标功能API
        /// </summary>
        /// <param name="flags"></param>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        /// <param name="data"></param>
        /// <param name="extraInfo"></param>
        [DllImport("user32.dll")]
        static extern void mouse_event(MouseEventFlag flags, int dx, int dy, uint data, UIntPtr extraInfo);

        #endregion

        #region 配置
        static class MouseConfig
        {
            //鼠标指针每秒移动多少像素点
            public static int MovelForSecond = 100;
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 移动到指定的屏幕坐标（带轨迹）
        /// </summary>
        /// <param name="targetX">X坐标</param>
        /// <param name="targetY">Y坐标</param>
        public static void Move(int targetX, int targetY)
        {
            Thread th = new Thread(() =>
            {
                int count = MouseConfig.MovelForSecond;
                while (count != 0)
                {
                    Thread.Sleep(10);
                    int stepx = (targetX - Cursor.Position.X) / count;
                    int stepy = (targetY - Cursor.Position.Y) / count;
                    count--;
                    if (count != 0)
                        mouse_event(MouseEventFlag.Move, stepx, stepy, 0, UIntPtr.Zero);
                }

            });
            th.IsBackground = true;
            th.Start();
        }
        #endregion

        #region 公开方法
        /// <summary>
        /// 移动到指定的元素
        /// </summary>
        /// <param name="element">元素</param>
        public static void Move(UIElement element)
        {
            //获取该控件在屏幕上的坐标
            Point coordinate = element.PointToScreen(new Point());
            //将鼠标位置定到控件的中心位置
            coordinate = new Point(coordinate.X + element.RenderSize.Width / 2, coordinate.Y + element.RenderSize.Height / 2);
            Move((int)coordinate.X, (int)coordinate.Y);
        }
        #endregion
    }
}
