using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASP_spr321.Migrations
{
    /// <inheritdoc />
    public partial class ProductAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Categories_Slug",
                schema: "ASP",
                table: "Categories");

            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                schema: "ASP",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateTable(
                name: "Products",
                schema: "ASP",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImagesCsv = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "ASP",
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ParentId",
                schema: "ASP",
                table: "Categories",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                schema: "ASP",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Categories_ParentId",
                schema: "ASP",
                table: "Categories",
                column: "ParentId",
                principalSchema: "ASP",
                principalTable: "Categories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Categories_ParentId",
                schema: "ASP",
                table: "Categories");

            migrationBuilder.DropTable(
                name: "Products",
                schema: "ASP");

            migrationBuilder.DropIndex(
                name: "IX_Categories_ParentId",
                schema: "ASP",
                table: "Categories");

            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                schema: "ASP",
                table: "Categories",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Slug",
                schema: "ASP",
                table: "Categories",
                column: "Slug",
                unique: true);
        }
    }
}
