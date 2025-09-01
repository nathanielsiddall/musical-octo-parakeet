using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Boilerplate.Api.Auth;

public class UpdateGroupEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/auth/groups/{id:guid}", async (
            Guid id,
            [FromBody] GroupRequest body,
            AppDbContext db) =>
        {
            var group = await db.Groups.FindAsync(id);
            if (group == null) return Results.NotFound();

            group.Name = body.Name;
            await db.SaveChangesAsync();

            return Results.Ok(group);
        });
    }
}