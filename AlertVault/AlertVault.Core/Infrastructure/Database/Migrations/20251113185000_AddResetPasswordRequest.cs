using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AlertVault.Core.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddResetPasswordRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserCredential_UserId",
                table: "UserCredential");

            migrationBuilder.CreateTable(
                name: "ResetPasswordRequest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Token = table.Column<Guid>(type: "uuid", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsUsed = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResetPasswordRequest", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserToken_Token",
                table: "UserToken",
                column: "Token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserCredential_UserId_CredentialType",
                table: "UserCredential",
                columns: new[] { "UserId", "CredentialType" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAgent_UserAgentString",
                table: "UserAgent",
                column: "UserAgentString",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                table: "User",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Alert_Uuid",
                table: "Alert",
                column: "Uuid",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResetPasswordRequest");

            migrationBuilder.DropIndex(
                name: "IX_UserToken_Token",
                table: "UserToken");

            migrationBuilder.DropIndex(
                name: "IX_UserCredential_UserId_CredentialType",
                table: "UserCredential");

            migrationBuilder.DropIndex(
                name: "IX_UserAgent_UserAgentString",
                table: "UserAgent");

            migrationBuilder.DropIndex(
                name: "IX_User_Email",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_Alert_Uuid",
                table: "Alert");

            migrationBuilder.CreateIndex(
                name: "IX_UserCredential_UserId",
                table: "UserCredential",
                column: "UserId");
        }
    }
}
