using System.Collections.Generic;
using ImageGallery.Model;

namespace ImageGallery.Client.ViewModels
{
    public class GalleryIndexViewModel
    {
        public GalleryIndexViewModel(List<Image> images, string imagesUri)
        {
           Images = images;
           ImagesUri = imagesUri;
        }

        public IEnumerable<Image> Images { get; private set; }

        public string ImagesUri { get; private set; }
    }
}