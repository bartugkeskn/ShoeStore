import { useState } from 'react';
import { keepPreviousData, useQuery } from '@tanstack/react-query';
import { getProducts } from '../api/products';

const formatPrice = (value: number) =>
  new Intl.NumberFormat('tr-TR', { style: 'currency', currency: 'TRY' }).format(value);

export default function ProductListPage() {
  const [page, setPage] = useState(1);

  const { data, isLoading, isError } = useQuery({
    queryKey: ['products', page],
    queryFn: () => getProducts(page),
    placeholderData: keepPreviousData,
  });

  if (isLoading) return <div className="container">Yükleniyor...</div>;
  if (isError || !data) return <div className="container">Ürünler yüklenemedi.</div>;

  return (
    <div className="container">
      <h1>Ayakkabılar</h1>
      <p>{data.totalCount} ürün</p>

      <div className="grid">
        {data.items.map((p) => (
          <div className="card" key={p.id}>
            {p.imageUrl && <img src={p.imageUrl} alt={p.name} />}
            <div className="card-body">
              <div className="brand">{p.brandName}</div>
              <div className="name">{p.name}</div>
              <div className="price">{formatPrice(p.price)}</div>
              {!p.inStock && <div className="out">Tükendi</div>}
            </div>
          </div>
        ))}
      </div>

      <div className="pager">
        <button onClick={() => setPage((p) => p - 1)} disabled={page <= 1}>
          Önceki
        </button>
        <span>
          Sayfa {data.page} / {data.totalPages}
        </span>
        <button onClick={() => setPage((p) => p + 1)} disabled={page >= data.totalPages}>
          Sonraki
        </button>
      </div>
    </div>
  );
}