using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Linka.Cargo.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class mig2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Districst",
                table: "CargoCustomers",
                newName: "District");

            migrationBuilder.AddColumn<string>(
                name: "UserCustomerId",
                table: "CargoCustomers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserCustomerId",
                table: "CargoCustomers");

            migrationBuilder.RenameColumn(
                name: "District",
                table: "CargoCustomers",
                newName: "Districst");
        }
    }
}
