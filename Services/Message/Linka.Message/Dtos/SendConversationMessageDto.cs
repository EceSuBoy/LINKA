namespace Linka.Message.Dtos
{
    public class SendConversationMessageDto
    {
        public int ConversationId { get; set; }

        public string SenderId { get; set; }

        public string ReceiverId { get; set; }

        public string MessageDetail { get; set; }
    }
}
