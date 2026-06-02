using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Linka.Order.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderStatusToOrdering : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrderStatus",
                table: "Orderings",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "Paid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderStatus",
                table: "Orderings");
        }
    }
}
