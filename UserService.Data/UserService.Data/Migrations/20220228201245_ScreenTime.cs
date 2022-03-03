using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UserService.Data.Migrations
{
    public partial class ScreenTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                value: "3cdff202-32d2-4b0c-928f-fe72bf546da8");

            _ = migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6d36eba4-f62d-4c0b-9a0d-d8c09797fe7e", "MDAwMDcyMDcxMTI1MjQxMDI1MjQ1MDY2MDAwMDY0MDczMTcyMjMyMTg4MTk3MDY2MDAwMjMyMDI3MTk2MTI0MDk0MjQxMTk0MDAwMDM2MTYwMTYwMDgxMDU1MDAyMTk1MDAwMTI0MjA0MDIxMTQwMTQ4MDA3MTk1MDAwMDQzMjQ2MjA4MTA1MjA3MDE2MTk1MDAwMTg3MDAxMDU1MDg0MTQ3MDIxMTk1MDAwMDYwMTA0MDE2MTM2MDY1MDI0MTk1MTI4MTE3MDYxMDM2MTM0MTQxMDI4MTk1LUU2NkYzQUFBQ0JFMTQ2RkVCRjZDRkE1QUREQzExMEY0", "9bca7b37-bd00-41a7-8c6a-a22e0c5d22b2" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropColumn(
                name: "ScreenTime",
                table: "AspNetUsers");

            _ = migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"),
                column: "ConcurrencyStamp",
                value: "3c763e1b-b8e8-4ddf-80e9-b5fb281d6ed5");

            _ = migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "3141ee40-0acb-465b-a35e-5e46dcac13a1", "MDAwMTYwMDI0MTkwMTg5MDA0MjMxMDY2MDAwMDk2MDQyMTYzMTMxMjM5MTk1MTk0MDAwMjUyMDk1MDMwMDI4MTU2MjMyMTk0MDAwMTE0MTExMTMwMjI5MDMxMjQ5MTk0MDAwMjAxMjQxMTcwMDg1MTg2MDAxMTk1MDAwMDc4MjU0MjMzMjI3MTYxMDA2MTk1MDAwMDMwMDQ3MTU3MTYxMDExMDEzMTk1MTI4MTc1MTY2MDQ4MDE5MDI3MDE3MTk1MTI4MDk5MDExMTM5MDIzMTgwMDIwMTk1LTFCMDhFQjA1MEMxMDQzRDhBN0NDQUFDNTUwMzE3N0NC", "a89f686f-b08e-486d-b25b-7aa20f4fe799" });
        }
    }
}
