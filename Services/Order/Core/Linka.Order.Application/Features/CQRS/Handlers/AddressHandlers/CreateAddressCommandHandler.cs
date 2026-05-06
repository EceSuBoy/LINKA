using Linka.Order.Application.Features.CQRS.Commands.AddressCommands;
using Linka.Order.Application.Interfaces;
using Linka.Order.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linka.Order.Application.Features.CQRS.Handlers.AddressHandlers
{
    public class CreateAddressCommandHandler 
    {
        private readonly IRepository<Address> _repository;
        public CreateAddressCommandHandler(IRepository<Address> repository)
        {
            _repository = repository;
        }
        public async Task Handle(CreateAddressCommand createAddressCommand)
        {
            await _repository.CreateAsync(new Address
            {
                UserId = createAddressCommand.UserId,
                District = createAddressCommand.District,
                City = createAddressCommand.City,
                Detail = createAddressCommand.Detail
            });
        }
    }
}
