using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiWarehouse.Migrations
{
    /// <inheritdoc />
    public partial class firstmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Enterprises",
                columns: table => new
                {
                    Id_E = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name_E = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Contact = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enterprises", x => x.Id_E);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id_P = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name_P = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Unit_price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Enterprise_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id_P);
                });

            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    Id_Status = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.Id_Status);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id_U = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name_U = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Admin = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Enterprise_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id_U);
                    table.ForeignKey(
                        name: "FK_Users_Enterprises",
                        column: x => x.Enterprise_id,
                        principalTable: "Enterprises",
                        principalColumn: "Id_E");
                });

            migrationBuilder.CreateTable(
                name: "Warehouses",
                columns: table => new
                {
                    Id_W = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Enterprise_id = table.Column<int>(type: "int", nullable: false),
                    Name_W = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warehouses", x => x.Id_W);
                    table.ForeignKey(
                        name: "FK_Warehouses_Enterprises",
                        column: x => x.Enterprise_id,
                        principalTable: "Enterprises",
                        principalColumn: "Id_E");
                });

            migrationBuilder.CreateTable(
                name: "ActivityLog",
                columns: table => new
                {
                    Id_AL = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User_id = table.Column<int>(type: "int", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityLog", x => x.Id_AL);
                    table.ForeignKey(
                        name: "FK_ActivityLog_Users",
                        column: x => x.User_id,
                        principalTable: "Users",
                        principalColumn: "Id_U");
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id_O = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Client_id = table.Column<int>(type: "int", nullable: false),
                    Status_id = table.Column<int>(type: "int", nullable: false),
                    Enterprise_id = table.Column<int>(type: "int", nullable: false),
                    Created_at = table.Column<DateTime>(type: "datetime", nullable: false),
                    Product_id = table.Column<int>(type: "int", nullable: false),
                    Quantity_O = table.Column<int>(type: "int", nullable: false),
                    ProductIdP = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id_O);
                    table.ForeignKey(
                        name: "FK_Orders_Enterprises",
                        column: x => x.Enterprise_id,
                        principalTable: "Enterprises",
                        principalColumn: "Id_E");
                    table.ForeignKey(
                        name: "FK_Orders_Products",
                        column: x => x.Product_id,
                        principalTable: "Products",
                        principalColumn: "Id_P");
                    table.ForeignKey(
                        name: "FK_Orders_Products_ProductIdP",
                        column: x => x.ProductIdP,
                        principalTable: "Products",
                        principalColumn: "Id_P");
                    table.ForeignKey(
                        name: "FK_Orders_Status",
                        column: x => x.Status_id,
                        principalTable: "Status",
                        principalColumn: "Id_Status");
                    table.ForeignKey(
                        name: "FK_Orders_Users",
                        column: x => x.Client_id,
                        principalTable: "Users",
                        principalColumn: "Id_U");
                });

            migrationBuilder.CreateTable(
                name: "Inventory",
                columns: table => new
                {
                    Id_I = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Warehouse_id = table.Column<int>(type: "int", nullable: false),
                    Product_id = table.Column<int>(type: "int", nullable: false),
                    Quantity_I = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventory", x => x.Id_I);
                    table.ForeignKey(
                        name: "FK_Inventory_Products",
                        column: x => x.Product_id,
                        principalTable: "Products",
                        principalColumn: "Id_P");
                    table.ForeignKey(
                        name: "FK_Inventory_Warehouses",
                        column: x => x.Warehouse_id,
                        principalTable: "Warehouses",
                        principalColumn: "Id_W");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLog_User_id",
                table: "ActivityLog",
                column: "User_id");

            migrationBuilder.CreateIndex(
                name: "IX_Inventory_Product_id",
                table: "Inventory",
                column: "Product_id");

            migrationBuilder.CreateIndex(
                name: "IX_Inventory_Warehouse_id",
                table: "Inventory",
                column: "Warehouse_id");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Client_id",
                table: "Orders",
                column: "Client_id");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Enterprise_id",
                table: "Orders",
                column: "Enterprise_id");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Product_id",
                table: "Orders",
                column: "Product_id");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ProductIdP",
                table: "Orders",
                column: "ProductIdP");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Status_id",
                table: "Orders",
                column: "Status_id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Enterprise_id",
                table: "Users",
                column: "Enterprise_id");

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_Enterprise_id",
                table: "Warehouses",
                column: "Enterprise_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityLog");

            migrationBuilder.DropTable(
                name: "Inventory");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Warehouses");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Status");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Enterprises");
        }
    }
}
