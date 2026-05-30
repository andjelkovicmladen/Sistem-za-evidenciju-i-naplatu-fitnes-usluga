import { createContext, useContext, useState, type ReactNode } from 'react';
import { authApi } from '../api/services';
import { ADMIN_KEY, TOKEN_KEY } from '../api/client';
import type { Administrator } from '../types';

interface AuthContextType {
  admin: Administrator | null;
  isAuthenticated: boolean;
  login: (email: string, password: string) => Promise<void>;
  logout: () => void;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

function loadAdmin(): Administrator | null {
  const raw = localStorage.getItem(ADMIN_KEY);
  if (!raw) return null;
  try {
    return JSON.parse(raw) as Administrator;
  } catch {
    return null;
  }
}

export function AuthProvider({ children }: { children: ReactNode }) {
  const [admin, setAdmin] = useState<Administrator | null>(loadAdmin);

  const login = async (email: string, password: string) => {
    const data = await authApi.login(email, password);
    localStorage.setItem(TOKEN_KEY, data.token);
    localStorage.setItem(ADMIN_KEY, JSON.stringify(data.admin));
    setAdmin(data.admin);
  };

  const logout = () => {
    localStorage.removeItem(TOKEN_KEY);
    localStorage.removeItem(ADMIN_KEY);
    setAdmin(null);
  };

  return (
    <AuthContext.Provider value={{ admin, isAuthenticated: !!admin, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
}

// eslint-disable-next-line react-refresh/only-export-components
export function useAuth() {
  const ctx = useContext(AuthContext);
  if (!ctx) throw new Error('useAuth mora biti unutar AuthProvider-a');
  return ctx;
}
