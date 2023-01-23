using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
using BeyondIT.MicroLoan.Api.Infrastructure.Filters;
using BeyondIT.MicroLoan.Api.Infrastructure.Helpers;
using BeyondIT.MicroLoan.Api.Infrastructure.Interfaces;
using BeyondIT.MicroLoan.Api.Infrastructure.Middleware;
using BeyondIT.MicroLoan.Api.Infrastructure.Service;
using BeyondIT.MicroLoan.Api.Infrastructure.Services;
using BeyondIT.MicroLoan.Api.Infrastructuree.Services;
using BeyondIT.MicroLoan.Api.Models;
using BeyondIT.MicroLoan.Application.Autofac;
using BeyondIT.MicroLoan.Application.Autofac.Aop;
using BeyondIT.MicroLoan.Application.Interfaces;
using BeyondIT.MicroLoan.Application.Mappings;
using BeyondIT.MicroLoan.Infrastructure.Configurations;
using BeyondIT.MicroLoan.Infrastructure.Constants;
using BeyondIT.MicroLoan.Infrastructure.Database;
//using BeyondIT.MicroLoan.Infrastructure.Database;
using BeyondIT.MicroLoan.Infrastructure.Extensions;
using BeyondIT.MicroLoan.Infrastructure.Http;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json.Serialization;

namespace BeyondIT.MicroLoan.Api
{
    public class Startup
    {
        private IConfiguration Configuration { get; set; }
        public static IContainer Container;

