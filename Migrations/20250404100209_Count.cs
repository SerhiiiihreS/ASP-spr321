using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASP_spr321.Migrations
{
    /// <inheritdoc />
    public partial class Count : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "sumQw",
                schema: "ASP",
                table: "Carts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "sumQw",
                schema: "ASP",
                table: "Carts");
        }
    }
}
