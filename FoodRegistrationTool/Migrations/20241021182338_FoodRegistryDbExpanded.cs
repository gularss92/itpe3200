using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodRegistrationTool.Migrations
{
    /// <inheritdoc />
    public partial class FoodRegistryDbExpanded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Nutrition",
                table: "Products",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nutrition",
                table: "Products");
        }
    }
}
