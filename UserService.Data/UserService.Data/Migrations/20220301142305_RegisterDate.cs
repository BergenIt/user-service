using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UserService.Data.Migrations
{
    public partial class RegisterDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AddColumn<DateTime>(
                name: "RegistredDate",
                table: "AspNetUsers",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            _ = migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"),
                column: "ConcurrencyStamp",
                value: "00bc7d9e-9cba-407b-aea2-7be73aec89c1");

            _ = migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "654d522b-4127-413a-9003-899b4b3fd637", "MDAwMTI1MDIxMTM5MDM1MDA4MjQxMDY2MDAwMTkyMTE5MDcxMjExMDM2MTU5MTk0MDAwMjA4MTUzMTE3MTU4MDkxMjMxMTk0MDAwMjUyMTEzMTg1MTMyMTgxMjQ2MTk0MDAwMDcyMTYxMTkyMDQwMjAzMDA2MTk1MDAwMDA5MDg1MDk5MjM1MDk5MDEyMTk1MTI4MjAxMDgzMDE3MjI3MjQzMDE2MTk1MTkyMTk3MTE0MjE5MTY1MTcxMDIyMTk1MDY0MTgxMDA3MDAyMDM5MDQxMDI2MTk1LThEQTBGRENFM0YyRDRBMUM5MDgwQkRGMUZGQ0I1OTcz", "55eeb423-e06a-4360-b5e8-b3366578d0fa" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropColumn(
                name: "RegistredDate",
                table: "AspNetUsers");

            _ = migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"),
                column: "ConcurrencyStamp",
                value: "3cdff202-32d2-4b0c-928f-fe72bf546da8");

            _ = migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6d36eba4-f62d-4c0b-9a0d-d8c09797fe7e", "MDAwMDcyMDcxMTI1MjQxMDI1MjQ1MDY2MDAwMDY0MDczMTcyMjMyMTg4MTk3MDY2MDAwMjMyMDI3MTk2MTI0MDk0MjQxMTk0MDAwMDM2MTYwMTYwMDgxMDU1MDAyMTk1MDAwMTI0MjA0MDIxMTQwMTQ4MDA3MTk1MDAwMDQzMjQ2MjA4MTA1MjA3MDE2MTk1MDAwMTg3MDAxMDU1MDg0MTQ3MDIxMTk1MDAwMDYwMTA0MDE2MTM2MDY1MDI0MTk1MTI4MTE3MDYxMDM2MTM0MTQxMDI4MTk1LUU2NkYzQUFBQ0JFMTQ2RkVCRjZDRkE1QUREQzExMEY0", "9bca7b37-bd00-41a7-8c6a-a22e0c5d22b2" });
        }
    }
}
