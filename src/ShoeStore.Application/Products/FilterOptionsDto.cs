namespace ShoeStore.Application.Products;

public record LookupDto(int Id, string Name);

public record CategoryLookupDto(int Id, string Name, int? ParentId);

public record FilterOptionsDto(
    IReadOnlyList<LookupDto> Brands,
    IReadOnlyList<CategoryLookupDto> Categories,
    IReadOnlyList<string> Colors,
    IReadOnlyList<string> Sizes);