using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SpiderConsole
{
    public class HttpPostHelper
    {

        public static string HttpPost(string Url, string postDataStr)
        {
            //创建httpWebRequest对象
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            #region 填充httpWebRequest的基本信息

            request.Method = "POST";
            Encoding ec = Encoding.GetEncoding("utf-8");
            request.ContentLength = ec.GetByteCount(postDataStr);
            request.ContentType = "application/x-www-form-urlencoded;charset=utf8";
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36";
            #endregion

            #region 填充要post的内容httpRequest.ContentLength = data.Length;
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(ec.GetBytes(postDataStr), 0, ec.GetBytes(postDataStr).Length);
            requestStream.Close();
            #endregion

            #region 发送post请求到服务器并读取服务器返回信息
            Stream responseStream;
            try
            {
                responseStream = request.GetResponse().GetResponseStream();
            }
            catch (Exception e)
            {
                Console.WriteLine(
                    string.Format("POST操作发生异常：{0}", e.Message)
                    );
                throw e;

            }

            #endregion
            #region 读取服务器返回信息
            string stringResponse = string.Empty;
            using (StreamReader responseReader = new StreamReader(responseStream, Encoding.UTF8))
            {
                stringResponse = responseReader.ReadToEnd();
            }
            responseStream.Close();
            #endregion
            return stringResponse;

        }
    }
}
