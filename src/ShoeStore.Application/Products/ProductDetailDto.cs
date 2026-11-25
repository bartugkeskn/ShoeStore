namespace ShoeStore.Application.Products;

public record ProductImageDto(string Url, string? Color, int SortOrder);

public record ProductVariantDto(int Id, string Color, string Size, string Sku, decimal Price, int StockQuantity);

public record ProductDetailDto(
    int Id,
    string Name,
    string Slug,
    string? Description,
    int BrandId,
    string BrandName,
    int CategoryId,
    string CategoryName,
    string Gender,
    decimal BasePrice,
    IReadOnlyList<ProductImageDto> Images,
    IReadOnlyList<ProductVariantDto> Variants);