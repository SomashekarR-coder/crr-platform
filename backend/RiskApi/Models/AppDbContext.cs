using Microsoft.EntityFrameworkCore;

namespace RiskApi.Models;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<RiskPosition> RiskPositions => Set<RiskPosition>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    { 
        modelBuilder.Entity<RiskPosition>()
        .HasIndex(r => r.TradeId).IsUnique();

    // Composite index speeds up common filter combos
        modelBuilder.Entity<RiskPosition>()
        .HasIndex(r => new { r.Book, r.Commodity, r.Region });

        modelBuilder.Entity<RiskPosition>().HasIndex(r => r.Pnl);
        modelBuilder.Entity<RiskPosition>().HasIndex(r => r.FxExposure);
    }
}
