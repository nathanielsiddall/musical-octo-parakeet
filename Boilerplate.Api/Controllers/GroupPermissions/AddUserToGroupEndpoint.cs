using Microsoft.AspNetCore.Mvc;

namespace Boilerplate.Api.Auth;

public class AddUserToGroupEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/groups/{groupId:guid}/users/{userId:guid}", async (
            Guid groupId,
            Guid userId,
            AppDbContext db) =>
        {
            var group = await db.Groups.FindAsync(groupId);
            var user = await db.Users.FindAsync(userId);

            if (group == null || user == null) return Results.NotFound();

            db.UserGroups.Add(new UserGroup { GroupId = groupId, UserId = userId });
            await db.SaveChangesAsync();

            return Results.Ok($"User {user.Email} added to group {group.Name}.");
        });
    }
}