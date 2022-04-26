using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IUGOCare.Infrastructure.Persistence.Migrations
{
    public partial class AddPreferencesToPatient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DateFormat",
                table: "Patients",
                nullable: true,
                defaultValue: "MM DD, YYYY");

            migrationBuilder.AddColumn<string>(
                name: "PatientTheme",
                table: "Patients",
                nullable: true,
                defaultValue: "Light Mode");

            migrationBuilder.AddColumn<string>(
                name: "TimeFormat",
                table: "Patients",
                nullable: true,
                defaultValue: "12 H");

            migrationBuilder.AddColumn<bool>(
                name: "Tooltips",
                table: "Patients",
                nullable: false,
                defaultValue: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateFormat",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "PatientTheme",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "TimeFormat",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "Tooltips",
                table: "Patients");
        }
    }
}
