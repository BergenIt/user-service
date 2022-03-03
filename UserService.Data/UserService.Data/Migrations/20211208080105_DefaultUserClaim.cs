using System;

using Microsoft.EntityFrameworkCore.Migrations;

namespace UserService.Data.Migrations
{
    public partial class DefaultUserClaim : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"),
                column: "ConcurrencyStamp",
                value: "16542c0c-7066-4655-a2eb-8e2177fb8776");

            _ = migrationBuilder.InsertData(
                table: "AspNetUserClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "UserId" },
                values: new object[] { new Guid("00000001-0000-0000-0000-000000000000"), "AccessObjectIds", "00000001-0000-0000-0000-000000000000", new Guid("00000001-0000-0000-0000-000000000000") });

            _ = migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "41ba7d9a-1bd5-4210-a7c8-942bdc8caeac", "MDAwMDc2MDIwMTU3MTMzMDkyMjQxMDY2MDAwMDAwMjQyMTc1MjA3MTk2MTgwMDY2MDAwMDcxMDQyMTEwMjI3MDE0MjQwMTk0MDAwMDAzMTMwMDQ3MjQwMDE0MDAwMTk1MDAwMDg1MTUxMjM3MTIxMTMzMDA3MTk1MDY0MDA0MDIyMDE3MTM3MDI1MDE2MTk1MTkyMDU0MDM3MjE1MjAzMDI5MDIwMTk1MTkyMDQzMTIzMTI4MTE0MjE3MDIzMTk1MDY0MjA5MDUyMDQyMTI1MjQ1MDI4MTk1LTg4NTVBNzJDRThFMTRGMjRBQTM0RDgyQjI2MDRFMDA3", "e04268c6-c92c-4323-a34a-1cd0b7460d8e" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DeleteData(
                table: "AspNetUserClaims",
                keyColumn: "Id",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"));

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
        }
    }
}
