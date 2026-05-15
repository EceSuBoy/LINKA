namespace Linka.DtoLayer.CatalogDtos.ProductImageDtos
{
    public class CreateProductImageDto
    {
        public List<string> Images { get; set; } = new List<string>();
        public string ProductId { get; set; }
    }
}
