using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace ImageGallery.Client.Test.UI.Fixtures
{
    public class ConfigFixture : IDisposable
    {
        public ConfigFixture()
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
            Console.WriteLine("Config-ENV:" + env);

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            string appPath = Directory.GetCurrentDirectory();
            string artifactsPath = @"../../../../../artifacts";

            // Create Artifacts Directory
            ArtifactsDirectory = Path.GetFullPath(Path.Combine(appPath, artifactsPath));

            Navigator.Common.Helpers.FileHelper.CheckFilePath(ArtifactsDirectory);
            Console.WriteLine($"ArtifactsDirectory:{ArtifactsDirectory}");
        }

        public string ArtifactsDirectory { get; private set; }

        public void Dispose()
        {
        }
    }
}
