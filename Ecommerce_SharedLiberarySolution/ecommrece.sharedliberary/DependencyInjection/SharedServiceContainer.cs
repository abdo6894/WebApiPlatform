using ecommrece.sharedliberary.MiddleWare;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace ecommrece.sharedliberary.DependencyInjection
{
    public static class SharedServiceContainer
    {
        public static IServiceCollection AddSharedService<Tcontext>
            (this IServiceCollection services, IConfiguration config, string filename) where Tcontext : DbContext
        {
            // add generic database
            services.AddDbContext<Tcontext>(option=>
            {
                option.UseSqlServer(config.GetConnectionString("ecommerceconnection"),sqlserveroption=>
                sqlserveroption.EnableRetryOnFailure());

            });
             // configue serilog 

            Log.Logger=new LoggerConfiguration()
               .MinimumLevel.Information()
                .WriteTo.Debug()
                .WriteTo.Console()
                .WriteTo.File(path : $"{filename}- .text",
            restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
            outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
            rollingInterval: RollingInterval.Day)
               .CreateLogger();

            JwtAuthentiactionSchema.AddJwtAuthentiactionSchema(services, config);
            return services;
        }
        public static IApplicationBuilder UseSharedPolicies(this IApplicationBuilder app)
        {
            app.UseMiddleware<GlobalException>();
            app.UseMiddleware<ListenToOnlyApiGetAway>();
            return app;
        }
    }
}
