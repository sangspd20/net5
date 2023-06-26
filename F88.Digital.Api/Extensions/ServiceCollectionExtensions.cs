using F88.Digital.Api.Services;
using F88.Digital.Application.DTOs.Settings;
using F88.Digital.Application.Interfaces;
using F88.Digital.Application.Interfaces.Shared;
using F88.Digital.Infrastructure.DbContexts;
using F88.Digital.Infrastructure.Identity.Models;
using F88.Digital.Infrastructure.Identity.Services;
using F88.Digital.Infrastructure.Shared.Services;
using AspNetCoreHero.Results;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
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
using System.Text;
using F88.Digital.Application.Interfaces.Repositories.AppPartner;
using F88.Digital.Infrastructure.Services.AppPartner;

namespace F88.Digital.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddSharedInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
            services.Configure<CacheSettings>(configuration.GetSection("CacheSettings"));
            services.Configure<OTPSettings>(configuration.GetSection("OTPSettings"));           
            services.Configure<ApiShareServiceSettings>(configuration.GetSection("ApiShareServiceSettings"));
            services.Configure<AffiliateSettings>(configuration.GetSection("AffiliateSettings"));
            services.Configure<S3Settings>(configuration.GetSection("S3Settings"));
            services.AddSingleton(configuration.GetSection("SendOTPSettings").Get<SendOTPSettings>());

            services.AddTransient<IDateTimeService, SystemDateTimeService>();
            services.AddTransient<IMailService, SMTPMailService>();
            services.AddTransient<IAuthenticatedUserService, AuthenticatedUserService>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IApiShareService, ApiShareService>();
            services.AddTransient<IApiPolService, ApiPolService>();
        }

        public static void AddEssentials(this IServiceCollection services)
        {
            services.RegisterSwagger();
            services.AddVersioning();
        }

        private static void RegisterSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                //TODO - Lowercase Swagger Documents
                //c.DocumentFilter<LowercaseDocumentFilter>();
                //Refer - https://gist.github.com/rafalkasa/01d5e3b265e5aa075678e0adfd54e23f
                c.IncludeXmlComments(string.Format(@"{0}\F88.Digital.Api.xml", System.AppDomain.CurrentDomain.BaseDirectory));
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "F88.Digital.API",
                    License = new OpenApiLicense()
                    {
                        Name = "Digital License",
                        Url = new Uri("https://opensource.org/licenses/MIT")
                    }
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Description = "Input your Bearer token in this format - Bearer {your token here} to access this API",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                            Scheme = "Bearer",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        }, new List<string>()
                    },
                });
            });
        }

        private static void AddVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
            });
        }

        public static void AddContextInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<IdentityContext>(options =>
                    options.UseInMemoryDatabase("F88_Digital_Identity"));
                services.AddDbContext<AppPartnerDbContext>(options =>
                   options.UseInMemoryDatabase("F88_Digital_AppPartner"));
                services.AddDbContext<AffiliateDbContext>(options =>
                    options.UseInMemoryDatabase("F88_Digital_Affiliate"), ServiceLifetime.Transient);
            }
            else
            {
                services.AddDbContext<IdentityContext>(options => options.UseSqlServer(configuration.GetConnectionString("IdentityConnection")));
                services.AddDbContext<AppPartnerDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("AppPartnerConnection"), b => b.MigrationsAssembly(typeof(AppPartnerDbContext).Assembly.FullName)));
                services.AddDbContext<AffiliateDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("AffiliateConnection"), b => b.MigrationsAssembly(typeof(AffiliateDbContext).Assembly.FullName)), ServiceLifetime.Transient);
            }
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.Password.RequireNonAlphanumeric = false;
            }).AddEntityFrameworkStores<IdentityContext>().AddDefaultUI().AddDefaultTokenProviders();

            #region Services

            services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient<PhoneAccessSetting>();
            services.AddTransient<SpreadSheetIdSetting>();
            services.AddTransient<UrlPOLSetting>();
            services.AddTransient<UrlShareServiceSetting>();
            #endregion Services

            services.Configure<JWTSettings>(configuration.GetSection("JWTSettings"));
            services.Configure<PhoneAccessSetting>(configuration.GetSection("PhoneAccess"));
            services.Configure<SpreadSheetIdSetting>(configuration.GetSection("spreadSheetId"));
            services.Configure<UrlPOLSetting>(configuration.GetSection("UrlPOL"));
            services.Configure<UrlShareServiceSetting>(configuration.GetSection("UrlShareService"));
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.SaveToken = false;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidIssuer = configuration["JWTSettings:Issuer"],
                        ValidAudience = configuration["JWTSettings:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTSettings:Key"]))
                    };
                    o.Events = new JwtBearerEvents()
                    {
                        OnAuthenticationFailed = c =>
                        {
                            c.NoResult();
                            c.Response.StatusCode = 500;
                            c.Response.ContentType = "text/plain";
                            return c.Response.WriteAsync(c.Exception.ToString());
                        },
                        OnChallenge = context =>
                        {
                            context.HandleResponse();
                            context.Response.StatusCode = 401;
                            context.Response.ContentType = "application/json";
                            var result = JsonConvert.SerializeObject(Result.Fail("You are not Authorized"));
                            return context.Response.WriteAsync(result.ToLower());
                        },
                        OnForbidden = context =>
                        {
                            context.Response.StatusCode = 403;
                            context.Response.ContentType = "application/json";
                            var result = JsonConvert.SerializeObject(Result.Fail("You are not authorized to access this resource"));
                            return context.Response.WriteAsync(result.ToLower());
                        },
                    };
                });
        }

        public static void AddCorsConfig(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
        }
    }
}