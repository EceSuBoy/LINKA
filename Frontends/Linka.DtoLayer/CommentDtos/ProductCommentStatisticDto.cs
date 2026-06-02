using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linka.DtoLayer.CommentDtos
{
    public class ProductCommentStatisticDto
    {
        public string ProductId { get; set; }

        public int CommentCount { get; set; }

        public double AverageRating { get; set; }
    }
}
