namespace RiskApi.Models;

public class RiskPosition
{
    public int Id { get; set; }
    public string TradeId { get; set; } = "";
    public string Book { get; set; } = "";
    public string Commodity { get; set; } = "";
    public string Counterparty { get; set; } = "";
    public string Region { get; set; } = "";
    public string InstrumentType { get; set; } = "";
    public decimal Position { get; set; }
    public string Currency { get; set; } = "";
    public decimal MarketPrice { get; set; }
    public decimal Pnl { get; set; }
    public decimal FxExposure { get; set; }
    public DateOnly AsOfDate { get; set; }
}