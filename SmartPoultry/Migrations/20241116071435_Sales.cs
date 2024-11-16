using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartPoultry.Migrations
{
    /// <inheritdoc />
    public partial class Sales : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sales",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    product_list = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    price_list = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    quantity_list = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    purchase_date = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    variation_list = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    status = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    total_price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    payment_mode = table.Column<string>(type: "TEXT", nullable: false),
                    employee_incharge = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sales");
        }
    }
}
