using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MDLibrary.Migrations.MDLibraryDb
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    AuthorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(32)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.AuthorId);
                });

            migrationBuilder.CreateTable(
                name: "Keywords",
                columns: table => new
                {
                    KeywordId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "varchar(32)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Keywords", x => x.KeywordId);
                });

            migrationBuilder.CreateTable(
                name: "Literature",
                columns: table => new
                {
                    LiteratureId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PublishYear = table.Column<short>(type: "smallint", nullable: false),
                    PageCount = table.Column<short>(type: "smallint", nullable: false),
                    Caption = table.Column<string>(type: "varchar(256)", nullable: false),
                    PublishLocation = table.Column<string>(type: "varchar(64)", nullable: false),
                    Publisher = table.Column<string>(type: "varchar(64)", nullable: false),
                    Isbn = table.Column<string>(type: "varchar(32)", nullable: false),
                    Bbc = table.Column<string>(type: "varchar(32)", nullable: false),
                    Udc = table.Column<string>(type: "varchar(32)", nullable: false),
                    Abstract = table.Column<string>(type: "varchar(700)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Literature", x => x.LiteratureId);
                });

            migrationBuilder.CreateTable(
                name: "AuthorLiterature",
                columns: table => new
                {
                    AuthorsAuthorId = table.Column<int>(type: "int", nullable: false),
                    LiteratureId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorLiterature", x => new { x.AuthorsAuthorId, x.LiteratureId });
                    table.ForeignKey(
                        name: "FK_AuthorLiterature_Authors_AuthorsAuthorId",
                        column: x => x.AuthorsAuthorId,
                        principalTable: "Authors",
                        principalColumn: "AuthorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuthorLiterature_Literature_LiteratureId",
                        column: x => x.LiteratureId,
                        principalTable: "Literature",
                        principalColumn: "LiteratureId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    FileId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LiteratureId = table.Column<int>(type: "int", nullable: false),
                    Filename = table.Column<string>(type: "varchar(64)", nullable: false),
                    Extension = table.Column<string>(type: "varchar(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.FileId);
                    table.ForeignKey(
                        name: "FK_Files_Literature_LiteratureId",
                        column: x => x.LiteratureId,
                        principalTable: "Literature",
                        principalColumn: "LiteratureId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KeywordLiterature",
                columns: table => new
                {
                    KeywordsKeywordId = table.Column<int>(type: "int", nullable: false),
                    LiteratureId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KeywordLiterature", x => new { x.KeywordsKeywordId, x.LiteratureId });
                    table.ForeignKey(
                        name: "FK_KeywordLiterature_Keywords_KeywordsKeywordId",
                        column: x => x.KeywordsKeywordId,
                        principalTable: "Keywords",
                        principalColumn: "KeywordId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KeywordLiterature_Literature_LiteratureId",
                        column: x => x.LiteratureId,
                        principalTable: "Literature",
                        principalColumn: "LiteratureId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthorLiterature_LiteratureId",
                table: "AuthorLiterature",
                column: "LiteratureId");

            migrationBuilder.CreateIndex(
                name: "IX_Files_LiteratureId",
                table: "Files",
                column: "LiteratureId");

            migrationBuilder.CreateIndex(
                name: "IX_KeywordLiterature_LiteratureId",
                table: "KeywordLiterature",
                column: "LiteratureId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthorLiterature");

            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "KeywordLiterature");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.DropTable(
                name: "Keywords");

            migrationBuilder.DropTable(
                name: "Literature");
        }
    }
}
