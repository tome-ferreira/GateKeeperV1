using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GateKeeperV1.Migrations
{
    /// <inheritdoc />
    public partial class projecttocompany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComapnyWorkers_Projects_CompanyId",
                table: "ComapnyWorkers");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyAdmins_Projects_CompanyId",
                table: "CompanyAdmins");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyManagers_Projects_CompanyId",
                table: "CompanyManagers");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanySupervisors_Projects_CompanyId",
                table: "CompanySupervisors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Projects",
                table: "Projects");

            migrationBuilder.RenameTable(
                name: "Projects",
                newName: "Companies");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Companies",
                table: "Companies",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ComapnyWorkers_Companies_CompanyId",
                table: "ComapnyWorkers",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyAdmins_Companies_CompanyId",
                table: "CompanyAdmins",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyManagers_Companies_CompanyId",
                table: "CompanyManagers",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanySupervisors_Companies_CompanyId",
                table: "CompanySupervisors",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComapnyWorkers_Companies_CompanyId",
                table: "ComapnyWorkers");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyAdmins_Companies_CompanyId",
                table: "CompanyAdmins");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyManagers_Companies_CompanyId",
                table: "CompanyManagers");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanySupervisors_Companies_CompanyId",
                table: "CompanySupervisors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Companies",
                table: "Companies");

            migrationBuilder.RenameTable(
                name: "Companies",
                newName: "Projects");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Projects",
                table: "Projects",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ComapnyWorkers_Projects_CompanyId",
                table: "ComapnyWorkers",
                column: "CompanyId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyAdmins_Projects_CompanyId",
                table: "CompanyAdmins",
                column: "CompanyId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyManagers_Projects_CompanyId",
                table: "CompanyManagers",
                column: "CompanyId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanySupervisors_Projects_CompanyId",
                table: "CompanySupervisors",
                column: "CompanyId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
