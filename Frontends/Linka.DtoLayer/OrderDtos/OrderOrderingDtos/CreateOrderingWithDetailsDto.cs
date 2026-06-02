namespace Linka.DtoLayer.OrderDtos
    .OrderOrderingDtos
{
    public class CreateOrderingWithDetailsDto
    {
        public string UserId { get; set; }
            = string.Empty;

        public decimal TotalPrice { get; set; }

        public List<CreateOrderingDetailItemDto>
            OrderDetails
        {
            get;
            set;
        } = new List<CreateOrderingDetailItemDto>();
    }

    public class CreateOrderingDetailItemDto
    {
        public string ProductId { get; set; }
            = string.Empty;

        public string ProductName { get; set; }
            = string.Empty;

        public decimal ProductPrice { get; set; }

        public int ProductAmount { get; set; }
    }
}