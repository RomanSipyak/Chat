using AutoMapper;
using Chat.BusinessLogic.Helpers;
using Chat.BusinessLogic.Mapping;
using Chat.BusinessLogic.Services;
using Chat.Contracts.ConfigurationObjects;
using Chat.Contracts.Constats.GeneralConstants;
using Chat.Contracts.Entity;
using Chat.Contracts.Interfaces.Services;
using Chat.Infrastructure.AppContext.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Chat.WebApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigurationServices(this IServiceCollection services)
        {
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IAuthenticationService, AuthenticationService>();
        }

        public static void ConfigureSwagger(this IServiceCollection services, IConfiguration cofiguration)
        {
            // Register the Swagger generator, defining 1 or more Swagger documents this comment from msDocs
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

        public static void ConfigurationCors(this IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy("Policy",
                builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                }));
        }

        public static void ConfigurationMapper(this IServiceCollection services)
        {
            MapperConfiguration mappingConfig = new MapperConfiguration(mc => mc.AddProfile(new MappingProfile()));
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
        }

        public static void ConfigAuthenticate(this IServiceCollection services , IConfiguration configuration)
        {
            //start configure JWT
            //we can get our  "JwtSettings": { "SecretKey": "some key" }
            //like configuration["JwtSettings:SecretKey"].ToString();
            var jwtSettings = new JwtSettings();
            configuration.Bind(nameof(JwtSettings), jwtSettings);//that part code binding our jwt setting object that have one property (secretKey) with our settings.json that have the same jsonObject with jwt secutity key(mapping them) 
            services.AddSingleton(jwtSettings);
            // configure jwt authentication
            // services.AddScoped<IIdentityService, IdentityService>();
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x => //setting for how we work with token
            {
                x.RequireHttpsMetadata = false;
                //x.SaveToken = true; //don`t save token in db
                x.TokenValidationParameters = AuthenticationHelper.GetTokenValidationParameters(jwtSettings);
            });
            // configure jwt authentication
            //end configure JWT
        }

        public static void ConfigIdentity(this IServiceCollection services)
        {
            //add configure for identity and db connection
            //services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<DataContext>();
            /* services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                 .AddRoles<IdentityRole>()
                 .AddEntityFrameworkStores<AppDbContext>();*/
            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;
                options.User.RequireUniqueEmail = true;
            })
               .AddDefaultTokenProviders()
               .AddEntityFrameworkStores<AppDbContext>();
        }
    }
}
