using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace ImageGallery.Client.Test.UI.Pages
{
    public class GalleryPage : BaseSecurePage
    {
        public GalleryPage(IWebDriver driver)
            : base(driver)
        {
        }

        public GalleryPage(IWebDriver driver, string url)
            : base(driver, url)
        {
        }

        [FindsBy(How = How.LinkText, Using = "Add an image")]
        protected IWebElement AddImageButton { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".danger.validation-summary-errors")]
        protected IWebElement ValidationErrorText { get; set; }

        public bool IsAddImageButtonAvailable()
        {
            bool buttonIsAvailable;
            try
            {
                AddImageButton = LoadElement(nameof(AddImageButton));
                var text = AddImageButton.Text;
                buttonIsAvailable = true;
            }
            catch (NoSuchElementException)
            {
                buttonIsAvailable = false;
            }
            catch (WebDriverTimeoutException)
            {
                buttonIsAvailable = false;
            }

            return buttonIsAvailable;
        }


        public string GetValidationErrorText()
        {
            ValidationErrorText = LoadElement(nameof(ValidationErrorText));
            return ValidationErrorText.Text;
        }
    }
}
