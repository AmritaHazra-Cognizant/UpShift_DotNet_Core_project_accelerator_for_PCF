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
using DotNetCore.Framework.RestService;
using DotNetCore.API.DataService.Providers.Api;
using DotNetCore.API.DBAccess.Contracts;
using DotNetCore.API.DBAccess.Implementations;
using DotNetCore.Framework.Caching.CoreCaching;
using DotNetCore.Framework.Interception.Strategies;
using DotNetCore.API.Caching.MemoryCache;
using DotNetCore.API.Caching;

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
            //Logging Object
            services.AddScoped<TransactionLogEntry>();
            // SSO User Object
            services.AddScoped<SignOnUser>();

            // JWT Authentication Service
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            // Read WCF configurations from AppSettings.Json file
            // To Do: - If there is no WCF service used please remove this line as well as 
            // configuration from appsettings.json
            services.Configure<ServiceBindings>(config.GetSection(ServiceBindings.ServiceBindingsKey));

            //*********** This section for API REST Call **********
            // Add Rest service endpoint in AppSettings.Json with Key name 'RestApiEndpoint' or 
            // with any other name. Just need to change the key name here.
            // To DO :- If there is no REST API call we can remove this section.
            var restServiceSection = config.GetSection("RestApiEndpoint");
            if (restServiceSection.Exists())
            {
                services.AddSingleton<ApiConfiguration>();
                services.AddScoped<IApiClient>(s =>
                new ApiClient(s.GetRequiredService<ApiConfiguration>(), config["RestApiEndpoint"], null));
            }

            //*********** End of  API REST Call Section **********


            // To Enable Interception for Logging/Caching
            services.EnableInterceptionProxy(p =>
            p.AddInterceptor<LogMethodAttribute, LogMethodInterceptor>().
            AddInterceptor<MemoryCacheAttribute, MemoryCacheInterceptor>().
            WithOrderingStrategy<PyramidOrderStrategy>());

            // Memory Cache Register
            services.AddMemoryCache();
            services.Configure<CacheConfig>(config.GetSection(CacheConfig.CacheConfigKey));
            services.AddScoped<ICacheConfigFactory, CacheConfigFactory>(); 
            services.AddScoped<IMemoryCacheService, MemoryCacheService>();
        }
        internal static void RegisterUserProcessingServices(this IServiceCollection services)
        {
            /*
             * ************************ Sample Service Registration **********
             * This is just to show diffrent scenarios like calling WCF, REST, DB external call
             * To DO- Needs to remove this as pe project requirements
             */

            // Register External WCF Proxy
            services.AddScoped<IEndpointFactory<IWcfService>, EndpointFactory<IWcfServiceChannel>>();
            // Register External REST API Proxy
            services.AddScoped<IEmployeeRestApi, EmployeeRestApi>();
            // Sac layer
            services.AddScopedWithProxy<ISampleSac, SampleSac>();

            // DAC layer
            services.AddScopedWithProxy<ISampleDac, SampleDac>();
            // BC Layer
            services.AddScopedWithProxy<ISampleBc, SampleBc>();

            //*************************End Section **********
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
