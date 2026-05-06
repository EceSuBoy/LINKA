using Linka.Order.Application.Features.CQRS.Results.OrderDetailResults;
using Linka.Order.Application.Interfaces;
using Linka.Order.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linka.Order.Application.Features.CQRS.Handlers.OrderDetailHandlers
{
    public class GetOrderDetailQueryHandler
    {
        private readonly IRepository<OrderDetail> _repository;

        public GetOrderDetailQueryHandler(IRepository<OrderDetail> repository)
        {
            _repository = repository;
        }
        public async Task<List<GetOrderDetailQueryResult>> Handle()
        {
            var value = await _repository.GetAllAsync();
            return value.Select(x => new GetOrderDetailQueryResult
            {
                OrderDetailId = x.OrderDetailId,
                ProductId = x.ProductId,
                ProductName = x.ProductName,    
                ProductPrice = x.ProductPrice,
                ProductAmount = x.ProductAmount,
                ProductTotalPrice = x.ProductTotalPrice,
                OrderingId = x.OrderingId
            }).ToList();
        }
    }
}
