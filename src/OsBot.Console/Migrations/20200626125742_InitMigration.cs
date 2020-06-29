using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OsBot.Console.Migrations
{
    public partial class InitMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RemindNotes",
                columns: table => new
                {
                    RemindNoteId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ChannelId = table.Column<ulong>(type: "INTEGER", nullable: false),
                    AuthorId = table.Column<ulong>(type: "INTEGER", nullable: false),
                    MessageId = table.Column<ulong>(type: "INTEGER", nullable: false),
                    When = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RemindNotes", x => x.RemindNoteId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RemindNotes");
        }
    }
}
