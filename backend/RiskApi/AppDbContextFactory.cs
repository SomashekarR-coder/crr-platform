using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using RiskApi.Models;

namespace RiskApi;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
        .UseNpgsql("Host=localhost;Database=crrdb;Username=crruser;Password=crrpass")
        .Options;

        return new AppDbContext(options);
    }
}