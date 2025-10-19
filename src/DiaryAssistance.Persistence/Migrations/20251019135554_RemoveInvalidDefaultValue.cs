using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiaryAssistance.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveInvalidDefaultValue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Expires",
                table: "RefreshTokens",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2025, 10, 19, 13, 53, 1, 888, DateTimeKind.Utc).AddTicks(9062));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Expires",
                table: "RefreshTokens",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2025, 10, 19, 13, 53, 1, 888, DateTimeKind.Utc).AddTicks(9062),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");
        }
    }
}
