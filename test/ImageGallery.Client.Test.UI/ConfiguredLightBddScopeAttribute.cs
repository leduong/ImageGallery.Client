using System;
using ImageGallery.Client.Test.UI;
using LightBDD.Core.Configuration;
using LightBDD.Framework.Reporting.Configuration;
using LightBDD.Framework.Reporting.Formatters;
using LightBDD.XUnit2;

[assembly: ConfiguredLightBddScope]
namespace ImageGallery.Client.Test.UI
{
    internal class ConfiguredLightBddScopeAttribute : LightBddScopeAttribute
    {
        protected override void OnConfigure(LightBddConfiguration configuration)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
            string reportsPath = @"~\\..\\..\\..\\reports\\";

            if (env == "Testing" || env == "Staging")
            {
                reportsPath = string.Empty; //@"~//reports//";
            }

            Console.WriteLine($"LightBDD|ENV:{env}|ReportsPath:{reportsPath}");

            configuration
              .ReportWritersConfiguration()
              .AddFileWriter<XmlReportFormatter>(reportsPath + "{TestDateTimeUtc:yyyy-MM-dd-HH_mm_ss}_FeaturesReport.xml")
              .AddFileWriter<PlainTextReportFormatter>(reportsPath + "{TestDateTimeUtc:yyyy-MM-dd-HH_mm_ss}_FeaturesReport.txt")
              .AddFileWriter<HtmlReportFormatter>(reportsPath + "{TestDateTimeUtc:yyyy-MM-dd-HH_mm_ss}_FeaturesReport.html");
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
