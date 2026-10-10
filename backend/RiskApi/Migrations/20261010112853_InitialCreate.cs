using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RiskApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RiskPositions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TradeId = table.Column<string>(type: "text", nullable: false),
                    Book = table.Column<string>(type: "text", nullable: false),
                    Commodity = table.Column<string>(type: "text", nullable: false),
                    Counterparty = table.Column<string>(type: "text", nullable: false),
                    Region = table.Column<string>(type: "text", nullable: false),
                    InstrumentType = table.Column<string>(type: "text", nullable: false),
                    Position = table.Column<decimal>(type: "numeric", nullable: false),
                    Currency = table.Column<string>(type: "text", nullable: false),
                    MarketPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    Pnl = table.Column<decimal>(type: "numeric", nullable: false),
                    FxExposure = table.Column<decimal>(type: "numeric", nullable: false),
                    AsOfDate = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskPositions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RiskPositions_Book_Commodity_Region",
                table: "RiskPositions",
                columns: new[] { "Book", "Commodity", "Region" });

            migrationBuilder.CreateIndex(
                name: "IX_RiskPositions_FxExposure",
                table: "RiskPositions",
                column: "FxExposure");

            migrationBuilder.CreateIndex(
                name: "IX_RiskPositions_Pnl",
                table: "RiskPositions",
                column: "Pnl");

            migrationBuilder.CreateIndex(
                name: "IX_RiskPositions_TradeId",
                table: "RiskPositions",
                column: "TradeId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RiskPositions");
        }
    }
}
