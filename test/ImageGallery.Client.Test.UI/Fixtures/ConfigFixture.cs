using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ImageGallery.Client.Test.UI.Fixtures
{
    public class ConfigFixture : IDisposable
    {
        public ConfigFixture()
        {
            Console.WriteLine("Pre-Config-ENV:" + Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));

            // Windows Build Get Environment Variables
            var propertiesfile = "Properties/launchSettings.json";
            if (File.Exists(propertiesfile))
            {
                using (var file = File.OpenText(propertiesfile))
                {
                    var reader = new JsonTextReader(file);
                    var jObject = JObject.Load(reader);

                    var variables = jObject
                        .GetValue("profiles")
                        //select a proper profile here
                        .SelectMany(profiles => profiles.Children())
                        .SelectMany(profile => profile.Children<JProperty>())
                        .Where(prop => prop.Name == "environmentVariables")
                        .SelectMany(prop => prop.Value.Children<JProperty>())
                        .ToList();

                    foreach (var variable in variables)
                    {
                        Environment.SetEnvironmentVariable(variable.Name, variable.Value.ToString());
                    }
                }
            }

            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
            Console.WriteLine("Config-ENV:" + env);

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            string appPath = Directory.GetCurrentDirectory();
            string artifactsPath = @"../../../../../artifacts";

            var appSetting = Configuration.GetSection("ApplicationSettings");
            ApplicationUrl = appSetting["ApplicationUrl"];

            // Create Artifacts Directory
            ArtifactsDirectory = Path.GetFullPath(Path.Combine(appPath, artifactsPath));

            Navigator.Common.Helpers.FileHelper.CheckFilePath(ArtifactsDirectory);
            Console.WriteLine($"ArtifactsDirectory:{ArtifactsDirectory}");
        }

        public string ArtifactsDirectory { get; private set; }

        public string ApplicationUrl { get; private set; }

        private IConfiguration Configuration { get; set; }

        public void Dispose()
        {
        }
    }
}
