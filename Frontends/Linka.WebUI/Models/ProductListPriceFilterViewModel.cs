namespace Linka.WebUI.Models
{
    public class ProductListPriceFilterViewModel
    {
        public string? CategoryId { get; set; }

        public decimal? SelectedMinPrice { get; set; }

        public decimal? SelectedMaxPrice { get; set; }

        public string Sort { get; set; } = "default";

        public int PageSize { get; set; } = 10;

        public List<ProductListPriceRangeViewModel>
            PriceRanges
        {
            get;
            set;
        } = new List<ProductListPriceRangeViewModel>();

        public bool DiscountedOnly { get; set; }
    }

    public class ProductListPriceRangeViewModel
    {
        public string Label { get; set; } =
            string.Empty;

        public decimal? MinPrice { get; set; }

        public decimal? MaxPrice { get; set; }

        public int ProductCount { get; set; }  
    }
}