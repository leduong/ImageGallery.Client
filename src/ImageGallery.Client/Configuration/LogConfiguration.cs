using System;
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

            string machineName = Environment.MachineName;

            return $"ImageGallery.Client.{machineName}.log";
        }
    }
}
