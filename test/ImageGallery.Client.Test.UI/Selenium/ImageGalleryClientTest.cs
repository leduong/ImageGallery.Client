using System;
using System.IO;
using System.Reflection;
using ImageGallery.Client.Test.UI.Fixtures;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using Xunit;
using Xunit.Abstractions;

namespace ImageGallery.Client.Test.UI.Selenium
{
    public class ImageGalleryClientTest : IClassFixture<SeleniumFixture>
    {
        private readonly IWebDriver _driver;

        private readonly ITestOutputHelper _output;

        public ImageGalleryClientTest(SeleniumFixture fixture, ITestOutputHelper output)
        {
            _driver = fixture.Driver;
            _output = output;
        }

        [Fact]
        [Trait("Category", "Intergration")]
        public void ShouldLoadApplicationPage_SmokeTest()
        {
            _driver.Navigate().GoToUrl("https://www.google.com/webhp?ie=utf-8&oe=utf-8");

            Screenshot ss = ((ITakesScreenshot)_driver).GetScreenshot();

            ss.SaveAsFile($"D://SeleniumTestingScreenshot_{DateTime.Now.Ticks}.png", ScreenshotImageFormat.Png);

            Assert.Equal("Google", _driver.Title);
        }
    }
}
