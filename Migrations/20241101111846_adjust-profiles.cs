using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GateKeeperV1.Migrations
{
    /// <inheritdoc />
    public partial class adjustprofiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkerProfiles_AspNetUsers_ApplicationUserId",
                table: "WorkerProfiles");

            migrationBuilder.DropTable(
                name: "ComapnyWorkers");

            migrationBuilder.DropTable(
                name: "CompanyAdmins");

            migrationBuilder.DropTable(
                name: "CompanyManagers");

            migrationBuilder.DropTable(
                name: "CompanySupervisors");

            migrationBuilder.DropIndex(
                name: "IX_WorkerProfiles_ApplicationUserId",
                table: "WorkerProfiles");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "WorkerProfiles",
                newName: "ApplicationUserEmail");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerProfiles_ApplicationUserEmail",
                table: "WorkerProfiles",
                column: "ApplicationUserEmail");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkerProfiles_AspNetUsers_ApplicationUserEmail",
                table: "WorkerProfiles",
                column: "ApplicationUserEmail",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkerProfiles_AspNetUsers_ApplicationUserEmail",
                table: "WorkerProfiles");

            migrationBuilder.DropIndex(
                name: "IX_WorkerProfiles_ApplicationUserEmail",
                table: "WorkerProfiles");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserEmail",
                table: "WorkerProfiles",
                newName: "ApplicationUserId");

            migrationBuilder.CreateTable(
                name: "ComapnyWorkers",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComapnyWorkers", x => new { x.UserId, x.CompanyId });
                    table.ForeignKey(
                        name: "FK_ComapnyWorkers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ComapnyWorkers_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CompanyAdmins",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyAdmins", x => new { x.UserId, x.CompanyId });
                    table.ForeignKey(
                        name: "FK_CompanyAdmins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompanyAdmins_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CompanyManagers",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyManagers", x => new { x.UserId, x.CompanyId });
                    table.ForeignKey(
                        name: "FK_CompanyManagers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompanyManagers_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CompanySupervisors",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanySupervisors", x => new { x.UserId, x.CompanyId });
                    table.ForeignKey(
                        name: "FK_CompanySupervisors_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompanySupervisors_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkerProfiles_ApplicationUserId",
                table: "WorkerProfiles",
                column: "ApplicationUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ComapnyWorkers_CompanyId",
                table: "ComapnyWorkers",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyAdmins_CompanyId",
                table: "CompanyAdmins",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyManagers_CompanyId",
                table: "CompanyManagers",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanySupervisors_CompanyId",
                table: "CompanySupervisors",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkerProfiles_AspNetUsers_ApplicationUserId",
                table: "WorkerProfiles",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
