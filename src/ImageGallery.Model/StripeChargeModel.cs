using System.ComponentModel.DataAnnotations;

namespace ImageGallery.Model
{
    public class StripeChargeModel
    {
        [Required]
        public string Token { get; set; }

        [Required]
        public double Amount { get; set; }

        public string Currency { get; set; } = "EUR";

        public string Description { get; set; }

        [EmailAddress]
        public string Email { get; set; }
    }
}
