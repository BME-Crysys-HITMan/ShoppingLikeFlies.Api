using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShoppingLikeFiles.DomainServices;
using ShoppingLikeFlies.Api.Security.DAL;

namespace ShoppingLikeFlies.Api.Extensions;

public static class WebApplicationExtensions
{
    public static async Task<WebApplication> UseDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<SecurityDbContext>();

        try
        {
            await context.Database.MigrateAsync();
        }
        catch
        {
            await context.Database.EnsureCreatedAsync();
        }

        using var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        using var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        if (!roleManager.Roles.Any())
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
            await roleManager.CreateAsync(new IdentityRole("User"));
        }

        bool isDevelopment = app.Environment.IsDevelopment();
        string rootPw = app.Configuration.GetValue<string>("RootUserPassword");
        var user = new ApplicationUser { UserName = "root", Email = "root@root.com", LastName = "User", FirstName = "Root", EmailConfirmed = true };

        if (isDevelopment)
        {
            if (!userManager.Users.Any(u => u.UserName == "root"))
            {
                var result = await userManager.CreateAsync(user, rootPw);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }
        else
        {
            if (!userManager.Users.Any())
            {
                var result = await userManager.CreateAsync(user, rootPw);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }


        app.UseDomainServices(app.Environment.IsDevelopment());

        return app;
    }

    public static WebApplication UsePipeline(this WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers().RequireAuthorization("ValidToken");
        return app;
    }
}
