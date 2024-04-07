using MDLibrary.Domain.Entities;
using MDLibrary.Helpers;
using Microsoft.EntityFrameworkCore;

namespace MDLibrary.Domain
{
    public class MDLibraryBusinessDbContext : DbContext
    {
        public MDLibraryBusinessDbContext(DbContextOptions<MDLibraryBusinessDbContext> options)
            : base(options)
        {
            
        }

        public DbSet<Literature> Literature { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Keyword> Keywords { get; set; }
        public DbSet<File> Files { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // For naming conventions purposes. EF uses CamelCase, and Pgsql uses snake_case
            modelBuilder.MigrateTablenamesToPostgres();
            // Literature configuration //

            modelBuilder.Entity<Literature>(
                e =>
                {
                    e.Property(p => p.Caption).HasColumnType("varchar(1024)");
                    e.Property(p => p.PublishLocation).HasColumnType("varchar(256)");
                    e.Property(p => p.Publisher).HasColumnType("varchar(256)");
                    e.Property(p => p.Isbn).HasColumnType("varchar(128)");
                    e.Property(p => p.Bbc).HasColumnType("varchar(128)");
                    e.Property(p => p.Udc).HasColumnType("varchar(128)");
                    e.Property(p => p.Abstract).HasColumnType("text");
                });

            // File configuration //

            modelBuilder.Entity<File>(
                e =>
                {
                    e.Property(p => p.Filename).HasColumnType("varchar(256)");
                    e.Property(p => p.Extension).HasColumnType("varchar(10)");
                });

            modelBuilder.Entity<File>().Ignore(e => e.Content);

            // Keyword configuration //

            modelBuilder.Entity<Keyword>(
                e =>
                {
                    e.Property(p => p.Value).HasColumnType("varchar(256)");
                });

            // Author configuration //

            modelBuilder.Entity<Author>(
                e =>
                {
                    e.Property(p => p.Name).HasColumnType("varchar(128)");
                });
        }
    }
}
