using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MDLibrary.Migrations.MDLibraryBusinessDb
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "authors",
                columns: table => new
                {
                    author_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "varchar(128)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_authors", x => x.author_id);
                });

            migrationBuilder.CreateTable(
                name: "keywords",
                columns: table => new
                {
                    keyword_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    value = table.Column<string>(type: "varchar(256)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_keywords", x => x.keyword_id);
                });

            migrationBuilder.CreateTable(
                name: "literature",
                columns: table => new
                {
                    literature_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    publish_year = table.Column<short>(type: "smallint", nullable: true),
                    page_count = table.Column<short>(type: "smallint", nullable: true),
                    caption = table.Column<string>(type: "varchar(1024)", nullable: false),
                    publish_location = table.Column<string>(type: "varchar(256)", nullable: true),
                    publisher = table.Column<string>(type: "varchar(256)", nullable: true),
                    isbn = table.Column<string>(type: "varchar(128)", nullable: true),
                    bbc = table.Column<string>(type: "varchar(128)", nullable: true),
                    udc = table.Column<string>(type: "varchar(128)", nullable: true),
                    @abstract = table.Column<string>(name: "abstract", type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_literature", x => x.literature_id);
                });

            migrationBuilder.CreateTable(
                name: "author_literature",
                columns: table => new
                {
                    authors_author_id = table.Column<int>(type: "integer", nullable: false),
                    literature_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_author_literature", x => new { x.authors_author_id, x.literature_id });
                    table.ForeignKey(
                        name: "fk_author_literature_authors_authors_author_id",
                        column: x => x.authors_author_id,
                        principalTable: "authors",
                        principalColumn: "author_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_author_literature_literature_literature_id",
                        column: x => x.literature_id,
                        principalTable: "literature",
                        principalColumn: "literature_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "files",
                columns: table => new
                {
                    file_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    literature_id = table.Column<int>(type: "integer", nullable: false),
                    filename = table.Column<string>(type: "varchar(256)", nullable: false),
                    extension = table.Column<string>(type: "varchar(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_files", x => x.file_id);
                    table.ForeignKey(
                        name: "fk_files_literature_literature_id",
                        column: x => x.literature_id,
                        principalTable: "literature",
                        principalColumn: "literature_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "keyword_literature",
                columns: table => new
                {
                    keywords_keyword_id = table.Column<int>(type: "integer", nullable: false),
                    literature_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_keyword_literature", x => new { x.keywords_keyword_id, x.literature_id });
                    table.ForeignKey(
                        name: "fk_keyword_literature_keywords_keywords_keyword_id",
                        column: x => x.keywords_keyword_id,
                        principalTable: "keywords",
                        principalColumn: "keyword_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_keyword_literature_literature_literature_id",
                        column: x => x.literature_id,
                        principalTable: "literature",
                        principalColumn: "literature_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_author_literature_literature_id",
                table: "author_literature",
                column: "literature_id");

            migrationBuilder.CreateIndex(
                name: "ix_files_literature_id",
                table: "files",
                column: "literature_id");

            migrationBuilder.CreateIndex(
                name: "ix_keyword_literature_literature_id",
                table: "keyword_literature",
                column: "literature_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "author_literature");

            migrationBuilder.DropTable(
                name: "files");

            migrationBuilder.DropTable(
                name: "keyword_literature");

            migrationBuilder.DropTable(
                name: "authors");

            migrationBuilder.DropTable(
                name: "keywords");

            migrationBuilder.DropTable(
                name: "literature");
        }
    }
}
