using System;

using Microsoft.EntityFrameworkCore.Migrations;

namespace UserService.Data.CacheMigrations
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
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    SubdivisionId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RoleId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Enable = table.Column<bool>(type: "INTEGER", nullable: false),
                    ContractProfileId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TargetNotifies = table.Column<string>(type: "TEXT", nullable: true)
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
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Enable = table.Column<bool>(type: "INTEGER", nullable: false),
                    ContractProfileId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TargetNotifies = table.Column<string>(type: "TEXT", nullable: true)
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
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ContractProfileId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Enable = table.Column<bool>(type: "INTEGER", nullable: false),
                    RoleId = table.Column<Guid>(type: "TEXT", nullable: false),
                    SubdivisionId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TargetNotifies = table.Column<string>(type: "TEXT", nullable: true)
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
