using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Boilerplate.Api.Schoolhouses;

public class CreateSchoolhouseEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/schoolhouses", async (
            ClaimsPrincipal claims,
            [FromBody] CreateSchoolhouseRequest body,
            AppDbContext db,
            UserManager<AppUser> userManager,
            RoleManager<Group> roleManager) =>
        {
            var owner = await userManager.GetUserAsync(claims);
            if (owner == null) return Results.Unauthorized();

            if (await db.Schoolhouses.AnyAsync(s => s.Subdomain == body.Subdomain))
                return Results.BadRequest("Subdomain already in use.");

            if (await db.Schoolhouses.AnyAsync(s => s.Name == body.Name))
                return Results.BadRequest("Name already in use.");

            var school = new Schoolhouse
            {
                Id = Guid.NewGuid(),
                OwnerId = owner.Id,
                Name = body.Name,
                Subdomain = body.Subdomain,
                Description = body.Description ?? string.Empty,
                Tagline = body.Tagline ?? string.Empty,
                Location = body.Location ?? string.Empty,
                LogoUrl = body.LogoUrl ?? string.Empty,
                BannerUrl = body.BannerUrl ?? string.Empty,
                ThemeColor = body.ThemeColor ?? string.Empty
            };

            db.Schoolhouses.Add(school);
            await db.SaveChangesAsync();

            await EnsureDefaultRolesAsync(roleManager);
            await userManager.AddToRoleAsync(owner, "Schoolmaster");

            // TODO: Generate welcome packet and schedule onboarding emails.

            return Results.Created($"/api/schoolhouses/{school.Id}", school);
        })
        .WithName("CreateSchoolhouse")
        .WithTags("Schoolhouses")
        .WithOpenApi()
        .RequireAuthorization();
    }

    private static async Task EnsureDefaultRolesAsync(RoleManager<Group> roleManager)
    {
        var roles = new[] { "Schoolmaster", "Dean", "Instructor", "Apprentice" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new Group
                {
                    Name = role,
                    NormalizedName = role.ToUpper()
                });
            }
        }
    }
}

public class CreateSchoolhouseRequest
{
    public string Name { get; set; } = string.Empty;
    public string Subdomain { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Tagline { get; set; }
    public string? Location { get; set; }
    public string? LogoUrl { get; set; }
    public string? BannerUrl { get; set; }
    public string? ThemeColor { get; set; }
}
