using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Reports.Infrastructure.Migrations
{
    public partial class AddingIsSelectedProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassRoom_Teacher_TeacherId",
                table: "ClassRoom");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_ClassRoom_ClassRoomId",
                table: "Students");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClassRoom",
                table: "ClassRoom");

            migrationBuilder.RenameTable(
                name: "ClassRoom",
                newName: "Classroom");

            migrationBuilder.RenameIndex(
                name: "IX_ClassRoom_TeacherId",
                table: "Classroom",
                newName: "IX_Classroom_TeacherId");

            migrationBuilder.AddColumn<bool>(
                name: "IsSelcted",
                table: "Students",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSelected",
                table: "Classroom",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Classroom",
                table: "Classroom",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Classroom_Teacher_TeacherId",
                table: "Classroom",
                column: "TeacherId",
                principalTable: "Teacher",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Classroom_ClassRoomId",
                table: "Students",
                column: "ClassRoomId",
                principalTable: "Classroom",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classroom_Teacher_TeacherId",
                table: "Classroom");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Classroom_ClassRoomId",
                table: "Students");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Classroom",
                table: "Classroom");

            migrationBuilder.DropColumn(
                name: "IsSelcted",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "IsSelected",
                table: "Classroom");

            migrationBuilder.RenameTable(
                name: "Classroom",
                newName: "ClassRoom");

            migrationBuilder.RenameIndex(
                name: "IX_Classroom_TeacherId",
                table: "ClassRoom",
                newName: "IX_ClassRoom_TeacherId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClassRoom",
                table: "ClassRoom",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClassRoom_Teacher_TeacherId",
                table: "ClassRoom",
                column: "TeacherId",
                principalTable: "Teacher",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_ClassRoom_ClassRoomId",
                table: "Students",
                column: "ClassRoomId",
                principalTable: "ClassRoom",
                principalColumn: "Id");
        }
    }
}
