using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IUGOCare.Audit.Persistence.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApiAudit",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RequestId = table.Column<Guid>(nullable: true),
                    Uri = table.Column<string>(nullable: true),
                    Method = table.Column<string>(nullable: true),
                    StatusCode = table.Column<string>(nullable: true),
                    ReasonPhrase = table.Column<string>(nullable: true),
                    Headers = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    RequestReliqUserId = table.Column<Guid>(nullable: true),
                    CreateDate = table.Column<DateTimeOffset>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiAudit", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApiAudit");
        }
    }
}
