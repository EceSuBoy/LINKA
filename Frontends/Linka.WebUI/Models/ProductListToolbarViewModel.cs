namespace Linka.WebUI.Models
{
    public class ProductListToolbarViewModel
    {
        public string? CategoryId { get; set; }

        public decimal? MinPrice { get; set; }

        public decimal? MaxPrice { get; set; }

        public string Sort { get; set; } =
            "default";

        public int PageSize { get; set; } =
            10;

        public int TotalProductCount { get; set; }

        public bool DiscountedOnly { get; set; }
    }
}
