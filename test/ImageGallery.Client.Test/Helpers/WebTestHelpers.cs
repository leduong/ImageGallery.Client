using System.IO;

namespace ImageGallery.Client.Test.Helpers
{
    public static class WebTestHelpers
    {
        public static string GetWebApplicationPath()
        {
            string appPath = Directory.GetCurrentDirectory();
            string webPath = @"../../../../../src/ImageGallery.Client";
            string path = Path.GetFullPath(Path.Combine(appPath, webPath));

            return path;
        }
    }
}
