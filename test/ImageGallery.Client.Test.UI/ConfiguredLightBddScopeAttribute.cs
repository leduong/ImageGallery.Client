using System;
using System.IO;
using ImageGallery.Client.Test.UI;
using LightBDD.Core.Configuration;
using LightBDD.Framework.Reporting.Configuration;
using LightBDD.Framework.Reporting.Formatters;
using LightBDD.XUnit2;
using Navigator.Common.Helpers;

[assembly: ConfiguredLightBddScope]
namespace ImageGallery.Client.Test.UI
{
    internal class ConfiguredLightBddScopeAttribute : LightBddScopeAttribute
    {
        protected override void OnConfigure(LightBddConfiguration configuration)
        {
            string appPath = Directory.GetCurrentDirectory();
            string reportsPath = @"../../../../../reports";
            string reportsDirectory = Path.GetFullPath(Path.Combine(appPath, reportsPath));

            Console.WriteLine($"ReportsDirectory:{reportsDirectory}");
            FileHelper.CheckFilePath(reportsDirectory);

            var x = "{TestDateTimeUtc:yyyy-MM-dd-HH_mm_ss}_FeaturesReport.txt";
            var y = Path.GetFullPath(Path.Combine(x, reportsDirectory));

            configuration
                .ReportWritersConfiguration()
                .AddFileWriter<PlainTextReportFormatter>(y);
        }


        protected override void OnSetUp()
        {
            // additional code that has to be run before any LightBDD tests
        }

        protected override void OnTearDown()
        {
            // additional code that has to be run after all LightBDD tests
        }
    }
}
