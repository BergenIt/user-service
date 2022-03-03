using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UserService.Data.Migrations
{
    public partial class RmScreenTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropColumn(
                name: "ScreenTime",
                table: "AspNetUsers");

            _ = migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"),
                column: "ConcurrencyStamp",
                value: "9ded1cb3-85ee-4268-b697-52af6e786c8a");

            _ = migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "271a119e-7a0b-4785-a604-998f616a9661", "MDAwMTI4MTA0MDA1MDY4MDI5MjQ1MDY2MDAwMTYwMDI2MTE2MDk1MDA3MTc2MDY2MDAwMDk5MTI0MTU3MjA1MjQ4MjQxMTk0MDAwMDI5MTY0MTU2MjI3MDc2MDAyMTk1MDAwMjExMDQ3MjM1MDYxMDYzMDA3MTk1MTkyMDY2MjI5MDk4MjQ3MDkyMDE2MTk1MTI4MTU5MTcyMjIwMTc1MDA1MDIxMTk1MTI4MDcyMDY4MTU1MTg4MTI2MDIzMTk1MDAwMTY5MTc1MjQ4MDgzMDA3MDI4MTk1LTlBOUNFQzhGQUI0MzQ5RDRCMEE1RjFFMENEMDdBMEI5", "3ce92689-db2b-4ddc-96ee-c16e588ce464" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AddColumn<TimeSpan>(
                name: "ScreenTime",
                table: "AspNetUsers",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

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
    }
}
