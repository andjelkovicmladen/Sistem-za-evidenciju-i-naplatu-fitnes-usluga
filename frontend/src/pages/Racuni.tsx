import { useEffect, useMemo, useState, type FormEvent } from 'react';
import { clanoviApi, lookupApi, racuniApi } from '../api/services';
import { getErrorMessage } from '../api/client';
import { useAuth } from '../context/AuthContext';
import { useToast } from '../components/Toast';
import Modal from '../components/Modal';
import { EmptyState, ErrorAlert, Loading } from '../components/common';
import { formatDate, formatRsd } from '../utils/format';
import type { Clan, FitnesUsluga, Racun, StavkaRacuna } from '../types';

interface NovaStavka {
  idFitnesUsluga: number;
  brojSati: number;
}

function danasISO(offsetDana = 0): string {
  const d = new Date();
  d.setDate(d.getDate() + offsetDana);
  return d.toISOString().slice(0, 10);
}

export default function Racuni() {
  const { admin } = useAuth();
  const { notify } = useToast();
  const [racuni, setRacuni] = useState<Racun[]>([]);
  const [clanovi, setClanovi] = useState<Clan[]>([]);
  const [usluge, setUsluge] = useState<FitnesUsluga[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  const [filterClan, setFilterClan] = useState('');

  // detalji
  const [detaljiId, setDetaljiId] = useState<number | null>(null);
  const [detalji, setDetalji] = useState<Racun | null>(null);
  const [detaljiLoading, setDetaljiLoading] = useState(false);

  // kreiranje
  const [createOpen, setCreateOpen] = useState(false);
  const [novIdClan, setNovIdClan] = useState('');
  const [datumIzdavanja, setDatumIzdavanja] = useState(danasISO());
  const [datumDospeca, setDatumDospeca] = useState(danasISO(30));
  const [stavke, setStavke] = useState<NovaStavka[]>([{ idFitnesUsluga: 0, brojSati: 1 }]);
  const [saving, setSaving] = useState(false);
  const [formError, setFormError] = useState('');

  const uslugaMap = useMemo(() => {
    const m = new Map<number, FitnesUsluga>();
    usluge.forEach((u) => m.set(u.idFitnesUsluga, u));
    return m;
  }, [usluge]);

  const loadAll = async () => {
    setLoading(true);
    setError('');
    try {
      const [r, c, u] = await Promise.all([
        racuniApi.getAll(),
        clanoviApi.getAll(),
        lookupApi.usluge(),
      ]);
      setRacuni(r);
      setClanovi(c);
      setUsluge(u);
    } catch (err) {
      setError(getErrorMessage(err));
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadAll();
  }, []);

  const clanMap = useMemo(() => {
    const m = new Map<number, string>();
    clanovi.forEach((c) => m.set(c.idClan, `${c.ime} ${c.prezime}`));
    return m;
  }, [clanovi]);

  const handleFilter = async (e: FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError('');
    try {
      const rez = await racuniApi.search({ idClan: filterClan ? Number(filterClan) : undefined });
      setRacuni(rez);
    } catch (err) {
      setError(getErrorMessage(err));
    } finally {
      setLoading(false);
    }
  };

  const openDetalji = async (id: number) => {
    setDetaljiId(id);
    setDetalji(null);
    setDetaljiLoading(true);
    try {
      const r = await racuniApi.getWithStavke(id);
      setDetalji(r);
    } catch (err) {
      notify(getErrorMessage(err), 'error');
      setDetaljiId(null);
    } finally {
      setDetaljiLoading(false);
    }
  };

  const closeDetalji = () => {
    setDetaljiId(null);
    setDetalji(null);
  };

  const openCreate = () => {
    setNovIdClan(clanovi[0] ? String(clanovi[0].idClan) : '');
    setDatumIzdavanja(danasISO());
    setDatumDospeca(danasISO(30));
    setStavke([{ idFitnesUsluga: usluge[0]?.idFitnesUsluga ?? 0, brojSati: 1 }]);
    setFormError('');
    setCreateOpen(true);
  };

  const ukupnoNovi = stavke.reduce((sum, s) => {
    const u = uslugaMap.get(s.idFitnesUsluga);
    return sum + (u ? u.cenaPoSatu * s.brojSati : 0);
  }, 0);

  const handleCreate = async (e: FormEvent) => {
    e.preventDefault();
    if (!novIdClan) {
      setFormError('Izaberite člana.');
      return;
    }
    if (stavke.length === 0 || stavke.some((s) => !s.idFitnesUsluga || s.brojSati <= 0)) {
      setFormError('Sve stavke moraju imati uslugu i broj sati veći od nule.');
      return;
    }
    setSaving(true);
    setFormError('');
    try {
      const stavkeRacuna: Partial<StavkaRacuna>[] = stavke.map((s) => {
        const u = uslugaMap.get(s.idFitnesUsluga);
        return {
          idFitnesUsluga: s.idFitnesUsluga,
          brojSati: s.brojSati,
          iznos: u ? u.cenaPoSatu * s.brojSati : 0,
        };
      });
      await racuniApi.create({
        idClan: Number(novIdClan),
        idAdministrator: admin?.idAdministrator ?? 0,
        datumIzdavanja,
        datumDospeca,
        stavkeRacuna: stavkeRacuna as StavkaRacuna[],
      });
      notify('Račun je uspešno kreiran.');
      setCreateOpen(false);
      await loadAll();
    } catch (err) {
      setFormError(getErrorMessage(err));
    } finally {
      setSaving(false);
    }
  };

  return (
    <>
      <div className="page-header">
        <div>
          <h1>Računi</h1>
          <p>Pregled i izdavanje računa članovima.</p>
        </div>
        <button className="btn btn-primary" onClick={openCreate}>
          + Novi račun
        </button>
      </div>

      <form className="toolbar" onSubmit={handleFilter}>
        <div className="field">
          <label>Član</label>
          <select className="input" value={filterClan} onChange={(e) => setFilterClan(e.target.value)}>
            <option value="">Svi članovi</option>
            {clanovi.map((c) => (
              <option key={c.idClan} value={c.idClan}>
                {c.ime} {c.prezime}
              </option>
            ))}
          </select>
        </div>
        <button type="submit" className="btn btn-secondary">
          🔍 Filtriraj
        </button>
        <button
          type="button"
          className="btn btn-ghost"
          onClick={() => {
            setFilterClan('');
            loadAll();
          }}
        >
          Poništi
        </button>
      </form>

      {error && <ErrorAlert message={error} />}

      <div className="card">
        {loading ? (
          <Loading />
        ) : racuni.length === 0 ? (
          <EmptyState icon="🧾" text="Nema računa za prikaz." />
        ) : (
          <div className="table-wrap">
            <table className="table">
              <thead>
                <tr>
                  <th>#</th>
                  <th>Član</th>
                  <th>Administrator</th>
                  <th>Izdat</th>
                  <th>Dospeva</th>
                  <th className="right">Iznos</th>
                  <th></th>
                </tr>
              </thead>
              <tbody>
                {racuni.map((r) => (
                  <tr key={r.idRacun}>
                    <td className="muted">#{r.idRacun}</td>
                    <td>{r.clanImePrezime ?? clanMap.get(r.idClan) ?? `Član ${r.idClan}`}</td>
                    <td>{r.administratorImePrezime ?? `Admin ${r.idAdministrator}`}</td>
                    <td>{formatDate(r.datumIzdavanja)}</td>
                    <td>{formatDate(r.datumDospeca)}</td>
                    <td className="right nowrap">
                      <strong>{formatRsd(r.ukupanIznos ?? r.iznos)}</strong>
                    </td>
                    <td>
                      <div className="actions">
                        <button className="btn btn-ghost btn-sm" onClick={() => openDetalji(r.idRacun)}>
                          👁 Detalji
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

      {detaljiId !== null && (
        <Modal title={`Račun #${detaljiId}`} onClose={closeDetalji} large>
          {detaljiLoading || !detalji ? (
            <Loading />
          ) : (
            <>
              <div className="form-grid" style={{ marginBottom: 8 }}>
                <div className="field">
                  <label>Član</label>
                  <div>{detalji.clanImePrezime ?? clanMap.get(detalji.idClan) ?? detalji.idClan}</div>
                </div>
                <div className="field">
                  <label>Administrator</label>
                  <div>{detalji.administratorImePrezime ?? detalji.idAdministrator}</div>
                </div>
                <div className="field">
                  <label>Datum izdavanja</label>
                  <div>{formatDate(detalji.datumIzdavanja)}</div>
                </div>
                <div className="field">
                  <label>Datum dospeća</label>
                  <div>{formatDate(detalji.datumDospeca)}</div>
                </div>
              </div>

              <div className="section-title">Stavke</div>
              <div className="table-wrap card">
                <table className="table">
                  <thead>
                    <tr>
                      <th>Rb</th>
                      <th>Usluga</th>
                      <th className="right">Sati</th>
                      <th className="right">Iznos</th>
                    </tr>
                  </thead>
                  <tbody>
                    {(detalji.stavkeRacuna ?? []).length === 0 ? (
                      <tr>
                        <td colSpan={4} className="muted" style={{ textAlign: 'center' }}>
                          Nema stavki.
                        </td>
                      </tr>
                    ) : (
                      detalji.stavkeRacuna.map((s) => (
                        <tr key={s.rb}>
                          <td className="muted">{s.rb}</td>
                          <td>{s.fitnesUsluga?.naziv ?? uslugaMap.get(s.idFitnesUsluga)?.naziv ?? s.idFitnesUsluga}</td>
                          <td className="right">{s.brojSati}</td>
                          <td className="right nowrap">{formatRsd(s.iznos)}</td>
                        </tr>
                      ))
                    )}
                  </tbody>
                  <tfoot>
                    <tr>
                      <td colSpan={3} className="right">
                        <strong>Ukupno</strong>
                      </td>
                      <td className="right nowrap">
                        <strong>
                          {formatRsd(
                            detalji.ukupanIznos ??
                              detalji.iznos ??
                              (detalji.stavkeRacuna ?? []).reduce((a, s) => a + s.iznos, 0),
                          )}
                        </strong>
                      </td>
                    </tr>
                  </tfoot>
                </table>
              </div>
            </>
          )}
        </Modal>
      )}

      {createOpen && (
        <Modal
          title="Novi račun"
          onClose={() => setCreateOpen(false)}
          large
          footer={
            <>
              <button className="btn btn-secondary" onClick={() => setCreateOpen(false)}>
                Otkaži
              </button>
              <button className="btn btn-primary" onClick={handleCreate} disabled={saving}>
                {saving ? 'Čuvanje...' : `Sačuvaj (${formatRsd(ukupnoNovi)})`}
              </button>
            </>
          }
        >
          {formError && <ErrorAlert message={formError} />}
          <form onSubmit={handleCreate}>
            <div className="form-grid">
              <div className="field">
                <label>Član</label>
                <select
                  className="input"
                  value={novIdClan}
                  onChange={(e) => setNovIdClan(e.target.value)}
                  required
                >
                  <option value="" disabled>
                    Izaberite člana
                  </option>
                  {clanovi.map((c) => (
                    <option key={c.idClan} value={c.idClan}>
                      {c.ime} {c.prezime}
                    </option>
                  ))}
                </select>
              </div>
              <div className="field">
                <label>Administrator</label>
                <input className="input" value={admin?.imePrezime ?? ''} disabled />
              </div>
              <div className="field">
                <label>Datum izdavanja</label>
                <input
                  type="date"
                  className="input"
                  value={datumIzdavanja}
                  onChange={(e) => setDatumIzdavanja(e.target.value)}
                  required
                />
              </div>
              <div className="field">
                <label>Datum dospeća</label>
                <input
                  type="date"
                  className="input"
                  value={datumDospeca}
                  onChange={(e) => setDatumDospeca(e.target.value)}
                  required
                />
              </div>
            </div>

            <div
              className="section-title"
              style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}
            >
              <span>Stavke</span>
              <button
                type="button"
                className="btn btn-ghost btn-sm"
                onClick={() =>
                  setStavke([...stavke, { idFitnesUsluga: usluge[0]?.idFitnesUsluga ?? 0, brojSati: 1 }])
                }
              >
                + Dodaj stavku
              </button>
            </div>

            {stavke.map((s, idx) => {
              const u = uslugaMap.get(s.idFitnesUsluga);
              const linija = u ? u.cenaPoSatu * s.brojSati : 0;
              return (
                <div
                  key={idx}
                  style={{ display: 'grid', gridTemplateColumns: '1fr 90px 110px 40px', gap: 10, marginBottom: 10, alignItems: 'center' }}
                >
                  <select
                    className="input"
                    value={s.idFitnesUsluga}
                    onChange={(e) => {
                      const copy = [...stavke];
                      copy[idx] = { ...copy[idx], idFitnesUsluga: Number(e.target.value) };
                      setStavke(copy);
                    }}
                  >
                    <option value={0} disabled>
                      Izaberite uslugu
                    </option>
                    {usluge.map((opt) => (
                      <option key={opt.idFitnesUsluga} value={opt.idFitnesUsluga}>
                        {opt.naziv} ({formatRsd(opt.cenaPoSatu)}/h)
                      </option>
                    ))}
                  </select>
                  <input
                    type="number"
                    min={1}
                    className="input"
                    value={s.brojSati}
                    onChange={(e) => {
                      const copy = [...stavke];
                      copy[idx] = { ...copy[idx], brojSati: Number(e.target.value) };
                      setStavke(copy);
                    }}
                  />
                  <div className="right nowrap" style={{ fontWeight: 600 }}>
                    {formatRsd(linija)}
                  </div>
                  <button
                    type="button"
                    className="btn btn-ghost btn-sm"
                    style={{ color: 'var(--danger)' }}
                    disabled={stavke.length === 1}
                    onClick={() => setStavke(stavke.filter((_, i) => i !== idx))}
                  >
                    ✕
                  </button>
                </div>
              );
            })}

            <div
              style={{
                display: 'flex',
                justifyContent: 'flex-end',
                gap: 12,
                paddingTop: 12,
                borderTop: '1px solid var(--border)',
                fontSize: 16,
              }}
            >
              <span className="muted">Ukupno:</span>
              <strong>{formatRsd(ukupnoNovi)}</strong>
            </div>
            <button type="submit" hidden />
          </form>
        </Modal>
      )}
    </>
  );
}
