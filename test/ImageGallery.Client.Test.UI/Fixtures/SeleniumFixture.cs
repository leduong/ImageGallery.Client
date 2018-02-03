using System;
using System.IO;
using System.Reflection;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Support.UI;

namespace ImageGallery.Client.Test.UI.Fixtures
{
    public class SeleniumFixture : IDisposable
    {
        public SeleniumFixture()
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
            Console.WriteLine("Selenium-ENV:" + env);

            //var location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            //AppDomain.CurrentDomain.BaseDirectory

            var location = "/usr/local/bin/";

            var driverService = PhantomJSDriverService.CreateDefaultService(location);
            driverService.HideCommandPromptWindow = true;
            driverService.LoadImages = false;

            var options = new PhantomJSOptions();
            options.AddAdditionalCapability("IsJavaScriptEnabled", true);

            options.AddAdditionalCapability("phantomjs.page.settings.userAgent", "Mozilla / 5.0(Windows NT 6.1) AppleWebKit / 537.36(KHTML, like Gecko) Chrome / 40.0.2214.94 Safari / 537.36");

            Driver = new PhantomJSDriver(driverService, options);
            Driver.Manage().Window.Size = new System.Drawing.Size(1280, 1024);




            // Driver = new ChromeDriver(location);

            //Driver = new PhantomJSDriver(location,);
        }

        public IWebDriver Driver { get; set; }

        public void Dispose()
        {
            Driver.Quit();
            Driver.Dispose();
        }
    }
}
