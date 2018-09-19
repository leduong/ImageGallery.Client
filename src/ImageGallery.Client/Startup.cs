using System;
using System.Collections.Generic;
using System.IO;
using ImageGallery.Client.Configuration;
using ImageGallery.Client.Services;
using Loggly;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using StackExchange.Redis;
using Swashbuckle.AspNetCore.Swagger;

namespace ImageGallery.Client
{
    /// <summary>
    ///
    /// </summary>
    public class Startup
    {
        /// <summary />
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Log.Information("Startup:ImageGallery.Client");
            Configuration = configuration;
        }

        /// <summary>
        ///
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddSerilog();
            });

            Log.Information("ConfigureServices:ImageGallery.Client");

            services.AddMvc();

            services.AddOptions();
            services.Configure<ApplicationOptions>(Configuration);
            services.Configure<ApplicationOptions>(Configuration.GetSection("applicationSettings"));

            var config = Configuration.Get<ApplicationOptions>();
            Console.WriteLine($"Dataprotection Enabled: {config.Dataprotection.Enabled}");
            Console.WriteLine($"Dataprotection Redis: {config.Dataprotection.RedisConnection}");
            Console.WriteLine($"RedisKey: {config.Dataprotection.RedisKey}");

            Console.WriteLine($"ApiAttractionsUri: {config.ClientConfiguration.ApiAttractionsUri}");

            Console.WriteLine($"Authority: {config.OpenIdConnectConfiguration.Authority}");
            Console.WriteLine($"ClientId: {config.OpenIdConnectConfiguration.ClientId}");
            Console.WriteLine($"ClientSecret: {config.OpenIdConnectConfiguration.ClientSecret}");
            Console.WriteLine($"LogglyKey: {config.LogglyClientConfiguration.LogglyKey}");

            if (config.Dataprotection.Enabled)
            {
                var redis = ConnectionMultiplexer.Connect(config.Dataprotection.RedisConnection);
                services.AddDataProtection().PersistKeysToRedis(redis, config.Dataprotection.RedisKey);
            }

            if (config.SwaggerUiConfiguration.Enabled)
            {
                services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("v1", new Info
                    {
                        Title = "ImageGallery.Client",
                        Description = "ImageGallery.Client",
                        Version = "v1",
                    });

                    // Handle OAuth
                    options.AddSecurityDefinition("oauth2", new OAuth2Scheme
                    {
                        Type = "oauth2",
                        Flow = "implicit",
                        AuthorizationUrl = ValidateUrl($"{config.OpenIdConnectConfiguration.Authority}/connect/authorize"),
                        TokenUrl = ValidateUrl($"{config.OpenIdConnectConfiguration.Authority}/connect/token"),
                        Scopes = new Dictionary<string, string>()
                        {
                            { "imagegalleryapi", "Image Gallery API" },
                        },
                    });
                    options.IncludeXmlComments(GetXmlCommentsPath(PlatformServices.Default.Application));
                });
            }

            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.Authority = config.OpenIdConnectConfiguration.Authority;
                    options.RequireHttpsMetadata = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = true,
                        ValidAudiences = new List<string>
                        {
                            $"{config.OpenIdConnectConfiguration.Authority}/resources",
                            config.OpenIdConnectConfiguration.ClientId,
                        },
                    };
                });

            services.AddAuthorization(authorizationOptions =>
            {
                authorizationOptions.AddPolicy(
                    "CanOrderFrame",
                    policyBuilder =>
                    {
                        policyBuilder.RequireAuthenticatedUser();
                        policyBuilder.RequireClaim("country", "be");
                        policyBuilder.RequireClaim("subscriptionlevel", "PayingUser");
                        policyBuilder.RequireRole("PayingUser");
                    });
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IImageGalleryHttpClient, ImageGalleryHttpClient>();

            services.AddSingleton<ILogglyClient, LogglyClient>();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="loggerFactory"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true,
                    HotModuleReplacementEndpoint = "/dist/__webpack_hmr",
                });
            }
            else
            {
                app.UseExceptionHandler("/Shared/Error");
            }

            var forwardOptions = new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
                RequireHeaderSymmetry = false,
            };

            forwardOptions.KnownNetworks.Clear();
            forwardOptions.KnownProxies.Clear();

            app.UseForwardedHeaders(forwardOptions);

            var config = Configuration.Get<ApplicationOptions>();

            app.UseAuthentication();
            app.UseStaticFiles();

            if (config.SwaggerUiConfiguration.Enabled)
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "ImageGallery.Client V1");
                    options.ConfigureOAuth2(config.SwaggerUiConfiguration.ClientId, string.Empty, string.Empty, "Swagger UI");
                });
            }

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Gallery}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Gallery", action = "Index" });
            });
        }

        private static string GetXmlCommentsPath(ApplicationEnvironment appEnvironment)
        {
            var documentationFile = $"{appEnvironment.ApplicationName}.xml";
            return Path.Combine(appEnvironment.ApplicationBasePath, documentationFile);
        }

        private string ValidateUrl(string url)
        {
            // ADD LOGIC - THROW EXCEPTION IF NOT VALID
            return url;
        }
    }
}
