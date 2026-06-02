using Linka.Order.Application.Constants;
using Linka.Order.Application.Features.Mediator
    .Commands.OrderingCommands;
using Linka.Order.Application.Interfaces;
using MediatR;

namespace Linka.Order.Application.Features.Mediator
    .Handlers.OrderingHandlers
{
    public class UpdateOrderingStatusCommandHandler
        : IRequestHandler<
            UpdateOrderingStatusCommand,
            bool>
    {
        private readonly IOrderingRepository
            _orderingRepository;

        public UpdateOrderingStatusCommandHandler(
            IOrderingRepository orderingRepository)
        {
            _orderingRepository =
                orderingRepository;
        }

        public async Task<bool> Handle(
            UpdateOrderingStatusCommand request,
            CancellationToken cancellationToken)
        {
            if (request.OrderingId <= 0)
            {
                throw new ArgumentException(
                    "Order ID must be greater than zero.");
            }

            if (!OrderStatusValues.IsValid(
                    request.OrderStatus))
            {
                throw new ArgumentException(
                    "Invalid order status. " +
                    "Allowed values: Paid, Preparing, " +
                    "Shipped, Delivered, Cancelled.");
            }

            var normalizedStatus =
                OrderStatusValues.Normalize(
                    request.OrderStatus);

            return await _orderingRepository
                .UpdateOrderingStatusAsync(
                    request.OrderingId,
                    normalizedStatus);
        }
    }
}