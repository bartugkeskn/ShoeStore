using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeStore.Application.Products;

public record ProductListItemDto(
    int Id,
    string Name,
    string Slug,
    string BrandName,
    string CategoryName,
    string Gender,
    decimal Price,
    string? ImageUrl,
    bool InStock);