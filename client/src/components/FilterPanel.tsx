import type { FilterOptions, ProductFilters } from '../types';

interface Props {
  options?: FilterOptions;
  filters: ProductFilters;
  onChange: (patch: Partial<ProductFilters>) => void;
  onReset: () => void;
}

const genders = [
  { value: 'Men', label: 'Erkek' },
  { value: 'Women', label: 'Kadın' },
  { value: 'Kids', label: 'Çocuk' },
  { value: 'Unisex', label: 'Unisex' },
];

export default function FilterPanel({ options, filters, onChange, onReset }: Props) {
  const parents = options?.categories.filter((c) => c.parentId === null) ?? [];
  const childrenOf = (id: number) => options?.categories.filter((c) => c.parentId === id) ?? [];

  return (
    <aside className="filters">
      <label>
        Ara
        <input
          type="search"
          placeholder="Ürün adı"
          value={filters.search}
          onChange={(e) => onChange({ search: e.target.value })}
        />
      </label>

      <label>
        Marka
        <select value={filters.brandId} onChange={(e) => onChange({ brandId: e.target.value })}>
          <option value="">Hepsi</option>
          {options?.brands.map((b) => (
            <option key={b.id} value={b.id}>{b.name}</option>
          ))}
        </select>
      </label>

      <label>
        Kategori
        <select value={filters.categoryId} onChange={(e) => onChange({ categoryId: e.target.value })}>
          <option value="">Hepsi</option>
          {parents.map((p) => (
            <optgroup key={p.id} label={p.name}>
              <option value={p.id}>Tüm {p.name}</option>
              {childrenOf(p.id).map((c) => (
                <option key={c.id} value={c.id}>{c.name}</option>
              ))}
            </optgroup>
          ))}
        </select>
      </label>

      <label>
        Cinsiyet
        <select value={filters.gender} onChange={(e) => onChange({ gender: e.target.value })}>
          <option value="">Hepsi</option>
          {genders.map((g) => (
            <option key={g.value} value={g.value}>{g.label}</option>
          ))}
        </select>
      </label>

      <label>
        Renk
        <select value={filters.color} onChange={(e) => onChange({ color: e.target.value })}>
          <option value="">Hepsi</option>
          {options?.colors.map((c) => (
            <option key={c} value={c}>{c}</option>
          ))}
        </select>
      </label>

      <label>
        Beden
        <select value={filters.size} onChange={(e) => onChange({ size: e.target.value })}>
          <option value="">Hepsi</option>
          {options?.sizes.map((s) => (
            <option key={s} value={s}>{s}</option>
          ))}
        </select>
      </label>

      <div className="price-row">
        <label>
          Min fiyat
          <input
            type="number"
            min="0"
            value={filters.minPrice}
            onChange={(e) => onChange({ minPrice: e.target.value })}
          />
        </label>
        <label>
          Max fiyat
          <input
            type="number"
            min="0"
            value={filters.maxPrice}
            onChange={(e) => onChange({ maxPrice: e.target.value })}
          />
        </label>
      </div>

      <label className="check">
        <input
          type="checkbox"
          checked={filters.inStockOnly}
          onChange={(e) => onChange({ inStockOnly: e.target.checked })}
        />
        Sadece stokta olanlar
      </label>

      <button type="button" className="reset" onClick={onReset}>
        Filtreleri temizle
      </button>
    </aside>
  );
}