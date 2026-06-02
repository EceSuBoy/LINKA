namespace Linka.Message.Dtos
{
    public class CreateConversationDto
    {
        public string FirstUserId { get; set; }
        public string SecondUserId { get; set; }

        public string ProductId { get; set; }

        public int SourceCommentId { get; set; }
    }
}
