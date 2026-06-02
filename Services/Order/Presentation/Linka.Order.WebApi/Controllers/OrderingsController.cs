using Linka.Order.Application.Features.Mediator.Commands.OrderingCommands;
using Linka.Order.Application.Features.Mediator.Queries.OrderingQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Linka.Order.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderingsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderingsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> OrderingList()
        {
            var values = await _mediator.Send(new GetOrderingQuery());
            return Ok(values);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderingById(int id)
        {
            var values = await _mediator.Send(new GetOrderingByIdQuery(id));
            return Ok(values);
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrdering(CreateOrderingCommand command)
        {
            await _mediator.Send(command);
            return Ok("Order added successfully");
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveOrdering(int id)
        {
            await _mediator.Send(new RemoveOrderingCommand(id));
            return Ok("Order deleted successfully");
        }
        [HttpPut]
        public async Task<IActionResult> UpdateOrdering(UpdateOrderingCommand command)
        {
            await _mediator.Send(command);
            return Ok("Order updated successfully");
        }
        [HttpGet ("GetOrderingByUserId/{id}")]
        public async Task<IActionResult> GetOrderingByUserId(string id)
        {
            var values = await _mediator.Send(new GetOrderingByUserIdQuery(id));
            return Ok(values);
        }

        [HttpPost("CreateOrderingWithDetails")]
        public async Task<IActionResult>
    CreateOrderingWithDetails(
        CreateOrderingWithDetailsCommand command)
        {
            try
            {
                var orderingId =
                    await _mediator.Send(command);

                return Ok(orderingId);
            }
            catch (ArgumentException exception)
            {
                return BadRequest(
                    exception.Message);
            }
        }

        [HttpGet("GetOrderingDetail/{orderingId}/{userId}")]
        public async Task<IActionResult>
    GetOrderingDetail(
        int orderingId,
        string userId)
        {
            var value =
                await _mediator.Send(
                    new GetOrderingDetailQuery(
                        orderingId,
                        userId));

            if (value == null)
            {
                return NotFound(
                    "Order could not be found.");
            }

            return Ok(value);
        }
        [HttpPut("UpdateOrderingStatus")]
        public async Task<IActionResult>
    UpdateOrderingStatus(
        UpdateOrderingStatusCommand command)
        {
            try
            {
                var isUpdated =
                    await _mediator.Send(
                        command);

                if (!isUpdated)
                {
                    return NotFound(
                        "Order could not be found.");
                }

                return Ok(
                    "Order status updated successfully.");
            }
            catch (ArgumentException exception)
            {
                return BadRequest(
                    exception.Message);
            }
        }

        [HttpGet("GetAllOrdering")]
        public async Task<IActionResult>
    GetAllOrdering()
        {
            var values =
                await _mediator.Send(
                    new GetAllOrderingQuery());

            return Ok(values);
        }

    }
}
