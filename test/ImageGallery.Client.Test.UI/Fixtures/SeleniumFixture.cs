using System;
using System.IO;
using System.Reflection;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace ImageGallery.Client.Test.UI.Fixtures
{
    public class SeleniumFixture : IDisposable
    {
        public SeleniumFixture()
        {
            var location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Driver = new ChromeDriver(location);
        }

        public IWebDriver Driver { get; set; }

        public void Dispose()
        {
            Driver.Quit();
            Driver.Dispose();
        }
    }
}
