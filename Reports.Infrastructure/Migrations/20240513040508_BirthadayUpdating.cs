using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Reports.Infrastructure.Migrations
{
    public partial class BirthadayUpdating : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Birthday",
                table: "Students",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Birthday",
                table: "Students",
                type: "int",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");
        }
    }
}
