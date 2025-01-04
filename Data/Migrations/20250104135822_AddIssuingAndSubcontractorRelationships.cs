using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarehouseManagment.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIssuingAndSubcontractorRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subcontractor_AspNetUsers_UserId",
                table: "Subcontractor");

            migrationBuilder.AddColumn<int>(
                name: "SubcontractorId",
                table: "WarehouseEvents",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseEvents_SubcontractorId",
                table: "WarehouseEvents",
                column: "SubcontractorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subcontractor_AspNetUsers_UserId",
                table: "Subcontractor",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseEvents_Subcontractor_SubcontractorId",
                table: "WarehouseEvents",
                column: "SubcontractorId",
                principalTable: "Subcontractor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subcontractor_AspNetUsers_UserId",
                table: "Subcontractor");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseEvents_Subcontractor_SubcontractorId",
                table: "WarehouseEvents");

            migrationBuilder.DropIndex(
                name: "IX_WarehouseEvents_SubcontractorId",
                table: "WarehouseEvents");

            migrationBuilder.DropColumn(
                name: "SubcontractorId",
                table: "WarehouseEvents");

            migrationBuilder.AddForeignKey(
                name: "FK_Subcontractor_AspNetUsers_UserId",
                table: "Subcontractor",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
