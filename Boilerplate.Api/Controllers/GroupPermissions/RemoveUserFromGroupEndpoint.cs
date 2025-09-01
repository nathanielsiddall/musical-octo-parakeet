using Microsoft.AspNetCore.Mvc;

namespace Boilerplate.Api.Auth;

public class RemoveUserFromGroupEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/auth/groups/{groupId:guid}/users/{userId:guid}", async (
            Guid groupId,
            Guid userId,
            AppDbContext db) =>
        {
            var ug = await db.UserGroups.FindAsync(userId, groupId);
            if (ug == null) return Results.NotFound();

            db.UserGroups.Remove(ug);
            await db.SaveChangesAsync();

            return Results.Ok("User removed from group.");
        });
    }
}