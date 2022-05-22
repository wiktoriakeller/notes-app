using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NotesApp.DataAccess.Migrations
{
    public partial class AddNoteHashId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HashId",
                table: "Notes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HashId",
                table: "Notes");
        }
    }
}
