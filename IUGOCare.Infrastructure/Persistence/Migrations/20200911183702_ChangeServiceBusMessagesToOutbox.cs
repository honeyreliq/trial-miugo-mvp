using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IUGOCare.Infrastructure.Persistence.Migrations
{
    public partial class ChangeServiceBusMessagesToOutbox : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServiceBusMessages");

            migrationBuilder.CreateTable(
                name: "Outbox",
                schema: "staging",
                columns: table => new
                {
                    MessageId = table.Column<Guid>(nullable: false),
                    CorrelationId = table.Column<string>(maxLength: 500, nullable: false),
                    Label = table.Column<string>(maxLength: 500, nullable: false),
                    MessageBody = table.Column<string>(nullable: false),
                    DateStaged = table.Column<DateTimeOffset>(nullable: false),
                    ExpectsAcknowledgement = table.Column<bool>(nullable: false),
                    MessageSent = table.Column<bool>(nullable: false),
                    MessageAcknowledged = table.Column<bool>(nullable: false),
                    Errors = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Outbox", x => x.MessageId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Outbox",
                schema: "staging");

            migrationBuilder.CreateTable(
                name: "ServiceBusMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CorrelationId = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DateStaged = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ExpectsAcknowledgement = table.Column<bool>(type: "bit", nullable: false),
                    Label = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    MessageAcknowledged = table.Column<bool>(type: "bit", nullable: false),
                    MessageBody = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MessageSent = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceBusMessages", x => x.Id);
                });
        }
    }
}
