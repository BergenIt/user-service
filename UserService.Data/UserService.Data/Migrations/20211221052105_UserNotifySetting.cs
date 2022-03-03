using System;

using Microsoft.EntityFrameworkCore.Migrations;

namespace UserService.Data.Migrations
{
    public partial class UserNotifySetting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropTable(
                name: "NotificationSettings");

            _ = migrationBuilder.CreateTable(
                name: "RoleNotificationSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SubdivisionId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    Enable = table.Column<bool>(type: "boolean", nullable: false),
                    ContractProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    TargetNotifies = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_RoleNotificationSettings", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_RoleNotificationSettings_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    _ = table.ForeignKey(
                        name: "FK_RoleNotificationSettings_ContractProfiles_ContractProfileId",
                        column: x => x.ContractProfileId,
                        principalTable: "ContractProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    _ = table.ForeignKey(
                        name: "FK_RoleNotificationSettings_Subdivisions_SubdivisionId",
                        column: x => x.SubdivisionId,
                        principalTable: "Subdivisions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "UserNotificationSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Enable = table.Column<bool>(type: "boolean", nullable: false),
                    ContractProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    TargetNotifies = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_UserNotificationSettings", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_UserNotificationSettings_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    _ = table.ForeignKey(
                        name: "FK_UserNotificationSettings_ContractProfiles_ContractProfileId",
                        column: x => x.ContractProfileId,
                        principalTable: "ContractProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            _ = migrationBuilder.CreateIndex(
                name: "IX_RoleNotificationSettings_ContractProfileId",
                table: "RoleNotificationSettings",
                column: "ContractProfileId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_RoleNotificationSettings_RoleId",
                table: "RoleNotificationSettings",
                column: "RoleId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_RoleNotificationSettings_SubdivisionId",
                table: "RoleNotificationSettings",
                column: "SubdivisionId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_UserNotificationSettings_ContractProfileId",
                table: "UserNotificationSettings",
                column: "ContractProfileId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_UserNotificationSettings_UserId",
                table: "UserNotificationSettings",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropTable(
                name: "RoleNotificationSettings");

            _ = migrationBuilder.DropTable(
                name: "UserNotificationSettings");

            _ = migrationBuilder.CreateTable(
                name: "NotificationSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ContractProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    Enable = table.Column<bool>(type: "boolean", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    SubdivisionId = table.Column<Guid>(type: "uuid", nullable: false),
                    TargetNotifies = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_NotificationSettings", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_NotificationSettings_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    _ = table.ForeignKey(
                        name: "FK_NotificationSettings_ContractProfiles_ContractProfileId",
                        column: x => x.ContractProfileId,
                        principalTable: "ContractProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    _ = table.ForeignKey(
                        name: "FK_NotificationSettings_Subdivisions_SubdivisionId",
                        column: x => x.SubdivisionId,
                        principalTable: "Subdivisions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"),
                column: "ConcurrencyStamp",
                value: "8f112af6-5add-44c9-921b-96daa3729c72");

            _ = migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "56ddbcde-40cd-4684-a394-e3b11198d9a0", "MDAwMTA0MjE3MDk4MTA0MDc5MjI2MDY2MDAwMTYwMjM0MjU0MTE1MDI0MTk5MTk0MDAwMTEyMTQ5MTcwMjA3MDA0MjM2MTk0MDAwMjE2MjA1MjI1MDAwMDk5MjQ5MTk0MDAwMDM3MDgxMTg2MDgwMjU0MDAxMTk1MDAwMDAwMDEwMDcwMDcxMTQxMDA3MTk1MDAwMTg1MDc1MDIwMTg1MDYxMDEzMTk1MTI4MDAzMDk1MjMyMjM3MDY4MDE3MTk1MDAwMDQ1MDc3MTc5MDk1MDc5MDIwMTk1LTE5Q0VBQURGOTY1NzRBQjU4MkYwQjQ4MDExOUU3RDAy", "9c8fb487-f664-4e75-b68a-f6e1d6e6f506" });

            _ = migrationBuilder.CreateIndex(
                name: "IX_NotificationSettings_ContractProfileId",
                table: "NotificationSettings",
                column: "ContractProfileId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_NotificationSettings_RoleId",
                table: "NotificationSettings",
                column: "RoleId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_NotificationSettings_SubdivisionId",
                table: "NotificationSettings",
                column: "SubdivisionId");
        }
    }
}
