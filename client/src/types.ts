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

export interface Lookup {
  id: number;
  name: string;
}

export interface CategoryLookup {
  id: number;
  name: string;
  parentId: number | null;
}

export interface FilterOptions {
  brands: Lookup[];
  categories: CategoryLookup[];
  colors: string[];
  sizes: string[];
}

// Form alanlarıyla kolay bağlansın diye hepsi string; boş string = "filtre yok"
export interface ProductFilters {
  search: string;
  brandId: string;
  categoryId: string;
  gender: string;
  color: string;
  size: string;
  minPrice: string;
  maxPrice: string;
  inStockOnly: boolean;
  sort: string;
}

export const emptyFilters: ProductFilters = {
  search: '',
  brandId: '',
  categoryId: '',
  gender: '',
  color: '',
  size: '',
  minPrice: '',
  maxPrice: '',
  inStockOnly: false,
  sort: 'newest',
};

export interface ProductImage {
  url: string;
  color: string | null;
  sortOrder: number;
}

export interface ProductVariant {
  id: number;
  color: string;
  size: string;
  sku: string;
  price: number;
  stockQuantity: number;
}

export interface ProductDetail {
  id: number;
  name: string;
  slug: string;
  description: string | null;
  brandId: number;
  brandName: string;
  categoryId: number;
  categoryName: string;
  gender: string;
  basePrice: number;
  images: ProductImage[];
  variants: ProductVariant[];
}