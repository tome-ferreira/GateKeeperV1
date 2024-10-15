using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GateKeeperV1.Migrations
{
    /// <inheritdoc />
    public partial class Modelsforfirstdiagram : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Buildings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buildings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Buildings_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EnterpirseRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Salt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BuildingsN = table.Column<int>(type: "int", nullable: false),
                    BuildingsNUnlimited = table.Column<bool>(type: "bit", nullable: false),
                    RegistsPerMonth = table.Column<int>(type: "int", nullable: false),
                    RegistsPerMonthUnlimited = table.Column<bool>(type: "bit", nullable: false),
                    Workers = table.Column<int>(type: "int", nullable: false),
                    WorkersUnlimited = table.Column<bool>(type: "bit", nullable: false),
                    DashboardAccounts = table.Column<int>(type: "int", nullable: false),
                    DashboardAccountsUnlimited = table.Column<bool>(type: "bit", nullable: false),
                    HasExcel = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnterpirseRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnterpirseRequests_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkerProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InternalNumber = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkerProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkerProfiles_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkerProfiles_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Shifts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Starts = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Ends = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BuildingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shifts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shifts_Buildings_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Buildings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Shifts_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AbsenceJustifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AbsenceStarted = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AbsenceFinished = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Justification = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbsenceJustifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbsenceJustifications_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AbsenceJustifications_WorkerProfiles_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "WorkerProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OffdayVacationRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Starts = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Ends = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OffdayVacationRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OffdayVacationRequests_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OffdayVacationRequests_WorkerProfiles_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "WorkerProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Movements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShiftId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BuildingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Movements_Buildings_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Buildings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Movements_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Movements_Shifts_ShiftId",
                        column: x => x.ShiftId,
                        principalTable: "Shifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Movements_WorkerProfiles_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "WorkerProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkerShifts",
                columns: table => new
                {
                    WorkerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShiftId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkerShifts", x => new { x.WorkerId, x.ShiftId });
                    table.ForeignKey(
                        name: "FK_WorkerShifts_Shifts_ShiftId",
                        column: x => x.ShiftId,
                        principalTable: "Shifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkerShifts_WorkerProfiles_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "WorkerProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "JustificationDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AbsenceJustificationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UploadDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JustificationDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JustificationDocuments_AbsenceJustifications_AbsenceJustificationId",
                        column: x => x.AbsenceJustificationId,
                        principalTable: "AbsenceJustifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AbsenceJustifications_CompanyId",
                table: "AbsenceJustifications",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_AbsenceJustifications_WorkerId",
                table: "AbsenceJustifications",
                column: "WorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_CompanyId",
                table: "Buildings",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_EnterpirseRequests_UserId",
                table: "EnterpirseRequests",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_JustificationDocuments_AbsenceJustificationId",
                table: "JustificationDocuments",
                column: "AbsenceJustificationId");

            migrationBuilder.CreateIndex(
                name: "IX_Movements_BuildingId",
                table: "Movements",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_Movements_CompanyId",
                table: "Movements",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Movements_ShiftId",
                table: "Movements",
                column: "ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_Movements_WorkerId",
                table: "Movements",
                column: "WorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_OffdayVacationRequests_CompanyId",
                table: "OffdayVacationRequests",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_OffdayVacationRequests_WorkerId",
                table: "OffdayVacationRequests",
                column: "WorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_Shifts_BuildingId",
                table: "Shifts",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_Shifts_CompanyId",
                table: "Shifts",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerProfiles_ApplicationUserId",
                table: "WorkerProfiles",
                column: "ApplicationUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkerProfiles_CompanyId",
                table: "WorkerProfiles",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerShifts_ShiftId",
                table: "WorkerShifts",
                column: "ShiftId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EnterpirseRequests");

            migrationBuilder.DropTable(
                name: "JustificationDocuments");

            migrationBuilder.DropTable(
                name: "Movements");

            migrationBuilder.DropTable(
                name: "OffdayVacationRequests");

            migrationBuilder.DropTable(
                name: "WorkerShifts");

            migrationBuilder.DropTable(
                name: "AbsenceJustifications");

            migrationBuilder.DropTable(
                name: "Shifts");

            migrationBuilder.DropTable(
                name: "WorkerProfiles");

            migrationBuilder.DropTable(
                name: "Buildings");
        }
    }
}
