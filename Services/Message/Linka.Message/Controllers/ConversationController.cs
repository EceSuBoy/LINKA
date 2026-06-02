using Linka.Message.Dtos;
using Linka.Message.Services;
using Microsoft.AspNetCore.Mvc;

namespace Linka.Message.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConversationsController : ControllerBase
    {
        private readonly IConversationService
            _conversationService;

        public ConversationsController(
            IConversationService conversationService)
        {
            _conversationService = conversationService;
        }

        [HttpPost("StartConversation")]
        public async Task<IActionResult> StartConversation(
            CreateConversationDto createConversationDto)
        {
            try
            {
                var value =
                    await _conversationService
                        .GetOrCreateConversationAsync(
                            createConversationDto);

                return Ok(value);
            }
            catch (ArgumentException exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpGet("GetConversationsByUserId/{userId}")]
        public async Task<IActionResult>
    GetConversationsByUserId(string userId)
        {
            try
            {
                var values =
                    await _conversationService
                        .GetConversationsByUserIdAsync(userId);

                return Ok(values);
            }
            catch (ArgumentException exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpGet("GetConversationById/{id}")]
        public async Task<IActionResult>
    GetConversationById(int id)
        {
            try
            {
                var value =
                    await _conversationService
                        .GetConversationByIdAsync(id);

                return Ok(value);
            }
            catch (ArgumentException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (KeyNotFoundException exception)
            {
                return NotFound(exception.Message);
            }
        }
    }
}