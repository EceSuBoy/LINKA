using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linka.DtoLayer.CatalogDtos.CategoryDtos
{
    public class ResultCategoryDto
    {
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string ImageUrl { get; set; }

        public long ProductCount { get; set; }
    }
}
