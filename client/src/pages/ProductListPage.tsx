import { useState } from 'react';
import { keepPreviousData, useQuery } from '@tanstack/react-query';
import { getFilterOptions, getProducts } from '../api/products';
import FilterPanel from '../components/FilterPanel';
import { useDebouncedValue } from '../hooks/useDebouncedValue';
import { emptyFilters, type ProductFilters } from '../types';
import { Link } from 'react-router-dom';

const formatPrice = (value: number) =>
  new Intl.NumberFormat('tr-TR', { style: 'currency', currency: 'TRY' }).format(value);

export default function ProductListPage() {
  const [filters, setFilters] = useState<ProductFilters>(emptyFilters);
  const [page, setPage] = useState(1);

  // Kullanıcı yazarken beklenir, durunca bu değer güncellenir ve istek atılır
  const debouncedFilters = useDebouncedValue(filters, 400);

  const { data: options } = useQuery({
    queryKey: ['filter-options'],
    queryFn: getFilterOptions,
    staleTime: Infinity,
  });

  const { data, isLoading, isError, isFetching } = useQuery({
    queryKey: ['products', debouncedFilters, page],
    queryFn: () => getProducts(debouncedFilters, page),
    placeholderData: keepPreviousData,
  });

  // Filtre değişince ilk sayfaya dön
  const updateFilters = (patch: Partial<ProductFilters>) => {
    setFilters((f) => ({ ...f, ...patch }));
    setPage(1);
  };

  const resetFilters = () => {
    setFilters(emptyFilters);
    setPage(1);
  };

  return (
    <div className="container">
      <h1>Ayakkabılar</h1>

      <div className="layout">
        <FilterPanel
          options={options}
          filters={filters}
          onChange={updateFilters}
          onReset={resetFilters}
        />

        <main>
          <div className="toolbar">
            <span>{data ? `${data.totalCount} ürün` : ''}</span>
            <label>
              Sırala{' '}
              <select value={filters.sort} onChange={(e) => updateFilters({ sort: e.target.value })}>
                <option value="newest">En yeni</option>
                <option value="price_asc">Fiyat: düşükten yükseğe</option>
                <option value="price_desc">Fiyat: yüksekten düşüğe</option>
                <option value="name">İsme göre</option>
              </select>
            </label>
          </div>

          {isLoading && <p>Yükleniyor...</p>}
          {isError && <p>Ürünler yüklenemedi.</p>}

          {data && data.items.length === 0 && <p>Bu filtrelere uygun ürün bulunamadı.</p>}

          {data && data.items.length > 0 && (
            <>
              <div className="grid" style={{ opacity: isFetching ? 0.6 : 1 }}>
                {data.items.map((p) => (
                  <Link className="card" key={p.id} to={`/products/${p.slug}`}>
                    {p.imageUrl && <img src={p.imageUrl} alt={p.name} />}
                    <div className="card-body">
                      <div className="brand">{p.brandName}</div>
                      <div className="name">{p.name}</div>
                      <div className="price">{formatPrice(p.price)}</div>
                      {!p.inStock && <div className="out">Tükendi</div>}
                    </div>
                  </Link>
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
            </>
          )}
        </main>
      </div>
    </div>
  );
}