import { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { clanoviApi, racuniApi } from '../api/services';
import { getErrorMessage } from '../api/client';
import { useAuth } from '../context/AuthContext';
import { Loading, ErrorAlert } from '../components/common';
import { formatRsd, formatDate } from '../utils/format';
import type { Clan, Racun } from '../types';

export default function Dashboard() {
  const { admin } = useAuth();
  const [clanovi, setClanovi] = useState<Clan[]>([]);
  const [racuni, setRacuni] = useState<Racun[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    (async () => {
      try {
        const [c, r] = await Promise.all([clanoviApi.getAll(), racuniApi.getAll()]);
        setClanovi(c);
        setRacuni(r);
      } catch (err) {
        setError(getErrorMessage(err));
      } finally {
        setLoading(false);
      }
    })();
  }, []);

  if (loading) return <Loading />;

  const ukupanPrihod = racuni.reduce((sum, r) => sum + (r.ukupanIznos ?? r.iznos ?? 0), 0);
  const poslednji = [...racuni]
    .sort((a, b) => new Date(b.datumIzdavanja).getTime() - new Date(a.datumIzdavanja).getTime())
    .slice(0, 5);

  const stats = [
    { label: 'Ukupno članova', value: clanovi.length, icon: '🧑‍🤝‍🧑', bg: 'var(--info-soft)' },
    { label: 'Ukupno računa', value: racuni.length, icon: '🧾', bg: 'var(--primary-soft)' },
    {
      label: 'Ukupan prihod',
      value: formatRsd(ukupanPrihod),
      icon: '💰',
      bg: '#fef3c7',
    },
  ];

  return (
    <>
      <div className="page-header">
        <div>
          <h1>Zdravo, {admin?.imePrezime?.split(' ')[0]} 👋</h1>
          <p>Pregled poslovanja fitnes centra.</p>
        </div>
      </div>

      {error && <ErrorAlert message={error} />}

      <div className="stat-grid">
        {stats.map((s) => (
          <div className="stat-card" key={s.label}>
            <div className="stat-icon" style={{ background: s.bg }}>
              {s.icon}
            </div>
            <div className="stat-value">{s.value}</div>
            <div className="stat-label">{s.label}</div>
          </div>
        ))}
      </div>

      <div className="card">
        <div className="card-pad" style={{ display: 'flex', justifyContent: 'space-between' }}>
          <h3 style={{ fontSize: 16 }}>Poslednji računi</h3>
          <Link to="/racuni" className="btn btn-ghost btn-sm">
            Svi računi →
          </Link>
        </div>
        <div className="table-wrap">
          <table className="table">
            <thead>
              <tr>
                <th>#</th>
                <th>Član</th>
                <th>Datum izdavanja</th>
                <th className="right">Iznos</th>
              </tr>
            </thead>
            <tbody>
              {poslednji.length === 0 ? (
                <tr>
                  <td colSpan={4} className="muted" style={{ textAlign: 'center' }}>
                    Nema računa.
                  </td>
                </tr>
              ) : (
                poslednji.map((r) => (
                  <tr key={r.idRacun}>
                    <td>#{r.idRacun}</td>
                    <td>{r.clanImePrezime ?? `Član ${r.idClan}`}</td>
                    <td>{formatDate(r.datumIzdavanja)}</td>
                    <td className="right nowrap">{formatRsd(r.ukupanIznos ?? r.iznos)}</td>
                  </tr>
                ))
              )}
            </tbody>
          </table>
        </div>
      </div>
    </>
  );
}
