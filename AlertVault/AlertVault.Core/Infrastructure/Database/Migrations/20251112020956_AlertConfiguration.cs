using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlertVault.Core.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AlertConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Method",
                table: "Request",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "AlertConfiguration",
                table: "Alert",
                type: "jsonb",
                nullable: false,
                defaultValue: "{}");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AlertConfiguration",
                table: "Alert");

            migrationBuilder.AlterColumn<int>(
                name: "Method",
                table: "Request",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
