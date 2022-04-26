using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IUGOCare.Infrastructure.Persistence.Migrations
{
    public partial class AddClinicPatientIdToTargetRanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TargetRanges_Patients_PatientId",
                table: "TargetRanges");

            migrationBuilder.DropIndex(
                name: "IX_TargetRanges_PatientId",
                table: "TargetRanges");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "TargetRanges");

            migrationBuilder.AddColumn<Guid>(
                name: "ClinicPatientId",
                table: "TargetRanges",
                nullable: false);

            migrationBuilder.CreateIndex(
                name: "IX_TargetRanges_ClinicPatientId",
                table: "TargetRanges",
                column: "ClinicPatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_TargetRanges_ClinicPatients_ClinicPatientId",
                table: "TargetRanges",
                column: "ClinicPatientId",
                principalTable: "ClinicPatients",
                principalColumn: "ClinicPatientId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TargetRanges_ClinicPatients_ClinicPatientId",
                table: "TargetRanges");

            migrationBuilder.DropIndex(
                name: "IX_TargetRanges_ClinicPatientId",
                table: "TargetRanges");

            migrationBuilder.DropColumn(
                name: "ClinicPatientId",
                table: "TargetRanges");

            migrationBuilder.AddColumn<Guid>(
                name: "PatientId",
                table: "TargetRanges",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_TargetRanges_PatientId",
                table: "TargetRanges",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_TargetRanges_Patients_PatientId",
                table: "TargetRanges",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
