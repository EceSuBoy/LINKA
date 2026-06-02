namespace Linka.Message.DAL.Entities
{
    public class Conversation
    {
        public int ConversationId { get; set; }

        /*
         * Sohbete katılan iki kullanıcının IdentityServer ID değerleri.
         * Gönderici ve alıcı zaman içinde yer değiştirebileceği için
         * burada Sender ve Receiver isimlerini kullanmıyoruz.
         */
        public string FirstUserId { get; set; }
        public string SecondUserId { get; set; }

        /*
         * Sohbetin hangi ürün yorumu üzerinden başladığını bilmek için
         * ürün ve yorum ID değerlerini saklıyoruz.
         */
        public string ProductId { get; set; }
        public int SourceCommentId { get; set; }

        public DateTime CreatedDate { get; set; }

        public ICollection<UserMessage> UserMessages { get; set; }
            = new List<UserMessage>();
    }
}
