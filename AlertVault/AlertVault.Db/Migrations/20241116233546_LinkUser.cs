using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlertVault.Db.Migrations
{
    /// <inheritdoc />
    public partial class LinkUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Alert_UserId",
                table: "Alert",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Alert_User_UserId",
                table: "Alert",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alert_User_UserId",
                table: "Alert");

            migrationBuilder.DropIndex(
                name: "IX_Alert_UserId",
                table: "Alert");
        }
    }
}
