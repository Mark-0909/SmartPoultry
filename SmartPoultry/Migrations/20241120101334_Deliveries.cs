using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartPoultry.Migrations
{
    /// <inheritdoc />
    public partial class Deliveries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Deliveries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    order_id = table.Column<long>(type: "INTEGER", nullable: false),
                    type = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    address = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    status = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    contact_no = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    added_date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    delivery_date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    delivery_man = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    charges = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    employee_incharge = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deliveries", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Deliveries");
        }
    }
}
