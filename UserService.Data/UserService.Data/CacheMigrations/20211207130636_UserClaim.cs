using System;

using Microsoft.EntityFrameworkCore.Migrations;

namespace UserService.Data.CacheMigrations
{
    public partial class UserClaim : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropIndex(
                name: "IX_AspNetRoleClaims_RoleId_ClaimValue_ClaimType",
                table: "AspNetRoleClaims");

            _ = migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
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

            _ = migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId_ClaimType",
                table: "AspNetRoleClaims",
                columns: new[] { "RoleId", "ClaimType" },
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId_ClaimValue_ClaimType",
                table: "AspNetUserClaims",
                columns: new[] { "UserId", "ClaimValue", "ClaimType" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            _ = migrationBuilder.DropIndex(
                name: "IX_AspNetRoleClaims_RoleId_ClaimType",
                table: "AspNetRoleClaims");

            _ = migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId_ClaimValue_ClaimType",
                table: "AspNetRoleClaims",
                columns: new[] { "RoleId", "ClaimValue", "ClaimType" },
                unique: true);
        }
    }
}
