using Microsoft.EntityFrameworkCore.Migrations;

namespace IUGOCare.Infrastructure.Persistence.Migrations
{
    public partial class FixObservationsDataNull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LastModified",
                table: "ObservationsData",
                type: "DATETIMEOFFSET(7)",
                nullable: true
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LastModified",
                table: "ObservationsData",
                type: "DATETIMEOFFSET(7)",
                nullable: false
            );
        }
    }
}
