using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GateKeeperV1.Migrations
{
    /// <inheritdoc />
    public partial class aaaaaaaaaaaaa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuildingsN",
                table: "EnterpirseRequests");

            migrationBuilder.DropColumn(
                name: "BuildingsNUnlimited",
                table: "EnterpirseRequests");

            migrationBuilder.DropColumn(
                name: "DashboardAccounts",
                table: "EnterpirseRequests");

            migrationBuilder.DropColumn(
                name: "DashboardAccountsUnlimited",
                table: "EnterpirseRequests");

            migrationBuilder.DropColumn(
                name: "HasExcel",
                table: "EnterpirseRequests");

            migrationBuilder.DropColumn(
                name: "RegistsPerMonth",
                table: "EnterpirseRequests");

            migrationBuilder.DropColumn(
                name: "RegistsPerMonthUnlimited",
                table: "EnterpirseRequests");

            migrationBuilder.DropColumn(
                name: "Workers",
                table: "EnterpirseRequests");

            migrationBuilder.DropColumn(
                name: "WorkersUnlimited",
                table: "EnterpirseRequests");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BuildingsN",
                table: "EnterpirseRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "BuildingsNUnlimited",
                table: "EnterpirseRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "DashboardAccounts",
                table: "EnterpirseRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "DashboardAccountsUnlimited",
                table: "EnterpirseRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasExcel",
                table: "EnterpirseRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "RegistsPerMonth",
                table: "EnterpirseRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "RegistsPerMonthUnlimited",
                table: "EnterpirseRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Workers",
                table: "EnterpirseRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "WorkersUnlimited",
                table: "EnterpirseRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
