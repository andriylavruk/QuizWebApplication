using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizApp.Server.Migrations
{
    /// <inheritdoc />
    public partial class DeleteTableTestResultEditTableTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Groups_GroupId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "TestResults");

            migrationBuilder.DropColumn(
                name: "IsActivated",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AvailableFrom",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "IsRandomAnswers",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "IsRandomQuestions",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "Questions");

            migrationBuilder.RenameColumn(
                name: "Duration",
                table: "Tests",
                newName: "StartedAt");

            migrationBuilder.RenameColumn(
                name: "AvailableUntil",
                table: "Tests",
                newName: "FinishedAt");

            migrationBuilder.AlterColumn<Guid>(
                name: "GroupId",
                table: "Users",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Groups_GroupId",
                table: "Users",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Groups_GroupId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "StartedAt",
                table: "Tests",
                newName: "Duration");

            migrationBuilder.RenameColumn(
                name: "FinishedAt",
                table: "Tests",
                newName: "AvailableUntil");

            migrationBuilder.AlterColumn<Guid>(
                name: "GroupId",
                table: "Users",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActivated",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "AvailableFrom",
                table: "Tests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Tests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRandomAnswers",
                table: "Tests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRandomQuestions",
                table: "Tests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Number",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TestResults",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SelectedAnswerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TestParticipantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TestQuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestResults_Answers_SelectedAnswerId",
                        column: x => x.SelectedAnswerId,
                        principalTable: "Answers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TestResults_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TestResults_TestParticipants_TestParticipantId",
                        column: x => x.TestParticipantId,
                        principalTable: "TestParticipants",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TestResults_QuestionId",
                table: "TestResults",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_TestResults_SelectedAnswerId",
                table: "TestResults",
                column: "SelectedAnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_TestResults_TestParticipantId",
                table: "TestResults",
                column: "TestParticipantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Groups_GroupId",
                table: "Users",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
