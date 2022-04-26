using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IUGOCare.Infrastructure.Persistence.Migrations
{
    public partial class AddPatientClinicIdToObservations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ClinicPatientId",
                table: "Observations",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.DropForeignKey("FK_Observation_Patient_PatientId", "Observations");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey("FK_Observation_Patient_PatientId", "Observations", "PatientId", "Patients");

            migrationBuilder.DropColumn(
                name: "ClinicPatientId",
                table: "Observations");
        }
    }
}
