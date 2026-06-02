namespace Linka.Message.Dtos
{
    public class ResultUserConversationDto
    {
        public int ConversationId { get; set; }

        /*
         * Giriş yapan kullanıcının konuştuğu diğer kişinin ID değeri.
         */
        public string OtherUserId { get; set; } = string.Empty;

        /*
         * Konuşmanın hangi ürün üzerinden başladığını gösterir.
         */
        public string ProductId { get; set; } = string.Empty;

        public int SourceCommentId { get; set; }

        /*
         * Konuşma henüz oluşturulmuş ama ilk mesaj gönderilmemiş olabilir.
         * Bu nedenle boş kalabilir.
         */
        public string LastMessage { get; set; } = string.Empty;

        public DateTime? LastMessageDate { get; set; }

        public int UnreadMessageCount { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
