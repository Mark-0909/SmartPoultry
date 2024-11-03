using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartPoultry.Migrations
{
    /// <inheritdoc />
    public partial class Products : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    product_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    product_name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    animal_type = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    product_type = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    employee_incharge = table.Column<int>(type: "INTEGER", maxLength: 100, nullable: false),
                    supplier_id = table.Column<int>(type: "INTEGER", nullable: false),
                    status = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    image = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    added_date = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.product_id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
