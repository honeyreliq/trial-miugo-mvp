using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IUGOCare.Infrastructure.Persistence.Migrations
{
    public partial class AddEmergencyContactToClinicPatient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ClinicPatientId",
                table: "PatientsEmergencyContact",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

//             migrationBuilder.Sql(@"UPDATE PatientsEmergencyContact
// SET PatientsEmergencyContact.ClinicPatientId = (
//   SELECT TOP 1 ClinicPatients.ClinicPatientId
//   FROM ClinicPatients
//   WHERE ClinicPatients.PatientId = PatientsEmergencyContact.PatientId
// )");

            migrationBuilder.CreateIndex(
                name: "IX_PatientsEmergencyContact_ClinicPatientId",
                table: "PatientsEmergencyContact",
                column: "ClinicPatientId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientsEmergencyContact_ClinicPatients_ClinicPatientId",
                table: "PatientsEmergencyContact",
                column: "ClinicPatientId",
                principalTable: "ClinicPatients",
                principalColumn: "ClinicPatientId",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientsEmergencyContact_ClinicPatients_ClinicPatientId",
                table: "PatientsEmergencyContact");

            migrationBuilder.DropIndex(
                name: "IX_PatientsEmergencyContact_ClinicPatientId",
                table: "PatientsEmergencyContact");

            migrationBuilder.DropColumn(
                name: "ClinicPatientId",
                table: "PatientsEmergencyContact");
        }
    }
}
