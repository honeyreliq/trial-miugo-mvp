using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IUGOCare.Infrastructure.Persistence.Migrations
{
    public partial class AddClinicPatientIdToPatientCareManagementProgram : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ClinicPatientId",
                table: "PatientCareManagementPrograms",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

//            migrationBuilder.Sql(@"UPDATE PatientCareManagementPrograms
//SET ClinicPatientId = (
//	SELECT TOP 1 ClinicPatientId
//    FROM ClinicPatients
//    WHERE ClinicPatients.PatientId = PatientCareManagementPrograms.PatientId
//)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClinicPatientId",
                table: "PatientCareManagementPrograms");
        }
    }
}
