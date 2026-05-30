import type { ReactNode } from 'react';

interface ModalProps {
  title: string;
  onClose: () => void;
  children: ReactNode;
  footer?: ReactNode;
  large?: boolean;
}

export default function Modal({ title, onClose, children, footer, large }: ModalProps) {
  return (
    <div className="modal-overlay" onMouseDown={onClose}>
      <div
        className={`modal${large ? ' modal-lg' : ''}`}
        onMouseDown={(e) => e.stopPropagation()}
      >
        <div className="modal-header">
          <h2>{title}</h2>
          <button className="btn btn-ghost" onClick={onClose} aria-label="Zatvori">
            ✕
          </button>
        </div>
        <div className="modal-body">{children}</div>
        {footer && <div className="modal-footer">{footer}</div>}
      </div>
    </div>
  );
}
