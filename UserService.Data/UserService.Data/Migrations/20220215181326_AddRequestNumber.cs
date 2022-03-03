using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UserService.Data.Migrations
{
    public partial class AddRequestNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AddColumn<string>(
                name: "RequestNumber",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropColumn(
                name: "RequestNumber",
                table: "AspNetUsers");

            _ = migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"),
                column: "ConcurrencyStamp",
                value: "3c7a163b-41b0-46f4-be47-b58106e379cc");

            _ = migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "3f6f55e2-c22d-4d5a-9fd5-c8d4089d09d9", "MDAwMDAwMDU5MDQzMTYyMDA5MjA5MDY2MDAwMDg4MDUwMDgyMTkyMDIyMjAyMTk0MDAwMTY2MTM0MDAxMDkxMjMyMjI0MTk0MDAwMDk0MjI5MTA5MTczMDIyMjM2MTk0MDAwMjQ1MDUwMjM4MTYwMTYxMjQ1MTk0MDAwMDExMDAxMTE0MDkyMjExMjUwMTk0MTI4MDUxMDI0MjEyMDY2MDUzMDAwMTk1MTI4MDg2MTg0MjM5MTAzMDAwMDA0MTk1MDAwMTQwMjQ2MDkyMTYyMjExMDA4MTk1LTY1NzE4QUNFN0NEMzRCMEVBRTMxRUYwNEEzQkI1Qjc0", "013365d4-62cf-486b-8be3-e68ac9d0c464" });
        }
    }
}
