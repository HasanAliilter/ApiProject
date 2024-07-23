using Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Persistence.Configurations
{
    public class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
    {
        public void Configure(EntityTypeBuilder<ProductCategory> builder)
        {
            builder.HasKey(x => new { x.ProductId, x.CategoryId }); //2 tane Primary key im oluştu

            builder.HasOne(p => p.Product) // Bir tane product birden fazla ProductCategory e karşı gelebilir
                .WithMany(pc => pc.ProductCategories)
                .HasForeignKey(p => p.ProductId)
                .OnDelete(DeleteBehavior.Cascade); //Bu ondelete içindeki cascade bir product silindiğinde ona bağlı verilerin de silinmesini sağlar

            builder.HasOne(c => c.Category)
                .WithMany(ca => ca.ProductCategories)
                .HasForeignKey(c => c.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
