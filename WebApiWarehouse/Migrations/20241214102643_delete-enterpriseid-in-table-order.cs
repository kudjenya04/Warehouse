using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiWarehouse.Migrations
{
    /// <inheritdoc />
    public partial class deleteenterpriseidintableorder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Enterprises",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "Enterprise_id",
                table: "Orders",
                newName: "EnterpriseIdE");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_Enterprise_id",
                table: "Orders",
                newName: "IX_Orders_EnterpriseIdE");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Enterprises_EnterpriseIdE",
                table: "Orders",
                column: "EnterpriseIdE",
                principalTable: "Enterprises",
                principalColumn: "Id_E",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Enterprises_EnterpriseIdE",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "EnterpriseIdE",
                table: "Orders",
                newName: "Enterprise_id");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_EnterpriseIdE",
                table: "Orders",
                newName: "IX_Orders_Enterprise_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Enterprises",
                table: "Orders",
                column: "Enterprise_id",
                principalTable: "Enterprises",
                principalColumn: "Id_E");
        }
    }
}
