using Microsoft.AspNetCore.Mvc;

namespace Boilerplate.Api.Auth;

public class DeleteGroupEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/auth/groups/{id:guid}", async (Guid id, AppDbContext db) =>
        {
            var group = await db.Groups.FindAsync(id);
            if (group == null) return Results.NotFound();

            db.Groups.Remove(group);
            await db.SaveChangesAsync();

            return Results.Ok($"Group {group.Name} deleted.");
        });
    }
}