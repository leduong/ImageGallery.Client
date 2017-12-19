using System;
using System.Collections.Generic;
using ImageGallery.Client.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using ImageGallery.Client.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using ConfigurationOptions = ImageGallery.Client.Configuration.ConfigurationOptions;

namespace ImageGallery.Client
{
    public class Startup
    {
        public Startup(IHostingEnvironment env, IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddOptions();
            services.Configure<ConfigurationOptions>(Configuration);

            services.Configure<ConfigurationOptions>(Configuration.GetSection("applicationSettings"));
            services.Configure<Dataprotection>(Configuration.GetSection("dataprotection"));
            services.Configure<OpenIdConnectConfiguration>(Configuration.GetSection("openIdConnectConfiguration"));

            var config = Configuration.Get<ConfigurationOptions>();

            Console.WriteLine($"Dataprotection Enabled: {config.Dataprotection.Enabled}");
            Console.WriteLine($"DataprotectionRedis: {config.Dataprotection.RedisConnection}");
            Console.WriteLine($"RedisKey: {config.Dataprotection.RedisKey}");

            if (config.Dataprotection.Enabled)
            {
                var redis = ConnectionMultiplexer.Connect(config.Dataprotection.RedisConnection);
                services.AddDataProtection().PersistKeysToRedis(redis, config.Dataprotection.RedisKey);
            }

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info
                {
                    Title = "ImageGallery.Client",
                    Description = "ImageGallery.Client",
                    Version = "v1"
                });
            });

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
                            config.OpenIdConnectConfiguration.ClientId
                        }
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
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            Console.WriteLine($"EnvironmentName: {env.EnvironmentName}");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true,
                    HotModuleReplacementEndpoint = "/dist/__webpack_hmr"
                });
            }
            else
            {
                app.UseExceptionHandler("/Shared/Error");
            }

            var forwardOptions = new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
                RequireHeaderSymmetry = false
            };

            forwardOptions.KnownNetworks.Clear();
            forwardOptions.KnownProxies.Clear();

            app.UseForwardedHeaders(forwardOptions);

            #region Moved to Configure Services 

            //app.UseCookieAuthentication(new CookieAuthenticationOptions
            //{
            //    AuthenticationScheme = "Cookies",
            //    AccessDeniedPath = "/Authorization/AccessDenied"
            //});

            #endregion 


            var config = Configuration.Get<ConfigurationOptions>();
            Console.WriteLine("Authority" + config.OpenIdConnectConfiguration.Authority);

            #region Moved
            //app.UseOpenIdConnectAuthentication(new OpenIdConnectOptions
            //{
            //    RequireHttpsMetadata = false,

            //    AuthenticationScheme = "oidc",
            //    Authority = config.OpenIdConnectConfiguration.Authority, 
            //    ClientId = config.OpenIdConnectConfiguration.ClientId,
            //    Scope = { "openid", "profile", "address", "roles", "imagegalleryapi", "subscriptionlevel", "country", "offline_access" },
            //    ResponseType = "code id_token",
            //    // CallbackPath = new PathString("...")
            //    //SignedOutCallbackPath = new PathString("")
            //    SignInScheme = "Cookies",
            //    SaveTokens = true,
            //    ClientSecret = config.OpenIdConnectConfiguration.ClientSecret,
            //    GetClaimsFromUserInfoEndpoint = true,
            //    Events = new OpenIdConnectEvents()
            //    {
            //        OnTokenValidated = tokenValidatedContext =>
            //        {
            //            var identity = tokenValidatedContext.Ticket.Principal.Identity
            //                as ClaimsIdentity;

            //            var subjectClaim = identity.Claims.FirstOrDefault(z => z.Type == "sub");

            //            var newClaimsIdentity = new ClaimsIdentity(
            //                tokenValidatedContext.Ticket.AuthenticationScheme,
            //                "given_name",
            //                "role");

            //            newClaimsIdentity.AddClaim(subjectClaim);

            //            tokenValidatedContext.Ticket = new AuthenticationTicket(
            //                new ClaimsPrincipal(newClaimsIdentity),
            //                tokenValidatedContext.Ticket.Properties,
            //                tokenValidatedContext.Ticket.AuthenticationScheme);

            //            return Task.FromResult(0);
            //        },
            //        OnUserInformationReceived = userInformationReceivedContext =>
            //        {
            //            userInformationReceivedContext.User.Remove("address");
            //            return Task.FromResult(0);
            //        }
            //    }
            //});
            #endregion

            app.UseAuthentication();

            app.UseStaticFiles();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "ImageGallery.Client V1");
                options.ConfigureOAuth2("swaggerui", "", "", "Swagger UI");
            });

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
    }
}
