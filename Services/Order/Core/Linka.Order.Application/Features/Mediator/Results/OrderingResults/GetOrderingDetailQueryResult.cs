namespace Linka.Order.Application.Features.Mediator
    .Results.OrderingResults
{
    public class GetOrderingDetailQueryResult
    {
        public int OrderingId { get; set; }

        public string UserId { get; set; } =
            string.Empty;

        public decimal TotalPrice { get; set; }

        public DateTime OrderDate { get; set; }

        public string OrderStatus { get; set; } =
            string.Empty;

        public List<GetOrderingDetailItemQueryResult>
            OrderDetails
        {
            get;
            set;
        } = new List<GetOrderingDetailItemQueryResult>();
    }

    public class GetOrderingDetailItemQueryResult
    {
        public int OrderDetailId { get; set; }

        public string ProductId { get; set; } =
            string.Empty;

        public string ProductName { get; set; } =
            string.Empty;

        public decimal ProductPrice { get; set; }

        public int ProductAmount { get; set; }

        public decimal ProductTotalPrice { get; set; }
    }
}