using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IUGOCare.Infrastructure.Persistence.Migrations
{
    public partial class AddPatientTours : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PatientTour",
                columns: table => new
                {
                    PatientId = table.Column<Guid>(nullable: false),
                    TourKey = table.Column<string>(maxLength: 200, nullable: false),
                    Completed = table.Column<DateTimeOffset>(nullable: false, defaultValue: new DateTimeOffset(new DateTime(2020, 7, 21, 13, 55, 25, 247, DateTimeKind.Unspecified).AddTicks(9202), new TimeSpan(0, 0, 0, 0, 0))),
                    CompletionReason = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_PatientTour_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PatientTour_PatientId_TourKey",
                table: "PatientTour",
                columns: new[] { "PatientId", "TourKey" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PatientTour");
        }
    }
}
