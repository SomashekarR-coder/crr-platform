import React from 'react';
import { RiskPosition } from '../types';

interface Props { row: RiskPosition | null; onClose: () => void; }

const fmt = (n: number) =>
    new Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD', maximumFractionDigits: 0 }).format(n);

const DetailPanel: React.FC<Props> = ({ row, onClose }) => {
    if (!row) return null;

    const fields: [string, string][] = [
        ['Trade ID', row.tradeId], ['Book', row.book],
        ['Commodity', row.commodity], ['Counterparty', row.counterparty],
        ['Region', row.region], ['Instrument', row.instrumentType],
        ['Currency', row.currency], ['Position', row.position.toLocaleString()],
        ['Market Price', fmt(row.marketPrice)], ['P&L (USD)', fmt(row.pnl)],
        ['FX Exposure', fmt(row.fxExposure)], ['As-of Date', row.asOfDate],
    ];
    return (
        <div onClick={onClose} style={{
            position: 'fixed', inset: 0, background: 'rgba(0,0,0,0.6)',
            display: 'flex', alignItems: 'center', justifyContent: 'center', zIndex: 1000
        }}>
            <div onClick={e => e.stopPropagation()} style={{
                background: '#1f2937', border: '1px solid #374151', borderRadius: 12,
                padding: 32, minWidth: 420, boxShadow: '0 25px 60px rgba(0,0,0,0.5)'
            }}>
                <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: 24 }}>
                    <h2 style={{ margin: 0, color: '#f9fafb', fontSize: 18 }}>Position Detail</h2>
                    <button onClick={onClose} style={{
                        background: 'transparent', border: 'none',
                        color: '#6b7280', fontSize: 22, cursor: 'pointer'
                    }}>×</button>
                </div>
                <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '12px 24px' }}>
                    {fields.map(([label, value]) => (
                        <div key={label}>
                            <div style={{ fontSize: 11, color: '#6b7280', marginBottom: 2 }}>{label}</div>
                            <div style={{
                                fontSize: 14, fontWeight: 500,
                                color: label.includes('P&L') ? (row.pnl >= 0 ? '#34d399' : '#f87171') : '#e5e7eb'
                            }}>{value}</div>
                        </div>
                    ))}
                </div>
            </div>
        </div>
    );
};

export default DetailPanel;