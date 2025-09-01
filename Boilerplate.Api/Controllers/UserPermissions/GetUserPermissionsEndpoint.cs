using Boilerplate.Api.UserPermissions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Boilerplate.Api.UserPermissions;

public class GetUserPermissionsEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/users/{userId:guid}/permissions", async (
            Guid userId,
            UserManager<AppUser> userManager,
            AppDbContext db) =>
        {
            var user = await userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return Results.NotFound("User not found");

            // ✅ Ensure EF Core async extensions are used
            var permissions = await db.UserGroups
                .Where(ug => ug.UserId == userId)
                .SelectMany(ug => ug.Group.GroupPermissions.Select(gp => gp.Permission))
                .Distinct()
                .ToListAsync();

            return Results.Ok(permissions);
        });
    }
}