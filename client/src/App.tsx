import ProductListPage from './pages/ProductListPage';
import { Route, Routes } from 'react-router-dom';
import ProductDetailPage from './pages/ProductDetailPage';

export default function App() {
  return (
    <Routes>
      <Route path="/" element={<ProductListPage />} />
      <Route path="/products/:slug" element={<ProductDetailPage />} />
    </Routes>
  );
}