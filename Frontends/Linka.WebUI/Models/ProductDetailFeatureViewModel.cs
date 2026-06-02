namespace Linka.WebUI.Models
{
    public class ProductDetailFeatureViewModel
    {
        public string ProductId { get; set; } =
            string.Empty;

        public string ProductName { get; set; } =
            string.Empty;

        public decimal ProductPrice { get; set; }

        public decimal DiscountRate { get; set; }

        public string ProductDescription { get; set; } =
            string.Empty;

        public int CommentCount { get; set; }

        public double AverageRating { get; set; }

        public bool HasDiscount =>
            DiscountRate > 0 &&
            DiscountRate <= 100;

        public decimal DiscountedPrice =>
            HasDiscount
                ? ProductPrice -
                  (ProductPrice *
                   DiscountRate / 100m)
                : ProductPrice;
    }
}