using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IUGOCare.Infrastructure.Persistence.Migrations
{
    public partial class AddShortNameAndSeedCarePrograms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShortName",
                table: "CareManagementPrograms",
                maxLength: 4,
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "CareManagementPrograms",
                columns: new[] { "Id", "Created", "CreatedBy", "LastModified", "LastModifiedBy", "Name", "ShortName" },
                values: new object[,]
                {
                    { new Guid("0ac9cd3c-e8fa-49bb-9b12-011b2ac87f4c"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, "Chronic Care Management", "CCM" },
                    { new Guid("4758ee46-f738-4c9e-83af-6598f451128a"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, "Remote Patient Monitoring", "RPM" },
                    { new Guid("bb10362b-a9d9-4249-ae1c-f9a3b0bee797"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, "Behavioral Health Integration", "BHI" },
                    { new Guid("fc56c18a-038f-43d9-a83f-d4cf7bc9cb67"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, "Principal Care Management", "PCM" },
                    { new Guid("0dabb363-e803-4e99-8f22-52f61a6960a6"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, "Psychiatric Collaborative Care Model", "CoCM" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CareManagementPrograms",
                keyColumn: "Id",
                keyValue: new Guid("0ac9cd3c-e8fa-49bb-9b12-011b2ac87f4c"));

            migrationBuilder.DeleteData(
                table: "CareManagementPrograms",
                keyColumn: "Id",
                keyValue: new Guid("0dabb363-e803-4e99-8f22-52f61a6960a6"));

            migrationBuilder.DeleteData(
                table: "CareManagementPrograms",
                keyColumn: "Id",
                keyValue: new Guid("4758ee46-f738-4c9e-83af-6598f451128a"));

            migrationBuilder.DeleteData(
                table: "CareManagementPrograms",
                keyColumn: "Id",
                keyValue: new Guid("bb10362b-a9d9-4249-ae1c-f9a3b0bee797"));

            migrationBuilder.DeleteData(
                table: "CareManagementPrograms",
                keyColumn: "Id",
                keyValue: new Guid("fc56c18a-038f-43d9-a83f-d4cf7bc9cb67"));

            migrationBuilder.DropColumn(
                name: "ShortName",
                table: "CareManagementPrograms");
        }
    }
}
