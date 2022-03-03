using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UserService.Data.Migrations
{
    public partial class LockSuperAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"),
                column: "ConcurrencyStamp",
                value: "72f6dc0d-2fb6-47cb-9c1e-63a5284961f1");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"),
                columns: new[] { "ConcurrencyStamp", "Email", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "65bef180-8994-4139-9887-8a06b51e5b07", "SuperAdmin@bergen.tech", "SUPERADMIN@BERGEN.TECH", "SUPERADMIN", "MDAwMTYwMjUyMDE2MDkzMTQ1MjExMDY2MDAwMDAwMDgwMTQ1MTgzMjA2MTY1MTk0MDAwMTkyMDM3MDA3MDY0MDA1MjA2MTk0MDAwMTkyMDY0MjI1MDUwMjM5MjIyMTk0MDAwMDk2MDQyMDA5MDQ1MTkxMjMwMTk0MDAwMjA4MjIyMTQ1MTQ1MjI3MjM2MTk0MDAwMjA4MDM0MTczMDMzMTA5MjQyMTk0MDAwMjA4MDU5MjI1MDc0MDE3MjQ2MTk0MDAwMTkyMTU2MTIxMTI1MTQ3MjUzMTk0LUJGNTQzNjU0OTFBMDQ1RDU5ODExMTIyQjYzNUE0N0Iz", "100dd1f8-17cd-4cc7-8fea-30d852d638c6", "SuperAdmin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"),
                column: "ConcurrencyStamp",
                value: "6cca466d-a218-4905-940f-89fc6c5707e8");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"),
                columns: new[] { "ConcurrencyStamp", "Email", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "80a6a09e-81bb-4b2a-a22d-e55406dbe546", "developer@bergen.tech", "DEVELOPER@BERGEN.TECH", "DEVELOPER", "MDAwMjQwMTY1MjExMTY2MjI1MjEwMDY2MDAwMTI4MjUxMTQ5MTgwMDg2MTYwMDY2MDAwMjI0MDQyMDMyMTMwMTk1MjE0MTk0MDAwMjQwMTc2MDQ1MTg0MDY5MjI0MTk0MDAwMDQwMTQ5MDEwMTI2MTU5MjM0MTk0MDAwMjQ0MTY0MDE1MjA3MTMxMjQzMTk0MDAwMDkyMDIyMTk3MDQyMjQ2MjQ1MTk0MDAwMjUyMjI2MDI0MjUxMDM1MjUxMTk0MDAwMTU0MTQ3MDE1MTQwMDMzMDAwMTk1LUJBQUM3QTk0NUFDRDRFN0RCNUNFQTU3MkEwNUU5MkEx", "dfd06f95-0fee-4c67-89f6-8ece5e74ead3", "developer" });
        }
    }
}
