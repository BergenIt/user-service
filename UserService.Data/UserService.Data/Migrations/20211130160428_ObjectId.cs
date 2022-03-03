using System;

using Microsoft.EntityFrameworkCore.Migrations;

namespace UserService.Data.Migrations
{
    public partial class ObjectId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.RenameColumn(
                name: "ContourId",
                table: "Notifications",
                newName: "ObjectId");

            _ = migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"),
                column: "ConcurrencyStamp",
                value: "00af9206-2169-4a18-9ab3-9c837de268a0");

            _ = migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b817721a-942f-4a37-b6b0-3a826e59e1ee", "MDAwMDk2MTE0MjEzMjEwMjUwMjI1MDY2MDAwMTkyMTE3MjM4MDY2MTY1MjA5MDY2MDAwMDAwMTczMjM0MTY2MTU0MTgzMTk0MDAwMDgwMjI5MDM0MTI4MTgxMjMxMTk0MDAwMDA4MTM5MDQ0MTkxMjMxMjQxMTk0MDAwMDcyMjQ2MDI1MDMyMjAyMjQ3MTk0MDAwMTEyMDcyMjM3MTcwMDIyMDAxMTk1MDAwMDU2MDc0MTAzMTk5MDI4MDA0MTk1MDAwMDI4MjA4MDEzMDg2MDYxMDA4MTk1LUFFQjk5RTMzQTg4QjQzNTU4REZBNDI1MTJFQjM1QTky", "09b7335a-8de6-45d8-b18f-c13147d91201" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.RenameColumn(
                name: "ObjectId",
                table: "Notifications",
                newName: "ContourId");

            _ = migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"),
                column: "ConcurrencyStamp",
                value: "f5c83589-6314-40cc-8f73-80eae74d33a8");

            _ = migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ed680f3d-caef-4a88-b581-279b4df93a8f", "MDAwMTYwMTkwMDE2MTk1MjAzMjQwMDY2MDAwMDgwMDc3MTM1MDc5MjA0MjE3MDY2MDAwMTkyMTkxMTA0MjE4MDQ0MjA3MTk0MDAwMDg0MTE0MjI3MTkzMTIxMjM5MTk0MDAwMTU2MTU0MTE2MTAxMjMwMjQ3MTk0MDAwMjQ0MTk0MDgxMTM4MDMxMDAxMTk1MDAwMTQxMDE5MDM2MDQ1MDExMDA3MTk1MDAwMTkzMDY1MTYwMDYzMDMyMDExMTk1MDAwMTE2MjIwMjUyMDc1MTQxMDE1MTk1LTdDNjE2MzMwNjQyRDRCNkY4MjY0NUQ0NTI5MjZCNjc2", "3ddcaf81-b82c-4440-b91a-c228711aa400" });
        }
    }
}
