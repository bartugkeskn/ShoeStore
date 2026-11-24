using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShoeStore.Domain.Entities;

namespace ShoeStore.Infrastructure.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(p => p.Name).IsRequired().HasMaxLength(200);
        builder.Property(p => p.Slug).IsRequired().HasMaxLength(220);
        builder.Property(p => p.Description).HasMaxLength(2000);
        builder.Property(p => p.BasePrice).HasPrecision(18, 2);

        builder.HasIndex(p => p.Slug).IsUnique();
        builder.HasIndex(p => new { p.CategoryId, p.BrandId });

        // Kategori/marka silinirken ürünleri sessizce silmeyelim
        builder.HasOne(p => p.Brand)
               .WithMany(b => b.Products)
               .HasForeignKey(p => p.BrandId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Category)
               .WithMany(c => c.Products)
               .HasForeignKey(p => p.CategoryId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}