using System;

using Microsoft.EntityFrameworkCore.Migrations;

namespace UserService.Data.Migrations
{
    public partial class RoleExpiration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AddColumn<DateTime>(
                name: "RoleExpiration",
                table: "AspNetRoles",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            _ = migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"),
                columns: new[] { "ConcurrencyStamp", "RoleExpiration" },
                values: new object[] { "f18389eb-4a8f-4e0a-ad22-176a49711d5d", new DateTime(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Unspecified).AddTicks(9999) });

            _ = migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "90453dae-5452-454a-a9aa-8911cfe083c6", "MDAwMTI2MTk1MDc0MDY0MTUxMjQ0MDY2MDAwMTkyMjUzMjA0MDA5MDg2MTY1MDY2MDAwMTQwMTEwMTU0MTQyMTQ4MjM0MTk0MDAwMDUxMjE2MjAzMDQxMTAxMDAyMTk1MDAwMTkwMjUwMTY3MDczMDMwMDA5MTk1MDAwMjM4MTI5MDAyMDUzMDEyMDE2MTk1MDAwMTI5MTA5MTk5MDQyMjM1MDIxMTk1MTI4MDI5MDgxMTY5MTQyMDcxMDI1MTk1MTI4MDMzMTMyMTA5MDMzMDYxMDI5MTk1LUVDMEVDQUUzMzkxNDRGM0Q5MTM1NTEyNEUzMUFFN0RD", "5139162c-3f93-4268-89b4-8d6293755897" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropColumn(
                name: "RoleExpiration",
                table: "AspNetRoles");

            _ = migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"),
                column: "ConcurrencyStamp",
                value: "2de5c7ea-3111-4bd5-8150-98ceaa26a3c4");

            _ = migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "cee28b38-ef0c-4f92-b5a3-2710b70058ca", "MDAwMTkyMTE5MDcxMjExMDM2MjM5MDY2MDAwMDAwMDgzMTUwMTkxMTE1MTU5MTk0MDAwMTYyMjU1MDY0MTc2MDY2MjM1MTk0MTI4MDI5MTk5MDY1MjA2MDkxMDAxMTk1MDAwMTY1MTMxMDUwMTI4MTc1MDEwMTk1MTkyMTE1MjAzMDk3MTYyMTYwMDE2MTk1MDY0MDE0MTQzMTIyMDUxMjMwMDIxMTk1MDY0MjMzMTQ3MDMyMTA0MTQ0MDI2MTk1MTkyMjUzMDU4MTEyMjE2MTc1MDI5MTk1LThCNDA4NzkwRjZFQzRBOEFBNUIwQTlDRDZDNTAzNDUz", "189493bd-d0f0-41a6-b788-6d4bd47fea80" });
        }
    }
}
