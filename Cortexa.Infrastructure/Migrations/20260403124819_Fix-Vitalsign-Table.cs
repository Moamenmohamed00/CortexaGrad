using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cortexa.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixVitalsignTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CVP",
                table: "VitalSigns",
                newName: "Cvp");

            migrationBuilder.RenameColumn(
                name: "GCS_Verbal",
                table: "VitalSigns",
                newName: "GcsVerbal");

            migrationBuilder.RenameColumn(
                name: "GCS_Motor",
                table: "VitalSigns",
                newName: "GcsMotor");

            migrationBuilder.RenameColumn(
                name: "GCS_Eye",
                table: "VitalSigns",
                newName: "GcsEye");

            migrationBuilder.RenameColumn(
                name: "BP_Systolic",
                table: "VitalSigns",
                newName: "BpSystolic");

            migrationBuilder.RenameColumn(
                name: "BP_Diastolic",
                table: "VitalSigns",
                newName: "BpDiastolic");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Cvp",
                table: "VitalSigns",
                newName: "CVP");

            migrationBuilder.RenameColumn(
                name: "GcsVerbal",
                table: "VitalSigns",
                newName: "GCS_Verbal");

            migrationBuilder.RenameColumn(
                name: "GcsMotor",
                table: "VitalSigns",
                newName: "GCS_Motor");

            migrationBuilder.RenameColumn(
                name: "GcsEye",
                table: "VitalSigns",
                newName: "GCS_Eye");

            migrationBuilder.RenameColumn(
                name: "BpSystolic",
                table: "VitalSigns",
                newName: "BP_Systolic");

            migrationBuilder.RenameColumn(
                name: "BpDiastolic",
                table: "VitalSigns",
                newName: "BP_Diastolic");
        }
    }
}
