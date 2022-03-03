using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UserService.Data.Migrations
{
    public partial class RmRequestNumberFromLite : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"),
                column: "ConcurrencyStamp",
                value: "2d55eabd-60e2-4af8-9e2d-38c065074fa9");

            _ = migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1ac2908d-b5a1-4e48-b42e-bc622161480c", "MDAwMDgwMjE1MTA4MDMwMDQ1MjI1MDY2MDAwMTI4MTc2MTIzMTAyMDA5MTc3MDY2MDAwMTQ0MDkzMDMyMDA2MTI5MjI0MTk0MDAwMTQ0MDU4MDkzMTYwMTI5MjQwMTk0MDAwMTYwMTIzMTI3MTY5MjM3MjQ4MTk0MDAwMTg0MTc4MTIzMDk3MDMxMDAxMTk1MDAwMTU2MDU2MDM0MjQwMDYzMDA1MTk1MDAwMDM2MDg5MTc5MjQ0MTE3MDA5MTk1MDAwMDYwMTIwMTU3MTA1MDY3MDE1MTk1LUFGMjQzQ0JFMDRFOTREMzM5NkIzNjVEQjBCRDUzODM4", "a00acefd-d89d-4a7e-851f-046bea286646" });
        }
    }
}
