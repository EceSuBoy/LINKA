using Linka.Order.Application.Features.Mediator
    .Queries.OrderingQueries;
using Linka.Order.Application.Features.Mediator
    .Results.OrderingResults;
using Linka.Order.Application.Interfaces;
using MediatR;

namespace Linka.Order.Application.Features.Mediator
    .Handlers.OrderingHandlers
{
    public class GetAllOrderingQueryHandler
        : IRequestHandler<
            GetAllOrderingQuery,
            List<GetAllOrderingQueryResult>>
    {
        private readonly IOrderingRepository
            _orderingRepository;

        public GetAllOrderingQueryHandler(
            IOrderingRepository orderingRepository)
        {
            _orderingRepository =
                orderingRepository;
        }

        public async Task<
            List<GetAllOrderingQueryResult>>
            Handle(
                GetAllOrderingQuery request,
                CancellationToken cancellationToken)
        {
            var values =
                await _orderingRepository
                    .GetAllOrderingsAsync();

            return values
                .Select(item =>
                    new GetAllOrderingQueryResult
                    {
                        OrderingId =
                            item.OrderingId,

                        UserId =
                            item.UserId,

                        TotalPrice =
                            item.TotalPrice,

                        OrderDate =
                            item.OrderDate,

                        OrderStatus =
                            item.OrderStatus
                    })
                .ToList();
        }
    }
}