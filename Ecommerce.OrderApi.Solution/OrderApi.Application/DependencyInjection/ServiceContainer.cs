using ecommrece.sharedliberary.Logs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderApi.Application.Interfaces;
using OrderApi.Application.Services;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApi.Application.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {

            services.AddHttpClient<IOrderService, OrderServvice>(
                opton =>
                {
                    opton.BaseAddress = new Uri(config["ApiGateway:BaseAddress"]!);
                    opton.Timeout = TimeSpan.FromSeconds(30);
                });
            var retrystratigy = new RetryStrategyOptions()
            {
                ShouldHandle = new PredicateBuilder().Handle<TaskCanceledException>(),
                BackoffType = DelayBackoffType.Constant,
                UseJitter = true,
                MaxRetryAttempts = 3,
                Delay = TimeSpan.FromMilliseconds(500),
                OnRetry = args =>
                {
                    string message = $"ONRetry,Attempts : {args.AttemptNumber} outcome {args.Outcome}";
                    LogException.LogToConsole(message);
                    LogException.LogToDebugger(message);
                    return ValueTask.CompletedTask;


                }

            };
            services.AddResiliencePipeline("my-retry-pipline", builder =>
            {
            builder.AddRetry(retrystratigy);
            });


            return services;

        }
    }
}
