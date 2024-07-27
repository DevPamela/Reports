using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Reports.Infrastructure.Migrations
{
    public partial class AddAbsencesAsInt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Birthday",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "longtext",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "Absences",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "longtext",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Birthday",
                table: "Students",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Absences",
                table: "Students",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
