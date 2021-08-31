using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SunriseSunsetDataProvider.Interface;
using SunriseSunsetDataProvider.VariableClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunriseSunsetDataProvider.Module
{
    public static class RequestLogMiddleWearExtension
    {
        /// <summary>
        /// 自定義的中介層log
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseRequestLog(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestLogMiddleware>();
        }

        /// <summary>
        /// 中介層的主體
        /// </summary>
        public class RequestLogMiddleware
        {
            private readonly RequestDelegate _next;

            public RequestLogMiddleware(RequestDelegate next)
            {
                _next = next;
            }

            public async Task InvokeAsync(HttpContext context)
            {
                var logger = context.RequestServices.GetService(typeof(ILog)) as ILog;
                if (logger == null)
                {
                    throw new ArgumentNullException("ILogger");
                }

                string errMessage = string.Empty;
                long cost = DateTime.Now.Ticks;
                try
                {
                    // Call the next delegate/middleware in the pipeline
                    await _next(context);

                    if (context.Items.TryGetValue("ErrorMsg", out object itemErrorMsg))
                    {
                        errMessage = itemErrorMsg as string;
                    }
                }
                catch (Exception ex)
                {
                    errMessage = ex.ToString();
                }

                cost = DateTime.Now.Ticks - cost;
                var information = getInformation(context);
                var logInfo = new LogInfo
                {
                    Action = context.Request.Path,
                    Message = errMessage,
                    Info = JsonConvert.SerializeObject(information),
                    Cost = cost / TimeSpan.TicksPerMillisecond,
                    Success = context.Response.StatusCode == StatusCodes.Status200OK || context.Response.StatusCode == StatusCodes.Status204NoContent
                };

                logger.AddLog($"[{DateTime.Now:yyyy-MM-dd}],{JsonConvert.SerializeObject(logInfo)}{Environment.NewLine}", DateTime.Now.ToString("yyyy-MM-dd"));
            }
        }

        private static object getInformation(HttpContext context)
        {
            int.TryParse(context.User?.FindFirst("sub")?.Value, out int memberId);
            int.TryParse(context.User?.FindFirst("app_id")?.Value, out int appId);

            var queryValue = context.Request.QueryString.Value;

            string formValue = GetFormValue(context);

            var information = new
            {
                MemberId = memberId,
                AppId = appId,
                context.Request.Method,
                QueryValue = queryValue,
                FormValue = formValue,
                context.Response.StatusCode, // 用來篩選401的
                ClientIp = context.Connection?.RemoteIpAddress?.ToString()
            };

            return information;
        }

        public static string GetFormValue(HttpContext context)
        {
            string formValue = string.Empty;
            if (context.Request.Body.CanRead && context.Request.Body.CanSeek)
            {
                using (var buffer = new MemoryStream())
                {
                    context.Request.Body.Position = 0;
                    context.Request.Body.CopyTo(buffer);
                    buffer.Position = 0;
                    formValue = new StreamReader(buffer, Encoding.UTF8).ReadToEnd();
                    buffer.Position = 0;
                }
            }

            return formValue;
        }
    }
}
