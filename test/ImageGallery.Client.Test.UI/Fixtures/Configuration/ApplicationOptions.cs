namespace ImageGallery.Client.Test.UI.Fixtures.Configuration
{
    public class ApplicationOptions
    {
        public SeleniumConfig SeleniumConfig { get; set; }
    }

    public class SeleniumConfig
    {
       public string SeleniumHub { get; set; }
    }
}
