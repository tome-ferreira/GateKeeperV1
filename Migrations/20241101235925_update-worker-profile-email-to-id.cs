using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GateKeeperV1.Migrations
{
    /// <inheritdoc />
    public partial class updateworkerprofileemailtoid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkerProfiles_AspNetUsers_ApplicationUserEmail",
                table: "WorkerProfiles");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserEmail",
                table: "WorkerProfiles",
                newName: "ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkerProfiles_ApplicationUserEmail",
                table: "WorkerProfiles",
                newName: "IX_WorkerProfiles_ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkerProfiles_AspNetUsers_ApplicationUserId",
                table: "WorkerProfiles",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkerProfiles_AspNetUsers_ApplicationUserId",
                table: "WorkerProfiles");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "WorkerProfiles",
                newName: "ApplicationUserEmail");

            migrationBuilder.RenameIndex(
                name: "IX_WorkerProfiles_ApplicationUserId",
                table: "WorkerProfiles",
                newName: "IX_WorkerProfiles_ApplicationUserEmail");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkerProfiles_AspNetUsers_ApplicationUserEmail",
                table: "WorkerProfiles",
                column: "ApplicationUserEmail",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
