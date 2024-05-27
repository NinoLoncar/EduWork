using EduWork.Data;
using EduWork.WebApi.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

namespace EduWork.WebApi
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
            return services;
        }
        public static IServiceCollection AddConfiguredControllers(this IServiceCollection services)
        {
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.WriteIndented = true;
            });
            return services;
        }
        public static IServiceCollection AddConfiguredCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowBlazorClient", builder =>
                {
                    builder.WithOrigins("https://localhost:7110", "http://localhost:5143")
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowCredentials();
                });
            });
            return services;
        }

        public static IServiceCollection AddConfiguredSwagger(this IServiceCollection services, SwaggerAuthorizationConfiguration swaggerAuthorizationConfiguration)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EduWork", Version = "v1" });
                c.AddSecurityDefinition(
                    "OAuth2",
                    new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.OAuth2,
                        Flows = new OpenApiOAuthFlows
                        {
                            AuthorizationCode = new OpenApiOAuthFlow
                            {
                                TokenUrl = new Uri(swaggerAuthorizationConfiguration.TokenUrl),
                                AuthorizationUrl = new Uri(swaggerAuthorizationConfiguration.AuthorizationUrl),
                                Scopes = new Dictionary<string, string>
                                {
                                    { swaggerAuthorizationConfiguration.Scope , "Access API as User" }
                                }
                            },
                        },
                    });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference{Type=ReferenceType.SecurityScheme,Id="OAuth2"}
                        },
                        new[]{swaggerAuthorizationConfiguration.Scope
                        }
                    }
                });
            });
            return services;
        }

    }
}
