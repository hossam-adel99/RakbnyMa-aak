using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RakbnyMa_aak.Migrations
{
    /// <inheritdoc />
    public partial class CreateAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Drivers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Drivers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Drivers",
                type: "datetime2",
                nullable: true);
        }
    }
}
