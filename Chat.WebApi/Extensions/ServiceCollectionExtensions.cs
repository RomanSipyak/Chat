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
               c.SwaggerDoc(cofiguration["Swagger:Doc"],
                            new OpenApiInfo
                            {
                                Title = "ChatApi",
                                Version = "v1"
                            });

               // Set the comments path for the Swagger JSON and UI.
               var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
               var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
               c.IncludeXmlComments(xmlPath);

                #region setting for bearer
                c.AddSecurityDefinition(
                   "Bearer",
                   new OpenApiSecurityScheme
                   {
                       Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                       Name = "Authorization",
                       In = ParameterLocation.Header,
                       Type = SecuritySchemeType.ApiKey,
                       Scheme = "Bearer",
                   });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                   {
                       {
                         new OpenApiSecurityScheme
                         {
                             Reference = new OpenApiReference
                             {
                                 Type = ReferenceType.SecurityScheme,
                                 Id = "Bearer"
                             },
                             Scheme = "oath2",
                             Name = "Bearer",
                             In = ParameterLocation.Header
                         },
                         new List<String>()
                       }
                   });
                #endregion setting for bearer
            });
        }
    }
}
