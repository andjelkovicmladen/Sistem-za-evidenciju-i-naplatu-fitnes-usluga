import { Navigate, Route, Routes } from 'react-router-dom';
import ProtectedRoute from './components/ProtectedRoute';
import Layout from './components/Layout';
import Login from './pages/Login';
import Dashboard from './pages/Dashboard';
import Clanovi from './pages/Clanovi';
import Racuni from './pages/Racuni';
import Termini from './pages/Termini';

function App() {
  return (
    <Routes>
      <Route path="/login" element={<Login />} />
      <Route element={<ProtectedRoute />}>
        <Route element={<Layout />}>
          <Route index element={<Dashboard />} />
          <Route path="clanovi" element={<Clanovi />} />
          <Route path="racuni" element={<Racuni />} />
          <Route path="termini" element={<Termini />} />
        </Route>
      </Route>
      <Route path="*" element={<Navigate to="/" replace />} />
    </Routes>
  );
}

export default App;
