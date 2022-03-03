using System;

using Microsoft.EntityFrameworkCore.Migrations;

using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace UserService.Data.Migrations
{
    public partial class UserClaim : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserClaims",
                table: "AspNetUserClaims");

            _ = migrationBuilder.DropIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims");

            _ = migrationBuilder.DropIndex(
                name: "IX_AspNetRoleClaims_RoleId_ClaimValue_ClaimType",
                table: "AspNetRoleClaims");

            _ = migrationBuilder.DropColumn(
                name: "Id",
                table: "AspNetUserClaims");

            _ = migrationBuilder.AlterColumn<string>(
                name: "ClaimValue",
                table: "AspNetUserClaims",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            _ = migrationBuilder.AlterColumn<string>(
                name: "ClaimType",
                table: "AspNetUserClaims",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            _ = migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserClaims",
                table: "AspNetUserClaims",
                columns: new[] { "UserId", "ClaimValue", "ClaimType" });

            _ = migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"),
                column: "ConcurrencyStamp",
                value: "dc31e5b8-1c75-4a09-9078-ed470307cbe5");

            _ = migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "cca19c90-08e5-46e0-ade8-9e0a60c5326a", "MDAwMTc2MjEzMDQxMDE3MDAyMjIzMDY2MDAwMDk2MTU2MTY1MDAyMTU3MjAwMDY2MDAwMTkyMDY0MjM0MTIzMTQyMTg1MTk0MDAwMjQwMjQwMTg2MDQ5MDg0MjE4MTk0MDAwMDgwMjE3MTE5MTY0MTczMjM1MTk0MDAwMDY0MDY4MDc5MDkwMTMxMjQyMTk0MDAwMTEyMDkyMjU1MTI2MTI3MjQ3MTk0MDAwMjIwMTQwMjA0MDY4MTkzMjU0MTk0MDAwMjE2MjMyMTIwMTcyMDM3MDAyMTk1LTU5NDE0NkFGNzkyMjQ2NUFBM0QyMzY4RkZGRDk5MzQ1", "75dea6ac-0166-43f5-829c-507d6890279a" });

            _ = migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId_ClaimType",
                table: "AspNetRoleClaims",
                columns: new[] { "RoleId", "ClaimType" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserClaims",
                table: "AspNetUserClaims");

            _ = migrationBuilder.DropIndex(
                name: "IX_AspNetRoleClaims_RoleId_ClaimType",
                table: "AspNetRoleClaims");

            _ = migrationBuilder.AlterColumn<string>(
                name: "ClaimType",
                table: "AspNetUserClaims",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            _ = migrationBuilder.AlterColumn<string>(
                name: "ClaimValue",
                table: "AspNetUserClaims",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            _ = migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "AspNetUserClaims",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            _ = migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserClaims",
                table: "AspNetUserClaims",
                column: "Id");

            _ = migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"),
                column: "ConcurrencyStamp",
                value: "abddf57d-85a6-4176-82d9-69cb47f3d0de");

            _ = migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e93fdb49-bfee-43db-b5fe-b422fc6dcff1", "MDAwMTI4MDA0MTk1MDUyMjUyMjIxMDY2MDAwMjQwMDQ0MjUwMDY2MTM1MjIyMTk0MDAwMDc2MTUyMDUyMTc1MTUyMjQ3MTk0MDAwMTIyMTI0MDM0MTQ4MTcyMjU0MTk0MDAwMTE4MTg3MTI4MDc5MjAyMDA1MTk1MDAwMTg1MjMxMDAwMjQzMTk3MDEzMTk1MTI4MTc1MTQxMjQ2MjA3MTY3MDE2MTk1MDAwMDM0MTkxMDM1MTI2MjI1MDE5MTk1MDAwMDUzMjA5MjQxMTUyMjIyMDIzMTk1LUQ4MzhEMTc5NzhFRjRFRDU4RkQ3Njc1RkI1N0Q0MTM0", "bab36061-f4e7-4cb8-afb8-afd4d1e64309" });

            _ = migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId_ClaimValue_ClaimType",
                table: "AspNetRoleClaims",
                columns: new[] { "RoleId", "ClaimValue", "ClaimType" },
                unique: true);
        }
    }
}
