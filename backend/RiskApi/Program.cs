using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using RiskApi.Models;
using RiskApi.Services;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
 options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<RiskQueryService>();

builder.Services.AddCors(o => o.AddDefaultPolicy(p =>
 p.WithOrigins("http://localhost:3000")
 .AllowAnyHeader()
 .AllowAnyMethod()));

var app = builder.Build();

// Migrate and seed on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
    var csvPath = Path.GetFullPath(
   Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "data", "risk_positions.csv")
    );
    CsvSeeder.Seed(db, csvPath);
}

app.UseCors();
app.MapOpenApi();
app.MapScalarApiReference();
app.MapControllers();
app.Run("http://localhost:5270");