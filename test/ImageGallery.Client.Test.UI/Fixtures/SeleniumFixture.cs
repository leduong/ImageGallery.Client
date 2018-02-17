using System;
using System.IO;
using ImageGallery.Client.Test.UI.Fixtures.Configuration;
using ImageGallery.Client.Test.UI.Fixtures.Patch;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;

namespace ImageGallery.Client.Test.UI.Fixtures
{
    public class SeleniumFixture : IDisposable
    {
        private readonly SeleniumConfig _seleniumConfig;

        public SeleniumFixture()
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
            Console.WriteLine($"Selenium-Fixture-ENV:{env}");

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env}.json", optional: true)
                .AddEnvironmentVariables();

            IConfiguration configuration = builder.Build();

            var seleniumConfig = new SeleniumConfig();
            configuration.GetSection("SeleniumConfig").Bind(seleniumConfig);
            _seleniumConfig = seleniumConfig;

            Console.WriteLine("Selenium-ENV:" + env);
            Console.WriteLine("Selenium-Hub:" + _seleniumConfig.SeleniumHub);

            var location = AppDomain.CurrentDomain.BaseDirectory;
            if (env == "Development")
            {
                Driver = new FirefoxDriver(location);
            }

            if (env == "Local")
            {
                 Driver = SeleniumGrid();
            }

            if (env == "Staging")
            {
                location = "/usr/local/bin/";
                Driver = SeleniumLocal(location);
            }

            if (env == "Testing")
            {
                Console.WriteLine("Running Selenium Testing");
                /*
                 Selenium Grid - Remote Expermental
                */

                // Driver = SeleniumGrid();
                location = "/usr/local/bin/";
                Console.WriteLine(location);
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
            var driverService = FirefoxDriverService.CreateDefaultService("/usr/local/bin/");
            driverService.HideCommandPromptWindow = true;

            var firefoxOptions = new FirefoxOptions();
            firefoxOptions.AddArgument("--headless");

            var driver = new FirefoxDriver(driverService, firefoxOptions, TimeSpan.FromSeconds(10));
            driver.Manage().Window.Size = new System.Drawing.Size(1280, 1024);

            return driver;
        }

        private IWebDriver SeleniumGrid()
        {
            var seleniumHub = _seleniumConfig.SeleniumHub;
            Uri uri = new Uri($"http://{seleniumHub}/wd/hub");

            /*
             See: Work Arround
             https://github.com/SeleniumHQ/selenium/issues/4770
            */

            var executor = new NetCoreHttpCommandExecutor(uri, new TimeSpan(0, 0, 0, 20));
            var capabilities = new ChromeOptions().ToCapabilities();
            IWebDriver driver = new RemoteWebDriver(executor, capabilities);

            return driver;
        }
    }
}
