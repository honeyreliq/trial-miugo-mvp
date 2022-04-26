using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IUGOCare.Infrastructure.Persistence.Migrations
{
    public partial class UpdateTestClinicData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                 table: "Clinics",
                 columns: new[] { "Id", "Created", "CreatedBy", "LastModified", "LastModifiedBy", "Subdomain", "EmailsEnabled" },
                 values: new object[,]
                 {
                    {
                         new Guid("c9b9d356-94ca-4efa-9488-7fdaf36fd898"),
                         new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                         null,
                         null,
                         null,
                         null,
                         true
                     },
                 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DeleteData(
                table: "Clinics",
                keyColumn: "Id",
                keyValue: new Guid("c9b9d356-94ca-4efa-9488-7fdaf36fd898"));
        }
    }
}
