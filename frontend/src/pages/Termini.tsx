import { useEffect, useMemo, useState, type FormEvent } from 'react';
import { lookupApi, terminiApi } from '../api/services';
import { getErrorMessage } from '../api/client';
import { useToast } from '../components/Toast';
import { ErrorAlert, Loading } from '../components/common';
import { formatDateTime, TIP_USLUGE_LABEL } from '../utils/format';
import type { FitnesUsluga, TerminTreninga } from '../types';

const STATUSI = ['Zakazan', 'Otkazan', 'Završen'];

function sadISO(): string {
  const d = new Date();
  d.setMinutes(d.getMinutes() - d.getTimezoneOffset());
  return d.toISOString().slice(0, 16);
}

export default function Termini() {
  const { notify } = useToast();
  const [usluge, setUsluge] = useState<FitnesUsluga[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  const [idUsluga, setIdUsluga] = useState('');
  const [datumVreme, setDatumVreme] = useState(sadISO());
  const [trajanje, setTrajanje] = useState(60);
  const [status, setStatus] = useState('Zakazan');
  const [saving, setSaving] = useState(false);
  const [formError, setFormError] = useState('');

  const [zakazani, setZakazani] = useState<TerminTreninga[]>([]);

  const uslugaMap = useMemo(() => {
    const m = new Map<number, FitnesUsluga>();
    usluge.forEach((u) => m.set(u.idFitnesUsluga, u));
    return m;
  }, [usluge]);

  useEffect(() => {
    (async () => {
      try {
        const u = await lookupApi.usluge();
        setUsluge(u);
        if (u[0]) setIdUsluga(String(u[0].idFitnesUsluga));
      } catch (err) {
        setError(getErrorMessage(err));
      } finally {
        setLoading(false);
      }
    })();
  }, []);

  const handleSubmit = async (e: FormEvent) => {
    e.preventDefault();
    if (!idUsluga) {
      setFormError('Izaberite uslugu.');
      return;
    }
    setSaving(true);
    setFormError('');
    try {
      const kreiran = await terminiApi.create(
        {
          idFitnesUsluga: Number(idUsluga),
          datumVreme,
          trajanjeMinuta: trajanje,
        },
        status,
      );
      notify('Termin je uspešno zakazan.');
      setZakazani((prev) => [
        { ...kreiran, idFitnesUsluga: Number(idUsluga), datumVreme, trajanjeMinuta: trajanje, statusOpis: status },
        ...prev,
      ]);
    } catch (err) {
      setFormError(getErrorMessage(err));
    } finally {
      setSaving(false);
    }
  };

  if (loading) return <Loading />;

  return (
    <>
      <div className="page-header">
        <div>
          <h1>Termini treninga</h1>
          <p>Zakazivanje termina za fitnes usluge.</p>
        </div>
      </div>

      {error && <ErrorAlert message={error} />}

      <div style={{ display: 'grid', gridTemplateColumns: '380px 1fr', gap: 24, alignItems: 'start' }}>
        <div className="card card-pad">
          <h3 style={{ fontSize: 16, marginBottom: 18 }}>Novi termin</h3>
          {formError && <ErrorAlert message={formError} />}
          <form onSubmit={handleSubmit}>
            <div className="field">
              <label>Fitnes usluga</label>
              <select className="input" value={idUsluga} onChange={(e) => setIdUsluga(e.target.value)} required>
                <option value="" disabled>
                  Izaberite uslugu
                </option>
                {usluge.map((u) => (
                  <option key={u.idFitnesUsluga} value={u.idFitnesUsluga}>
                    {u.naziv} — {TIP_USLUGE_LABEL[u.tipUsluge]}
                  </option>
                ))}
              </select>
            </div>
            <div className="field">
              <label>Datum i vreme</label>
              <input
                type="datetime-local"
                className="input"
                value={datumVreme}
                onChange={(e) => setDatumVreme(e.target.value)}
                required
              />
            </div>
            <div className="field">
              <label>Trajanje (minuta)</label>
              <input
                type="number"
                min={15}
                step={15}
                className="input"
                value={trajanje}
                onChange={(e) => setTrajanje(Number(e.target.value))}
                required
              />
            </div>
            <div className="field">
              <label>Status</label>
              <select className="input" value={status} onChange={(e) => setStatus(e.target.value)}>
                {STATUSI.map((s) => (
                  <option key={s} value={s}>
                    {s}
                  </option>
                ))}
              </select>
            </div>
            <button type="submit" className="btn btn-primary btn-block" disabled={saving}>
              {saving ? 'Zakazivanje...' : 'Zakaži termin'}
            </button>
          </form>
        </div>

        <div className="card">
          <div className="card-pad">
            <h3 style={{ fontSize: 16 }}>Zakazano u ovoj sesiji</h3>
            <p className="muted" style={{ fontSize: 13, marginTop: 4 }}>
              API ne nudi listanje termina, pa se ovde prikazuju samo termini zakazani tokom trenutne sesije.
            </p>
          </div>
          <div className="table-wrap">
            <table className="table">
              <thead>
                <tr>
                  <th>Usluga</th>
                  <th>Datum i vreme</th>
                  <th className="right">Trajanje</th>
                  <th>Status</th>
                </tr>
              </thead>
              <tbody>
                {zakazani.length === 0 ? (
                  <tr>
                    <td colSpan={4} className="muted" style={{ textAlign: 'center' }}>
                      Još uvek nema zakazanih termina.
                    </td>
                  </tr>
                ) : (
                  zakazani.map((t, i) => (
                    <tr key={i}>
                      <td>{uslugaMap.get(t.idFitnesUsluga)?.naziv ?? t.idFitnesUsluga}</td>
                      <td>{formatDateTime(t.datumVreme)}</td>
                      <td className="right nowrap">{t.trajanjeMinuta} min</td>
                      <td>
                        <span className="badge badge-info">{t.statusOpis}</span>
                      </td>
                    </tr>
                  ))
                )}
              </tbody>
            </table>
          </div>
        </div>
      </div>
    </>
  );
}