        private const string SecretKey = "42b71756-9b45-475c-a897-29d3d14df09b";
        private readonly SymmetricSecurityKey
           _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));

        public Startup(IHostingEnvironment env)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables();

            AutoMapperConfig.CreateMappers();
            Configuration = builder.Build();
        }

      

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy("AllowAllHeaders", policyBuilder =>
            {
                policyBuilder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));
            IConfigurationSection jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));
            services.AddLogging(logging =>
            {
                logging.AddConfiguration(Configuration.GetSection("Logging"));
                logging.AddConsole();
                logging.AddLog4Net();
            }).Configure<LoggerFilterOptions>(options => options.MinLevel =
            LogLevel.Information);

            // Configure JwtIssuerOptions
            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                 .AddJwtBearer(cfg =>
                 {
                     cfg.RequireHttpsMetadata = false;
                     cfg.SaveToken = true;

                     cfg.TokenValidationParameters = new TokenValidationParameters
                     {
                         ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],
                         ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],
                         IssuerSigningKey = _signingKey,
                         ValidateIssuer = false,
                         ValidateAudience = false
                     };
                 });
            services.AddOptions();

            services.AddResponseCaching();

            services.Configure<AppSettings>(Configuration);

            services.AddMemoryCache();

            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddResponseCaching();

            services.AddMvc()
            .AddJsonOptions(
                options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                    options.SerializerSettings.DateFormatHandling =
                        Newtonsoft.Json.DateFormatHandling.IsoDateFormat;
                    options.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Local;
                });

            var builder = new ContainerBuilder();
            builder.Populate(services);


            AutofacRegisterServices(builder);
            AutofacCongfig.RegisterServices(builder);

            builder.RegisterType<JwtFactory>().As<IJwtFactory>().SingleInstance();

            builder.RegisterType<JwtAuthorizationAttribute>().AsSelf();

            Container = builder.Build();
            InitData();
            return Container.Resolve<IServiceProvider>();

        }
        private static void AutofacRegisterServices(ContainerBuilder builder)
        {
            builder.RegisterType<ContainerAccessor>().As<IContainerAccessor>();
            builder.RegisterType<ActionContextAccessor>().As<IActionContextAccessor>();
            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>();
            builder.RegisterType<HttpRequestService>().As<IHttpRequestService>();
            builder.RegisterType<ExecutionEnvironmentAccessor>().AsImplementedInterfaces();

            builder.RegisterType<AppSettingsAccessor>().As<IAppSettingsAccessor>();


            builder.RegisterType<ViewRender>().AsSelf();

            builder.RegisterType<UserContext>().As<IUserContext>();
            //Filters
            builder.RegisterType<JsonService>().As<IJsonService>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(ExceptionInterceptor));

            builder.RegisterType<JwtFactory>().As<IJwtFactory>().SingleInstance();

            builder.RegisterType<JwtAuthorizationAttribute>().AsSelf();
        }
        private static void InitData()
        {
            //Seed data
            var seedDataService = Container.Resolve<SeedDataService>();
             seedDataService.SeedData();

            ////Update application pages and navigations            
            //var applicationStartupService = Container.Resolve<ApplicationStartupService>();
            //applicationStartupService.InitializeApplication();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = MicroLoanConstants.ZaCultureInfo;
            System.Threading.Thread.CurrentThread.CurrentUICulture = MicroLoanConstants.ZaCultureInfo;

            app.UseCors("AllowAllHeaders");

            RewriteOptions options = new RewriteOptions().Add(Rewrite404Requests);
            app.UseRewriter(options);

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.Use(next => async context =>
            {
                // Keep the original stream in a separate
                // variable to restore it later if necessary.
                Stream stream = context.Request.Body;

                // Optimization: don't buffer the request if
                // there was no stream or if it is rewindable.
                if (stream == Stream.Null || stream.CanSeek)
                {
                    await next(context);

                    return;
                }

                try
                {
                    using (var buffer = new MemoryStream())
                    {
                        // Copy the request stream to the memory stream.
                        await stream.CopyToAsync(buffer);

                        // Rewind the memory stream.
                        buffer.Position = 0L;

                        // Replace the request stream by the memory stream.
                        context.Request.Body = buffer;

                        // Invoke the rest of the pipeline.
                        await next(context);
                    }
                }

                finally
                {
                    // Restore the original stream.
                    context.Request.Body = stream;
                }
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler($"/{MicroLoanConstants.DefaultClient}/errors");

            }

            app.UseExceptionLogger();
            SetupMvcRoutes(app);
        }
        private static void SetupMvcRoutes(IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                //Route with client
                routes.MapRoute("BeyondITLoanRoute1", "api/{controller}/{action}/{id}",
                    new
                    {
                        client = MicroLoanConstants.DefaultClient
                    }, new { client = new NotInControllerNames() }
                );

                routes.MapRoute("BeyondITLoanRoute2", "api/{controller}/{id}",
                    new
                    {
                        client = MicroLoanConstants.DefaultClient,
                        action = "GetById"
                    }, new
                    {
                        client = new NotInControllerNames(),
                        id = new NotInActionNames()
                    }
                );

                routes.MapRoute("BeyondITLoanRoute3", "api/{controller}/{action}",
                    new
                    {
                        client = MicroLoanConstants.DefaultClient,
                    }, new { client = new NotInControllerNames() }
                );


                routes.MapRoute("BeyondITLoanRoute4", "api/{controller}",
                    new
                    {
                        client = MicroLoanConstants.DefaultClient,
                        action = "get"
                    }, new { client = new NotInControllerNames() }
                );
            });
        }

        private static void Rewrite404Requests(RewriteContext context)
        {
            if (!Path.HasExtension(context.HttpContext.Request.Path.Value) &&
                !context.HttpContext.Request.Path.Value.Contains("/api/"))
            {
                var env = Container.Resolve<IHostingEnvironment>();
                if (File.Exists(Path.Combine(env.ContentRootPath, @"wwwroot\index.html")))
                {
                    context.HttpContext.Request.Path = "/index.html";
                    ResponseHeaders headers = context.HttpContext.Response.GetTypedHeaders();
                    headers.CacheControl = new CacheControlHeaderValue
                    {
                        Public = true,
                        MaxAge = TimeSpan.FromSeconds(5)
                    };

                    context.HttpContext.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Vary] = new[] { "Accept-Encoding" };
                }
                else
                {
                    context.HttpContext.Response.WriteAsync("MicroLoan  API is running!");
                    context.Result = RuleResult.EndResponse;
                }
            }
        }

       
    }
    internal class NotInControllerNames : IRouteConstraint
    {
        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values,
            RouteDirection routeDirection)
        {
            List<string> controllerNames =
                GetControllerNames().Select(c => c.RegexReplace("Controller$", string.Empty)).ToList();

            //Add area names to exclusion
            controllerNames.Add(MicroLoanConstants.TemplatesArea);
            controllerNames.Add(MicroLoanConstants.ApiArea);

            string value = values[routeKey].ToString();

            return !controllerNames.Contains(value, StringComparer.CurrentCultureIgnoreCase);
        }

        private static List<Type> GetSubClasses<T>()
        {
            return Assembly.GetCallingAssembly().GetTypes().Where(type => type.IsSubclassOf(typeof(T))).ToList();
        }

        private static IEnumerable<string> GetControllerNames()
        {
            var controllerNames = new List<string>();

            GetSubClasses<Controller>().ForEach(type => controllerNames.Add(type.Name));

            return controllerNames;
        }
    }
    public class NotInActionNames : IRouteConstraint
    {
        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values,
            RouteDirection routeDirection)
        {
            string cotrollerName = values["controller"].ToString();
            Type controllerType = GetControllerNames(cotrollerName);

            MethodInfo[] actions =
                controllerType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            List<string> actionNames = actions.Select(s => s.Name).ToList();
            string value = values[routeKey].ToString();

            return !actionNames.Contains(value, StringComparer.CurrentCultureIgnoreCase);
        }

        private static IEnumerable<Type> GetSubClasses<T>()
        {
            return Assembly.GetCallingAssembly().GetTypes().Where(type => type.IsSubclassOf(typeof(T))).ToList();
        }

        private static Type GetControllerNames(string controllerName)
        {
            IEnumerable<Type> controllers = GetSubClasses<Controller>();
            return controllers.First(c =>
                string.Equals(c.Name, $"{controllerName}controller", StringComparison.CurrentCultureIgnoreCase));
        }
    }
}
