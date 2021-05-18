using DotNetCore.API.Web.Security.Configuration;
using DotNetCore.API.Web.Security.Implementation;
using DotNetCore.API.Web.Security.Service;
using DotNetCore.Framework.Logging.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DotNetCore.API.DataService.Contracts;
using DotNetCore.API.Example.Service;
using DotNetCore.Framework.WebServices;
using DotNetCore.Framework.Interception.Extensions;
using DotNetCore.API.Logging;
using DotNetCore.API.DataService.Implementations;
using DotNetCore.API.BusinessLogic.Contracts;
using DotNetCore.API.BusinessLogic.Implementations;
using DotNetCore.Framework.WebServices.Configuration;

namespace DotNetCore.API.Web.Extensions
{
    public static class ServiceExtensions
    {
        internal static void RegisterDefaultCommonService(this IServiceCollection services,
            IConfiguration config)
        {
            // JWT WEB Token Configuration Load
            services.Configure<ConfigJwtWebToken>(config.GetSection(ConfigJwtWebToken.JwtWebToken));
            services.AddHttpContextAccessor();
            services.AddScoped<TransactionLogEntry>();
            services.AddScoped<SignOnUser>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.Configure<ServiceBindings>(config.GetSection(ServiceBindings.ServiceBindingsKey));

            // To Enable Interception for Logging/Caching
            services.EnableInterceptionProxy(p =>
            p.AddInterceptor<LogMethodAttribute, LogMethodInterceptor>());
        }
        internal static void RegisterUserProcessingServices(this IServiceCollection services)
        {
            // Register External WCF Proxy
            services.AddScoped<IEndpointFactory<IWcfService>, EndpointFactory<IWcfServiceChannel>>();
            // Sac layer
            services.AddScopedWithProxy<ISampleSac, SampleSac>();
            // BC Layer
            services.AddScoped<ISampleBc, SampleBc>();
        }
        internal static void SetCorsPolicyDeployment(this IServiceCollection services, IConfiguration config)
        {
            services.AddCors(option =>
            {
                option.AddPolicy("CorsPolicyDeployment", builder =>
                {
                    builder.WithOrigins(new string[] { config.GetValue<string>("AllowedHosts") })
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
                });
            });
        }
        internal static void SetCorsPolicyDevelopment(this IServiceCollection services)
        {
            services.AddCors(option =>
            {
                option.AddPolicy("CorsPolicyDevelopment", builder =>
                {
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
            });
        }

        internal static void AddSwaggerGenWithJwtAuthorization(this IServiceCollection services)
        {
            services.AddSwaggerGen(swagger =>
            {
                // To Add xml documentation to the swagger UI, enable xml file generation in project build properties.
                var docPath = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
                if (File.Exists(docPath))
                {
                    swagger.IncludeXmlComments(docPath);
                }
                // To Enable authorization using Swagger (JWT)  
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "-Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                });

                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}

                    }
                });
            });
        }
    }
}
