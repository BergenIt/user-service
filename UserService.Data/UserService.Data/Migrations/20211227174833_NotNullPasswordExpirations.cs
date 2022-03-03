using System;

using Microsoft.EntityFrameworkCore.Migrations;

namespace UserService.Data.Migrations
{
    public partial class NotNullPasswordExpirations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AlterColumn<DateTime>(
                name: "PasswordExpiration",
                table: "AspNetUsers",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            _ = migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"),
                column: "ConcurrencyStamp",
                value: "f18bc9cd-1c68-4f68-adb8-38408783adc1");

            _ = migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ad9faeb2-91c7-4db5-adf2-dd5fb485df54", "MDAwMDAwMTU4MjAxMTg2MjE0MjEzMDY2MDAwMDAwMDIyMDczMDQ4MTgxMTQ5MDY2MDAwMTYwMjIzMDc1MDM5MjA0MjExMTk0MDAwMDE2MjQ4MDM1MDA4MjM0MjI3MTk0MDAwMDE2MTk5MTM2MTAxMjEzMjM4MTk0MDAwMjAwMTI3MDg4MTQ1MTgwMjQ0MTk0MDAwMTY4MDk0MjA5MTk4MTgxMjQ5MTk0MDAwMDQwMTk4MTMxMTE3MDQzMjU1MTk0MDAwMDk2MTMzMjExMTQyMDk1MDAyMTk1LUI0NzU1NzVFMDNGQTRBOTlCRkQxNzFEMEZGMUY5NEIy", "29d542a1-381c-4481-9665-634fcd52a973" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AlterColumn<DateTime>(
                name: "PasswordExpiration",
                table: "AspNetUsers",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            _ = migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"),
                column: "ConcurrencyStamp",
                value: "fdaf1537-c8df-4be9-b7ae-8cf88cada3e0");

            _ = migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4a2e1c79-5499-4d90-9fbf-c044258ca004", "MDAwMTkyMDcxMTM4MjU0MDczMjA5MDY2MDAwMTI4MTY1MDIxMDQyMTgxMjA0MTk0MDAwMTg0MjQ4MTI5MTEwMjA1MjI3MTk0MDAwMTY4MjA3MjQwMTgzMTk1MjM1MTk0MDAwMTQ4MDIyMDI4MjE2MDcyMjQzMTk0MDAwMDY0MDk0MDI2MjM0MTUyMjQ5MTk0MDAwMTg0MjAxMjA5MDE0MTQ4MjUzMTk0MDAwMDE4MTI4MDQ2MTUxMTI1MDAxMTk1MDAwMTYyMDg2MjEwMDkxMjQ3MDAzMTk1LUI1NkY0M0ZENDZFRDQwMjg4Nzk1NUQzNjhEOEM3RjNC", "75e7097a-7ec8-4340-9859-bd32643f26b2" });
        }
    }
}
