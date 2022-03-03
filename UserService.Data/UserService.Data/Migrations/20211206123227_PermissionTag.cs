using System;

using Microsoft.EntityFrameworkCore.Migrations;

namespace UserService.Data.Migrations
{
    public partial class PermissionTag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropForeignKey(
                name: "FK_Permissions_PermissionTags_PermissionTagId",
                table: "Permissions");

            _ = migrationBuilder.AlterColumn<string>(
                name: "Tag",
                table: "PermissionTags",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            _ = migrationBuilder.AddColumn<Guid>(
                name: "ResourceTagId",
                table: "Permissions",
                type: "uuid",
                nullable: true);

            _ = migrationBuilder.AddUniqueConstraint(
                name: "AK_PermissionTags_Tag",
                table: "PermissionTags",
                column: "Tag");

            _ = migrationBuilder.CreateTable(
                name: "ResourceTagRelation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ResourceTagValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_ResourceTagRelation", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_ResourceTagRelation_PermissionTags_ResourceTagValue",
                        column: x => x.ResourceTagValue,
                        principalTable: "PermissionTags",
                        principalColumn: "Tag",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"),
                column: "ConcurrencyStamp",
                value: "3675f29c-0018-42a4-ad8d-0b0fe856070b");

            _ = migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1d7f0553-ccfe-4845-bd70-bbd429cb6535", "MDAwMTUyMTI3MTAxMjA2MjEyMjE5MDY2MDAwMDAwMTU5MTAwMTM1MjAyMTQ3MTk0MDAwMjQwMDYyMDIyMDU0MjM3MjIyMTk0MDAwMjQ2MDk1MTY1MDIxMTE4MjM4MTk0MDAwMTQ3MDgzMTMyMTg4MDg4MjQ2MTk0MDAwMDUxMDE1MDc0MTc1MTk2MjUzMTk0MDAwMTY2MTEwMTgzMjAxMTYyMDAyMTk1MDAwMDAwMTI5MDY2MTM5MDQ5MDA2MTk1MDAwMTU2MDI2MjUwMjI3MDA1MDEwMTk1LTA4QTU2RTMzMEI0QjQ5MDdBQzczMUFCMTdFQkUxREFC", "86295945-d749-4ef1-b8df-0e506db7a7d8" });

            _ = migrationBuilder.CreateIndex(
                name: "IX_Permissions_ResourceTagId",
                table: "Permissions",
                column: "ResourceTagId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_ResourceTagRelation_ResourceTagValue",
                table: "ResourceTagRelation",
                column: "ResourceTagValue");

            _ = migrationBuilder.AddForeignKey(
                name: "FK_Permissions_PermissionTags_ResourceTagId",
                table: "Permissions",
                column: "ResourceTagId",
                principalTable: "PermissionTags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropForeignKey(
                name: "FK_Permissions_PermissionTags_ResourceTagId",
                table: "Permissions");

            _ = migrationBuilder.DropTable(
                name: "ResourceTagRelation");

            _ = migrationBuilder.DropUniqueConstraint(
                name: "AK_PermissionTags_Tag",
                table: "PermissionTags");

            _ = migrationBuilder.DropIndex(
                name: "IX_Permissions_ResourceTagId",
                table: "Permissions");

            _ = migrationBuilder.DropColumn(
                name: "ResourceTagId",
                table: "Permissions");

            _ = migrationBuilder.AlterColumn<string>(
                name: "Tag",
                table: "PermissionTags",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

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

            _ = migrationBuilder.AddForeignKey(
                name: "FK_Permissions_PermissionTags_PermissionTagId",
                table: "Permissions",
                column: "PermissionTagId",
                principalTable: "PermissionTags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
