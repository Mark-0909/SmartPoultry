using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartPoultry.Migrations
{
    /// <inheritdoc />
    public partial class AddRemarksInDbs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "UserLogs",
                type: "TEXT",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "Sales",
                type: "TEXT",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "FinancialLiabilities",
                type: "TEXT",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "Deliveries",
                type: "TEXT",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "UserLogs");

            

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "FinancialLiabilities");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "Deliveries");
        }
    }
}
