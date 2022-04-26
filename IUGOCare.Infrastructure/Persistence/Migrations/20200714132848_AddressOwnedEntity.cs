using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IUGOCare.Infrastructure.Persistence.Migrations
{
    public partial class AddressOwnedEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Providers");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Organizations");

            migrationBuilder.AddColumn<string>(
                name: "Address_AddressLines",
                table: "Providers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_City",
                table: "Providers",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_Country",
                table: "Providers",
                maxLength: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_State",
                table: "Providers",
                maxLength: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_ZipCode",
                table: "Providers",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_AddressLines",
                table: "Organizations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_City",
                table: "Organizations",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_Country",
                table: "Organizations",
                maxLength: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_State",
                table: "Organizations",
                maxLength: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_ZipCode",
                table: "Organizations",
                maxLength: 20,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    PatientId = table.Column<Guid>(nullable: false),
                    AddressLines = table.Column<string>(nullable: true),
                    City = table.Column<string>(maxLength: 200, nullable: true),
                    State = table.Column<string>(maxLength: 2, nullable: true),
                    Country = table.Column<string>(maxLength: 2, nullable: true),
                    ZipCode = table.Column<string>(maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.PatientId);
                    table.ForeignKey(
                        name: "FK_Addresses_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropColumn(
                name: "Address_AddressLines",
                table: "Providers");

            migrationBuilder.DropColumn(
                name: "Address_City",
                table: "Providers");

            migrationBuilder.DropColumn(
                name: "Address_Country",
                table: "Providers");

            migrationBuilder.DropColumn(
                name: "Address_State",
                table: "Providers");

            migrationBuilder.DropColumn(
                name: "Address_ZipCode",
                table: "Providers");

            migrationBuilder.DropColumn(
                name: "Address_AddressLines",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "Address_City",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "Address_Country",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "Address_State",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "Address_ZipCode",
                table: "Organizations");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Providers",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Organizations",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }
    }
}
