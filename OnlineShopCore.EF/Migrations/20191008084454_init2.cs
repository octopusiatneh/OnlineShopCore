using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineShopCore.Data.EF.Migrations
{
    public partial class init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "HomeFlag",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "HomeOrder",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "SeoDescription",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "SeoKeywords",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "SeoPageTitle",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "SortOrder",
                table: "Categories");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Categories",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HomeFlag",
                table: "Categories",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HomeOrder",
                table: "Categories",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Categories",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "Categories",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeoDescription",
                table: "Categories",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeoKeywords",
                table: "Categories",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeoPageTitle",
                table: "Categories",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SortOrder",
                table: "Categories",
                nullable: false,
                defaultValue: 0);
        }
    }
}
