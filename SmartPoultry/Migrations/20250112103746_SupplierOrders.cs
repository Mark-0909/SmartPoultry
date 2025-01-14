using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartPoultry.Migrations
{
    /// <inheritdoc />
    public partial class SupplierOrders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SupplierOrders",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    supplierID = table.Column<int>(type: "INTEGER", nullable: false),
                    productList = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    orderQty = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Added_Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Delivery_Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Delivered_Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    employee_incharge = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierOrders", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SupplierOrders");
        }
    }
}
