using MDLibrary.Domain;
using MDLibrary.Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MDLibrary.Models
{
    public static class IdentitySeedData
    {
        public static async void AddAdminUser(IApplicationBuilder app, IConfiguration configuration)
        {
            // get dependecies
            MDLibraryIdentityDbContext context = app.ApplicationServices
                .CreateScope().ServiceProvider.GetRequiredService<MDLibraryIdentityDbContext>();
            UserManager<LibraryUser> userManager = app.ApplicationServices
                .CreateScope().ServiceProvider.GetRequiredService<UserManager<LibraryUser>>();
            RoleManager<IdentityRole> roleManager = app.ApplicationServices
                .CreateScope().ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var username = configuration["Data:AdminUser:Name"] ?? "admin";
            var email = configuration["Data:AdminUser:Email"] ?? "admin@example.com";
            var password = configuration["Data:AdminUser:Password"] ?? "Secret123*";
            var role = configuration["Data:AdminUser:Role"] ?? "admins";

            if (await userManager.FindByNameAsync(username) is null)
            {
                if (await roleManager.FindByNameAsync(role) is null)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }

                var user = new LibraryUser
                {
                    UserName = username,
                    Email = email,
                    EmailConfirmed = true,
                };

                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);
                }
            }
        }

        public static async void PopulateRoles(IApplicationBuilder app, params string[] roles)
        {
            RoleManager<IdentityRole> roleManager = app.ApplicationServices
                .CreateScope().ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            foreach (var role in roles)
            {
                if (await roleManager.FindByNameAsync(role) is null)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

        }
    }
}
