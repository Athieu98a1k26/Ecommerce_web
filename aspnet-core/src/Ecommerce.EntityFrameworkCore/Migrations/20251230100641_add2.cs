using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Migrations
{
    /// <inheritdoc />
    public partial class add2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "ProductStores",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Sold",
                table: "ProductStores",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Stars",
                table: "ProductStores",
                type: "real",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "ProductStores");

            migrationBuilder.DropColumn(
                name: "Sold",
                table: "ProductStores");

            migrationBuilder.DropColumn(
                name: "Stars",
                table: "ProductStores");
        }
    }
}
