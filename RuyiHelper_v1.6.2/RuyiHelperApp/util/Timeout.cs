using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace RueHelper.util
{
    public delegate string DoHttpPostHandler(string Url, string postDataStr);
    public delegate string DoPostHandler(string action, string data); 
    class Timeout
    {
        private ManualResetEvent mTimeoutObject;
        //标记变量  
        private bool mBoTimeout;

        public DoHttpPostHandler DoHttpPost;
        public DoPostHandler DoPost;
        public string resp;
        public Timeout()
        {
            //  初始状态为 停止  
            this.mTimeoutObject = new ManualResetEvent(true);
        }
        ///<summary>  
        /// 指定超时时间 异步执行某个方法  
        ///</summary>  
        ///<returns>执行 是否超时</returns>  
        public bool DoHttpTimeout(string url,string data,TimeSpan timeSpan)
        {
            if (this.DoHttpPost == null)
            {
                return false;
            }
            this.mTimeoutObject.Reset();
            this.mBoTimeout = true; //标记  
            this.DoHttpPost.BeginInvoke(url, data, DoAsyncCallBack, null);
            // 等待 信号Set  
            if (!this.mTimeoutObject.WaitOne(timeSpan, false))
            {
                this.mBoTimeout = true;
            }
            return this.mBoTimeout;
        }
        public bool DoPostTimeout(string action, string data, TimeSpan timeSpan)
        {
            if (this.DoPost == null)
            {
                return false;
            }
            this.mTimeoutObject.Reset();
            this.mBoTimeout = true; //标记  
            this.DoPost.BeginInvoke(action, data, DoPostCallBack, null);
            // 等待 信号Set  
            if (!this.mTimeoutObject.WaitOne(timeSpan, false))
            {
                this.mBoTimeout = true;
            }
            return this.mBoTimeout;
        }
        ///<summary>  
        /// 异步委托 回调函数  
        ///</summary>  
        ///<param name="result"></param>  
        private void DoAsyncCallBack(IAsyncResult result)
        {
            try
            {
                string resp = this.DoHttpPost.EndInvoke(result);
                // 指示方法的执行未超时  
                this.mBoTimeout = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                this.mBoTimeout = true;
            }
            finally
            {
                this.mTimeoutObject.Set();
            }
        }

        ///<summary>  
        /// 异步委托 回调函数  
        ///</summary>  
        ///<param name="result"></param>  
        private void DoPostCallBack(IAsyncResult result)
        {
            try
            {
                resp = this.DoPost.EndInvoke(result); 
                this.mBoTimeout = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                this.mBoTimeout = true;
            }
            finally
            {
                this.mTimeoutObject.Set();
            }
        }  
    }
}
