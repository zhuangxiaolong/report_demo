using System;
using System.Net;

namespace Domain.Core.Http
{
    public class CustomWebClient : WebClient
    {
        public int Timeout { get; set; }
        protected override WebRequest GetWebRequest(Uri address)
        {
            HttpWebRequest request = (HttpWebRequest)base.GetWebRequest(address);
            request.Timeout = Timeout;
            request.ReadWriteTimeout = Timeout;
            return request;
        }
    }
}