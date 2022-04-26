using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IUGOCare.Infrastructure.Persistence.Migrations
{
    public partial class AddServiceBusMessageStagingTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ServiceBusMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CorrelationId = table.Column<string>(maxLength: 500, nullable: true),
                    Label = table.Column<string>(maxLength: 500, nullable: true),
                    MessageBody = table.Column<string>(nullable: true),
                    DateStaged = table.Column<DateTimeOffset>(nullable: false),
                    ExpectsAcknowledgement = table.Column<bool>(nullable: false),
                    MessageSent = table.Column<bool>(nullable: false),
                    MessageAcknowledged = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceBusMessages", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServiceBusMessages");
        }
    }
}
