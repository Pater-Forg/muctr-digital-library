using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MDLibrary.Migrations.MDLibraryBusinessDb
{
    /// <inheritdoc />
    public partial class CreateBookmarksTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "bookmarks",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<string>(type: "text", nullable: false),
                    literature_page_id = table.Column<int>(type: "integer", nullable: true),
                    title = table.Column<string>(type: "varchar(1024)", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_bookmarks", x => x.id);
                    table.ForeignKey(
                        name: "fk_bookmarks_literature_pages_literature_page_id",
                        column: x => x.literature_page_id,
                        principalTable: "literature_pages",
                        principalColumn: "literature_page_id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_bookmarks_literature_page_id",
                table: "bookmarks",
                column: "literature_page_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bookmarks");
        }
    }
}
