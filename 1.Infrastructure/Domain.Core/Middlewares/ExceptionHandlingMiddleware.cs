using System;
using System.Net;
using System.Threading.Tasks;
using Domain.Core.Http;
using Exceptionless;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Domain.Core.Middlewares
{
   public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ExceptionlessConfiguration _exceptionlessConfiguration;
        public ExceptionHandlingMiddleware(RequestDelegate next,
                                           ExceptionlessConfiguration exceptionlessConfiguration)
        {
            _next = next;
            _exceptionlessConfiguration = exceptionlessConfiguration;
        }

        public async Task Invoke(HttpContext context /* other scoped dependencies */)
        {
            try
            {
                await _next(context);
                log(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        public void log(HttpContext context)
        {
//            var logBuilder = ExceptionlessClient.Default.CreateLog(context.Request.Path)
//                                                .SetHttpContext(context)
//                                                .AddObject(context.GetRequestInfo(_exceptionlessConfiguration));
//#if !DEV
//            if (!context.Request.Path.StartsWithSegments(new PathString("/authorization")))
//            {
//                var jwtStr = context.Request.Headers["Authorization"].FirstOrDefault();
//                if (jwtStr != null)
//                {
//                    var handler = new JwtSecurityTokenHandler();
//                    var jwt = handler.ReadJwtToken(jwtStr);
//                    var userInfoStr = jwt.Claims.FirstOrDefault(e => e.Type == ClaimTypes.UserData).Value;
//                    var userInfo = JsonConvert.DeserializeObject<UserInfo>(userInfoStr);
//                    logBuilder.SetUserIdentity(jwt.Id, userInfo.UserName);
//                }
//            } 
//#endif
//            if (context.Request.Method.ToUpper() == "GET")
//            {
//                logBuilder.SetProperty("Parameters", context.Request.QueryString);
//            }
//            logBuilder.Submit();
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            exception.ToExceptionless()
                     .SetProperty("body", context?.Request.Path)
                     .SetProperty("path", context?.Request.Path)
                     .SetMessage(exception.Message)
                     .Submit();
            var code = HttpStatusCode.OK; // 500 if unexpected
            var result = ExceptionResponse.CreateResponse();
            result.err_code = 500;
            result.err_msg = "服务器错误";
            var resultStr = JsonConvert.SerializeObject(result);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(resultStr);
        }
    }

    public class ExceptionResponse : APIResponseBodyBase<ExceptionResponse>
    {
        public int err_code { get; set; }
        public string err_msg { get; set; }
    }
}