import React, { useEffect, useState, useCallback } from 'react';
import { api } from './services/api';
import { AggregatesResult, FilterOptions, QueryParams, RiskPosition } from './types';
import FilterBar from './components/FilterBar';
import AggregatesPanel from './components/AggregatesPanel';
import RiskGrid from './components/RiskGrid';
import DetailPanel from './components/DetailPanel';
const App: React.FC = () => {
    const [filterOptions, setFilterOptions] = useState<FilterOptions | null>(null);
    const [filters, setFilters] = useState<QueryParams>({ page: 0, pageSize: 100 });
    const [aggregates, setAggregates] = useState<AggregatesResult | null>(null);
    const [aggLoading, setAggLoading] = useState(false);
    const [selectedRow, setSelectedRow] = useState<RiskPosition | null>(null);
    const [totalRows, setTotalRows] = useState(0);

    useEffect(() => {
        api.getFilterOptions().then(setFilterOptions).catch(console.error);
    }, []);
    useEffect(() => {
        setAggLoading(true);
        api.getAggregates(filters)
            .then(setAggregates)
            .catch(console.error)
            .finally(() => setAggLoading(false));
    }, [filters]);

    const handleFiltersChange = useCallback((f: QueryParams) => setFilters(f), []);

    return (
        <div style={{
            display: 'flex', flexDirection: 'column', height: '100vh',
            background: '#0d1117', fontFamily: "'Inter','Segoe UI',sans-serif"
        }}>
            <div style={{
                background: '#111827', borderBottom: '1px solid #1f2937',
                padding: '0 20px', height: 56, display: 'flex', alignItems: 'center', gap: 16, flexShrink: 0
            }}>
                <div style={{
                    width: 28, height: 28, background: 'linear-gradient(135deg,#6366f1,#8b5cf6)', borderRadius: 7, display: 'flex', alignItems: 'center', justifyContent: 'center',
                    fontSize: 14, fontWeight: 700, color: '#fff'
                }}>C</div>
                <span style={{ color: '#f9fafb', fontWeight: 700, fontSize: 16 }}>CRR Platform</span>
                <span style={{ color: '#4b5563', fontSize: 14 }}>/ Risk Positions</span>
                <span style={{ marginLeft: 'auto', color: '#6b7280', fontSize: 13 }}>
                    {totalRows > 0 && `${totalRows.toLocaleString()} positions`}
                </span>
            </div>

            <FilterBar options={filterOptions} filters={filters} onChange={handleFiltersChange} />
            <AggregatesPanel data={aggregates} loading={aggLoading} />
            <RiskGrid filters={filters} onRowClick={setSelectedRow}
                onTotalChange={setTotalRows} />
            <DetailPanel row={selectedRow} onClose={() => setSelectedRow(null)} />
        </div>
    );
};

export default App;