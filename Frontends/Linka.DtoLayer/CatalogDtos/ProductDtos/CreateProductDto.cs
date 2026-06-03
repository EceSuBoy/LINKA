using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linka.DtoLayer.CatalogDtos.ProductDtos
{
    public class CreateProductDto
    {
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        [Range(
            typeof(decimal),
            "0",
            "100",
            ErrorMessage = "Discount rate must be between 0 and 100.")]
        public decimal DiscountRate { get; set; }
        public string ProductImageUrl { get; set; }

        public bool IsFeatured { get; set; }

        public int FeaturedOrder { get; set; }
        public string ProductDescription { get; set; }
        public string CategoryId { get; set; }
    }
}
