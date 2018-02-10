using System;
using System.IO;
using System.Reflection;
using ImageGallery.Client.Test.UI.Fixtures;
using ImageGallery.Client.Test.UI.Fixtures.TestData;
using ImageGallery.Client.Test.UI.Pages;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using Xunit;
using Xunit.Abstractions;

namespace ImageGallery.Client.Test.UI.Selenium
{
    public class ImageGalleryClientTest : IClassFixture<ConfigFixture>, IDisposable
    {
        private const string BasicUserName = "Frank";
        private const string BasicUserPassword = "password";
        private const string PrivilegedUserName = "Claire";
        private const string PrivilegedUserPassword = "password";
        private const string IncorrectPassword = "WRONG_PASSWORD";
        private const string LoginRequiredMessage = "The Username field is required.";
        private const string PasswordRequiredessage = "The Password field is required.";
        private const string InvalidLoginMessage = "Invalid username or password";

        private readonly IWebDriver _driver;

        private readonly ITestOutputHelper _output;

        private readonly string _artifactsDirectory;

        private readonly string _applicationUrl;

        public ImageGalleryClientTest(ConfigFixture config, ITestOutputHelper output)
        {
            SeleniumFixture fixture = new SeleniumFixture();
            _artifactsDirectory = config.ArtifactsDirectory;
            _applicationUrl = config.ApplicationUrl;
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

        [Fact]
        [Trait("Category", "UI")]
        public void BasicUserLoginTest()
        {
            using (var galleryPage = new GalleryPage(_driver, _applicationUrl))
            {
                galleryPage.Login(BasicUserName, BasicUserPassword);
                TakeScreenshot(galleryPage);

                Assert.False(
                    galleryPage.IsAddImageButtonAvailable(),
                    "User is logged in with elevated permissions, so 'Add Image' button should be available.");
            }
        }

        [Fact]
        [Trait("Category", "UI")]
        public void PrivilegedUserLoginTest()
        {
            using (var galleryPage = new GalleryPage(_driver, _applicationUrl))
            {
                galleryPage.Login(PrivilegedUserName, PrivilegedUserPassword);
                TakeScreenshot(galleryPage);

                Assert.True(
                    galleryPage.IsAddImageButtonAvailable(),
                    "User is logged in with elevated permissions, so 'Add Image' button should be available.");
            }
        }

        [Fact]
        [Trait("Category", "UI")]
        public void EmptyuUsernamePasswordTest()
        {
            using (var galleryPage = new GalleryPage(_driver, _applicationUrl))
            {
                galleryPage.Login(string.Empty, string.Empty);
                TakeScreenshot(galleryPage);

                var validationText = galleryPage.GetValidationErrorText();

                Assert.Contains(LoginRequiredMessage, validationText);
                Assert.Contains(PasswordRequiredessage, validationText);
            }
        }

        [Fact]
        [Trait("Category", "UI")]
        public void IncorrectLoginAttemptTest()
        {
            using (var galleryPage = new GalleryPage(_driver, _applicationUrl))
            {
                galleryPage.Login(BasicUserName, IncorrectPassword);
                TakeScreenshot(galleryPage);

                var validationText = galleryPage.GetValidationErrorText();

                Assert.Equal(InvalidLoginMessage, validationText);
            }
        }

        [Theory]
        [UserDataCsvData(FileName = "Data/users.csv")]
        [Trait("Category", "UI")]
        public void UserRolesTest(string userName, string password, string role)
        {
            using (var galleryPage = new GalleryPage(_driver, _applicationUrl))
            {
                galleryPage.Login(userName, password);
                TakeScreenshot(galleryPage);
                string actualRole = GetRole(galleryPage);

                Assert.Equal(role, actualRole);
            }
        }

        [Theory]
        [ImageDataCsvData(FileName = "Data/images.csv")]
        [Trait("Category", "UI")]
        public void GalleryImageAddRemoveTest(
            string userName,
            string password,
            string imageTitle,
            string imageType,
            string imageFilePath)
        {
            using (var galleryPage = new GalleryPage(_driver, _applicationUrl))
            {
                galleryPage.Login(userName, password);
                galleryPage.AddImageToGallery(imageTitle, imageType, imageFilePath);
                TakeScreenshot(galleryPage);
                var successMessage = galleryPage.GetSuccessMessage();
                Assert.Equal("Image has been added successfully!", successMessage);
            }
        }

        public void Dispose()
        {
            _driver.Quit();
            _driver?.Dispose();
        }

        private string GetRole(GalleryPage galleryPage)
        {
            return galleryPage.IsAddImageButtonAvailable() ? "basic" : "admin";
        }

        private void TakeScreenshot(BasePage page)
        {
            var filePath = Path.Combine(_artifactsDirectory, $"Selenium_Smoke_Test_{DateTime.Now.Ticks}.png");
            page.SaveScreenshotAs(filePath, ScreenshotImageFormat.Png);
        }
    }
}
