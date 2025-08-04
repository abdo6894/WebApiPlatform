using Microsoft.AspNetCore.Http;

namespace ecommrece.sharedliberary.MiddleWare
{
    public class ListenToOnlyApiGetAway(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            var signedheader = context.Request.Headers["Api-Gateway"];
            if (signedheader.FirstOrDefault() is null)
            {
                context.Response.StatusCode = StatusCodes.Status405MethodNotAllowed;
                await context.Response.WriteAsync("Method Not Allowed");
                return;
            }
            else
            {
                await next(context);
            }
        }
    }
}
