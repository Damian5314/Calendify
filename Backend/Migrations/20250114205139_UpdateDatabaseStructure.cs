using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StarterKit.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabaseStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Admin",
                keyColumn: "AdminId",
                keyValue: 1,
                column: "Password",
                value: "XohImNooBHFR0OVvjcYpJ3NgPQ1qq73WKhHvch0VQtg=");

            migrationBuilder.UpdateData(
                table: "Admin",
                keyColumn: "AdminId",
                keyValue: 2,
                column: "Password",
                value: "XE4WQDYBE/idR9XKQWU9BQ5qX47mYSUwo1EGf1Wqnlw=");

            migrationBuilder.UpdateData(
                table: "Admin",
                keyColumn: "AdminId",
                keyValue: 3,
                column: "Password",
                value: "k2oYXKqiZrucvpgengXLeM1zKwsygOuURBK7b4+PB68=");

            migrationBuilder.UpdateData(
                table: "Admin",
                keyColumn: "AdminId",
                keyValue: 4,
                column: "Password",
                value: "kl0um7NnnBud1YqyC7l0/cYfP/TbXhK7VPqwYAJhx7M=");

            migrationBuilder.UpdateData(
                table: "Admin",
                keyColumn: "AdminId",
                keyValue: 5,
                column: "Password",
                value: "RaA9gen2Oq4tkMjcHplnZAMb9omCqGJG8BGbODBd55E=");

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                table: "User",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_User_Email",
                table: "User");

            migrationBuilder.UpdateData(
                table: "Admin",
                keyColumn: "AdminId",
                keyValue: 1,
                column: "Password",
                value: "^�H��(qQ��o��)'s`=\rj���*�rB�");

            migrationBuilder.UpdateData(
                table: "Admin",
                keyColumn: "AdminId",
                keyValue: 2,
                column: "Password",
                value: "\\N@6��G��Ae=j_��a%0�QU��\\");

            migrationBuilder.UpdateData(
                table: "Admin",
                keyColumn: "AdminId",
                keyValue: 3,
                column: "Password",
                value: "�j\\��f������x�s+2��D�o���");

            migrationBuilder.UpdateData(
                table: "Admin",
                keyColumn: "AdminId",
                keyValue: 4,
                column: "Password",
                value: "�].��g��Պ��t��?��^�T��`aǳ");

            migrationBuilder.UpdateData(
                table: "Admin",
                keyColumn: "AdminId",
                keyValue: 5,
                column: "Password",
                value: "E�=���:�-����gd����bF��80]�");
        }
    }
}
