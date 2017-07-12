using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RueHelper
{
    public class MovingDisk
    {
        public DriveInfo[] GetUSBDiskList()
        {
            return DriveInfo.GetDrives();
        }

        public int MessageResult(Message m)
        {
            int r = 0;
            try
            {
                if (m.Msg == Win32.WM_DEVICECHANGE)
                {
                    switch (m.WParam.ToInt32())
                    {
                        case Win32.WM_DEVICECHANGE:
                            break;
                        case Win32.DBT_DEVICEARRIVAL:
                            {
                                r = 1;
                            }
                            break;
                        case Win32.DBT_DEVICEREMOVECOMPLETE:
                            {
                                r = 2;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
            return r;
        }        
    }
}
