using Linka.Message.Dtos;
using Linka.Message.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Linka.Message.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserMessageController : ControllerBase
    {
        private readonly IUserMessageService _userMessageService;

        public UserMessageController(IUserMessageService userMessageService)
        {
            _userMessageService = userMessageService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMessage()
        {
            var values = await _userMessageService.GetAllMessageAsync();
            return Ok(values);

        }
        [HttpGet("GetMessageSendBox")]
        public async Task<IActionResult> GetMessageSendBox(string id)
        {
            var values = await _userMessageService.GetSendboxMessageAsync(id);
            return Ok(values);
        }

        [HttpGet("GetMessageInBox")]
        public async Task<IActionResult> GetMessageInBox(string id)
        {
            var values = await _userMessageService.GetInboxMessageAsync(id);
            return Ok(values);
        }
        [HttpPost]
        public async Task<IActionResult> CreateMessageAsync(CreateMessageDto createMessageDto)
        {
            await _userMessageService.CreateMessageAsync(createMessageDto);
            return Ok("Message added successfully.");
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteMessageAsync(int id)
        {
            await _userMessageService.DeleteMessageAsync(id);
            return Ok("Message deleted successfully.");
        }
        [HttpPut]
        public async Task<IActionResult> UpdateMessageAsync(UpdateMessageDto updateMessageDto)
        {
            await _userMessageService.UpdateMessageAsync(updateMessageDto);
            return Ok("Message deleted successfully.");
        }

        [HttpGet("GetTotalMessageCount")]
        public async Task<IActionResult> GetTotalMessageCount()
        {
            int values = await _userMessageService.GetTotalMessageCount();
            return Ok(values);

        }


        [HttpGet("GetTotalMessageCountByReceiverId")]
        public async Task<IActionResult> GetTotalMessageCountByReceiverId(string id)
        {
            int values = await _userMessageService.GetTotalMessageCountByReceiverId(id);
            return Ok(values);

        }

        [HttpPost("SendConversationMessage")]
        public async Task<IActionResult> SendConversationMessage(
    SendConversationMessageDto sendConversationMessageDto)
        {
            try
            {
                await _userMessageService
                    .CreateConversationMessageAsync(
                        sendConversationMessageDto);

                return Ok("Message sent successfully.");
            }
            catch (ArgumentException exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpGet("GetConversationMessages/{conversationId}")]
        public async Task<IActionResult> GetConversationMessages(
    int conversationId)
        {
            try
            {
                var values =
                    await _userMessageService
                        .GetConversationMessagesAsync(
                            conversationId);

                return Ok(values);
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

        [HttpPut("MarkConversationMessagesAsRead/{conversationId}/{receiverId}")]
        public async Task<IActionResult> MarkConversationMessagesAsRead(
    int conversationId,
    string receiverId)
        {
            try
            {
                await _userMessageService
                    .MarkConversationMessagesAsReadAsync(
                        conversationId,
                        receiverId);

                return Ok("Messages marked as read successfully.");
            }
            catch (ArgumentException exception)
            {
                return BadRequest(exception.Message);
            }
        }

    }
}
