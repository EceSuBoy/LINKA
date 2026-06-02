using Linka.Order.Application.Features.Mediator
    .Results.OrderingResults;
using MediatR;

namespace Linka.Order.Application.Features.Mediator
    .Queries.OrderingQueries
{
    public class GetOrderingDetailQuery
        : IRequest<GetOrderingDetailQueryResult?>
    {
        public int OrderingId { get; set; }

        public string UserId { get; set; } =
            string.Empty;

        public GetOrderingDetailQuery(
            int orderingId,
            string userId)
        {
            OrderingId =
                orderingId;

            UserId =
                userId;
        }
    }
}