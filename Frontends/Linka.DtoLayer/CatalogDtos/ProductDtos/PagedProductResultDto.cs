namespace Linka.DtoLayer.CatalogDtos.ProductDtos
{
    public class PagedProductResultDto<T>
    {
        public List<T> Items { get; set; } =
            new List<T>();

        public int Page { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public int TotalPages { get; set; }

        public string Search { get; set; } =
            string.Empty;

        public string CategoryId { get; set; } =
            string.Empty;
    }
}