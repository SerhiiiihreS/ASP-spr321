using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASP_spr321.Migrations
{
    /// <inheritdoc />
    public partial class New : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClothingSize",
                schema: "ASP",
                table: "UsersData",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DateBirth",
                schema: "ASP",
                table: "UsersData",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FingerSize",
                schema: "ASP",
                table: "UsersData",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShoeSize",
                schema: "ASP",
                table: "UsersData",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClothingSize",
                schema: "ASP",
                table: "UsersData");

            migrationBuilder.DropColumn(
                name: "DateBirth",
                schema: "ASP",
                table: "UsersData");

            migrationBuilder.DropColumn(
                name: "FingerSize",
                schema: "ASP",
                table: "UsersData");

            migrationBuilder.DropColumn(
                name: "ShoeSize",
                schema: "ASP",
                table: "UsersData");
        }
    }
}
