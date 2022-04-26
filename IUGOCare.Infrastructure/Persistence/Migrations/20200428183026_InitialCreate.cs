using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace IUGOCare.Infrastructure.Persistence.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Observations",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ObservationDate = table.Column<DateTime>(),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Observations", x => x.Id);
                }
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("Observations");
        }
    }
}
