using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartPoultry.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderIdinExpenses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Order_ID",
                table: "Expenses",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order_ID",
                table: "Expenses");
        }
    }
}
