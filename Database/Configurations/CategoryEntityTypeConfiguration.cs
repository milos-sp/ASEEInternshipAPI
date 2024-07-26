using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductAPI.Database.Entities;

namespace ProductAPI.Database.Configurations
{
    public class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<CategoryEntity>
    {

        public CategoryEntityTypeConfiguration() 
        {
        
        }
        public void Configure(EntityTypeBuilder<CategoryEntity> builder)
        {
            builder.ToTable("categories");

            //primary key
            builder.HasKey(x => x.Code);
            //columns
            builder.Property(x => x.Code).IsRequired();
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.ParentCode).IsRequired(false);

        }
    }
}
