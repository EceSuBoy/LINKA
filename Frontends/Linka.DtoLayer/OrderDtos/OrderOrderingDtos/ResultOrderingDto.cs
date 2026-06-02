namespace Linka.DtoLayer.OrderDtos
    .OrderOrderingDtos
{
    public class ResultOrderingDto
    {
        public int OrderingId { get; set; }

        public string UserId { get; set; } =
            string.Empty;

        public decimal TotalPrice { get; set; }

        public DateTime OrderDate { get; set; }

        public string OrderStatus { get; set; } =
            string.Empty;
    }
}