# DESIGN.md
 
## Key Decisions
 
### Data Store — PostgreSQL
PostgreSQL is used as the primary store with EF Core 10.
Indexes on (book, commodity, region) and numeric columns (pnl, fxExposure)
keep filtered and sorted queries fast at 50,000 rows.
EF Core migrations manage schema changes cleanly.
 
Tradeoff vs in-memory: An in-memory store would start faster and need
no setup, but cannot use real indexes, does not survive restarts, and
is not representative of production behaviour at scale.
### Pagination — Offset based
Chosen for simplicity and AG Grid Infinite Row Model compatibility.
At 50,000 rows offset pagination is fast. For millions of rows,
keyset (cursor) pagination would be preferred to avoid slow deep-offset scans.
### Sorting and Filtering — Server-side via LINQ
All sort and filter logic lives in RiskQueryService and is translated
to SQL by EF Core. No client-side filtering — the full dataset is
never loaded into the browser. The grid requests 100 rows at a time
as the user scrolls.
### AG Grid — Infinite Row Model (Community edition)
Because AG Grid Community does not support server-side row grouping
natively, grouping and aggregation is handled through a dedicated
/api/risk/aggregates endpoint that returns pre-computed totals
grouped by book, commodity, or region. These appear as summary
cards and a bar chart above the grid.
Drilldown is implemented as a modal detail panel triggered by
clicking any grid row.
### Position Aggregation
Position quantities are NOT summed across different commodities
because units differ (metric tons of steel vs copper vs freight).
Only P&L and FX Exposure — both in USD — are aggregated.
### Data Seeding
The seeder runs on startup automatically:
- If risk_positions.csv exists in data/ folder it reads it
- If CSV has fewer than 50,000 rows it pads with synthetic data
- If CSV is missing it generates 50,000 synthetic rows
- Inserts in batches of 5,000 to avoid timeouts
 ## What I Would Improve With More Time
 
1. OpenSearch for free-text search — current LIKE-based search works
   but is not fuzzy. With OpenSearch I would index counterparty and
   commodity with edge_ngram analyzer so partial and misspelled names
   still return results.
 
2. Keyset pagination for very large datasets (1M+ rows) to avoid
   slow OFFSET queries deep in the dataset.
 
3. Saved filter views — persist column state and active filters
   in localStorage so analysts can return to a configured view.
 
4. WebSocket or SSE push for live P&L updates without page refresh.

5. Automated tests — unit tests for RiskQueryService and React
   Testing Library tests for FilterBar and AggregatesPanel.
 
6. CI pipeline — GitHub Actions running dotnet build and npm run build
   triggered on every pull request.
