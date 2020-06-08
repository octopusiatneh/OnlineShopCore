using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineShopCore.Data.EF.Migrations
{
    public partial class cleansql : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Publishers");

            migrationBuilder.DropColumn(
                name: "SortOrder",
                table: "Publishers");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "ProductCategories");

            migrationBuilder.DropColumn(
                name: "HomeFlag",
                table: "ProductCategories");

            migrationBuilder.DropColumn(
                name: "HomeOrder",
                table: "ProductCategories");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "ProductCategories");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "ProductCategories");

            migrationBuilder.DropColumn(
                name: "SeoDescription",
                table: "ProductCategories");

            migrationBuilder.DropColumn(
                name: "SeoKeywords",
                table: "ProductCategories");

            migrationBuilder.DropColumn(
                name: "SeoPageTitle",
                table: "ProductCategories");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "SortOrder",
                table: "Authors");

            migrationBuilder.RenameColumn(
                name: "NamePublisher",
                table: "Publishers",
                newName: "PublisherName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PublisherName",
                table: "Publishers",
                newName: "NamePublisher");

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "Publishers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SortOrder",
                table: "Publishers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ProductCategories",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HomeFlag",
                table: "ProductCategories",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HomeOrder",
                table: "ProductCategories",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "ProductCategories",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "ProductCategories",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeoDescription",
                table: "ProductCategories",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeoKeywords",
                table: "ProductCategories",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeoPageTitle",
                table: "ProductCategories",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "Authors",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SortOrder",
                table: "Authors",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    IsDefault = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Resources = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CanCreate = table.Column<bool>(nullable: false),
                    CanDelete = table.Column<bool>(nullable: false),
                    CanRead = table.Column<bool>(nullable: false),
                    CanUpdate = table.Column<bool>(nullable: false),
                    FunctionId = table.Column<string>(nullable: false),
                    RoleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Permissions_Functions_FunctionId",
                        column: x => x.FunctionId,
                        principalTable: "Functions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Permissions_AppRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AppRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_FunctionId",
                table: "Permissions",
                column: "FunctionId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_RoleId",
                table: "Permissions",
                column: "RoleId");
        }
    }
}
