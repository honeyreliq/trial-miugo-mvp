using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IUGOCare.Infrastructure.Persistence.Migrations
{
    public partial class AddTargetRange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TargetRanges",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    Created = table.Column<DateTimeOffset>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModified = table.Column<DateTimeOffset>(nullable: true),
                    ObservationCode = table.Column<string>(maxLength: 255, nullable: false),
                    PatientId = table.Column<Guid>(nullable: false),
                    Unit = table.Column<string>(maxLength: 100, nullable: false),
                    CriticalHigh = table.Column<decimal>(type: "decimal(7,1)", nullable: false),
                    AtRiskHigh = table.Column<decimal>(type: "decimal(7,1)", nullable: false),
                    AtRiskLow = table.Column<decimal>(type: "decimal(7,1)", nullable: false),
                    CriticalLow = table.Column<decimal>(type: "decimal(7,1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TargetRanges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TargetRanges_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TargetRanges_PatientId",
                table: "TargetRanges",
                column: "PatientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TargetRanges");
        }
    }
}
