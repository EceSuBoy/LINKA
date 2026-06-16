namespace Linka.WebUI.Models
{
    public class FeaturedProductViewModel
    {

        public decimal DiscountRate { get; set; }

        public bool HasDiscount =>
            DiscountRate > 0 &&
            DiscountRate <= 100;

        public decimal DiscountedPrice =>
            HasDiscount
                ? ProductPrice -
                  (ProductPrice * DiscountRate / 100m)
                : ProductPrice;
        public string ProductId { get; set; } = string.Empty;

        public string ProductName { get; set; } = string.Empty;

        public decimal ProductPrice { get; set; }
        public int StockCount { get; set; }

        public bool IsInStock =>
            StockCount > 0;

        public string ProductImageUrl { get; set; } = string.Empty;

        public string ProductDescription { get; set; } = string.Empty;

        public string CategoryId { get; set; } = string.Empty;

        public int CommentCount { get; set; }

        public double AverageRating { get; set; }
    }
}
