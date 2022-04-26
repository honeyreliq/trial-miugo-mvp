using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IUGOCare.Infrastructure.Persistence.Migrations
{
    public partial class Translation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Translations",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    Created = table.Column<DateTimeOffset>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModified = table.Column<DateTimeOffset>(nullable: true),
                    ElementName = table.Column<string>(nullable: false),
                    Language = table.Column<string>(maxLength: 2, nullable: false),
                    FileContent = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Translations", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Translations",
                columns: new[] { "Id", "Created", "CreatedBy", "ElementName", "FileContent", "Language", "LastModified", "LastModifiedBy" },
                values: new object[,]
                {
                    { new Guid("2db14024-e78d-4f54-b693-107da6a876fe"), new DateTimeOffset(new DateTime(2020, 7, 1, 16, 2, 21, 773, DateTimeKind.Unspecified).AddTicks(322), new TimeSpan(0, -6, 0, 0, 0)), null, "about", null, "en", null, null },
                    { new Guid("37edd58c-72ae-40c2-853c-68e787b98128"), new DateTimeOffset(new DateTime(2020, 7, 1, 16, 2, 21, 775, DateTimeKind.Unspecified).AddTicks(4048), new TimeSpan(0, -6, 0, 0, 0)), null, "about", null, "es", null, null },
                    { new Guid("8c8a00c9-63aa-4f06-b0f4-30d03163e5e7"), new DateTimeOffset(new DateTime(2020, 7, 1, 16, 2, 21, 775, DateTimeKind.Unspecified).AddTicks(4077), new TimeSpan(0, -6, 0, 0, 0)), null, "privacyPolicy", null, "en", null, null },
                    { new Guid("a0d5dd16-5fd8-49a3-bcb0-990d6236c86b"), new DateTimeOffset(new DateTime(2020, 7, 1, 16, 2, 21, 775, DateTimeKind.Unspecified).AddTicks(4080), new TimeSpan(0, -6, 0, 0, 0)), null, "privacyPolicy", null, "es", null, null },
                    { new Guid("561a8240-7acd-4759-ad10-1662ddf51f69"), new DateTimeOffset(new DateTime(2020, 7, 1, 16, 2, 21, 775, DateTimeKind.Unspecified).AddTicks(4083), new TimeSpan(0, -6, 0, 0, 0)), null, "announcements", null, "en", null, null },
                    { new Guid("805a649a-3433-4a7c-9de4-1ca4a64e7915"), new DateTimeOffset(new DateTime(2020, 7, 1, 16, 2, 21, 775, DateTimeKind.Unspecified).AddTicks(4085), new TimeSpan(0, -6, 0, 0, 0)), null, "announcements", null, "es", null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Translations_ElementName_Language",
                table: "Translations",
                columns: new[] { "ElementName", "Language" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Translations");
        }
    }
}
