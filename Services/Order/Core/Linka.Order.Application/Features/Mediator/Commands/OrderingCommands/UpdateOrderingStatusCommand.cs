using MediatR;

namespace Linka.Order.Application.Features.Mediator
    .Commands.OrderingCommands
{
    public class UpdateOrderingStatusCommand
        : IRequest<bool>
    {
        public int OrderingId { get; set; }

        public string OrderStatus { get; set; }
            = string.Empty;
    }
}