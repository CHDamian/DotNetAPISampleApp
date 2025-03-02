using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotNetAPISampleApp.Migrations
{
    /// <inheritdoc />
    public partial class ExaminationAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Examinations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ResearcherId = table.Column<int>(type: "INTEGER", nullable: true),
                    PatientId = table.Column<int>(type: "INTEGER", nullable: true),
                    ExamDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    Results = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Examinations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Examinations_ResearchSigneds_PatientId",
                        column: x => x.PatientId,
                        principalTable: "ResearchSigneds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Examinations_ResearchSigneds_ResearcherId",
                        column: x => x.ResearcherId,
                        principalTable: "ResearchSigneds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Examinations_PatientId",
                table: "Examinations",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Examinations_ResearcherId",
                table: "Examinations",
                column: "ResearcherId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Examinations");
        }
    }
}
