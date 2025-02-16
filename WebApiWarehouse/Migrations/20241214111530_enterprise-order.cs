using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiWarehouse.Migrations
{
    /// <inheritdoc />
    public partial class enterpriseorder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Products_Enterprise_id",
                table: "Products",
                column: "Enterprise_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Enterprises_Enterprise_id",
                table: "Products",
                column: "Enterprise_id",
                principalTable: "Enterprises",
                principalColumn: "Id_E",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Enterprises_Enterprise_id",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_Enterprise_id",
                table: "Products");
        }
    }
}
