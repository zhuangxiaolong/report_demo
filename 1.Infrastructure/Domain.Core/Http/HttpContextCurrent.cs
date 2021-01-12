using System;
using Microsoft.AspNetCore.Http;

namespace Domain.Core.Http
{
    public static class HttpContextCurrent
    {
        public static IServiceProvider ServiceProvider;
        static HttpContextCurrent()
        {

        }

        public static HttpContext HttpContext
        {
            get
            {
                object factory = ServiceProvider.GetService(typeof(IHttpContextAccessor));
                HttpContext context = ((IHttpContextAccessor)factory).HttpContext;
                return context;
            }
        }
    }
}