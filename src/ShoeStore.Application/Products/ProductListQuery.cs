using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoeStore.Domain.Enums;

namespace ShoeStore.Application.Products;

// Tüm parametreler opsiyonel; query string'den otomatik bağlanacak
public record ProductListQuery(
    string? Search = null,
    int? BrandId = null,
    int? CategoryId = null,
    Gender? Gender = null,
    decimal? MinPrice = null,
    decimal? MaxPrice = null,
    string? Size = null,
    string? Color = null,
    bool InStockOnly = false,
    string? Sort = null,      // newest (varsayılan), price_asc, price_desc, name
    int Page = 1,
    int PageSize = 12);