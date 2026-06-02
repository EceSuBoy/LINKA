namespace Linka.DtoLayer.OrderDtos
    .OrderOrderingDtos
{
    public class UpdateOrderingStatusDto
    {
        public int OrderingId { get; set; }

        public string OrderStatus { get; set; } =
            string.Empty;
    }
}