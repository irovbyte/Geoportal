using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Geoportal.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDashboardFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdditionalDataJson",
                table: "InfraObjects",
                type: "jsonb",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Capacity",
                table: "InfraObjects",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CompletionPercentage",
                table: "InfraObjects",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CurrentPeopleCount",
                table: "InfraObjects",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "HasDrinkingWater",
                table: "InfraObjects",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasInternet",
                table: "InfraObjects",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalDataJson",
                table: "InfraObjects");

            migrationBuilder.DropColumn(
                name: "Capacity",
                table: "InfraObjects");

            migrationBuilder.DropColumn(
                name: "CompletionPercentage",
                table: "InfraObjects");

            migrationBuilder.DropColumn(
                name: "CurrentPeopleCount",
                table: "InfraObjects");

            migrationBuilder.DropColumn(
                name: "HasDrinkingWater",
                table: "InfraObjects");

            migrationBuilder.DropColumn(
                name: "HasInternet",
                table: "InfraObjects");
        }
    }
}
