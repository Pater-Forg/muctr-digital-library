using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MDLibrary.Migrations.MDLibraryBusinessDb
{
    /// <inheritdoc />
    public partial class CreateLiteraturePagesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "literature_pages",
                columns: table => new
                {
                    literature_page_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    literature_id = table.Column<int>(type: "integer", nullable: false),
                    page_number = table.Column<short>(type: "smallint", nullable: false),
                    text = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_literature_pages", x => x.literature_page_id);
                    table.ForeignKey(
                        name: "fk_literature_pages_literature_literature_id",
                        column: x => x.literature_id,
                        principalTable: "literature",
                        principalColumn: "literature_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_literature_pages_literature_id",
                table: "literature_pages",
                column: "literature_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "literature_pages");
        }
    }
}
