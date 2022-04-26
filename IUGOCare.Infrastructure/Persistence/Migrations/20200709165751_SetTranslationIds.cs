using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IUGOCare.Infrastructure.Persistence.Migrations
{
    public partial class SetTranslationIds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Translations");

            migrationBuilder.InsertData(
                table: "Translations",
                columns: new[] { "Id", "Created", "CreatedBy", "ElementName", "FileContent", "Language", "LastModified", "LastModifiedBy" },
                values: new object[,]
                {
                    { new Guid("48ee8729-8c43-4ca2-95c5-5f5f2c0cfc3b"), DateTimeOffset.UtcNow, null, "about", null, "en", null, null },
                    { new Guid("7bc5937a-3e33-4abe-8202-86bcee0d555b"), DateTimeOffset.UtcNow, null, "about", null, "es", null, null },
                    { new Guid("4de11867-663e-4e13-a8cd-1174e7d0e7f1"), DateTimeOffset.UtcNow, null, "privacyPolicy", null, "en", null, null },
                    { new Guid("ed4e76f8-425b-444e-bc55-2e48b2ec4b05"), DateTimeOffset.UtcNow, null, "privacyPolicy", null, "es", null, null },
                    { new Guid("f8a902c9-f7fe-4203-a97d-b9fc7f3135ad"), DateTimeOffset.UtcNow, null, "announcements", null, "en", null, null },
                    { new Guid("7a9c9d5d-d5ef-4380-9f16-5b1b7faf092d"), DateTimeOffset.UtcNow, null, "announcements", null, "es", null, null }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Translations");

            migrationBuilder.InsertData(
                table: "Translations",
                columns: new[] { "Id", "Created", "CreatedBy", "ElementName", "FileContent", "Language", "LastModified", "LastModifiedBy" },
                values: new object[,]
                {
                    { new Guid("4bedc27f-cf93-4b12-9c2e-aede42d0259e"), DateTimeOffset.UtcNow, null, "about", null, "en", null, null },
                    { new Guid("80f0238c-0efa-45f5-a344-125553fbf4d0"), DateTimeOffset.UtcNow, null, "about", null, "es", null, null },
                    { new Guid("61ef3e96-5698-4950-9703-db9f68c71a46"), DateTimeOffset.UtcNow, null, "privacyPolicy", null, "en", null, null },
                    { new Guid("b16811c4-afed-4ad6-8772-f499bb41dfbe"), DateTimeOffset.UtcNow, null, "privacyPolicy", null, "es", null, null },
                    { new Guid("e71fdbf2-ee0e-4c17-a352-416276e2b037"), DateTimeOffset.UtcNow, null, "announcements", null, "en", null, null },
                    { new Guid("208b2cd3-799f-4d11-bb2a-1fa65765ebb9"), DateTimeOffset.UtcNow, null, "announcements", null, "es", null, null }
                });
        }
    }
}
