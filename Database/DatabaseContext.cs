
using Microsoft.EntityFrameworkCore;
using ProductAPI.Database.Configurations;
using ProductAPI.Database.Entities;

namespace ProductAPI.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        public DbSet<TransactionEntity> Transactions { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(
                new TransactionEntityTypeConfiguration()
                );

            base.OnModelCreating(modelBuilder);
        }
    }
}
