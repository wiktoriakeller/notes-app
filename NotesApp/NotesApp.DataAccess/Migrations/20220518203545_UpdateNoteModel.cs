using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NotesApp.DataAccess.Migrations
{
    public partial class UpdateNoteModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HashIdSalt",
                table: "Notes",
                newName: "PublicHashIdSalt");

            migrationBuilder.RenameColumn(
                name: "HashId",
                table: "Notes",
                newName: "PublicHashId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PublicHashIdSalt",
                table: "Notes",
                newName: "HashIdSalt");

            migrationBuilder.RenameColumn(
                name: "PublicHashId",
                table: "Notes",
                newName: "HashId");
        }
    }
}
