using Linka.Message.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Linka.Message.DAL.Context
{
    public class MessageContext : DbContext
    {
        public MessageContext(DbContextOptions<MessageContext> options)
            : base(options)
        {
        }

        public DbSet<UserMessage> UserMessages { get; set; }
        public DbSet<Conversation> Conversations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserMessage>()
                .HasOne(x => x.Conversation)
                .WithMany(x => x.UserMessages)
                .HasForeignKey(x => x.ConversationId)
                .OnDelete(DeleteBehavior.Cascade);

            /*
             * Daha sonra iki kullanıcı ve ürün üzerinden mevcut sohbeti
             * hızlıca bulmak için index oluşturuyoruz.
             */
            modelBuilder.Entity<Conversation>()
                .HasIndex(x => new
                {
                    x.FirstUserId,
                    x.SecondUserId,
                    x.ProductId
                });
        }
    }
}