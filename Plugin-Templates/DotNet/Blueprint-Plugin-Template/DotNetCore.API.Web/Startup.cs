using DotNetCore.API.Web.ActionFilters;
using DotNetCore.API.Web.Extensions;
using DotNetCore.API.Web.Security.Configuration;
using DotNetCore.API.Web.Security.Implementation;
using DotNetCore.API.Web.Security.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore.API.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
        }


        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            // Set CORS Policy based on Environments
            if (_webHostEnvironment.IsDevelopment())
            {
                services.SetCorsPolicyDevelopment();
            }
            else
            {
                services.SetCorsPolicyDeployment(_configuration);
            }
            //Swagger Service
            if (_webHostEnvironment.IsDevelopment())
            {
                services.AddSwaggerGenWithJwtAuthorization();
            }
            // Core Service
            services.RegisterDefaultCommonService(_configuration);
            // User provided servcies
            services.RegisterUserProcessingServices();

            //Optionally disable the default model validation handling
            // to allow amanging validation error response with custom code.
            // See the ModelStateInvalidFilter done as an example
            services.Configure<ApiBehaviorOptions>(opts =>
            {
                opts.SuppressModelStateInvalidFilter = true;
            });
            // Add custom Filter Globally
            services.AddControllers(opts =>
            {
                // The custom model state filter can be aded globally
                // or added to specific controller or controller action(s)
                // Using the attribute construct
                opts.Filters.Add(typeof(ModelStateInvalidFilter));
                opts.Filters.Add(typeof(TransactionExceptionLoggingFilter));
            });
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Cors should remain first

            var namedCorsPolicy = env.IsDevelopment() ? "CorsPolicyDevelopment" : "CorsPolicyDeployment";
            app.UseCors(namedCorsPolicy);
            
            // Add Response Headers
            app.UseCoreResponseHaedersMiddleware();

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web API Template");
                    c.RoutePrefix = string.Empty;
                });
            }
            app.UseAuthenticationMiddleware();
            app.UseRouting();
            app.UseExceptionHandlingMiddleware();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
