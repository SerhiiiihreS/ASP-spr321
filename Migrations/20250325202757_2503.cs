using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASP_spr321.Migrations
{
    /// <inheritdoc />
    public partial class _2503 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ActionId",
                schema: "ASP",
                table: "CartItems",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActionId",
                schema: "ASP",
                table: "CartItems");
        }
    }
}
