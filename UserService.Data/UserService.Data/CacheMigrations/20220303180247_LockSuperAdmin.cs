using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UserService.Data.CacheMigrations
{
    public partial class LockSuperAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"),
                columns: new[] { "Email", "UserName" },
                values: new object[] { "SuperAdmin@bergen.tech", "SuperAdmin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"),
                columns: new[] { "Email", "UserName" },
                values: new object[] { "developer@bergen.tech", "developer" });
        }
    }
}
