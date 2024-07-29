
using Microsoft.EntityFrameworkCore;
using ProductAPI.Database.Configurations;
using ProductAPI.Database.Entities;
using System.Reflection.Metadata;

namespace ProductAPI.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        public DbSet<TransactionEntity> Transactions { get; set; }

        public DbSet<CategoryEntity> Categories { get; set; }

        public DbSet<SplitEntity> Splits { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(
                new TransactionEntityTypeConfiguration()
                );

            modelBuilder.ApplyConfiguration(
                new CategoryEntityTypeConfiguration()
            );

            modelBuilder.ApplyConfiguration(
                new SplitEntityTypeConfiguration()
            );

            modelBuilder.Entity<CategoryEntity>()
               .HasMany(e => e.Transactions)
               .WithOne(e => e.Category)
               .HasForeignKey(e => e.Catcode)
               .IsRequired(false);

            modelBuilder.Entity<TransactionEntity>()
                .HasMany(e => e.Splits)
                .WithOne(e => e.Transaction)
                .HasForeignKey(e => e.TransactionId)
                .IsRequired(true);

            // modelBuilder.UseIdentityColumns();

            base.OnModelCreating(modelBuilder);
        }
    }
}
