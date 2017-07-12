using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace RueHelper
{
    public class Asynchronous
    {
        private string url;
        private string data;
        private string courseid;
        private string classid;
        private string lessonid;

        public Asynchronous(string url, string data, string courseid, string classid, string lessonid)
        {
            this.url = url;
            this.data = data;
            this.courseid = courseid;
            this.classid = classid;
            this.lessonid = lessonid;
        }

        public void ThreadGo()
        {
            HTTPReq.HttpGet(url, "action=handon.upload&lessonid=" + lessonid + "&courseid=" + courseid + "&classid=" + classid + "&result=" + data + "");  
        }
    }
}
