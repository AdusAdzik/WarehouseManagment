using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarehouseManagment.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddOptionalWarehouseInventoryToWarehouseEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WarehouseInventories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WarehouseId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseInventories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WarehouseInventories_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WarehouseInventories_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WarehouseEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WarehouseId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<double>(type: "float", nullable: false),
                    EventType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    WarehouseInventoryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WarehouseEvents_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WarehouseEvents_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WarehouseEvents_WarehouseInventories_WarehouseInventoryId",
                        column: x => x.WarehouseInventoryId,
                        principalTable: "WarehouseInventories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WarehouseEvents_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseEvents_ProductId",
                table: "WarehouseEvents",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseEvents_UserId",
                table: "WarehouseEvents",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseEvents_WarehouseId",
                table: "WarehouseEvents",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseEvents_WarehouseInventoryId",
                table: "WarehouseEvents",
                column: "WarehouseInventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseInventories_ProductId",
                table: "WarehouseInventories",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseInventories_WarehouseId",
                table: "WarehouseInventories",
                column: "WarehouseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WarehouseEvents");

            migrationBuilder.DropTable(
                name: "WarehouseInventories");
        }
    }
}
