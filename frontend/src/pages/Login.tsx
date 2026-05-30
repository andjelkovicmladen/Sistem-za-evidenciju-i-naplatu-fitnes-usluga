import { useState, type FormEvent } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import { getErrorMessage } from '../api/client';
import { ErrorAlert } from '../components/common';

export default function Login() {
  const { login } = useAuth();
  const navigate = useNavigate();
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (e: FormEvent) => {
    e.preventDefault();
    setError('');
    setLoading(true);
    try {
      await login(email, password);
      navigate('/');
    } catch (err) {
      setError(getErrorMessage(err, 'Prijava nije uspela.'));
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="auth-page">
      <div className="auth-card">
        <div className="auth-brand">
          <div className="brand-logo">💪</div>
          <div>
            <div className="brand-name" style={{ color: 'var(--text)' }}>
              FitCenter
            </div>
            <div className="brand-sub" style={{ color: 'var(--text-faint)' }}>
              Evidencija &amp; Naplata
            </div>
          </div>
        </div>

        <h1>Dobrodošli nazad</h1>
        <p className="subtitle">Prijavite se administratorskim nalogom.</p>

        {error && <ErrorAlert message={error} />}

        <form onSubmit={handleSubmit}>
          <div className="field">
            <label htmlFor="email">Email</label>
            <input
              id="email"
              type="email"
              className="input"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              placeholder="admin@fitcenter.rs"
              required
              autoFocus
            />
          </div>
          <div className="field">
            <label htmlFor="password">Lozinka</label>
            <input
              id="password"
              type="password"
              className="input"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              placeholder="••••••••"
              required
            />
          </div>
          <button type="submit" className="btn btn-primary btn-block" disabled={loading}>
            {loading ? 'Prijavljivanje...' : 'Prijavi se'}
          </button>
        </form>
      </div>
    </div>
  );
}
