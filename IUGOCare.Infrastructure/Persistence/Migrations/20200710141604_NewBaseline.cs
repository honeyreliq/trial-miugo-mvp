using Microsoft.EntityFrameworkCore.Migrations;

namespace IUGOCare.Infrastructure.Persistence.Migrations
{
    public partial class NewBaseline : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Tooltips",
                table: "Patients",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<string>(
                name: "TimeFormat",
                table: "Patients",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldDefaultValue: "12 H");

            migrationBuilder.AlterColumn<string>(
                name: "PatientTheme",
                table: "Patients",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldDefaultValue: "Light Mode");

            migrationBuilder.AlterColumn<string>(
                name: "DateFormat",
                table: "Patients",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldDefaultValue: "MM DD, YYYY");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Tooltips",
                table: "Patients",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<string>(
                name: "TimeFormat",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "12 H",
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PatientTheme",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "Light Mode",
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DateFormat",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "MM DD, YYYY",
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
