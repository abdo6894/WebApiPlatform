using ecommrece.sharedliberary.Logs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ecommrece.sharedliberary.MiddleWare
{
    public class GlobalException(RequestDelegate next)
    {
        public  async Task InvokeAsync(HttpContext context)
        {
            string message = "sorry , internal server error happend ,Kindley try again";
            int statuescode = (int)HttpStatusCode.InternalServerError;
            string title = "Error";


            try
            {
                await next(context);

                if(context.Response.StatusCode== StatusCodes.Status429TooManyRequests)
                {
                    title= "Too many requests";
                    message = "You have made too many requests, please try again later.";

                    statuescode = StatusCodes.Status429TooManyRequests;
                    await Modifayheader(context, statuescode, title, message);

                }
                if(context.Response.StatusCode == StatusCodes.Status401Unauthorized)
                {
                    title = "Alert";
                    message = "you are not un Authorized";
                    await Modifayheader(context, statuescode, title, message);
                }
                if(context.Response.StatusCode == StatusCodes.Status403Forbidden)
                {
                    title = "Alert";
                    message = "you are not allowed to access this resource";
                    statuescode = StatusCodes.Status403Forbidden;
                    await Modifayheader(context, statuescode, title, message);
                }

            }
            catch(Exception ex) 
            {
                LogException.LogExceptions(ex);
                if(ex is TaskCanceledException || ex is TimeoutException)
                {
                    title = "Out Of Time ";
                    message = "Request Time Out ......... Try Again";
                    statuescode = StatusCodes.Status408RequestTimeout;
                }
                await Modifayheader(context, statuescode, title, message);

            }

        }

        private async Task Modifayheader(HttpContext context, int statuescode, string title, string message)
        {
            context.Request.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new ProblemDetails()
            {
                Detail=message,
                Title=title,
                Status=statuescode
            }),CancellationToken.None);

            return;
            
        }
    }
}
