using Linka.Order.Application.Features.Mediator
    .Queries.OrderingQueries;
using Linka.Order.Application.Features.Mediator
    .Results.OrderingResults;
using Linka.Order.Application.Interfaces;
using MediatR;

namespace Linka.Order.Application.Features.Mediator
    .Handlers.OrderingHandlers
{
    public class GetOrderingDetailQueryHandler
        : IRequestHandler<
            GetOrderingDetailQuery,
            GetOrderingDetailQueryResult?>
    {
        private readonly IOrderingRepository
            _orderingRepository;

        public GetOrderingDetailQueryHandler(
            IOrderingRepository orderingRepository)
        {
            _orderingRepository =
                orderingRepository;
        }

        public async Task<
            GetOrderingDetailQueryResult?>
            Handle(
                GetOrderingDetailQuery request,
                CancellationToken cancellationToken)
        {
            var ordering =
                await _orderingRepository
                    .GetOrderingWithDetailsByIdAsync(
                        request.OrderingId,
                        request.UserId);

            if (ordering == null)
            {
                return null;
            }

            return new GetOrderingDetailQueryResult
            {
                OrderingId =
                    ordering.OrderingId,

                UserId =
                    ordering.UserId,

                TotalPrice =
                    ordering.TotalPrice,

                OrderDate =
                    ordering.OrderDate,

                OrderStatus =
                    ordering.OrderStatus,

                OrderDetails =
                    ordering.OrderDetails
                        .Select(item =>
                            new GetOrderingDetailItemQueryResult
                            {
                                OrderDetailId =
                                    item.OrderDetailId,

                                ProductId =
                                    item.ProductId,

                                ProductName =
                                    item.ProductName,

                                ProductPrice =
                                    item.ProductPrice,

                                ProductAmount =
                                    item.ProductAmount,

                                ProductTotalPrice =
                                    item.ProductTotalPrice
                            })
                        .ToList()
            };
        }
    }
}