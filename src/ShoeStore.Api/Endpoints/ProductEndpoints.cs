using ShoeStore.Application.Products;

namespace ShoeStore.Api.Endpoints;

public static class ProductEndpoints
{
    public static void MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/products").WithTags("Products");

        group.MapGet("/", async (
            [AsParameters] ProductListQuery query,
            IProductQueryService service,
            CancellationToken ct) =>
        {
            var result = await service.GetListAsync(query, ct);
            return Results.Ok(result);
        });
    }
}