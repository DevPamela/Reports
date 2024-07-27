using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Reports.Infrastructure.Migrations
{
    public partial class AddingAssessmentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assessment_Goals_GoalId",
                table: "Assessment");

            migrationBuilder.DropForeignKey(
                name: "FK_Assessment_Students_StudentId",
                table: "Assessment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Assessment",
                table: "Assessment");

            migrationBuilder.RenameTable(
                name: "Assessment",
                newName: "Assessments");

            migrationBuilder.RenameIndex(
                name: "IX_Assessment_StudentId",
                table: "Assessments",
                newName: "IX_Assessments_StudentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Assessments",
                table: "Assessments",
                columns: new[] { "GoalId", "StudentId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Assessments_Goals_GoalId",
                table: "Assessments",
                column: "GoalId",
                principalTable: "Goals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Assessments_Students_StudentId",
                table: "Assessments",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assessments_Goals_GoalId",
                table: "Assessments");

            migrationBuilder.DropForeignKey(
                name: "FK_Assessments_Students_StudentId",
                table: "Assessments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Assessments",
                table: "Assessments");

            migrationBuilder.RenameTable(
                name: "Assessments",
                newName: "Assessment");

            migrationBuilder.RenameIndex(
                name: "IX_Assessments_StudentId",
                table: "Assessment",
                newName: "IX_Assessment_StudentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Assessment",
                table: "Assessment",
                columns: new[] { "GoalId", "StudentId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Assessment_Goals_GoalId",
                table: "Assessment",
                column: "GoalId",
                principalTable: "Goals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Assessment_Students_StudentId",
                table: "Assessment",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
