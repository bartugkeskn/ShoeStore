using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeStore.Domain.Entities;

// Asıl satılan şey: Model + Renk + Beden
public class ProductVariant
{
    public int Id { get; set; }
    public string Color { get; set; } = string.Empty;
    public string Size { get; set; } = string.Empty;   // "42", "42.5" gibi
    public string Sku { get; set; } = string.Empty;
    public decimal? Price { get; set; }                // null ise Product.BasePrice geçerli
    public int StockQuantity { get; set; }

    // Eşzamanlı stok güncellemelerini yakalamak için (sonra EF'te yapılandıracağız)
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
}
