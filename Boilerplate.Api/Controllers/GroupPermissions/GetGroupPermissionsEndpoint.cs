using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Boilerplate.Api.Auth;

public class GetGroupPermissionsEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/auth/groups/{groupId:guid}/permissions", async (
            Guid groupId,
            AppDbContext db) =>
        {
            var group = await db.Groups
                .Include(g => g.GroupPermissions)
                .ThenInclude(gp => gp.Permission)
                .FirstOrDefaultAsync(g => g.Id == groupId);

            if (group == null) return Results.NotFound();

            return Results.Ok(group.GroupPermissions.Select(gp => gp.Permission));
        });
    }
}