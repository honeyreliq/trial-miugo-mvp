using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IUGOCare.Infrastructure.Persistence.Migrations
{
    public partial class ChangeClinicIdToClinicSubdomain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClinicId",
                table: "ClinicPatients");

            migrationBuilder.AddColumn<string>(
                name: "ClinicSubdomain",
                table: "ClinicPatients",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClinicSubdomain",
                table: "ClinicPatients");

            migrationBuilder.AddColumn<Guid>(
                name: "ClinicId",
                table: "ClinicPatients",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
