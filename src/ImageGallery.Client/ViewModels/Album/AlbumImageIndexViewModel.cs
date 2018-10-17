using System.Collections.Generic;
using ImageGallery.Model;

namespace ImageGallery.Client.ViewModels.Album
{
    /// <summary>
    ///
    /// </summary>
    public class AlbumImageIndexViewModel
    {
        public AlbumImageIndexViewModel(List<AlbumImage> images, string imagesUri)
        {
            Images = images;
            ImagesUri = imagesUri;
        }

        /// <summary>
        ///
        /// </summary>
        public IEnumerable<AlbumImage> Images { get; private set; }

        /// <summary>
        /// Image Root Folder. 
        /// </summary>
        public string ImagesUri { get; private set; }
    }
}
