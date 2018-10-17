using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ImageGallery.Client.ViewModels
{
    /// <summary>
    ///
    /// </summary>
    public class AddImageViewModel
    {
        /// <summary>
        /// File.
        /// </summary>
        public IFormFile File { get; set; }

        /// <summary>
        /// Title.
        /// </summary>
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// Category.
        /// </summary>
        [Required]
        public string Category { get; set; }
    }
}
