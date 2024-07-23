
using Microsoft.EntityFrameworkCore;
using ProductAPI.Database.Configurations;
using ProductAPI.Database.Entities;

namespace ProductAPI.Database
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<ProductEntity> Products { get; set; }

        public DbSet<TransactionEntity> Transactions { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(
               new ProductEntityTypeConfiguration()
                );
            modelBuilder.ApplyConfiguration(
                new TransactionEntityTypeConfiguration()
                );

            base.OnModelCreating(modelBuilder);
        }
    }
}
