using Microsoft.EntityFrameworkCore.Migrations;

namespace IUGOCare.Infrastructure.Persistence.Migrations
{
    public partial class AddPatientTimeZoneAndWindowsTimeZone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TimeZone",
                table: "Patients",
                maxLength: 100,
                nullable: false,
                defaultValue: "Etc/UTC");

            migrationBuilder.AddColumn<string>(
                name: "WindowsTimeZone",
                table: "Patients",
                maxLength: 100,
                nullable: false,
                defaultValue: "UTC");

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
            migrationBuilder.DropColumn(
                name: "TimeZone",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "WindowsTimeZone",
                table: "Patients");

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
        }
    }
}
