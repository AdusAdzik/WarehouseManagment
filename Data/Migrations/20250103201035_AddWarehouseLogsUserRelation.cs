using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarehouseManagment.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddWarehouseLogsUserRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "WarehouseLogs",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseLogs_UserId",
                table: "WarehouseLogs",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseLogs_AspNetUsers_UserId",
                table: "WarehouseLogs",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseLogs_AspNetUsers_UserId",
                table: "WarehouseLogs");

            migrationBuilder.DropIndex(
                name: "IX_WarehouseLogs_UserId",
                table: "WarehouseLogs");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "WarehouseLogs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
