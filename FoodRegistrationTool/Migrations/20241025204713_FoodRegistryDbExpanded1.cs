using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodRegistrationTool.Migrations
{
    /// <inheritdoc />
    public partial class FoodRegistryDbExpanded1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NutriScore",
                table: "Products",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NutriScore",
                table: "Products");
        }
    }
}
