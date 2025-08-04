using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecommrece.sharedliberary.DependencyInjection
{
    public static class JwtAuthentiactionSchema
    {
        public static IServiceCollection AddJwtAuthentiactionSchema(this IServiceCollection services,IConfiguration config)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).
                AddJwtBearer("Bearer", option =>
                {
                    var key = Encoding.UTF8.GetBytes(config.GetSection("Authentication:Key").Value!);
                    var issuer = config.GetSection("Authentication:Issuer").Value!;
                    var audience = config.GetSection("Authentication:Audience").Value!;
                    option.RequireHttpsMetadata = false;
                    option.SaveToken = true;
                    option.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = false,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = issuer,
                        ValidAudience = audience,
                        IssuerSigningKey = new SymmetricSecurityKey(key)

                    };

                });
            return services;

        }
    }
}
