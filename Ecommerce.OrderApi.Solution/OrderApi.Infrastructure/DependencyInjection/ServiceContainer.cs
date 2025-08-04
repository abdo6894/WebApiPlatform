using ecommrece.sharedliberary.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderApi.Application.Interfaces;
using OrderApi.Infrastructure.Data;
using OrderApi.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApi.Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddinfrastructureServices(this IServiceCollection services, IConfiguration config)
        {
            SharedServiceContainer.AddSharedService<OrderDBContext>(services, config, config["MySerilog:fileName"]!);

            services.AddScoped<IOrder, OrderRepository>();
            return services;
        }
        public static IApplicationBuilder UserinfrastructurePoliciy(this IApplicationBuilder app)
        {
           SharedServiceContainer.UseSharedPolicies(app);
            return app;
        }
    }
}
