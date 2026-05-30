import { NavLink, Outlet, useLocation, useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

const NAV = [
  { to: '/', label: 'Kontrolna tabla', icon: '📊', end: true },
  { to: '/clanovi', label: 'Članovi', icon: '🧑‍🤝‍🧑' },
  { to: '/racuni', label: 'Računi', icon: '🧾' },
  { to: '/termini', label: 'Termini', icon: '🗓️' },
];

const TITLES: Record<string, string> = {
  '/': 'Kontrolna tabla',
  '/clanovi': 'Članovi',
  '/racuni': 'Računi',
  '/termini': 'Termini treninga',
};

export default function Layout() {
  const { admin, logout } = useAuth();
  const navigate = useNavigate();
  const location = useLocation();

  const title = TITLES[location.pathname] ?? 'FitCenter';
  const initials = (admin?.imePrezime || 'A')
    .split(' ')
    .map((p) => p[0])
    .slice(0, 2)
    .join('')
    .toUpperCase();

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  return (
    <div className="app-shell">
      <aside className="sidebar">
        <div className="brand">
          <div className="brand-logo">💪</div>
          <div>
            <div className="brand-name">FitCenter</div>
            <div className="brand-sub">Evidencija &amp; Naplata</div>
          </div>
        </div>

        <nav className="sidebar-nav">
          <div className="nav-section">Meni</div>
          {NAV.map((item) => (
            <NavLink
              key={item.to}
              to={item.to}
              end={item.end}
              className={({ isActive }) => `nav-link${isActive ? ' active' : ''}`}
            >
              <span className="nav-icon">{item.icon}</span>
              {item.label}
            </NavLink>
          ))}
        </nav>

        <div className="sidebar-footer">
          <div className="user-chip">
            <div className="avatar">{initials}</div>
            <div style={{ overflow: 'hidden' }}>
              <div className="user-name">{admin?.imePrezime}</div>
              <div className="user-email">{admin?.email}</div>
            </div>
          </div>
          <button className="btn btn-secondary btn-block btn-sm" onClick={handleLogout}>
            Odjava
          </button>
        </div>
      </aside>

      <div className="main">
        <header className="topbar">
          <div className="page-title">{title}</div>
          <div className="muted" style={{ fontSize: 13 }}>
            {new Date().toLocaleDateString('sr-RS', {
              weekday: 'long',
              day: 'numeric',
              month: 'long',
              year: 'numeric',
            })}
          </div>
        </header>
        <main className="content">
          <Outlet />
        </main>
      </div>
    </div>
  );
}
