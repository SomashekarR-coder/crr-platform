using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RiskApi.Models;
using RiskApi.Services;

namespace RiskApi.Controllers;

[ApiController]
[Route("api/risk")]
public class RiskController(AppDbContext db, RiskQueryService svc) : ControllerBase
{
    // GET /api/risk?page=0&pageSize=100&book=Ferrous&sortField=pnl&sortDir=desc
    [HttpGet]
    public async Task<IActionResult> GetPositions([FromQuery] QueryParams q)
    {
        var filtered = svc.ApplyFilters(q);
        var total = await filtered.CountAsync();
        var rows = await svc.ApplySort(filtered, q)
        .Skip(q.Page * q.PageSize)
        .Take(q.PageSize)
       .ToListAsync();
        return Ok(new { total, rows });
    }

    // GET /api/risk/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetOne(int id)
    {
        var row = await db.RiskPositions.FindAsync(id);
        return row is null ? NotFound() : Ok(row);
    }

    // GET /api/risk/aggregates  — respects same filters
    [HttpGet("aggregates")]
    public async Task<IActionResult> GetAggregates([FromQuery] QueryParams q)
    {
        var filtered = svc.ApplyFilters(q);

        var byBook = await filtered
        .GroupBy(r => r.Book)
        .Select(g => new {
            Group = g.Key,
            TotalPnl = g.Sum(r => r.Pnl),
            TotalFx = g.Sum(r => r.FxExposure),
            RowCount = g.Count()
            // Position NOT summed: units differ per commodity
        })
        .OrderBy(g => g.Group)
        .ToListAsync();
        var byCommodity = await filtered
         .GroupBy(r => r.Commodity)
         .Select(g => new {
             Group = g.Key,
             TotalPnl = g.Sum(r => r.Pnl),
             TotalFx = g.Sum(r => r.FxExposure),
             RowCount = g.Count()
         })
         .OrderByDescending(g => g.TotalPnl)
         .ToListAsync();

        var byRegion = await filtered
        .GroupBy(r => r.Region)
        .Select(g => new {
            Group = g.Key,
            TotalPnl = g.Sum(r => r.Pnl),
            TotalFx = g.Sum(r => r.FxExposure),
            RowCount = g.Count()
        })
        .OrderBy(g => g.Group)
        .ToListAsync();
        return Ok(new { byBook, byCommodity, byRegion });
    }

    // GET /api/risk/filters  — values for dropdowns
    [HttpGet("filters")]
    public async Task<IActionResult> GetFilterOptions()
    {
        return Ok(new
        {
            books = await db.RiskPositions.Select(r => r.Book).Distinct().OrderBy(x => x).ToListAsync(),
            commodities = await db.RiskPositions.Select(r => r.Commodity).Distinct().OrderBy(x => x).ToListAsync(),
            regions = await db.RiskPositions.Select(r => r.Region).Distinct().OrderBy(x => x).ToListAsync(),
            instrumentTypes = await db.RiskPositions.Select(r => r.InstrumentType).Distinct().OrderBy(x => x).ToListAsync(),
            currencies = await db.RiskPositions.Select(r => r.Currency).Distinct().OrderBy(x => x).ToListAsync()
        });
    }
}