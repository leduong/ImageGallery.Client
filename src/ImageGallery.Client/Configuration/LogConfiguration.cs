using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace ImageGallery.Client.Configuration
{
    /// <summary>
    ///
    /// </summary>
    public static class LogConfiguration
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static string GetLoggingPath(IConfiguration configuration)
        {
            LoggingConfiguration loggingConfiguration = new LoggingConfiguration();
            configuration.GetSection("LoggingConfiguration").Bind(loggingConfiguration);

            Console.WriteLine("RollingFilePath:" + loggingConfiguration.RollingFilePath);
            if (!Directory.Exists(loggingConfiguration.RollingFilePath))
            {
                Directory.CreateDirectory(loggingConfiguration.RollingFilePath);
            }

            var localLogFilePath = $"ImageGallery.Client.{Environment.MachineName}";
            var logfilePath = Path.Combine(loggingConfiguration.RollingFilePath, localLogFilePath);

            return logfilePath;
        }
    }
}
