using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UserService.Data.Migrations
{
    public partial class DefaultSuperAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"),
                columns: new[] { "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "1e8fbbaf-5af2-4e35-b719-82814d12de0c", "SuperAdmin", "SUPERADMIN" });

            _ = migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "79c56f16-e733-49e7-8148-3e4405f4ecd9", "MDAwMjI4MTk1MTgyMDMyMTIxMjMwMDY2MDAwMDY0MDUxMjUzMDI1MDUwMTY2MTk0MDAwMTU2MDY0MjUzMDA1MjI0MjI2MTk0MDAwMTE2MjEzMjQ5MTgwMDE1MjQ2MTk0MDAwMTU0MDMzMDEzMjQ1MTk2MDAzMTk1MDAwMTY2MTAzMjIzMjQ0MDM1MDA4MTk1MDAwMDU3MDAyMjIxMjA1MTE1MDE0MTk1MTI4MDI5MTAzMTY4MDYwMTUzMDE5MTk1MTI4MTQxMDg5MDY2MTgwMTIzMDIzMTk1LUQyQ0I5NkI5NjI2NTQwODc5RkFBNjg2QTY1MkJCMjU5", "62a0b1ef-bb6b-4bf5-81d4-93fd7aa0a8ed" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"),
                columns: new[] { "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "f18bc9cd-1c68-4f68-adb8-38408783adc1", "Admin", "ADMIN" });

            _ = migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ad9faeb2-91c7-4db5-adf2-dd5fb485df54", "MDAwMDAwMTU4MjAxMTg2MjE0MjEzMDY2MDAwMDAwMDIyMDczMDQ4MTgxMTQ5MDY2MDAwMTYwMjIzMDc1MDM5MjA0MjExMTk0MDAwMDE2MjQ4MDM1MDA4MjM0MjI3MTk0MDAwMDE2MTk5MTM2MTAxMjEzMjM4MTk0MDAwMjAwMTI3MDg4MTQ1MTgwMjQ0MTk0MDAwMTY4MDk0MjA5MTk4MTgxMjQ5MTk0MDAwMDQwMTk4MTMxMTE3MDQzMjU1MTk0MDAwMDk2MTMzMjExMTQyMDk1MDAyMTk1LUI0NzU1NzVFMDNGQTRBOTlCRkQxNzFEMEZGMUY5NEIy", "29d542a1-381c-4481-9665-634fcd52a973" });
        }
    }
}
