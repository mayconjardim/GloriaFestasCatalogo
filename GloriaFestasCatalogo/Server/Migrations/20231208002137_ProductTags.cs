using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GloriaFestasCatalogo.Server.Migrations
{
    public partial class ProductTags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Tags",
                table: "Products",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tags",
                table: "Products");
        }
    }
}
