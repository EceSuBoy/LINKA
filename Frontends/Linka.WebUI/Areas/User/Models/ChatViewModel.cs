using Linka.DtoLayer.MessageDtos;

namespace Linka.WebUI.Areas.User.Models
{
    public class ChatViewModel
    {
        public int ConversationId { get; set; }

        public string CurrentUserId { get; set; } = string.Empty;

        public string OtherUserId { get; set; } = string.Empty;

        public string ProductId { get; set; } = string.Empty;

        public List<ResultConversationMessageDto> Messages { get; set; }
            = new List<ResultConversationMessageDto>();

        public string ProductName { get; set; } = string.Empty;

        public string OtherUserDisplayName { get; set; }
    = string.Empty;
    }
}
