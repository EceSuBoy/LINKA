using System.ComponentModel.DataAnnotations;

namespace Linka.Catalog.Dtos.ProductDtos
{
    public class UpdateProductDto
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        [Range(
            typeof(decimal),
            "0",
            "100",
            ErrorMessage = "Discount rate must be between 0 and 100.")]
        public decimal DiscountRate { get; set; }
        public string ProductImageUrl { get; set; }
        public string ProductDescription { get; set; }
        public string CategoryId { get; set; }
    }
}
