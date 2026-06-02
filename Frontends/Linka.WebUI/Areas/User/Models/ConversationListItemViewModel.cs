namespace Linka.WebUI.Areas.User.Models
{
    public class ConversationListItemViewModel
    {
        public int ConversationId { get; set; }

        public string OtherUserId { get; set; } = string.Empty;

        public string OtherUserDisplayName { get; set; }
            = string.Empty;

        public string ProductId { get; set; } = string.Empty;

        public string ProductName { get; set; } = string.Empty;

        public string LastMessage { get; set; } = string.Empty;

        public DateTime? LastMessageDate { get; set; }

        public int UnreadMessageCount { get; set; }
    }
}
