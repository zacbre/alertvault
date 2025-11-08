using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlertVault.Db.Migrations
{
    /// <inheritdoc />
    public partial class AddRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AlertId",
                table: "Request",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Request_AlertId",
                table: "Request",
                column: "AlertId");

            migrationBuilder.AddForeignKey(
                name: "FK_Request_Alert_AlertId",
                table: "Request",
                column: "AlertId",
                principalTable: "Alert",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Request_Alert_AlertId",
                table: "Request");

            migrationBuilder.DropIndex(
                name: "IX_Request_AlertId",
                table: "Request");

            migrationBuilder.DropColumn(
                name: "AlertId",
                table: "Request");
        }
    }
}
