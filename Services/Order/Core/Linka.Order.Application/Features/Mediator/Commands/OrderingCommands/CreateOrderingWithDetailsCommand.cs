using MediatR;

namespace Linka.Order.Application.Features.Mediator
    .Commands.OrderingCommands
{
    public class CreateOrderingWithDetailsCommand
        : IRequest<int>
    {
        public string UserId { get; set; }
            = string.Empty;

        public decimal TotalPrice { get; set; }

        public List<CreateOrderingDetailItem>
            OrderDetails
        {
            get;
            set;
        } = new List<CreateOrderingDetailItem>();
    }

    public class CreateOrderingDetailItem
    {
        public string ProductId { get; set; }
            = string.Empty;

        public string ProductName { get; set; }
            = string.Empty;
        public decimal ProductPrice { get; set; }

        public int ProductAmount { get; set; }
    }
}