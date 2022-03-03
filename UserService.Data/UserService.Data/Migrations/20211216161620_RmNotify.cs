using System;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore.Migrations;

namespace UserService.Data.Migrations
{
    public partial class RmNotify : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropTable(
                name: "UserNotifications");

            _ = migrationBuilder.DropTable(
                name: "Notifications");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    JsonData = table.Column<IDictionary<string, string>>(type: "jsonb", nullable: true),
                    NotifyEventType = table.Column<string>(type: "text", nullable: true),
                    ObjectId = table.Column<string>(type: "text", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
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
                name: "UserNotifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IsRead = table.Column<bool>(type: "boolean", nullable: false),
                    NotificationId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
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

            _ = migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"),
                column: "ConcurrencyStamp",
                value: "16ec68db-ddd2-4a1d-835a-33f8cfbdc2ae");

            _ = migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2eb4ac9e-f997-47f2-b5e4-9f4c2a124063", "MDAwMDQ4MjQxMDQ5MTI0MDIzMjE3MDY2MDAwMTkyMDkyMTcwMDA1MTYxMTY5MTk0MDAwMjE2MDE0MTMyMDQyMjM2MjEyMTk0MDAwMTM2MTYzMTYyMTU3MTUyMjMzMTk0MDAwMTE2MTE5MTMzMjQ1MTIwMjQxMTk0MDAwMTMyMTc0MDA5MTA4MjMxMjQ1MTk0MDAwMDYwMDY3MTY4MTI2MTIwMjUzMTk0MDAwMTA2MjA1MDkzMTMxMDE4MDAxMTk1MDAwMTUyMDU2MTQ4MTA4MTE2MDA0MTk1LTUwRkJERjg4QTg4NDQwRUNCMEI3Q0FBREY3OEY1RkIx", "ce8441c3-be76-4f26-9d5d-76cd2488344e" });

            _ = migrationBuilder.CreateIndex(
                name: "IX_Notifications_NotifyEventType",
                table: "Notifications",
                column: "NotifyEventType");

            _ = migrationBuilder.CreateIndex(
                name: "IX_UserNotifications_NotificationId",
                table: "UserNotifications",
                column: "NotificationId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_UserNotifications_UserId",
                table: "UserNotifications",
                column: "UserId");
        }
    }
}
