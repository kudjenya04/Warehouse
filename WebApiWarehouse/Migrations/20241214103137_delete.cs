using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiWarehouse.Migrations
{
    /// <inheritdoc />
    public partial class delete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Enterprises_EnterpriseIdE",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_EnterpriseIdE",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "EnterpriseIdE",
                table: "Orders");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EnterpriseIdE",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_EnterpriseIdE",
                table: "Orders",
                column: "EnterpriseIdE");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Enterprises_EnterpriseIdE",
                table: "Orders",
                column: "EnterpriseIdE",
                principalTable: "Enterprises",
                principalColumn: "Id_E");
        }
    }
}
