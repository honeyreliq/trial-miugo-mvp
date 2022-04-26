using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IUGOCare.Infrastructure.Persistence.Migrations
{
    public partial class RemovePatientIdFromEmergencyContact : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientsEmergencyContact_Patients_PatientId",
                table: "PatientsEmergencyContact");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PatientsEmergencyContact",
                table: "PatientsEmergencyContact");

            migrationBuilder.DropIndex(
                name: "IX_PatientsEmergencyContact_ClinicPatientId",
                table: "PatientsEmergencyContact");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "PatientsEmergencyContact");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PatientsEmergencyContact",
                table: "PatientsEmergencyContact",
                column: "ClinicPatientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PatientsEmergencyContact",
                table: "PatientsEmergencyContact");

            migrationBuilder.AddColumn<Guid>(
                name: "PatientId",
                table: "PatientsEmergencyContact",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_PatientsEmergencyContact",
                table: "PatientsEmergencyContact",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientsEmergencyContact_ClinicPatientId",
                table: "PatientsEmergencyContact",
                column: "ClinicPatientId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientsEmergencyContact_Patients_PatientId",
                table: "PatientsEmergencyContact",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
