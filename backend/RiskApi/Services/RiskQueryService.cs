using Microsoft.EntityFrameworkCore;
using RiskApi.Models;

namespace RiskApi.Services;

public class RiskQueryService(AppDbContext db)
{
    public IQueryable<RiskPosition> ApplyFilters(QueryParams q)
    {
        var query = db.RiskPositions.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(q.Book))
            query = query.Where(r => r.Book == q.Book);
        if (!string.IsNullOrWhiteSpace(q.Commodity))
            query = query.Where(r => r.Commodity == q.Commodity);

        if (!string.IsNullOrWhiteSpace(q.Region))
            query = query.Where(r => r.Region == q.Region);

        if (!string.IsNullOrWhiteSpace(q.InstrumentType))
            query = query.Where(r => r.InstrumentType == q.InstrumentType);

        if (!string.IsNullOrWhiteSpace(q.Currency))
            query = query.Where(r => r.Currency == q.Currency);
        if (!string.IsNullOrWhiteSpace(q.Search))
        {
            var s = q.Search.ToLower();
            query = query.Where(r =>
            r.Counterparty.ToLower().Contains(s) ||
            r.Commodity.ToLower().Contains(s));
        }

        return query;
    }
    public IQueryable<RiskPosition> ApplySort(IQueryable<RiskPosition> query, QueryParams q)
    {
        bool asc = q.SortDir?.ToLower() != "desc";
        return q.SortField switch
        {
            "tradeId" => asc ? query.OrderBy(r => r.TradeId) : query.OrderByDescending(r => r.TradeId),
            "book" => asc ? query.OrderBy(r => r.Book) : query.OrderByDescending(r => r.Book),
            "commodity" => asc ? query.OrderBy(r => r.Commodity) : query.OrderByDescending(r => r.Commodity),
            "counterparty" => asc ? query.OrderBy(r => r.Counterparty) : query.OrderByDescending(r => r.Counterparty),
            "region" => asc ? query.OrderBy(r => r.Region) : query.OrderByDescending(r => r.Region),
            "instrumentType" => asc ? query.OrderBy(r => r.InstrumentType) : query.OrderByDescending(r => r.InstrumentType),
            "position" => asc ? query.OrderBy(r => r.Position) : query.OrderByDescending(r => r.Position),
            "marketPrice" => asc ? query.OrderBy(r => r.MarketPrice) : query.OrderByDescending(r => r.MarketPrice),
            "pnl" => asc ? query.OrderBy(r => r.Pnl) : query.OrderByDescending(r => r.Pnl),
            "fxExposure" => asc ? query.OrderBy(r => r.FxExposure) : query.OrderByDescending(r => r.FxExposure),
            _ => query.OrderBy(r => r.TradeId)
        };
    }
}