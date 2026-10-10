import React from 'react';
import { FilterOptions, QueryParams } from '../types';

interface Props {
    options: FilterOptions | null;
    filters: QueryParams;
    onChange: (filters: QueryParams) => void;
}

const FilterBar: React.FC<Props> = ({ options, filters, onChange }) => {
    const set = (key: keyof QueryParams, value: string) =>
        onChange({ ...filters, [key]: value || undefined, page: 0 });

    const selectStyle: React.CSSProperties = {
        background: '#1e2530', border: '1px solid #374151', borderRadius: 6,
        color: '#e5e7eb', padding: '6px 10px', fontSize: 13, cursor: 'pointer'
    };
    return (
        <div style={{
            display: 'flex', flexWrap: 'wrap', gap: 16, alignItems: 'flex-end',
            background: '#111827', padding: '14px 20px', borderBottom: '1px solid #1f2937'
        }}>
            <label style={{ display: 'flex', flexDirection: 'column', gap: 2, fontSize: 12, color: '#9ca3af' }}>
                Search
                <input
                    style={{ ...selectStyle, width: 180 }}
                    placeholder="Counterparty / commodity…"
                    value={filters.search ?? ''}
                    onChange={e => set('search', e.target.value)}
                />
            </label>

            {([
                ['book', 'Book', options?.books],
                ['commodity', 'Commodity', options?.commodities],
                ['region', 'Region', options?.regions],
                ['instrumentType', 'Instrument', options?.instrumentTypes],
                ['currency', 'Currency', options?.currencies],
            ] as [keyof QueryParams, string, string[] | undefined][]).map(([key, label, vals]) => (
                <label key={key} style={{ display: 'flex', flexDirection: 'column', gap: 2, fontSize: 12, color: '#9ca3af' }}>
                    {label}
                    <select
                        style={selectStyle}
                        value={(filters[key] as string) ?? ''}
                        onChange={e => set(key, e.target.value)}
                    >
                        <option value="">All</option>
                        {vals?.map(v => <option key={v} value={v}>{v}</option>)}
                    </select>
                </label>
            ))}
            <button
                onClick={() => onChange({ page: 0, pageSize: 100 })}
                style={{
                    background: '#374151', border: '1px solid #4b5563', borderRadius: 6,
                    color: '#9ca3af', padding: '6px 14px', cursor: 'pointer', fontSize: 13, alignSelf: 'flex-end'
                }}
            >Clear</button>
        </div>
    );
};

export default FilterBar;