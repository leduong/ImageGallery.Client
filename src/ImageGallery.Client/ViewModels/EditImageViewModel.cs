using System;
using System.ComponentModel.DataAnnotations;

namespace ImageGallery.Client.ViewModels
{
    /// <summary>
    ///
    /// </summary>
    public class EditImageViewModel
    {
        /// <summary>
        ///
        /// </summary>
        [Required]
        public string Title { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Required]
        public string Category { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Required]
        public Guid Id { get; set; }
    }
}
