﻿// <auto-generated />
using System;
using MDLibrary.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MDLibrary.Migrations.MDLibraryBusinessDb
{
    [DbContext(typeof(MDLibraryBusinessDbContext))]
    [Migration("20240603172857_CreateBookmarksTable")]
    partial class CreateBookmarksTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AuthorLiterature", b =>
                {
                    b.Property<int>("AuthorsAuthorId")
                        .HasColumnType("integer")
                        .HasColumnName("authors_author_id");

                    b.Property<int>("LiteratureId")
                        .HasColumnType("integer")
                        .HasColumnName("literature_id");

                    b.HasKey("AuthorsAuthorId", "LiteratureId")
                        .HasName("pk_author_literature");

                    b.HasIndex("LiteratureId")
                        .HasDatabaseName("ix_author_literature_literature_id");

                    b.ToTable("author_literature");
                });

            modelBuilder.Entity("KeywordLiterature", b =>
                {
                    b.Property<int>("KeywordsKeywordId")
                        .HasColumnType("integer")
                        .HasColumnName("keywords_keyword_id");

                    b.Property<int>("LiteratureId")
                        .HasColumnType("integer")
                        .HasColumnName("literature_id");

                    b.HasKey("KeywordsKeywordId", "LiteratureId")
                        .HasName("pk_keyword_literature");

                    b.HasIndex("LiteratureId")
                        .HasDatabaseName("ix_keyword_literature_literature_id");

                    b.ToTable("keyword_literature");
                });

            modelBuilder.Entity("MDLibrary.Domain.Entities.Author", b =>
                {
                    b.Property<int>("AuthorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("author_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("AuthorId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(128)")
                        .HasColumnName("name");

                    b.HasKey("AuthorId")
                        .HasName("pk_authors");

                    b.ToTable("authors");
                });

            modelBuilder.Entity("MDLibrary.Domain.Entities.Bookmark", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<int?>("LiteraturePageId")
                        .HasColumnType("integer")
                        .HasColumnName("literature_page_id");

                    b.Property<string>("Title")
                        .HasColumnType("varchar(1024)")
                        .HasColumnName("title");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_bookmarks");

                    b.HasIndex("LiteraturePageId")
                        .HasDatabaseName("ix_bookmarks_literature_page_id");

                    b.ToTable("bookmarks");
                });

            modelBuilder.Entity("MDLibrary.Domain.Entities.Keyword", b =>
                {
                    b.Property<int>("KeywordId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("keyword_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("KeywordId"));

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("varchar(256)")
                        .HasColumnName("value");

                    b.HasKey("KeywordId")
                        .HasName("pk_keywords");

                    b.ToTable("keywords");
                });

            modelBuilder.Entity("MDLibrary.Domain.Entities.Literature", b =>
                {
                    b.Property<int>("LiteratureId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("literature_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("LiteratureId"));

                    b.Property<string>("Abstract")
                        .HasColumnType("text")
                        .HasColumnName("abstract");

                    b.Property<string>("Bbc")
                        .HasColumnType("varchar(128)")
                        .HasColumnName("bbc");

                    b.Property<string>("Caption")
                        .IsRequired()
                        .HasColumnType("varchar(1024)")
                        .HasColumnName("caption");

                    b.Property<string>("Isbn")
                        .HasColumnType("varchar(128)")
                        .HasColumnName("isbn");

                    b.Property<short?>("PageCount")
                        .HasColumnType("smallint")
                        .HasColumnName("page_count");

                    b.Property<string>("PublishLocation")
                        .HasColumnType("varchar(256)")
                        .HasColumnName("publish_location");

                    b.Property<short?>("PublishYear")
                        .HasColumnType("smallint")
                        .HasColumnName("publish_year");

                    b.Property<string>("Publisher")
                        .HasColumnType("varchar(256)")
                        .HasColumnName("publisher");

                    b.Property<string>("Udc")
                        .HasColumnType("varchar(128)")
                        .HasColumnName("udc");

                    b.HasKey("LiteratureId")
                        .HasName("pk_literature");

                    b.ToTable("literature");
                });

            modelBuilder.Entity("MDLibrary.Domain.Entities.LiteratureFile", b =>
                {
                    b.Property<int>("LiteratureFileId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("literature_file_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("LiteratureFileId"));

                    b.Property<string>("Extension")
                        .IsRequired()
                        .HasColumnType("varchar(10)")
                        .HasColumnName("extension");

                    b.Property<string>("Filename")
                        .IsRequired()
                        .HasColumnType("varchar(256)")
                        .HasColumnName("filename");

                    b.Property<int>("LiteratureId")
                        .HasColumnType("integer")
                        .HasColumnName("literature_id");

                    b.HasKey("LiteratureFileId")
                        .HasName("pk_literature_files");

                    b.HasIndex("LiteratureId")
                        .HasDatabaseName("ix_literature_files_literature_id");

                    b.ToTable("literature_files");
                });

            modelBuilder.Entity("MDLibrary.Domain.Entities.LiteraturePage", b =>
                {
                    b.Property<int>("LiteraturePageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("literature_page_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("LiteraturePageId"));

                    b.Property<int>("LiteratureId")
                        .HasColumnType("integer")
                        .HasColumnName("literature_id");

                    b.Property<short>("PageNumber")
                        .HasColumnType("smallint")
                        .HasColumnName("page_number");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("text");

                    b.HasKey("LiteraturePageId")
                        .HasName("pk_literature_pages");

                    b.HasIndex("LiteratureId")
                        .HasDatabaseName("ix_literature_pages_literature_id");

                    b.ToTable("literature_pages");
                });

            modelBuilder.Entity("AuthorLiterature", b =>
                {
                    b.HasOne("MDLibrary.Domain.Entities.Author", null)
                        .WithMany()
                        .HasForeignKey("AuthorsAuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_author_literature_authors_authors_author_id");

                    b.HasOne("MDLibrary.Domain.Entities.Literature", null)
                        .WithMany()
                        .HasForeignKey("LiteratureId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_author_literature_literature_literature_id");
                });

            modelBuilder.Entity("KeywordLiterature", b =>
                {
                    b.HasOne("MDLibrary.Domain.Entities.Keyword", null)
                        .WithMany()
                        .HasForeignKey("KeywordsKeywordId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_keyword_literature_keywords_keywords_keyword_id");

                    b.HasOne("MDLibrary.Domain.Entities.Literature", null)
                        .WithMany()
                        .HasForeignKey("LiteratureId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_keyword_literature_literature_literature_id");
                });

            modelBuilder.Entity("MDLibrary.Domain.Entities.Bookmark", b =>
                {
                    b.HasOne("MDLibrary.Domain.Entities.LiteraturePage", "LiteraturePage")
                        .WithMany()
                        .HasForeignKey("LiteraturePageId")
                        .HasConstraintName("fk_bookmarks_literature_pages_literature_page_id");

                    b.Navigation("LiteraturePage");
                });

            modelBuilder.Entity("MDLibrary.Domain.Entities.LiteratureFile", b =>
                {
                    b.HasOne("MDLibrary.Domain.Entities.Literature", "Literature")
                        .WithMany()
                        .HasForeignKey("LiteratureId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_literature_files_literature_literature_id");

                    b.Navigation("Literature");
                });

            modelBuilder.Entity("MDLibrary.Domain.Entities.LiteraturePage", b =>
                {
                    b.HasOne("MDLibrary.Domain.Entities.Literature", "Literature")
                        .WithMany()
                        .HasForeignKey("LiteratureId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_literature_pages_literature_literature_id");

                    b.Navigation("Literature");
                });
#pragma warning restore 612, 618
        }
    }
}
