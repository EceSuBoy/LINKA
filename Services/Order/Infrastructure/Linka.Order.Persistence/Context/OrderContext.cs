using Linka.Order.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linka.Order.Persistence.Context
{
    public class OrderContext: DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost,1440;initial Catalog=LinkaOrderDb;TrustServerCertificate=True;User=sa;Password=123456aA*");
        }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Ordering> Orderings { get; set; }

        protected override void OnModelCreating(
    ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Ordering>(
                entity =>
                {
                    entity.Property(x => x.TotalPrice)
                        .HasPrecision(18, 2);

                    entity.Property(x => x.OrderStatus)
                        .HasMaxLength(30)
                        .HasDefaultValue("Paid");
                });

            modelBuilder.Entity<OrderDetail>(
                entity =>
                {
                    entity.Property(x => x.ProductPrice)
                        .HasPrecision(18, 2);

                    entity.Property(x => x.ProductTotalPrice)
                        .HasPrecision(18, 2);
                });
        }
    }
}
