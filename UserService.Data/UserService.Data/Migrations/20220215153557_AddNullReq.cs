using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UserService.Data.Migrations
{
    public partial class AddNullReq : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Subdivisions",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            _ = migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Positions",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            _ = migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Permissions",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            _ = migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "AspNetUsers",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256,
                oldNullable: true);

            _ = migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetRoles",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256,
                oldNullable: true);

            _ = migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"),
                column: "ConcurrencyStamp",
                value: "3c7a163b-41b0-46f4-be47-b58106e379cc");

            _ = migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "3f6f55e2-c22d-4d5a-9fd5-c8d4089d09d9", "MDAwMDAwMDU5MDQzMTYyMDA5MjA5MDY2MDAwMDg4MDUwMDgyMTkyMDIyMjAyMTk0MDAwMTY2MTM0MDAxMDkxMjMyMjI0MTk0MDAwMDk0MjI5MTA5MTczMDIyMjM2MTk0MDAwMjQ1MDUwMjM4MTYwMTYxMjQ1MTk0MDAwMDExMDAxMTE0MDkyMjExMjUwMTk0MTI4MDUxMDI0MjEyMDY2MDUzMDAwMTk1MTI4MDg2MTg0MjM5MTAzMDAwMDA0MTk1MDAwMTQwMjQ2MDkyMTYyMjExMDA4MTk1LTY1NzE4QUNFN0NEMzRCMEVBRTMxRUYwNEEzQkI1Qjc0", "013365d4-62cf-486b-8be3-e68ac9d0c464" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Subdivisions",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            _ = migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Positions",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            _ = migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Permissions",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            _ = migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "AspNetUsers",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256);

            _ = migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetRoles",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256);

            _ = migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"),
                column: "ConcurrencyStamp",
                value: "1e8fbbaf-5af2-4e35-b719-82814d12de0c");

            _ = migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "79c56f16-e733-49e7-8148-3e4405f4ecd9", "MDAwMjI4MTk1MTgyMDMyMTIxMjMwMDY2MDAwMDY0MDUxMjUzMDI1MDUwMTY2MTk0MDAwMTU2MDY0MjUzMDA1MjI0MjI2MTk0MDAwMTE2MjEzMjQ5MTgwMDE1MjQ2MTk0MDAwMTU0MDMzMDEzMjQ1MTk2MDAzMTk1MDAwMTY2MTAzMjIzMjQ0MDM1MDA4MTk1MDAwMDU3MDAyMjIxMjA1MTE1MDE0MTk1MTI4MDI5MTAzMTY4MDYwMTUzMDE5MTk1MTI4MTQxMDg5MDY2MTgwMTIzMDIzMTk1LUQyQ0I5NkI5NjI2NTQwODc5RkFBNjg2QTY1MkJCMjU5", "62a0b1ef-bb6b-4bf5-81d4-93fd7aa0a8ed" });
        }
    }
}
