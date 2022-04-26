using Microsoft.EntityFrameworkCore.Migrations;

namespace IUGOCare.Infrastructure.Persistence.Migrations
{
    public partial class UpdatePatientNameFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
                name: "FamilyName",
                table: "Patients",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GivenName",
                table: "Patients",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MiddleName",
                table: "Patients",
                nullable: true);

            migrationBuilder.DropColumn(
                name: "PatientName",
                table: "Patients");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PatientName",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.Sql("UPDATE [dbo].[Patients] " +
                "SET[PatientName] = CONCAT([GivenName], COALESCE(' ' + [MiddleName], ''), COALESCE(' ' + [FamilyName], '')); ");

            migrationBuilder.DropColumn(
                name: "FamilyName",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "GivenName",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "MiddleName",
                table: "Patients");
        }
    }
}
