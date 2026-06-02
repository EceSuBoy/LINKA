using Linka.Order.Application.Features.Mediator
    .Commands.OrderingCommands;
using Linka.Order.Application.Interfaces;
using Linka.Order.Domain.Entities;
using MediatR;

namespace Linka.Order.Application.Features.Mediator
    .Handlers.OrderingHandlers
{
    public class CreateOrderingWithDetailsCommandHandler
        : IRequestHandler<
            CreateOrderingWithDetailsCommand,
            int>
    {
        private readonly IOrderingRepository
            _orderingRepository;

        public CreateOrderingWithDetailsCommandHandler(
            IOrderingRepository orderingRepository)
        {
            _orderingRepository =
                orderingRepository;
        }

        public async Task<int> Handle(
            CreateOrderingWithDetailsCommand request,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(
                    request.UserId))
            {
                throw new ArgumentException(
                    "User ID cannot be empty.");
            }

            if (request.OrderDetails == null ||
                !request.OrderDetails.Any())
            {
                throw new ArgumentException(
                    "An order must contain at least one product.");
            }

            var ordering =
                new Ordering
                {       
                    UserId =
                        request.UserId,

                    TotalPrice =
                        request.TotalPrice,

                    OrderDate =
                        DateTime.Now,

                    OrderStatus =
                          "Paid",

                    OrderDetails =
                        request.OrderDetails
                            .Where(x =>
                                x.ProductAmount > 0)
                            .Select(x =>
                                new OrderDetail
                                {
                                    ProductId =
                                        x.ProductId,

                                    ProductName =
                                        x.ProductName,

                                    ProductPrice =
                                        x.ProductPrice,

                                    ProductAmount =
                                        x.ProductAmount,

                                    /*
                                     * Satır toplamını frontend'den
                                     * kabul etmiyoruz.
                                     */
                                    ProductTotalPrice =
                                        x.ProductPrice *
                                        x.ProductAmount
                                })
                            .ToList()
                };

            if (!ordering.OrderDetails.Any())
            {
                throw new ArgumentException(
                    "An order must contain at least one valid product.");
            }

            return await _orderingRepository
                .CreateOrderingWithDetailsAsync(
                    ordering);
        }
    }
}