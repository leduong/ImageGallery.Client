using System.Collections.Generic;
using ImageGallery.Model;

namespace ImageGallery.Client.ViewModels
{
    /// <summary>
    ///
    /// </summary>
    public class AlbumIndexViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AlbumIndexViewModel"/> class.
        /// </summary>
        /// <param name="albums"></param>
        public AlbumIndexViewModel(List<Album> albums)
        {
            Albums = albums;
        }

        /// <summary>
        ///
        /// </summary>
        public IEnumerable<Album> Albums { get; private set; }
    }
}
