using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IUGOCare.Infrastructure.Persistence.Migrations
{
    public partial class AddPrimaryKeyToPatientTours : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientTour_Patients_PatientId",
                table: "PatientTour");

            migrationBuilder.RenameTable(
                name: "PatientTour",
                newName: "PatientTours");

            migrationBuilder.RenameIndex(
                name: "IX_PatientTour_PatientId_TourKey",
                table: "PatientTours",
                newName: "IX_PatientTours_PatientId_TourKey");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "Completed",
                table: "PatientTours",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2020, 7, 21, 13, 55, 25, 247, DateTimeKind.Unspecified).AddTicks(9202), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddPrimaryKey(
                name: "PK_PatientTours",
                table: "PatientTours",
                columns: new[] { "PatientId", "TourKey" });

            migrationBuilder.AddForeignKey(
                name: "FK_PatientTours_Patients_PatientId",
                table: "PatientTours",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientTours_Patients_PatientId",
                table: "PatientTours");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PatientTours",
                table: "PatientTours");

            migrationBuilder.RenameTable(
                name: "PatientTours",
                newName: "PatientTour");

            migrationBuilder.RenameIndex(
                name: "IX_PatientTours_PatientId_TourKey",
                table: "PatientTour",
                newName: "IX_PatientTour_PatientId_TourKey");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "Completed",
                table: "PatientTour",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2020, 7, 21, 13, 55, 25, 247, DateTimeKind.Unspecified).AddTicks(9202), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset));

            migrationBuilder.AddForeignKey(
                name: "FK_PatientTour_Patients_PatientId",
                table: "PatientTour",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
