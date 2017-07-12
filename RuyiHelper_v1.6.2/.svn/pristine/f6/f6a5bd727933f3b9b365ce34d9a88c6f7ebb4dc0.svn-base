using RueHelper.model;
using RueHelper.util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Security;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Forms;

namespace RueHelper
{
    public class HTTPReq
    {
        private static log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly string DefaultUserAgent = "EduHttp";

        public static string HttpGet(string url, bool bWithMsgbox)
        {
            string ret = "";
            try
            {
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.Method = "GET";
                //设置代理UserAgent和超时
                //request.UserAgent = userAgent;
                request.Timeout = 2000;

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream receiveStream = response.GetResponseStream();
                Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
                // Pipes the stream to a higher level stream reader with the required encoding format. 
                StreamReader readStream = new StreamReader(receiveStream, encode);
                string resp = readStream.ReadToEnd();
                ret = resp;

                if (resp.StartsWith("CB"))
                    ret = resp.Substring(3, resp.Length - 4);
                return ret;
            }
            catch (Exception e)
            {
                Log.Error(url + " exception. " + e.Message);
                return "";
            }
            finally
            {
                if (bWithMsgbox)
                {
                    if (ret.Length > 0)
                        System.Windows.Forms.MessageBox.Show("请求" + url + "成功！\n" + ret, "采集器测试");
                    else
                        System.Windows.Forms.MessageBox.Show("请求" + url + "失败！\n请检查配置文件和网络状况！", "采集器测试");
                }
            }


        }

        public static string HttpGet(string Url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";

            //request.Timeout = 2000;
            //request.ReadWriteTimeout = 2000;

            string retString = "";
            Stream myResponseStream = null;
            StreamReader myStreamReader = null;
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                myResponseStream = response.GetResponseStream();
                myStreamReader = new StreamReader(myResponseStream, System.Text.UnicodeEncoding.UTF8);
                retString = myStreamReader.ReadToEnd();
            }
            catch (Exception ex)
            {
                Log.Error("HttpGet " + Url + ", exception: " + ex.Message);
            }
            finally
            {
                if (myStreamReader != null)
                {
                    myStreamReader.Close();
                    myResponseStream.Close();
                }
            }
            return uncode(retString);
        }

        public static string HttpGet(string Url, int timeout)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";

            request.Timeout = timeout;
            request.ReadWriteTimeout = timeout;

