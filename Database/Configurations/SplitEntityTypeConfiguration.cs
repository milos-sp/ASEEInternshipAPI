using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductAPI.Database.Entities;

namespace ProductAPI.Database.Configurations
{
    public class SplitEntityTypeConfiguration : IEntityTypeConfiguration<SplitEntity>
    {

        public SplitEntityTypeConfiguration() 
        { 
            
        }
        public void Configure(EntityTypeBuilder<SplitEntity> builder)
        {
            builder.ToTable("splits");

            // primary key
            builder.HasKey(x => x.Id);
            // columns
            builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired(true);
            builder.Property(x => x.Catcode).IsRequired(true);
            builder.Property(x => x.Amount).IsRequired(true);
            builder.Property(x => x.TransactionId).IsRequired(true);
        }
    }
}
