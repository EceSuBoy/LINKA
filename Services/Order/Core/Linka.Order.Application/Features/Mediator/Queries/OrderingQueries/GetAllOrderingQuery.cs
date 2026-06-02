using Linka.Order.Application.Features.Mediator
    .Results.OrderingResults;
using MediatR;

namespace Linka.Order.Application.Features.Mediator
    .Queries.OrderingQueries
{
    public class GetAllOrderingQuery
        : IRequest<List<GetAllOrderingQueryResult>>
    {
    }
}