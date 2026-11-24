using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using ShoeStore.Application.Common;
using ShoeStore.Application.Products;
using ShoeStore.Infrastructure.Persistence;

namespace ShoeStore.Infrastructure.Services;

public class ProductQueryService : IProductQueryService
{
    private readonly AppDbContext _db;

    public ProductQueryService(AppDbContext db) => _db = db;

    public async Task<PagedResult<ProductListItemDto>> GetListAsync(ProductListQuery q, CancellationToken ct = default)
    {
        var page = Math.Max(q.Page, 1);
        var pageSize = Math.Clamp(q.PageSize, 1, 50);

        // AsNoTracking: sadece okuyoruz, değişiklik takibine gerek yok (daha hızlı)
        var query = _db.Products.AsNoTracking().Where(p => p.IsActive);

        if (!string.IsNullOrWhiteSpace(q.Search))
            query = query.Where(p => p.Name.Contains(q.Search));

        if (q.BrandId.HasValue)
            query = query.Where(p => p.BrandId == q.BrandId.Value);

        if (q.CategoryId.HasValue)
        {
            // Ana kategori seçilirse alt kategorilerin ürünleri de gelsin
            var id = q.CategoryId.Value;
            query = query.Where(p => p.CategoryId == id || p.Category.ParentId == id);
        }

        if (q.Gender.HasValue)
            query = query.Where(p => p.Gender == q.Gender.Value);

        if (q.MinPrice.HasValue)
            query = query.Where(p => p.BasePrice >= q.MinPrice.Value);

        if (q.MaxPrice.HasValue)
            query = query.Where(p => p.BasePrice <= q.MaxPrice.Value);

        // Beden, renk ve stok filtreleri aynı varyant üzerinde birlikte aranmalı:
        // "42 numara + siyah + stokta" bir varyanta ait olmalı
        var filterByVariant = q.Size != null || q.Color != null || q.InStockOnly;
        if (filterByVariant)
        {
            query = query.Where(p => p.Variants.Any(v =>
                (q.Size == null || v.Size == q.Size) &&
                (q.Color == null || v.Color == q.Color) &&
                (!q.InStockOnly || v.StockQuantity > 0)));
        }

        // Id ile ikinci sıralama: sayfalar arasında tutarlı sıra için şart
        query = q.Sort switch
        {
            "price_asc" => query.OrderBy(p => p.BasePrice).ThenBy(p => p.Id),
            "price_desc" => query.OrderByDescending(p => p.BasePrice).ThenBy(p => p.Id),
            "name" => query.OrderBy(p => p.Name).ThenBy(p => p.Id),
            _ => query.OrderByDescending(p => p.CreatedAt).ThenBy(p => p.Id)
        };

        var totalCount = await query.CountAsync(ct);

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new ProductListItemDto(
                p.Id,
                p.Name,
                p.Slug,
                p.Brand.Name,
                p.Category.Name,
                p.Gender.ToString(),
                p.BasePrice,
                p.Images.OrderBy(i => i.SortOrder).Select(i => i.Url).FirstOrDefault(),
                p.Variants.Any(v => v.StockQuantity > 0)))
            .ToListAsync(ct);

        return new PagedResult<ProductListItemDto>(items, page, pageSize, totalCount);
    }
}