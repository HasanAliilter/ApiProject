using Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Persistence.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            Category category1 = new()
            {
                Id = 1,
                Name = "Elektronik",
                Priorty = 1,
                ParentId = 0,
                IsDeleted = false,
                CreatedDate = DateTime.Now,
            };

            Category category2 = new()
            {
                Id = 2,
                Name = "Moda",
                Priorty = 2,
                ParentId = 0,
                IsDeleted = false,
                CreatedDate = DateTime.Now,
            };

            Category child1 = new()
            {
                Id = 3,
                Name = "Bilgisayar",
                Priorty = 1,
                ParentId = 1,
                IsDeleted = false,
                CreatedDate = DateTime.Now,
            };

            Category child2 = new()
            {
                Id = 4,
                Name = "Kadın",
                Priorty = 1,
                ParentId = 2,
                IsDeleted = false,
                CreatedDate = DateTime.Now,
            };

            builder.HasData(category1, category2, child1, child2);
        }
    }
}
