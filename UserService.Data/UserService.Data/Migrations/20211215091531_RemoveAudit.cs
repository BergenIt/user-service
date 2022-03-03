using System;

using Microsoft.EntityFrameworkCore.Migrations;

namespace UserService.Data.Migrations
{
    public partial class RemoveAudit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropTable(
                name: "Audits");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.CreateTable(
                name: "Audits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Action = table.Column<string>(type: "text", nullable: true),
                    IpAddress = table.Column<string>(type: "text", nullable: true),
                    Message = table.Column<string>(type: "text", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
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

            _ = migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"),
                column: "ConcurrencyStamp",
                value: "16542c0c-7066-4655-a2eb-8e2177fb8776");

            _ = migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "41ba7d9a-1bd5-4210-a7c8-942bdc8caeac", "MDAwMDc2MDIwMTU3MTMzMDkyMjQxMDY2MDAwMDAwMjQyMTc1MjA3MTk2MTgwMDY2MDAwMDcxMDQyMTEwMjI3MDE0MjQwMTk0MDAwMDAzMTMwMDQ3MjQwMDE0MDAwMTk1MDAwMDg1MTUxMjM3MTIxMTMzMDA3MTk1MDY0MDA0MDIyMDE3MTM3MDI1MDE2MTk1MTkyMDU0MDM3MjE1MjAzMDI5MDIwMTk1MTkyMDQzMTIzMTI4MTE0MjE3MDIzMTk1MDY0MjA5MDUyMDQyMTI1MjQ1MDI4MTk1LTg4NTVBNzJDRThFMTRGMjRBQTM0RDgyQjI2MDRFMDA3", "e04268c6-c92c-4323-a34a-1cd0b7460d8e" });

            _ = migrationBuilder.CreateIndex(
                name: "IX_Audits_UserId",
                table: "Audits",
                column: "UserId");
        }
    }
}
