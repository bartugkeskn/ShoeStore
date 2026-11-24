using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShoeStore.Domain.Entities;

namespace ShoeStore.Infrastructure.Persistence.Configurations;

public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
{
    public void Configure(EntityTypeBuilder<ProductImage> builder)
    {
        builder.Property(i => i.Url).IsRequired().HasMaxLength(500);
        builder.Property(i => i.Color).HasMaxLength(50);

        builder.HasOne(i => i.Product)
               .WithMany(p => p.Images)
               .HasForeignKey(i => i.ProductId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}