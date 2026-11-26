import type { PagedResult, ProductListItem } from '../types';

const API_URL = import.meta.env.VITE_API_URL ?? 'https://localhost:7001';

export async function getProducts(page: number): Promise<PagedResult<ProductListItem>> {
  const res = await fetch(`${API_URL}/api/products?page=${page}&pageSize=12`);
  if (!res.ok) throw new Error('Ürünler yüklenemedi');
  return res.json();
}