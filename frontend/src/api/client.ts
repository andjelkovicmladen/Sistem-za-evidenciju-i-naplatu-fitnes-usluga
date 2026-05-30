import axios, { type AxiosError } from 'axios';

export const TOKEN_KEY = 'fc_token';
export const ADMIN_KEY = 'fc_admin';

const api = axios.create({
  baseURL: '/api',
  headers: { 'Content-Type': 'application/json' },
});

api.interceptors.request.use((config) => {
  const token = localStorage.getItem(TOKEN_KEY);
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

api.interceptors.response.use(
  (response) => response,
  (error: AxiosError) => {
    if (error.response?.status === 401 && !location.pathname.startsWith('/login')) {
      localStorage.removeItem(TOKEN_KEY);
      localStorage.removeItem(ADMIN_KEY);
      location.href = '/login';
    }
    return Promise.reject(error);
  },
);

// Izvlači čitljivu poruku greške iz API odgovora
export function getErrorMessage(error: unknown, fallback = 'Došlo je do greške.'): string {
  if (axios.isAxiosError(error)) {
    const data = error.response?.data as { message?: string } | string | undefined;
    if (typeof data === 'string') return data || error.message || fallback;
    if (data?.message) return data.message;
    return error.message || fallback;
  }
  return fallback;
}

export default api;
