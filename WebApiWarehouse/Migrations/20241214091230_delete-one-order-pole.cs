using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiWarehouse.Migrations
{
    /// <inheritdoc />
    public partial class deleteoneorderpole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Products_ProductIdP",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ProductIdP",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ProductIdP",
                table: "Orders");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductIdP",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ProductIdP",
                table: "Orders",
                column: "ProductIdP");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Products_ProductIdP",
                table: "Orders",
                column: "ProductIdP",
                principalTable: "Products",
                principalColumn: "Id_P");
        }
    }
}
