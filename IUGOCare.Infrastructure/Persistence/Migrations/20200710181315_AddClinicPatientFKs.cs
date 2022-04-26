using Microsoft.EntityFrameworkCore.Migrations;

namespace IUGOCare.Infrastructure.Persistence.Migrations
{
    public partial class AddClinicPatientFKs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ClinicPatients_ClinicId",
                table: "ClinicPatients",
                column: "ClinicId");

            // migrationBuilder.AddForeignKey(
            //     name: "FK_ClinicPatients_Clinics_ClinicId",
            //     table: "ClinicPatients",
            //     column: "ClinicId",
            //     principalTable: "Clinics",
            //     principalColumn: "Id",
            //     onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DropForeignKey(
            //     name: "FK_ClinicPatients_Clinics_ClinicId",
            //     table: "ClinicPatients");

            migrationBuilder.DropIndex(
                name: "IX_ClinicPatients_ClinicId",
                table: "ClinicPatients");
        }
    }
}
