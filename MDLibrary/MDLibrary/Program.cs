using Microsoft.AspNetCore.Builder;
using MDLibrary.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("MDLibraryIdentityDbContextConnection") ?? throw new InvalidOperationException("Connection string 'MDLibraryIdentityDbContextConnection' not found.");

builder.Services.AddDbContext<MDLibraryIdentityDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<MDLibraryIdentityDbContext>();
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
