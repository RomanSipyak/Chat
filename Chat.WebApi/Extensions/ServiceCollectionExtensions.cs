using Chat.Contracts.Constats.GeneralConstants;
using Chat.Infrastructure.AppContext.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Chat.WebApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureSwagger(this IServiceCollection services, IConfiguration cofiguration)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(cofiguration[ApiConstants.Configuration_SwaggerDoc],
                             new OpenApiInfo
                             {
                                 Title = ApiConstants.Configuration_SwaggerApiTitle,
                                 Version = ApiConstants.Configuration_SwaggerApiVersion
                             }); ;

               // Set the comments path for the Swagger JSON and UI.
               var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
               var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
               c.IncludeXmlComments(xmlPath);

                #region setting for bearer
                c.AddSecurityDefinition(
                   "Bearer",
                   new OpenApiSecurityScheme
                   {
                       Description = ApiConstants.Configuration_SwaggerApiSecuritySchemeDescription,
                       Name = ApiConstants.Configuration_SwaggerApiSecuritySchemeName,
                       In = ParameterLocation.Header,
                       Type = SecuritySchemeType.ApiKey,
                       Scheme = ApiConstants.Configuration_SwaggerBearer,
                   });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                   {
                       {
                         new OpenApiSecurityScheme
                         {
                             Reference = new OpenApiReference
                             {
                                 Type = ReferenceType.SecurityScheme,
                                 Id = ApiConstants.Configuration_SwaggerBearer
                             },
                             Scheme = ApiConstants.Configuration_SwaggerApiSecurityScheme,
                             Name = ApiConstants.Configuration_SwaggerBearer,
                             In = ParameterLocation.Header
                         },
                         new List<String>()
                       }
                   });
                #endregion setting for bearer
            });
        }

        public static void ConfigurationDataBase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString(ApiConstants.Configuration_DbConnection)));
        }
    }
}
