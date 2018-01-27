using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using Loggly;
using Loggly.Config;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;
using Serilog.Core.Enrichers;
using Serilog.Enrichers;
using Serilog.Events;
using Serilog.Sinks.RollingFileAlternate;

namespace ImageGallery.Client
{
    public class Program
    {
        /// <summary>
        ///https://github.com/LeagueOfDevelopers/EduHub/blob/62817cb8d9ed95382f30ac26b6ca152a75fc647b/Backend/EduHub/Program.cs
        /// </summary>
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development"}.json", optional: true)
            .Build();

        private const string LogglyCustomerTokenEnvironmentVariableName = "LOGGLY_CUSTOMER_TOKEN";


        public static int Main(string[] args)
        {
            ConfigureLoggly();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Loggly()
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
                .Build();

        private static void ConfigureLoggly()
        {
            var customerToken = "c3176aed-1b75-4315-9ee6-21cf1bd84dd8";  //Environment.GetEnvironmentVariable(LogglyCustomerTokenEnvironmentVariableName);
            if (string.IsNullOrEmpty(customerToken))
                throw new ArgumentNullException(nameof(customerToken), $"No loggly customer token has been defined, please create the environment variable {LogglyCustomerTokenEnvironmentVariableName}");

            var config = LogglyConfig.Instance;
            config.CustomerToken = customerToken;
            config.ApplicationName = $"ImageGallery";

            config.Transport.EndpointHostname = "logs-01.loggly.com";
            config.Transport.EndpointPort = 6514;
            config.Transport.LogTransport = LogTransport.SyslogSecure;

            var ct = new ApplicationNameTag
            {
                Formatter = "application-{0}"
            };
            config.TagConfig.Tags.Add(ct);
        }




        static Logger CreateLogger(string logFilePath)
        {
            //write selflog to stderr
            Serilog.Debugging.SelfLog.Enable(Console.Error);

            return new LoggerConfiguration()
                .MinimumLevel.Debug()
                //Add enrichers
                .Enrich.FromLogContext()
                .Enrich.WithProcessId()
                .Enrich.WithThreadId()
                .Enrich.With(new EnvironmentUserNameEnricher())
                .Enrich.With(new MachineNameEnricher())
                .Enrich.With(new PropertyEnricher("Environment", "development"))
                //Add sinks
                .WriteTo.Async(s => s.Loggly(
                        bufferBaseFilename: logFilePath + "buffer",
                        formatProvider: CreateLoggingCulture())
                    .MinimumLevel.Information()
                )
                .WriteTo.Async(s => s.RollingFileAlternate(
                        logFilePath,
                        outputTemplate:
                        "[{ProcessId}] {Timestamp} [{ThreadId}] [{Level}] [{SourceContext}] [{Category}] {Message}{NewLine}{Exception}",
                        fileSizeLimitBytes: 10 * 1024 * 1024,
                        retainedFileCountLimit: 100,
                        formatProvider: CreateLoggingCulture())
                    .MinimumLevel.Debug()
                )
                .CreateLogger();
        }
        static CultureInfo CreateLoggingCulture()
        {
            var loggingCulture = new CultureInfo("");

            //with this DateTime and DateTimeOffset string representations will be sortable. By default, 
            // serialization without a culture or formater will use InvariantCulture. This may or may not be 
            // desirable, depending on the sorting needs you require or even the region your in. In this sample
            // the invariant culture is used as a base, but the DateTime format is changed to a specific representation.
            // Instead of the dd/MM/yyyy hh:mm:ss, we'll force yyyy-MM-dd HH:mm:ss.fff which is sortable and obtainable
            // by overriding ShortDatePattern and LongTimePattern.
            //
            //Do note that they don't include the TimeZone by default, so a datetime will not have the TZ
            // while a DateTimeOffset will in it's string representation. 
            // Both use the longTimePattern for time formatting, but including the time zone in the 
            // pattern will duplicate the TZ representation when using DateTimeOffset which serilog does
            // for the timestamp.
            //
            //If you do not require specific formats, this method will not be required. Just pass in null (the default) 
            // for IFormatProvider in the Loggly() sink configuration method. 
            loggingCulture.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";
            loggingCulture.DateTimeFormat.LongTimePattern = "HH:mm:ss.fff";

            return loggingCulture;
        }

        static void SetupLogglyConfiguration()
        {
            //CHANGE THESE TWO TO YOUR LOGGLY ACCOUNT: DO NOT COMMIT TO Source control!!!
            const string appName = "ImageGallery";
            const string customerToken = "c3176aed-1b75-4315-9ee6-21cf1bd84dd8";

            //Configure Loggly
            var config = LogglyConfig.Instance;
            config.CustomerToken = customerToken;
            config.ApplicationName = appName;
            config.Transport = new TransportConfiguration()
            {
                EndpointHostname = "logs-01.loggly.com",
                EndpointPort = 443,
                LogTransport = LogTransport.Https
            };
            config.ThrowExceptions = true;

            //Define Tags sent to Loggly
            config.TagConfig.Tags.AddRange(new ITag[]{
                new ApplicationNameTag {Formatter = "application-{0}"},
                new HostnameTag { Formatter = "host-{0}" }
            });
        }

    }
}
