using System;

using Microsoft.EntityFrameworkCore.Migrations;

namespace UserService.Data.Migrations
{
    public partial class RelationTagPermissionAssert : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AddColumn<int>(
                name: "PermissionAssert",
                table: "ResourceTagRelations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            _ = migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"),
                column: "ConcurrencyStamp",
                value: "abddf57d-85a6-4176-82d9-69cb47f3d0de");

            _ = migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e93fdb49-bfee-43db-b5fe-b422fc6dcff1", "MDAwMTI4MDA0MTk1MDUyMjUyMjIxMDY2MDAwMjQwMDQ0MjUwMDY2MTM1MjIyMTk0MDAwMDc2MTUyMDUyMTc1MTUyMjQ3MTk0MDAwMTIyMTI0MDM0MTQ4MTcyMjU0MTk0MDAwMTE4MTg3MTI4MDc5MjAyMDA1MTk1MDAwMTg1MjMxMDAwMjQzMTk3MDEzMTk1MTI4MTc1MTQxMjQ2MjA3MTY3MDE2MTk1MDAwMDM0MTkxMDM1MTI2MjI1MDE5MTk1MDAwMDUzMjA5MjQxMTUyMjIyMDIzMTk1LUQ4MzhEMTc5NzhFRjRFRDU4RkQ3Njc1RkI1N0Q0MTM0", "bab36061-f4e7-4cb8-afb8-afd4d1e64309" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropColumn(
                name: "PermissionAssert",
                table: "ResourceTagRelations");

            _ = migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"),
                column: "ConcurrencyStamp",
                value: "a4f73ca9-3220-4e57-98c4-e85e86a7e671");

            _ = migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "49889b14-eeb6-44fc-90bc-d099715be8e3", "MDAwMTYwMTU4MDU3MTY2MDU5MjI4MDY2MDAwMDY0MjA2MDI3MTYyMTM0MjE0MTk0MDAwMDY4MjE4MjI1MDU5MDcyMjQyMTk0MDAwMTQ4MTk0MjEwMDMxMDA1MjUxMTk0MDAwMDA3MTgyMDI0MTE4MDkyMDAxMTk1MDAwMTUzMTgzMDEwMDI2MTc1MDA3MTk1MDAwMDA1MDE0MDM3MDI3MDE0MDEyMTk1MDAwMTk0MDk4MDg0MDAxMjMyMDE1MTk1MDAwMDU2MjA4MDk5MDgwMDc5MDE4MTk1LTdBQUMwRDJGNjQ4RDQxODg5NTZDRkU1NDNDNjZGMTBB", "b7ffe63f-ec64-430c-aba1-917191cd80ff" });
        }
    }
}
