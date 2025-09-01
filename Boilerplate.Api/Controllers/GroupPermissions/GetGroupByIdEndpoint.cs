using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Boilerplate.Api.Auth;

public class GetGroupByIdEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/auth/groups/{id:guid}", async (Guid id, AppDbContext db) =>
        {
            var group = await db.Groups
                .Include(g => g.GroupPermissions)
                .ThenInclude(gp => gp.Permission)
                .Include(g => g.UserGroups)
                .ThenInclude(ug => ug.User)
                .FirstOrDefaultAsync(g => g.Id == id);

            return group is null ? Results.NotFound() : Results.Ok(group);
        });
    }
}