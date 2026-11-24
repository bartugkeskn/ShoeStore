using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShoeStore.Domain.Entities;

namespace ShoeStore.Infrastructure.Persistence.Configurations;

public class ProductVariantConfiguration : IEntityTypeConfiguration<ProductVariant>
{
    public void Configure(EntityTypeBuilder<ProductVariant> builder)
    {
        builder.Property(v => v.Color).IsRequired().HasMaxLength(50);
        builder.Property(v => v.Size).IsRequired().HasMaxLength(10);
        builder.Property(v => v.Sku).IsRequired().HasMaxLength(60);
        builder.Property(v => v.Price).HasPrecision(18, 2);

        // Eşzamanlı stok güncellemelerini yakalamak için
        builder.Property(v => v.RowVersion).IsRowVersion();

        builder.HasIndex(v => v.Sku).IsUnique();
        // Aynı ürünün aynı renk + beden kombinasyonu iki kez olamaz
        builder.HasIndex(v => new { v.ProductId, v.Color, v.Size }).IsUnique();

        builder.HasOne(v => v.Product)
               .WithMany(p => p.Variants)
               .HasForeignKey(v => v.ProductId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}