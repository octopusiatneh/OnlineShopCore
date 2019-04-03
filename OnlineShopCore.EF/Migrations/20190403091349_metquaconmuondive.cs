using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineShopCore.Data.EF.Migrations
{
    public partial class metquaconmuondive : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "Slides");

            migrationBuilder.DropColumn(
                name: "GroupAlias",
                table: "Slides");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "Slides");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Slides",
                nullable: false,
                oldClrType: typeof(bool));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "Slides",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "Slides",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GroupAlias",
                table: "Slides",
                maxLength: 25,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Slides",
                maxLength: 250,
                nullable: true);
        }
    }
}
