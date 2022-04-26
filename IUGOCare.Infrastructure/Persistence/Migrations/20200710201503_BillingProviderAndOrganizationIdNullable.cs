using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IUGOCare.Infrastructure.Persistence.Migrations
{
    public partial class BillingProviderAndOrganizationIdNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientCareManagementPrograms_Providers_BillingProviderId",
                table: "PatientCareManagementPrograms");

            migrationBuilder.DropForeignKey(
                name: "FK_Providers_Organizations_OrganizationId",
                table: "Providers");

            migrationBuilder.AlterColumn<Guid>(
                name: "OrganizationId",
                table: "Providers",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "BillingProviderId",
                table: "PatientCareManagementPrograms",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientCareManagementPrograms_Providers_BillingProviderId",
                table: "PatientCareManagementPrograms",
                column: "BillingProviderId",
                principalTable: "Providers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Providers_Organizations_OrganizationId",
                table: "Providers",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientCareManagementPrograms_Providers_BillingProviderId",
                table: "PatientCareManagementPrograms");

            migrationBuilder.DropForeignKey(
                name: "FK_Providers_Organizations_OrganizationId",
                table: "Providers");

            migrationBuilder.AlterColumn<Guid>(
                name: "OrganizationId",
                table: "Providers",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "BillingProviderId",
                table: "PatientCareManagementPrograms",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientCareManagementPrograms_Providers_BillingProviderId",
                table: "PatientCareManagementPrograms",
                column: "BillingProviderId",
                principalTable: "Providers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Providers_Organizations_OrganizationId",
                table: "Providers",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
