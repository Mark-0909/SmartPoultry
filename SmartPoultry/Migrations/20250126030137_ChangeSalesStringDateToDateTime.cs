using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartPoultry.Migrations
{
    /// <inheritdoc />
    public partial class ChangeSalesStringDateToDateTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Alter the column type from string to DateTime
            migrationBuilder.AlterColumn<DateTime>(
                name: "purchase_date",
                table: "Sales",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revert the column type back to string
            migrationBuilder.AlterColumn<string>(
                name: "purchase_date",
                table: "Sales",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }
    }
}
