namespace Linka.Message.Dtos
{
    public class ResultConversationMessageDto
    {
        public int UserMessageId { get; set; }

        public int? ConversationId { get; set; }

        public string SenderId { get; set; }

        public string ReceiverId { get; set; }

        public string MessageDetail { get; set; }

        public bool IsRead { get; set; }

        public DateTime MessageDate { get; set; }
    }
}
