using Bogus;
using Microsoft.EntityFrameworkCore;
using ShoeStore.Domain.Entities;
using ShoeStore.Domain.Enums;

namespace ShoeStore.Infrastructure.Persistence;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        // Veri zaten varsa tekrar ekleme
        if (await db.Products.AnyAsync()) return;

        Randomizer.Seed = new Random(12345); // Her seferinde aynı veri üretilsin
        var f = new Faker();

        var brands = new[] { "Nike", "Adidas", "Puma", "New Balance", "Converse", "Skechers" }
            .Select(n => new Brand { Name = n, Slug = Slugify(n) })
            .ToList();

        var sport = new Category { Name = "Spor Ayakkabı", Slug = "spor-ayakkabi" };
        var running = new Category { Name = "Koşu", Slug = "kosu", Parent = sport };
        var basketball = new Category { Name = "Basketbol", Slug = "basketbol", Parent = sport };
        var casual = new Category { Name = "Günlük", Slug = "gunluk" };
        var classic = new Category { Name = "Klasik", Slug = "klasik" };
        var boots = new Category { Name = "Bot", Slug = "bot" };

        var allCategories = new List<Category> { sport, running, basketball, casual, classic, boots };
        var leafCategories = new List<Category> { running, basketball, casual, classic, boots };

        db.Brands.AddRange(brands);
        db.Categories.AddRange(allCategories);
        await db.SaveChangesAsync();

        var models = new[] { "Air Runner", "Street Pro", "Classic Low", "Trail Max", "Court Vision",
                             "Urban Flex", "Speed Lite", "Retro 90", "Cloud Step", "Peak Boost" };
        var colors = new[] { "Siyah", "Beyaz", "Lacivert", "Gri", "Kırmızı" };

        for (var i = 1; i <= 40; i++)
        {
            var brand = f.PickRandom(brands);
            var category = f.PickRandom(leafCategories);
            var gender = f.PickRandom<Gender>();
            var name = $"{brand.Name} {f.PickRandom(models)}";

            var product = new Product
            {
                Name = name,
                Slug = $"{Slugify(name)}-{i}",
                Description = f.Lorem.Sentences(2),
                BasePrice = Math.Round(f.Random.Decimal(799, 5999) / 10) * 10,
                Gender = gender,
                BrandId = brand.Id,
                CategoryId = category.Id
            };

            var sizes = gender switch
            {
                Gender.Men => Enumerable.Range(40, 7),     // 40-46
                Gender.Women => Enumerable.Range(36, 6),   // 36-41
                Gender.Kids => Enumerable.Range(28, 8),    // 28-35
                _ => Enumerable.Range(38, 8)               // 38-45
            };

            var chosenColors = f.PickRandom(colors, f.Random.Int(2, 3)).ToList();

            foreach (var color in chosenColors)
            {
                var colorIndex = Array.IndexOf(colors, color);

                product.Images.Add(new ProductImage
                {
                    Url = $"https://placehold.co/600x600/png?text={Uri.EscapeDataString(name + " " + color)}",
                    Color = color,
                    SortOrder = colorIndex
                });

                foreach (var size in sizes)
                {
                    product.Variants.Add(new ProductVariant
                    {
                        Color = color,
                        Size = size.ToString(),
                        Sku = $"SS-{i:D3}-{colorIndex}-{size}",
                        StockQuantity = f.Random.Int(0, 25)
                    });
                }
            }

            db.Products.Add(product);
        }

        await db.SaveChangesAsync();
    }

    private static string Slugify(string text) =>
        text.ToLowerInvariant()
            .Replace('ı', 'i').Replace('ş', 's').Replace('ğ', 'g')
            .Replace('ü', 'u').Replace('ö', 'o').Replace('ç', 'c')
            .Replace(' ', '-');
}