using System.ComponentModel.DataAnnotations;

namespace Linka.WebUI.Models
{
    public class PaymentViewModel
    {
        [Required(
            ErrorMessage =
                "Please enter the card holder name.")]
        [StringLength(
            80,
            MinimumLength = 3,
            ErrorMessage =
                "Card holder name must contain at least 3 characters.")]
        public string CardHolderName { get; set; } =
            string.Empty;

        [Required(
            ErrorMessage =
                "Please enter a card number.")]
        [RegularExpression(
            @"^(?:\d{4}\s?){3}\d{4}$",
            ErrorMessage =
                "Card number must contain 16 digits.")]
        public string CardNumber { get; set; } =
            string.Empty;

        [Required(
            ErrorMessage =
                "Please select an expiration month.")]
        public int? ExpirationMonth { get; set; }

        [Required(
            ErrorMessage =
                "Please select an expiration year.")]
        public int? ExpirationYear { get; set; }

        [Required(
            ErrorMessage =
                "Please enter the CVV.")]
        [RegularExpression(
            @"^\d{3,4}$",
            ErrorMessage =
                "CVV must contain 3 or 4 digits.")]
        public string Cvv { get; set; } =
            string.Empty;
    }
}