            string retString = "";
            Stream myResponseStream = null;
            StreamReader myStreamReader = null;
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                myResponseStream = response.GetResponseStream();
                myStreamReader = new StreamReader(myResponseStream, System.Text.UnicodeEncoding.UTF8);
                retString = myStreamReader.ReadToEnd();
            }
            catch (Exception ex)
            {
                Log.Error("HttpGet " + Url + ", exception: " + ex.Message);
            }
            finally
            {
                if (myStreamReader != null)
                {
                    myStreamReader.Close();
                    myResponseStream.Close();
                }
            }
            return uncode(retString);
        }

        public static string HttpPost(string Url, string postDataStr)
        {
            bool bNetOk = IsConnectInternet();
            System.Net.ServicePointManager.DefaultConnectionLimit = 50;
            int tmp = ServicePointManager.DefaultConnectionLimit;

            string retString = "";
            Stream myResponseStream = null;
            StreamReader myStreamReader = null;
            HttpWebResponse response = null;
            HttpWebRequest request = null;

            try
            {
                request = (HttpWebRequest)WebRequest.Create(Url);
                request.Method = "POST";
                //request.ContentType = "application/x-www-form-urlencoded";
                request.ContentType = "text/html;charset=UTF-8";
                request.Timeout = 2000;
                request.ReadWriteTimeout = 2000;
                request.KeepAlive = false;

                Encoding encoding = Encoding.UTF8;//根据网站的编码自定义  
                byte[] postData = encoding.GetBytes(postDataStr);//postDataStr即为发送的数据，格式还是和上次说的一样  
                request.ContentLength = postData.Length;
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(postData, 0, postData.Length);

                response = (HttpWebResponse)request.GetResponse();
                myResponseStream = response.GetResponseStream();
                myStreamReader = new StreamReader(myResponseStream, System.Text.UnicodeEncoding.UTF8);
                retString = myStreamReader.ReadToEnd();
            }
            catch (WebException ex)
            {
                try
                {
                    Log.Error(ex.Message);
                    HttpWebResponse response2 = (HttpWebResponse)ex.Response;
                    if (response2 != null)
                    {
                        Console.WriteLine("Error code: {0}", response2.StatusCode);
                        using (Stream data = response2.GetResponseStream())
                        {
                            using (StreamReader reader = new StreamReader(data))
                            {
                                Log.Error("URL=" + Url);
                                Log.Error("DATA=" + postDataStr);

                                string text = reader.ReadToEnd();
                                string textSimple = text;
                                string tag = "<b>exception</b>";
                                int pos = textSimple.IndexOf(tag);
                                if (pos > 0)
                                {
                                    textSimple = textSimple.Substring(pos + tag.Length);
                                }
                                Log.Error(textSimple);

                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Log.Error(e.Message);
                }
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
            }
            finally
            {
                System.GC.Collect();
                
                if (response != null)
                {
                    response.Close();
                    response = null;
                }
                if (myStreamReader != null)
                {
                    myStreamReader.Close();
                    myResponseStream.Close();
                }
            }
            return uncode(retString);
        }

        public static string doPost(string url, IDictionary<string, string> parameters, int? timeout, string userAgent, Encoding requestEncoding, CookieCollection cookies)
        {
            string retString = "";
            Stream myResponseStream = null;
            StreamReader myStreamReader = null;

            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }
            if (requestEncoding == null)
            {
                throw new ArgumentNullException("requestEncoding");
            }
            HttpWebRequest request = null;
            //如果是发送HTTPS请求  
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            if (!string.IsNullOrEmpty(userAgent))
            {
                request.UserAgent = userAgent;
            }
            else
            {
                request.UserAgent = DefaultUserAgent;
            }

            if (timeout.HasValue)
            {
                request.Timeout = timeout.Value;
            }
            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }
            //如果需要POST数据  
            if (!(parameters == null || parameters.Count == 0))
            {
                StringBuilder buffer = new StringBuilder();
                int i = 0;
                foreach (string key in parameters.Keys)
                {
                    if (i > 0)
                    {
                        buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                    }
                    else
                    {
                        buffer.AppendFormat("{0}={1}", key, parameters[key]);
                    }
                    i++;
                }
                byte[] data = requestEncoding.GetBytes(buffer.ToString());
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }


            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                string cookieString = response.Headers["Set-Cookie"];
                myResponseStream = response.GetResponseStream();
                myStreamReader = new StreamReader(myResponseStream, System.Text.UnicodeEncoding.UTF8);
                retString = myStreamReader.ReadToEnd();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            finally
            {
                if (myStreamReader != null)
                {
                    myStreamReader.Close();
                    myResponseStream.Close();
                }
            }
            return retString;
        }
        public static string doPost(string url, IDictionary<string, string> parameters)
        {

            ////IDictionary<string, string> parameters = new Dictionary<string, string>();
            ////parameters.Add("tpl", "fa");
            ////parameters.Add("tpl_reg", "fa");

            string retString = "";
            Stream myResponseStream = null;
            StreamReader myStreamReader = null;

            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }
            HttpWebRequest request = null;
            //如果是发送HTTPS请求  
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.UserAgent = DefaultUserAgent;
            request.Timeout = 10;
            //如果需要POST数据  
            if (!(parameters == null || parameters.Count == 0))
            {
                StringBuilder buffer = new StringBuilder();
                int i = 0;
                foreach (string key in parameters.Keys)
                {
                    if (i > 0)
                    {
                        buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                    }
                    else
                    {
                        buffer.AppendFormat("{0}={1}", key, parameters[key]);
                    }
                    i++;
                }

                byte[] data = System.Text.UnicodeEncoding.UTF8.GetBytes(buffer.ToString());
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }

            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                string cookieString = response.Headers["Set-Cookie"];
                myResponseStream = response.GetResponseStream();
                myStreamReader = new StreamReader(myResponseStream, System.Text.UnicodeEncoding.UTF8);
                retString = myStreamReader.ReadToEnd();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            finally
            {
                if (myStreamReader != null)
                {
                    myStreamReader.Close();
                    myResponseStream.Close();
                }
            }
            return retString;
        }
        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受  
        }
        public static string UploadFile(string Url, string path)
        {
            // 设置参数
            try
            {
                HttpWebRequest request = WebRequest.Create(Url) as HttpWebRequest;
                CookieContainer cookieContainer = new CookieContainer();
                request.CookieContainer = cookieContainer;
                request.AllowAutoRedirect = true;
                request.Method = "POST";
                string boundary = DateTime.Now.Ticks.ToString("X"); // 随机分隔线
                request.ContentType = "multipart/form-data;charset=utf-8;boundary=" + boundary;
                byte[] itemBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "\r\n");
                byte[] endBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");
                int pos = path.LastIndexOf("\\");
                string fileName = path.Substring(pos + 1);

                //请求头部信息 
                StringBuilder sbHeader = new StringBuilder(string.Format("Content-Disposition:form-data;name=\"file\";filename=\"{0}\"\r\nContent-Type:application/octet-stream\r\n\r\n", fileName));
                byte[] postHeaderBytes = Encoding.UTF8.GetBytes(sbHeader.ToString());
                FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                byte[] bArr = new byte[fs.Length];
                fs.Read(bArr, 0, bArr.Length);
                fs.Close();

                Stream postStream = request.GetRequestStream();
                postStream.Write(itemBoundaryBytes, 0, itemBoundaryBytes.Length);
                postStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);
                postStream.Write(bArr, 0, bArr.Length);
                postStream.Write(endBoundaryBytes, 0, endBoundaryBytes.Length);
                postStream.Close();

                //发送请求并获取相应回应数据
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                //直到request.GetResponse()程序才开始向目标网页发送Post请求
                Stream instream = response.GetResponseStream();
                StreamReader sr = new StreamReader(instream, Encoding.UTF8);

                //返回结果网页（html）代码
                string content = sr.ReadToEnd();
                Log.Info("upload ret: " + content);
                return content;
            }
            catch (Exception e)
            {
                OfflineProcessor.AddEvent("fileupload",Url,path,DateTime.Now.ToString("yyyyMMddHHmmss"));
                Log.Error(e.Message);
                return "";
            }
        }

        public static string uncode(string str)
        {
            string outStr = "";
            Regex reg = new Regex(@"(?i)\\u([0-9a-f]{4})");
            outStr = reg.Replace(str, delegate(Match m1)
            {
                return ((char)Convert.ToInt32(m1.Groups[1].Value, 16)).ToString();
            });
            return outStr;
        }

        public static string StringToUnicode(string s)
        {
            if (s != null)
            {
                char[] charbuffers = s.ToCharArray();
                byte[] buffer;
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < charbuffers.Length; i++)
                {
                    buffer = System.Text.Encoding.Unicode.GetBytes(charbuffers[i].ToString());
                    sb.Append(String.Format("//u{0:X2}{1:X2}", buffer[1], buffer[0]));
                }
                return sb.ToString();
            }
            else
            {
                return "";
            }
        }

        //##########################################################
        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(int Description, int ReservedValue);

        #region 检查网络连接是否可以正常,不保证连接互联网
        /// <summary>
        /// 用于检查网络是否可以连接互联网,true表示连接成功,false表示连接失败 
        /// </summary>
        /// <returns></returns>
        public static bool IsConnectInternet()
        {
            int Description = 0;
            return InternetGetConnectedState(Description, 0);
        }
        #endregion

        #region 检查网络连接是否可以正常,不保证连接互联网,通过ping方式
        /// <summary>
        /// 用于检查IP地址或域名是否可以使用TCP/IP协议访问(使用Ping命令),true表示Ping成功,false表示Ping失败 
        /// </summary>
        /// <param name="strIpOrDName">输入参数,表示IP地址或域名</param>
        /// <returns></returns>
        public static bool PingIpOrDomainName(string strIpOrDName)
        {
            try
            {
                Ping objPingSender = new Ping();
                PingOptions objPinOptions = new PingOptions();
                objPinOptions.DontFragment = true;
                string data = "";
                byte[] buffer = Encoding.UTF8.GetBytes(data);
                int intTimeout = 300;
                PingReply objPinReply = objPingSender.Send(strIpOrDName, intTimeout, buffer, objPinOptions);
                string strInfo = objPinReply.Status.ToString();
                if (strInfo == "Success")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }
        #endregion

        //#region 通过定时器判断是否超时
        //public static string doPost(string url, string data, int nSec)
        //{
        //    Timeout timeout = new Timeout();
        //    timeout.DoHttpPost = HttpPost;

        //    bool bret = timeout.DoHttpTimeout(url, data, new TimeSpan(0, 0, 0, nSec));
        //    return 
        //}
        //#endregion
    }
}
