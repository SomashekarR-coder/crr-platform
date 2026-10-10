import React, { useCallback, useRef, useEffect } from 'react';
import { AgGridReact } from 'ag-grid-react';
import { ColDef, GridReadyEvent, IGetRowsParams } from 'ag-grid-community';
import 'ag-grid-community/styles/ag-grid.css';
import 'ag-grid-community/styles/ag-theme-alpine.css';
import { api } from '../services/api';
import { RiskPosition, QueryParams } from '../types';
interface Props {
    filters: QueryParams;
    onRowClick: (row: RiskPosition) => void;
    onTotalChange: (total: number) => void;
}

const fmtUsd = (v: number) =>
    new Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD', maximumFractionDigits: 0 }).format(v);
const RiskGrid: React.FC<Props> = ({ filters, onRowClick, onTotalChange }) => {
    const gridRef = useRef<AgGridReact>(null);
    const filtersRef = useRef<QueryParams>(filters);
    useEffect(() => {
        filtersRef.current = filters;
    }, [filters]);

    const columnDefs: ColDef[] = [
        { field: 'tradeId', headerName: 'Trade ID', width: 130, pinned: 'left' },
        { field: 'book', headerName: 'Book', width: 120 },
        { field: 'commodity', headerName: 'Commodity', width: 140 },
        { field: 'counterparty', headerName: 'Counterparty', width: 160 },
        { field: 'region', headerName: 'Region', width: 100 },
        { field: 'instrumentType', headerName: 'Instrument', width: 120 },
        {
            field: 'position', headerName: 'Position', width: 120,
            valueFormatter: (p: any) => p.value?.toLocaleString(),
            cellStyle: { textAlign: 'right' }
        },
        { field: 'currency', headerName: 'CCY', width: 80 },
        {
            field: 'marketPrice', headerName: 'Mkt Price', width: 130,
            valueFormatter: (p: any) => fmtUsd(p.value),
            cellStyle: { textAlign: 'right' }
        },
        {
            field: 'pnl', headerName: 'P&L (USD)', width: 140,
            valueFormatter: (p: any) => fmtUsd(p.value),
            cellStyle: (p: any) => ({ textAlign: 'right', color: p.value >= 0 ? '#34d399' : '#f87171', fontWeight: 600 })
        },
        {
            field: 'fxExposure', headerName: 'FX Exposure', width: 140,
            valueFormatter: (p: any) => fmtUsd(p.value),
            cellStyle: (p: any) => ({ textAlign: 'right', color: p.value >= 0 ? '#34d399' : '#f87171', fontWeight: 600 })
        },
        { field: 'asOfDate', headerName: 'As-of Date', width: 120 },
    ];

    const getDatasource = useCallback(() => ({
        getRows: async (params: IGetRowsParams) => {
            const sortModel = params.sortModel[0];
            const q: QueryParams = {
                ...filtersRef.current,
                page: Math.floor(params.startRow / 100),
                pageSize: 100,
                sortField: sortModel?.colId,
                sortDir: sortModel?.sort ?? 'asc',
            };
            try {
                const result = await api.getPositions(q);
                onTotalChange(result.total);
                params.successCallback(result.rows, result.total);
            } catch {
                params.failCallback();
            }
        }
    }), [onTotalChange]);

    const onGridReady = useCallback((e: GridReadyEvent) => {
        e.api.setGridOption('datasource', getDatasource());
    }, [getDatasource]);

    useEffect(() => {
        if (gridRef.current?.api) {
            gridRef.current.api.setGridOption('datasource', getDatasource());
        }
    }, [filters, getDatasource]);
    return (
        <div className="ag-theme-alpine-dark" style={{ flex: 1, width: '100%' }}>
            <AgGridReact
                ref={gridRef}
                columnDefs={columnDefs}
                rowModelType="infinite"
                cacheBlockSize={100}
                maxBlocksInCache={20}
                infiniteInitialRowCount={1000}
                onGridReady={onGridReady}
                onRowClicked={e => e.data && onRowClick(e.data)}
                defaultColDef={{ sortable: true, resizable: true }}
                rowStyle={{ cursor: 'pointer' }}
            />
        </div>
    );
};

export default RiskGrid;