using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AzureIoTServer.Migrations
{
    /// <inheritdoc />
    public partial class RenamingTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tempHumidities");

            migrationBuilder.CreateTable(
                name: "temperatures",
                columns: table => new
                {
                    dateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    temperature = table.Column<float>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_temperatures", x => x.dateTime);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "temperatures");

            migrationBuilder.CreateTable(
                name: "tempHumidities",
                columns: table => new
                {
                    dateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    humidity = table.Column<float>(type: "float", nullable: false),
                    temperature = table.Column<float>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tempHumidities", x => x.dateTime);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
