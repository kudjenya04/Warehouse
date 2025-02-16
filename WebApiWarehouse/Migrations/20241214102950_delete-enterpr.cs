using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiWarehouse.Migrations
{
    /// <inheritdoc />
    public partial class deleteenterpr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Enterprises_EnterpriseIdE",
                table: "Orders");

            migrationBuilder.AlterColumn<int>(
                name: "EnterpriseIdE",
                table: "Orders",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Enterprises_EnterpriseIdE",
                table: "Orders",
                column: "EnterpriseIdE",
                principalTable: "Enterprises",
                principalColumn: "Id_E");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Enterprises_EnterpriseIdE",
                table: "Orders");

            migrationBuilder.AlterColumn<int>(
                name: "EnterpriseIdE",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Enterprises_EnterpriseIdE",
                table: "Orders",
                column: "EnterpriseIdE",
                principalTable: "Enterprises",
                principalColumn: "Id_E",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
