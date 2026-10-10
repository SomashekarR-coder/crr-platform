# CRR Platform — Risk Reporting View
 
A full-stack risk-reporting tool built with .NET 10 / ASP.NET Core + React + TypeScript + AG Grid.
 
## Prerequisites
 
| Tool | Version |
|------|---------|
| .NET SDK | 10.0+ |
| Node.js | 18+ |
| PostgreSQL | 14+ |
 
## Step 1 — Clone the repo
 
```bash
git clone https://github.com/SomashekarR-coder/crr-platform.git
cd crr-platform
```
## Step 2 — Create the PostgreSQL database
 
Open SQL Shell (psql) and run:
 
```sql
CREATE DATABASE crrdb;
CREATE USER crruser WITH PASSWORD 'crrpass';
GRANT ALL PRIVILEGES ON DATABASE crrdb TO crruser;
\c crrdb
GRANT ALL ON SCHEMA public TO crruser;
ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT ALL ON TABLES TO crruser;
\q
## Step 3 — Run the backend
 
```bash
cd backend/RiskApi
dotnet tool install --global dotnet-ef
dotnet restore
dotnet ef migrations add InitialCreate
dotnet ef database update
dotnet run
```
 
- API runs at: http://localhost:5270
- API docs at: http://localhost:5270/scalar/v1
- First run seeds 50,000 rows automatically
## Step 4 — Run the frontend
 
```bash
cd frontend
npm install --legacy-peer-deps
npm start
```
 
- UI runs at: http://localhost:3000
 
## API Endpoints
 
| Endpoint | Description |
|----------|-------------|
| GET /api/risk | Paginated, sorted, filtered positions |
| GET /api/risk/{id} | Single position by ID |
| GET /api/risk/aggregates | P&L totals grouped by book/commodity/region |
| GET /api/risk/filters | Distinct values for dropdown filters |
## Features
 
- AG Grid Infinite Row Model — 50,000 rows scroll smoothly
- Server-side sorting, filtering and pagination
- Aggregates panel — P&L by Book, Commodity, Region
- Bar chart summary using Recharts
- Free-text search across counterparty and commodity
- Click any row for a detailed drilldown modal
