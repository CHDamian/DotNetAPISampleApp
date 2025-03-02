using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotNetAPISampleApp.Migrations
{
    /// <inheritdoc />
    public partial class SigningAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ResearchSigneds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ResearchId = table.Column<int>(type: "INTEGER", nullable: false),
                    SignedUserId = table.Column<string>(type: "TEXT", nullable: true),
                    ResearchRole = table.Column<string>(type: "TEXT", nullable: false),
                    ActiveFrom = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ActiveTo = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResearchSigneds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResearchSigneds_AspNetUsers_SignedUserId",
                        column: x => x.SignedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ResearchSigneds_Researches_ResearchId",
                        column: x => x.ResearchId,
                        principalTable: "Researches",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ResearchSigneds_ResearchId",
                table: "ResearchSigneds",
                column: "ResearchId");

            migrationBuilder.CreateIndex(
                name: "IX_ResearchSigneds_SignedUserId",
                table: "ResearchSigneds",
                column: "SignedUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResearchSigneds");
        }
    }
}
