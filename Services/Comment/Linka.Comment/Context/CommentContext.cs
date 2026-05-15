using Linka.Comment.Entities;
using Microsoft.EntityFrameworkCore;

namespace Linka.Comment.Context
{
    public class CommentContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost,1442;initial Catalog=LinkaCommentDb;TrustServerCertificate=True;User=sa;Password=123456aA*");
        }
        public DbSet<UserComment> UserComments { get; set; }
    }
}
