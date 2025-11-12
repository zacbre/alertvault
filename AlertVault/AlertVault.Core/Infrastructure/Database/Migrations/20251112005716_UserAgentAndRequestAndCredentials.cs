using System;
using System.Net;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AlertVault.Core.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class UserAgentAndRequestAndCredentials : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlertNotifications_Alert_AlertId",
                table: "AlertNotifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AlertNotifications",
                table: "AlertNotifications");

            migrationBuilder.DropColumn(
                name: "UserAgent",
                table: "Request");

            migrationBuilder.RenameTable(
                name: "AlertNotifications",
                newName: "AlertNotification");

            migrationBuilder.RenameIndex(
                name: "IX_AlertNotifications_AlertId",
                table: "AlertNotification",
                newName: "IX_AlertNotification_AlertId");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "User",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "User",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<IPAddress>(
                name: "IpAddress",
                table: "Request",
                type: "inet",
                nullable: true,
                oldClrType: typeof(IPAddress),
                oldType: "inet");

            migrationBuilder.AddColumn<string>(
                name: "Body",
                table: "Request",
                type: "character varying(1024)",
                maxLength: 1024,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserAgentId",
                table: "Request",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Alert",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Alert",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AlertNotification",
                table: "AlertNotification",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "UserAgent",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserAgentString = table.Column<string>(type: "text", nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAgent", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserCredential",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    CredentialType = table.Column<string>(type: "text", nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Credentials = table.Column<string>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCredential", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserCredential_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Request_UserAgentId",
                table: "Request",
                column: "UserAgentId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCredential_UserId",
                table: "UserCredential",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AlertNotification_Alert_AlertId",
                table: "AlertNotification",
                column: "AlertId",
                principalTable: "Alert",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Request_UserAgent_UserAgentId",
                table: "Request",
                column: "UserAgentId",
                principalTable: "UserAgent",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlertNotification_Alert_AlertId",
                table: "AlertNotification");

            migrationBuilder.DropForeignKey(
                name: "FK_Request_UserAgent_UserAgentId",
                table: "Request");

            migrationBuilder.DropTable(
                name: "UserAgent");

            migrationBuilder.DropTable(
                name: "UserCredential");

            migrationBuilder.DropIndex(
                name: "IX_Request_UserAgentId",
                table: "Request");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AlertNotification",
                table: "AlertNotification");

            migrationBuilder.DropColumn(
                name: "Body",
                table: "Request");

            migrationBuilder.DropColumn(
                name: "UserAgentId",
                table: "Request");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Alert");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Alert");

            migrationBuilder.RenameTable(
                name: "AlertNotification",
                newName: "AlertNotifications");

            migrationBuilder.RenameIndex(
                name: "IX_AlertNotification_AlertId",
                table: "AlertNotifications",
                newName: "IX_AlertNotifications_AlertId");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "User",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "User",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<IPAddress>(
                name: "IpAddress",
                table: "Request",
                type: "inet",
                nullable: false,
                oldClrType: typeof(IPAddress),
                oldType: "inet",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserAgent",
                table: "Request",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AlertNotifications",
                table: "AlertNotifications",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AlertNotifications_Alert_AlertId",
                table: "AlertNotifications",
                column: "AlertId",
                principalTable: "Alert",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
