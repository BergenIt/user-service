using System;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore.Migrations;

namespace UserService.Data.Migrations
{
    public partial class AddNotify : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.CreateTable(
                name: "Audit",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IpAddress = table.Column<string>(type: "text", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: true),
                    Action = table.Column<string>(type: "text", nullable: true),
                    FullName = table.Column<string>(type: "text", nullable: true),
                    UserName = table.Column<string>(type: "text", nullable: true),
                    Subdivision = table.Column<string>(type: "text", nullable: true),
                    Position = table.Column<string>(type: "text", nullable: true),
                    Roles = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_Audit", x => x.Id);
                });

            _ = migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ObjectId = table.Column<string>(type: "text", nullable: true),
                    NotifyEventType = table.Column<string>(type: "text", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    JsonData = table.Column<IDictionary<string, string>>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_Notification", x => x.Id);
                });

            _ = migrationBuilder.CreateTable(
                name: "UserNotification",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: true),
                    IsRead = table.Column<bool>(type: "boolean", nullable: false),
                    NotificationId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_UserNotification", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_UserNotification_Notification_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notification",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"),
                column: "ConcurrencyStamp",
                value: "fdaf1537-c8df-4be9-b7ae-8cf88cada3e0");

            _ = migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4a2e1c79-5499-4d90-9fbf-c044258ca004", "MDAwMTkyMDcxMTM4MjU0MDczMjA5MDY2MDAwMTI4MTY1MDIxMDQyMTgxMjA0MTk0MDAwMTg0MjQ4MTI5MTEwMjA1MjI3MTk0MDAwMTY4MjA3MjQwMTgzMTk1MjM1MTk0MDAwMTQ4MDIyMDI4MjE2MDcyMjQzMTk0MDAwMDY0MDk0MDI2MjM0MTUyMjQ5MTk0MDAwMTg0MjAxMjA5MDE0MTQ4MjUzMTk0MDAwMDE4MTI4MDQ2MTUxMTI1MDAxMTk1MDAwMTYyMDg2MjEwMDkxMjQ3MDAzMTk1LUI1NkY0M0ZENDZFRDQwMjg4Nzk1NUQzNjhEOEM3RjNC", "75e7097a-7ec8-4340-9859-bd32643f26b2" });

            _ = migrationBuilder.CreateIndex(
                name: "IX_UserNotification_NotificationId",
                table: "UserNotification",
                column: "NotificationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropTable(
                name: "Audit");

            _ = migrationBuilder.DropTable(
                name: "UserNotification");

            _ = migrationBuilder.DropTable(
                name: "Notification");

            _ = migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"),
                column: "ConcurrencyStamp",
                value: "f18389eb-4a8f-4e0a-ad22-176a49711d5d");

            _ = migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "90453dae-5452-454a-a9aa-8911cfe083c6", "MDAwMTI2MTk1MDc0MDY0MTUxMjQ0MDY2MDAwMTkyMjUzMjA0MDA5MDg2MTY1MDY2MDAwMTQwMTEwMTU0MTQyMTQ4MjM0MTk0MDAwMDUxMjE2MjAzMDQxMTAxMDAyMTk1MDAwMTkwMjUwMTY3MDczMDMwMDA5MTk1MDAwMjM4MTI5MDAyMDUzMDEyMDE2MTk1MDAwMTI5MTA5MTk5MDQyMjM1MDIxMTk1MTI4MDI5MDgxMTY5MTQyMDcxMDI1MTk1MTI4MDMzMTMyMTA5MDMzMDYxMDI5MTk1LUVDMEVDQUUzMzkxNDRGM0Q5MTM1NTEyNEUzMUFFN0RD", "5139162c-3f93-4268-89b4-8d6293755897" });
        }
    }
}
