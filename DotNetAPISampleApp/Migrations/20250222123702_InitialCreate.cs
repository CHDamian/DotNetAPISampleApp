using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotNetAPISampleApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Researches",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    subject = table.Column<string>(type: "TEXT", nullable: false),
                    summary = table.Column<string>(type: "TEXT", nullable: false),
                    finished = table.Column<bool>(type: "INTEGER", nullable: false),
                    resultsSummary = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Researches", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Researches");
        }
    }
}
