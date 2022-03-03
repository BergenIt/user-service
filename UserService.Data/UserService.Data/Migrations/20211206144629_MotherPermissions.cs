using System;

using Microsoft.EntityFrameworkCore.Migrations;

namespace UserService.Data.Migrations
{
    public partial class MotherPermissions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Positions_PositionId",
                table: "AspNetUsers");

            _ = migrationBuilder.DropForeignKey(
                name: "FK_Permissions_AspNetRoles_RoleId",
                table: "Permissions");

            _ = migrationBuilder.DropForeignKey(
                name: "FK_Permissions_ResourceTags_ResourceTagId",
                table: "Permissions");

            _ = migrationBuilder.DropIndex(
                name: "IX_Permissions_ResourceTagId_RoleId",
                table: "Permissions");

            _ = migrationBuilder.DropIndex(
                name: "IX_Permissions_RoleId",
                table: "Permissions");

            _ = migrationBuilder.DropColumn(
                name: "PermissionAssert",
                table: "Permissions");

            _ = migrationBuilder.DropColumn(
                name: "ResourceTagId",
                table: "Permissions");

            _ = migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Permissions");

            _ = migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "Permissions",
                type: "text",
                nullable: true);

            _ = migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Permissions",
                type: "text",
                nullable: true);

            _ = migrationBuilder.CreateTable(
                name: "PermissionPermission",
                columns: table => new
                {
                    ChildPermissionsId = table.Column<Guid>(type: "uuid", nullable: false),
                    MotherPermissionsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_PermissionPermission", x => new { x.ChildPermissionsId, x.MotherPermissionsId });
                    _ = table.ForeignKey(
                        name: "FK_PermissionPermission_Permissions_ChildPermissionsId",
                        column: x => x.ChildPermissionsId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    _ = table.ForeignKey(
                        name: "FK_PermissionPermission_Permissions_MotherPermissionsId",
                        column: x => x.MotherPermissionsId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "PermissionResourceTag",
                columns: table => new
                {
                    PermissionsId = table.Column<Guid>(type: "uuid", nullable: false),
                    ResourceTagsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_PermissionResourceTag", x => new { x.PermissionsId, x.ResourceTagsId });
                    _ = table.ForeignKey(
                        name: "FK_PermissionResourceTag_Permissions_PermissionsId",
                        column: x => x.PermissionsId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    _ = table.ForeignKey(
                        name: "FK_PermissionResourceTag_ResourceTags_ResourceTagsId",
                        column: x => x.ResourceTagsId,
                        principalTable: "ResourceTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "PermissionRole",
                columns: table => new
                {
                    PermissionsId = table.Column<Guid>(type: "uuid", nullable: false),
                    RolesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_PermissionRole", x => new { x.PermissionsId, x.RolesId });
                    _ = table.ForeignKey(
                        name: "FK_PermissionRole_AspNetRoles_RolesId",
                        column: x => x.RolesId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    _ = table.ForeignKey(
                        name: "FK_PermissionRole_Permissions_PermissionsId",
                        column: x => x.PermissionsId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            _ = migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "Comment", "Name" },
                values: new object[] { new Guid("00000001-0000-0000-0000-000000000000"), null, "Default" });

            _ = migrationBuilder.CreateIndex(
                name: "IX_PermissionPermission_MotherPermissionsId",
                table: "PermissionPermission",
                column: "MotherPermissionsId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_PermissionResourceTag_ResourceTagsId",
                table: "PermissionResourceTag",
                column: "ResourceTagsId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_PermissionRole_RolesId",
                table: "PermissionRole",
                column: "RolesId");

            _ = migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Positions_PositionId",
                table: "AspNetUsers",
                column: "PositionId",
                principalTable: "Positions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Positions_PositionId",
                table: "AspNetUsers");

            _ = migrationBuilder.DropTable(
                name: "PermissionPermission");

            _ = migrationBuilder.DropTable(
                name: "PermissionResourceTag");

            _ = migrationBuilder.DropTable(
                name: "PermissionRole");

            _ = migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"));

            _ = migrationBuilder.DropColumn(
                name: "Comment",
                table: "Permissions");

            _ = migrationBuilder.DropColumn(
                name: "Name",
                table: "Permissions");

            _ = migrationBuilder.AddColumn<int>(
                name: "PermissionAssert",
                table: "Permissions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            _ = migrationBuilder.AddColumn<Guid>(
                name: "ResourceTagId",
                table: "Permissions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            _ = migrationBuilder.AddColumn<Guid>(
                name: "RoleId",
                table: "Permissions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            _ = migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"),
                column: "ConcurrencyStamp",
                value: "2e65be62-2425-4831-9a16-42b16b1b96ed");

            _ = migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "09a9b178-dc13-4f54-92e6-0c8c00470145", "MDAwMjI4MDc1MDc1MDEyMTYxMjMyMDY2MDAwMjA0MDU3MjUyMDQwMTQ1MjMwMTk0MDAwMDIwMTM1MDg2MDMyMjU0MjUyMTk0MDAwMTQzMTg3MDM1MDM0MTI5MDA2MTk1MDAwMTc1MDU2MDQzMjQ3MDE2MDEyMTk1MDAwMjI2MTQ4MTQyMTY0MTE3MDE4MTk1MTI4MDkzMTI5MDE5MTIxMTE4MDIyMTk1MTI4MjIzMTIxMDEwMTM2MDYyMDI1MTk1MTI4MTc5MjQ5MTYzMTI2MDMxMDI5MTk1LTI1QzZFRkRGMzZGRjRDQ0RBQTlGNDgyNUUwRDI3NEUz", "9b4c6253-a351-40e3-b396-cf771b8240bb" });

            _ = migrationBuilder.CreateIndex(
                name: "IX_Permissions_ResourceTagId_RoleId",
                table: "Permissions",
                columns: new[] { "ResourceTagId", "RoleId" },
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_Permissions_RoleId",
                table: "Permissions",
                column: "RoleId");

            _ = migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Positions_PositionId",
                table: "AspNetUsers",
                column: "PositionId",
                principalTable: "Positions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            _ = migrationBuilder.AddForeignKey(
                name: "FK_Permissions_AspNetRoles_RoleId",
                table: "Permissions",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            _ = migrationBuilder.AddForeignKey(
                name: "FK_Permissions_ResourceTags_ResourceTagId",
                table: "Permissions",
                column: "ResourceTagId",
                principalTable: "ResourceTags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
