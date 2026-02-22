using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cortexa.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Oringin2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhoneNumbers",
                table: "Patients",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "PhoneNumbers",
                table: "Nurses",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "PhoneNumbers",
                table: "Doctors",
                newName: "PhoneNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Patients",
                newName: "PhoneNumbers");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Nurses",
                newName: "PhoneNumbers");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Doctors",
                newName: "PhoneNumbers");
        }
    }
}
