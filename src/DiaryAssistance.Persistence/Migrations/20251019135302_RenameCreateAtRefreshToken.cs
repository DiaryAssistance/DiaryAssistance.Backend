using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiaryAssistance.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RenameCreateAtRefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "RefreshTokens");

            migrationBuilder.AddColumn<DateTime>(
                name: "Expires",
                table: "RefreshTokens",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2025, 10, 19, 13, 53, 1, 888, DateTimeKind.Utc).AddTicks(9062));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Expires",
                table: "RefreshTokens");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "RefreshTokens",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2025, 10, 19, 13, 51, 22, 884, DateTimeKind.Utc).AddTicks(3439));
        }
    }
}
