using System;

using Microsoft.EntityFrameworkCore.Migrations;

namespace UserService.Data.Migrations
{
    public partial class TagToRoleClaim : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.CreateTable(
                name: "PermissionResourceTag1",
                columns: table => new
                {
                    LockedPermissionsId = table.Column<Guid>(type: "uuid", nullable: false),
                    LockedResourceTagsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_PermissionResourceTag1", x => new { x.LockedPermissionsId, x.LockedResourceTagsId });
                    _ = table.ForeignKey(
                        name: "FK_PermissionResourceTag1_Permissions_LockedPermissionsId",
                        column: x => x.LockedPermissionsId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    _ = table.ForeignKey(
                        name: "FK_PermissionResourceTag1_ResourceTags_LockedResourceTagsId",
                        column: x => x.LockedResourceTagsId,
                        principalTable: "ResourceTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            _ = migrationBuilder.InsertData(
                table: "PermissionRole",
                columns: new[] { "PermissionsId", "RolesId" },
                values: new object[] { new Guid("00000001-0000-0000-0000-000000000000"), new Guid("00000001-0000-0000-0000-000000000000") });

            _ = migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_ClaimType",
                table: "AspNetRoleClaims",
                column: "ClaimType");

            _ = migrationBuilder.CreateIndex(
                name: "IX_PermissionResourceTag1_LockedResourceTagsId",
                table: "PermissionResourceTag1",
                column: "LockedResourceTagsId");

            _ = migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoleClaims_ResourceTags_ClaimType",
                table: "AspNetRoleClaims",
                column: "ClaimType",
                principalTable: "ResourceTags",
                principalColumn: "Tag",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoleClaims_ResourceTags_ClaimType",
                table: "AspNetRoleClaims");

            _ = migrationBuilder.DropTable(
                name: "PermissionResourceTag1");

            _ = migrationBuilder.DropIndex(
                name: "IX_AspNetRoleClaims_ClaimType",
                table: "AspNetRoleClaims");

            _ = migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionsId", "RolesId" },
                keyValues: new object[] { new Guid("00000001-0000-0000-0000-000000000000"), new Guid("00000001-0000-0000-0000-000000000000") });

            _ = migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"),
                column: "ConcurrencyStamp",
                value: "7387ce88-1bd4-4338-ba7d-3f840016577c");

            _ = migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8e7517cc-ac3f-47a4-96f0-f16026ad390a", "MDAwMjQ0MDMwMTgxMTM1MjUwMjMzMDY2MDAwMTkyMjE1MjQyMTY5MTM2MjEzMTk0MDAwMjA4MDA2MTEyMjM0MjIyMjQyMTk0MDAwMTIwMjExMTQyMDQ1MDQ2MDAwMTk1MDAwMTIyMjIwMTkzMDUwMDkxMDA4MTk1MDAwMDc0MDkzMDk0MDU4MDI1MDE1MTk1MDAwMDEzMTExMjUzMTYwMjM1MDE4MTk1MTI4MjQxMjMzMjA1MDA4MDAyMDIzMTk1MTI4MDM2MTg4MDU1MjUxMjUwMDI3MTk1LTJGQTNGNkNERUEyMjREMDg4NEQ1MUY3REVDRjYxMTdC", "1c5ac6df-b5ac-4419-a5cf-3f7f349572a9" });
        }
    }
}
