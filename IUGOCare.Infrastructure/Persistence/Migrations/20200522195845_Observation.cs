using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IUGOCare.Infrastructure.Persistence.Migrations
{
    public partial class Observation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            this.CheckTablesExist(migrationBuilder);

            migrationBuilder.CreateTable(
                "Observations",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PatientId = table.Column<Guid>(nullable: false),
                    ObservationCode = table.Column<string>(nullable: true, maxLength: 200),
                    EffectiveDate = table.Column<DateTimeOffset>(nullable: false),
                    Source = table.Column<string>(nullable: false),
                    ObservationStatus = table.Column<string>(nullable: false),
                    ObservationLevel = table.Column<string>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    Created = table.Column<DateTimeOffset>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModified = table.Column<DateTimeOffset>(nullable: true),
                    IsReviewed = table.Column<bool>(nullable: false),
                    IsReviewedDate = table.Column<DateTimeOffset>(nullable: true),
                    ReviewedByName = table.Column<string>(nullable: true),
                    Manufacturer = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Observation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Observation_Patient_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                }
                );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Observations");
        }

        protected void CheckTablesExist(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                                IF (EXISTS(SELECT * 
                                    FROM INFORMATION_SCHEMA.TABLES
                                    WHERE TABLE_SCHEMA = 'dbo'
                                    AND  TABLE_NAME = 'ObservationsData'))
                                BEGIN
                                    DROP TABLE ObservationsData
                                END

                                IF (EXISTS(SELECT * 
                                    FROM INFORMATION_SCHEMA.TABLES
                                    WHERE TABLE_SCHEMA = 'dbo'
                                    AND  TABLE_NAME = 'Observations'))
                                BEGIN
                                    DROP TABLE Observations
                                END
                            ");
        }
    }
}
