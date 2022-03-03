using System;

using Microsoft.EntityFrameworkCore.Migrations;

namespace UserService.Data.CacheMigrations
{
    public partial class InitCache : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            _ = migrationBuilder.CreateTable(
                name: "NotifyEventTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Type = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_NotifyEventTypes", x => x.Id);
                    _ = table.UniqueConstraint("AK_NotifyEventTypes_Type", x => x.Type);
                });

            _ = migrationBuilder.CreateTable(
                name: "Subdivisions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_Subdivisions", x => x.Id);
                });

            _ = migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    RoleId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "ContractProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    NotifyEventType = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_ContractProfiles", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_ContractProfiles_NotifyEventTypes_NotifyEventType",
                        column: x => x.NotifyEventType,
                        principalTable: "NotifyEventTypes",
                        principalColumn: "Type",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ObjectId = table.Column<string>(type: "TEXT", nullable: true),
                    NotifyEventType = table.Column<string>(type: "TEXT", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    JsonData = table.Column<byte[]>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_Notifications", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_Notifications_NotifyEventTypes_NotifyEventType",
                        column: x => x.NotifyEventType,
                        principalTable: "NotifyEventTypes",
                        principalColumn: "Type",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserExpiration = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PasswordExpiration = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UserLock = table.Column<bool>(type: "INTEGER", nullable: false),
                    SubdivisionId = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_AspNetUsers_Subdivisions_SubdivisionId",
                        column: x => x.SubdivisionId,
                        principalTable: "Subdivisions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "ContractSettingLines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserProppertyName = table.Column<string>(type: "TEXT", nullable: true),
                    UserTemplate = table.Column<string>(type: "TEXT", nullable: true),
                    Enable = table.Column<bool>(type: "INTEGER", nullable: false),
                    LineNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    ContractProfileId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_ContractSettingLines", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_ContractSettingLines_ContractProfiles_ContractProfileId",
                        column: x => x.ContractProfileId,
                        principalTable: "ContractProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "NotificationSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Enable = table.Column<bool>(type: "INTEGER", nullable: false),
                    ContractProfileId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TargetNotifies = table.Column<string>(type: "TEXT", nullable: true),
                    SubdivisionId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RoleId = table.Column<Guid>(type: "TEXT", nullable: false)
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

            _ = migrationBuilder.CreateTable(
                name: "WebHooks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Url = table.Column<string>(type: "TEXT", nullable: true),
                    Enable = table.Column<bool>(type: "INTEGER", nullable: false),
                    WebHookContractType = table.Column<int>(type: "INTEGER", nullable: false),
                    ContractProfileId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_WebHooks", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_WebHooks_ContractProfiles_ContractProfileId",
                        column: x => x.ContractProfileId,
                        principalTable: "ContractProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RoleId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_AspNetUserRoles", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    _ = table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "UserNotifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    NotificationId = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_UserNotifications", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_UserNotifications_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    _ = table.ForeignKey(
                        name: "FK_UserNotifications_Notifications_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "ContractPropperties",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ContractName = table.Column<string>(type: "TEXT", nullable: true),
                    Position = table.Column<byte>(type: "INTEGER", nullable: false),
                    ContractSettingLineId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_ContractPropperties", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_ContractPropperties_ContractSettingLines_ContractSettingLineId",
                        column: x => x.ContractSettingLineId,
                        principalTable: "ContractSettingLines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.InsertData(
                table: "AspNetRoles",
                column: "Id",
                value: new Guid("00000001-0000-0000-0000-000000000000"));

            _ = migrationBuilder.InsertData(
                table: "Subdivisions",
                column: "Id",
                value: new Guid("00000001-0000-0000-0000-000000000000"));

            _ = migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "Email", "PasswordExpiration", "SubdivisionId", "UserExpiration", "UserLock", "UserName" },
                values: new object[] { new Guid("00000001-0000-0000-0000-000000000000"), "developer@bergen.tech", new DateTime(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Unspecified).AddTicks(9999), new Guid("00000001-0000-0000-0000-000000000000"), new DateTime(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Unspecified).AddTicks(9999), false, "developer" });

            _ = migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "Id", "RoleId", "UserId" },
                values: new object[] { new Guid("00000001-0000-0000-0000-000000000000"), new Guid("00000001-0000-0000-0000-000000000000"), new Guid("00000001-0000-0000-0000-000000000000") });

            _ = migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId_ClaimValue_ClaimType",
                table: "AspNetRoleClaims",
                columns: new[] { "RoleId", "ClaimValue", "ClaimType" },
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_UserId",
                table: "AspNetUserRoles",
                column: "UserId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Email",
                table: "AspNetUsers",
                column: "Email",
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_SubdivisionId",
                table: "AspNetUsers",
                column: "SubdivisionId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UserName",
                table: "AspNetUsers",
                column: "UserName",
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_ContractProfiles_NotifyEventType",
                table: "ContractProfiles",
                column: "NotifyEventType");

            _ = migrationBuilder.CreateIndex(
                name: "IX_ContractPropperties_ContractSettingLineId",
                table: "ContractPropperties",
                column: "ContractSettingLineId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_ContractSettingLines_ContractProfileId",
                table: "ContractSettingLines",
                column: "ContractProfileId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_Notifications_NotifyEventType",
                table: "Notifications",
                column: "NotifyEventType");

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

            _ = migrationBuilder.CreateIndex(
                name: "IX_NotifyEventTypes_Type",
                table: "NotifyEventTypes",
                column: "Type",
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_UserNotifications_NotificationId",
                table: "UserNotifications",
                column: "NotificationId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_UserNotifications_UserId",
                table: "UserNotifications",
                column: "UserId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_WebHooks_ContractProfileId",
                table: "WebHooks",
                column: "ContractProfileId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            _ = migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            _ = migrationBuilder.DropTable(
                name: "ContractPropperties");

            _ = migrationBuilder.DropTable(
                name: "NotificationSettings");

            _ = migrationBuilder.DropTable(
                name: "UserNotifications");

            _ = migrationBuilder.DropTable(
                name: "WebHooks");

            _ = migrationBuilder.DropTable(
                name: "ContractSettingLines");

            _ = migrationBuilder.DropTable(
                name: "AspNetRoles");

            _ = migrationBuilder.DropTable(
                name: "AspNetUsers");

            _ = migrationBuilder.DropTable(
                name: "Notifications");

            _ = migrationBuilder.DropTable(
                name: "ContractProfiles");

            _ = migrationBuilder.DropTable(
                name: "Subdivisions");

            _ = migrationBuilder.DropTable(
                name: "NotifyEventTypes");
        }
    }
}
