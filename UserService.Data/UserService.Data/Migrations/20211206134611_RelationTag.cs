using System;

using Microsoft.EntityFrameworkCore.Migrations;

namespace UserService.Data.Migrations
{
    public partial class RelationTag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropForeignKey(
                name: "FK_Permissions_PermissionTags_ResourceTagId",
                table: "Permissions");

            _ = migrationBuilder.DropForeignKey(
                name: "FK_ResourceTagRelation_PermissionTags_ResourceTagValue",
                table: "ResourceTagRelation");

            _ = migrationBuilder.DropIndex(
                name: "IX_Permissions_PermissionTagId_RoleId",
                table: "Permissions");

            _ = migrationBuilder.DropIndex(
                name: "IX_Permissions_ResourceTagId",
                table: "Permissions");

            _ = migrationBuilder.DropPrimaryKey(
                name: "PK_ResourceTagRelation",
                table: "ResourceTagRelation");

            _ = migrationBuilder.DropUniqueConstraint(
                name: "AK_PermissionTags_Tag",
                table: "PermissionTags");

            _ = migrationBuilder.DropPrimaryKey(
                name: "PK_PermissionTags",
                table: "PermissionTags");

            _ = migrationBuilder.DropColumn(
                name: "PermissionTagId",
                table: "Permissions");

            _ = migrationBuilder.RenameTable(
                name: "ResourceTagRelation",
                newName: "ResourceTagRelations");

            _ = migrationBuilder.RenameTable(
                name: "PermissionTags",
                newName: "ResourceTags");

            _ = migrationBuilder.RenameIndex(
                name: "IX_ResourceTagRelation_ResourceTagValue",
                table: "ResourceTagRelations",
                newName: "IX_ResourceTagRelations_ResourceTagValue");

            _ = migrationBuilder.AlterColumn<Guid>(
                name: "ResourceTagId",
                table: "Permissions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            _ = migrationBuilder.AddColumn<string>(
                name: "DependentResourceTagValue",
                table: "ResourceTagRelations",
                type: "text",
                nullable: true);

            _ = migrationBuilder.AddPrimaryKey(
                name: "PK_ResourceTagRelations",
                table: "ResourceTagRelations",
                column: "Id");

            _ = migrationBuilder.AddUniqueConstraint(
                name: "AK_ResourceTags_Tag",
                table: "ResourceTags",
                column: "Tag");

            _ = migrationBuilder.AddPrimaryKey(
                name: "PK_ResourceTags",
                table: "ResourceTags",
                column: "Id");

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
                name: "IX_ResourceTagRelations_DependentResourceTagValue",
                table: "ResourceTagRelations",
                column: "DependentResourceTagValue");

            _ = migrationBuilder.AddForeignKey(
                name: "FK_Permissions_ResourceTags_ResourceTagId",
                table: "Permissions",
                column: "ResourceTagId",
                principalTable: "ResourceTags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            _ = migrationBuilder.AddForeignKey(
                name: "FK_ResourceTagRelations_ResourceTags_DependentResourceTagValue",
                table: "ResourceTagRelations",
                column: "DependentResourceTagValue",
                principalTable: "ResourceTags",
                principalColumn: "Tag",
                onDelete: ReferentialAction.Cascade);

            _ = migrationBuilder.AddForeignKey(
                name: "FK_ResourceTagRelations_ResourceTags_ResourceTagValue",
                table: "ResourceTagRelations",
                column: "ResourceTagValue",
                principalTable: "ResourceTags",
                principalColumn: "Tag",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropForeignKey(
                name: "FK_Permissions_ResourceTags_ResourceTagId",
                table: "Permissions");

            _ = migrationBuilder.DropForeignKey(
                name: "FK_ResourceTagRelations_ResourceTags_DependentResourceTagValue",
                table: "ResourceTagRelations");

            _ = migrationBuilder.DropForeignKey(
                name: "FK_ResourceTagRelations_ResourceTags_ResourceTagValue",
                table: "ResourceTagRelations");

            _ = migrationBuilder.DropIndex(
                name: "IX_Permissions_ResourceTagId_RoleId",
                table: "Permissions");

            _ = migrationBuilder.DropUniqueConstraint(
                name: "AK_ResourceTags_Tag",
                table: "ResourceTags");

            _ = migrationBuilder.DropPrimaryKey(
                name: "PK_ResourceTags",
                table: "ResourceTags");

            _ = migrationBuilder.DropPrimaryKey(
                name: "PK_ResourceTagRelations",
                table: "ResourceTagRelations");

            _ = migrationBuilder.DropIndex(
                name: "IX_ResourceTagRelations_DependentResourceTagValue",
                table: "ResourceTagRelations");

            _ = migrationBuilder.DropColumn(
                name: "DependentResourceTagValue",
                table: "ResourceTagRelations");

            _ = migrationBuilder.RenameTable(
                name: "ResourceTags",
                newName: "PermissionTags");

            _ = migrationBuilder.RenameTable(
                name: "ResourceTagRelations",
                newName: "ResourceTagRelation");

            _ = migrationBuilder.RenameIndex(
                name: "IX_ResourceTagRelations_ResourceTagValue",
                table: "ResourceTagRelation",
                newName: "IX_ResourceTagRelation_ResourceTagValue");

            _ = migrationBuilder.AlterColumn<Guid>(
                name: "ResourceTagId",
                table: "Permissions",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            _ = migrationBuilder.AddColumn<Guid>(
                name: "PermissionTagId",
                table: "Permissions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            _ = migrationBuilder.AddUniqueConstraint(
                name: "AK_PermissionTags_Tag",
                table: "PermissionTags",
                column: "Tag");

            _ = migrationBuilder.AddPrimaryKey(
                name: "PK_PermissionTags",
                table: "PermissionTags",
                column: "Id");

            _ = migrationBuilder.AddPrimaryKey(
                name: "PK_ResourceTagRelation",
                table: "ResourceTagRelation",
                column: "Id");

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
                name: "IX_Permissions_PermissionTagId_RoleId",
                table: "Permissions",
                columns: new[] { "PermissionTagId", "RoleId" },
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_Permissions_ResourceTagId",
                table: "Permissions",
                column: "ResourceTagId");

            _ = migrationBuilder.AddForeignKey(
                name: "FK_Permissions_PermissionTags_ResourceTagId",
                table: "Permissions",
                column: "ResourceTagId",
                principalTable: "PermissionTags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            _ = migrationBuilder.AddForeignKey(
                name: "FK_ResourceTagRelation_PermissionTags_ResourceTagValue",
                table: "ResourceTagRelation",
                column: "ResourceTagValue",
                principalTable: "PermissionTags",
                principalColumn: "Tag",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
