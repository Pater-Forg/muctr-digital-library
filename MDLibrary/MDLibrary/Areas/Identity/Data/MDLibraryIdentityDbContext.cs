using MDLibrary.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MDLibrary.Areas.Identity.Data;

public class MDLibraryIdentityDbContext : IdentityDbContext<IdentityUser>
{
    public MDLibraryIdentityDbContext(DbContextOptions<MDLibraryIdentityDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

		// For naming conventions purposes. EF uses CamelCase, and Pgsql uses snake_case
		builder.MigrateTablenamesToPostgres();
    }
}
