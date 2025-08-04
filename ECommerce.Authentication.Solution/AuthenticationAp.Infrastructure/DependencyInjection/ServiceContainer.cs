using AuthenticationAp.Application.Interfaces;
using AuthenticationAp.Infrastructure.Data;
using AuthenticationAp.Infrastructure.Repository;
using ecommrece.sharedliberary.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationAp.Infrastructure.DependencyInjection
{
    public static class ServiceContainer 
    {
        public static IServiceCollection AddinfrastructureServices(this IServiceCollection services, IConfiguration config)
        {
            SharedServiceContainer.AddSharedService<AuthenticationDBContext>(services, config, config["MySerilog:fileName"]!);

            services.AddScoped<IUser, UserRepository>();
            return services;
        }
        public static IApplicationBuilder UserinfrastructurePoliciy(this IApplicationBuilder app)
        {
            SharedServiceContainer.UseSharedPolicies(app);
            return app;
        }
    }
}
