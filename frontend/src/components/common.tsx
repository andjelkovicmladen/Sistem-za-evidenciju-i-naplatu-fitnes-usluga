export function Loading({ text = 'Učitavanje...' }: { text?: string }) {
  return (
    <div className="center-box">
      <div className="spinner" />
      <span>{text}</span>
    </div>
  );
}

export function EmptyState({ icon = '📭', text }: { icon?: string; text: string }) {
  return (
    <div className="empty">
      <div className="empty-icon">{icon}</div>
      <div>{text}</div>
    </div>
  );
}

export function ErrorAlert({ message }: { message: string }) {
  return <div className="alert alert-error">{message}</div>;
}
