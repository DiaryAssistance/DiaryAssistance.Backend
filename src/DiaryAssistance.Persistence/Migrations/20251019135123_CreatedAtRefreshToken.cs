using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiaryAssistance.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CreatedAtRefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "RefreshTokens",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2025, 10, 19, 13, 51, 22, 884, DateTimeKind.Utc).AddTicks(3439));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "RefreshTokens");
        }
    }
}
