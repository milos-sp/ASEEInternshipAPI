using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductAPI.Database.Entities;

namespace ProductAPI.Database.Configurations
{
    public class TransactionEntityTypeConfiguration : IEntityTypeConfiguration<TransactionEntity>
    {

        public TransactionEntityTypeConfiguration()
        {
            
        }
        public void Configure(EntityTypeBuilder<TransactionEntity> builder)
        {
            builder.ToTable("transactions");
            // primary key
            builder.HasKey(x => x.Id);
            // columns
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.BeneficiaryName).IsRequired(false).HasMaxLength(64);
            builder.Property(x => x.Date).IsRequired();
            builder.Property(x => x.Direction).HasConversion<string>().IsRequired();
            builder.Property(x => x.Amount).IsRequired();
            builder.Property(x => x.Description).IsRequired(false);
            builder.Property(x => x.Currency).IsRequired();
            builder.Property(x => x.Mcc).IsRequired(false);
            builder.Property(x => x.Kind).HasConversion<string>().IsRequired();
        }
    }
}
