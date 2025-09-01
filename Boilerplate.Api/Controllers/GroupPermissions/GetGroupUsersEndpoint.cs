using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Boilerplate.Api.Auth;

public class GetGroupUsersEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/auth/groups/{groupId:guid}/users", async (
            Guid groupId,
            AppDbContext db) =>
        {
            var group = await db.Groups
                .Include(g => g.UserGroups)
                .ThenInclude(ug => ug.User)
                .FirstOrDefaultAsync(g => g.Id == groupId);

            if (group == null) return Results.NotFound();

            return Results.Ok(group.UserGroups.Select(ug => ug.User.Email));
        });
    }
}