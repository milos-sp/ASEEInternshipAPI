using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductAPI.Database.Entities;

namespace ProductAPI.Database.Configurations
{
    public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<ProductEntity>
    {
        public ProductEntityTypeConfiguration()
        {
        }

        public void Configure(EntityTypeBuilder<ProductEntity> builder)
        {
            builder.ToTable("products");
            // primary key
            builder.HasKey(x => x.Code);
            // definition of columns
            builder.Property(x => x.Code).IsRequired().HasMaxLength(64);
            builder.Property(x => x.Kind).HasConversion<string>().IsRequired();
            builder.Property(x => x.Name).HasMaxLength(64);
            builder.Property(x => x.Description).HasMaxLength(1024);
            builder.Property(x => x.ImageUrl).HasMaxLength(128);
            builder.Property(x => x.Status).HasConversion<string>();
            builder.Property(x => x.AvailabilityStart);
            builder.Property(x => x.AvailabilityEnd);
            builder.Property(x => x.IsPackage);
            builder.Property(x => x.Price);
            builder.Property(x => x.Metadata).HasMaxLength(50);
        }
    }
}
