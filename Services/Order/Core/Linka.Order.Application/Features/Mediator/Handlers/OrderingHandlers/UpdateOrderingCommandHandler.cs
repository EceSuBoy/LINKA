using Linka.Order.Application.Features.Mediator.Commands.OrderingCommands;
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
    public class UpdateOrderingCommandHandler : IRequestHandler<UpdateOrderingCommand>
    {
        private readonly IRepository<Ordering> _repository;

        public UpdateOrderingCommandHandler(IRepository<Ordering> repository)
        {
            _repository = repository;
        }

        public async Task Handle(UpdateOrderingCommand request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetByIdAsync(request.OrderingId);
            values.OrderDate=request.OrderDate;
            values.TotalPrice=request.TotalPrice;
            values.UserId=request.UserId;  
            await _repository.UpdateAsync(values);
        }
    }
}
