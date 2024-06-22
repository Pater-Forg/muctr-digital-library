using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MDLibrary.Migrations.MDLibraryBusinessDb
{
    /// <inheritdoc />
    public partial class DropFilesAndCreateLiteratureFiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_files_literature_literature_id",
                table: "files");

            migrationBuilder.DropPrimaryKey(
                name: "pk_files",
                table: "files");

            migrationBuilder.RenameTable(
                name: "files",
                newName: "literature_files");

            migrationBuilder.RenameColumn(
                name: "file_id",
                table: "literature_files",
                newName: "literature_file_id");

            migrationBuilder.RenameIndex(
                name: "ix_files_literature_id",
                table: "literature_files",
                newName: "ix_literature_files_literature_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_literature_files",
                table: "literature_files",
                column: "literature_file_id");

            migrationBuilder.AddForeignKey(
                name: "fk_literature_files_literature_literature_id",
                table: "literature_files",
                column: "literature_id",
                principalTable: "literature",
                principalColumn: "literature_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_literature_files_literature_literature_id",
                table: "literature_files");

            migrationBuilder.DropPrimaryKey(
                name: "pk_literature_files",
                table: "literature_files");

            migrationBuilder.RenameTable(
                name: "literature_files",
                newName: "files");

            migrationBuilder.RenameColumn(
                name: "literature_file_id",
                table: "files",
                newName: "file_id");

            migrationBuilder.RenameIndex(
                name: "ix_literature_files_literature_id",
                table: "files",
                newName: "ix_files_literature_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_files",
                table: "files",
                column: "file_id");

            migrationBuilder.AddForeignKey(
                name: "fk_files_literature_literature_id",
                table: "files",
                column: "literature_id",
                principalTable: "literature",
                principalColumn: "literature_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
