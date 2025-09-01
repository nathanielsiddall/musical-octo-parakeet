using Boilerplate.Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // ✅ Needed for ToListAsync

namespace Boilerplate.Api.UserPermissions;

public class GetPermissionsEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/users/{userId:guid}/permissions", async (
            Guid userId,
            AppDbContext db) =>
        {
            var permissions = await db.UserGroups
                .Where(ug => ug.UserId == userId)
                .SelectMany(ug => ug.Group.GroupPermissions.Select(gp => gp.Permission))
                .Select(p => new
                {
                    p.Id,
                    p.Resource,
                    p.Action,
                    Name = p.Name
                })
                .ToListAsync();

            return Results.Ok(permissions);
        });
    }
}