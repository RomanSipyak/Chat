using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chat.Contracts.Constats.GeneralConstants;
using Chat.Infrastructure.AppContext.Persistence;
using Chat.WebApi.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Chat.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigurationServices();
            services.ConfigurationDataBase(Configuration);
            services.ConfigurationMapper();
            services.ConfigureSwagger(Configuration);
            services.ConfigAuthenticate(Configuration);
            services.ConfigIdentity();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
            options.SwaggerEndpoint(Configuration[ApiConstants.Configuration_SwaggerEndpoint],
                string.Concat(ApiConstants.Configuration_SwaggerApiTitle, " ", ApiConstants.Configuration_SwaggerApiVersion));
            });
            //Policy extract in config file
            app.UseCors(ApiConstants.PolicyForCors);

            /*  //swagger configure
              var swaggerOptions = new Options.SwaggerOptions();
              Configuration.GetSection(nameof(Options.SwaggerOptions)).Bind(swaggerOptions);
              app.UseSwagger(option => { option.RouteTemplate = swaggerOptions.JsonRoute; });
              app.UseSwaggerUI(options => { options.SwaggerEndpoint(swaggerOptions.UIEndPoint, swaggerOptions.Description); });
              //end of swagger configure*/



            /*app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint(Configuration[GeneralApiConstants.Configuration_SwaggerEndpoint],
                    string.Concat(GeneralApiConstants.SwaggerApiTitle, " ", GeneralApiConstants.SwaggerApiVersion));
            });*/

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
