namespace Linka.WebUI.Models
{
    public class FeaturedProductViewModel
    {
        public string ProductId { get; set; } = string.Empty;

        public string ProductName { get; set; } = string.Empty;

        public decimal ProductPrice { get; set; }

        public string ProductImageUrl { get; set; } = string.Empty;

        public string ProductDescription { get; set; } = string.Empty;

        public string CategoryId { get; set; } = string.Empty;

        public int CommentCount { get; set; }

        public double AverageRating { get; set; }
    }
}
