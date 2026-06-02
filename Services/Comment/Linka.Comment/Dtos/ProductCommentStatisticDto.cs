namespace Linka.Comment.Dtos
{
    public class ProductCommentStatisticDto
    {
        public string ProductId { get; set; }

        public int CommentCount { get; set; }

        public double AverageRating { get; set; }
    }
}
