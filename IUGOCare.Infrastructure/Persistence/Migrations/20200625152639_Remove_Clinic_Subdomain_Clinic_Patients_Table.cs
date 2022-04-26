using Microsoft.EntityFrameworkCore.Migrations;

namespace IUGOCare.Infrastructure.Persistence.Migrations
{
    public partial class Remove_Clinic_Subdomain_Clinic_Patients_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClinicSubdomain",
                table: "ClinicPatients");

            migrationBuilder.AlterColumn<string>(
                name: "Subdomain",
                table: "Clinics",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "EmailsEnabled",
                table: "Clinics",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Subdomain",
                table: "Clinics",
                type: "varchar(500)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "EmailsEnabled",
                table: "Clinics",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool));

            migrationBuilder.AddColumn<string>(
                name: "ClinicSubdomain",
                table: "ClinicPatients",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }
    }
}
