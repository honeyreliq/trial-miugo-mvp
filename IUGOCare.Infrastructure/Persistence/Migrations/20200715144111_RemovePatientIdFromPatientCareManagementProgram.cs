using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IUGOCare.Infrastructure.Persistence.Migrations
{
    public partial class RemovePatientIdFromPatientCareManagementProgram : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientCareManagementPrograms_Patients_PatientId",
                table: "PatientCareManagementPrograms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PatientCareManagementPrograms",
                table: "PatientCareManagementPrograms");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "PatientCareManagementPrograms");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PatientCareManagementPrograms",
                table: "PatientCareManagementPrograms",
                columns: new[] { "ClinicPatientId", "CareManagementProgramId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientCareManagementPrograms_Patients_PatientId",
                table: "PatientCareManagementPrograms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PatientCareManagementPrograms",
                table: "PatientCareManagementPrograms");

            migrationBuilder.AddColumn<Guid>(
                name: "PatientId",
                table: "PatientCareManagementPrograms",
                type: "uniqueidentifier",
                nullable: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PatientCareManagementPrograms",
                table: "PatientCareManagementPrograms",
                columns: new[] { "PatientId", "CareManagementProgramId" });

            migrationBuilder.AddForeignKey(
                name: "FK_PatientCareManagementPrograms_Patients_PatientId",
                table: "PatientCareManagementPrograms",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
