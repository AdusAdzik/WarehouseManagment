using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarehouseManagment.Data.Migrations
{
    /// <inheritdoc />
    public partial class EnableCascadeDeleteOnWarehouseLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseLogs_Warehouses_WarehouseId1",
                table: "WarehouseLogs");

            migrationBuilder.DropIndex(
                name: "IX_WarehouseLogs_WarehouseId1",
                table: "WarehouseLogs");

            migrationBuilder.DropColumn(
                name: "WarehouseId1",
                table: "WarehouseLogs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WarehouseId1",
                table: "WarehouseLogs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseLogs_WarehouseId1",
                table: "WarehouseLogs",
                column: "WarehouseId1");

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseLogs_Warehouses_WarehouseId1",
                table: "WarehouseLogs",
                column: "WarehouseId1",
                principalTable: "Warehouses",
                principalColumn: "Id");
        }
    }
}
