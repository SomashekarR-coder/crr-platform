import axios from 'axios';
import { PagedResult, AggregatesResult, FilterOptions, QueryParams, RiskPosition } from '../types';

const BASE = 'http://localhost:5270/api/risk';

export const api = {
    getPositions: async (params: QueryParams): Promise<PagedResult> => {
        const { data } = await axios.get<PagedResult>(BASE, { params });
        return data;
    },
    getOne: async (id: number): Promise<RiskPosition> => {
        const { data } = await axios.get<RiskPosition>(`${BASE}/${id}`);
        return data;
    },
    getAggregates: async (params: QueryParams): Promise<AggregatesResult> => {
        const { data } = await axios.get<AggregatesResult>(`${BASE}/aggregates`, { params });
        return data;
    },
    getFilterOptions: async (): Promise<FilterOptions> => {
        const { data } = await axios.get<FilterOptions>(`${BASE}/filters`);
        return data;
    },
};