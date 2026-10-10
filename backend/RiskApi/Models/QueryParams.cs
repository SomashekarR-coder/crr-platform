namespace RiskApi.Models;

public class QueryParams
{
    public int Page { get; set; } = 0;
    public int PageSize { get; set; } = 100;
    public string? SortField { get; set; }
    public string? SortDir { get; set; } = "asc";
    public string? Book { get; set; }
    public string? Commodity { get; set; }
    public string? Region { get; set; }
    public string? InstrumentType { get; set; }
    public string? Currency { get; set; }
    public string? Search { get; set; }
}