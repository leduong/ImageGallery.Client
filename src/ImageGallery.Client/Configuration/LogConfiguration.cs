using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace ImageGallery.Client.Configuration
{
    public static class LogConfiguration
    {
        public static string GetLoggingPath(IConfiguration configuration)
        {
            LoggingConfiguration loggingConfiguration = new LoggingConfiguration();
            configuration.GetSection("LoggingConfiguration").Bind(loggingConfiguration);

            // Check if Path Exists => Create Directory
            Console.WriteLine("RollingFilePath:" + loggingConfiguration.RollingFilePath);
            if (!Directory.Exists(loggingConfiguration.RollingFilePath))
            {
                Directory.CreateDirectory(loggingConfiguration.RollingFilePath);
            }

            var logfile = $"ImageGallery.Client.{Environment.MachineName}.log";
            var logfilePath = Path.Combine(loggingConfiguration.RollingFilePath, logfile);

            return logfilePath;
        }
    }
}
