import api from './client';
import type {
  Clan,
  ClanSearchParametri,
  FitnesUsluga,
  LoginResponse,
  Racun,
  RacunSearchParametri,
  TerminTreninga,
  TipClanarine,
} from '../types';

// --- Auth ---
export const authApi = {
  login: (email: string, password: string) =>
    api.post<LoginResponse>('/auth/login', { email, password }).then((r) => r.data),
};

// --- Članovi ---
export const clanoviApi = {
  getAll: () => api.get<Clan[]>('/clanovi').then((r) => r.data),
  getById: (id: number) => api.get<Clan>(`/clanovi/${id}`).then((r) => r.data),
  create: (clan: Omit<Clan, 'idClan'>) => api.post<Clan>('/clanovi', clan).then((r) => r.data),
  update: (id: number, clan: Clan) => api.put(`/clanovi/${id}`, clan).then((r) => r.data),
  remove: (id: number) => api.delete(`/clanovi/${id}`).then((r) => r.data),
  search: (parametri: ClanSearchParametri) =>
    api.post<Clan[]>('/clanovi/search', parametri).then((r) => r.data),
};

// --- Računi ---
export const racuniApi = {
  getAll: () => api.get<Racun[]>('/racuni').then((r) => r.data),
  getById: (id: number) => api.get<Racun>(`/racuni/${id}`).then((r) => r.data),
  getWithStavke: (id: number) => api.get<Racun>(`/racuni/${id}/stavke`).then((r) => r.data),
  create: (racun: Partial<Racun>) => api.post<Racun>('/racuni', racun).then((r) => r.data),
  search: (parametri: RacunSearchParametri) =>
    api.post<Racun[]>('/racuni/search', parametri).then((r) => r.data),
};

// --- Termini ---
export const terminiApi = {
  create: (termin: Partial<TerminTreninga>, statusOpis: string) =>
    api
      .post<TerminTreninga>(`/termini?statusOpis=${encodeURIComponent(statusOpis)}`, termin)
      .then((r) => r.data),
};

// --- Šifarnici ---
export const lookupApi = {
  tipoviClanarina: () => api.get<TipClanarine[]>('/tipovi-clanarina').then((r) => r.data),
  usluge: () => api.get<FitnesUsluga[]>('/usluge').then((r) => r.data),
};
