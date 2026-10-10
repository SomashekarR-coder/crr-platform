using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using System.Globalization;
using RiskApi.Models;

namespace RiskApi.Services;

public static class CsvSeeder
{
    public static void Seed(AppDbContext db, string csvPath)
    {
        if (db.RiskPositions.Any())
        {
            Console.WriteLine("Already seeded, skipping.");
            return;
        }

        List<RiskPosition> records;

        if (!File.Exists(csvPath))
        {
            Console.WriteLine("CSV not found — generating 50,000 synthetic rows...");
            records = GenerateSyntheticRows(0, 50000);
        }
        else
        {
            Console.WriteLine($"Reading CSV from {csvPath}...");
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null,
                MissingFieldFound = null,
                PrepareHeaderForMatch = args => args.Header.ToLower()
            };
            using var reader = new StreamReader(csvPath);
            using var csv = new CsvReader(reader, config);
            records = csv.GetRecords<RiskPositionCsv>()
           .Select(r => new RiskPosition
           {
               TradeId = r.TradeId,
               Book = r.Book,
               Commodity = r.Commodity,
               Counterparty = r.Counterparty,
               Region = r.Region,
               InstrumentType = r.InstrumentType,
               Position = r.Position,
               Currency = r.Currency,
               MarketPrice = r.MarketPrice,
               Pnl = r.Pnl,
               FxExposure = r.FxExposure,
               AsOfDate = DateOnly.Parse(r.AsOfDate)
           })
            .ToList();
            if (records.Count < 50000)
            {
                Console.WriteLine($"CSV has {records.Count} rows. Padding to 50,000...");
                records.AddRange(GenerateSyntheticRows(records.Count, 50000 - records.Count));
            }
        }
        // Insert in batches of 5,000 so it doesn't time out
        const int batchSize = 5000;
        for (int i = 0; i < records.Count; i += batchSize)
        {
            db.RiskPositions.AddRange(records.Skip(i).Take(batchSize));
            db.SaveChanges();
            Console.WriteLine($"  Inserted {Math.Min(i + batchSize, records.Count)}/{records.Count}");
        }
        Console.WriteLine("Seeding complete.");
    }
    private static List<RiskPosition> GenerateSyntheticRows(int startIndex, int count)
    {
        var rng = new Random(42);
        string[] books = { "Ferrous", "Non-Ferrous", "Freight", "Ags" };
        var commodities = new Dictionary<string, string[]>
        {
            ["Ferrous"] = new[] { "HRC Steel", "Iron Ore", "Scrap Steel", "Coking Coal" },
            ["Non-Ferrous"] = new[] { "Copper", "Aluminium", "Zinc", "Nickel", "Lead" },
            ["Freight"] = new[] { "Capesize", "Panamax", "Supramax", "Handysize" },
            ["Ags"] = new[] { "Wheat", "Corn", "Soybeans", "Sugar", "Cotton" }
        };
        string[] regions = { "APAC", "EMEA", "AMER" };
        string[] instruments = { "Physical", "Future", "Swap" };
        string[] currencies = { "USD", "EUR", "CNY", "SGD" };
        string[] counterparties = {
             "Glencore", "Trafigura", "Vitol", "Cargill", "ADM", "Bunge",
             "Louis Dreyfus", "Gunvor", "Mercuria", "COFCO", "Mitsui", "Marubeni"
     };
        var rows = new List<RiskPosition>(count);
        var asOfDate = new DateOnly(2024, 1, 15);

        for (int i = 0; i < count; i++)
        {
            var book = books[rng.Next(books.Length)];
            var commodity = commodities[book][rng.Next(commodities[book].Length)];
            var currency = currencies[rng.Next(currencies.Length)];
            var fxRate = currency switch { "EUR" => 1.08m, "CNY" => 0.14m, "SGD" => 0.74m, _ => 1m };
            var position = Math.Round((decimal)(rng.NextDouble() * 20000 - 10000), 2);
            var price = Math.Round((decimal)(rng.NextDouble() * 1500 + 50), 2);
            var pnl = Math.Round(position * price * (decimal)(rng.NextDouble() * 0.1 - 0.05), 2);

            rows.Add(new RiskPosition
            {
                TradeId = $"TRD-{(startIndex + i + 1):D6}",
                Book = book,
                Commodity = commodity,
                Counterparty = counterparties[rng.Next(counterparties.Length)],
                Region = regions[rng.Next(regions.Length)],
                InstrumentType = instruments[rng.Next(instruments.Length)],
                Position = position,
                Currency = currency,
                MarketPrice = price,
                Pnl = pnl,
                FxExposure = Math.Round(pnl * fxRate, 2),
                AsOfDate = asOfDate
            });
        }
        return rows;
    }

    private class RiskPositionCsv
    {
        [Name("tradeId")] public string TradeId { get; set; } = "";
        [Name("book")] public string Book { get; set; } = "";
        [Name("commodity")] public string Commodity { get; set; } = "";
        [Name("counterparty")] public string Counterparty { get; set; } = "";
        [Name("region")] public string Region { get; set; } = "";
        [Name("instrumentType")] public string InstrumentType { get; set; } = "";
        [Name("position")] public decimal Position { get; set; }
        [Name("currency")] public string Currency { get; set; } = "";
        [Name("marketPrice")] public decimal MarketPrice { get; set; }
        [Name("pnl")] public decimal Pnl { get; set; }
        [Name("fxExposure")] public decimal FxExposure { get; set; }
        [Name("asOfDate")] public string AsOfDate { get; set; } = "";
    }
}