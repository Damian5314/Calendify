using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StarterKit.Migrations
{
    /// <inheritdoc />
    public partial class AddRefreshTokenToAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "Admin",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenExpiry",
                table: "Admin",
                type: "TEXT",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Admin",
                keyColumn: "AdminId",
                keyValue: 1,
                columns: new[] { "RefreshToken", "RefreshTokenExpiry" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Admin",
                keyColumn: "AdminId",
                keyValue: 2,
                columns: new[] { "RefreshToken", "RefreshTokenExpiry" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Admin",
                keyColumn: "AdminId",
                keyValue: 3,
                columns: new[] { "RefreshToken", "RefreshTokenExpiry" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Admin",
                keyColumn: "AdminId",
                keyValue: 4,
                columns: new[] { "RefreshToken", "RefreshTokenExpiry" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Admin",
                keyColumn: "AdminId",
                keyValue: 5,
                columns: new[] { "RefreshToken", "RefreshTokenExpiry" },
                values: new object[] { null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "Admin");

            migrationBuilder.DropColumn(
                name: "RefreshTokenExpiry",
                table: "Admin");
        }
    }
}
