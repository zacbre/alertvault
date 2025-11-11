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
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedUtc",
                table: "Request",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedUtc",
                table: "Request",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedUtc",
                table: "Alert",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
            
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION update_updated_at_column()
                RETURNS TRIGGER AS $$
                BEGIN
                    NEW.""UpdatedAtUtc"" = NOW() AT TIME ZONE 'UTC';
                    RETURN NEW;
                END;
                $$ LANGUAGE plpgsql;
            ");

            // Create trigger for Alerts table
            migrationBuilder.Sql(@"
                CREATE TRIGGER update_alerts_timestamp
                BEFORE UPDATE ON ""Alerts""
                FOR EACH ROW
                EXECUTE FUNCTION update_updated_at_column();
            ");

            // Create trigger for Requests table
            migrationBuilder.Sql(@"
                CREATE TRIGGER update_requests_timestamp
                BEFORE UPDATE ON ""Requests""
                FOR EACH ROW
                EXECUTE FUNCTION update_updated_at_column();
            ");
            
            // Create trigger for Requests table
            migrationBuilder.Sql(@"
                CREATE TRIGGER update_requests_timestamp
                BEFORE UPDATE ON ""Users""
                FOR EACH ROW
                EXECUTE FUNCTION update_updated_at_column();
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS update_alerts_timestamp ON \"Alerts\";");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS update_requests_timestamp ON \"Requests\";");
        
            // Drop function
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS update_updated_at_column();");
            
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
