using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GateKeeperV1.Migrations
{
    /// <inheritdoc />
    public partial class addshiftleader : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ShiftLeaderId",
                table: "Shifts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Shifts_ShiftLeaderId",
                table: "Shifts",
                column: "ShiftLeaderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shifts_WorkerProfiles_ShiftLeaderId",
                table: "Shifts",
                column: "ShiftLeaderId",
                principalTable: "WorkerProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shifts_WorkerProfiles_ShiftLeaderId",
                table: "Shifts");

            migrationBuilder.DropIndex(
                name: "IX_Shifts_ShiftLeaderId",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "ShiftLeaderId",
                table: "Shifts");
        }
    }
}
