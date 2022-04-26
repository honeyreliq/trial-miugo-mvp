using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IUGOCare.Infrastructure.Persistence.Migrations
{
    public partial class AddInbox : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "staging");

            migrationBuilder.CreateTable(
                name: "Inbox",
                schema: "staging",
                columns: table => new
                {
                    MessageId = table.Column<Guid>(nullable: false),
                    Label = table.Column<string>(maxLength: 500, nullable: false),
                    MessageBody = table.Column<string>(nullable: false),
                    Errors = table.Column<string>(nullable: true),
                    Status = table.Column<string>(maxLength: 100, nullable: true),
                    Created = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inbox", x => x.MessageId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Inbox",
                schema: "staging");
        }
    }
}
