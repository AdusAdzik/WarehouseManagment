using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarehouseManagment.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMissingSubcontractorLogsRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubcontractorLog_AspNetUsers_UserId",
                table: "SubcontractorLog");

            migrationBuilder.DropForeignKey(
                name: "FK_SubcontractorLog_Subcontractor_SubcontractorId",
                table: "SubcontractorLog");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubcontractorLog",
                table: "SubcontractorLog");

            migrationBuilder.RenameTable(
                name: "SubcontractorLog",
                newName: "SubcontractorLogs");

            migrationBuilder.RenameIndex(
                name: "IX_SubcontractorLog_UserId",
                table: "SubcontractorLogs",
                newName: "IX_SubcontractorLogs_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_SubcontractorLog_SubcontractorId",
                table: "SubcontractorLogs",
                newName: "IX_SubcontractorLogs_SubcontractorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubcontractorLogs",
                table: "SubcontractorLogs",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SubcontractorLogs_AspNetUsers_UserId",
                table: "SubcontractorLogs",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubcontractorLogs_Subcontractor_SubcontractorId",
                table: "SubcontractorLogs",
                column: "SubcontractorId",
                principalTable: "Subcontractor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubcontractorLogs_AspNetUsers_UserId",
                table: "SubcontractorLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_SubcontractorLogs_Subcontractor_SubcontractorId",
                table: "SubcontractorLogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubcontractorLogs",
                table: "SubcontractorLogs");

            migrationBuilder.RenameTable(
                name: "SubcontractorLogs",
                newName: "SubcontractorLog");

            migrationBuilder.RenameIndex(
                name: "IX_SubcontractorLogs_UserId",
                table: "SubcontractorLog",
                newName: "IX_SubcontractorLog_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_SubcontractorLogs_SubcontractorId",
                table: "SubcontractorLog",
                newName: "IX_SubcontractorLog_SubcontractorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubcontractorLog",
                table: "SubcontractorLog",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SubcontractorLog_AspNetUsers_UserId",
                table: "SubcontractorLog",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubcontractorLog_Subcontractor_SubcontractorId",
                table: "SubcontractorLog",
                column: "SubcontractorId",
                principalTable: "Subcontractor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
