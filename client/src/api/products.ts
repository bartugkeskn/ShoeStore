import type { FilterOptions, PagedResult, ProductFilters, ProductListItem } from '../types';

const API_URL = import.meta.env.VITE_API_URL ?? 'https://localhost:7001';

export async function getProducts(
  filters: ProductFilters,
  page: number,
): Promise<PagedResult<ProductListItem>> {
  const params = new URLSearchParams({ page: String(page), pageSize: '12' });

  // Sadece dolu olan filtreleri query string'e ekle
  const textFilters = ['search', 'brandId', 'categoryId', 'gender', 'color', 'size', 'minPrice', 'maxPrice', 'sort'] as const;
  for (const key of textFilters) {
    const value = filters[key].trim();
    if (value) params.set(key, value);
  }
  if (filters.inStockOnly) params.set('inStockOnly', 'true');

  const res = await fetch(`${API_URL}/api/products?${params}`);
  if (!res.ok) throw new Error('Ürünler yüklenemedi');
  return res.json();
}

export async function getFilterOptions(): Promise<FilterOptions> {
  const res = await fetch(`${API_URL}/api/products/filters`);
  if (!res.ok) throw new Error('Filtreler yüklenemedi');
  return res.json();
}