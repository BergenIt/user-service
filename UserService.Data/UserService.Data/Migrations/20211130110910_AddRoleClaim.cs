using System;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore.Migrations;

using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace UserService.Data.Migrations
{
    public partial class AddRoleClaim : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            _ = migrationBuilder.CreateTable(
                name: "NotifyEventTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_NotifyEventTypes", x => x.Id);
                    _ = table.UniqueConstraint("AK_NotifyEventTypes_Type", x => x.Type);
                });

            _ = migrationBuilder.CreateTable(
                name: "PermissionTags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Tag = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_PermissionTags", x => x.Id);
                });

            _ = migrationBuilder.CreateTable(
                name: "Positions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Comment = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_Positions", x => x.Id);
                });

            _ = migrationBuilder.CreateTable(
                name: "ServiceSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ServiceSettingAttribute = table.Column<int>(type: "integer", nullable: false),
                    ServiceSettingValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_ServiceSettings", x => x.Id);
                });

            _ = migrationBuilder.CreateTable(
                name: "Subdivisions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Comment = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_Subdivisions", x => x.Id);
                });

            _ = migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
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
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Comment = table.Column<string>(type: "text", nullable: true),
                    NotifyEventType = table.Column<string>(type: "text", nullable: true)
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
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ContourId = table.Column<string>(type: "text", nullable: true),
                    NotifyEventType = table.Column<string>(type: "text", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    JsonData = table.Column<IDictionary<string, string>>(type: "jsonb", nullable: true)
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
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PermissionAssert = table.Column<int>(type: "integer", nullable: false),
                    PermissionTagId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_Permissions", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_Permissions_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    _ = table.ForeignKey(
                        name: "FK_Permissions_PermissionTags_PermissionTagId",
                        column: x => x.PermissionTagId,
                        principalTable: "PermissionTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    LastLogin = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UserExpiration = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    PasswordExpiration = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UserLock = table.Column<bool>(type: "boolean", nullable: false),
                    PositionId = table.Column<Guid>(type: "uuid", nullable: true),
                    SubdivisionId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_AspNetUsers_Positions_PositionId",
                        column: x => x.PositionId,
                        principalTable: "Positions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserProppertyName = table.Column<string>(type: "text", nullable: true),
                    UserTemplate = table.Column<string>(type: "text", nullable: true),
                    Enable = table.Column<bool>(type: "boolean", nullable: false),
                    LineNumber = table.Column<int>(type: "integer", nullable: false),
                    ContractProfileId = table.Column<Guid>(type: "uuid", nullable: false)
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
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Enable = table.Column<bool>(type: "boolean", nullable: false),
                    ContractProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    TargetNotifies = table.Column<string>(type: "text", nullable: true),
                    SubdivisionId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false)
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
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Comment = table.Column<string>(type: "text", nullable: true),
                    Url = table.Column<string>(type: "text", nullable: true),
                    Enable = table.Column<bool>(type: "boolean", nullable: false),
                    WebHookContractType = table.Column<int>(type: "integer", nullable: false),
                    ContractProfileId = table.Column<Guid>(type: "uuid", nullable: false)
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
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    _ = table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false)
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
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    _ = table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "Audits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    IpAddress = table.Column<string>(type: "text", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: true),
                    Action = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_Audits", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_Audits_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "UserNotifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    NotificationId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsRead = table.Column<bool>(type: "boolean", nullable: false)
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
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ContractName = table.Column<string>(type: "text", nullable: true),
                    Position = table.Column<byte>(type: "smallint", nullable: false),
                    ContractSettingLineId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_ContractPropperties", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_ContractPropperties_ContractSettingLines_ContractSettingLin~",
                        column: x => x.ContractSettingLineId,
                        principalTable: "ContractSettingLines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "Comment", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("00000001-0000-0000-0000-000000000000"), null, "f5c83589-6314-40cc-8f73-80eae74d33a8", "Admin", "ADMIN" });

            _ = migrationBuilder.InsertData(
                table: "ServiceSettings",
                columns: new[] { "Id", "ServiceSettingAttribute", "ServiceSettingValue" },
                values: new object[,]
                {
                    { new Guid("00000011-0000-0000-0000-000000000000"), 16, "" },
                    { new Guid("00000010-0000-0000-0000-000000000000"), 15, "" },
                    { new Guid("0000000f-0000-0000-0000-000000000000"), 14, "" },
                    { new Guid("0000000e-0000-0000-0000-000000000000"), 13, "" },
                    { new Guid("0000000d-0000-0000-0000-000000000000"), 12, "" },
                    { new Guid("0000000c-0000-0000-0000-000000000000"), 11, "" },
                    { new Guid("0000000b-0000-0000-0000-000000000000"), 10, "" },
                    { new Guid("0000000a-0000-0000-0000-000000000000"), 9, "" },
                    { new Guid("00000009-0000-0000-0000-000000000000"), 8, "" },
                    { new Guid("00000008-0000-0000-0000-000000000000"), 7, "" },
                    { new Guid("00000007-0000-0000-0000-000000000000"), 6, "" },
                    { new Guid("00000006-0000-0000-0000-000000000000"), 5, "" },
                    { new Guid("00000005-0000-0000-0000-000000000000"), 4, "" },
                    { new Guid("00000004-0000-0000-0000-000000000000"), 3, "" },
                    { new Guid("00000003-0000-0000-0000-000000000000"), 2, "" },
                    { new Guid("00000002-0000-0000-0000-000000000000"), 1, "" },
                    { new Guid("00000001-0000-0000-0000-000000000000"), 0, "" },
                    { new Guid("00000012-0000-0000-0000-000000000000"), 17, "" }
                });

            _ = migrationBuilder.InsertData(
                table: "Subdivisions",
                columns: new[] { "Id", "Comment", "Name" },
                values: new object[] { new Guid("00000001-0000-0000-0000-000000000000"), null, "Стандартное подразделение" });

            _ = migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Description", "Email", "EmailConfirmed", "FullName", "LastLogin", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordExpiration", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PositionId", "SecurityStamp", "SubdivisionId", "TwoFactorEnabled", "UserExpiration", "UserLock", "UserName" },
                values: new object[] { new Guid("00000001-0000-0000-0000-000000000000"), 0, "ed680f3d-caef-4a88-b581-279b4df93a8f", null, "developer@bergen.tech", false, null, null, false, null, "DEVELOPER@BERGEN.TECH", "DEVELOPER", new DateTime(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Unspecified).AddTicks(9999), "MDAwMTYwMTkwMDE2MTk1MjAzMjQwMDY2MDAwMDgwMDc3MTM1MDc5MjA0MjE3MDY2MDAwMTkyMTkxMTA0MjE4MDQ0MjA3MTk0MDAwMDg0MTE0MjI3MTkzMTIxMjM5MTk0MDAwMTU2MTU0MTE2MTAxMjMwMjQ3MTk0MDAwMjQ0MTk0MDgxMTM4MDMxMDAxMTk1MDAwMTQxMDE5MDM2MDQ1MDExMDA3MTk1MDAwMTkzMDY1MTYwMDYzMDMyMDExMTk1MDAwMTE2MjIwMjUyMDc1MTQxMDE1MTk1LTdDNjE2MzMwNjQyRDRCNkY4MjY0NUQ0NTI5MjZCNjc2", null, false, null, "3ddcaf81-b82c-4440-b91a-c228711aa400", new Guid("00000001-0000-0000-0000-000000000000"), false, new DateTime(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Unspecified).AddTicks(9999), false, "developer" });

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
                name: "IX_AspNetRoles_Name",
                table: "AspNetRoles",
                column: "Name",
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_UserId",
                table: "AspNetUserRoles",
                column: "UserId");

            _ = migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail",
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Email",
                table: "AspNetUsers",
                column: "Email",
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PositionId",
                table: "AspNetUsers",
                column: "PositionId");

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
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_Audits_UserId",
                table: "Audits",
                column: "UserId");

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
                name: "IX_Permissions_PermissionTagId_RoleId",
                table: "Permissions",
                columns: new[] { "PermissionTagId", "RoleId" },
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_Permissions_RoleId",
                table: "Permissions",
                column: "RoleId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_ServiceSettings_ServiceSettingAttribute",
                table: "ServiceSettings",
                column: "ServiceSettingAttribute",
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
                name: "AspNetUserClaims");

            _ = migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            _ = migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            _ = migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            _ = migrationBuilder.DropTable(
                name: "Audits");

            _ = migrationBuilder.DropTable(
                name: "ContractPropperties");

            _ = migrationBuilder.DropTable(
                name: "NotificationSettings");

            _ = migrationBuilder.DropTable(
                name: "Permissions");

            _ = migrationBuilder.DropTable(
                name: "ServiceSettings");

            _ = migrationBuilder.DropTable(
                name: "UserNotifications");

            _ = migrationBuilder.DropTable(
                name: "WebHooks");

            _ = migrationBuilder.DropTable(
                name: "ContractSettingLines");

            _ = migrationBuilder.DropTable(
                name: "AspNetRoles");

            _ = migrationBuilder.DropTable(
                name: "PermissionTags");

            _ = migrationBuilder.DropTable(
                name: "AspNetUsers");

            _ = migrationBuilder.DropTable(
                name: "Notifications");

            _ = migrationBuilder.DropTable(
                name: "ContractProfiles");

            _ = migrationBuilder.DropTable(
                name: "Positions");

            _ = migrationBuilder.DropTable(
                name: "Subdivisions");

            _ = migrationBuilder.DropTable(
                name: "NotifyEventTypes");
        }
    }
}
