using System.Collections.Generic;
using ImageGallery.Model;

namespace ImageGallery.Client.ViewModels
{
    /// <summary>
    ///
    /// </summary>
    public class GalleryIndexViewModel
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="images"></param>
        /// <param name="imagesUri"></param>
        public GalleryIndexViewModel(List<Image> images, string imagesUri)
        {
            Images = images;
            ImagesUri = imagesUri;
        }

        /// <summary>
        ///
        /// </summary>
        public IEnumerable<Image> Images { get; private set; }

        /// <summary>
        ///
        /// </summary>
        public string ImagesUri { get; private set; }
    }
}
