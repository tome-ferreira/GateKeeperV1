using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GateKeeperV1.Migrations
{
    /// <inheritdoc />
    public partial class aaaaaaaaaa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDateTime",
                table: "ShiftDays");

            migrationBuilder.RenameColumn(
                name: "StartDateTime",
                table: "ShiftDays",
                newName: "Date");

            migrationBuilder.AddColumn<bool>(
                name: "isOvernight",
                table: "ShiftDays",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ShiftDaysOfWeeks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DayOfWeek = table.Column<int>(type: "int", nullable: false),
                    isOvernight = table.Column<bool>(type: "bit", nullable: false),
                    ShiftId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShiftDaysOfWeeks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShiftDaysOfWeeks_Shifts_ShiftId",
                        column: x => x.ShiftId,
                        principalTable: "Shifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShiftDaysOfWeeks_ShiftId_DayOfWeek",
                table: "ShiftDaysOfWeeks",
                columns: new[] { "ShiftId", "DayOfWeek" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShiftDaysOfWeeks");

            migrationBuilder.DropColumn(
                name: "isOvernight",
                table: "ShiftDays");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "ShiftDays",
                newName: "StartDateTime");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDateTime",
                table: "ShiftDays",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
