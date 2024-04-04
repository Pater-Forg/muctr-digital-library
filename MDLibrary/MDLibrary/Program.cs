using Microsoft.AspNetCore.Builder;
using MDLibrary.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MDLibrary.Domain;
using MDLibrary.Models;

var builder = WebApplication.CreateBuilder(args);

#region Connection

var identityConnectionString = builder.Configuration.GetConnectionString(
    "MDLibraryIdentityConnection")
        ?? throw new InvalidOperationException(
            "Connection string 'MDLibraryIdentityConnection' not found.");

var modelsConnectionString = builder.Configuration.GetConnectionString(
    "MDLibraryModelsConnection")
        ?? throw new InvalidOperationException(
            "Connection string 'MDLibraryModelsConnection' not found");
#endregion

#region Services

builder.Services.AddDbContext<MDLibraryIdentityDbContext>(options =>
    options.UseSqlServer(identityConnectionString));

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
    options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<MDLibraryIdentityDbContext>();

builder.Services.AddDbContext<MDLibraryDbContext>(options =>
    options.UseSqlServer(modelsConnectionString)
);

builder.Services.AddControllersWithViews();

#endregion

var app = builder.Build();

#region EnvironmentConfiguration

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

#endregion

#region Uses

app.UseRouting();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

#endregion

#region Routes

app.MapRazorPages();
//app.MapAreaControllerRoute(name: "admin", areaName: "Admin", pattern: "Admin/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller}/{action=Index}"
);
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/"
);

#endregion

SeedData.PopulateFromDataFile(app, builder.Configuration);

app.Run();
