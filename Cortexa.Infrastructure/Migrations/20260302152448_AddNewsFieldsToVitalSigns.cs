using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cortexa.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNewsFieldsToVitalSigns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ConsciousnessLevel",
                table: "VitalSigns",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NewsRiskLevel",
                table: "VitalSigns",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NewsScore",
                table: "VitalSigns",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "SupplementalOxygen",
                table: "VitalSigns",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConsciousnessLevel",
                table: "VitalSigns");

            migrationBuilder.DropColumn(
                name: "NewsRiskLevel",
                table: "VitalSigns");

            migrationBuilder.DropColumn(
                name: "NewsScore",
                table: "VitalSigns");

            migrationBuilder.DropColumn(
                name: "SupplementalOxygen",
                table: "VitalSigns");
        }
    }
}
