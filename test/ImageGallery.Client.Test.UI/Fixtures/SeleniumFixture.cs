using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Remote;

namespace ImageGallery.Client.Test.UI.Fixtures
{
    public class SeleniumFixture : IDisposable
    {
        public SeleniumFixture()
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            Console.WriteLine("Selenium-ENV:" + env);

            var location = AppDomain.CurrentDomain.BaseDirectory;
            if (env == "Development")
            {
                Driver = new ChromeDriver(location);
            }

            if (env == "Local")
            {
                // Driver = SeleniumGrid();
                location = AppDomain.CurrentDomain.BaseDirectory;
                Driver = SeleniumLocal(location);
            }

            if (env == "Staging")
            {
                location = "/usr/local/bin/";
                Driver = SeleniumLocal(location);
            }

            if (env == "Testing")
            {
                /*
                 Selenium Grid - Remote Expermental
                */

                // Driver = SeleniumGrid();
                location = "/usr/local/bin/";
                Driver = SeleniumLocal(location);
            }
        }

        public IWebDriver Driver { get; set; }

        public void Dispose()
        {
            Driver.Quit();
            Driver.Dispose();
        }

        private IWebDriver SeleniumLocal(string location)
        {
            var driverService = ChromeDriverService.CreateDefaultService(location);
            driverService.HideCommandPromptWindow = true;

            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("headless");

            var driver = new ChromeDriver(driverService, chromeOptions);
            driver.Manage().Window.Size = new System.Drawing.Size(1280, 1024);

            return driver;
        }

        private IWebDriver SeleniumGrid()
        {
           var seleniumHub = "selenium_hub:4444";
           seleniumHub = "192.168.99.100:4444";

           Uri uri = new Uri($"http://{seleniumHub}/wd/hub");

           DesiredCapabilities capabilities = new DesiredCapabilities();
           capabilities.SetCapability(CapabilityType.BrowserName, "chrome");
           capabilities.SetCapability(CapabilityType.Version, "62.0.3202.94");
           capabilities.SetCapability(CapabilityType.Platform, "LINUX");

           IWebDriver driver = new RemoteWebDriver(uri, capabilities);

           return driver;
        }
    }
}
