using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlertVault.Db.Migrations
{
    /// <inheritdoc />
    public partial class AddUpdatedUtc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedUtc",
                table: "User",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: DateTime.UtcNow);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedUtc",
                table: "Request",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: DateTime.UtcNow);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedUtc",
                table: "Request",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: DateTime.UtcNow);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedUtc",
                table: "Alert",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: DateTime.UtcNow);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatedUtc",
                table: "User");

            migrationBuilder.DropColumn(
                name: "CreatedUtc",
                table: "Request");

            migrationBuilder.DropColumn(
                name: "UpdatedUtc",
                table: "Request");

            migrationBuilder.DropColumn(
                name: "UpdatedUtc",
                table: "Alert");
        }
    }
}
