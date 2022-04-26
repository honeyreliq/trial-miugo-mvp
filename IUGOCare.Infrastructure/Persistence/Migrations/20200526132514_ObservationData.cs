using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IUGOCare.Infrastructure.Persistence.Migrations
{
    public partial class ObservationData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ObservationsData",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ObservationId = table.Column<Guid>(nullable: false),
                    ObservationCode = table.Column<string>(nullable: true, maxLength: 200),
                    Value = table.Column<decimal>(nullable: false, type: "decimal(18,2)"),
                    Unit = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    Created = table.Column<DateTimeOffset>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModified = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObservationData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ObservationData_Observation_ObservationId",
                        column: x => x.ObservationId,
                        principalTable: "Observations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                        );
                }
                );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("ObservationsData");
        }
    }
}
