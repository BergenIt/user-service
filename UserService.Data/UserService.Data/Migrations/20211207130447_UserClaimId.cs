using System;

using Microsoft.EntityFrameworkCore.Migrations;

namespace UserService.Data.Migrations
{
    public partial class UserClaimId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserClaims",
                table: "AspNetUserClaims");

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

            _ = migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "AspNetUserClaims",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            _ = migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserClaims",
                table: "AspNetUserClaims",
                column: "Id");

            _ = migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"),
                column: "ConcurrencyStamp",
                value: "7305300c-5769-4fc1-8b40-16d9663e413f");

            _ = migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f111549e-a28f-4f05-ae53-49194aba69f4", "MDAwMTQ0MTk3MDgxMDMwMTc1MjM2MDY2MDAwMDU2MDU5MDE3MjQ0MTQ0MjE3MTk0MDAwMTc2MTQ5MTA2MDMwMTk3MjQ2MTk0MDAwMDkxMTM2MjQxMDA2MDk5MDAxMTk1MDAwMDEzMDM3MTc3MTk2MDIzMDA4MTk1MDAwMTM2MTMzMDE2MjI5MDM1MDE2MTk1MDAwMDcwMDU3MjM3MDkxMDM2MDE5MTk1MDAwMjAzMTU0MjQyMDk4MTI2MDIyMTk1MDAwMDY1MDk1MTgyMTE3MTk2MDI1MTk1LTNFMTk4NjFFRTNBMTQ2OTlCQ0I4NTk4RTlCM0NGRDU3", "a691b299-8914-488a-b972-0cf96948fe17" });

            _ = migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId_ClaimValue_ClaimType",
                table: "AspNetUserClaims",
                columns: new[] { "UserId", "ClaimValue", "ClaimType" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserClaims",
                table: "AspNetUserClaims");

            _ = migrationBuilder.DropIndex(
                name: "IX_AspNetUserClaims_UserId_ClaimValue_ClaimType",
                table: "AspNetUserClaims");

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
        }
    }
}
