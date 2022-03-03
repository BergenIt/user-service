using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UserService.Data.Migrations
{
    public partial class AddDateTimeOffset : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "Timestamp",
                table: "Notification",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            _ = migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "Timestamp",
                table: "Audit",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            _ = migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UserExpiration",
                table: "AspNetUsers",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            _ = migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "RegistredDate",
                table: "AspNetUsers",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            _ = migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "PasswordExpiration",
                table: "AspNetUsers",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            _ = migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "LockoutEnd",
                table: "AspNetUsers",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone");

            _ = migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "LastLogin",
                table: "AspNetUsers",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            _ = migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"),
                column: "ConcurrencyStamp",
                value: "6cca466d-a218-4905-940f-89fc6c5707e8");

            _ = migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"),
                columns: new[] { "ConcurrencyStamp", "LockoutEnd", "PasswordExpiration", "PasswordHash", "RegistredDate", "SecurityStamp", "UserExpiration" },
                values: new object[] { "80a6a09e-81bb-4b2a-a22d-e55406dbe546", null, new DateTimeOffset(new DateTime(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Unspecified).AddTicks(9999), new TimeSpan(0, 3, 0, 0, 0)), "MDAwMjQwMTY1MjExMTY2MjI1MjEwMDY2MDAwMTI4MjUxMTQ5MTgwMDg2MTYwMDY2MDAwMjI0MDQyMDMyMTMwMTk1MjE0MTk0MDAwMjQwMTc2MDQ1MTg0MDY5MjI0MTk0MDAwMDQwMTQ5MDEwMTI2MTU5MjM0MTk0MDAwMjQ0MTY0MDE1MjA3MTMxMjQzMTk0MDAwMDkyMDIyMTk3MDQyMjQ2MjQ1MTk0MDAwMjUyMjI2MDI0MjUxMDM1MjUxMTk0MDAwMTU0MTQ3MDE1MTQwMDMzMDAwMTk1LUJBQUM3QTk0NUFDRDRFN0RCNUNFQTU3MkEwNUU5MkEx", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "dfd06f95-0fee-4c67-89f6-8ece5e74ead3", new DateTimeOffset(new DateTime(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Unspecified).AddTicks(9999), new TimeSpan(0, 3, 0, 0, 0)) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AlterColumn<DateTime>(
                name: "Timestamp",
                table: "Notification",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone");

            _ = migrationBuilder.AlterColumn<DateTime>(
                name: "Timestamp",
                table: "Audit",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone");

            _ = migrationBuilder.AlterColumn<DateTime>(
                name: "UserExpiration",
                table: "AspNetUsers",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone");

            _ = migrationBuilder.AlterColumn<DateTime>(
                name: "RegistredDate",
                table: "AspNetUsers",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone");

            _ = migrationBuilder.AlterColumn<DateTime>(
                name: "PasswordExpiration",
                table: "AspNetUsers",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone");

            _ = migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "LockoutEnd",
                table: "AspNetUsers",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true);

            _ = migrationBuilder.AlterColumn<DateTime>(
                name: "LastLogin",
                table: "AspNetUsers",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true);

            _ = migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"),
                column: "ConcurrencyStamp",
                value: "9ded1cb3-85ee-4268-b697-52af6e786c8a");

            _ = migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"),
                columns: new[] { "ConcurrencyStamp", "LockoutEnd", "PasswordExpiration", "PasswordHash", "RegistredDate", "SecurityStamp", "UserExpiration" },
                values: new object[] { "271a119e-7a0b-4785-a604-998f616a9661", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTime(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Unspecified).AddTicks(9999), "MDAwMTI4MTA0MDA1MDY4MDI5MjQ1MDY2MDAwMTYwMDI2MTE2MDk1MDA3MTc2MDY2MDAwMDk5MTI0MTU3MjA1MjQ4MjQxMTk0MDAwMDI5MTY0MTU2MjI3MDc2MDAyMTk1MDAwMjExMDQ3MjM1MDYxMDYzMDA3MTk1MTkyMDY2MjI5MDk4MjQ3MDkyMDE2MTk1MTI4MTU5MTcyMjIwMTc1MDA1MDIxMTk1MTI4MDcyMDY4MTU1MTg4MTI2MDIzMTk1MDAwMTY5MTc1MjQ4MDgzMDA3MDI4MTk1LTlBOUNFQzhGQUI0MzQ5RDRCMEE1RjFFMENEMDdBMEI5", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "3ce92689-db2b-4ddc-96ee-c16e588ce464", new DateTime(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Unspecified).AddTicks(9999) });
        }
    }
}
