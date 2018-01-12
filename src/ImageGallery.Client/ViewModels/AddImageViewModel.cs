using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ImageGallery.Client.ViewModels
{
    public class AddImageViewModel
    {
        public IFormFile File { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Category { get; set; }
    }
}
