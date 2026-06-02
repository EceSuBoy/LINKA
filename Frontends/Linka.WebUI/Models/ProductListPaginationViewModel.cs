namespace Linka.WebUI.Models
{
    public class ProductListPaginationViewModel
    {
        public string? CategoryId { get; set; }

        public decimal? MinPrice { get; set; }

        public decimal? MaxPrice { get; set; }

        public string Sort { get; set; } =
            "default";

        public int CurrentPage { get; set; }

        public int PageSize { get; set; }

        public int TotalProductCount { get; set; }

        public int TotalPages { get; set; }
    }
}
