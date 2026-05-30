import { useEffect, useMemo, useState, type FormEvent } from 'react';
import { clanoviApi, lookupApi } from '../api/services';
import { getErrorMessage } from '../api/client';
import { useToast } from '../components/Toast';
import Modal from '../components/Modal';
import { EmptyState, ErrorAlert, Loading } from '../components/common';
import type { Clan, TipClanarine } from '../types';

const prazanClan: Clan = {
  idClan: 0,
  ime: '',
  prezime: '',
  brojTelefona: '',
  email: '',
  password: '',
  idTipClanarine: 0,
};

export default function Clanovi() {
  const { notify } = useToast();
  const [clanovi, setClanovi] = useState<Clan[]>([]);
  const [tipovi, setTipovi] = useState<TipClanarine[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  // pretraga
  const [searchIme, setSearchIme] = useState('');
  const [searchPrezime, setSearchPrezime] = useState('');
  const [searchEmail, setSearchEmail] = useState('');
  const [searchTip, setSearchTip] = useState('');

  // modal forme
  const [modalOpen, setModalOpen] = useState(false);
  const [form, setForm] = useState<Clan>(prazanClan);
  const [saving, setSaving] = useState(false);
  const [formError, setFormError] = useState('');

  // brisanje
  const [deleting, setDeleting] = useState<Clan | null>(null);
  const [deleteLoading, setDeleteLoading] = useState(false);

  const tipMap = useMemo(() => {
    const m = new Map<number, string>();
    tipovi.forEach((t) => m.set(t.idTipClanarine, t.naziv));
    return m;
  }, [tipovi]);

  const loadAll = async () => {
    setLoading(true);
    setError('');
    try {
      const [c, t] = await Promise.all([clanoviApi.getAll(), lookupApi.tipoviClanarina()]);
      setClanovi(c);
      setTipovi(t);
    } catch (err) {
      setError(getErrorMessage(err));
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadAll();
  }, []);

  const handleSearch = async (e: FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError('');
    try {
      const rezultat = await clanoviApi.search({
        ime: searchIme || undefined,
        prezime: searchPrezime || undefined,
        email: searchEmail || undefined,
        idTipClanarine: searchTip ? Number(searchTip) : undefined,
      });
      setClanovi(rezultat);
    } catch (err) {
      setError(getErrorMessage(err));
    } finally {
      setLoading(false);
    }
  };

  const resetSearch = () => {
    setSearchIme('');
    setSearchPrezime('');
    setSearchEmail('');
    setSearchTip('');
    loadAll();
  };

  const openCreate = () => {
    setForm({ ...prazanClan, idTipClanarine: tipovi[0]?.idTipClanarine ?? 0 });
    setFormError('');
    setModalOpen(true);
  };

  const openEdit = (clan: Clan) => {
    setForm({ ...clan });
    setFormError('');
    setModalOpen(true);
  };

  const handleSave = async (e: FormEvent) => {
    e.preventDefault();
    setSaving(true);
    setFormError('');
    try {
      if (form.idClan === 0) {
        await clanoviApi.create(form);
        notify('Član je uspešno dodat.');
      } else {
        await clanoviApi.update(form.idClan, form);
        notify('Član je uspešno izmenjen.');
      }
      setModalOpen(false);
      await loadAll();
    } catch (err) {
      setFormError(getErrorMessage(err));
    } finally {
      setSaving(false);
    }
  };

  const handleDelete = async () => {
    if (!deleting) return;
    setDeleteLoading(true);
    try {
      await clanoviApi.remove(deleting.idClan);
      notify('Član je obrisan.');
      setDeleting(null);
      await loadAll();
    } catch (err) {
      notify(getErrorMessage(err), 'error');
    } finally {
      setDeleteLoading(false);
    }
  };

  return (
    <>
      <div className="page-header">
        <div>
          <h1>Članovi</h1>
          <p>Upravljanje članovima fitnes centra.</p>
        </div>
        <button className="btn btn-primary" onClick={openCreate}>
          + Novi član
        </button>
      </div>

      <form className="toolbar" onSubmit={handleSearch}>
        <div className="field">
          <label>Ime</label>
          <input className="input" value={searchIme} onChange={(e) => setSearchIme(e.target.value)} />
        </div>
        <div className="field">
          <label>Prezime</label>
          <input
            className="input"
            value={searchPrezime}
            onChange={(e) => setSearchPrezime(e.target.value)}
          />
        </div>
        <div className="field">
          <label>Email</label>
          <input
            className="input"
            value={searchEmail}
            onChange={(e) => setSearchEmail(e.target.value)}
          />
        </div>
        <div className="field">
          <label>Tip članarine</label>
          <select className="input" value={searchTip} onChange={(e) => setSearchTip(e.target.value)}>
            <option value="">Sve</option>
            {tipovi.map((t) => (
              <option key={t.idTipClanarine} value={t.idTipClanarine}>
                {t.naziv}
              </option>
            ))}
          </select>
        </div>
        <button type="submit" className="btn btn-secondary">
          🔍 Pretraži
        </button>
        <button type="button" className="btn btn-ghost" onClick={resetSearch}>
          Poništi
        </button>
      </form>

      {error && <ErrorAlert message={error} />}

      <div className="card">
        {loading ? (
          <Loading />
        ) : clanovi.length === 0 ? (
          <EmptyState icon="🧑‍🤝‍🧑" text="Nema članova za prikaz." />
        ) : (
          <div className="table-wrap">
            <table className="table">
              <thead>
                <tr>
                  <th>#</th>
                  <th>Ime i prezime</th>
                  <th>Telefon</th>
                  <th>Email</th>
                  <th>Tip članarine</th>
                  <th></th>
                </tr>
              </thead>
              <tbody>
                {clanovi.map((c) => (
                  <tr key={c.idClan}>
                    <td className="muted">#{c.idClan}</td>
                    <td>
                      <strong>
                        {c.ime} {c.prezime}
                      </strong>
                    </td>
                    <td>{c.brojTelefona}</td>
                    <td>{c.email}</td>
                    <td>
                      <span className="badge badge-primary">
                        {tipMap.get(c.idTipClanarine) ?? `Tip ${c.idTipClanarine}`}
                      </span>
                    </td>
                    <td>
                      <div className="actions">
                        <button className="btn btn-ghost btn-sm" onClick={() => openEdit(c)}>
                          ✏️ Izmeni
                        </button>
                        <button
                          className="btn btn-ghost btn-sm"
                          style={{ color: 'var(--danger)' }}
                          onClick={() => setDeleting(c)}
                        >
                          🗑 Obriši
                        </button>
                      </div>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
      </div>

      {modalOpen && (
        <Modal
          title={form.idClan === 0 ? 'Novi član' : 'Izmena člana'}
          onClose={() => setModalOpen(false)}
          footer={
            <>
              <button className="btn btn-secondary" onClick={() => setModalOpen(false)}>
                Otkaži
              </button>
              <button className="btn btn-primary" onClick={handleSave} disabled={saving}>
                {saving ? 'Čuvanje...' : 'Sačuvaj'}
              </button>
            </>
          }
        >
          {formError && <ErrorAlert message={formError} />}
          <form onSubmit={handleSave}>
            <div className="form-grid">
              <div className="field">
                <label>Ime</label>
                <input
                  className="input"
                  value={form.ime}
                  onChange={(e) => setForm({ ...form, ime: e.target.value })}
                  required
                />
              </div>
              <div className="field">
                <label>Prezime</label>
                <input
                  className="input"
                  value={form.prezime}
                  onChange={(e) => setForm({ ...form, prezime: e.target.value })}
                  required
                />
              </div>
            </div>
            <div className="form-grid">
              <div className="field">
                <label>Broj telefona</label>
                <input
                  className="input"
                  value={form.brojTelefona}
                  onChange={(e) => setForm({ ...form, brojTelefona: e.target.value })}
                  required
                />
              </div>
              <div className="field">
                <label>Email</label>
                <input
                  type="email"
                  className="input"
                  value={form.email}
                  onChange={(e) => setForm({ ...form, email: e.target.value })}
                  required
                />
              </div>
            </div>
            <div className="form-grid">
              <div className="field">
                <label>Lozinka</label>
                <input
                  type="text"
                  className="input"
                  value={form.password}
                  onChange={(e) => setForm({ ...form, password: e.target.value })}
                  required
                />
              </div>
              <div className="field">
                <label>Tip članarine</label>
                <select
                  className="input"
                  value={form.idTipClanarine}
                  onChange={(e) => setForm({ ...form, idTipClanarine: Number(e.target.value) })}
                  required
                >
                  <option value={0} disabled>
                    Izaberite tip
                  </option>
                  {tipovi.map((t) => (
                    <option key={t.idTipClanarine} value={t.idTipClanarine}>
                      {t.naziv}
                    </option>
                  ))}
                </select>
              </div>
            </div>
            <button type="submit" hidden />
          </form>
        </Modal>
      )}

      {deleting && (
        <Modal
          title="Brisanje člana"
          onClose={() => setDeleting(null)}
          footer={
            <>
              <button className="btn btn-secondary" onClick={() => setDeleting(null)}>
                Otkaži
              </button>
              <button className="btn btn-danger" onClick={handleDelete} disabled={deleteLoading}>
                {deleteLoading ? 'Brisanje...' : 'Obriši'}
              </button>
            </>
          }
        >
          <p>
            Da li ste sigurni da želite da obrišete člana{' '}
            <strong>
              {deleting.ime} {deleting.prezime}
            </strong>
            ? Ova akcija je nepovratna.
          </p>
        </Modal>
      )}
    </>
  );
}
