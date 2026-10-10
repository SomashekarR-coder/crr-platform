export interface RiskPosition {
    id: number;
    tradeId: string;
    book: string;
    commodity: string;
    counterparty: string;
    region: string;
    instrumentType: string;
    position: number;
    currency: string;
    marketPrice: number;
    pnl: number;
    fxExposure: number;
    asOfDate: string;
}

export interface PagedResult {
    total: number;
    rows: RiskPosition[];
}
export interface AggregateRow {
    group: string;
    totalPnl: number;
    totalFx: number;
    rowCount: number;
}

export interface AggregatesResult {
    byBook: AggregateRow[];
    byCommodity: AggregateRow[];
    byRegion: AggregateRow[];
}

export interface FilterOptions {
    books: string[];
    commodities: string[];
    regions: string[];
    instrumentTypes: string[];
    currencies: string[];
}
export interface QueryParams {
    page?: number;
    pageSize?: number;
    sortField?: string;
    sortDir?: string;
    book?: string;
    commodity?: string;
    region?: string;
    instrumentType?: string;
    currency?: string;
    search?: string;
}