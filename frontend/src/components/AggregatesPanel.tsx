import React, { useState } from 'react';
import { BarChart, Bar, XAxis, YAxis, Tooltip, ResponsiveContainer, Cell } from 'recharts';
import { AggregatesResult } from '../types';

interface Props { data: AggregatesResult | null; loading: boolean; }

const fmt = (n: number) =>
    Math.abs(n) >= 1e6 ? `$${(n / 1e6).toFixed(1)}M` :
        Math.abs(n) >= 1e3 ? `$${(n / 1e3).toFixed(0)}K` : `$${n.toFixed(0)}`;

const COLOURS = ['#6366f1', '#34d399', '#f59e0b', '#f87171', '#38bdf8', '#a78bfa'];
const AggregatesPanel: React.FC<Props> = ({ data, loading }) => {
    const [tab, setTab] = useState<'book' | 'commodity' | 'region'>('book');
    const rows = tab === 'book' ? data?.byBook : tab === 'commodity' ? data?.byCommodity : data?.byRegion;

    return (
        <div style={{ background: '#111827', borderBottom: '1px solid #1f2937', padding: '16px 20px' }}>
            <div style={{ display: 'flex', gap: 10, marginBottom: 14, alignItems: 'center' }}>
                <span style={{ color: '#9ca3af', fontSize: 13, fontWeight: 600 }}>GROUP BY</span>
                {(['book', 'commodity', 'region'] as const).map(t => (
                    <button key={t} onClick={() => setTab(t)} style={{
                        padding: '5px 14px', borderRadius: 6, cursor: 'pointer', fontSize: 13,
                        background: tab === t ? '#4f46e5' : 'transparent',
                        border: `1px solid ${tab === t ? '#6366f1' : '#374151'}`,
                        color: tab === t ? '#fff' : '#9ca3af'
                    }}>{t.charAt(0).toUpperCase() + t.slice(1)}</button>
                ))}
                {loading && <span style={{ color: '#6b7280', fontSize: 12 }}>Refreshing…</span>}
            </div>

            <div style={{ display: 'flex', gap: 24, overflowX: 'auto' }}>
                <div style={{ display: 'flex', gap: 12, flexShrink: 0 }}>
                    {rows?.map((r, i) => (
                        <div key={r.group} style={{
                            background: '#1f2937', border: '1px solid #374151',
                            borderRadius: 10, padding: '12px 18px', minWidth: 160
                        }}>
                            <div style={{ fontSize: 12, color: '#6b7280', marginBottom: 6 }}>{r.group}</div>
                            <div style={{ fontSize: 18, fontWeight: 700, color: r.totalPnl >= 0 ? '#34d399' : '#f87171' }}>
                                {fmt(r.totalPnl)}
                            </div>
                            <div style={{ fontSize: 11, color: '#6b7280', marginTop: 4 }}>
                                FX: {fmt(r.totalFx)} · {r.rowCount.toLocaleString()} trades
                            </div>
                        </div>
                    ))}
                </div>

                {rows && rows.length > 0 && (
                    <div style={{ flex: 1, minWidth: 300, height: 120 }}>
                        <ResponsiveContainer width="100%" height="100%">
                            <BarChart data={rows} margin={{ top: 4, right: 8, left: 0, bottom: 0 }}>
                                <XAxis dataKey="group" tick={{ fill: '#6b7280', fontSize: 11 }} axisLine={false} tickLine={false} />
                                <YAxis hide />
                                <Tooltip contentStyle={{ background: '#1f2937', border: '1px solid #374151', borderRadius: 8 }}
                                    formatter={(v: any) => [fmt(v as number), 'P&L']}
                                />
                                <Bar dataKey="totalPnl" radius={[4, 4, 0, 0]}>
                                    {rows.map((r, i) => (
                                        <Cell key={r.group} fill={r.totalPnl >= 0 ? COLOURS[i % COLOURS.length] : '#f87171'} />
                                    ))}
                                </Bar>
                            </BarChart>
                        </ResponsiveContainer>
                    </div>
                )}
            </div>
        </div>
    );
};

export default AggregatesPanel;