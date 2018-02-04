using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

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

            if (env == "Staging")
            {
                location = "/usr/local/bin/";
                Driver = SeleniumLocal(location);
            }

            if (env == "Local" || env == "Testing")
            {
                location = "/usr/local/bin/";
                // Driver = SeleniumGrid();
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
            var driverService = PhantomJSDriverService.CreateDefaultService(location);
            driverService.HideCommandPromptWindow = true;
            driverService.LoadImages = false;

            var options = new PhantomJSOptions();
            options.AddAdditionalCapability("IsJavaScriptEnabled", true);
            options.AddAdditionalCapability("phantomjs.page.settings.userAgent", "Mozilla / 5.0(Windows NT 6.1) AppleWebKit / 537.36(KHTML, like Gecko) Chrome / 40.0.2214.94 Safari / 537.36");

            var driver = new PhantomJSDriver(driverService, options);
            driver.Manage().Window.Size = new System.Drawing.Size(1280, 1024);

            return driver;
        }

        private IWebDriver SeleniumGrid()
        {
           IWebDriver _driver;

           DesiredCapabilities capabilities = new DesiredCapabilities();
           capabilities = DesiredCapabilities.Chrome();
           capabilities.SetCapability(CapabilityType.BrowserName, "chrome");
           capabilities.SetCapability(CapabilityType.Platform, new Platform(PlatformType.Windows));

           _driver = new RemoteWebDriver(new Uri("http://192.168.99.100:4444/wd/hub"), capabilities);

           return _driver;
        }
    }
}
