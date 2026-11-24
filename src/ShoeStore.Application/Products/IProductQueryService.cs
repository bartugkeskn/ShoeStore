using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ShoeStore.Application.Common;

namespace ShoeStore.Application.Products;

public interface IProductQueryService
{
    Task<PagedResult<ProductListItemDto>> GetListAsync(ProductListQuery query, CancellationToken ct = default);
}