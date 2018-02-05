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
    public class ImageGalleryClientTest : IClassFixture<ConfigFixture>, IClassFixture<SeleniumFixture>
    {
        private readonly IWebDriver _driver;

        private readonly ITestOutputHelper _output;

        private readonly string _artifactsDirectory;

        public ImageGalleryClientTest(ConfigFixture configFixture, SeleniumFixture fixture, ITestOutputHelper output)
        {
            _artifactsDirectory = configFixture.ArtifactsDirectory;
            _driver = fixture.Driver;
            _output = output;
        }

        [Fact]
        [Trait("Category", "Intergration")]
        public void ShouldLoadApplicationPage_SmokeTest()
        {
            _driver.Navigate().GoToUrl("https://www.google.com/webhp?ie=utf-8&oe=utf-8");

            Screenshot ss = ((ITakesScreenshot)_driver).GetScreenshot();

            string filePath = Path.Combine(_artifactsDirectory, $"Selenium_Smoke_Test_{DateTime.Now.Ticks}.png");
            ss.SaveAsFile(filePath, ScreenshotImageFormat.Png);

            Assert.Equal("Google", _driver.Title);
        }
    }
}
