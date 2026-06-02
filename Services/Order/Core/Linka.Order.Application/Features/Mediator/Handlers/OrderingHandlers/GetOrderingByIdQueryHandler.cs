using Linka.Order.Application.Features.Mediator.Queries.OrderingQueries;
using Linka.Order.Application.Features.Mediator.Results.OrderingResults;
using Linka.Order.Application.Interfaces;
using Linka.Order.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linka.Order.Application.Features.Mediator.Handlers.OrderingHandlers
{
    public class GetOrderingByIdQueryHandler : IRequestHandler<GetOrderingByIdQuery, GetOrderingByIdQueryResult>
    {
        private readonly IRepository<Ordering> _repository;

        public GetOrderingByIdQueryHandler(IRepository<Ordering> repository)
        {
            _repository = repository;
        }

        public async Task<GetOrderingByIdQueryResult> Handle(GetOrderingByIdQuery request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetByIdAsync(request.Id);
            return new GetOrderingByIdQueryResult
            {
                OrderingId = values.OrderingId,
                UserId = values.UserId,
                TotalPrice = values.TotalPrice,
                OrderDate = values.OrderDate,
                OrderStatus =
                            values.OrderStatus,
            };
        }
            
    }
}
