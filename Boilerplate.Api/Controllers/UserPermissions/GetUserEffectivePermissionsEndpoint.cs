using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Boilerplate.Api.Auth;

public class GetUserEffectivePermissionsEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/auth/users/{userId:guid}/effective-permissions", async (
            Guid userId,
            AppDbContext db) =>
        {
            var user = await db.Users
                .Include(u => u.UserGroups)
                .ThenInclude(ug => ug.Group)
                .ThenInclude(g => g.GroupPermissions)
                .ThenInclude(gp => gp.Permission)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null) return Results.NotFound("User not found.");

            var effectivePermissions = user.UserGroups
                .SelectMany(ug => ug.Group.GroupPermissions.Select(gp => gp.Permission))
                .Distinct()
                .Select(p => new { p.Id, p.Name, p.Resource, p.Action })
                .ToList();

            return Results.Ok(effectivePermissions);
        });
    }
}