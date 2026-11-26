import { useState } from 'react';
import { useQuery } from '@tanstack/react-query';
import { Link, useParams } from 'react-router-dom';
import { getProductBySlug } from '../api/products';

const formatPrice = (value: number) =>
  new Intl.NumberFormat('tr-TR', { style: 'currency', currency: 'TRY' }).format(value);

const genderLabels: Record<string, string> = {
  Men: 'Erkek',
  Women: 'Kadın',
  Kids: 'Çocuk',
  Unisex: 'Unisex',
};

export default function ProductDetailPage() {
  const { slug = '' } = useParams();
  const [selectedColor, setSelectedColor] = useState<string | null>(null);
  const [selectedSize, setSelectedSize] = useState<string | null>(null);

  const { data: product, isLoading, isError } = useQuery({
    queryKey: ['product', slug],
    queryFn: () => getProductBySlug(slug),
  });

  if (isLoading) return <div className="container">Yükleniyor...</div>;
  if (isError) return <div className="container">Ürün yüklenemedi.</div>;
  if (!product) {
    return (
      <div className="container">
        <p>Ürün bulunamadı.</p>
        <Link to="/">Ürünlere dön</Link>
      </div>
    );
  }

  // Seçim yapılmadıysa ilk rengi göster
  const colors = [...new Set(product.variants.map((v) => v.color))];
  const color = selectedColor ?? colors[0];
  const variantsOfColor = product.variants.filter((v) => v.color === color);
  const selectedVariant = variantsOfColor.find((v) => v.size === selectedSize) ?? null;
  const image = product.images.find((i) => i.color === color) ?? product.images[0];
  const price = selectedVariant?.price ?? product.basePrice;

  const stockText = !selectedVariant
    ? 'Beden seçin'
    : selectedVariant.stockQuantity === 0
      ? 'Tükendi'
      : selectedVariant.stockQuantity <= 5
        ? `Son ${selectedVariant.stockQuantity} adet`
        : 'Stokta var';

  const pickColor = (c: string) => {
    setSelectedColor(c);
    setSelectedSize(null); // Renk değişince bedenler de değişir, seçimi sıfırla
  };

  return (
    <div className="container">
      <Link to="/">← Ürünlere dön</Link>

      <div className="detail">
        <div className="detail-image">
          {image && <img src={image.url} alt={`${product.name} ${color}`} />}
        </div>

        <div className="detail-info">
          <div className="brand">{product.brandName}</div>
          <h1>{product.name}</h1>
          <div className="meta">
            {product.categoryName} · {genderLabels[product.gender] ?? product.gender}
          </div>
          <div className="detail-price">{formatPrice(price)}</div>

          <h3>Renk: {color}</h3>
          <div className="options">
            {colors.map((c) => (
              <button
                key={c}
                type="button"
                className={c === color ? 'opt selected' : 'opt'}
                onClick={() => pickColor(c)}
              >
                {c}
              </button>
            ))}
          </div>

          <h3>Beden</h3>
          <div className="options">
            {variantsOfColor.map((v) => (
              <button
                key={v.id}
                type="button"
                className={v.size === selectedSize ? 'opt selected' : 'opt'}
                disabled={v.stockQuantity === 0}
                onClick={() => setSelectedSize(v.size)}
              >
                {v.size}
              </button>
            ))}
          </div>

          <p className="stock">{stockText}</p>

          <button type="button" className="add-to-cart" disabled={!selectedVariant || selectedVariant.stockQuantity === 0}>
            Sepete ekle
          </button>

          {product.description && <p className="desc">{product.description}</p>}
        </div>
      </div>
    </div>
  );
}