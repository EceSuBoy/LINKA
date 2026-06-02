namespace Linka.Message.Dtos
{
    public class ResultConversationDto
    {
        public int ConversationId { get; set; }

        public string FirstUserId { get; set; }
        public string SecondUserId { get; set; }

        public string ProductId { get; set; }

        public int SourceCommentId { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
