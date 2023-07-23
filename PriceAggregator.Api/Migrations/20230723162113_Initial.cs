using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceAggregator.Api.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "prices",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    close = table.Column<decimal>(type: "TEXT", nullable: false),
                    startdatetime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    fromcurrency = table.Column<int>(type: "INTEGER", nullable: false),
                    tocurrency = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_prices", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "prices");
        }
    }
}
