using Microsoft.EntityFrameworkCore.Migrations;

namespace Yad2Proj.Migrations
{
    public partial class addedInCart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "InCart",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InCart",
                table: "Products");
        }
    }
}
