using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MDLibrary.Domain;
using MDLibrary.Models;
using MDLibrary.Domain.Entities;
using System.IO;
using Microsoft.AspNetCore.HttpOverrides;

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
    options.UseNpgsql(identityConnectionString));

builder.Services.AddDefaultIdentity<LibraryUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.User.RequireUniqueEmail = true;
}).AddRoles<IdentityRole>().AddEntityFrameworkStores<MDLibraryIdentityDbContext>();

builder.Services.AddDbContext<MDLibraryBusinessDbContext>(options =>
    options.UseNpgsql(modelsConnectionString)
);

builder.Services.ConfigureApplicationCookie(options =>
{
    //Location for your Custom Access Denied Page
    options.AccessDeniedPath = "/Identity/Auth/AccessDenied";

    //Location for your Custom Login Page
    options.LoginPath = "/Identity/Auth/Login";

    options.LogoutPath = "/Identity/Auth/Logout";
});

builder.Services.AddControllersWithViews();

#endregion

var app = builder.Build();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
	ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

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

//app.MapRazorPages();
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

IdentitySeedData.AddAdminUser(app, builder.Configuration);
SeedData.PopulateFromDataFile(app, builder.Configuration);

LiteratureFile.RootPath = builder.Configuration["RootPathToLiteratureFiles"]
    ?? throw new InvalidOperationException("RootPathToLiteratureFiles not found");

if (!Directory.Exists(LiteratureFile.RootPath))
	Directory.CreateDirectory(LiteratureFile.RootPath);

app.Run();
