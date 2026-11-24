using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShoeStore.Domain.Entities;

namespace ShoeStore.Infrastructure.Persistence.Configurations;

public class BrandConfiguration : IEntityTypeConfiguration<Brand>
{
    public void Configure(EntityTypeBuilder<Brand> builder)
    {
        builder.Property(b => b.Name).IsRequired().HasMaxLength(100);
        builder.Property(b => b.Slug).IsRequired().HasMaxLength(120);
        builder.HasIndex(b => b.Slug).IsUnique();
    }
}