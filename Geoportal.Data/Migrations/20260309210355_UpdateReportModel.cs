using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Geoportal.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateReportModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Reports",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Reports",
                newName: "Sector");

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "Reports",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "District",
                table: "Reports",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "InfraName",
                table: "Reports",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Region",
                table: "Reports",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "InfraObjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Region = table.Column<string>(type: "text", nullable: false),
                    District = table.Column<string>(type: "text", nullable: false),
                    Sector = table.Column<string>(type: "text", nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false),
                    AllocatedBudget = table.Column<decimal>(type: "numeric", nullable: false),
                    SpentBudget = table.Column<decimal>(type: "numeric", nullable: false),
                    DonorName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InfraObjects", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InfraObjects");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "District",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "InfraName",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "Region",
                table: "Reports");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Reports",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "Sector",
                table: "Reports",
                newName: "Description");
        }
    }
}
