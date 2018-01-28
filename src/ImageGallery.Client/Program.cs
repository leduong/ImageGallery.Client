using System;
using System.Diagnostics;
using System.IO;
using ImageGallery.Client.Configuration;
using Loggly;
using Loggly.Config;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;

namespace ImageGallery.Client
{
    public class Program
    {
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development"}.json", optional: true)
            .Build();

        public static int Main(string[] args)
        {
            //ConfigureLoggly();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Debug)
                .Enrich.FromLogContext()
                .WriteTo.RollingFile(LogConfiguration.GetLoggingPath(Configuration))
             //   .WriteTo.Loggly()
                .CreateLogger();

            Serilog.Debugging.SelfLog.Enable(msg =>
            {
                Debug.Print(msg);
                Debugger.Break();
            });

            try
            {
                Log.Information("Init:ImageGallery.Client");
                BuildWebHost(args).Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog()
                .UseSetting("detailedErrors", "true")
                .CaptureStartupErrors(true)
                .Build();

        private static void ConfigureLoggly()
        {
            var logglyToken = Environment.GetEnvironmentVariable("LOGGLY_TOKEN");
            Console.WriteLine($"LogglyToken:{logglyToken}");

            var config = LogglyConfig.Instance;
            config.CustomerToken = "c3176aed-1b75-4315-9ee6-21cf1bd84dd8";
            config.ApplicationName = "ImageGallery.Client";

            config.Transport.EndpointHostname = "logs-01.loggly.com";
            config.Transport.EndpointPort = 443;
            config.Transport.LogTransport = LogTransport.Https;

            var ct = new ApplicationNameTag
            {
                Formatter = "application-{0}",
            };

            config.TagConfig.Tags.Add(ct);
        }
    }
}
