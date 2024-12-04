using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GateKeeperV1.Migrations
{
    /// <inheritdoc />
    public partial class updateshiftandshiftDayssimdenovo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DayOfWeek",
                table: "ShiftDays",
                newName: "StartDateTime");

            migrationBuilder.AddColumn<bool>(
                name: "IsOvernight",
                table: "Shifts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDateTime",
                table: "ShiftDays",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOvernight",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "EndDateTime",
                table: "ShiftDays");

            migrationBuilder.RenameColumn(
                name: "StartDateTime",
                table: "ShiftDays",
                newName: "DayOfWeek");
        }
    }
}
