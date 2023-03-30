using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace portfolio_api.Migrations
{
    /// <inheritdoc />
    public partial class initialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuthServerUsers",
                columns: table => new
                {
                    AuthServerUserId = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Joined = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthServerUsers", x => x.AuthServerUserId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    roleId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    roleName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.roleId);
                });

            migrationBuilder.CreateTable(
                name: "Households",
                columns: table => new
                {
                    HouseholdId = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    ownerId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Households", x => x.HouseholdId);
                    table.ForeignKey(
                        name: "FK_Households_AuthServerUsers_ownerId",
                        column: x => x.ownerId,
                        principalTable: "AuthServerUsers",
                        principalColumn: "AuthServerUserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HouseholdUsers",
                columns: table => new
                {
                    AuthServerUserId = table.Column<string>(type: "text", nullable: false),
                    HouseholdId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<int>(type: "integer", nullable: false),
                    MemberSince = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HouseholdUsers", x => new { x.HouseholdId, x.AuthServerUserId });
                    table.ForeignKey(
                        name: "FK_HouseholdUsers_AuthServerUsers_AuthServerUserId",
                        column: x => x.AuthServerUserId,
                        principalTable: "AuthServerUsers",
                        principalColumn: "AuthServerUserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HouseholdUsers_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "Households",
                        principalColumn: "HouseholdId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HouseholdUsers_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "roleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AuthServerUsers",
                columns: new[] { "AuthServerUserId", "Email", "Joined" },
                values: new object[,]
                {
                    { "auth0|6416fc4ce1118da83ff07523", "marek2.puurunen@gmail.com", new DateTime(2023, 3, 28, 10, 1, 5, 578, DateTimeKind.Utc).AddTicks(532) },
                    { "google-oauth2|117233659145082710607", "marek.puurunen@gmail.com", new DateTime(2023, 3, 28, 10, 1, 5, 578, DateTimeKind.Utc).AddTicks(528) }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "roleId", "roleName" },
                values: new object[,]
                {
                    { 1, "Owner" },
                    { 2, "Admin" },
                    { 3, "User" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Households_ownerId",
                table: "Households",
                column: "ownerId");

            migrationBuilder.CreateIndex(
                name: "IX_HouseholdUsers_AuthServerUserId",
                table: "HouseholdUsers",
                column: "AuthServerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_HouseholdUsers_RoleId",
                table: "HouseholdUsers",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HouseholdUsers");

            migrationBuilder.DropTable(
                name: "Households");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "AuthServerUsers");
        }
    }
}
