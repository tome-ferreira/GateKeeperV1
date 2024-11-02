using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GateKeeperV1.Migrations
{
    /// <inheritdoc />
    public partial class transferplantocompany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Plans_PlanId",
                table: "Companies");

            migrationBuilder.DropTable(
                name: "Plans");

            migrationBuilder.DropIndex(
                name: "IX_Companies_PlanId",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "PlanId",
                table: "Companies");

            migrationBuilder.AddColumn<decimal>(
                name: "AnualPrice",
                table: "Companies",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "BuildingsN",
                table: "Companies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DashboardAccounts",
                table: "Companies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "HasExcel",
                table: "Companies",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "MonthlyPrice",
                table: "Companies",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "RegistsPerMonth",
                table: "Companies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkersN",
                table: "Companies",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnualPrice",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "BuildingsN",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "DashboardAccounts",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "HasExcel",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "MonthlyPrice",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "RegistsPerMonth",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "WorkersN",
                table: "Companies");

            migrationBuilder.AddColumn<Guid>(
                name: "PlanId",
                table: "Companies",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Plans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnualPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    BuildingsN = table.Column<int>(type: "int", nullable: false),
                    CanBeDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DashboardAccounts = table.Column<int>(type: "int", nullable: false),
                    HasExcel = table.Column<bool>(type: "bit", nullable: false),
                    MonthlyPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegistsPerMonth = table.Column<int>(type: "int", nullable: false),
                    Workers = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plans", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Companies_PlanId",
                table: "Companies",
                column: "PlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Plans_PlanId",
                table: "Companies",
                column: "PlanId",
                principalTable: "Plans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
