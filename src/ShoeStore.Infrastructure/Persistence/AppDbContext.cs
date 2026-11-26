using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShoeStore.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ShoeStore.Infrastructure.Identity;

namespace ShoeStore.Infrastructure.Persistence;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Brand> Brands => Set<Brand>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductVariant> ProductVariants => Set<ProductVariant>();
    public DbSet<ProductImage> ProductImages => Set<ProductImage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // Identity'nin kendi tablo ayarlarını kurar, ilk satır olmalı
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly); // Configurations klasöründeki tüm ayar sınıflarını otomatik uygular
    }
}