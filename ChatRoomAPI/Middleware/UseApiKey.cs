using ChatRoomAPI.Models;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Net;

namespace ChatRoomAPI.Middleware
{
    public class UseApiKey
    {
        private readonly RequestDelegate _next;
        private readonly AdminSettings _adminSettings;
        public UseApiKey(RequestDelegate next, IOptions<AdminSettings> adminSettings)
        {
            _next = next;
            _adminSettings = adminSettings.Value;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.Value.ToLowerInvariant().Contains("admin"))
            {
                if (context.Request.Headers.ContainsKey(Constants.ApiKey))
                {
                    if (!_adminSettings.ApiKeys.Contains(context.Request.Headers[Constants.ApiKey]))
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                        context.Response.ContentType = "text/plain";
                        await context.Response.WriteAsync("Not found");
                        await context.Response.CompleteAsync();
                    }
                }
                else
                {
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    context.Response.ContentType = "text/plain";
                    await context.Response.WriteAsync("Not found");
                    await context.Response.CompleteAsync();
                }
            }
            await _next(context);
        }
    }
}
