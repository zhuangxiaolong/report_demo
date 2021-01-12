using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Domain.Core.Http;

namespace Domain.Core.APIClient
{
    public class RestfulClient
    {
        /// <summary>
        /// 发送Post请求
        /// UTF-8
        /// 10000毫秒超时
        /// </summary>
        /// <param name="postUrl">URL</param>
        /// <param name="postStr">数据</param>
        /// <param name="contentType">contentType</param>
        /// <param name="timeOut">超时时间，默认10000ms</param>
        /// <returns></returns>
        public string DoPostRequest(string postUrl, string postStr, string contentType, int timeOut = 10000)
        {
            string strResult = string.Empty;
            HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(postUrl);
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentType = contentType;
            httpWebRequest.KeepAlive = true;
            httpWebRequest.Timeout = timeOut; //10000毫秒超时
            //添加参数
            byte[] postData = Encoding.UTF8.GetBytes(postStr);
            httpWebRequest.ContentLength = postData.Length;
            Stream requestStream = httpWebRequest.GetRequestStream();
            requestStream.Write(postData, 0, postData.Length);
            requestStream.Close();
            try
            {
                HttpWebResponse httpWebResponse = (HttpWebResponse) httpWebRequest.GetResponse();
                Stream responseStream = httpWebResponse.GetResponseStream();
                StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8);
                strResult = streamReader.ReadToEnd();
                httpWebResponse.Close();
                responseStream.Dispose();
                streamReader.Dispose();
            }
            catch (WebException ex)
            {
                strResult = ex.Message;
            }

            return strResult;
        }

        public string Download(string url, string fileName)
        {
            ServicePointManager.DefaultConnectionLimit = 1000;
            var wc = new WebClient();
            var stream = wc.OpenRead(url);
            var reader = new StreamReader(stream);
            byte[] mbyte = new byte[1000000];
            int allmybyte = (int) mbyte.Length;
            int startmbyte = 0;
            while (allmybyte > 0)
            {

                int m = stream.Read(mbyte, startmbyte, allmybyte);
                if (m == 0)
                    break;

                startmbyte += m;
                allmybyte -= m;
            }

            using (var fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                fs.Write(mbyte, 0, startmbyte);
                fs.Flush();
                fs.Close();
            }

            return fileName;
        }

        public string DoGet(string url,
            string requestParam,
            IDictionary<string, string> headParams = null)
        {
            var wc = new WebClient();
            wc.Encoding = Encoding.UTF8;
            if (headParams != null)
            {
                foreach (var s in headParams)
                {
                    wc.Headers.Add(s.Key, s.Value);
                }
            }

            ServicePointManager.DefaultConnectionLimit = 1000;
            var uri = string.Format("{0}?{1}", url, requestParam);
            var resultBytes = wc.DownloadData(uri);
            var result = Encoding.UTF8.GetString(resultBytes);
            return result;
        }

        public async Task<string> DoGetAsync(string url,
            string requestParam,
            IDictionary<string, string> headParams = null,
            int timeout = 20000)
        {
            var wc = new CustomWebClient();
            wc.Encoding = Encoding.UTF8;
            if (headParams != null)
            {
                foreach (var s in headParams)
                {
                    wc.Headers.Add(s.Key, s.Value);
                }
            }

            ServicePointManager.DefaultConnectionLimit = 1000;
            wc.Timeout = timeout;
            var uri = string.Format("{0}?{1}", url, requestParam);
            var resultBytes = await wc.DownloadDataTaskAsync(uri);
            var result = Encoding.UTF8.GetString(resultBytes);
            return result;
        }


        public string DoPost(string url,
            string questParam,
            IDictionary<string, string> headParams = null,
            bool security = false,
            SecurityProtocolType protocolType = SecurityProtocolType.Ssl3,
            int timeout = 200000)
        {
            if (security)
            {
                ServicePointManager.SecurityProtocol = protocolType;
            }

            ServicePointManager.DefaultConnectionLimit = 1000;
            var wc = new CustomWebClient();
            wc.Encoding = Encoding.UTF8;
            wc.Timeout = timeout;
            wc.Headers.Add(HttpRequestHeader.ContentType, "application/x-www-form-urlencoded");
            //添加参数
            if (headParams != null)
            {
                foreach (KeyValuePair<string, string> s in headParams)
                {
                    wc.Headers.Add(s.Key, s.Value);
                }
            }

            var buffer = Encoding.UTF8.GetBytes(questParam);
            var resultBuffer = wc.UploadData(url, buffer);
            return Encoding.UTF8.GetString(resultBuffer);
        }

        public string DoPost(string url,
            string contentType,
            IDictionary<string, string> headParams,
            string bodyParam,
            SecurityProtocolType securityProtocolType = SecurityProtocolType.Tls,
            int timeOut = 200000,
            bool isSecurity = false)
        {
            if (isSecurity)
            {
                ServicePointManager.SecurityProtocol = securityProtocolType;
            }

            ServicePointManager.DefaultConnectionLimit = 1000;
            var wc = new CustomWebClient();
            wc.Timeout = timeOut;
            wc.Encoding = Encoding.UTF8;
            wc.Headers.Add(HttpRequestHeader.ContentType, contentType);
            //添加参数
            if (headParams != null)
            {
                foreach (KeyValuePair<string, string> s in headParams)
                {
                    wc.Headers.Add(s.Key, s.Value);
                }
            }

            var buffer = Encoding.UTF8.GetBytes(bodyParam);
            var resultBuffer = wc.UploadData(url, buffer);
            return Encoding.UTF8.GetString(resultBuffer);
        }

        public async Task<string> PostAsync(string url,
            string contentType,
            IDictionary<string, string> headParams,
            string bodyParam,
            SecurityProtocolType securityProtocolType = SecurityProtocolType.Tls,
            int timeOut = 200000,
            bool isSecurity = false)
        {
            try
            {
                if (isSecurity)
                {
                    ServicePointManager.SecurityProtocol = securityProtocolType;
                }

                ServicePointManager.DefaultConnectionLimit = 1000;
                HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(url);
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentType = contentType;
                httpWebRequest.KeepAlive = true;
                httpWebRequest.Timeout = timeOut;
                //添加参数
                if (headParams != null)
                {
                    foreach (KeyValuePair<string, string> s in headParams)
                    {
                        httpWebRequest.Headers.Add(s.Key, s.Value);
                    }
                }

                if (bodyParam != null)
                {
                    byte[] postData = Encoding.UTF8.GetBytes(bodyParam);
                    httpWebRequest.ContentLength = postData.Length;
                    Stream requestStream = httpWebRequest.GetRequestStream();
                    requestStream.Write(postData, 0, postData.Length);
                    requestStream.Close();
                }

                var response = await httpWebRequest.GetResponseAsync();
                var httpWebResponse = (HttpWebResponse) response;
                Stream responseStream = httpWebResponse.GetResponseStream();
                StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8);
                var strResult = await streamReader.ReadToEndAsync();
                httpWebResponse.Close();
                responseStream.Dispose();
                streamReader.Dispose();
                return strResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}