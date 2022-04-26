using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IUGOCare.Infrastructure.Persistence.Migrations
{
    public partial class AddPatientColumnsToClinicPatient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "BirthDate",
                table: "ClinicPatients",
                type: "DATE",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FamilyName",
                table: "ClinicPatients",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GivenName",
                table: "ClinicPatients",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InsuranceNumber",
                table: "ClinicPatients",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MedicaidNumber",
                table: "ClinicPatients",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MedicalRecordNumber",
                table: "ClinicPatients",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MedicareNumber",
                table: "ClinicPatients",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MiddleName",
                table: "ClinicPatients",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "ClinicPatients",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PrimaryCareProviderId",
                table: "ClinicPatients",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TimeZone",
                table: "ClinicPatients",
                maxLength: 100,
                nullable: false,
                defaultValue: "Etc/UTC");

            migrationBuilder.AddColumn<string>(
                name: "WindowsTimeZone",
                table: "ClinicPatients",
                maxLength: 100,
                nullable: false,
                defaultValue: "UTC");

            migrationBuilder.AddColumn<string>(
                name: "Address_AddressLines",
                table: "ClinicPatients",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_City",
                table: "ClinicPatients",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_Country",
                table: "ClinicPatients",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_State",
                table: "ClinicPatients",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_ZipCode",
                table: "ClinicPatients",
                maxLength: 20,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClinicPatients_PrimaryCareProviderId",
                table: "ClinicPatients",
                column: "PrimaryCareProviderId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClinicPatients_Providers_PrimaryCareProviderId",
                table: "ClinicPatients",
                column: "PrimaryCareProviderId",
                principalTable: "Providers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

//            migrationBuilder.Sql(@"UPDATE ClinicPatients
//SET ClinicPatients.BirthDate = Patients.BirthDate,
//ClinicPatients.FamilyName = Patients.FamilyName,
//ClinicPatients.GivenName = Patients.GivenName,
//ClinicPatients.InsuranceNumber = Patients.InsuranceNumber,
//ClinicPatients.MedicaidNumber = Patients.MedicaidNumber,
//ClinicPatients.MedicalRecordNumber = Patients.MedicalRecordNumber,
//ClinicPatients.MedicareNumber = Patients.MedicareNumber,
//ClinicPatients.MiddleName = Patients.MiddleName,
//ClinicPatients.Phone = Patients.Phone,
//ClinicPatients.PrimaryCareProviderId = Patients.PrimaryCareProviderId,
//ClinicPatients.TimeZone = Patients.TimeZone,
//ClinicPatients.WindowsTimeZone = Patients.WindowsTimeZone,
//ClinicPatients.Address_AddressLines = Patients.Address_AddressLines,
//ClinicPatients.Address_City = Patients.Address_City,
//ClinicPatients.Address_Country = Patients.Address_Country,
//ClinicPatients.Address_State = Patients.Address_State,
//ClinicPatients.Address_ZipCode = Patients.Address_ZipCode
//FROM ClinicPatients
//INNER JOIN Patients ON Patients.Id = ClinicPatients.PatientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClinicPatients_Providers_PrimaryCareProviderId",
                table: "ClinicPatients");

            migrationBuilder.DropIndex(
                name: "IX_ClinicPatients_PrimaryCareProviderId",
                table: "ClinicPatients");

            migrationBuilder.DropColumn(
                name: "BirthDate",
                table: "ClinicPatients");

            migrationBuilder.DropColumn(
                name: "FamilyName",
                table: "ClinicPatients");

            migrationBuilder.DropColumn(
                name: "GivenName",
                table: "ClinicPatients");

            migrationBuilder.DropColumn(
                name: "InsuranceNumber",
                table: "ClinicPatients");

            migrationBuilder.DropColumn(
                name: "MedicaidNumber",
                table: "ClinicPatients");

            migrationBuilder.DropColumn(
                name: "MedicalRecordNumber",
                table: "ClinicPatients");

            migrationBuilder.DropColumn(
                name: "MedicareNumber",
                table: "ClinicPatients");

            migrationBuilder.DropColumn(
                name: "MiddleName",
                table: "ClinicPatients");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "ClinicPatients");

            migrationBuilder.DropColumn(
                name: "PrimaryCareProviderId",
                table: "ClinicPatients");

            migrationBuilder.DropColumn(
                name: "TimeZone",
                table: "ClinicPatients");

            migrationBuilder.DropColumn(
                name: "WindowsTimeZone",
                table: "ClinicPatients");

            migrationBuilder.DropColumn(
                name: "Address_AddressLines",
                table: "ClinicPatients");

            migrationBuilder.DropColumn(
                name: "Address_City",
                table: "ClinicPatients");

            migrationBuilder.DropColumn(
                name: "Address_Country",
                table: "ClinicPatients");

            migrationBuilder.DropColumn(
                name: "Address_State",
                table: "ClinicPatients");

            migrationBuilder.DropColumn(
                name: "Address_ZipCode",
                table: "ClinicPatients");
        }
    }
}
