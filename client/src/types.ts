export interface ProductListItem {
  id: number;
  name: string;
  slug: string;
  brandName: string;
  categoryName: string;
  gender: string;
  price: number;
  imageUrl: string | null;
  inStock: boolean;
}

export interface PagedResult<T> {
  items: T[];
  page: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
}