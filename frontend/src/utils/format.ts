import { TipUsluge } from '../types';

export function formatRsd(value: number | undefined | null): string {
  const n = value ?? 0;
  return new Intl.NumberFormat('sr-RS', {
    style: 'currency',
    currency: 'RSD',
    maximumFractionDigits: 0,
  }).format(n);
}

export function formatDate(iso: string | undefined | null): string {
  if (!iso) return '—';
  const d = new Date(iso);
  if (isNaN(d.getTime())) return '—';
  return d.toLocaleDateString('sr-RS', { day: '2-digit', month: '2-digit', year: 'numeric' });
}

export function formatDateTime(iso: string | undefined | null): string {
  if (!iso) return '—';
  const d = new Date(iso);
  if (isNaN(d.getTime())) return '—';
  return d.toLocaleString('sr-RS', {
    day: '2-digit',
    month: '2-digit',
    year: 'numeric',
    hour: '2-digit',
    minute: '2-digit',
  });
}

export const TIP_USLUGE_LABEL: Record<TipUsluge, string> = {
  [TipUsluge.GrupniTrening]: 'Grupni trening',
  [TipUsluge.PersonalniTrening]: 'Personalni trening',
  [TipUsluge.Spa]: 'Spa',
};
