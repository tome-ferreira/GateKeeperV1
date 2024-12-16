using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GateKeeperV1.Migrations
{
    /// <inheritdoc />
    public partial class addworkrerstemas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkersTeams",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkersTeams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkersTeams_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkerTeamMemberships",
                columns: table => new
                {
                    WorkerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkerTeamMemberships", x => new { x.WorkerId, x.TeamId });
                    table.ForeignKey(
                        name: "FK_WorkerTeamMemberships_WorkerProfiles_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "WorkerProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkerTeamMemberships_WorkersTeams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "WorkersTeams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkersTeams_CompanyId",
                table: "WorkersTeams",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerTeamMemberships_TeamId",
                table: "WorkerTeamMemberships",
                column: "TeamId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkerTeamMemberships");

            migrationBuilder.DropTable(
                name: "WorkersTeams");
        }
    }
}
